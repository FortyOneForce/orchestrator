using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    [WarmupCount(5)]
    [IterationCount(10)]
    [MemoryDiagnoser]
    public class ExtensionMethodBenchmark
    {


        [Benchmark(Baseline = true)]
        public void Direct() => new Sample().DoSomething();

        [Benchmark]
        public async Task Extension() => new Sample().DoSomethingExtension();

        public class Sample
        {
            public void DoSomething()
            {
            }
        }
    }

    public static class SampleExtensions
    {
        public static void DoSomethingExtension(this ExtensionMethodBenchmark.Sample sample)
        {
            sample.DoSomething();
        }
    }
}
