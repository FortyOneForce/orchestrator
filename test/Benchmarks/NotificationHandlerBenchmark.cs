using BenchmarkDotNet.Attributes;
using FortyOne.OrchestratR;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Benchmarks
{
    [WarmupCount(5)]
    [IterationCount(10)]
    [MemoryDiagnoser]
    public class NotificationHandlerBenchmark
    {
        private Notification _notification;
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

            _notification = new Notification();
        }


        [Benchmark(Baseline = true)]
        public async Task OrchestratorExecute() => await _serviceProvider.GetRequiredService<IOrchestrator>().NotifyAsync(_notification);

        [Benchmark]
        public async Task OrchestratorExecuteSequential() => await _serviceProvider.GetRequiredService<IOrchestrator>().NotifyAsync(_notification, NotificationProcessingOptions.Sequential);

        [Benchmark]
        public async Task OrchestratorExecuteTransactional() => await _serviceProvider.GetRequiredService<IOrchestrator>().NotifyAsync(_notification, NotificationProcessingOptions.Transactional);

        [Benchmark]
        public async Task OrchestratorExecuteContinueOnError() => await _serviceProvider.GetRequiredService<IOrchestrator>().NotifyAsync(_notification, NotificationProcessingOptions.ContinueOnError);

        [Benchmark]
        public async Task MediatorSendRequest() => await _serviceProvider.GetRequiredService<IMediator>().Publish(_notification);


        #region [ Notification ]

        public class Notification : M.INotification, O.INotification
        {
        }

        #endregion

        #region [ Handler ]

        public class Handler1 : M.INotificationHandler<Notification>, O.INotificationHandler<Notification>
        {
            public Task Handle(Notification notification, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            public Task HandleAsync(Notification notification, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }

        public class Handler2 : M.INotificationHandler<Notification>, O.INotificationHandler<Notification>
        {
            public Task Handle(Notification notification, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            public Task HandleAsync(Notification notification, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }

        public class Handler3 : M.INotificationHandler<Notification>, O.INotificationHandler<Notification>
        {
            public Task Handle(Notification notification, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            public Task HandleAsync(Notification notification, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }

        public class Handler4 : M.INotificationHandler<Notification>, O.INotificationHandler<Notification>
        {
            public Task Handle(Notification notification, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            public Task HandleAsync(Notification notification, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }

        public class Handler5 : M.INotificationHandler<Notification>, O.INotificationHandler<Notification>
        {
            public Task Handle(Notification notification, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            public Task HandleAsync(Notification notification, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }

        #endregion
    }
}
