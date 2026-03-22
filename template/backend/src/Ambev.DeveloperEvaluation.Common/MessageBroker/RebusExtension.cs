using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;
using Serilog;

namespace Ambev.DeveloperEvaluation.Common.MessageBroker;

/// <summary>
/// Extension methods for configuring Rebus as the application message broker.
/// Uses in-memory transport for message pub/sub with Serilog logging integration.
/// </summary>
public static class RebusExtension
{
    private const string QueueName = "sales-queue";

    /// <summary>
    /// Configures Rebus with in-memory transport and Serilog logging.
    /// Automatically registers all IHandleMessages implementations from the specified assemblies.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder</param>
    /// <param name="handlerAssemblies">Assemblies containing Rebus message handlers</param>
    /// <returns>The WebApplicationBuilder for chaining</returns>
    public static WebApplicationBuilder AddDefaultRebus(this WebApplicationBuilder builder, params System.Reflection.Assembly[] handlerAssemblies)
    {
        var network = new InMemNetwork();

        builder.Services.AddRebus(configure => configure
            .Logging(l => l.Serilog(Log.Logger))
            .Transport(t => t.UseInMemoryTransport(network, QueueName))
            .Routing(r => r.TypeBased())
        );

        foreach (var assembly in handlerAssemblies)
        {
            builder.Services.AutoRegisterHandlersFromAssembly(assembly);
        }

        return builder;
    }

    /// <summary>
    /// Logs a startup message confirming that the Rebus message broker is active.
    /// </summary>
    /// <param name="app">The WebApplication</param>
    /// <returns>The WebApplication for chaining</returns>
    public static WebApplication UseDefaultRebus(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILogger<Rebus.Bus.IBus>>();
        logger.LogInformation("Rebus message broker configured with in-memory transport on queue '{QueueName}'", QueueName);
        return app;
    }
}
