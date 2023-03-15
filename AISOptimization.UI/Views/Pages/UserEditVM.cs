using System;
using System.Collections.ObjectModel;

using AISOptimization.Domain;
using AISOptimization.Utils.Dialog;
using AISOptimization.VMs;

using Mapster;

using WPF.Base;


namespace AISOptimization.Views.Pages;

public class UserEditVM:BaseVM, IDataHolder,IResultHolder, IInteractionAware
{
    public UserEditVM(AisOptimizationContext context)
    {
        UserRoles = context.UsersRoles.Adapt<ObservableCollection<UserRoleVM>>();
    }

    public UserVM User { get; set; }
    public ObservableCollection<UserRoleVM> UserRoles { get; set; }
    public object Data
    {
        get => User;
        set => User = (UserVM) value;
    }
    public object Result { get; private set; }
    public Action FinishInteraction { get; set; }

    private RelayCommand _cancelCommand;

    public RelayCommand CancelCommand
    {
        get
        {
            return _cancelCommand ??= new RelayCommand(o =>
            {
                FinishInteraction();
            });
        }
    }

    private RelayCommand _applyCommand;

    public RelayCommand ApplyCommand
    {
        get
        {
            return _applyCommand ??= new RelayCommand(o =>
            {
                Result = User;
                FinishInteraction();
            },_=>
            {
                if (User is null)
                {
                    return false;
                }
                return !string.IsNullOrEmpty(User.UserName) && !string.IsNullOrEmpty(User.Password);
            });
        }
    }


}
