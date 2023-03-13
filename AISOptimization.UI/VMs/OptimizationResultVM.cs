using System.Collections.ObjectModel;
using WPF.Base;


namespace AISOptimization.UI.VM.VMs;

public class OptimizationResultVM : OptimizationProblemVM
{
    public double ObjectiveFunctionResult { get; set; }
}
