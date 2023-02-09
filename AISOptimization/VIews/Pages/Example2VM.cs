using Wpf.Ui.Common;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls.Navigation;


namespace AISOptimization.VIews.Pages;

public class Example2VM 
{

    private RelayCommand _toExample1;

    public RelayCommand ToExample1
    {
        get
        {
            return _toExample1 ??= new RelayCommand(o =>
            {
                App.GetService<INavigationService>().Navigate(typeof(Example));
            });
        }
    }

}
