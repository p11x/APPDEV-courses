/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Delegates and Events - Delegate Basics Part 2
 * FILE      : DelegateBasics_Part2.cs
 * PURPOSE   : Advanced delegate concepts including delegates as
 *            parameters, delegates as return types, and built-in
 *            delegate types (Func, Action, Predicate)
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._02_Delegates_Events
{
    class DelegateBasics_Part2
    {
        // Custom delegate for demonstration
        public delegate int TransformDelegate(int value);
        public delegate bool ValidationDelegate(string input);
        public delegate void ProgressDelegate(int percent);

        static void Main(string[] args)
        {
            Console.WriteLine("=== Delegate Basics Part 2 ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Delegate as Parameter
            // ═══════════════════════════════════════════════════════════

            // Delegates can be passed as parameters to methods
            // This enables callback patterns and strategy pattern

            // ── EXAMPLE 1: Simple Callback Delegate ─────────────────────
            Console.WriteLine("--- Delegate as Parameter ---");
            ProcessNumbers(5, Double);
            ProcessNumbers(5, Square);

            // ── EXAMPLE 2: Using Built-in Action Delegate ───────────────
            Console.WriteLine("\n--- Action Delegate Callback ---");
            ProcessWithAction("Hello", s => Console.WriteLine($"  Processing: {s.ToUpper()}"));

            // ── EXAMPLE 3: Using Built-in Func Delegate ─────────────────
            Console.WriteLine("\n--- Func Delegate Callback ---");
            string result = TransformString("test", s => s.ToUpper() + "!");
            Console.WriteLine($"  Result: {result}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Delegate as Return Type
            // ═══════════════════════════════════════════════════════════

            // Methods can return delegates for deferred execution
            // Useful for factory patterns and lazy evaluation

            // ── EXAMPLE 1: Return Delegate from Method ──────────────────
            Console.WriteLine("\n--- Delegate as Return Type ---");
            TransformDelegate transformer = GetOperation("double");
            Console.WriteLine($"  Double(10): {transformer(10)}");

            transformer = GetOperation("square");
            Console.WriteLine($"  Square(10): {transformer(10)}");

            // ── EXAMPLE 2: Factory Pattern with Delegates ───────────────
            Console.WriteLine("\n--- Factory Pattern ---");
            var processor = CreateProcessor("filter");
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var filtered = FilterNumbers(numbers, processor);
            Console.WriteLine($"  Filtered (even only): {string.Join(", ", filtered)}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Built-in Action Delegate
            // ═══════════════════════════════════════════════════════════

            // Action represents a delegate that returns void
            // Supports 0 to 16 parameters

            // ── EXAMPLE 1: Action with No Parameters ────────────────────
            Console.WriteLine("\n--- Action Delegate (no params) ---");
            Action greeting = () => Console.WriteLine("  Hello from Action!");
            greeting();

            // ── EXAMPLE 2: Action with One Parameter ───────────────────
            Console.Write.WriteLine("\n--- Action Delegate (1 param) ---");
            Action<string> printMessage = msg => Console.WriteLine($"  Message: {msg}");
            printMessage("Hello, World!");

            // ── EXAMPLE 3: Action with Multiple Parameters ───────────────
            Console.WriteLine("\n--- Action Delegate (multiple params) ---");
            Action<string, int> showFormatted = (name, age) =>
                Console.WriteLine($"  {name} is {age} years old");
            showFormatted("Alice", 30);

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Built-in Func Delegate
            // ═══════════════════════════════════════════════════════════

            // Func represents a delegate that returns a value
            // Last type parameter is the return type

            // ── EXAMPLE 1: Func with No Input Parameters ────────────────
            Console.WriteLine("\n--- Func Delegate (no input) ---");
            Func<int> getRandom = () => new Random().Next(1, 100);
            Console.WriteLine($"  Random number: {getRandom()}");

            // ── EXAMPLE 2: Func with One Input Parameter ─────────────────
            Console.WriteLine("\n--- Func Delegate (1 input) ---");
            Func<int, int> doubleFunc = x => x * 2;
            Console.WriteLine($"  Double(21): {doubleFunc(21)}");

            // ── EXAMPLE 3: Func with Multiple Parameters ─────────────────
            Console.WriteLine("\n--- Func Delegate (multiple inputs) ---");
            Func<int, int, string> formatNumbers = (a, b) =>
                $"Sum: {a + b}, Product: {a * b}";
            Console.WriteLine($"  {formatNumbers(4, 5)}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Built-in Predicate Delegate
            // ═══════════════════════════════════════════════════════════

            // Predicate represents a method that returns true/false
            // Commonly used for filtering and validation

            // ── EXAMPLE 1: Basic Predicate ───────────────────────────────
            Console.WriteLine("\n--- Predicate Delegate ---");
            Predicate<int> isEven = n => n % 2 == 0;
            Console.WriteLine($"  Is 4 even? {isEven(4)}");    // Output: True
            Console.WriteLine($"  Is 7 even? {isEven(7)}");    // Output: False

            // ── EXAMPLE 2: Predicate with String ──────────────────────────
            Predicate<string> isLongEnough = s => s.Length >= 5;
            Console.WriteLine($"  'Hello' long enough? {isLongEnough("Hello")}");   // Output: True
            Console.WriteLine($"  'Hi' long enough? {isLongEnough("Hi")}");        // Output: False

            // ── EXAMPLE 3: Using Predicate with List ─────────────────────
            Console.WriteLine("\n--- Predicate with List ---");
            var names = new List<string> { "Alice", "Bob", "Charlie", "Diana" };
            Predicate<string> startsWithA = s => s.StartsWith("A");
            var filteredNames = names.FindAll(startsWithA);
            Console.WriteLine($"  Names starting with A: {string.Join(", ", filteredNames)}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World: Data Processing Pipeline
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Processing Pipeline with Func ────────────────
            Console.WriteLine("\n--- Real-World: Processing Pipeline ---");
            var pipeline = new DataPipeline();
            
            pipeline.Process(
                new[] { 1, 2, 3, 4, 5 },
                x => x * 2,        // Transform step
                x => x > 5         // Filter step
            );

            // ── EXAMPLE 2: Validation Chain ──────────────────────────────
            Console.WriteLine("\n--- Real-World: Validation Chain ---");
            var validator = new UserValidator();
            
            bool isValid = validator.Validate("john@example.com", "password123");
            Console.WriteLine($"  Validation result: {isValid}");

            Console.WriteLine("\n=== Delegate Basics Part 2 Complete ===");
        }

        // Methods for delegate examples
        static int Double(int x) => x * 2;
        static int Square(int x) => x * x;

        static void ProcessNumbers(int value, TransformDelegate transform)
        {
            int result = transform(value);
            Console.WriteLine($"  Transformed {value}: {result}");
        }

        static void ProcessWithAction(string input, Action<string> action)
        {
            action(input);
        }

        static string TransformString(string input, Func<string, string> transform)
        {
            return transform(input);
        }

        static TransformDelegate GetOperation(string operationType)
        {
            return operationType switch
            {
                "double" => x => x * 2,
                "square" => x => x * x,
                "triple" => x => x * 3,
                _ => x => x
            };
        }

        static Func<int, bool> CreateProcessor(string type)
        {
            return type switch
            {
                "filter" => x => x % 2 == 0,
                "greater" => x => x > 5,
                _ => x => true
            };
        }

        static int[] FilterNumbers(int[] numbers, Func<int, bool> filter)
        {
            var result = new List<int>();
            foreach (var num in numbers)
            {
                if (filter(num))
                    result.Add(num);
            }
            return result.ToArray();
        }

        // ═══════════════════════════════════════════════════════════
        // Real-World: Data Pipeline Class
        // ═══════════════════════════════════════════════════════════

        class DataPipeline
        {
            public void Process(int[] data, Func<int, int> transform, Func<int, bool> filter)
            {
                Console.WriteLine("\n--- Pipeline Processing ---");
                foreach (var item in data)
                {
                    var transformed = transform(item);
                    if (filter(transformed))
                    {
                        Console.WriteLine($"  {item} -> {transformed} [PASSED]");
                    }
                }
            }
        }

        // ═══════════════════════════════════════════════════════════
        // Real-World: User Validator Class
        // ═══════════════════════════════════════════════════════════

        class UserValidator
        {
            public bool Validate(string email, string password)
            {
                return IsValidEmail(email) && IsValidPassword(password);
            }

            private bool IsValidEmail(string email)
            {
                Predicate<string> emailPattern = e =>
                    e.Contains("@") && e.Contains(".") && e.Length > 5;
                return emailPattern(email);
            }

            private bool IsValidPassword(string password)
            {
                Predicate<string> passwordCheck = p =>
                    p.Length >= 8 && p.Any(char.IsDigit);
                return passwordCheck(password);
            }
        }
    }
}
