/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : Unit Testing Basics
 * FILE      : 01_UnitTesting.cs
 * PURPOSE   : Demonstrates unit testing fundamentals in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._14_Testing._01_UnitTesting
{
    /// <summary>
    /// Demonstrates unit testing basics
    /// </summary>
    public class UnitTestingDemo
    {
        /// <summary>
        /// Entry point for unit testing examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Unit Testing Basics ===
            Console.WriteLine("=== Unit Testing Basics ===\n");

            // ── CONCEPT: What is Unit Testing? ───────────────────────────────
            // Testing individual units in isolation

            // Example 1: Testing a Calculator
            // Output: 1. Testing a Calculator:
            Console.WriteLine("1. Testing a Calculator:");
            
            var calculator = new Calculator();
            
            // Test addition
            var addResult = calculator.Add(2, 3);
            // Assert: Expected 5
            Console.WriteLine($"   Add(2, 3) = {addResult} (Expected: 5)");
            
            // Test subtraction
            var subResult = calculator.Subtract(10, 4);
            // Assert: Expected 6
            Console.WriteLine($"   Subtract(10, 4) = {subResult} (Expected: 6)");
            
            // Test multiplication
            var mulResult = calculator.Multiply(3, 4);
            // Assert: Expected 12
            Console.WriteLine($"   Multiply(3, 4) = {mulResult} (Expected: 12)");

            // ── CONCEPT: Arrange-Act-Assert ───────────────────────────────────
            // Standard structure for tests

            // Example 2: AAA Pattern
            // Output: 2. Arrange-Act-Assert:
            Console.WriteLine("\n2. Arrange-Act-Assert:");
            
            // Arrange
            var stringHelper = new StringHelper();
            var input = "hello world";
            
            // Act
            var result = stringHelper.ToTitleCase(input);
            
            // Assert
            // Output: Result: Hello World (Expected: Hello World)
            Console.WriteLine($"   Result: {result} (Expected: Hello World)");

            // ── CONCEPT: Testing Edge Cases ───────────────────────────────────
            // Handle boundary conditions

            // Example 3: Edge Cases
            // Output: 3. Edge Cases:
            Console.WriteLine("\n3. Edge Cases:");
            
            var validator = new EmailValidator();
            
            // Valid email
            var valid1 = validator.IsValid("test@email.com");
            Console.WriteLine($"   test@email.com is valid: {valid1} (Expected: True)");
            
            // Invalid - no @ symbol
            var valid2 = validator.IsValid("testemail.com");
            Console.WriteLine($"   testemail.com is valid: {valid2} (Expected: False)");
            
            // Empty string
            var valid3 = validator.IsValid("");
            Console.WriteLine($"   Empty string is valid: {valid3} (Expected: False)");

            Console.WriteLine("\n=== Unit Testing Complete ===");
        }
    }

    /// <summary>
    /// Calculator - simple class to test
    /// </summary>
    public class Calculator
    {
        /// <summary>
        /// Adds two numbers
        /// </summary>
        public int Add(int a, int b) => a + b;
        
        /// <summary>
        /// Subtracts two numbers
        /// </summary>
        public int Subtract(int a, int b) => a - b;
        
        /// <summary>
        /// Multiplies two numbers
        /// </summary>
        public int Multiply(int a, int b) => a * b;
        
        /// <summary>
        /// Divides two numbers
        /// </summary>
        public int Divide(int a, int b)
        {
            if (b == 0) throw new DivideByZeroException();
            return a / b;
        }
    }

    /// <summary>
    /// String helper class
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// Converts string to title case
        /// </summary>
        public string ToTitleCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            
            var words = input.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
            }
            return string.Join(" ", words);
        }
    }

    /// <summary>
    /// Email validator
    /// </summary>
    public class EmailValidator
    {
        /// <summary>
        /// Validates email format
        /// </summary>
        public bool IsValid(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            return email.Contains("@") && email.Contains(".");
        }
    }
}