using System.Windows.Controls;

using Wpf.Ui.Controls.Navigation;


namespace AISOptimization.VIews.Pages;

public partial class Example : INavigableView<Example1VM>
{
    public Example(Example1VM example1Vm)
    {
        ViewModel = example1Vm;
        DataContext = this;
        InitializeComponent();
    }

    public Example1VM ViewModel { get; }
}

