using WPF.Base;

using AISOptimization.Domain;
namespace AISOptimization.VMs;

/// <summary>
/// VM для <see cref="FuncExpression"/>
/// </summary>
public class FunctionExpressionVM : BaseVM
{
    public long Id { get; set; }
    public string Formula { get; set; }
}


