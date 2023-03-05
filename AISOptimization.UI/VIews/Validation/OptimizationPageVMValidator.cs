using FluentValidation;


namespace AISOptimization.VIews.Pages;

public class OptimizationPageVMValidator : AbstractValidator<OptimizationPageVM>
{
    public OptimizationPageVMValidator()
    {
        this.CascadeMode = CascadeMode.StopOnFirstFailure;
        this.RuleFor(x => x.ObjectiveFunctionInput)
            .NotEmpty()
            .WithMessage("Поле не должно быть пустым");

        this.RuleFor(x => x.ObjectiveParameter)
            .NotEmpty()
            .WithMessage("Целевой параметр не должен быть пустым");
        this.RuleFor(x => x.OptimizationProblem)
            .SetValidator(new OptimizationProblemValidator());
    }
}
