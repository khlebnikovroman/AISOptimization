using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using AISOptimization.Domain;
using AISOptimization.Services;
using AISOptimization.Views.Validators;
using AISOptimization.VMs;
using AISOptimization.VMs.Validators;

using FluentValidation;

using WPF.Base;

using Wpf.Ui.Contracts;


namespace AISOptimization.Views.Pages;

public class ProblemEditControlVM:BaseVM,INotifyDataErrorInfo
{
    private readonly MyDialogService _dialogService;

    public ProblemEditControlVM(MyDialogService dialogService)
    {
        _dialogService = dialogService;
    }
    public string ObjectiveParameter { get; set; } = "z";
    public string SecondRoundConstraintInput { get; set; }
    public string ObjectiveFunctionInput { get; set; } = "x^2 + y^2";
    public List<string> VariablesKeys { get; private set; }

    private readonly AbstractValidator<ProblemEditControlVM> _validator = new ProblemEditControlVMValidator();


    private RelayCommand _inputObjectiveFunction;

    public OptimizationProblemVM OptimizationProblemVM { get; set; }

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
                    OptimizationProblemVM.ObjectiveParameter = ObjectiveParameter;
                    VariablesKeys = OptimizationProblem.GetVariables(OptimizationProblemVM.ObjectiveFunction.Formula).ToList();
                    //OptimizationProblemResult = null;
                }
            });
        }
    }
    private RelayCommand _addSecondRoundConstraint;

    public RelayCommand AddSecondRoundConstraint
    {
        get
        {
            return _addSecondRoundConstraint ??= new RelayCommand(o =>
            {
                OptimizationProblemVM.SecondRoundConstraints.Add(new SecondRoundConstraintVM
                {
                    ConstraintFunction = new FunctionExpressionVM
                        {Formula = SecondRoundConstraintInput,},
                });

                SecondRoundConstraintInput = "";
            }, _ =>
            {
                if (SecondRoundConstraintInput is not null)
                {
                    return SecondRoundConstraintInput.Length > 0 && !_validator.Validate(this).HasError(nameof(SecondRoundConstraintInput));
                }

                return false;
            });
        }
    }
    private RelayCommand _removeSecondRoundConstraint;

    public RelayCommand RemoveSecondRoundConstraint
    {
        get
        {
            return _removeSecondRoundConstraint ??= new RelayCommand(o =>
            {
                OptimizationProblemVM.SecondRoundConstraints.Remove(o as SecondRoundConstraintVM);
            });
        }
    }
    public IEnumerable GetErrors(string? propertyName)
    {
        var errors = _validator.Validate(this).GetErrors(propertyName);

        return errors;
    }

    public bool HasErrors { get
    {
        var res = _validator.Validate(this);

        return !res.IsValid;
    } }
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
}
