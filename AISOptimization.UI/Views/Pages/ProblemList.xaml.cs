using System.Windows.Controls;

using AISOptimization.Utils;


namespace AISOptimization.Views.Pages;

public partial class ProblemList : IViewWithVM<ProblemListVM>
{
    public ProblemList(ProblemListVM vm)
    {
        ViewModel = vm;
        DataContext = this;
        InitializeComponent();
    }

    public ProblemListVM ViewModel { get; set; }
}

