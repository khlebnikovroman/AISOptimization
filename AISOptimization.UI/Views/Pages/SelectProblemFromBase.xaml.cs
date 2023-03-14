using System.Windows.Controls;

using AISOptimization.Utils;
using AISOptimization.Utils.Dialog;


namespace AISOptimization.Views.Pages;

public partial class SelectProblemFromBase : IViewWithVM<SelectProblemFromBaseVM>,IDialogAware
{
    public SelectProblemFromBaseVM ViewModel { get; set; }
    public SelectProblemFromBase(SelectProblemFromBaseVM vm, ProblemList problemList)
    {
        ViewModel = vm;
        ViewModel.ProblemListVm = problemList.ViewModel;
        DataContext = this;
        InitializeComponent();
        ProblemListContentControl.Content = problemList;
    }

    public object Footer
    {
        get
        {
            var footer = Resources["Footer"] as StackPanel;

            footer.DataContext ??= this;

            return footer;
        }
    }
}

