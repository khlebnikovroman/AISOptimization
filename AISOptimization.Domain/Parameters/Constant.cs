using AISOptimization.Domain.Common;


namespace AISOptimization.Domain.Parameters;

/// <summary>
/// Константа
/// </summary>
public class Constant : Entity, IVariable
{
    public string? Description { get; set; }
    public string Key { get; set; }

    public double Value { get; set; }
}



