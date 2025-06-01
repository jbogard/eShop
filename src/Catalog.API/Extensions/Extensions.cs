using eShop.Catalog.API.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        // Avoid loading full database config and migrations if startup
        // is being invoked from build-time OpenAPI generation
        if (builder.Environment.IsBuild())
        {
            builder.Services.AddDbContext<CatalogContext>();
            return;
        }

        builder.AddNpgsqlDbContext<CatalogContext>("catalogdb", configureDbContextOptions: options =>
        {
            options.UseNpgsql(builder =>
            {
                builder.UseVector();
            });
            options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        // REVIEW: This is done for development ease but shouldn't be here in production
        builder.Services.AddMigration<CatalogContext, CatalogContextSeed>();

        builder.Services.AddOptions<CatalogOptions>()
            .BindConfiguration(nameof(CatalogOptions));

        builder.Services.AddScoped<ICatalogAI, CatalogAI>();
    }
}
