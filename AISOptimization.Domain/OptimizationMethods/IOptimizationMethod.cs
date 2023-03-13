using AISOptimization.Core.Parameters;


namespace AISOptimization.Core.OptimizationMethods;

public interface IOptimizationMethod
{
    public Point SolveProblem();
}

