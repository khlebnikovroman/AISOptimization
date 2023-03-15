using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using AISOptimization.Domain.Constraints;
using AISOptimization.VMs.Validators;

using FluentValidation;

using Mapster;

using WPF.Base;


namespace AISOptimization.VMs;

/// <summary>
/// VM для <see cref="FirstRoundConstraint"/>
/// </summary>
public class FirstRoundConstraintVM : BaseVM, INotifyDataErrorInfo
{
    public long Id { get; set; }
    private double _max = 1;
    private double _min;
    public static List<string> Signs { get; } = new() {"<", "≤",};
    public string LessSign { get; set; } = "<";
    public string BiggerSign { get; set; } = "<";

    public double Min
    {
        get => _min;
        set
        {
            _min = value;
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Min)));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Max)));
        }
    }

    public double Max
    {
        get => _max;
        set
        {
            _max = value;
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Min)));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Max)));
        }
    }

    public IValidator<FirstRoundConstraintVM> Validator { get; set; } = new FirstRoundConstraintVMValidator();

    public IEnumerable GetErrors(string? propertyName)
    {
        return Validator.Validate(this).Errors.Where(e => e.PropertyName == propertyName);
    }

    public bool HasErrors => !Validator.Validate(this).IsValid;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
}


