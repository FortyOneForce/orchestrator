using BenchmarkDotNet.Attributes;
using FortyOne.OrchestratR;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Benchmarks
{
    [WarmupCount(5)]
    [IterationCount(10)]
    [MemoryDiagnoser]
    public class RequestResponseInterceptorBenchmark
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
                    configure.AddOpenBehavior(typeof(Interceptor1<,>));
                    configure.AddOpenBehavior(typeof(Interceptor2<,>));
                    configure.AddOpenBehavior(typeof(Interceptor3<,>));
                    configure.AddOpenBehavior(typeof(Interceptor4<,>));
                    configure.AddOpenBehavior(typeof(Interceptor5<,>));
                })
                .AddOrchestrator(configure =>
                {
                    configure.RegisterServicesFromAssembly(assembly);
                    configure.AddRequestInterceptor(typeof(Interceptor1<,>));
                    configure.AddRequestInterceptor(typeof(Interceptor2<,>));
                    configure.AddRequestInterceptor(typeof(Interceptor3<,>));
                    configure.AddRequestInterceptor(typeof(Interceptor4<,>));
                    configure.AddRequestInterceptor(typeof(Interceptor5<,>));
                })
                .BuildServiceProvider();

            _request = new Request();
        }


        [Benchmark(Baseline = true)]
        public async Task OrchestratorExecute() => await _serviceProvider.GetRequiredService<IOrchestrator>().SendAsync(_request);

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

        #region [ Interceptors ]

        public class Interceptor1<TRequest, TResponse> : IRequestInterceptor<TRequest, TResponse>, IPipelineBehavior<TRequest, TResponse>
        {
            public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                return next();
            }

            public Task<TResponse> HandleAsync(TRequest request, NextDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                return next();
            }
        }

        public class Interceptor2<TRequest, TResponse> : IRequestInterceptor<TRequest, TResponse>, IPipelineBehavior<TRequest, TResponse>
        {
            public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                return next();
            }

            public Task<TResponse> HandleAsync(TRequest request, NextDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                return next();
            }
        }

        public class Interceptor3<TRequest, TResponse> : IRequestInterceptor<TRequest, TResponse>, IPipelineBehavior<TRequest, TResponse>
        {
            public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                return next();
            }

            public Task<TResponse> HandleAsync(TRequest request, NextDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                return next();
            }
        }

        public class Interceptor4<TRequest, TResponse> : IRequestInterceptor<TRequest, TResponse>, IPipelineBehavior<TRequest, TResponse>
        {
            public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                return next();
            }

            public Task<TResponse> HandleAsync(TRequest request, NextDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                return next();
            }
        }

        public class Interceptor5<TRequest, TResponse> : IRequestInterceptor<TRequest, TResponse>, IPipelineBehavior<TRequest, TResponse>
        {
            public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                return next();
            }

            public Task<TResponse> HandleAsync(TRequest request, NextDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                return next();
            }
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
