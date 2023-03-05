namespace AISOptimization.Core.OptimizationMethods;

public class ScanMethod : IOptimizationMethod
{
    private readonly OptimizationProblem _optimizationProblem;
    private readonly double _eps;
    private int _countOfSegments = 100;
    public ScanMethod(OptimizationProblem optimizationProblem, double eps)
    {
        _optimizationProblem = optimizationProblem;
        _eps = eps;
    }
    public Point GetBestXPoint()
    {
        throw new NotImplementedException();
    }

    public Point FindBestPointWithSecondRoundRestriction()
    {
        throw new NotImplementedException();
    }
}
