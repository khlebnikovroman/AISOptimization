using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using AISOptimization.VMs.Validators;

using FluentValidation;

using Mapster;

using WPF.Base;


namespace  AISOptimization.UI.VM.VMs;

[AdaptTo("[name]Dto")]
public class FirstRoundRestrictionVM: BaseVM, INotifyDataErrorInfo
{
    public static List<string> Signs { get; } = new() {"<", "≤",};
    public string LessSign { get; set; } = "<";
    public string BiggerSign { get; set; } = "<";
    public double Min { get; set; } = 0;
    public double Max { get; set; } = 1;
    public IValidator<FirstRoundRestrictionVM> Validator { get; set; } = new FirstRoundRestrictionVMValidator();
    public IEnumerable GetErrors(string? propertyName)
    {
        return Validator.Validate(this).Errors.Where(e => e.PropertyName == propertyName);
    }

    public bool HasErrors => !Validator.Validate(this).IsValid;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
}
