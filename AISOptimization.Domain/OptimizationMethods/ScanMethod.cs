using AISOptimization.Domain.Parameters;


namespace AISOptimization.Domain.OptimizationMethods;

public class ScanMethod : IOptimizationMethod
{
    private readonly double _eps;
    private readonly OptimizationProblem _optimizationProblem;
    private int _countOfSegments = 100;

    public ScanMethod(OptimizationProblem optimizationProblem, double eps)
    {
        _optimizationProblem = optimizationProblem;
        _eps = eps;
    }

    public Point SolveProblem()
    {
        throw new NotImplementedException();
    }

    public Point FindBestPointWithSecondRoundRestriction()
    {
        throw new NotImplementedException();
    }
}



