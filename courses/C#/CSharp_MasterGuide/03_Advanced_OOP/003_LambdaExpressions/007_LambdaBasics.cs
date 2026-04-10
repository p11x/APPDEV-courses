/*
 * TOPIC: CSharp_MasterGuide/03_Advanced_OOP/03_LambdaExpressions
 * SUBTOPIC: Lambda Expression Basics
 * FILE: LambdaBasics.cs
 * PURPOSE: Introduction to lambda expression syntax, parameters, and expression vs statement body
 */
using System;

namespace CSharp_MasterGuide._03_Advanced_OOP._03_LambdaExpressions
{
    // Delegate type for simple lambda examples
    public delegate int MathOperation(int a, int b);

    // Delegate that takes no parameters and returns a string
    public delegate string StringProducer();

    // Delegate with no return value (void)
    public delegate void MessagePrinter(string message);

    public class LambdaBasics
    {
        public static void RunMain(string[] args)
        {
            Console.WriteLine("=== Lambda Expression Basics ===\n");

            // Example 1: Basic lambda with expression body (single expression)
            // Lambda: x => x * 2 (read as "x goes to x times 2")
            // This is an expression lambda - no curly braces needed
            Func<int, int> doubleNumber = x => x * 2;
            Console.WriteLine($"Double of 5: {doubleNumber(5)}"); // Output: 10

            // Example 2: Lambda with explicit parameter type
            // When compiler cannot infer types, you can specify them explicitly
            Func<int, int, int> add = (int a, int b) => a + b;
            Console.WriteLine($"3 + 4 = {add(3, 4)}"); // Output: 7

            // Example 3: Lambda with multiple parameters
            // Parameters are enclosed in parentheses when there's more than one
            MathOperation multiply = (a, b) => a * b;
            Console.WriteLine($"6 * 7 = {multiply(6, 7)}"); // Output: 42

            // Example 4: Expression body vs Statement body
            // Expression body (single expression, implicitly returns)
            Func<int, int> cube = x => x * x * x;
            Console.WriteLine($"Cube of 3: {cube(3)}"); // Output: 27

            // Statement body (multiple statements, requires curly braces and return keyword)
            // This is called a "statement lambda"
            Func<int, int> factorial = n =>
            {
                int result = 1;
                for (int i = 1; i <= n; i++)
                {
                    result *= i;
                }
                return result;
            };
            Console.WriteLine($"5! = {factorial(5)}"); // Output: 120

            // Example 5: Lambda with no parameters
            StringProducer currentTime = () => DateTime.Now.ToShortTimeString();
            Console.WriteLine($"Current time: {currentTime()}"); // Output: varies (e.g., 10:30 AM)

            // Example 6: Lambda with void return (Action delegate)
            MessagePrinter printMessage = message => Console.WriteLine($"Message: {message}");
            printMessage("Hello from lambda!"); // Output: Message: Hello from lambda!

            // Example 7: Complex expression body
            Func<int, bool> isEven = n => n % 2 == 0;
            Console.WriteLine($"Is 10 even? {isEven(10)}"); // Output: True
            Console.WriteLine($"Is 7 even? {isEven(7)}");  // Output: False

            // Example 8: Using var with lambda (type inference)
            // Compiler infers delegate type from variable declaration
            var square = (Func<int, int>)(x => x * x); // Explicit cast needed for var
            Console.WriteLine($"Square of 8: {square(8)}"); // Output: 64

            // Example 9: Lambda returning boolean complex expression
            Func<int, int, bool> isDivisible = (a, b) => b != 0 && a % b == 0;
            Console.WriteLine($"Is 20 divisible by 5? {isDivisible(20, 5)}"); // Output: True
            Console.WriteLine($"Is 20 divisible by 7? {isDivisible(20, 7)}"); // Output: False
        }
    }
}