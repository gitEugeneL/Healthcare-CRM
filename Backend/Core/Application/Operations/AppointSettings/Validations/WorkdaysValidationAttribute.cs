namespace Application.Operations.AppointSettings.Validations;

internal static class WorkdaysValidationAttribute
{
    public static bool BeValidWorkdays(object? value)
    {
        if (value is not int[] workdays)
            return false;

        foreach (var day in workdays)
        {
            if (day is < 1 or > 7)
                return false;
        }
        return true;
    }
}
