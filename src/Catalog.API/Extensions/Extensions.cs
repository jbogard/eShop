using eShop.Catalog.API.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Sql;

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

        builder.Services.AddScoped(b =>
        {
            
            if (b.GetService<ISynchronizedStorageSession>() is ISqlStorageSession { Connection: not null } session)
            {
                var options = new DbContextOptionsBuilder<CatalogContext>();
                options.UseNpgsql(session.Connection, npgsqlOptions =>
                {
                    npgsqlOptions.UseVector();
                });
                options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));

                var context = new CatalogContext(options.Options, builder.Configuration);

                context.Database.UseTransaction(session.Transaction);

                session.OnSaveChanges((s, cancellationToken) => context.SaveChangesAsync(cancellationToken));

                return context;
            }
            else
            {
                var options = new DbContextOptionsBuilder<CatalogContext>();
                options.UseNpgsql(builder.Configuration.GetConnectionString("catalogdb"), npgsqlOptions =>
                {
                    npgsqlOptions.UseVector();
                });
                options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));

                var context = new CatalogContext(options.Options, builder.Configuration);

                return context;
            }
        });

        // REVIEW: This is done for development ease but shouldn't be here in production
        builder.Services.AddMigration<CatalogContext, CatalogContextSeed>();

        builder.Services.AddOptions<CatalogOptions>()
            .BindConfiguration(nameof(CatalogOptions));

        builder.Services.AddScoped<ICatalogAI, CatalogAI>();
    }
}
