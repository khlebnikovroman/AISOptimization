using System.Linq;
using System.Text;

using AISOptimization.Core;
using AISOptimization.Services;

using WPF.Base;

using Wpf.Ui.Controls;


namespace AISOptimization.VIews.Pages;

public class OptimizationPageVM : BaseVM
{
    private readonly MyDialogService _dialogService;

    public OptimizationPageVM(MyDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    public string ObjectiveFunctionInput { get; set; } = "a + b + c + d +ee";

    public OptimizationProblem OptimizationProblem { get; set; }
    
    private RelayCommand _inputObjectiveFunction;

    public RelayCommand InputObjectiveFunction
    {
        get
        {
            return _inputObjectiveFunction ??= new RelayCommand(async o =>
            {
                OptimizationProblem = await _dialogService.ShowDialog<SelectVariableParametersControl>(ObjectiveFunctionInput) as OptimizationProblem;

                if (OptimizationProblem != null)
                {
                    OptimizationProblem.Extremum = Extremum.Max;
                }
            });
        }
    }

    private RelayCommand _optimizeCommand;

    public RelayCommand OptimizeCommand
    {
        get
        {
            return _optimizeCommand ??= new RelayCommand(o =>
            {
                var p = OptimizationProblem.OptimizationMethod.GetBestXPoint();
                var mb = new MessageBox();
                mb.Title = "Результат  оптимизации";
                var sb = new StringBuilder();

                foreach (var variable in p.X)
                {
                    sb.AppendLine($"{variable.Key}: {variable.Value}");
                }
                mb.Content = sb.ToString();
                mb.ShowDialog();
            });
        }
    }

}
