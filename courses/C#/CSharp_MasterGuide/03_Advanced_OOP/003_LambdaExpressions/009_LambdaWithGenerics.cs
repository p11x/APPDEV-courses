/*
 * TOPIC: CSharp_MasterGuide/03_Advanced_OOP/03_LambdaExpressions
 * SUBTOPIC: Lambda with Generic Types
 * FILE: LambdaWithGenerics.cs
 * PURPOSE: Understanding generic delegates and lambda expressions with generic type parameters
 */
using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._03_LambdaExpressions
{
    // Custom generic delegate types
    public delegate T Identity<T>(T value);
    public delegate TResult Transform<TInput, TResult>(TInput input);
    public delegate T Combine<T>(T a, T b);

    public class LambdaWithGenerics
    {
        public static void RunMain(string[] args)
        {
            Console.WriteLine("=== Lambda with Generic Types ===\n");

            // ============================================
            // GENERIC DELEGATE BASICS
            // ============================================

            // Example 1: Simple identity lambda with generic type
            Identity<int> identityInt = x => x;
            Console.WriteLine($"Identity<int>(42): {identityInt(42)}"); // Output: 42

            Identity<string> identityString = x => x;
            Console.WriteLine($"Identity<string>(\"test\"): {identityString("test")}"); // Output: test

            // Example 2: Transform delegate with different input/output types
            Transform<int, string> intToString = x => $"Number: {x}";
            Console.WriteLine($"Transform<int, string>: {intToString(123)}"); // Output: Number: 123

            Transform<string, int> stringLength = x => x.Length;
            Console.WriteLine($"Transform<string, int>: {stringLength("Hello")}"); // Output: 5

            Transform<double, int> doubleToInt = x => (int)x;
            Console.WriteLine($"Transform<double, int>: {doubleToInt(3.7)}"); // Output: 3

            // ============================================
            // GENERIC LAMBDA WITH CONSTRAINTS
            // ============================================

            // Example 3: Using where clause in generic methods
            // Note: Lambda can't have where clause directly, but method can
            var list = new List<int> { 1, 2, 3, 4, 5 };
            Console.WriteLine($"Sum: {SumGeneric(list)}"); // Output: 15

            var stringList = new List<string> { "a", "bb", "ccc" };
            Console.WriteLine($"Total length: {SumLengthGeneric(stringList)}"); // Output: 6

            // Example 4: Generic lambda with class constraint simulation
            // We simulate this with regular lambdas since lambdas can't have constraints
            var items = new List<LambdaProduct>
            {
                new LambdaProduct { Name = "Laptop", Price = 999.99m },
                new LambdaProduct { Name = "Phone", Price = 599.99m },
                new LambdaProduct { Name = "Tablet", Price = 399.99m }
            };

            // Lambda accessing properties (no constraint needed)
            Func<LambdaProduct, decimal> getPrice = p => p.Price;
            Func<LambdaProduct, string> getName = p => p.Name;

            Console.WriteLine($"\nMost expensive: ${GetMax(items, getPrice):F2}"); // Output: $999.99
            Console.WriteLine($"First item: {GetFirst(items, getName)}"); // Output: Laptop

            // ============================================
            // BUILT-IN GENERIC DELEGATES WITH LAMBDA
            // ============================================

            // Example 5: Func<> delegate variants
            // Func<TResult> - no input, returns TResult
            Func<DateTime> now = () => DateTime.Now;
            Console.WriteLine($"\nCurrent time: {now():HH:mm:ss}");

            // Func<T, TResult> - one input, returns TResult
            Func<string, int> parseInt = s => int.Parse(s);
            Console.WriteLine($"Parse '123': {parseInt("123")}"); // Output: 123

            // Func<T1, T2, TResult> - two inputs
            Func<int, int, int> max = (a, b) => a > b ? a : b;
            Console.WriteLine($"Max(5, 10): {max(5, 10)}"); // Output: 10

            // Func<T1, T2, T3, TResult> - three inputs
            Func<int, int, int, int> sum3 = (a, b, c) => a + b + c;
            Console.WriteLine($"Sum(1, 2, 3): {sum3(1, 2, 3)}"); // Output: 6

            // Func<T1, T2, T3, T4, TResult> - four inputs
            Func<int, int, int, int, int> sum4 = (a, b, c, d) => a + b + c + d;
            Console.WriteLine($"Sum(1, 2, 3, 4): {sum4(1, 2, 3, 4)}"); // Output: 10

            // Example 6: Action<> delegate variants (void return)
            // Action - no parameters
            Action printHello = () => Console.WriteLine("Hello!");
            printHello(); // Output: Hello!

            // Action<T> - one parameter
            Action<string> print = s => Console.WriteLine(s);
            print("Hello from Action!"); // Output: Hello from Action!

            // Action<T1, T2> - two parameters
            Action<string, int> printMultiple = (s, n) =>
            {
                for (int i = 0; i < n; i++)
                {
                    Console.WriteLine(s);
                }
            };
            Console.WriteLine("\nPrint 3 times:");
            printMultiple("Echo!", 3); // Output: Echo! (3 times)

            // Action<T1, T2, T3> - three parameters
            Action<int, int, int> printSum = (a, b, c) => Console.WriteLine($"Sum: {a + b + c}");
            printSum(1, 2, 3); // Output: Sum: 6

            // Example 7: Predicate<T> (returns bool)
            Predicate<int> isPositive = n => n > 0;
            Console.WriteLine($"\nIs 5 positive? {isPositive(5)}"); // True
            Console.WriteLine($"Is -5 positive? {isPositive(-5)}"); // False

            Predicate<string> isLongEnough = s => s.Length >= 5;
            Console.WriteLine($"Is 'Hello world' long enough? {isLongEnough("Hello world")}"); // True
            Console.WriteLine($"Is 'Hi' long enough? {isLongEnough("Hi")}"); // False

            // Example 8: Custom generic delegate with Combine
            Combine<int> addInt = (a, b) => a + b;
            Combine<string> concatString = (a, b) => a + b;
            Combine<double> addDouble = (a, b) => a + b;

            Console.WriteLine($"\nCombine<int>(5, 3): {addInt(5, 3)}"); // Output: 8
            Console.WriteLine($"Combine<string>('Hello', ' World'): {concatString("Hello", " World")}"); // Output: Hello World
            Console.WriteLine($"Combine<double>(2.5, 3.5): {addDouble(2.5, 3.5)}"); // Output: 6
        }

        // Generic method to sum numeric values
        public static T SumGeneric<T>(List<T> list)
        {
            dynamic sum = default(T);
            foreach (var item in list)
            {
                sum += item;
            }
            return sum;
        }

        // Generic method for string lengths (specialized)
        public static int SumLengthGeneric(List<string> list)
        {
            int sum = 0;
            foreach (var s in list)
            {
                sum += s.Length;
            }
            return sum;
        }

        // Generic method to find max using selector lambda
        public static TResult GetMax<T>(List<T> list, Func<T, TResult> selector) where T : class
        {
            T max = null;
            dynamic maxValue = decimal.MinValue;
            foreach (var item in list)
            {
                dynamic current = selector(item);
                if (current > maxValue)
                {
                    maxValue = current;
                    max = item;
                }
            }
            return selector(max);
        }

        // Generic method to get first item using selector
        public static TResult GetFirst<T>(List<T> list, Func<T, TResult> selector) where T : class
        {
            return selector(list[0]);
        }
    }

    public class LambdaProduct
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}