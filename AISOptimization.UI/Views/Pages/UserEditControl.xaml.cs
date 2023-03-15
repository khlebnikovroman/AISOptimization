using System.Windows.Controls;

using AISOptimization.Utils;
using AISOptimization.Utils.Dialog;


namespace AISOptimization.Views.Pages;

public partial class UserEditControl : IViewWithVM<UserEditVM>, IDialogAware
{
    public UserEditControl(UserEditVM vm)
    {
        ViewModel = vm;
        DataContext = this;
        InitializeComponent();
    }

    public UserEditVM ViewModel { get; set; }
    public object Footer
    {
        get
        {
            var footer = Resources["Footer"] as StackPanel;

            footer.DataContext ??= this;

            return footer;
        }
    }
}

