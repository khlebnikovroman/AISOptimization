using System.Windows.Controls;

using Wpf.Ui.Controls.Navigation;


namespace AISOptimization.VIews.Pages;

public partial class Example2 : INavigableView<Example2VM>
{
    public Example2(Example2VM example2Vm)
    {
        ViewModel = example2Vm;
        DataContext = this;
        InitializeComponent();
    }

    public Example2VM ViewModel { get; }
}

