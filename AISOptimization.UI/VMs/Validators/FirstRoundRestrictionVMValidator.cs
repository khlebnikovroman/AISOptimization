using AISOptimization.Core.Restrictions;
using AISOptimization.UI.VM.VMs;

using FluentValidation;


namespace AISOptimization.VMs.Validators;

public class FirstRoundRestrictionVMValidator : AbstractValidator<FirstRoundRestrictionVM>
{
    public FirstRoundRestrictionVMValidator()
    {
        CascadeMode = CascadeMode.Continue;

        RuleFor(x => x.Max)
            .GreaterThanOrEqualTo(x => x.Min)
            .WithMessage("Верхняя граница должна быть больше нижней");

        RuleFor(x => x.Min)
            .LessThanOrEqualTo(x => x.Max)
            .WithMessage("Нижняя граница должна быть меньше верхней");
    }
}

