using System.Collections;
using System.ComponentModel;

using FluentValidation;

using WPF.Base;


namespace AISOptimization.Core;

public enum FirstRoundRestrictionSatisfactory
{
    
    LessThanMin,
    LessOrEqualMin,
    OK,
    BiggerOrEqualMax,
    BiggerThanMax
}

public class FirstRoundRestriction: BaseVM , ICloneable, INotifyDataErrorInfo
{
    public static List<string> Signs { get; } = new() {"<", "≤"};
    public double Max { get; set; } = 1;
    public double Min { get; set; } = 0;
    private string _lessSign = "<";
    public IValidator<FirstRoundRestriction> Validator { get; set; } = new FirstRoundRestrictionValidator();
    public string LessSign
    {
        get
        {
            return _lessSign;
        }
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

    private string _biggerSign = "<";

    public string BiggerSign
    {
        get
        {
            return _biggerSign;
        }
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

    public object Clone()
    {
        return MemberwiseClone();
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        return Validator.Validate(this).Errors.Where(e => e.PropertyName == propertyName);
    }

    public bool HasErrors => !Validator.Validate(this).IsValid;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
}
