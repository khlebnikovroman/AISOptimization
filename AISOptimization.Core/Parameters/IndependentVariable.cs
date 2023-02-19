using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace AISOptimization.Core;

public class IndependentVariable:IVariable, INotifyPropertyChanged
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
    public FirstRoundRestriction FirstRoundRestriction { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}
