namespace FortyOne.OrchestratR
{
    internal class InterceptorFactory
    {
        private readonly List<Type> _interceptorTypes = new();

        public void AddInterceptorType(Type interceptorType)
        {
            ArgumentNullException.ThrowIfNull(interceptorType);

            _interceptorTypes.Add(interceptorType);
        }
    }
}
