using WPF.Base;


namespace AISOptimization.Core;

public class StaticVariable: BaseVM, IVariable
{
    public string Key { get; set; }
    private double _value;

    public double Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            OnPropertyChanged();
        }
    }
}
