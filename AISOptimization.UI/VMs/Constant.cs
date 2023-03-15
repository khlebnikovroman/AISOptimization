using WPF.Base;


namespace AISOptimization.VMs;

/// <summary>
/// VM для <see cref="AISOptimization.Domain.Parameters.Constant"/>
/// </summary>
public class Constant : BaseVM
{
    public long Id { get; set; }
    public string Description { get; set; }
    public string Key { get; set; }
    public double Value { get; set; }

    public override string ToString()
    {
        return $"{Key} = {Value}";
    }
}


