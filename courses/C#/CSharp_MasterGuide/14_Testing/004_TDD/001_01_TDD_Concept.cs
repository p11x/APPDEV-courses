/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : TDD - Concept
 * FILE      : 01_TDD_Concept.cs
 * PURPOSE   : Test-Driven Development concept
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._14_Testing._04_TDD
{
    /// <summary>
    /// TDD concept
    /// </summary>
    public class TDDConcept
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== TDD Concept ===\n");

            Console.WriteLine("1. Red:");
            Console.WriteLine("   Write failing test first");
            
            Console.WriteLine("\n2. Green:");
            Console.WriteLine("   Make test pass with minimal code");
            
            Console.WriteLine("\n3. Refactor:");
            Console.WriteLine("   Improve code while keeping tests passing");

            Console.WriteLine("\n=== TDD Concept Complete ===");
        }
    }
}