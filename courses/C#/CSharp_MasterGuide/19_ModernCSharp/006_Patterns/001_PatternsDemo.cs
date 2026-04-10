/*
 * ============================================================
 * TOPIC     : Modern C#
 * SUBTOPIC  : Patterns
 * FILE      : 02_PatternsDemo.cs
 * PURPOSE   : Demonstrates modern C# patterns
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._19_ModernCSharp._02_Patterns
{
    public class PatternsDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Patterns Demo ===\n");
            Console.WriteLine("1. Pattern Matching:");
            object obj = "Hello";
            if (obj is string s) Console.WriteLine($"   String: {s}");
            Console.WriteLine("\n=== Patterns Complete ===");
        }
    }
}