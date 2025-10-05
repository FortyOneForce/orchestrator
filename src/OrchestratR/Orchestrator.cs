using Microsoft.Extensions.DependencyInjection;

namespace FortyOne.OrchestratR
{
    /// <summary>
    /// Default implementation of the orchestration service that coordinates request processing and event distribution.
    /// </summary>
    internal class Orchestrator : IOrchestrator
    {
        private readonly ActionExecutionChain _actionExecutionChain;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Orchestrator"/> class.
        /// </summary>
        public Orchestrator(ActionExecutionChain actionExecutionChain, IServiceProvider serviceProvider)
        {
            _actionExecutionChain = actionExecutionChain;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Publishes an event signal to all registered handlers and returns the number of handlers that processed it.
        /// </summary>
        public async Task<int> PublishAsync(IEventSignal signal, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(signal);

            var handlerType = typeof(IEventHandler<>).MakeGenericType(signal.GetType());
            var enumerableHandlerType = typeof(IEnumerable<>).MakeGenericType(handlerType);
            var eventHandlers = ((IEnumerable<IHandlerBase>)_serviceProvider.GetRequiredService(enumerableHandlerType))
                .Select(x => new
                {
                    Instance = x,
                    HandleAsyncMethod = x.GetType().GetMethod("HandleAsync")!
                })
                .ToArray();

            var tasks = eventHandlers.Select(h => (Task)h.HandleAsyncMethod.Invoke(h.Instance, new object[] { signal, cancellationToken })!);
            await Task.WhenAll(tasks);

            return eventHandlers.Length;
        }

        /// <summary>
        /// Sends a request to be processed by a single handler with no expected return value.
        /// </summary>
        public async Task SendAsync(IActionRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            var context = CreateActionExecutionContext(request, typeof(object));
            var actionHandler = ResolveActionHandler(context.RequestType, typeof(object));
            var next = CreateActionExecutionDelegate<object>(actionHandler, request, cancellationToken);
            var chain = CreateExecutionChain(context, next, cancellationToken);

            _ = await chain();
        }

        /// <summary>
        /// Sends a request to be processed by a single handler and returns the result.
        /// </summary>
        public async Task<TResponse> SendAsync<TResponse>(IActionRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            var context = CreateActionExecutionContext(request, typeof(TResponse));
            var actionHandler = ResolveActionHandler(context.RequestType, typeof(TResponse));
            var next = CreateActionExecutionDelegate<TResponse>(actionHandler, request, cancellationToken);
            var chain = CreateExecutionChain(context, next, cancellationToken);

            return await chain();
        }

        /// <summary>
        /// Creates an execution context for the request with the specified response type.
        /// </summary>
        private ActionExecutionContext CreateActionExecutionContext(IActionRequestBase request, Type responseType)
        {
            var requestType = request.GetType();
            var contextType = typeof(ActionExecutionContext<>).MakeGenericType(requestType);


            var context = (ActionExecutionContext)Activator.CreateInstance(contextType, _serviceProvider, request)!;
            context.ResponseType = responseType;
            return context;
        }

        /// <summary>
        /// Resolves the appropriate handler for the specified request and response types.
        /// </summary>
        private ResolvedActionHandler ResolveActionHandler(Type requestType, Type responseType)
        {
            ArgumentNullException.ThrowIfNull(requestType);
            ArgumentNullException.ThrowIfNull(responseType);

            var result = new ResolvedActionHandler();

            result.RequestType = requestType;
            result.ResponseType = responseType;

            if (requestType.IsAssignableTo(typeof(IActionRequest)))
            {
                result.HandlerType = typeof(IActionHandler<>).MakeGenericType(result.RequestType);
            }
            else
            {
                result.HandlerType = typeof(IActionHandler<,>).MakeGenericType(result.RequestType, result.ResponseType);
            }

            result.HandlerInstance = _serviceProvider.GetRequiredService(result.HandlerType);

            result.HandleAsyncMethod = result.HandlerType.GetMethod("HandleAsync")!;

            if (result.HandleAsyncMethod == null)
            {
                throw new InvalidOperationException($"HandleAsync not found on {result.HandlerType.Name}");
            }

            return result;
        }

        /// <summary>
        /// Creates a delegate that invokes the handler method with the specified request.
        /// </summary>
        private ActionExecutionDelegate<TResponse> CreateActionExecutionDelegate<TResponse>(ResolvedActionHandler resolvedActionHandler, IActionRequestBase request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(resolvedActionHandler);

            ActionExecutionDelegate<TResponse> nextDelegate = () =>
            {
                if (resolvedActionHandler.HandleAsyncMethod.ReturnType == typeof(Task))
                {
                    var task = (Task)resolvedActionHandler.HandleAsyncMethod.Invoke(resolvedActionHandler.HandlerInstance, new object[] { request, cancellationToken })!;
                    return Task.FromResult(default(TResponse)!);
                }
                else
                {
                    var task = (Task<TResponse>)resolvedActionHandler.HandleAsyncMethod.Invoke(resolvedActionHandler.HandlerInstance, new object[] { request, cancellationToken })!;
                    return task;
                }
            };

            return nextDelegate;
        }

        /// <summary>
        /// Creates an execution chain by wrapping the handler delegate with applicable interceptors.
        /// </summary>
        private ActionExecutionDelegate<TResponse> CreateExecutionChain<TResponse>(ActionExecutionContext context, ActionExecutionDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var responseType = typeof(TResponse);
            var interceptors = _actionExecutionChain.GetInterceptorsFor(context.RequestType, responseType);
            var interceptorInstances = interceptors
                .Select(t => (IActionExecutionInterceptor)_serviceProvider.GetRequiredService(t.MakeGenericType(context.RequestType, typeof(TResponse))))
                .ToArray();

            context.Interceptors = interceptorInstances;

            foreach (var interceptorInstance in interceptorInstances.Reverse())
            {
                var handleAsyncMethod = interceptorInstance.GetType().GetMethod("HandleAsync");

                if (handleAsyncMethod == null)
                    throw new InvalidOperationException($"HandleAsync not found on {interceptorInstance.GetType().Name}");

                var previousNext = next;

                next = () =>
                {
                    var task = (Task<TResponse>)handleAsyncMethod.Invoke(interceptorInstance, new object[] { context, previousNext, cancellationToken })!;
                    return task;
                };
            }

            return next;
        }
    }
}
