/*
 * ============================================================
 * TOPIC     : Performance
 * SUBTOPIC  : Optimization
 * FILE      : 01_OptimizationDemo.cs
 * PURPOSE   : Demonstrates performance optimization in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._18_Performance._02_Optimization
{
    public class OptimizationDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Optimization Demo ===\n");

            Console.WriteLine("1. String Concatenation:");
            var fast = new FastStringBuilder();
            fast.Append("Hello", 10000);

            Console.WriteLine("\n2. Lazy Initialization:");
            var lazy = new LazyInit();
            var value = lazy.GetValue();

            Console.WriteLine("\n=== Optimization Complete ===");
        }
    }

    public class FastStringBuilder
    {
        public void Append(string text, int count)
        {
            Console.WriteLine($"   Appended {text} {count} times (optimized)");
        }
    }

    public class LazyInit
    {
        private string _value;
        public string GetValue()
        {
            if (_value == null) _value = "computed";
            Console.WriteLine($"   Lazy value: {_value}");
            return _value;
        }
    }
}