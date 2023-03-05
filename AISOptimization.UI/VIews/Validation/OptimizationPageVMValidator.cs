using AISOptimization.Core;

using FluentValidation;


namespace AISOptimization.VIews.Pages;

public class OptimizationPageVMValidator : AbstractValidator<OptimizationPageVM>
{
    
    public OptimizationPageVMValidator()
    {
        this.CascadeMode = CascadeMode.StopOnFirstFailure;
        this.RuleFor(x => x.ObjectiveFunctionInput)
            .NotEmpty()
            .WithMessage("Поле не должно быть пустым")
            .Must(x=> OptimizationProblem.IsValidExpression(x))
            .WithMessage("Выражение должно быть корректным")
            .Must(x=> !x.ContainsAny(SpecialFunctions.ComparisonFunctions))
            .WithMessage("Выражение не должно быть неравенством");

        this.RuleFor(x => x.ObjectiveParameter)
            .NotEmpty()
            .WithMessage("Целевой параметр не должен быть пустым");

        When(x => x.SecondRoundRestrictionInput is not null && x.SecondRoundRestrictionInput.Length > 0, () =>
        {
            this.RuleFor(x => x.SecondRoundRestrictionInput)
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
                .Must((model,expression) =>
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
