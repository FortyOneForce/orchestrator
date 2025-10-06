using Samples.WebApp.Features.Books;
using FortyOne.OrchestratR.DependencyInjection;
using Samples.WebApp.Interceptors;

namespace Samples.WebApp;

internal static class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddOrchestrator(configure =>
            {
                // Registers all handlers from the current assembly as scoped

                configure.RegisterServicesFromAssembly(typeof(Program).Assembly, ServiceLifetime.Scoped);

                // Registers action execution interceptors

                configure
                    .AddRequestInterceptor(typeof(ExceptionHandlingInterceptor<,>))
                    .AddRequestInterceptor(typeof(RequestLoggerInterceptor<,>));
            });
            

        var app = builder.Build();

        app.Map("/", () => "Welcome!");
        app.MapBookEndpoints();
        
        await app.RunAsync();
    }
}
