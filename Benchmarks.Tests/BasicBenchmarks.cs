using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Xunit;

namespace Benchmarks.Tests
{
    [MemoryDiagnoser]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "Benchmark testing only - not for security")]
    public class BasicBenchmarks
    {
        private readonly Random _random = new Random(42); // Fixed seed for consistent results

        [Benchmark]
        public void EmptyBenchmark()
        {
            // TODO: Add actual benchmarks for existing functionality
            // This is a placeholder to ensure BenchmarkDotNet works
            _ = _random.Next(); // Access instance data to avoid CA1822
        }

        [Benchmark]
        public void StringAllocationBenchmark()
        {
            // Simple benchmark to test basic functionality
            var length = _random.Next(50, 150); // Use instance data
            var testString = new string('x', length);
            _ = testString.Length;
        }

        [Benchmark]
        public void ArrayAllocationBenchmark()
        {
            // Test array allocation performance
            var size = _random.Next(512, 2048); // Use instance data
            var testArray = new byte[size];
            for (int i = 0; i < testArray.Length; i++)
            {
                testArray[i] = (byte)(i + _random.Next(0, 10)); // Use instance data
            }
        }
    }

    public class BenchmarkTests
    {
        [Fact]
        [Trait("Category", "Benchmark")]
        public void RunBenchmarks()
        {
            // Bridge test that actually runs the benchmarks
            // This makes the benchmarks discoverable by dotnet test
            var summary = BenchmarkRunner.Run<BasicBenchmarks>();

            // Basic validation that benchmarks completed
            Assert.NotNull(summary);
            Assert.True(summary.Reports.Length > 0, "No benchmark reports generated");
        }
    }
}
