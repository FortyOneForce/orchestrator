using System.Collections.Concurrent;
using System.Reflection;

namespace FortyOne.OrchestratR
{
    internal class InterceptorRegistry
    {
        private readonly List<Type> _interceptorTypes = new();
        private readonly ConcurrentDictionary<(Type RequestType, Type ResponseType), Type[]> _interceptorTypeCache = new();

        public void AddInterceptorType(Type interceptorType)
        {
            ArgumentNullException.ThrowIfNull(interceptorType);

            _interceptorTypes.Add(interceptorType);
        }

        public Type[] GetReversedInterceptors<TRequest, TResponse>()
        {
            if (_interceptorTypes.Count == 0)
            {
                return Array.Empty<Type>();
            }

            var types = _interceptorTypeCache.GetOrAdd((typeof(TRequest), typeof(TResponse)), (key) =>
            {
                var result = new List<Type>();

                foreach (var interceptorType in _interceptorTypes)
                {
                    var genericArgs = interceptorType.GetGenericArguments(); 
                    var requestGeneric = genericArgs[0];
                    var responseGeneric = genericArgs[1];

                    if (SatisfiesConstraints(requestGeneric, key.RequestType) &&
                        SatisfiesConstraints(responseGeneric, key.ResponseType))
                    {
                        result.Add(interceptorType.MakeGenericType(key.RequestType, key.ResponseType));
                    }
                }

                return result.ToArray()
                    .Reverse()
                    .ToArray();
            });

            return types;
        }

        private bool SatisfiesConstraints(Type genericParam, Type concreteType)
        {
            var constraints = genericParam.GetGenericParameterConstraints();

            foreach (var constraint in constraints)
            {
                if (!constraint.IsAssignableFrom(concreteType))
                    return false;
            }

            var gpAttributes = genericParam.GenericParameterAttributes;

            if (gpAttributes.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint) && concreteType.IsValueType)
                return false;

            if (gpAttributes.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint) && !concreteType.IsValueType)
                return false;

            if (gpAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
            {
                if (concreteType.IsAbstract || concreteType.GetConstructor(Type.EmptyTypes) == null)
                    return false;
            }

            return true;
        }

    }
}
