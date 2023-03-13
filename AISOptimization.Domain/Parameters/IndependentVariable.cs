using System.ComponentModel.DataAnnotations.Schema;

using AISOptimization.Core.Common;
using AISOptimization.Core.Restrictions;


namespace AISOptimization.Core.Parameters;

public class IndependentVariable : Entity, IVariable, ICloneable
{
    public string Description { get; set; }
    public FirstRoundRestriction FirstRoundRestriction { get; set; }

    public object Clone()
    {
        return new IndependentVariable
            {Key = Key, Value = Value,Description = Description, FirstRoundRestriction = (FirstRoundRestriction) FirstRoundRestriction.Clone(),};
    }

    public string Key { get; set; }

    [NotMapped]
    public double Value { get; set; }
}

