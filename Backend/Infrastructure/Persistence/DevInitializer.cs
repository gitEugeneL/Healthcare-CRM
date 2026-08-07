using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Persistence;

namespace Persistence;

public static class DevInitializer
{
   public static void Initialize(IServiceProvider serviceProvider)
   {
      using var scope = serviceProvider.CreateScope();
      var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
      dataContext.Database.Migrate();
      DataGenerator.Seed(dataContext);
   }
}