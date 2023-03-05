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

    public double Max
    {
        get => _max;
        set
        {
            _max = value;
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Max)));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Min)));
        } 
    }

    public double Min
    {
        get => _min;
        set
        {
            _min = value;
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Max)));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Min)));
        }
    }

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
    private double _max = 1;
    private double _min = 0;

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
