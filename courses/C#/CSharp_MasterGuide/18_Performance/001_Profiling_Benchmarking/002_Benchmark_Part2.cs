/*
 * ============================================================
 * TOPIC     : Performance
 * SUBTOPIC  : Benchmarking Part 2
 * FILE      : Benchmark_Part2.cs
 * PURPOSE   : Advanced benchmarking techniques
 * ============================================================
 */
using System; // Core System namespace
using System.Diagnostics; // Diagnostics namespace

namespace CSharp_MasterGuide._18_Performance._01_Profiling_Benchmarking
{
    /// <summary>
    /// Advanced benchmarking
    /// </summary>
    public class BenchmarkPart2Demo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Benchmark Part 2 ===\n");

            // Output: --- Benchmark Runner ---
            Console.WriteLine("--- Benchmark Runner ---");

            var results = RunBenchmark();
            Console.WriteLine($"   Method: {results.Method}");
            Console.WriteLine($"   Mean: {results.Mean} ns");
            Console.WriteLine($"   Error: {results.Error} ns");
            // Output: Method: Method1
            // Output: Mean: 100.5 ns
            // Output: Error: 0.5 ns

            // Output: --- Memory Diagnostics ---
            Console.WriteLine("\n--- Memory Diagnostics ---");

            var memory = MeasureMemory();
            Console.WriteLine($"   Allocated: {memory.AllocatedBytes} B");
            Console.WriteLine($"   GC0: {memory.GC0}");
            // Output: Allocated: 1000 B
            // Output: GC0: 0

            // Output: --- Iteration Tuning ---
            Console.WriteLine("\n--- Iteration Tuning ---");

            var config = new BenchmarkConfig();
            Console.WriteLine($"   Warmup: {config.WarmupCount} iterations");
            Console.WriteLine($"   Iteration: {config.IterationCount} iterations");
            // Output: Warmup: 3 iterations
            // Output: Iteration: 100 iterations

            Console.WriteLine("\n=== Part 2 Complete ===");
        }
    }

    /// <summary>
    /// Benchmark results
    /// </summary>
    public class BenchmarkResult
    {
        public string Method { get; set; } = "Method1"; // property: method name
        public double Mean { get; set; } = 100.5; // property: mean time in ns
        public double Error { get; set; } = 0.5; // property: error margin
    }

    /// <summary>
    /// Run benchmark
    /// </summary>
    public static BenchmarkResult RunBenchmark()
    {
        return new BenchmarkResult();
    }

    /// <summary>
    /// Memory measurement
    /// </summary>
    public class MemoryResult
    {
        public long AllocatedBytes { get; set; } = 1000; // property: bytes
        public int GC0 { get; set; } = 0; // property: GC count
    }

    /// <summary>
    /// Measure memory
    /// </summary>
    public static MemoryResult MeasureMemory()
    {
        return new MemoryResult();
    }

    /// <summary>
    /// Benchmark configuration
    /// </summary>
    public class BenchmarkConfig
    {
        public int WarmupCount { get; set; } = 3; // property: warmup
        public int IterationCount { get; set; } = 100; // property: iterations
    }
}