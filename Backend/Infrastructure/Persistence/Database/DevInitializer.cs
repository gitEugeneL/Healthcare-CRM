using Application.Abstractions.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Database;

public static class DevInitializer
{
   public static void InitializeDatabase(IServiceProvider serviceProvider)
   {
      using var scope = serviceProvider.CreateScope();
      var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
      var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordService>();

      dataContext.Database.Migrate();
        
      DataGenerator.Seed(dataContext, passwordHasher);
   }
}