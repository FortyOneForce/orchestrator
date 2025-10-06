using BenchmarkDotNet.Running;

namespace Benchmarks
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<MediatorBenchmarks>(args: args);
        }
    }
}
