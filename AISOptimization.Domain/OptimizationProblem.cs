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
    public long UserId { get; set; }
    private readonly FuncExpression _objectiveFunction;

    public OptimizationProblem()
    {
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

    public string? ProblemText { get; set; }
    public string ObjectiveParameter { get; set; }
    public string? ObjectiveFunctionDescription { get; set; }

    public virtual FuncExpression ObjectiveFunction
    {
        get => _objectiveFunction;
        init
        {
            _objectiveFunction = value;
            _objectiveFunction.DisableFunction(SpecialFunctions.ComparisonFunctions);
        }
    }

    public virtual List<SecondRoundConstraint> SecondRoundConstraints { get; init; } = new();
    public virtual List<DecisionVariable> DecisionVariables { get; init; } = new();
    public virtual List<Constant> Constants { get; init; }


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
        foreach (var constant in Constants.Where(constant => ObjectiveFunction.GetVariables().Contains(constant.Key)))
        {
            ObjectiveFunction.Bind(constant.Key,constant.Value);
        }
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
        foreach (var secondRoundConstraint in SecondRoundConstraints)
        {
            foreach (var staticVariable in Constants.Where(staticVariable => secondRoundConstraint.ConstraintFunction.GetVariables().Contains(staticVariable.Key)))
            {
                secondRoundConstraint.ConstraintFunction.Bind(staticVariable.Key, staticVariable.Value);
            }
        }
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




