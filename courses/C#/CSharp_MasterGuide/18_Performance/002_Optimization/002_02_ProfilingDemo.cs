/*
 * ============================================================
 * TOPIC     : Performance
 * SUBTOPIC  : Profiling
 * FILE      : 02_ProfilingDemo.cs
 * PURPOSE   : Demonstrates profiling in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._18_Performance._02_Optimization
{
    public class ProfilingDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Profiling Demo ===\n");
            Console.WriteLine("1. Stopwatch Timing:");
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var result = 0;
            for (int i = 0; i < 1000000; i++) result += i;
            sw.Stop();
            Console.WriteLine($"   Elapsed: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine("\n=== Profiling Complete ===");
        }
    }
}