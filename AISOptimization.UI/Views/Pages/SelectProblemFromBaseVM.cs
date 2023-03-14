using System;
using System.Collections.ObjectModel;
using System.Linq;

using AISOptimization.Domain;
using AISOptimization.Utils.Dialog;
using AISOptimization.VMs;

using Mapster;


namespace AISOptimization.Views.Pages;

public class SelectProblemFromBaseVM: IInteractionAware,  IDataHolder, IResultHolder
{
    private readonly AisOptimizationContext _context;

    public SelectProblemFromBaseVM(AisOptimizationContext context)
    {
        _context = context;
    }
    public ProblemListVM ProblemListVm { get; set; }
    
    public UserVM User { get; set; }
    public Action FinishInteraction { get; set; }
    public object Data
    {
        get => User;
        set
        {
            User = (UserVM) value;
            var userProblems = _context.OptimizationProblems
                                       .Where(p => p.UserId == User.Id)
                                       .Adapt<ObservableCollection<OptimizationProblemVM>>();
            ProblemListVm.OptimizationProblems = userProblems;
        }
    }
    public object Result { get; private set; }
    private RelayCommand _selectProblem;

    public RelayCommand SelectProblem
    {
        get
        {
            return _selectProblem ??= new RelayCommand(o =>
            {
                Result = ProblemListVm.SelectedOptimizationProblem;
                FinishInteraction();
            },_=>ProblemListVm is not null || ProblemListVm.SelectedOptimizationProblem is not null);
        }
    }

    private RelayCommand _cancelCommand;

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

}
