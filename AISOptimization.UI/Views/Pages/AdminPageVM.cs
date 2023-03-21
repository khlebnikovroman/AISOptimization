using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using AISOptimization.Domain;
using AISOptimization.Services;
using AISOptimization.VMs;

using Mapster;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using WPF.Base;

using Wpf.Ui.Contracts;

using EntityState = Microsoft.EntityFrameworkCore.EntityState;


namespace AISOptimization.Views.Pages;

/// <summary>
/// VM для <see cref="AdminPage"/>
/// </summary>
public class AdminPageVM: BaseVM
{
    private readonly MyDialogService _dialogService;
    private  OptimizationProblemService _optimizationProblemService;
    private UserService _userService;
    private readonly IMessageBoxService _messageBoxService;
    private readonly ISnackbarService _snackbarService;


    public ObservableCollection<OptimizationProblemVM> OptimizationProblems { get; set; }
    public OptimizationProblemVM SelectedOptimizationProblem { get; set; }
    public ObservableCollection<UserVM> Users { get; set; }
    public UserVM SelectedUser { get; set; }
    public AdminPageVM(MyDialogService dialogService, AisOptimizationContext context, IMessageBoxService messageBoxService, ISnackbarService snackbarService, OptimizationProblemService optimizationProblemService, UserService userService)
    {
        _dialogService = dialogService;

        _messageBoxService = messageBoxService;
        _snackbarService = snackbarService;
        _optimizationProblemService = optimizationProblemService;
        _userService = userService;
        OptimizationProblems = _optimizationProblemService.GetAll().Result.Adapt<ObservableCollection<OptimizationProblemVM>>();
        Users = _userService.GetAll().Result.Adapt<ObservableCollection<UserVM>>();
    }

    private RelayCommand _deleteUser;

    public RelayCommand DeleteUser
    {
        get
        {
            return _deleteUser ??= new RelayCommand(async o =>
            {
                var mbRes = await _messageBoxService.Show("Вы действительно хотите удалить выбранного пользователя?", "Предупреждение",
                                                    MessageBoxButton.YesNo);
                if (mbRes == MessageBoxResult.Yes)
                {
                    await _userService.Delete(SelectedUser.Id);
                }
            },_=>SelectedUser is not null);
        }
    }

    private RelayCommand _addUser;

    public RelayCommand AddUser
    {
        get
        {
            return _addUser ??= new RelayCommand(async o =>
            {
                var newUser = await _dialogService.ShowDialog<UserEditControl>(new UserVM()) as UserVM;

                if (newUser is null)
                {
                    return;
                }

                await _userService.Create(newUser);
                _snackbarService.Timeout = 3000;
                _snackbarService.Show("База данных обновлена", "Пользователь успешно добавлен");
                Users.Add(newUser);
            });
        }
    }

    private RelayCommand _editUser;

    public RelayCommand EditUser
    {
        get
        {
            return _editUser ??= new RelayCommand(async _ =>
            {
                var newUser = await _dialogService.ShowDialog<UserEditControl>(SelectedUser) as UserVM;

                if (newUser is null)
                {
                    return;
                }

                await _userService.Update(newUser);
                _snackbarService.Timeout = 3000;
                _snackbarService.Show("База данных обновлена", "Пользователь успешно отредактирован");

            },_=>SelectedUser is not null);
        }
    }

    private RelayCommand _deleteProblem;

    public RelayCommand DeleteProblem
    {
        get
        {
            return _deleteProblem ??= new RelayCommand(async o =>
            {
                var mbRes = await _messageBoxService.Show("Вы действительно хотите удалить выбранную задачу?", "Предупреждение",
                                                    MessageBoxButton.YesNo);
                if (mbRes == MessageBoxResult.Yes)
                {
                    await _optimizationProblemService.Delete(SelectedOptimizationProblem.Id);
                }
            },_=>SelectedOptimizationProblem is not null);
        }
    }

    private RelayCommand _editProblem;

    public RelayCommand EditProblem
    {
        get
        {
            return _editProblem ??= new RelayCommand(async _ =>
            {
                var newProblem = await _dialogService.ShowDialog<ProblemEditControl>(SelectedOptimizationProblem) as OptimizationProblemVM;

                if (newProblem is null)
                {
                    return;
                }

                await _optimizationProblemService.Update(newProblem);
                _snackbarService.Timeout = 3000;
                _snackbarService.Show("База данных обновлена", "Задача успешно отредактирована");
            },_=>SelectedOptimizationProblem is not null);
        }
    }

    private RelayCommand _addProblem;
    private ProblemListVM _problemListVm;

    public RelayCommand AddProblem
    {
        get
        {
            return _addProblem ??= new RelayCommand(async _ =>
            {
                var newProblem = await _dialogService.ShowDialog<ProblemEditControl>(new OptimizationProblemVM()) as OptimizationProblemVM;

                if (newProblem is null)
                {
                    return;
                }

                await _optimizationProblemService.Create(newProblem);
                _snackbarService.Timeout = 3000;
                _snackbarService.Show("База данных обновлена", "Пользователь успешно добавлен");
                OptimizationProblems.Add(newProblem);
            });
        }
    }



}
