/*
================================================================================
TOPIC 25: LAMBDA EXPRESSIONS
================================================================================

Lambda expressions are anonymous functions used for concise code.

TABLE OF CONTENTS:
1. What are Lambdas?
2. Expression Lambdas
3. Statement Lambdas
4. Using with Delegates and LINQ
================================================================================
*/

using System;
using System.Linq;
using System.Collections.Generic;

namespace LambdaExamples
{
    class Program
    {
        delegate int MathOperation(int x);
        delegate int BinaryOperation(int a, int b);
        
        static void Main()
        {
            // Expression lambda
            MathOperation square = x => x * x;
            Console.WriteLine($"Square of 5: {square(5)}");
            
            // Multiple parameters
            BinaryOperation add = (a, b) => a + b;
            Console.WriteLine($"5 + 3 = {add(5, 3)}");
            
            // Statement lambda
            MathOperation cube = x => 
            {
                int result = x * x * x;
                return result;
            };
            Console.WriteLine($"Cube of 3: {cube(3)}");
            
            // With LINQ
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
            
            var evens = numbers.Where(n => n % 2 == 0);
            Console.WriteLine($"\nEvens: {string.Join(", ", evens)}");
            
            var doubled = numbers.Select(n => n * 2);
            Console.WriteLine($"Doubled: {string.Join(", ", doubled)}");
            
            // Action and Func
            Action<string> print = msg => Console.WriteLine(msg);
            print("Hello from lambda!");
            
            Func<int, int, int> multiply = (a, b) => a * b;
            Console.WriteLine($"4 * 5 = {multiply(4, 5)}");
        }
    }
}

/*
LAMBDA SYNTAX:
--------------
(parameters) => expression
(parameters) => { statements }

=> is read as "goes to"

BENEFITS:
---------
- Concise syntax
- Useful for delegates
- Great with LINQ
- Enables functional programming
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 26 covers Asynchronous Programming.
*/
