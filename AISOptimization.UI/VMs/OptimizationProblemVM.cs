using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using AISOptimization.Core;

using WPF.Base;


namespace  AISOptimization.UI.VM.VMs;

public class OptimizationProblemVM : BaseVM, INotifyDataErrorInfo
{
    public Extremum Extremum { get; set; }
    public FunctionExpressionVM FunctionExpression { get; set; }
    public ObservableCollection<SecondRoundRestrictionVM> SecondRoundRestrictions { get; set; } = new();
    public ObservableCollection<IndependentVariableVM> IndependentVariables { get; set; }= new();
    public ObservableCollection<StaticVariableVM> StaticVariables { get; set; }= new();
    public IEnumerable GetErrors(string? propertyName)
    {
        return null;
    }

    public bool HasErrors => IndependentVariables.Any(x => x.FirstRoundRestriction.HasErrors);
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
}
