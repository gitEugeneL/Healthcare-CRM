namespace Infrastructure.Persistence;

public static class ApplicationDbContextInitializer
{
    public static void Init(DataContext dataContext)
    {
        dataContext.Database.EnsureDeleted();
        dataContext.Database.EnsureCreated();
        DataGenerator.Seed(dataContext);
    }
}
