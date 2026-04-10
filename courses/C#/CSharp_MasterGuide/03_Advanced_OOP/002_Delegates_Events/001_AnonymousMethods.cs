/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Delegates and Events - Anonymous Methods
 * FILE      : AnonymousMethods.cs
 * PURPOSE   : Teaches anonymous methods, delegate inference,
 *            lambda expressions, and differences between
 *            anonymous methods and lambdas
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._02_Delegates_Events
{
    class AnonymousMethods
    {
        // Delegate declarations
        public delegate void MessageHandler(string message);
        public delegate int CalculationHandler(int a, int b);
        public delegate bool FilterHandler(int value);

        static void Main(string[] args)
        {
            Console.WriteLine("=== Anonymous Methods in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: What are Anonymous Methods?
            // ═══════════════════════════════════════════════════════════

            // Anonymous methods allow you to define delegate instances
            // inline without creating a separate method
            // Introduced in C# 2.0

            // ── EXAMPLE 1: Basic Anonymous Method ──────────────────────
            Console.WriteLine("--- Basic Anonymous Method ---");
            
            // Using delegate keyword with inline code
            MessageHandler handler1 = delegate(string msg)
            {
                Console.WriteLine($"  Received: {msg}");
            };
            
            handler1("Hello from anonymous method!");  // Output: Received: Hello from anonymous method!

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Anonymous Method Syntax
            // ═══════════════════════════════════════════════════════════

            // Anonymous methods use the delegate keyword
            // Parameters are declared in parentheses
            // Body is a code block

            // ── EXAMPLE 1: With Parameters ───────────────────────────────
            Console.WriteLine("\n--- Anonymous Method with Parameters ---");
            
            CalculationHandler calc = delegate(int x, int y)
            {
                return x + y;
            };
            
            int sum = calc(10, 20);  // Output: 30
            Console.WriteLine($"  Sum: {sum}");

            // ── EXAMPLE 2: With Multiple Statements ──────────────────────
            Console.WriteLine("\n--- Anonymous Method with Multiple Statements ---");
            
            MessageHandler multiLine = delegate(string msg)
            {
                Console.WriteLine($"  Starting processing...");
                Console.WriteLine($"  Message: {msg}");
                Console.WriteLine($"  Completed!");
            };
            
            multiLine("Test message");

            // ── EXAMPLE 3: Returning Value ───────────────────────────────
            Console.WriteLine("\n--- Anonymous Method Returning Value ---");
            
            FilterHandler filter = delegate(int value)
            {
                if (value > 10)
                    return true;
                return false;
            };
            
            Console.WriteLine($"  Filter(15): {filter(15)}");   // Output: True
            Console.WriteLine($"  Filter(5): {filter(5)}");     // Output: False

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Delegate Inference
            // ═══════════════════════════════════════════════════════════

            // C# compiler can infer delegate type from method group
            // No explicit delegate declaration needed

            // ── EXAMPLE 1: Method Group Conversion ───────────────────────
            Console.WriteLine("\n--- Delegate Inference ---");
            
            // Compiler automatically converts PrintMessage to delegate
            MessageHandler handler2 = PrintMessage;
            handler2("Inferred delegate");  // Output: Inferred delegate

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Lambda Expressions vs Anonymous Methods
            // ═══════════════════════════════════════════════════════════

            // Lambda expressions (C# 3.0+) are more concise
            // Anonymous methods (C# 2.0+) are more explicit
            // Lambdas can be converted to delegates or expression trees

            // ── EXAMPLE 1: Lambda Equivalent of Anonymous Method ─────────
            Console.WriteLine("\n--- Lambda vs Anonymous Method ---");
            
            // Anonymous method
            MessageHandler anonMethod = delegate(string msg)
            {
                Console.WriteLine($"  Anonymous: {msg}");
            };
            
            // Lambda expression (explicit parameters)
            MessageHandler lambdaExplicit = (string msg) =>
            {
                Console.WriteLine($"  Lambda (explicit): {msg}");
            };
            
            // Lambda expression (implicit parameters)
            MessageHandler lambdaImplicit = msg =>
            {
                Console.WriteLine($"  Lambda (implicit): {msg}");
            };
            
            anonMethod("Test");
            lambdaExplicit("Test");
            lambdaImplicit("Test");

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Key Differences
            // ═══════════════════════════════════════════════════════════

            // Anonymous methods can omit parameter list when not used
            // Lambdas have expression body syntax
            // Expression lambdas can be converted to expression trees

            // ── EXAMPLE 1: Anonymous Method Without Parameters ────────────
            Console.WriteLine("\n--- Anonymous Method Without Parameters ---");
            
            Action anonymousNoParams = delegate
            {
                Console.WriteLine("  No parameters needed!");
            };
            
            anonymousNoParams();

            // ── EXAMPLE 2: Expression Lambda ─────────────────────────────
            Console.WriteLine("\n--- Expression Lambda ---");
            
            // Single expression - no braces needed
            Func<int, int> doubleExpr = x => x * 2;
            Console.WriteLine($"  Double(5): {doubleExpr(5)}");
            
            // Equivalent to:
            Func<int, int> doubleBlock = x => { return x * 2; };
            Console.WriteLine($"  Double(5) block: {doubleBlock(5)}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Using with Built-in Delegates
            // ═══════════════════════════════════════════════════════════

            // Anonymous methods and lambdas work with Action, Func, Predicate

            // ── EXAMPLE 1: With Action ─────────────────────────────────────
            Console.WriteLine("\n--- Anonymous with Action ---");
            
            Action<string> print = delegate(string s)
            {
                Console.WriteLine($"  Printing: {s}");
            };
            print("Test");

            // ── EXAMPLE 2: With Func ──────────────────────────────────────
            Console.WriteLine("\n--- Anonymous with Func ---");
            
            Func<int, int, int> add = delegate(int a, int b)
            {
                return a + b;
            };
            Console.WriteLine($"  Add(3, 4): {add(3, 4)}");

            // ── EXAMPLE 3: With Predicate ──────────────────────────────────
            Console.WriteLine("\n--- Anonymous with Predicate ---");
            
            Predicate<int> isPositive = delegate(int n)
            {
                return n > 0;
            };
            Console.WriteLine($"  IsPositive(-5): {isPositive(-5)}");  // Output: False
            Console.WriteLine($"  IsPositive(10): {isPositive(10)}");  // Output: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Real-World: Event Handling
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Button Click with Anonymous Method ────────────
            Console.WriteLine("\n--- Real-World: Button Click ---");
            
            var button = new SimpleButton();
            
            // Subscribe using anonymous method
            button.Click += delegate(object sender, EventArgs e)
            {
                Console.WriteLine("  Button clicked! (anonymous method)");
            };
            
            button.ClickButton();

            // ── EXAMPLE 2: List Filtering ─────────────────────────────────
            Console.WriteLine("\n--- Real-World: List Filtering ---");
            
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            // Using anonymous method with Predicate
            Predicate<int> greaterThan5 = delegate(int n) { return n > 5; };
            var filtered = numbers.FindAll(greaterThan5);
            Console.WriteLine($"  Numbers > 5: {string.Join(", ", filtered)}");

            // ── EXAMPLE 3: LINQ-like Operations ───────────────────────────
            Console.WriteLine("\n--- Real-World: Custom LINQ-like Filter ---");
            
            var names = new List<string> { "Alice", "Bob", "Charlie", "Diana" };
            
            FilterDelegate<string> startsWith = delegate(string s) { return s.StartsWith("A"); };
            
            foreach (var name in names)
            {
                if (startsWith(name))
                    Console.WriteLine($"  Starts with A: {name}");
            }

            Console.WriteLine("\n=== Anonymous Methods Complete ===");
        }

        // Helper method for delegate inference
        static void PrintMessage(string message)
        {
            Console.WriteLine($"  {message}");
        }

        // Generic filter delegate
        public delegate bool FilterDelegate<T>(T value);
    }

    // Simple button class for event example
    class SimpleButton
    {
        public event EventHandler Click;

        public void ClickButton()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }
    }
}
