using AISOptimization.Core.Collections;

using WPF.Base;


namespace AISOptimization.Core;

//todo delete
public class Point: BaseVM
{
    public Point()
    {
        X = new FullyObservableCollection<IndependentVariable>();
    }

    public FullyObservableCollection<IndependentVariable> X { get; set; }

    
}
