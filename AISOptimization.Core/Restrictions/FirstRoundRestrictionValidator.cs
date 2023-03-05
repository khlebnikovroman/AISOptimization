using FluentValidation;


namespace AISOptimization.Core;

public class FirstRoundRestrictionValidator: AbstractValidator<FirstRoundRestriction>
{
    public FirstRoundRestrictionValidator()
    {
        this.CascadeMode = CascadeMode.Continue;

        this.RuleFor(x => x.Max)
            .GreaterThanOrEqualTo(x => x.Min)
            .WithMessage("Верхняя граница должна быть больше нижней");
        this.RuleFor(x => x.Min)
            .LessThanOrEqualTo(x => x.Max)
            .WithMessage("Нижняя граница должна быть меньше верхней");
    }
}
