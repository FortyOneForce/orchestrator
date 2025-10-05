using FortyOne.OrchestratR;
using FortyOne.OrchestratR.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddOrchestrator()
                .RegisterHandlersFromAssembly(typeof(Program).Assembly, (type) =>
                {
                    return ServiceLifetime.Transient;
                })
                .UseActionExecutionInterceptor(typeof(Pipe1<,>))
                .UseActionExecutionInterceptor(typeof(Pipe2<,>)); ;

            var serviceProvider = serviceCollection.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var orchestrator = scope.ServiceProvider.GetRequiredService<IOrchestrator>();

            //await orchestrator.SendAsync(new Request { Name = "Test" });
            var c = await orchestrator.PublishAsync(new EventSignal { Name = "Test" });

        }
    }

    public class Request : IActionRequest
    {
        public string Name { get; set; } = null!;
    }

    public class Pipe1<TRequest, TResponse> : IActionExecutionInterceptor<TRequest, TResponse>
    {
        public async Task<TResponse> HandleAsync(IActionExecutionContext<TRequest> context, ActionExecutionDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();
            return response;
        }
    }

    public class Pipe2<TRequest, TResponse> : IActionExecutionInterceptor<TRequest, TResponse>
    {
        public async Task<TResponse> HandleAsync(IActionExecutionContext<TRequest> context, ActionExecutionDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();
            return response;
        }
    }

    public class Handler : IActionHandler<Request>
    {
        public async Task HandleAsync(Request request, CancellationToken cancellationToken)
        {
            
        }
    }

    public class EventSignal : IEventSignal
    {
        public string Name { get; set; } = null!;
    }

    public class EventHandler : IEventHandler<EventSignal>
    {
        public async Task HandleAsync(EventSignal signal, CancellationToken cancellationToken)
        {

        }
    }


    public class EventHandler22 : IEventHandler<EventSignal>
    {
        public async Task HandleAsync(EventSignal signal, CancellationToken cancellationToken)
        {

        }
    }
}
