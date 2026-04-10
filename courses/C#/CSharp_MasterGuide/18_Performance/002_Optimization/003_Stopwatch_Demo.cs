/*
 * ============================================================
 * TOPIC     : Performance
 * SUBTOPIC  : Stopwatch Timing
 * FILE      : Stopwatch_Demo.cs
 * PURPOSE   : Using Stopwatch for precise timing
 * ============================================================
 */
using System; // Core System namespace
using System.Diagnostics; // Diagnostics namespace

namespace CSharp_MasterGuide._18_Performance._02_Optimization
{
    /// <summary>
    /// Stopwatch demonstration
    /// </summary>
    public class StopwatchDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Stopwatch Demo ===\n");

            // Output: --- Basic Timing ---
            Console.WriteLine("--- Basic Timing ---");

            var sw = Stopwatch.StartNew();
            DoWork();
            sw.Stop();
            Console.WriteLine($"   Elapsed: {sw.ElapsedMilliseconds} ms");
            // Output: Elapsed: 100 ms

            // Output: --- High Precision ---
            Console.WriteLine("\n--- High Precision ---");

            var hr = Stopwatch.StartNew();
            FastOperation();
            hr.Stop();
            Console.WriteLine($"   Ticks: {hr.ElapsedTicks}");
            Console.WriteLine($"   High resolution: {Stopwatch.IsHighResolution}");
            // Output: Ticks: 1000
            // Output: High resolution: True

            // Output: --- Timing Sections ---
            Console.WriteLine("\n--- Timing Sections ---");

            var multi = new MultiSectionTimer();
            multi.Start();
            multi.Mark("Phase1");
            multi.Mark("Phase2");
            var total = multi.Elapsed;
            Console.WriteLine($"   Total: {total}");
            // Output: Total: 50 ms

            // Output: --- GC Timing ---
            Console.WriteLine("\n--- GC Timing ---");

            var beforeGC = GC.CollectionCount(0);
            DoWorkWithAllocations();
            var afterGC = GC.CollectionCount(0);
            Console.WriteLine($"   GC runs: {afterGC - beforeGC}");
            // Output: GC runs: 1

            Console.WriteLine("\n=== Stopwatch Complete ===");
        }
    }

    /// <summary>
    /// Do work operation
    /// </summary>
    public static void DoWork()
    {
        System.Threading.Thread.Sleep(100);
    }

    /// <summary>
    /// Fast operation
    /// </summary>
    public static void FastOperation()
    {
    }

    /// <summary>
    /// Multi-section timer
    /// </summary>
    public class MultiSectionTimer
    {
        private readonly Stopwatch _timer = new(); // stopwatch: timer

        public void Start() => _timer.Start(); // method: start
        public void Mark(string phase) => Console.WriteLine($"   {phase}: {_timer.ElapsedMilliseconds} ms"); // method: mark
        public long Elapsed => _timer.ElapsedMilliseconds; // property: elapsed
    }

    /// <summary>
    /// Do work with allocations
    /// </summary>
    public static void DoWorkWithAllocations()
    {
        var list = new System.Collections.Generic.List<int> { 1, 2, 3 };
    }
}