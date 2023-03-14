using System.Collections.ObjectModel;

using AISOptimization.VMs;

using WPF.Base;


namespace AISOptimization.Views.Pages;

public class ProblemListVM: BaseVM
{
    public ObservableCollection<OptimizationProblemVM> OptimizationProblems { get; set; }
    public OptimizationProblemVM SelectedOptimizationProblem { get; set; }
}
