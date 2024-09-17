using Koi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Koi.WebAPI.Injection
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app, ILogger _logger)
        {
            try
            {
                using IServiceScope scope = app.ApplicationServices.CreateScope();

                using KoiFarmShopDbContext dbContext =
                    scope.ServiceProvider.GetRequiredService<KoiFarmShopDbContext>();

                dbContext.Database.Migrate();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An problem occurred during migration!");
            }
        }
    }
}
