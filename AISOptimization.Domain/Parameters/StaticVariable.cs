using AISOptimization.Core.Common;


namespace AISOptimization.Core.Parameters;

public class StaticVariable : Entity, IVariable
{
    public string Key { get; set; }

    public double Value { get; set; }
}

