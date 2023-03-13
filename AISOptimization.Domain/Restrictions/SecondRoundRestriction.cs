using AISOptimization.Core.Common;
using AISOptimization.Core.Parameters;


namespace AISOptimization.Core.Restrictions;

public class SecondRoundRestriction : Entity
{
    public FuncExpression Expression { get; set; }

    public bool IsSatisfied(Point point)
    {
        var variables = Expression.GetVariables();

        foreach (var xVariable in point.X)
        {
            if (variables.Contains(xVariable.Key))
            {
                Expression.Bind(xVariable.Key, xVariable.Value);
            }
        }

        return Expression.Eval<bool>();
    }
}

