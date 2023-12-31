using System.ComponentModel.DataAnnotations;

namespace Application.Operations.AppointSettings.Commands.Config;

internal class ArrayValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not int[] workdays) 
            return new ValidationResult(ErrorMessage);
     
        foreach (var day in workdays)
            if (day is < 1 or > 7)
                return new ValidationResult(ErrorMessage);

        return ValidationResult.Success;
    }
}
