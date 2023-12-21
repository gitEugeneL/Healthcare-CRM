using Infrastructure.Security;

namespace Infrastructure.Persistence;

public static class DataGenerator
{
    public static void Seed(DataContext context)
    {
        if (context.Users.Any())
            return;

        var passwordManager = new PasswordManager();
        passwordManager.CreatePasswordHash("defaultPassword1@", out var hash, out var salt);
        
        // bogus code...
    }
}