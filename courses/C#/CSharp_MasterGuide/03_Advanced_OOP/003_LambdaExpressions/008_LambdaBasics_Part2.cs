/*
 * TOPIC: CSharp_MasterGuide/03_Advanced_OOP/03_LambdaExpressions
 * SUBTOPIC: Lambda Expression Basics - Part 2
 * FILE: LambdaBasics_Part2.cs
 * PURPOSE: Variable capture, type inference, implicit vs explicit typing in lambdas
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._03_LambdaExpressions
{
    public class LambdaBasicsPart2
    {
        public static void RunMain(string[] args)
        {
            Console.WriteLine("=== Lambda Basics Part 2: Capturing Variables, Type Inference ===\n");

            // ============================================
            // VARIABLE CAPTURE IN LAMBDA EXPRESSIONS
            // ============================================

            // Example 1: Capturing local variables (closure)
            // Lambda can access and "capture" local variables from outer scope
            int multiplier = 10;
            Func<int, int> multiplyByTen = x => x * multiplier;
            Console.WriteLine($"5 * 10 = {multiplyByTen(5)}"); // Output: 50

            // The captured variable's value is evaluated at execution time, not definition time
            multiplier = 20;
            Console.WriteLine($"5 * 20 (after change) = {multiplyByTen(5)}"); // Output: 100

            // Example 2: Capturing multiple variables
            int add = 5;
            int multiply = 3;
            Func<int, int> calculate = x => (x + add) * multiply;
            Console.WriteLine($"(10 + 5) * 3 = {calculate(10)}"); // Output: 45

            // Example 3: Capturing loop variable (important: beware of closure in loops!)
            // This was a classic bug in older C# versions
            var actions = new List<Action>();
            for (int i = 0; i < 3; i++)
            {
                // In C# 5+, the loop variable is captured, not its value
                // Each action captures the same variable 'i'
                actions.Add(() => Console.WriteLine(i));
            }
            Console.WriteLine("Loop variable capture (C# 5+):");
            foreach (var action in actions)
            {
                action(); // Output: 0, 1, 2 (correct in modern C#)
            }

            // Example 4: Using local variable to capture correct loop value
            var correctActions = new List<Action>();
            for (int i = 0; i < 3; i++)
            {
                // Create a local copy for each iteration
                int copy = i;
                correctActions.Add(() => Console.WriteLine(copy));
            }
            Console.WriteLine("Correct loop variable capture:");
            foreach (var action in correctActions)
            {
                action(); // Output: 0, 1, 2
            }

            // ============================================
            // TYPE INFERENCE IN LAMBDA EXPRESSIONS
            // ============================================

            // Example 5: Implicit type inference from delegate signature
            // Compiler infers parameter types from Func<T, T> delegate
            Func<int, string> intToString = x => x.ToString(); // x is inferred as int
            Console.WriteLine($"Implicit: {intToString(42)}"); // Output: 42

            // Example 6: Lambda with string parameters
            Func<string, string, string> concat = (s1, s2) => s1 + s2;
            Console.WriteLine($"Concatenation: {concat("Hello", " World")}"); // Output: Hello World

            // Example 7: Mixed inference - explicitly typed and inferred
            // When one parameter is explicitly typed, others can be inferred
            Func<string, int, string> repeatString = (string s, int n) => string.Concat(Enumerable.Repeat(s, n));
            Console.WriteLine($"Repeat 'Hi' 3 times: {repeatString("Hi", 3)}"); // Output: HiHiHi

            // Example 8: Return type inference
            // Compiler infers return type from the expression
            Func<bool> getDefaultBool = () => default(bool);
            Console.WriteLine($"Default bool: {getDefaultBool()}"); // Output: False

            // ============================================
            // IMPLICIT VS EXPLICIT TYPING
            // ============================================

            // Example 9: Explicit parameter types (required in some contexts)
            // When the delegate type doesn't provide enough context
            DelegateWithExplicitParams explicitDel = (int a, int b) => a + b;
            Console.WriteLine($"Explicit types: {explicitDel(15, 25)}"); // Output: 40

            // Example 10: Implicit parameter types (most common)
            // Compiler figures out types from Func<> signature
            Func<int, int, int> implicitDel = (a, b) => a - b;
            Console.WriteLine($"Implicit types: {implicitDel(50, 30)}"); // Output: 20

            // Example 11: When to use explicit types
            // When working with object type or dynamic contexts
            Func<object, string> objectToString = (object obj) => obj?.ToString() ?? "null";
            Console.WriteLine($"Object conversion: {objectToString(123)}"); // Output: 123

            // Example 12: var keyword with parentheses for lambda
            // Need explicit cast when using var
            var lambdaWithParens = (Func<int, int>)(x => x + 1);
            Console.WriteLine($"var with cast: {lambdaWithParens(9)}"); // Output: 10

            // ============================================
            // CLOSURE REAL-WORLD SCENARIO
            // ============================================

            // Example 13: Closure for configuration
            Console.WriteLine("\n=== Closure for Configuration ===");
            var createAdder = CreateAdderFactory(100);
            var add100 = createAdder();
            Console.WriteLine($"Add 100 to 50: {add100(50)}"); // Output: 150

            var createMultiplier = CreateMultiplierFactory(3);
            var multiplyBy3 = createMultiplier();
            Console.WriteLine($"Multiply 10 by 3: {multiplyBy3(10)}"); // Output: 30
        }

        // Delegate for example 9
        public delegate int DelegateWithExplicitParams(int a, int b);

        // Factory method demonstrating closure
        public static Func<int, int> CreateAdderFactory(int addend)
        {
            // Returns a lambda that captures 'addend'
            return n => n + addend;
        }

        // Factory method demonstrating closure with multiplier
        public static Func<int, int> CreateMultiplierFactory(int multiplier)
        {
            return n => n * multiplier;
        }
    }
}

// Additional utility class to demonstrate more capture scenarios
namespace CSharp_MasterGuide._03_Advanced_OOP._03_LambdaExpressions.Part2Utilities
{
    public class Calculator
    {
        private int _baseValue;

        public Calculator(int baseValue)
        {
            _baseValue = baseValue;
        }

        // Method that returns a lambda capturing instance field
        public Func<int, int> CreateAdder()
        {
            return x => x + _baseValue;
        }

        // Method that returns a lambda capturing multiple fields
        public Func<int, int, int> CreateOperation()
        {
            return (a, b) => a + b + _baseValue;
        }

        public static void Demonstrate()
        {
            var calc = new Calculator(50);
            var add50 = calc.CreateAdder();
            Console.WriteLine($"Instance closure: 100 + 50 = {add50(100)}"); // Output: 150
        }
    }
}