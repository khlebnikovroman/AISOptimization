using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using AISOptimization.Services;
using AISOptimization.Utils;
using AISOptimization.VIews.Pages;

using Autofac;

using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;
using Wpf.Ui.Services;

using FrameworkElementFactory = AISOptimization.Services.FrameworkElementFactory;


namespace AISOptimization
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IContainer _container;
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var builder = new ContainerBuilder();
            builder.RegisterType<TaskBarService>().As<ITaskBarService>().SingleInstance();
            builder.RegisterType<ThemeService>().As<IThemeService>().SingleInstance();
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            builder.RegisterType<PageService>().As<IPageService>().SingleInstance();
            builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            builder.RegisterType<FrameworkElementFactory>().As<IFrameworkElementFactory>().SingleInstance();
            builder.RegisterType<MyDialogService>().AsSelf().SingleInstance();
            builder.RegisterType<MainWindow>().As<INavigationWindow>().SingleInstance();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainWindowVM>().AsSelf();
            builder.RegisterType<OptimizationPage>().AsSelf();;
            builder.RegisterType<OptimizationPageVM>().AsSelf();;
            builder.RegisterType<SelectVariableParametersControl>().AsSelf();
            builder.RegisterType<SelectVariableParametersControlVM>().AsSelf();
            
            builder.Register(c => new AutofacAdapter()).As<IServiceProvider>().SingleInstance();
            
            _container = builder.Build();
            var sp = _container.Resolve<IServiceProvider>() as AutofacAdapter;
            sp.Container = _container;
            var navigationWindow = _container.Resolve<INavigationWindow>();
            navigationWindow.ShowWindow();
            //navigationWindow.Navigate(typeof(Example));
            var nav = _container.Resolve<INavigationService>();
            nav.Navigate(typeof(OptimizationPage));
            var ds = _container.Resolve<MyDialogService>();
            var ee = _container.Resolve<SelectVariableParametersControl>();
            var s = await ds.ShowDialog<SelectVariableParametersControl>();
            Debug.WriteLine($"Конец диалога! {s}");
            
        }

        public static T GetService<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
