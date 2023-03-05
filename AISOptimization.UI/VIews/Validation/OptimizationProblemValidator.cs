using AISOptimization.Core;

using FluentValidation;
using FluentValidation.Results;


namespace AISOptimization.VIews.Pages;

public class OptimizationProblemValidator: AbstractValidator<OptimizationProblem>
{
    public OptimizationProblemValidator()
    {
        this.RuleForEach(x => x.VectorX)
            .ChildRules(x =>
            {
                x.RuleFor(x => x.FirstRoundRestriction)
                 .SetValidator(new FirstRoundRestrictionValidator());
            });
        
    }
}
