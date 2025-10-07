using FortyOne.OrchestratR.Extensions;
using FortyOne.OrchestratR.Proxies;
using Microsoft.Extensions.DependencyInjection;

namespace FortyOne.OrchestratR
{
    internal class NotificationPublisher
    {
        private readonly IServiceProvider _serviceProvider;
        public NotificationPublisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task NotifyAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            ArgumentNullException.ThrowIfNull(notification);

            var enumerableProxyType = notification.GetEnumerableProxyType();
            var proxyInstances = ((IEnumerable<INotificationHandlerProxy>)_serviceProvider.GetRequiredService(enumerableProxyType)).ToArray();
            
            if (proxyInstances.Length == 0)
            {
                return Task.CompletedTask;
            }

            var tasks = new Task[proxyInstances.Length];
            for(int i = 0; i < proxyInstances.Length; i++)
            {
                tasks[i] = proxyInstances[i].ProxyHandleAsync(notification, cancellationToken);
            }

            return Task.WhenAll(tasks);
        }
    }
}
