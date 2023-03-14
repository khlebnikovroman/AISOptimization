using AISOptimization.Domain.Common;


namespace AISOptimization.Domain.Constraints;

public enum FirstRoundConstraintSatisfactory
{
    LessThanMin,
    LessOrEqualMin,
    OK,
    BiggerOrEqualMax,
    BiggerThanMax,
}


public class FirstRoundConstraint : Entity, ICloneable
{
    private string _biggerSign = "<";

    private string _lessSign = "<";
    public static List<string> Signs { get; } = new() {"<", "≤",};

    public double Max { get; set; } = 1;

    public double Min { get; set; }

    public string LessSign
    {
        get => _lessSign;
        set
        {
            if (Signs.Contains(value))
            {
                _lessSign = value;
            }
            else
            {
                throw new ArgumentException($"{value} is not correct sign");
            }
        }
    }

    public string BiggerSign
    {
        get => _biggerSign;
        set
        {
            if (Signs.Contains(value))
            {
                _biggerSign = value;
            }
            else
            {
                throw new ArgumentException($"{value} is not correct sign");
            }
        }
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public FirstRoundConstraintSatisfactory IsSatisfied(double value)
    {
        switch (LessSign)
        {
            case "<":
                if (value <= Min)
                {
                    return FirstRoundConstraintSatisfactory.LessOrEqualMin;
                }

                break;
            case "≤":
                if (value < Min)
                {
                    return FirstRoundConstraintSatisfactory.LessThanMin;
                }

                break;
        }

        switch (BiggerSign)
        {
            case "<":
                if (value >= Max)
                {
                    return FirstRoundConstraintSatisfactory.BiggerOrEqualMax;
                }

                break;
            case "≤":
                if (value > Max)
                {
                    return FirstRoundConstraintSatisfactory.BiggerThanMax;
                }

                break;
        }

        return FirstRoundConstraintSatisfactory.OK;
    }
}




