using System.Linq;

using FluentValidation.Results;


namespace AISOptimization.VMs.Validators;

public static class ValidationResultExtension
{
    public static bool HasError(this ValidationResult result, string propertyName)
    {
        return result.Errors.Any(x => x.PropertyName == propertyName);
    }
}
