using org.matheval;


namespace AISOptimization.Core;

public interface IObjectiveFunction
{
    public Expression Expression { get; set; }
    public double GetValue();
}
