using System.Collections.ObjectModel;
using System.ComponentModel;

using AISOptimization.Domain.Common;
using AISOptimization.Domain.Constraints;
using AISOptimization.Domain.Parameters;

using org.matheval;


namespace AISOptimization.Domain;

public enum Extremum
{
    [Description("Минимизировать функцию")]
    Min,

    [Description("Максимизировать функцию")]
    Max,
}


public class OptimizationProblem : Entity
{
    private readonly FuncExpression _objectiveFunction;

    public OptimizationProblem()
    {
        Constants.CollectionChanged += (sender, args) =>
        {
            foreach (var secondRoundConstraint in SecondRoundConstraints)
            {
                foreach (Constant staticVariable in args.NewItems)
                {
                    if (secondRoundConstraint.ConstraintFunction.GetVariables().Contains(staticVariable.Key))
                    {
                        secondRoundConstraint.ConstraintFunction.Bind(staticVariable.Key, staticVariable.Value);
                    }
                }
            }
        };
    }

    public OptimizationProblem(string objectiveFunction, IEnumerable<IVariable> decisionVariables, IEnumerable<IVariable> constants) : this()
    {
        ObjectiveFunction = new FuncExpression(objectiveFunction);

        foreach (var decisionVariable in decisionVariables)
        {
            DecisionVariables.Add(new DecisionVariable
                                      {FirstRoundConstraint = new FirstRoundConstraint(), Key = decisionVariable.Key,});
        }

        foreach (var staticVariable in constants)
        {
            Constants.Add(new Constant
                              {Key = staticVariable.Key,});
        }
    }

    public Extremum Extremum { get; set; }

    public string ProblemText { get; set; }
    public string ObjectiveParameter { get; set; }
    public string ObjectiveFunctionDescription { get; set; }

    public FuncExpression ObjectiveFunction
    {
        get => _objectiveFunction;
        init
        {
            _objectiveFunction = value;
            _objectiveFunction.DisableFunction(SpecialFunctions.ComparisonFunctions);
        }
    }

    public List<SecondRoundConstraint> SecondRoundConstraints { get; init; } = new();
    public List<DecisionVariable> DecisionVariables { get; init; } = new();
    public ObservableCollection<Constant> Constants { get; init; } = new();


    public static IEnumerable<string> GetVariables(string expression)
    {
        var e = new Expression(expression);

        return e.getVariables();
    }

    public IEnumerable<string> GetVariables()
    {
        return ObjectiveFunction.GetVariables();
    }

    public static bool IsValidExpression(string expression)
    {
        var exp = new Expression(expression);
        var errors = exp.GetError();

        return errors.Count == 0;
    }

    public double GetValueInPoint(Point point)
    {
        foreach (var xVariable in point.DecisionVariables)
        {
            ObjectiveFunction.Bind(xVariable.Key, xVariable.Value);
        }

        foreach (var xVariable in Constants)
        {
            ObjectiveFunction.Bind(xVariable.Key, xVariable.Value);
        }

        switch (Extremum)
        {
            case Extremum.Min:
                return -ObjectiveFunction.Eval<double>();
            case Extremum.Max:
                return ObjectiveFunction.Eval<double>();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public bool IsSecondRoundConstraintsSatisfied(Point point)
    {
        return SecondRoundConstraints.All(r => r.IsSatisfied(point));
    }

    public Point CreatePoint()
    {
        var point = new Point();

        foreach (var decisionVariable in DecisionVariables)
        {
            point.DecisionVariables.Add((DecisionVariable) decisionVariable.Clone());
        }

        return point;
    }
}




