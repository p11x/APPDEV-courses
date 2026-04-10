/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : Unit Testing - MSTest
 * FILE      : 03_MSTest.cs
 * PURPOSE   : MSTest framework basics
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._14_Testing._01_UnitTesting
{
    /// <summary>
    /// MSTest framework
    /// </summary>
    public class MSTestDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== MSTest ===\n");

            Console.WriteLine("1. Test Class:");
            Console.WriteLine("   [TestClass] attribute");
            
            Console.WriteLine("\n2. Test Method:");
            Console.WriteLine("   [TestMethod] attribute");
            
            Console.WriteLine("\n3. Assertions:");
            Console.WriteLine("   Microsoft.VisualStudio.TestTools.UnitTesting");

            Console.WriteLine("\n=== MSTest Complete ===");
        }
    }
}