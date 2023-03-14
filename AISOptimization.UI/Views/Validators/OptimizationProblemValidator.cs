using AISOptimization.Domain;

using FluentValidation;


namespace AISOptimization.Views.Validators;

public class OptimizationProblemValidator : AbstractValidator<OptimizationProblem>
{
    // public OptimizationProblemValidator()
    // {
    //     this.RuleForEach(x => x.VectorX)
    //         .ChildRules(x =>
    //         {
    //             x.RuleFor(x => x.FirstRoundRestriction)
    //              .SetValidator(new FirstRoundRestrictionVMValidator());
    //         });
    //     
    // }
}


