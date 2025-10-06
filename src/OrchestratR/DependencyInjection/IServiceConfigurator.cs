using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FortyOne.OrchestratR.DependencyInjection;

public interface IServiceConfigurator
{
    IServiceConfigurator RegisterServicesFromAssembly(Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Transient);
    IServiceConfigurator RegisterServicesFromAssembly(Assembly assembly, Func<Type, ServiceLifetime> serviceLifetimeSelector);
    IServiceConfigurator AddRequestInterceptor(Type interceptorType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient);
}
