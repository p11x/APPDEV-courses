/*
 * ============================================================
 * TOPIC     : Modern C#
 * SUBTOPIC  : Top Level Statements
 * FILE      : TopLevelStatements.cs
 * PURPOSE   : Top-level statements in C# 9+
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._19_ModernCSharp._04_CSharp12_Features
{
    /// <summary>
    /// Top level statements demonstration
    /// </summary>
    public class TopLevelStatementsDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Top Level Statements ===\n");

            // In C# 9+, you can write top-level statements
            // instead of full class/namespace structure

            // Output: --- Simple Program ---
            Console.WriteLine("--- Simple Program ---");

            Console.WriteLine("   Hello, World!");
            // Output: Hello, World!

            // Output: --- Using Types ---
            Console.WriteLine("\n--- Using Types ---");

            var list = new System.Collections.Generic.List<int> { 1, 2, 3 };
            Console.WriteLine($"   Count: {list.Count}");
            // Output: Count: 3

            // Output: --- Async Support ---
            Console.WriteLine("\n--- Async Support ---");

            Console.WriteLine("   Async works too");
            // Output: Async works too

            Console.WriteLine("\n=== Top Level Complete ===");
        }
    }
}