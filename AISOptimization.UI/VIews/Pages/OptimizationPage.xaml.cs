﻿using Wpf.Ui.Controls.Navigation;


namespace AISOptimization.VIews.Pages;

public partial class OptimizationPage : INavigableView<OptimizationPageVM>
{
    public OptimizationPage(OptimizationPageVM vm)
    {
        ViewModel = vm;
        DataContext = this;
        InitializeComponent();
    }

    public OptimizationPageVM ViewModel { get; }
}




