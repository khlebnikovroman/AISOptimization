using AISOptimization.Domain;
using AISOptimization.Views.Pages;

using FluentValidation;


namespace AISOptimization.Views.Validators;

public class OptimizationPageVMValidator : AbstractValidator<OptimizationPageVM>
{
    public OptimizationPageVMValidator()
    {
        CascadeMode = CascadeMode.StopOnFirstFailure;

        
    }
}


