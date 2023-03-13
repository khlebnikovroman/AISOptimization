using System.Collections.Specialized;
using System.ComponentModel;

using AISOptimization.Core.Common;
using AISOptimization.Core.Parameters;
using AISOptimization.Core.Restrictions;

using org.matheval;


namespace AISOptimization.Core;

public enum Extremum
{
    [Description("Минимизировать функцию")]
    Min,

    [Description("Максимизировать функцию")]
    Max,
}


public class OptimizationProblem : Entity
{
    private readonly FuncExpression _function;

    public OptimizationProblem()
    {
    }

    public OptimizationProblem(string objectiveFunction, IEnumerable<IVariable> independentVariables, IEnumerable<IVariable> staticVariables)
    {
        Function = new FuncExpression(objectiveFunction);
        foreach (var independentVariable in independentVariables)
        {
            VectorX.Add(new IndependentVariable
                            {FirstRoundRestriction = new FirstRoundRestriction(), Key = independentVariable.Key,});
        }
        
        foreach (var staticVariable in staticVariables)
        {
            StaticVariables.Add(new StaticVariable
                                    {Key = staticVariable.Key,});
        }
    }

    public Extremum Extremum { get; set; }

    public FuncExpression Function
    {
        get => _function;
        init
        {
            _function = value;
            _function.DisableFunction(SpecialFunctions.ComparisonFunctions);
        }
    }

    public List<SecondRoundRestriction> SecondRoundRestrictions { get; init; } = new ();
    public List<IndependentVariable> VectorX { get; init; }=new ();
    public List<StaticVariable> StaticVariables { get; init; }=new ();
    


    public static IEnumerable<string> GetVariables(string expression)
    {
        var e = new Expression(expression);

        return e.getVariables();
    }

    public IEnumerable<string> GetVariables()
    {
        return Function.GetVariables();
    }

    public static bool IsValidExpression(string expression)
    {
        var exp = new Expression(expression);
        var errors = exp.GetError();

        return errors.Count == 0;
    }

    public double GetValueInPoint(Point point)
    {
        foreach (var xVariable in point.X)
        {
            Function.Bind(xVariable.Key, xVariable.Value);
        }

        switch (Extremum)
        {
            case Extremum.Min:
                return -Function.Eval<double>();
            case Extremum.Max:
                return Function.Eval<double>();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public bool IsSecondRoundRestrictionsSatisfied(Point point)
    {
        return SecondRoundRestrictions.All(r => r.IsSatisfied(point));
    }

    public Point CreatePoint()
    {
        var point = new Point();

        foreach (var independentVariable in VectorX)
        {
            point.X.Add((IndependentVariable) independentVariable.Clone());
        }

        return point;
    }
}


