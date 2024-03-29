﻿using System.Windows.Controls;

using AISOptimization.Utils;
using AISOptimization.Utils.Dialog;


namespace AISOptimization.Views.Pages;

public partial class SelectVariableParametersControl : IViewWithVM<SelectVariableParametersControlVM>, IDialogAware
{
    public SelectVariableParametersControl(SelectVariableParametersControlVM viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
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

    public SelectVariableParametersControlVM ViewModel { get; set; }
}




