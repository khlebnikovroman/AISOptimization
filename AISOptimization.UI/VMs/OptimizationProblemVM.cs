using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using AISOptimization.Domain;

using FluentValidation;

using WPF.Base;


namespace AISOptimization.VMs;

public class OptimizationProblemVM : BaseVM, INotifyDataErrorInfo
{
    public Extremum Extremum { get; set; }

    public string ProblemText { get; set; }
    public string ObjectiveParameter { get; set; }
    public string ObjectiveFunctionDescription { get; set; }
    public FunctionExpressionVM ObjectiveFunction { get; set; }
    public ObservableCollection<SecondRoundConstraintVM> SecondRoundConstraints { get; set; } = new();
    public ObservableCollection<DecisionVariableVM> DecisionVariables { get; set; } = new();
    public ObservableCollection<Constant> Constants { get; set; } = new();
    public IValidator<OptimizationProblemVM> Validator { get; set; }

    public IEnumerable GetErrors(string? propertyName)
    {
        return null;
    }

    public bool HasErrors => DecisionVariables.Any(x => x.FirstRoundConstraint.HasErrors);
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
}


