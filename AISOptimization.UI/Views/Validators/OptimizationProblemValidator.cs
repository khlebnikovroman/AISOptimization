using AISOptimization.Core;
using AISOptimization.Core.Restrictions;
using AISOptimization.VMs.Validators;

using FluentValidation;
using FluentValidation.Results;


namespace AISOptimization.Views.Pages;

public class OptimizationProblemValidator: AbstractValidator<OptimizationProblem>
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
