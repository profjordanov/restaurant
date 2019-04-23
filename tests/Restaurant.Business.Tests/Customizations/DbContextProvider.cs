using Microsoft.EntityFrameworkCore;
using Restaurant.Persistence.EntityFramework;

namespace Restaurant.Business.Tests.Customizations
{
    public static class DbContextProvider
    {
        public static ApplicationDbContext GetInMemoryDbContext() =>
            new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("Business.Tests").Options);
    }
}
