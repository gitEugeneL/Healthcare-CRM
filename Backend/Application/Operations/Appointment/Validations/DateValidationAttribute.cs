using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Application.Operations.Appointment.Validations;

internal class DateValidationAttribute : ValidationAttribute
{
    private const int Month = 1;
    private const string Pattern = @"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$";
    private const string Message = "Date must be Y-m-d (1999-12-31). " +
                                   "Please select a future date and date should not be later than +1 month."; 
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return new ValidationResult(Message);

        if (value is not string date)
            return new ValidationResult(Message);
        
        if (!Regex.IsMatch(date, Pattern))
            return new ValidationResult(Message);

        var currentDate = DateOnly.FromDateTime(DateTime.Now);
        
        var valueDate = DateOnly.Parse(date);

        if (valueDate <= currentDate || valueDate >= currentDate.AddMonths(Month))
            return new ValidationResult(Message);

        return ValidationResult.Success;
    }
}
