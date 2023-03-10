using System.Windows.Controls;

using AISOptimization.Utils;
using AISOptimization.Utils.Dialog;


namespace AISOptimization.Views.Pages;

public partial class ProblemEditControl : IViewWithVM<ProblemEditControlVM>, IDialogAware
{
    public ProblemEditControl(ProblemEditControlVM vm)
    {
        ViewModel = vm;
        DataContext = this;
        InitializeComponent();
    }

    public ProblemEditControlVM ViewModel { get; set; }
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

