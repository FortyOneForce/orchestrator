using Microsoft.Extensions.DependencyInjection;

namespace FortyOne.OrchestratR
{
    internal abstract class ActionExecutionContext : IActionExecutionContext
    {
        public IServiceProvider ServiceProvider { get; }
        public bool IsScoped { get; }
        public Type RequestType { get; }
        public Type ResponseType { get; set; } = null!;
        public IActionExecutionInterceptor[] Interceptors { get; set; } = Array.Empty<IActionExecutionInterceptor>();

        protected ActionExecutionContext(IServiceProvider serviceProvider, Type requestType)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(requestType);

            ServiceProvider = serviceProvider;
            IsScoped = serviceProvider.GetService<IServiceScopeFactory>() != serviceProvider;
            RequestType = requestType;
        }
    }

    internal sealed class ActionExecutionContext<TRequest> : ActionExecutionContext, IActionExecutionContext<TRequest>
    {
        public TRequest Request { get;  }
        

        public ActionExecutionContext(IServiceProvider serviceProvider, TRequest request) : base(serviceProvider, request!.GetType())
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(request);

            Request = request;
        }
    }
}
