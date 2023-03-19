using System;
using System.Linq;

using AISOptimization.Domain;
using AISOptimization.Services;
using AISOptimization.VMs;

using Mapster;

using Microsoft.EntityFrameworkCore;

using WPF.Base;

using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;


namespace AISOptimization.Views.Pages;

/// <summary>
/// Vm для <see cref="LoginPage"/>
/// </summary>
public class LoginPageVM: BaseVM
{
    private readonly INavigationService _navigationService;
    private readonly IMessageBoxService _messageBoxService;
    private readonly UserService _userService;

    public LoginPageVM(INavigationService navigationService, IMessageBoxService messageBoxService, UserService userService)
    {
        _navigationService = navigationService;
        _messageBoxService = messageBoxService;
        _userService = userService;
    }

    public UserVM User { get; set; } = new UserVM();
    private RelayCommand _authorizeCommand;

    public RelayCommand AuthorizeCommand
    {
        get
        {
            return _authorizeCommand ??= new RelayCommand(async o =>
            {


                var user = await _userService.GetByName(User.UserName);

                void showError()
                    {
                        _messageBoxService.Show("Неверное имя пользователя или пароль", "Ошибка"); 
                    }
                    if (user is null)
                    {
                        showError();
                        return;
                    }
                    if (User.Password!=user.Password)
                    {
                        showError();
                        return;
                    }

                    User = user.Adapt<UserVM>();
                    if (user.Role.RoleType=="Admin")
                    {
                        _navigationService.Navigate(typeof(AdminPage));
                        _userService.User = User;
                    }
                    if (user.Role.RoleType=="User")
                    {
                        _navigationService.Navigate(typeof(OptimizationPage));
                        _userService.User = User;
                    
                    }
                
                
            }, _ => !String.IsNullOrEmpty(User.UserName) && !String.IsNullOrEmpty(User.Password));
        }
    }

}
