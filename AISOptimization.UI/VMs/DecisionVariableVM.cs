using WPF.Base;


namespace AISOptimization.VMs;

public class DecisionVariableVM : BaseVM
{
    public long Id { get; set; }
    public string Description { get; set; }
    public string Key { get; set; }
    public double Value { get; set; }
    public FirstRoundConstraintVM FirstRoundConstraint { get; set; }

    public override string ToString()
    {
        return
            $"{Description} {FirstRoundConstraint.Min} {FirstRoundConstraint.LessSign} {Key} {FirstRoundConstraint.BiggerSign} {FirstRoundConstraint.Max}";
    }
}


