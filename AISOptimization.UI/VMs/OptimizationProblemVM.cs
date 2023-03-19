using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AISOptimization.Domain;

using FluentValidation;

using WPF.Base;


namespace AISOptimization.VMs;

/// <summary>
/// VM для <see cref="OptimizationProblem"/>
/// </summary>
public class OptimizationProblemVM : BaseVM, INotifyDataErrorInfo
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public Extremum Extremum { get; set; }

    public string ProblemText { get; set; }
    public string ObjectiveParameter { get; set; }
    public string ObjectiveFunctionDescription { get; set; }
    public FunctionExpressionVM ObjectiveFunction { get; set; }
    public ObservableCollection<SecondRoundConstraintVM> SecondRoundConstraints { get; set; } = new();
    public ObservableCollection<DecisionVariableVM> DecisionVariables { get; set; } = new();
    public ObservableCollection<ConstantVM> Constants { get; set; } = new();
    public IValidator<OptimizationProblemVM> Validator { get; set; }

    public IEnumerable GetErrors(string? propertyName)
    {
        return null;
    }

    public bool HasErrors => DecisionVariables.Any(x => x.FirstRoundConstraint.HasErrors);
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public override string ToString()
    {
        var sb = new StringBuilder();

        if (!String.IsNullOrEmpty(ObjectiveFunctionDescription))
        {
            sb.AppendLine(ObjectiveFunctionDescription);
        }

        if (!String.IsNullOrEmpty(ObjectiveFunction.Formula))
        {
            var x = String.IsNullOrEmpty(ObjectiveParameter) ? ObjectiveParameter : "Целевая функция";
            sb.AppendLine($"{x} = {ObjectiveFunction.Formula}");
        }
        if (DecisionVariables is not null)
        {
            foreach (var variable in DecisionVariables)
            {
                sb.AppendLine(variable.ToString());
            }
        }

        return sb.ToString();
    }
}


