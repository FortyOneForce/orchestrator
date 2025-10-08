using FortyOne.OrchestratR.Extensions;
using FortyOne.OrchestratR.Proxies;
using Microsoft.Extensions.DependencyInjection;
using System.Transactions;

namespace FortyOne.OrchestratR
{
    internal class NotificationPublisher
    {
        private readonly IServiceProvider _serviceProvider;
        public NotificationPublisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task NotifyAsync<TNotification>(TNotification notification, NotificationProcessingOptions options, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            ArgumentNullException.ThrowIfNull(notification);

            var enumerableProxyType = notification.GetEnumerableProxyType();
            var proxyInstances = ((IEnumerable<INotificationHandlerProxy>)_serviceProvider.GetRequiredService(enumerableProxyType)).ToArray();

            if (proxyInstances.Length == 0)
            {
                return;
            }

            var tasks = new Task[proxyInstances.Length];
            for (int i = 0; i < proxyInstances.Length; i++)
            {
                tasks[i] = proxyInstances[i].ProxyHandleAsync(notification, cancellationToken);
            }

            await ExecuteWithStrategy(tasks, options);
        }

        private static async Task ExecuteWithStrategy(IEnumerable<Task> tasks, NotificationProcessingOptions options)
        {
            ArgumentNullException.ThrowIfNull(tasks);

            var continueOnError = options.HasFlag(NotificationProcessingOptions.ContinueOnError);
            var isSequential = options.HasFlag(NotificationProcessingOptions.Sequential);
            var isTransactional = options.HasFlag(NotificationProcessingOptions.Transactional);

            Task[] taskArray = !continueOnError
                ? tasks.ToArray()
                : tasks.Select(async t =>
                {
                    try
                    {
                        await t;
                    }
                    catch
                    {
                        if (!continueOnError)
                        {
                            throw;
                        }
                    }
                }).ToArray();

            using var transaction = isTransactional
                ? new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled)
                : null;

            if (isSequential)
            {
                foreach (var task in taskArray)
                {
                    await task;
                }
            }
            else
            {
                await Task.WhenAll(taskArray);
            }

            transaction?.Complete();
        }
    }
}
