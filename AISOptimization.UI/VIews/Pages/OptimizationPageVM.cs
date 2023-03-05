using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using AISOptimization.Core;
using AISOptimization.Services;

using FluentValidation;

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
                OptimizationProblem = await _dialogService.ShowDialog<SelectVariableParametersControl>(ObjectiveFunctionInput) as OptimizationProblem;

                foreach (var independentVariable in OptimizationProblem.VectorX)
                {
                    independentVariable.FirstRoundRestriction.PropertyChanged += (sender, args) =>
                        ErrorsChanged?.Invoke(sender, new DataErrorsChangedEventArgs(args.PropertyName));
                }
                if (OptimizationProblem != null)
                {
                    OptimizationProblem.Extremum = Extremum.Max;
                }
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
            }, o =>
            {
                var res = _validator.Validate(this);
                
                foreach (var error in res.Errors)
                {
                    Debug.WriteLine($"{error.PropertyName} {error.ErrorMessage}");
                }
                Debug.WriteLine("______________________________________");
                return res.IsValid;
            });
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
            return !res.IsValid;
        }
    }

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    
}
