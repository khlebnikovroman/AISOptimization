using FluentValidation;


namespace AISOptimization.VMs.Validators;

public class FirstRoundConstraintVMValidator : AbstractValidator<FirstRoundConstraintVM>
{
    public FirstRoundConstraintVMValidator()
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



