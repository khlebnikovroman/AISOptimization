using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using AISOptimization.Views.Pages;

using Wpf.Ui.Common;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Controls.Window;


namespace AISOptimization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INavigationWindow
    {
        public MainWindowVM ViewModel { get; set; }
        
        public MainWindow(MainWindowVM vm,IPageService pageService,INavigationService navigationService ,IDialogService dialogService)
        {
            ViewModel = vm;
            DataContext = this;
            InitializeComponent();
            SetPageService(pageService);
            dialogService.SetDialogControl(RootDialog);
            navigationService.SetNavigationControl(RootNavigation);
            RootNavigation.MenuItems=  new ObservableCollection<object>
            {
                new NavigationViewItem()
                {
                    TargetPageType = typeof(OptimizationPage)
                },
            };
        }


        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);
        

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);
        

        public void ShowWindow() => Show();

        public void CloseWindow() => Close();
    }
}
