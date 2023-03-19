using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
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

using Microsoft.EntityFrameworkCore;

using WPF.Base;

using Wpf.Ui.Contracts;


namespace AISOptimization.Views.Pages;

/// <summary>
/// VM для <see cref="OptimizationPage"/>
/// </summary>
public class OptimizationPageVM : BaseVM, INotifyDataErrorInfo
{
    private readonly MyDialogService _dialogService;
    private readonly INavigationService _navigationService;
    private readonly UserService _userService;
    private readonly OptimizationProblemService _optimizationProblemService;
    private readonly ISnackbarService _snackbarService;
    private readonly AbstractValidator<OptimizationPageVM> _validator;

    public bool IsNewProblem { get; set; }

    public string SaveButtonText => IsNewProblem ? "Добавить в базу данных" : "Сохранить изменения";
    
    private RelayCommand _optimizeCommand;


    private RelayCommand _showPlot;

    public OptimizationPageVM(MyDialogService dialogService,
                              AbstractValidator<OptimizationPageVM> validator,
                              INavigationService navigationService, 
                              UserService userService,
                              OptimizationProblemService optimizationProblemService, ISnackbarService snackbarService)
    {
        _dialogService = dialogService;
        _validator = validator;
        _navigationService = navigationService;
        _userService = userService;
        _optimizationProblemService = optimizationProblemService;
        _snackbarService = snackbarService;
    }

    public List<string> VariablesKeys { get; private set; }
    
    public OptimizationResultVM OptimizationProblemResult { get; set; }

    public ProblemEditControlVM ProblemEditControlVm { get; set; }

    private RelayCommand _selectProblemFromBaseCommand;

    /// <summary>
    /// Команда для выбора задачи оптимизации из базы данных
    /// </summary>
    public RelayCommand SelectProblemFromBaseCommand
    {
        get
        {
            return _selectProblemFromBaseCommand ??= new RelayCommand(async o =>
            {
                var res = await _dialogService.ShowDialog<SelectProblemFromBase>(_userService.User) as OptimizationProblemVM;

                if (res is not null)
                {
                    ProblemEditControlVm.SetExistingProblem(res);
                    IsNewProblem = false;
                }
            });
        }
    }

    private RelayCommand _createNewOptimizationProblemCommand;

    /// <summary>
    /// Команда для создания новой задачи потимизации
    /// </summary>
    public RelayCommand CreateNewOptimizationProblemCommand
    {
        get
        {
            return _createNewOptimizationProblemCommand ??= new RelayCommand(o =>
            {
                ProblemEditControlVm.SetNewProblem();
                IsNewProblem = true;
            });
        }
    }

    private RelayCommand _saveCommand;

    /// <summary>
    /// Сохраняет текущую задачу оптимизации в базу данных
    /// </summary>
    public RelayCommand SaveCommand
    {
        get
        {
            return _saveCommand ??= new RelayCommand(async o =>
            {
                //todo вынести логику в сервис
                
                if (IsNewProblem)
                {
                    ProblemEditControlVm.OptimizationProblemVM.UserId = _userService.User.Id;
                    ProblemEditControlVm.OptimizationProblemVM.Id = await _optimizationProblemService.Create(ProblemEditControlVm.OptimizationProblemVM);
                    IsNewProblem = false;
                    _snackbarService.Timeout = 2000;
                    _snackbarService.Show("База данных обновлена", "Задача оптимизации добавлена");
                }
                else
                {
                    await _optimizationProblemService.Update(ProblemEditControlVm.OptimizationProblemVM);
                    _snackbarService.Timeout = 2000;
                    _snackbarService.Show("База данных обновлена", "Задача оптимизации обновлена");
                }
            });
        }
    }

    
    /// <summary>
    /// Команада для запуска процесса оптимизации
    /// </summary>
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
                resVM.ObjectiveFunctionResult = problem.GetValueInPointForDataView(p);
                OptimizationProblemResult = resVM;
            }, o => ProblemEditControlVm.OptimizationProblemVM is not null && !ProblemEditControlVm.OptimizationProblemVM.HasErrors);
        }
    }

    /// <summary>
    /// Команад для отображения окна с графиками
    /// </summary>
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


    #region INotifyDataErrorInfo

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

#endregion
    
}


