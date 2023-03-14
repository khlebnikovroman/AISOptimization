using AISOptimization.Domain;
using AISOptimization.Views.Pages;

using FluentValidation;


namespace AISOptimization.Views.Validators;

public class OptimizationPageVMValidator : AbstractValidator<OptimizationPageVM>
{
    public OptimizationPageVMValidator()
    {
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(x => x.ObjectiveFunctionInput)
            .NotEmpty()
            .WithMessage("Поле не должно быть пустым")
            .Must(x => OptimizationProblem.IsValidExpression(x))
            .WithMessage("Выражение должно быть корректным")
            .Must(x => !x.ContainsAny(SpecialFunctions.ComparisonFunctions))
            .WithMessage("Выражение не должно быть неравенством");

        RuleFor(x => x.ObjectiveParameter)
            .NotEmpty()
            .WithMessage("Целевой параметр не должен быть пустым");

        When(x => x.SecondRoundConstraintInput is not null && x.SecondRoundConstraintInput.Length > 0, () =>
        {
            RuleFor(x => x.SecondRoundConstraintInput)
                .Must(expression =>
                {
                    if (expression.Length == 0)
                    {
                        return true;
                    }

                    return OptimizationProblem.IsValidExpression(expression);
                })
                .WithMessage("Выражение должно быть корректным")
                .Must(x => x.ContainsAny(SpecialFunctions.ComparisonFunctions))
                .WithMessage("Выражение должно быть неравенством")
                .Must((model, expression) =>
                {
                    var expressionVariables = OptimizationProblem.GetVariables(expression);

                    foreach (var variable in expressionVariables)
                    {
                        if (!model.VariablesKeys.Contains(variable))
                        {
                            return false;
                        }
                    }

                    return true;
                })
                .WithMessage("Все переменные должны быть объявлены в целевой функции");
        });
    }
}


