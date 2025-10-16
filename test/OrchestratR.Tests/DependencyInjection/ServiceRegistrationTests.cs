using FortyOne.OrchestratR.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FortyOne.OrchestratR.Tests.DependencyInjection;

public class ServiceRegistrationTests
{
    [Fact]
    public void AddOrchestrator_RegistersCoreServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddOrchestrator(configure => { });

        // Assert
        var provider = services.BuildServiceProvider();

        // Verify core services were registered
        Assert.NotNull(provider.GetService<IOrchestrator>());
        Assert.NotNull(provider.GetService<IRequestOrchestrator>());
        Assert.NotNull(provider.GetService<INotificationOrchestrator>());
    }

    [Fact]
    public void AddOrchestrator_WhenCalledTwice_ThrowsInvalidOperationException()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddOrchestrator(configure => { });

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            services.AddOrchestrator(configure => { }));
    }

    [Fact]
    public void RegisterServicesFromAssembly_RegistersHandlers()
    {
        // Arrange
        var services = new ServiceCollection();
        var testAssembly = typeof(ServiceRegistrationTests).Assembly;

        // Act
        services.AddOrchestrator(configure =>
        {
            configure.RegisterServicesFromAssembly(testAssembly);
        });

        // Assert
        var provider = services.BuildServiceProvider();
        var descriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(IRequestHandler<TestRequest>) &&
            d.ImplementationType == typeof(TestRequestHandler));

        Assert.NotNull(descriptor);
    }

    [Fact]
    public void WithHandlerTypeLifetime_SetsCorrectLifetime()
    {
        // Arrange
        var services = new ServiceCollection();
        var testAssembly = typeof(ServiceRegistrationTests).Assembly;

        // Act
        services.AddOrchestrator(configure =>
        {
            configure.RegisterServicesFromAssembly(testAssembly);
            configure.WithHandlerTypeLifetime(_ => ServiceLifetime.Singleton);
        });

        // Assert
        var descriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(IRequestHandler<TestRequest>) &&
            d.ImplementationType == typeof(TestRequestHandler));

        Assert.NotNull(descriptor);
        Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
    }

    [Fact]
    public void WithHandlerTypeFilter_FiltersOutHandlers()
    {
        // Arrange
        var services = new ServiceCollection();
        var testAssembly = typeof(ServiceRegistrationTests).Assembly;

        // Act
        services.AddOrchestrator(configure =>
        {
            configure.RegisterServicesFromAssembly(testAssembly);
            configure.WithHandlerTypeFilter((type, kind) =>
                kind != HandlerKind.RequestHandler);
        });

        // Assert
        var descriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(IRequestHandler<TestRequest>) &&
            d.ImplementationType == typeof(TestRequestHandler));

        Assert.Null(descriptor);
    }

    // Test types for service registration testing
    public class TestRequest : IRequest { }

    public class TestRequestHandler : IRequestHandler<TestRequest>
    {
        public Task HandleAsync(TestRequest request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}