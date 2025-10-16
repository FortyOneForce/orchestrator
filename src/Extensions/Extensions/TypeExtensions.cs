using System.Collections.Concurrent;

namespace FortyOne.OrchestratR.Extensions.Extensions;

/// <summary>
/// Extensions for <see cref="Type"/>.
/// </summary>
public static class TypeExtensions
{
    private readonly static Type _resultType = typeof(Result);
    private readonly static Type _genericResultTypeDefinition = typeof(Result<>);
    private readonly static ConcurrentDictionary<Type, bool> _genericTypeDefinitionCache = new();

    /// <summary>
    /// Returns true if the type is <see cref="Result"/>, false otherwise.
    /// </summary>
    public static bool IsResultType(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return type == _resultType;
    }

    /// <summary>
    /// Returns true if the type is a generic version of <see cref="Result{T}"/>, false otherwise.
    /// </summary>
    public static bool IsGenericResultType(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (!type.IsGenericType)
            return false;

        return _genericTypeDefinitionCache
            .GetOrAdd(type, (key) => key.GetGenericTypeDefinition() == _genericResultTypeDefinition);
    }
}
