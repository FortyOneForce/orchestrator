using System.Reflection;

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

    public static bool TryGetEventHandlerInterfaces(this Type type, out Type[] eventHandlerInterfaces)
    {
        ArgumentNullException.ThrowIfNull(type);

        eventHandlerInterfaces = type.GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
            .ToArray();

        return eventHandlerInterfaces.Length != 0;
    }

    public static bool TryGetActionHandlerInterfaces(this Type type, out Type[] actionHandlerInterfaces)
    {
        ArgumentNullException.ThrowIfNull(type);

        actionHandlerInterfaces = type.GetInterfaces()
            .Where(i => i.IsGenericType)
            .Where(i => i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) || i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
            .ToArray();

        return actionHandlerInterfaces.Length != 0;
    }
        
}
