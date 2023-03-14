using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;

using AISOptimization.Domain;
using AISOptimization.Domain.OptimizationMethods;
using AISOptimization.Services;
using AISOptimization.VMs;
using AISOptimization.VMs.Validators;

using FluentValidation;

using Mapster;

using WPF.Base;

using Wpf.Ui.Contracts;


namespace AISOptimization.Views.Pages;

public class OptimizationPageVM : BaseVM, INotifyDataErrorInfo
{
    private readonly MyDialogService _dialogService;
    private readonly INavigationService _navigationService;
    private readonly AbstractValidator<OptimizationPageVM> _validator;

    private RelayCommand _addSecondRoundConstraint;

    private RelayCommand _inputObjectiveFunction;

    private RelayCommand _optimizeCommand;

    private RelayCommand _removeSecondRoundConstraint;

    private RelayCommand _showPlot;

    public OptimizationPageVM(MyDialogService dialogService, AbstractValidator<OptimizationPageVM> validator, INavigationService navigationService)
    {
        _dialogService = dialogService;
        _validator = validator;
        _navigationService = navigationService;
    }

    public List<string> VariablesKeys { get; private set; }
    public string SecondRoundConstraintInput { get; set; }
    public string ObjectiveParameter { get; set; } = "z";

    public string ObjectiveFunctionInput { get; set; } = "x^2 + y^2";

    public OptimizationProblemVM OptimizationProblemVM { get; set; }
    public OptimizationResultVM OptimizationProblemResult { get; set; }

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
                    OptimizationProblemResult = null;
                }
            });
        }
    }

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

    public RelayCommand OptimizeCommand
    {
        get
        {
            return _optimizeCommand ??= new RelayCommand(o =>
            {
                var json = JsonSerializer.Serialize(OptimizationProblemVM);
                File.WriteAllText("json.txt", json);
                var problem = OptimizationProblemVM.Adapt<OptimizationProblem>();
                var method = new ComplexBoxMethod(problem, 0.001);
                var p = method.SolveProblem();
                var resVM = problem.Adapt<OptimizationProblemVM>().Adapt<OptimizationResultVM>();
                resVM.DecisionVariables = p.DecisionVariables.Adapt<ObservableCollection<DecisionVariableVM>>();
                resVM.ObjectiveFunctionResult = problem.GetValueInPoint(p);
                OptimizationProblemResult = resVM;

                // var mb = new MessageBox();
                // mb.Title = "Результат  оптимизации";
                // var sb = new StringBuilder();
                //
                // foreach (var variable in p.X)
                // {
                //     sb.AppendLine($"{variable.Key}: {variable.Value}");
                // }
                //
                // mb.Content = sb.ToString();
                // mb.ButtonLeftName = "Ок";
                // mb.ShowDialog();
                //
                // mb.ButtonRightClick += (_, _) => mb.Close();
                // mb.ButtonLeftClick += (_, _) => mb.Close();
            }, o => OptimizationProblemVM is not null && !OptimizationProblemVM.HasErrors);
        }
    }

    public RelayCommand ShowPlot
    {
        get
        {
            return _showPlot ??= new RelayCommand(async o =>
            {
                switch (OptimizationProblemResult.DecisionVariables.Count)
                {
                    case 1:
                        break;
                    case 2:
                        await _dialogService.ShowDialog<ChartDirectorCharts>(OptimizationProblemResult);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        var errors = _validator.Validate(this).Errors.Where(e => e.PropertyName == propertyName);

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


