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
    private readonly AisOptimizationContext _context;
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
                              AisOptimizationContext context, ISnackbarService snackbarService)
    {
        _dialogService = dialogService;
        _validator = validator;
        _navigationService = navigationService;
        _userService = userService;
        _context = context;
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
            return _saveCommand ??= new RelayCommand(o =>
            {
                //todo вынести логику в сервис
                
                if (IsNewProblem)
                {
                    var problem = ProblemEditControlVm.OptimizationProblemVM.Adapt<OptimizationProblem>();
                    problem.UserId = _userService.User.Id;
                    _context.OptimizationProblems.Add(problem);
                    _context.SaveChanges();
                    ProblemEditControlVm.OptimizationProblemVM.Id = problem.Id;
                    IsNewProblem = false;
                    _snackbarService.Timeout = 2000;
                    _snackbarService.Show("База данных обновлена", "Задача оптимизации добавлена");
                }
                else
                {
                    var findedProblem = _context.OptimizationProblems.Find(ProblemEditControlVm.OptimizationProblemVM.Id);
                    //findedProblem.Adapt(ProblemEditControlVm.OptimizationProblemVM);
                    ProblemEditControlVm.OptimizationProblemVM.Adapt(findedProblem);
                    //_context.Update(findedProblem);
                    _context.SaveChanges();
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


