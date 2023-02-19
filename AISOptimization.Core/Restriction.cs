using org.matheval;


namespace AISOptimization.Core;

public class Restriction 
{
    private Expression _expression;
    public bool IsSatisfied()
    {
        return _expression.Eval<bool>();
    }
}
