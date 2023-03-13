using WPF.Base;


namespace  AISOptimization.UI.VM.VMs;

public class IndependentVariableVM: BaseVM
{
    public string Key { get; set; }
    public FirstRoundRestrictionVM FirstRoundRestriction { get; set; }
}
