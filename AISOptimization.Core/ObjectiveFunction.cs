using org.matheval;


namespace AISOptimization.Core;

public class ObjectiveFunction : IObjectiveFunction
{
    public Expression Expression { get; set; }
    public double GetValue()
    {
        throw new NotImplementedException();
    }
}
