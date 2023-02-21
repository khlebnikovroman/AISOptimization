using System.Windows.Controls;

using AISOptimization.Utils;


namespace AISOptimization.VIews.Pages;

public partial class SelectVariableParametersControl : IViewWithVM<SelectVariableParametersControlVM>, IDialogAware
{
    public SelectVariableParametersControlVM ViewModel { get; set; }

    public SelectVariableParametersControl(SelectVariableParametersControlVM viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }

    public object Footer => Resources["Footer"];
}

