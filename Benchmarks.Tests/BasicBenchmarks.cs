using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Benchmarks.Tests
{
    [MemoryDiagnoser]
    public static class BasicBenchmarks
    {
        [Benchmark]
        public static void EmptyBenchmark()
        {
            // TODO: Add actual benchmarks for existing functionality
            // This is a placeholder to ensure BenchmarkDotNet works
        }

        [Benchmark]
        public static void StringAllocationBenchmark()
        {
            // Simple benchmark to test basic functionality
            var testString = new string('x', 100);
            _ = testString.Length;
        }

        [Benchmark]
        public static void ArrayAllocationBenchmark()
        {
            // Test array allocation performance
            var testArray = new byte[1024];
            for (int i = 0; i < testArray.Length; i++)
            {
                testArray[i] = (byte)i;
            }
        }
    }
}
