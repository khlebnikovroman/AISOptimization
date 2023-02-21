using WPF.Base;


namespace AISOptimization.Core;

public class StaticVariable: BaseVM, IVariable
{
    public string Key { get; set; }
    public double Value { get; set; }
}
