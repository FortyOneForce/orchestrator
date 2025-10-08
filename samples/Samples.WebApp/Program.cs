using Samples.WebApp.Features.Books;
using FortyOne.OrchestratR.DependencyInjection;
using Samples.WebApp.Interceptors;
using FortyOne.OrchestratR;

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

        //using var scope = app.Services.CreateScope();
        //var orchestrator = scope.ServiceProvider.GetRequiredService<IOrchestrator>();
        //var requests = new GetBookRequest[]
        //{
        //    new GetBookRequest() { Id = 1 },
        //    new GetBookRequest() { Id = 2 },
        //    new GetBookRequest() { Id = 3 },
        //};

        //var a = await orchestrator
        //    .ExecuteBatchAsync(requests, RequestProcessingOptions.Transactional, (response) => response.Any(x => x.Id == 2), default);

        //foreach(var item in a)
        //{
            
        //}

        //if (a.IsFailed)
        //{
        //    var failed = a.FailedResponse;
        //}

        await app.RunAsync();
    }
}
