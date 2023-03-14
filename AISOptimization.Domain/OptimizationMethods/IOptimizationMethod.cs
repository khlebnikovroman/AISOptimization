using AISOptimization.Domain.Parameters;


namespace AISOptimization.Domain.OptimizationMethods;

public interface IOptimizationMethod
{
    public Point SolveProblem();
}



