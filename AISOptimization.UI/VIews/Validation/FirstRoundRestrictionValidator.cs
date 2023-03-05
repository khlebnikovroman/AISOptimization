using System.Linq;

using AISOptimization.Core;

using FluentValidation;


namespace AISOptimization.VIews.Pages;

public class FirstRoundRestrictionValidator: AbstractValidator<FirstRoundRestriction>
{
    public FirstRoundRestrictionValidator()
    {
        this.CascadeMode = CascadeMode.Continue;

        this.RuleFor(x => x.Max)
            .NotEmpty()
            .WithMessage("Поле не должно быть пустым")
            .GreaterThanOrEqualTo(x => x.Min)
            .WithMessage("Верхняя граница должна быть больше нижней");
        this.RuleFor(x => x.Min)
            .NotEmpty()
            .WithMessage("Поле не должно быть пустым")
            .LessThanOrEqualTo(x => x.Max)
            .WithMessage("Нижняя граница должна быть меньше верхней");
    }
}
