﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public bool IsProblemInitialized { get; set; }
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

    public OptimizationProblemVM OptimizationProblemVM { get; private set; }

    public void SetExistingProblem(OptimizationProblemVM problemVm)
    {
        OptimizationProblemVM = problemVm;
        
        if (OptimizationProblemVM.ObjectiveFunction is not null)
        {
            ObjectiveFunctionInput = OptimizationProblemVM.ObjectiveFunction.Formula;
        }
        VariablesKeys = OptimizationProblem.GetVariables(OptimizationProblemVM.ObjectiveFunction.Formula).ToList();
        ObjectiveParameter = OptimizationProblemVM.ObjectiveParameter;
        IsProblemInitialized = true;
    }

    public void SetNewProblem()
    {
        OptimizationProblemVM = new OptimizationProblemVM();
        IsProblemInitialized = false;
    }
    public RelayCommand InputObjectiveFunction
    {
        get
        {
            return _inputObjectiveFunction ??= new RelayCommand(async o =>
            {
                var res = await _dialogService.ShowDialog<SelectVariableParametersControl>(ObjectiveFunctionInput) as OptimizationProblemVM;
                
                //todo вынести это куда-нибудь в более подходящее место
                if (res != null)
                {
                    VariablesKeys = OptimizationProblem.GetVariables(res.ObjectiveFunction.Formula).ToList();
                    
                    if (IsProblemInitialized)
                    {
                        //если в новом выражении есть независимые переменные, которые были в старом, то устанавливаем им ограничения первого рода как в старых
                        var intersectedDecisionVariables = res.DecisionVariables
                                                              .Where(x => OptimizationProblemVM.DecisionVariables.Any(y => y.Key == x.Key));

                        foreach (var intersectedDecisionVariable in intersectedDecisionVariables)
                        {
                            var oldVar = OptimizationProblemVM.DecisionVariables
                                                              .First(x => x.Key == intersectedDecisionVariable.Key);
                            intersectedDecisionVariable.FirstRoundConstraint = oldVar.FirstRoundConstraint;
                        }
                        OptimizationProblemVM.DecisionVariables = res.DecisionVariables;
                        //если в новом выражении есть константы, которые были в старом, то утсанавливаем им значение как в старых
                        var intersectedConstants = res.Constants
                                                              .Where(x => OptimizationProblemVM.Constants.Any(y => y.Key == x.Key));

                        foreach (var intersectedConstant in intersectedConstants)
                        {
                            var oldVar = OptimizationProblemVM.Constants
                                                              .First(x => x.Key == intersectedConstant.Key);
                            intersectedConstant.Value = oldVar.Value;
                        }
                        OptimizationProblemVM.Constants = res.Constants;
                        //если старые ограничения второго рода валидны для новой целевой функции, то оставляем их

                        var secondRoundConstraintsToRemove = OptimizationProblemVM.SecondRoundConstraints
                                                                                  .Where(secondRoundConstraint => OptimizationProblem.GetVariables(secondRoundConstraint.ConstraintFunction.Formula).Any(key => !VariablesKeys.Contains(key)))
                                                                                  .ToList();

                        foreach (var secondRoundConstraint in secondRoundConstraintsToRemove)
                        {
                            OptimizationProblemVM.SecondRoundConstraints.Remove(secondRoundConstraint);
                        }
                    }
                    else
                    {
                        OptimizationProblemVM = res;
                        OptimizationProblemVM.Extremum = Extremum.Max;
                        OptimizationProblemVM.ObjectiveParameter = ObjectiveParameter;
                    }

                    OptimizationProblemVM.ObjectiveFunction.Formula = res.ObjectiveFunction.Formula;
                    IsProblemInitialized = true;

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
