using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Database;

public static class DevInitializer
{
   public static void Initialize(IServiceProvider serviceProvider)
   {
      using var scope = serviceProvider.CreateScope();
      var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
      var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordManager>();

      dataContext.Database.Migrate();
        
      DataGenerator.Seed(dataContext, passwordHasher);
   }
}