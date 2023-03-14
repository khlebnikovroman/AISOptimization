using WPF.Base;


namespace AISOptimization.VMs;

public class Constant : BaseVM
{
    public string Description { get; set; }
    public string Key { get; set; }
    public double Value { get; set; }

    public override string ToString()
    {
        return $"{Key} = {Value}";
    }
}


