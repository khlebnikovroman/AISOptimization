using System.Windows.Controls;

using Wpf.Ui.Controls;


namespace AISOptimization.Utils;

public interface IDialogAware : IViewWithVM
{
    public object Footer { get; }
}

