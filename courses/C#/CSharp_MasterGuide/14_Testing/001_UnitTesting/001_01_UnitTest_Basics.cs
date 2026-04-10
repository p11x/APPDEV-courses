/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : Unit Testing Basics
 * FILE      : 01_UnitTest_Basics.cs
 * PURPOSE   : Unit testing fundamentals in C#
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._14_Testing._01_UnitTesting
{
    /// <summary>
    /// Unit testing basics
    /// </summary>
    public class UnitTestBasics
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Unit Test Basics ===\n");

            // Test calculator
            var calc = new Calculator();
            
            // Test addition
            var result = calc.Add(2, 3);
            // Output: 2 + 3 = 5
            Console.WriteLine($"   2 + 3 = {result}");
            
            // Test division
            result = calc.Divide(10, 2);
            // Output: 10 / 2 = 5
            Console.WriteLine($"   10 / 2 = {result}");

            Console.WriteLine("\n=== Unit Test Basics Complete ===");
        }
    }

    /// <summary>
    /// Calculator class to test
    /// </summary>
    public class Calculator
    {
        public int Add(int a, int b) => a + b;
        public int Subtract(int a, int b) => a - b;
        public int Multiply(int a, int b) => a * b;
        public int Divide(int a, int b)
        {
            if (b == 0) throw new DivideByZeroException();
            return a / b;
        }
    }
}