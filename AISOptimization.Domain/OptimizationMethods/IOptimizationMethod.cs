using AISOptimization.Domain.Parameters;


namespace AISOptimization.Domain.OptimizationMethods;

/// <summary>
/// Общий интерфейс для методов оптимизации
/// </summary>
public interface IOptimizationMethod
{
    public Point SolveProblem();
}



