using AISOptimization.Core;
using AISOptimization.Services;

using WPF.Base;


namespace AISOptimization.VIews.Pages;

public class OptimizationPageVM : BaseVM
{
    private readonly MyDialogService _dialogService;

    public OptimizationPageVM(MyDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    public string ObjectiveFunctionInput { get; set; } = "a + b + c + d + e = 12";

    public OptimizationProblem OptimizationProblem { get; set; }
    
    private RelayCommand _inputObjectiveFunction;

    public RelayCommand InputObjectiveFunction
    {
        get
        {
            return _inputObjectiveFunction ??= new RelayCommand(async o =>
            {
                OptimizationProblem = await _dialogService.ShowDialog<SelectVariableParametersControl>(ObjectiveFunctionInput) as OptimizationProblem;
            });
        }
    }

}
