using AISOptimization.Domain.Common;
using AISOptimization.Domain.Parameters;


namespace AISOptimization.Domain.Constraints;

public class SecondRoundConstraint : Entity
{
    public virtual FuncExpression ConstraintFunction { get; set; }

    public bool IsSatisfied(Point point)
    {
        var variables = ConstraintFunction.GetVariables();

        foreach (var xVariable in point.DecisionVariables)
        {
            if (variables.Contains(xVariable.Key))
            {
                ConstraintFunction.Bind(xVariable.Key, xVariable.Value);
            }
        }

        return ConstraintFunction.Eval<bool>();
    }
}



