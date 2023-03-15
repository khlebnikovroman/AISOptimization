using System.ComponentModel.DataAnnotations.Schema;

using AISOptimization.Domain.Common;
using AISOptimization.Domain.Constraints;


namespace AISOptimization.Domain.Parameters;

/// <summary>
/// Независимая переменная
/// </summary>
public class DecisionVariable : Entity, IVariable, ICloneable
{
    public string? Description { get; set; }
    public virtual FirstRoundConstraint FirstRoundConstraint { get; set; }

    public object Clone()
    {
        return new DecisionVariable
            {Key = Key, Value = Value, Description = Description, FirstRoundConstraint = (FirstRoundConstraint) FirstRoundConstraint.Clone(),};
    }

    public string Key { get; set; }

    [NotMapped]
    public double Value { get; set; }
}



