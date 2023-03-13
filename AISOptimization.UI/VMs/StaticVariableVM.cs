using WPF.Base;


namespace  AISOptimization.UI.VM.VMs;

public class StaticVariableVM: BaseVM
{
    public string Description { get; set; }
    public string Key { get; set; }
    public double Value { get; set; }
    public override string ToString()
    {
        return $"{Key} = {Value}";
    }
}
