using System;
using System.Windows;

using AISOptimization.Domain;
using AISOptimization.Services;
using AISOptimization.Views;
using AISOptimization.Views.Pages;
using AISOptimization.Views.Validators;
using AISOptimization.VMs;

using Autofac;

using FluentValidation;

using Mapster;

using Wpf.Ui.Contracts;
using Wpf.Ui.Services;

using FrameworkElementFactory = AISOptimization.Services.FrameworkElementFactory;


namespace AISOptimization;

/// <summary>
///     Interaction logic for App.xaml
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
        builder.RegisterType<UserService>().AsSelf().SingleInstance();
        builder.RegisterType<FrameworkElementFactory>().As<IFrameworkElementFactory>().SingleInstance();
        builder.RegisterType<MyDialogService>().AsSelf().SingleInstance();
        builder.RegisterType<MainWindow>().As<INavigationWindow>().SingleInstance();
        builder.RegisterType<MessageBoxService>().As<IMessageBoxService>();
        builder.RegisterType<SnackbarService>().As<ISnackbarService>().SingleInstance();
        builder.RegisterType<AisOptimizationContext>().AsSelf();
        
        builder.RegisterType<MainWindow>().AsSelf();
        builder.RegisterType<MainWindowVM>().AsSelf();
        builder.RegisterType<OptimizationPage>().AsSelf();
        ;
        builder.RegisterType<OptimizationPageVM>().AsSelf();
        ;
        builder.RegisterType<SelectVariableParametersControl>().AsSelf();
        builder.RegisterType<SelectVariableParametersControlVM>().AsSelf();
        builder.RegisterType<ChartDirectorSurface>().AsSelf();
        builder.RegisterType<ChartDirectorCharts>().AsSelf();
        builder.RegisterType<ChartDirectorSurfaceProjection>().AsSelf();
        builder.RegisterType<ChartDirectorChartVM>().AsSelf();
        builder.RegisterType<ProblemEditControl>().AsSelf();
        builder.RegisterType<ProblemEditControlVM>().AsSelf();
        builder.RegisterType<LoginPage>().AsSelf();
        builder.RegisterType<LoginPageVM>().AsSelf();
        builder.RegisterType<SelectProblemFromBase>().AsSelf();
        builder.RegisterType<SelectProblemFromBaseVM>().AsSelf();
        builder.RegisterType<ProblemList>().AsSelf();
        builder.RegisterType<ProblemListVM>().AsSelf();
        

        builder.Register(c => new AutofacAdapter()).As<IServiceProvider>().SingleInstance();

        builder.RegisterType<OptimizationPageVMValidator>().As<AbstractValidator<OptimizationPageVM>>();
        builder.RegisterType<OptimizationPageVMValidator>().AsSelf();

        //todo в отдельную функцию
        TypeAdapterConfig<OptimizationProblem, OptimizationProblemVM>
            .NewConfig()
            .TwoWays()
            .Map(x => x.ObjectiveFunction, x => x.ObjectiveFunction)
            .Map(x => x.DecisionVariables, x => x.DecisionVariables);

        TypeAdapterConfig<FunctionExpressionVM, FuncExpression>
            .NewConfig()
            .ConstructUsing(src => new FuncExpression(src.Formula));


        _container = builder.Build();
        var sp = _container.Resolve<IServiceProvider>() as AutofacAdapter;
        sp.Container = _container;
        var navigationWindow = _container.Resolve<INavigationWindow>();
        navigationWindow.ShowWindow();

        //navigationWindow.Navigate(typeof(Example));
        var nav = _container.Resolve<INavigationService>();
        nav.Navigate(typeof(LoginPage));
    }
}

