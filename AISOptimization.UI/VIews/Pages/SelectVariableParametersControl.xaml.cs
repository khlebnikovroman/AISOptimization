using System.Windows.Controls;

using AISOptimization.Utils;


namespace AISOptimization.VIews.Pages;

public partial class SelectVariableParametersControl : IViewWithVM<SelectVariableParametersControlVM>
{
    public SelectVariableParametersControlVM ViewModel { get; set; }

    public SelectVariableParametersControl(SelectVariableParametersControlVM viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }

    public object ViewModelObject
    {
        get => ViewModel;
    }
}

