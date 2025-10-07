using BenchmarkDotNet.Attributes;
using FortyOne.OrchestratR;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Benchmarks
{
    [WarmupCount(5)]
    [IterationCount(10)]
    [MemoryDiagnoser]
    public class NotificationBenchmark
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
        public async Task OrchestratorNotify() => await _serviceProvider.GetRequiredService<IOrchestrator>().NotifyAsync(_notification);

        [Benchmark]
        public async Task MediatorPublish() => await _serviceProvider.GetRequiredService<IMediator>().Publish(_notification);


        #region [ Notification ]

        public class Notification : M.INotification, O.INotification
        {
        }

        #endregion

        #region [ Handler ]

        public class Handler : M.INotificationHandler<Notification>, O.INotificationHandler<Notification>
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
