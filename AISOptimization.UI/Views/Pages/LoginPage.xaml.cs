using System.Windows;
using System.Windows.Controls;

using Wpf.Ui.Controls.Navigation;


namespace AISOptimization.Views.Pages;

/// <summary>
/// Страница входа
/// </summary>
public partial class LoginPage : INavigableView<LoginPageVM>
{
    public LoginPage(LoginPageVM vm)
    {
        ViewModel = vm;
        DataContext = this;
        InitializeComponent();
    }

    public LoginPageVM ViewModel { get; }

    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        ViewModel.User.Password = PasswordBox.Password;
    }
}

