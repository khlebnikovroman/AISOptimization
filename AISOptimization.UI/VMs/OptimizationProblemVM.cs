using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using AISOptimization.Core;

using FluentValidation;

using WPF.Base;


namespace  AISOptimization.UI.VM.VMs;

public class OptimizationProblemVM : BaseVM, INotifyDataErrorInfo
{
    public Extremum Extremum { get; set; }

    public string ObjectiveParameter { get; set; }
    public string ObjectiveFunctionDescription { get; set; }
    public FunctionExpressionVM ObjectiveFunction { get; set; }
    public ObservableCollection<SecondRoundRestrictionVM> SecondRoundRestrictions { get; set; } = new();
    public ObservableCollection<IndependentVariableVM> IndependentVariables { get; set; }= new();
    public ObservableCollection<StaticVariableVM> StaticVariables { get; set; }= new();
    public IValidator<OptimizationProblemVM> Validator { get; set; }
    public IEnumerable GetErrors(string? propertyName)
    {
        return null;
    }

    public bool HasErrors => IndependentVariables.Any(x => x.FirstRoundRestriction.HasErrors);
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
}
