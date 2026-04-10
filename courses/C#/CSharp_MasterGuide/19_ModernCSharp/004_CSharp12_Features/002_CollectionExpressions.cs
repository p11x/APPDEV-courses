/*
 * ============================================================
 * TOPIC     : Modern C#
 * SUBTOPIC  : Collection Expressions
 * FILE      : CollectionExpressions.cs
 * PURPOSE   : Using collection expressions in C# 12
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._19_ModernCSharp._04_CSharp12_Features
{
    /// <summary>
    /// Collection expressions demonstration
    /// </summary>
    public class CollectionExpressionsDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Collection Expressions ===\n");

            // Output: --- Array Expression ---
            Console.WriteLine("--- Array Expression ---");

            int[] numbers = [1, 2, 3, 4, 5];
            Console.WriteLine($"   Count: {numbers.Length}");
            // Output: Count: 5

            // Output: --- Spread Element ---
            Console.WriteLine("\n--- Spread Element ---");

            var additional = [6, 7, .. numbers];
            Console.WriteLine($"   Additional: {additional.Length}");
            // Output: Additional: 7

            // Output: --- List Expression ---
            Console.WriteLine("\n--- List Expression ---");

            var list = new System.Collections.Generic.List<int> { 1, 2, 3 };
            Console.WriteLine($"   List: {list.Count}");
            // Output: List: 3

            // Output: --- Span ---
            Console.WriteLine("\n--- Span ---");

            Span<int> span = [1, 2, 3];
            Console.WriteLine($"   Span: {span.Length}");
            // Output: Span: 3

            Console.WriteLine("\n=== Collection Complete ===");
        }
    }
}