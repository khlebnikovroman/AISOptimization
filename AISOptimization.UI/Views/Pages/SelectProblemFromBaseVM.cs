using System;
using System.Collections.ObjectModel;

using AISOptimization.Services;
using AISOptimization.Utils.Dialog;
using AISOptimization.VMs;


namespace AISOptimization.Views.Pages;

/// <summary>
///     VM для <see cref="SelectProblemFromBase" />
/// </summary>
public class SelectProblemFromBaseVM : IInteractionAware, IDataHolder, IResultHolder
{
    private readonly OptimizationProblemService _optimizationProblemService;

    private RelayCommand _cancelCommand;
    private RelayCommand _selectProblem;

    public SelectProblemFromBaseVM(OptimizationProblemService optimizationProblemService)
    {
        _optimizationProblemService = optimizationProblemService;
    }

    public ProblemListVM ProblemListVm { get; set; }

    public UserVM User { get; set; }

    /// <summary>
    ///     Команда для установуи задачи задачи оптимизации как выбранной пользователем
    /// </summary>
    public RelayCommand SelectProblem
    {
        get
        {
            return _selectProblem ??= new RelayCommand(o =>
            {
                Result = ProblemListVm.SelectedOptimizationProblem;
                FinishInteraction();
            }, _ => ProblemListVm is not null || ProblemListVm.SelectedOptimizationProblem is not null);
        }
    }

    /// <summary>
    ///     Команда для отмены выбора задачи оптимизации
    /// </summary>
    public RelayCommand CancelCommand
    {
        get
        {
            return _cancelCommand ??= new RelayCommand(o =>
            {
                FinishInteraction();
            });
        }
    }

    public object Data
    {
        get => User;
        set
        {
            User = (UserVM) value;

            ProblemListVm.OptimizationProblems = new ObservableCollection<OptimizationProblemVM>(_optimizationProblemService.GetAll(User.Id).Result) ;

        }
    }

    public Action FinishInteraction { get; set; }
    public object Result { get; private set; }
}

