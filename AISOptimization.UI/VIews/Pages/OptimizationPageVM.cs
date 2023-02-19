using AISOptimization.Services;


namespace AISOptimization.VIews.Pages;

public class OptimizationPageVM : BaseVM
{
    private readonly MyDialogService _dialogService;

    public OptimizationPageVM(MyDialogService dialogService)
    {
        _dialogService = dialogService;
    }
    private string ObjectiveFunction { get; set; }

    public string ObjectiveFunctionInput { get; set; }
    
    
    
    private RelayCommand _inputObjectiveFunction;

    public RelayCommand InputObjectiveFunction
    {
        get
        {
            return _inputObjectiveFunction ??= new RelayCommand(async o =>
            {
                var res = await _dialogService.ShowDialog<SelectVariableParametersControl>(ObjectiveFunctionInput);
            });
        }
    }

}
