using AISOptimization.Domain;
using AISOptimization.Views.Pages;

using FluentValidation;


namespace AISOptimization.Views.Validators;

/// <summary>
/// Валидатор для <see cref="OptimizationPageVM"/>
/// </summary>
public class OptimizationPageVMValidator : AbstractValidator<OptimizationPageVM>
{
    public OptimizationPageVMValidator()
    {
        CascadeMode = CascadeMode.StopOnFirstFailure;

        
    }
}


