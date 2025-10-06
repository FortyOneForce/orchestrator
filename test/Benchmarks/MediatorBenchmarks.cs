using BenchmarkDotNet.Attributes;
using Benchmarks.Features;
using Benchmarks.Interceptors;
using FortyOne.OrchestratR;
using FortyOne.OrchestratR.DependencyInjection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Benchmarks
{
    [WarmupCount(5)]
    [IterationCount(10)]
    public class MediatorBenchmarks
    {
        private GetBook.Request _request = null!;
        private IServiceProvider _mediatorServiceProvider = null!;
        private IMediator _mediator = null!;

        private IServiceProvider _ochestratorServiceProvider = null!;
        private IOrchestrator _orchestrator = null!;


        [GlobalSetup]
        public void Setup()
        {
            // Setup MediatR

            _mediatorServiceProvider = new ServiceCollection()
                .AddLogging()
                .AddMediatR(configuration =>
                {
                    configuration.RegisterServicesFromAssembly(typeof(MediatorBenchmarks).Assembly);

                    //configuration.AddOpenBehavior(typeof(ExceptionHandlingInterceptor<,>));
                    //configuration.AddOpenBehavior(typeof(RequestLoggerInterceptor<,>));
                })
                .BuildServiceProvider();
            
            _mediator = _mediatorServiceProvider.GetRequiredService<IMediator>();

            // Setup OrchestratR

            _ochestratorServiceProvider = new ServiceCollection()
                .AddLogging()
                .AddOrchestrator(configure =>
                {
                    configure.RegisterServicesFromAssembly(typeof(MediatorBenchmarks).Assembly);

                    //configure.UseActionExecutionInterceptor(typeof(ExceptionHandlingInterceptor<,>));
                    //configure.UseActionExecutionInterceptor(typeof(RequestLoggerInterceptor<,>));
                })
                .BuildServiceProvider();
            _orchestrator = _ochestratorServiceProvider.GetRequiredService<IOrchestrator>();

            _request = new GetBook.Request { Id = 1 };
        }

        [Benchmark(Baseline = true)]
        public async Task<GetBook.Response> MediatorSend()
            => await _mediator.Send(_request);

        [Benchmark]
        public async Task<GetBook.Response> OrchestratorSend()
            => await _orchestrator.ExecuteAsync(_request);
    }
}
