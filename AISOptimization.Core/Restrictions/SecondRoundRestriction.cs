using org.matheval;

using WPF.Base;


namespace AISOptimization.Core;

public class SecondRoundRestriction : BaseVM
{
    public Expression Expression { get; set; }
    
    public bool IsSatisfied(Point point)
    {
        var variables = Expression.getVariables();
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
