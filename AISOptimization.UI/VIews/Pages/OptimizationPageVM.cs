using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Documents;

using AISOptimization.Core;
using AISOptimization.Services;

using FluentValidation;

using org.matheval;

using WPF.Base;

using Wpf.Ui.Controls;


namespace AISOptimization.VIews.Pages;

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

    public string ObjectiveFunctionInput { get; set; } = "x + f - d";

    public OptimizationProblem OptimizationProblem { get; set; }
    
    private RelayCommand _inputObjectiveFunction;

    public RelayCommand InputObjectiveFunction
    {
        get
        {
            return _inputObjectiveFunction ??= new RelayCommand(async o =>
            {
                var res = await _dialogService.ShowDialog<SelectVariableParametersControl>(ObjectiveFunctionInput) as OptimizationProblem;

                
                if (res != null)
                {
                    OptimizationProblem = res;
                    OptimizationProblem.Extremum = Extremum.Max;
                    VariablesKeys = OptimizationProblem.GetVariables().ToList();
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
                OptimizationProblem.SecondRoundRestrictions.Add(new SecondRoundRestriction(){Expression = new Expression(SecondRoundRestrictionInput)});
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
                var p = OptimizationProblem.OptimizationMethod.GetBestXPoint();
                var mb = new MessageBox();
                mb.Title = "Результат  оптимизации";
                var sb = new StringBuilder();

                foreach (var variable in p.X)
                {
                    sb.AppendLine($"{variable.Key}: {variable.Value}");
                }

                mb.Content = sb.ToString();
                mb.ShowDialog();
            }, o => OptimizationProblem is not null && !OptimizationProblem.HasErrors());
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
            return !res.IsValid || OptimizationProblem is null || OptimizationProblem.HasErrors();
        }
    }

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    
}
