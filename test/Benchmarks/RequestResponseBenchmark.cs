using BenchmarkDotNet.Attributes;
using FortyOne.OrchestratR;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Benchmarks
{
    [WarmupCount(5)]
    [IterationCount(10)]
    [MemoryDiagnoser]
    public class RequestResponseBenchmark
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
        public async Task OrchestratorExecute() => await _serviceProvider.GetRequiredService<IOrchestrator>().ExecuteAsync(_request);

        [Benchmark]
        public async Task MediatorSendRequest() => await _serviceProvider.GetRequiredService<IMediator>().Send(_request);


        #region [ Request ]

        public class Request : M.IRequest<Response>, O.IRequest<Response>
        {
        }

        #endregion

        #region [ Response ]

        public class Response
        {
        }

        #endregion

        #region [ Handler ]

        public class Handler : M.IRequestHandler<Request, Response>, O.IRequestHandler<Request, Response>
        {
            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Response());
            }

            public Task<Response> HandleAsync(Request request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Response());
            }
        }

        #endregion
    }
}
