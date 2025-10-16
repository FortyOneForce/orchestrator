using BenchmarkDotNet.Running;

namespace Benchmarks
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var types = new Type[]
            {
                typeof(RequestBenchmark),
                typeof(RequestInterceptorBenchmark),
                typeof(RequestResponseBenchmark),
                typeof(RequestResponseInterceptorBenchmark),
                typeof(NotificationBenchmark),
                typeof(NotificationHandlerBenchmark),
                typeof(ExtensionMethodBenchmark)
            };

            new BenchmarkSwitcher(types).Run(args: args);
        }
    }
}
