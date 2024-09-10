using Koi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Koi.WebAPI.Injection
{
    public static class MigrationExtensions
    {
        public static async Task ApplyMigrations(this IApplicationBuilder app, ILogger _logger)
        {
            try
            {
                using IServiceScope scope = app.ApplicationServices.CreateScope();

                using KoiFarmShopDbContext dbContext =
                    scope.ServiceProvider.GetRequiredService<KoiFarmShopDbContext>();

                await dbContext.Database.MigrateAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An problem occurred during migration!");
            }
        }
    }
}
