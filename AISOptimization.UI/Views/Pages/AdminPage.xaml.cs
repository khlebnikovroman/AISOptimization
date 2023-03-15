using Wpf.Ui.Controls.Navigation;


namespace AISOptimization.Views.Pages;

public partial class AdminPage : INavigableView<AdminPageVM>
{
    public AdminPage(AdminPageVM vm)
    {
        ViewModel = vm;
        DataContext = this;
        InitializeComponent();
    }

    public AdminPageVM ViewModel { get; }
}


