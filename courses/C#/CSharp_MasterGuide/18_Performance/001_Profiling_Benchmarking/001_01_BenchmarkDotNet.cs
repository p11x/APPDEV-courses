/*
 * ============================================================
 * TOPIC     : Performance
 * SUBTOPIC  : Profiling/Benchmarking
 * FILE      : 01_BenchmarkDotNet.cs
 * PURPOSE   : BenchmarkDotNet for performance testing
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._18_Performance._01_Profiling_Benchmarking
{
    /// <summary>
    /// BenchmarkDotNet basics
    /// </summary>
    public class BenchmarkDotNetDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== BenchmarkDotNet ===\n");

            Console.WriteLine("1. Install BenchmarkDotNet:");
            Console.WriteLine("   dotnet add package BenchmarkDotNet");
            
            Console.WriteLine("\n2. Create Benchmark Class:");
            Console.WriteLine("   [Benchmark] attribute on methods");
            
            Console.WriteLine("\n3. Run Benchmarks:");
            Console.WriteLine("   Method |       Mean |    Error |   StdDev");
            Console.WriteLine("   StringConcat | 1.2 us |  0.02 us |  0.01 us");
            Console.WriteLine("   StringBuilder | 0.8 us |  0.01 us |  0.01 us");

            Console.WriteLine("\n=== BenchmarkDotNet Complete ===");
        }
    }
}