using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using AISOptimization.VIews.Pages;

using Autofac;

using Wpf.Ui.Contracts;
using Wpf.Ui.Services;


namespace AISOptimization
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IContainer _container;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var builder = new ContainerBuilder();
            builder.RegisterType<TaskBarService>().As<ITaskBarService>().SingleInstance();
            builder.RegisterType<ThemeService>().As<IThemeService>().SingleInstance();
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            builder.RegisterType<PageService>().As<IPageService>().SingleInstance();
            
            builder.RegisterType<MainWindow>().As<INavigationWindow>().SingleInstance();
            builder.RegisterType<MainWindowVM>().AsSelf();
            builder.RegisterType<Example>().AsSelf();
            builder.RegisterType<Example2>().AsSelf();
            builder.RegisterType<Example1VM>().AsSelf();
            builder.RegisterType<Example2VM>().AsSelf();
            
            builder.Register(c => new AutofacAdapter()).As<IServiceProvider>().SingleInstance();
            
            _container = builder.Build();
            var sp = _container.Resolve<IServiceProvider>() as AutofacAdapter;
            sp.Container = _container;
            var navigationWindow = _container.Resolve<INavigationWindow>();
            navigationWindow.ShowWindow();
            //navigationWindow.Navigate(typeof(Example));
            var nav = _container.Resolve<INavigationService>();
            nav.Navigate(typeof(Example));
        }

        public static T GetService<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
