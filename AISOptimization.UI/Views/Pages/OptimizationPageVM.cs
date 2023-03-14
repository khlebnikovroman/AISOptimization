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



    private RelayCommand _optimizeCommand;


    private RelayCommand _showPlot;

    public OptimizationPageVM(MyDialogService dialogService, AbstractValidator<OptimizationPageVM> validator, INavigationService navigationService)
    {
        _dialogService = dialogService;
        _validator = validator;
        _navigationService = navigationService;
    }

    public List<string> VariablesKeys { get; private set; }


    public OptimizationResultVM OptimizationProblemResult { get; set; }

    public ProblemEditControlVM ProblemEditControlVm { get; set; }

    

    

    public RelayCommand OptimizeCommand
    {
        get
        {
            return _optimizeCommand ??= new RelayCommand(o =>
            {
                var problem = ProblemEditControlVm.OptimizationProblemVM.Adapt<OptimizationProblem>();
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
            }, o => ProblemEditControlVm.OptimizationProblemVM is not null && !ProblemEditControlVm.OptimizationProblemVM.HasErrors);
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

            return !res.IsValid || ProblemEditControlVm is null || ProblemEditControlVm.OptimizationProblemVM is null || ProblemEditControlVm.OptimizationProblemVM.HasErrors;
        }
    }

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
}


