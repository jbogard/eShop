using Npgsql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace eShop.ServiceDefaults;

public static class EndpointConfigurationService
{
    public static IHostApplicationBuilder UseNServiceBusWithConventions(this IHostApplicationBuilder builder, string endpointName,
        string rabbitMqConnectionStringName, string postgresConnectionStringName)
    {
        var endpointConfiguration = new EndpointConfiguration(endpointName);

        // RabbitMQ Transport: https://docs.particular.net/transports/rabbitmq/
        var rabbitMqConnectionString = builder.Configuration.GetConnectionString(rabbitMqConnectionStringName);
        var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), rabbitMqConnectionString)
        {
            TransportTransactionMode = TransportTransactionMode.ReceiveOnly
        };
        transport.DisabledBrokerRequirementChecks = (BrokerRequirementChecks.Version310OrNewer | BrokerRequirementChecks.StreamsEnabled);
        endpointConfiguration.UseTransport(transport);

        // Define routing for commands: https://docs.particular.net/nservicebus/messaging/routing#command-routing
        // routing.RouteToEndpoint(typeof(MessageType), "DestinationEndpointForType");
        // routing.RouteToEndpoint(typeof(MessageType).Assembly, "DestinationForAllCommandsInAssembly");

        // SQL Persistence: https://docs.particular.net/persistence/sql/
        // PostgreSQL dialect: https://docs.particular.net/persistence/sql/dialect-postgresql
        var dbConnectionString = builder.Configuration.GetConnectionString(postgresConnectionStringName);
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var dialect = persistence.SqlDialect<SqlDialect.PostgreSql>();
        
        persistence.ConnectionBuilder(() => new NpgsqlConnection(dbConnectionString));

        // Message serialization
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        // Installers are useful in development. Consider disabling in production.
        // https://docs.particular.net/nservicebus/operations/installers
        endpointConfiguration.EnableInstallers();
        
        builder.UseNServiceBus(endpointConfiguration);
        
        return builder;
    }

}
