/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : Unit Testing - xUnit
 * FILE      : 05_xUnit.cs
 * PURPOSE   : xUnit framework basics
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._14_Testing._01_UnitTesting
{
    /// <summary>
    /// xUnit framework
    /// </summary>
    public class xUnitDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== xUnit ===\n");
            Console.WriteLine("1. [Fact] attribute for tests");
            Console.WriteLine("2. [Theory] for parameterized tests");
            Console.WriteLine("3. Assert class for assertions");
            Console.WriteLine("\n=== xUnit Complete ===");
        }
    }
}