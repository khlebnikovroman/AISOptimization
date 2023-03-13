using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Documents;

using AISOptimization.Core;
using AISOptimization.Core.OptimizationMethods;
using AISOptimization.Core.Restrictions;
using AISOptimization.Services;
using AISOptimization.UI.VM.VMs;

using FluentValidation;

using Mapster;

using org.matheval;

using WPF.Base;

using Wpf.Ui.Controls;


namespace AISOptimization.Views.Pages;

public class OptimizationPageVM : BaseVM, INotifyDataErrorInfo
{
    private readonly MyDialogService _dialogService;
    private readonly AbstractValidator<OptimizationPageVM> _validator;

    public OptimizationPageVM(MyDialogService dialogService,AbstractValidator<OptimizationPageVM> validator)
    {
        _dialogService = dialogService;
        _validator = validator;
    }

    public List<string> VariablesKeys { get; private set; }
    public string SecondRoundRestrictionInput { get; set; }
    public string ObjectiveParameter { get; set; } = "z";

    public string ObjectiveFunctionInput { get; set; } = "x^2 + y^2";

    public OptimizationProblemVM OptimizationProblemVM { get; set; }
    
    private RelayCommand _inputObjectiveFunction;

    public RelayCommand InputObjectiveFunction
    {
        get
        {
            return _inputObjectiveFunction ??= new RelayCommand(async o =>
            {
                var res = await _dialogService.ShowDialog<SelectVariableParametersControl>(ObjectiveFunctionInput) as OptimizationProblemVM;

                
                if (res != null)
                {
                    OptimizationProblemVM = res;
                    OptimizationProblemVM.Extremum = Extremum.Max;
                    VariablesKeys = OptimizationProblem.GetVariables(OptimizationProblemVM.FunctionExpression.Formula).ToList();
                }
            });
        }
    }

    private RelayCommand _addSecondRoundRestriction;

    public RelayCommand AddSecondRoundRestriction
    {
        get
        {
            return _addSecondRoundRestriction ??= new RelayCommand(o =>
            {
                OptimizationProblemVM.SecondRoundRestrictions.Add(new SecondRoundRestrictionVM(){Expression = new FunctionExpressionVM(){Formula = SecondRoundRestrictionInput}});
            }, _ =>
            {
                if (SecondRoundRestrictionInput is not null)
                {
                    return SecondRoundRestrictionInput.Length > 0 && _validator.Validate(this)
                                                                               .Errors.All(e => e.PropertyName != nameof(SecondRoundRestrictionInput));
                }

                return false;
            });
        }
    }


    private RelayCommand _optimizeCommand;

    public RelayCommand OptimizeCommand
    {
        get
        {
            return _optimizeCommand ??= new RelayCommand(o =>
            {
                var problem = OptimizationProblemVM.Adapt<OptimizationProblem>();
                var method = new ComplexBoxMethod(problem, 0.001);
                var p = method.SolveProblem();
                var mb = new MessageBox();
                mb.Title = "Результат  оптимизации";
                var sb = new StringBuilder();

                foreach (var variable in p.X)
                {
                    sb.AppendLine($"{variable.Key}: {variable.Value}");
                }

                mb.Content = sb.ToString();
                mb.ShowDialog();
            }, o => OptimizationProblemVM is not null && !OptimizationProblemVM.HasErrors);
        }
    }
    
    public IEnumerable GetErrors(string? propertyName)
    {
        var errors = _validator.Validate(this).Errors.Where(e=>e.PropertyName==propertyName);
        return errors;
    }

    public bool HasErrors
    {
        get
        {
            var res = _validator.Validate(this);
            return !res.IsValid || OptimizationProblemVM is null || OptimizationProblemVM.HasErrors;
        }
    }

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    
}
