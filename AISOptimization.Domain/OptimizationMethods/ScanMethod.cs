using AISOptimization.Domain.Constraints;
using AISOptimization.Domain.Parameters;


namespace AISOptimization.Domain.OptimizationMethods;

public class ScanMethod : IOptimizationMethod
{
    private class DecisionVariableExtended
    {
        public DecisionVariableExtended(DecisionVariable decisionVariable, double min, double max)
        {
            DecisionVariable = decisionVariable;
            Min = min;
            Max = max;
            Step = 0;
        }
        public DecisionVariable DecisionVariable;
        public double Min;
        public double Max;
        public double Step;
    }
    private readonly double _eps;
    private readonly OptimizationProblem _optimizationProblem;
    private int _countOfSegments = 100;
    private List<DecisionVariableExtended> Vars = new ();
    public ScanMethod(OptimizationProblem optimizationProblem, double eps)
    {
        _optimizationProblem = optimizationProblem;
        _eps = eps;

        
    }

    public Point SolveProblem()
    {
        foreach (var decisionVariable in _optimizationProblem.DecisionVariables)
        {
            decisionVariable.Value = decisionVariable.FirstRoundConstraint.Min;
            Vars.Add(new DecisionVariableExtended(decisionVariable, decisionVariable.FirstRoundConstraint.Min, decisionVariable.FirstRoundConstraint.Max));
        }
        // while (expression)
        // {
        //     throw new NotImplementedException();
        // }
        return null;
    }

    private Point findBestPointOnGrid(int countOfSegmentsPerValue)
    {
        foreach (var decisionVariableExtended in Vars)
        {
            decisionVariableExtended.Step = (decisionVariableExtended.Max - decisionVariableExtended.Min) / countOfSegmentsPerValue;
        }

        foreach (var decisionVariableExtended in Vars)
        {
            
        }
        
        //для каждой переменной
        for (int i = 0; i < Vars.Count; i++)
        {
            for (int j = 0; j < countOfSegmentsPerValue; j++)
            {
                
            }
        }

        return null;
    }
}



