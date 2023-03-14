using System;
using System.Collections.ObjectModel;

using AISOptimization.Views.Pages;

using Wpf.Ui.Contracts;
using Wpf.Ui.Controls.Navigation;


namespace AISOptimization.Views;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : INavigationWindow
{
    public MainWindow(MainWindowVM vm, IPageService pageService, INavigationService navigationService, IDialogService dialogService)
    {
        ViewModel = vm;
        DataContext = this;
        InitializeComponent();
        SetPageService(pageService);
        dialogService.SetDialogControl(RootDialog);
        navigationService.SetNavigationControl(RootNavigation);

        RootNavigation.MenuItems = new ObservableCollection<object>
        {
            new NavigationViewItem
            {
                TargetPageType = typeof(OptimizationPage),
            },
        };
    }

    public MainWindowVM ViewModel { get; set; }


    public INavigationView GetNavigation()
    {
        return RootNavigation;
    }

    public bool Navigate(Type pageType)
    {
        return RootNavigation.Navigate(pageType);
    }


    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
    }

    public void SetPageService(IPageService pageService)
    {
        RootNavigation.SetPageService(pageService);
    }


    public void ShowWindow()
    {
        Show();
    }

    public void CloseWindow()
    {
        Close();
    }
}

