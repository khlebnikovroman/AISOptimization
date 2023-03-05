using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.InteropServices.JavaScript;

using AISOptimization.Core.Collections;
using AISOptimization.Core.OptimizationMethods;

using org.matheval;

using WPF.Base;


namespace AISOptimization.Core;

public enum Extremum
{
    [Description("Минимизировать функцию")]
    Min,

    [Description("Максимизировать функцию")]
    Max,
}


public class OptimizationProblem : BaseVM
{
    public OptimizationProblem(string objectiveFunction, IEnumerable<IVariable> independentVariables, IEnumerable<IVariable> staticVariables)
    {
        Function = new Expression(objectiveFunction);

        VectorX = new FullyObservableCollection<IndependentVariable>();

        foreach (var independentVariable in independentVariables)
        {
            VectorX.Add(new IndependentVariable
                            {FirstRoundRestriction = new FirstRoundRestriction(), Key = independentVariable.Key,});
        }

        StaticVariables = new FullyObservableCollection<StaticVariable>();

        foreach (var staticVariable in staticVariables)
        {
            StaticVariables.Add(new StaticVariable
                                    {Key = staticVariable.Key,});
        }

        SecondRoundRestrictions = new FullyObservableCollection<SecondRoundRestriction>();
        VectorX.ItemPropertyChanged += VectorXOnItemPropertyChanged;
        SecondRoundRestrictions.CollectionChanged+= SecondRoundRestrictionsOnCollectionChanged;
        StaticVariables.ItemPropertyChanged+= StaticVariablesOnItemPropertyChanged;

        OptimizationMethod = new ComplexBoxMethod(this, 0.01);

    }

    private void StaticVariablesOnItemPropertyChanged(object? sender, ItemPropertyChangedEventArgs e)
    {
        var staticVariable = StaticVariables[e.CollectionIndex];
        Function.Bind(staticVariable.Key, staticVariable.Value);
    }

    private void SecondRoundRestrictionsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        foreach (SecondRoundRestriction secondRoundRestriction in e.NewItems)
        {
            foreach (var key in secondRoundRestriction.Expression.getVariables())
            {
                 
                IVariable variable = StaticVariables.FirstOrDefault(v => v.Key == key);

                if (variable is null)
                {
                    variable = VectorX.FirstOrDefault(x => x.Key == key);
                }

                if (variable is null)
                {
                    throw new ArgumentNullException();
                }

                secondRoundRestriction.Expression.Bind(key, variable.Value);
            }
        }
    }

    public IOptimizationMethod OptimizationMethod { get; set; }
    public Extremum Extremum { get; set; }
    public string TESTPROPERTY { get; set; }

    private Expression Function { get; }
    
    public FullyObservableCollection<SecondRoundRestriction> SecondRoundRestrictions { get; init; }
    public FullyObservableCollection<IndependentVariable> VectorX { get; init; }
    public FullyObservableCollection<StaticVariable> StaticVariables { get; init; }

    public static IEnumerable<string> GetVariables(string expression)
    {
        var e = new Expression(expression);

        return e.getVariables();
    }
    
    private void VectorXOnItemPropertyChanged(object? sender, ItemPropertyChangedEventArgs e)
    {
        // var item = VectorX[e.CollectionIndex];
        // Function.Bind(item.Key, item.Value);
        // var secondRoundRestrictions = SecondRoundRestrictions.Where(r => r.Expression.getVariables().Contains(item.Key));
        //
        // foreach (var secondRoundRestriction in secondRoundRestrictions)
        // {
        //     secondRoundRestriction.Expression.Bind(item.Key, item.Value);
        // }
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

