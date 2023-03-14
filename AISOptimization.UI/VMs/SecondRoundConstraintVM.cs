using WPF.Base;


namespace AISOptimization.VMs;

public class SecondRoundConstraintVM : BaseVM
{
    public long Id { get; set; }
    public FunctionExpressionVM ConstraintFunction { get; set; }
}


