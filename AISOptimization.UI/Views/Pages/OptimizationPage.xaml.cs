using Wpf.Ui.Controls.Navigation;


namespace AISOptimization.Views.Pages;

public partial class OptimizationPage : INavigableView<OptimizationPageVM> 
{
    public OptimizationPage(OptimizationPageVM vm, ProblemEditControl problemEditControl)
    {
        ViewModel = vm;
        DataContext = this;
        ViewModel.ProblemEditControlVm = problemEditControl.ViewModel;
        InitializeComponent();
        ProblemEditContentControl.Content = problemEditControl;
        
    }

    public OptimizationPageVM ViewModel { get; }
}






