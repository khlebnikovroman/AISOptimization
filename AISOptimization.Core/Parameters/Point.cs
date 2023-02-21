using AISOptimization.Core.Collections;

using WPF.Base;


namespace AISOptimization.Core;

public class Point: BaseVM
{
    public Point()
    {
        X.ItemPropertyChanged += (sender, args) =>
        {
            var changedVariable = X[args.CollectionIndex];
            _function.Expression.Bind(changedVariable.Key, changedVariable.Value);
        };
    }
    private IObjectiveFunction _function;
    public FullyObservableCollection<IndependentVariable> X { get; set; }
    public double F
    {
        get => _function.GetValue();
    }
    
}
