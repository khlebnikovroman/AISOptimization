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
        PropertyChanged += (sender, args) => ErrorsChanged(sender,new DataErrorsChangedEventArgs(args.PropertyName));
    }

    public string ObjectiveParameter { get; set; } = "z";
    public string ObjectiveFunctionInput { get; set; } = "a + b + c + d +ee";

    public OptimizationProblem OptimizationProblem { get; set; }
    
    private RelayCommand _inputObjectiveFunction;

    public RelayCommand InputObjectiveFunction
    {
        get
        {
            return _inputObjectiveFunction ??= new RelayCommand(async o =>
            {
                OptimizationProblem = await _dialogService.ShowDialog<SelectVariableParametersControl>(ObjectiveFunctionInput) as OptimizationProblem;

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

    // public string Error { get
    // {
    //     if (_validator != null)
    //     {
    //         var results = _validator.Validate(this);
    //         if (results != null && results.Errors.Any())
    //         {
    //             var errors = string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray());
    //             return errors;
    //         }
    //     }
    //     return string.Empty;
    // } }
    //
    // public string this[string columnName]
    // {
    //     get
    //     {
    //         var firstOrDefault = _validator.Validate(this).Errors.FirstOrDefault(lol => lol.PropertyName == columnName);
    //         if (firstOrDefault != null)
    //             return _validator != null ? firstOrDefault.ErrorMessage : "";
    //         return "";
    //     }
    // }

    // public IEnumerable GetErrors(string? propertyName)
    // {
    //     return _validator.Validate(this).Errors.Where(e => e.PropertyName == propertyName);
    // }
    //
    // public bool HasErrors => _validator.Validate(this).IsValid;
    // public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    // public string Error {
    //     get
    //     {
    //         if (_validator != null)
    //         {
    //             var results = _validator.Validate(this);
    //             if (results != null && results.Errors.Any())
    //             {
    //                 var errors = string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray());
    //                 return errors;
    //             }
    //         }
    //         return string.Empty;
    //     }
    // }
    //
    // public string this[string columnName] {
    //     get
    //     {
    //         var firstOrDefault = _validator.Validate(this).Errors.FirstOrDefault(lol => lol.PropertyName == columnName);
    //         if (firstOrDefault != null)
    //             return _validator != null ? firstOrDefault.ErrorMessage : "";
    //         return "";
    //     }
    // }
    public IEnumerable GetErrors(string? propertyName)
    {
        var allErrors = _validator.Validate(this).Errors;
        var errors = allErrors.Where(e => e.PropertyName == propertyName);
        return errors;
    }

    public bool HasErrors
    {
        get
        {
            var isValid = _validator.Validate(this).IsValid;

            return isValid;
        }
    }

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
}
