using BenchmarkDotNet.Attributes;
using FortyOne.OrchestratR;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace Benchmarks
{
    [WarmupCount(5)]
    [IterationCount(10)]
    [MemoryDiagnoser]
    public class RequestBenchmark
    {
        private Request _request;
        private IServiceProvider _serviceProvider;

        [GlobalSetup]
        public void Setup()
        {
            var assembly = typeof(RequestBenchmark).Assembly;

            _serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddMediatR(configure =>
                {
                    configure.RegisterServicesFromAssembly(assembly);
                })
                .AddOrchestrator(configure =>
                {
                    configure.RegisterServicesFromAssembly(assembly);
                })
                .BuildServiceProvider();

            _request = new Request();
        }


        [Benchmark(Baseline = true)]
        public async Task OrchestratorExecute()
        {
            await _serviceProvider.GetRequiredService<IOrchestrator>().SendAsync(_request, middleware =>
            {
                
            });
        }

        [Benchmark]
        public async Task MediatorSendRequest() => await _serviceProvider.GetRequiredService<IMediator>().Send(_request);


        #region [ Request ]

        public class Request : M.IRequest, O.IRequest
        {
        }

        #endregion

        #region [ Handler ]

        public class Handler : M.IRequestHandler<Request>, O.IRequestHandler<Request>
        {
            public Task Handle(Request request, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            public Task HandleAsync(Request request, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }

        #endregion
    }
}
