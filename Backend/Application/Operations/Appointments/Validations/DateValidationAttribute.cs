using System.Text.RegularExpressions;

namespace Application.Operations.Appointments.Validations;

internal static class DateValidatorAttribute
{
    private const int Month = 1;
    private const string Pattern = @"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$";

    public static bool BeValidDate(string value)
    {
        if (!Regex.IsMatch(value, Pattern))
            return false;
        
        var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var valueDate = DateOnly.Parse(value);

        return valueDate > currentDate && valueDate < currentDate.AddMonths(Month);
    }
}