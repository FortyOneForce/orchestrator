namespace FortyOne.OrchestratR.Extensions;

internal static class TypeExtensions
{
    public static bool IsConcreteAssignableTo<T>(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return !type.IsGenericType && !type.IsAbstract && type.IsClass && type.IsAssignableTo(typeof(T));
    }

    public static bool IsRequestInterceptor(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return type.IsGenericType && !type.IsAbstract && type.IsClass && type.GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestInterceptor<,>));
    }

    public static bool TryGetNotificationHandlerInterfaces(this Type type, out Type[] notificationHandlerInterfaces)
    {
        ArgumentNullException.ThrowIfNull(type);

        notificationHandlerInterfaces = type.GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
            .ToArray();

        return notificationHandlerInterfaces.Length != 0;
    }

    public static bool TryGetRequstHandlerInterfaces(this Type type, out Type[] requestHandlerInterfaces)
    {
        ArgumentNullException.ThrowIfNull(type);

        requestHandlerInterfaces = type.GetInterfaces()
            .Where(i => i.IsGenericType)
            .Where(i => i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) || i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
            .ToArray();

        return requestHandlerInterfaces.Length != 0;
    }
        
}
