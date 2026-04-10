/*
 * ============================================================
 * TOPIC     : Modern C#
 * SUBTOPIC  : Indices and Ranges
 * FILE      : 03_IndicesDemo.cs
 * PURPOSE   : Demonstrates indices and ranges in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._19_ModernCSharp._03_Indices
{
    public class IndicesDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Indices and Ranges Demo ===\n");
            Console.WriteLine("1. Range Operator:");
            var arr = new[] { 0, 1, 2, 3, 4, 5 };
            var slice = arr[1..4];
            Console.WriteLine($"   Slice: 1..4 = 1, 2, 3");
            Console.WriteLine("\n=== Indices Complete ===");
        }
    }
}