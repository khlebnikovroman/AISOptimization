using System.Collections.ObjectModel;
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
    private readonly FuncExpression _objectiveFunction;

    public OptimizationProblem()
    {
        StaticVariables.CollectionChanged += (sender, args) =>
        {
            foreach (var secondRoundRestriction in SecondRoundRestrictions)
            {
                foreach (StaticVariable staticVariable in args.NewItems)
                {
                    if (secondRoundRestriction.Expression.GetVariables().Contains(staticVariable.Key))
                    {
                        secondRoundRestriction.Expression.Bind(staticVariable.Key, staticVariable.Value);
                    }
                }
            }
        };
    }

    public OptimizationProblem(string objectiveFunction, IEnumerable<IVariable> independentVariables, IEnumerable<IVariable> staticVariables) : this()
    {
        ObjectiveFunction = new FuncExpression(objectiveFunction);
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

    public List<SecondRoundRestriction> SecondRoundRestrictions { get; init; } = new ();
    public List<IndependentVariable> VectorX { get; init; }=new ();
    public ObservableCollection<StaticVariable> StaticVariables { get; init; }=new ();
    


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
        foreach (var xVariable in point.X)
        {
            ObjectiveFunction.Bind(xVariable.Key, xVariable.Value);
        }
        foreach (var xVariable in StaticVariables)
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


