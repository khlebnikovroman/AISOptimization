using System.Windows.Controls;

using Wpf.Ui.Controls;


namespace AISOptimization.Utils;

public interface IDialogAware
{
    public double Width { get; set; }
    public double Height { get; set; }
    public string Title { get; set; }
    public event HideRequstedHandler HideRequested;
}


public delegate void HideRequstedHandler(object sender, HideRequstedArgs args);


public class HideRequstedArgs
{
}

