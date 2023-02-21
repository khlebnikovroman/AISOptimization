using System.Collections.ObjectModel;
using System.Linq.Expressions;

using org.matheval;

using WPF.Base;

using Expression = org.matheval.Expression;


namespace AISOptimization.Core;

public class OptimizationProblem : BaseVM
{
    public OptimizationProblem(string objectiveFunction)
    {
        Function = new ObjectiveFunction() {Expression = new Expression(objectiveFunction)};
    }
    public IObjectiveFunction Function { get; set; }
    //public List<FirstRoundRestriction> FirstRoundRestrictions { get; set; }
    public ObservableCollection<SecondRoundRestriction> SecondRoundRestrictions { get; set; }
    public ObservableCollection<IndependentVariable> IndependentVariables { get; set; }
    public ObservableCollection<StaticVariable> StaticVariables { get; set; }
    
}
