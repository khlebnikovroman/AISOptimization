using Wpf.Ui.Contracts;
using Wpf.Ui.Controls.Navigation;


namespace AISOptimization.VIews.Pages;

public class Example1VM
{
    private RelayCommand _toExample2;

    public RelayCommand ToExample2
    {
        get
        {
            return _toExample2 ??= new RelayCommand(o =>
            {
                App.GetService<INavigationService>().Navigate(typeof(Example2));
            });
        }
    }

}
