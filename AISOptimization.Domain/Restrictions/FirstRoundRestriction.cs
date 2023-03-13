using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

using AISOptimization.Core.Common;

using FluentValidation;


namespace AISOptimization.Core.Restrictions;

public enum FirstRoundRestrictionSatisfactory
{
    LessThanMin,
    LessOrEqualMin,
    OK,
    BiggerOrEqualMax,
    BiggerThanMax,
}


public class FirstRoundRestriction : Entity, ICloneable
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
    
    public FirstRoundRestrictionSatisfactory IsSatisfied(double value)
    {
        switch (LessSign)
        {
            case "<":
                if (value <= Min)
                {
                    return FirstRoundRestrictionSatisfactory.LessOrEqualMin;
                }

                break;
            case "≤":
                if (value < Min)
                {
                    return FirstRoundRestrictionSatisfactory.LessThanMin;
                }

                break;
        }

        switch (BiggerSign)
        {
            case "<":
                if (value >= Max)
                {
                    return FirstRoundRestrictionSatisfactory.BiggerOrEqualMax;
                }

                break;
            case "≤":
                if (value > Max)
                {
                    return FirstRoundRestrictionSatisfactory.BiggerThanMax;
                }

                break;
        }

        return FirstRoundRestrictionSatisfactory.OK;
    }
}


