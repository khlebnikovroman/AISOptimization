using AISOptimization.Domain.Constraints;

using WPF.Base;


namespace AISOptimization.VMs;

/// <summary>
/// Vm для <see cref="SecondRoundConstraint"/>
/// </summary>
public class SecondRoundConstraintVM : BaseVM
{
    public long Id { get; set; }
    public FunctionExpressionVM ConstraintFunction { get; set; }
}


