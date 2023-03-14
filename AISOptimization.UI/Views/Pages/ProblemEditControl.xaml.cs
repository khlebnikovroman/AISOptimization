using System.Windows.Controls;

using AISOptimization.Utils;


namespace AISOptimization.Views.Pages;

public partial class ProblemEditControl : IViewWithVM<ProblemEditControlVM>
{
    public ProblemEditControl(ProblemEditControlVM vm)
    {
        ViewModel = vm;
        DataContext = this;
        InitializeComponent();
    }

    public ProblemEditControlVM ViewModel { get; set; }
}

