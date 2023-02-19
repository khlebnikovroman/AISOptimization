namespace AISOptimization.Core;

public enum FirstRoundRestrictionSatisfactory
{
    LessThanMin,
    OK,
    BiggerThanMax
}
public class FirstRoundRestriction 
{
    public double Max { get; set; }
    public double Min { get; set; }

    public FirstRoundRestrictionSatisfactory IsSatisfied(double value)
    {
        if (value > Max)
        {
            return FirstRoundRestrictionSatisfactory.BiggerThanMax;
        }

        if (value<Min)
        {
            return FirstRoundRestrictionSatisfactory.LessThanMin;
        }
        return FirstRoundRestrictionSatisfactory.OK;
    }
}
