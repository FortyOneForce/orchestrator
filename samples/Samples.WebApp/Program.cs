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

                configure.RegisterServicesFromAssembly(typeof(Program).Assembly);

                // Registers action execution interceptors

                configure
                    .AddRequestInterceptor(typeof(ExceptionHandlingInterceptor<,>))
                    .AddRequestInterceptor(typeof(RequestLoggerInterceptor<,>));

                // All handlers will be registered as scoped

                configure.WithHandlerTypeLifetime(type =>
                {
                    return ServiceLifetime.Scoped;
                });

                // All types will be considered as handlers

                configure.WithHandlerTypeFilter((type, kind) =>
                {
                    return true;
                });
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        var app = builder.Build();

        app.MapBookEndpoints();
        app.MapSwagger();
        app.UseSwaggerUI(setup =>
        {
            setup.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
            setup.RoutePrefix = string.Empty;
        });
        
        await app.RunAsync();
    }
}
