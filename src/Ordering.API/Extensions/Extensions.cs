using eShop.Ordering.API.Infrastructure.Behaviors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Sql;

internal static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        
        // Add the authentication services to DI
        builder.AddDefaultAuthentication();

        // Pooling is disabled because of the following error:
        // Unhandled exception. System.InvalidOperationException:
        // The DbContext of type 'OrderingContext' cannot be pooled because it does not have a public constructor accepting a single parameter of type DbContextOptions or has more than one constructor.

        services.AddScoped(b =>
        {
            if (b.GetService<ISynchronizedStorageSession>() is ISqlStorageSession { Connection: not null } session)
            {
                var options = new DbContextOptionsBuilder<OrderingContext>();
                options.UseNpgsql(session.Connection);
                options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));

                var mediator = b.GetRequiredService<IMediator>();

                var context = new OrderingContext(options.Options, mediator);
                
                context.Database.UseTransaction(session.Transaction);

                session.OnSaveChanges((s, cancellationToken) => context.SaveChangesAsync(cancellationToken));

                return context;
            }
            else
            {
                var options = new DbContextOptionsBuilder<OrderingContext>();
                options.UseNpgsql(builder.Configuration.GetConnectionString("orderingdb"));
                options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));

                var mediator = b.GetRequiredService<IMediator>();

                var context = new OrderingContext(options.Options, mediator);

                return context;
            }
        });
        
        //builder.EnrichNpgsqlDbContext<OrderingContext>();

        services.AddMigration<OrderingContext, OrderingContextSeed>();
        
        services.AddHttpContextAccessor();
        services.AddTransient<IIdentityService, IdentityService>();
        
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining<Program>();
            
            options.AddOpenBehavior(typeof(LoggingBehavior<,>));
            options.AddOpenBehavior(typeof(ValidatorBehavior<,>));
            //options.AddOpenBehavior(typeof(TransactionBehavior<,>));
            options.AddOpenBehavior(typeof(TransactionSessionBehavior<,>));
        });

        services.AddValidatorsFromAssemblyContaining<Program>();
    }
}
