/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : Mocking - Moq Part 2
 * FILE      : 02_Moq_Basics_Part2.cs
 * PURPOSE   : Advanced Moq features
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._14_Testing._02_Mocking
{
    /// <summary>
    /// Advanced Moq
    /// </summary>
    public class MoqBasicsPart2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Moq Part 2 ===\n");

            Console.WriteLine("1. Strict vs Loose:");
            Console.WriteLine("   Default: Loose (non-setups return default)");
            
            Console.WriteLine("\n2. Callbacks:");
            Console.WriteLine("   .Callback(() => { })");
            
            Console.WriteLine("\n3. Returns Async:");
            Console.WriteLine("   .ReturnsAsync()");

            Console.WriteLine("\n=== Moq Part 2 Complete ===");
        }
    }
}