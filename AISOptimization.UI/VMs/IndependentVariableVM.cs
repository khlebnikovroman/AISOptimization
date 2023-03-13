using WPF.Base;


namespace  AISOptimization.UI.VM.VMs;

public class IndependentVariableVM: BaseVM
{
    public string Description { get; set; }
    public string Key { get; set; }
    public double Value { get; set; }
    public FirstRoundRestrictionVM FirstRoundRestriction { get; set; }
    public override string ToString()
    {
        return
            $"{Description} {FirstRoundRestriction.Min} {FirstRoundRestriction.LessSign} {Key} {FirstRoundRestriction.BiggerSign} {FirstRoundRestriction.Max}";
    }
}
