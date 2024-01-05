using System.ComponentModel.DataAnnotations;

namespace Application.Operations.AppointSettings.Validations;

internal class WorkdaysValidationAttribute : ValidationAttribute
{
    private const string Message = "Workdays must contain values between 1 and 7";
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not int[] workdays) 
            return new ValidationResult(Message);
     
        foreach (var day in workdays)
            if (day is < 1 or > 7)
                return new ValidationResult(Message);

        return ValidationResult.Success;
    }
}
