using Microsoft.Extensions.DependencyInjection;

namespace FortyOne.OrchestratR.DependencyInjection;

/// <summary>
/// Provides extension methods for configuring the orchestrator in an <see cref="IServiceCollection"/>.
/// </summary>
/// <remarks>
/// These extension methods allow for fluent registration and configuration of the orchestration
/// services within the application's dependency injection container, enabling:
/// <list type="bullet">
///   <li>Registration of the core orchestration services</li>
///   <li>Configuration through a delegate or direct access to the configurator</li>
///   <li>Integration with the standard .NET dependency injection pattern</li>
/// </list>
/// </remarks>
public static class OrchestratorServiceCollectionExtensions
{
    /// <summary>
    /// Adds and configures the orchestrator services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add orchestrator services to.</param>
    /// <param name="configure">A delegate to configure the orchestrator services.</param>
    /// <returns>The same service collection for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="configure"/> is null.</exception>
    /// <remarks>
    /// This method registers core orchestrator services and allows configuration through a delegate.
    /// </remarks>
    /// <example>
    /// <code>
    /// services.AddOrchestrator(options => {
    ///     options.RegisterHandlersFromAssembly(typeof(Startup).Assembly);
    ///     options.UseActionExecutionInterceptor(typeof(LoggingInterceptor&lt;,&gt;));
    /// });
    /// </code>
    /// </example>
    public static IServiceCollection AddOrchestrator(this IServiceCollection services, Action<IOrchestratorConfigurator> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        var configurator = services.AddOrGetConfigurator();
        configure(configurator);

        return services;
    }

    /// <summary>
    /// Adds orchestrator services to the specified <see cref="IServiceCollection"/> and returns a configurator for further setup.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add orchestrator services to.</param>
    /// <returns>An <see cref="IOrchestratorConfigurator"/> for configuring the orchestrator.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is null.</exception>
    /// <remarks>
    /// This method registers core orchestrator services and returns a configurator object
    /// that can be used for further configuration through its fluent API.
    /// </remarks>
    /// <example>
    /// <code>
    /// services
    ///     .AddOrchestrator()
    ///     .RegisterHandlersFromAssembly(typeof(Program).Assembly)
    ///     .UseActionExecutionInterceptor(typeof(ValidationInterceptor&lt;,&gt;));
    /// </code>
    /// </example>
    public static IOrchestratorConfigurator AddOrchestrator(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        var configurator = services.AddOrGetConfigurator();
        return configurator;
    }

    /// <summary>
    /// Gets an existing configurator from the service collection or creates and adds a new one.
    /// </summary>
    private static IOrchestratorConfigurator AddOrGetConfigurator(this IServiceCollection services)
    {
        var existingDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IOrchestratorConfigurator));
        if (existingDescriptor != null)
        {
            return (IOrchestratorConfigurator)existingDescriptor.ImplementationInstance!;
        }

        var actionExecutionChain = new ActionExecutionChain();
        services.AddSingleton(actionExecutionChain);

        var configurator = new OrchestratorConfigurator(actionExecutionChain, services);
        services.AddSingleton<IOrchestratorConfigurator>(configurator);

        services.AddTransient<IOrchestrator, Orchestrator>();

        return configurator;
    }
}
