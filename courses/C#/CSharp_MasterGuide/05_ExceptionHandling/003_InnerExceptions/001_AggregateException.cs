/*
 * TOPIC: Exception Handling in C#
 * SUBTOPIC: AggregateException for Multiple Exceptions
 * FILE: AggregateException.cs
 * PURPOSE: Demonstrate AggregateException for handling multiple exceptions concurrently
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._05_ExceptionHandling._03_InnerExceptions
{
    class AggregateExceptionDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== AggregateException Demo ===\n");

            BasicAggregateException();

            Console.WriteLine("\n=== Handling Multiple Parallel Exceptions ===\n");

            HandleMultipleExceptions();

            Console.WriteLine("\n=== Flattening AggregateExceptions ===\n");

            FlattenAggregateExceptions();

            Console.WriteLine("\n=== Extracting Specific Exceptions ===\n");

            ExtractSpecificExceptions();
        }

        static void BasicAggregateException()
        {
            Console.WriteLine("1. Creating AggregateException:");
            Console.WriteLine("-------------------------------");

            var exceptions = new List<Exception>
            {
                new InvalidOperationException("First operation failed"),
                new ArgumentException("Invalid argument"),
                new FormatException("Format error")
            };

            var aggEx = new System.AggregateException("Multiple operations failed", exceptions);

            Console.WriteLine($"Message: {aggEx.Message}");
            Console.WriteLine($"Inner Exception Count: {aggEx.InnerExceptions.Count}");

            foreach (var ex in aggEx.InnerExceptions)
            {
                Console.WriteLine($"  - {ex.GetType().Name}: {ex.Message}");
            }

            // Output: Message: Multiple operations failed
            // Output: Inner Exception Count: 3
            // Output:   - InvalidOperationException: First operation failed
            // Output:   - ArgumentException: Invalid argument
            // Output:   - FormatException: Format error
        }

        static void HandleMultipleExceptions()
        {
            Console.WriteLine("2. Task Parallel Library Exceptions:");
            Console.WriteLine("-----------------------------");

            try
            {
                Parallel.Invoke(
                    () => { throw new DivideByZeroException("Division task failed"); },
                    () => { throw new IndexOutOfRangeException("Index task failed"); },
                    () => { throw new NullReferenceException("Null task failed"); }
                );
            }
            catch (System.AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                {
                    Console.WriteLine($"Handled: {ex.GetType().Name}");
                }

                ae.Handle(ex =>
                {
                    Console.WriteLine($"Handling exception: {ex.Message}");
                    return true;
                });
            }

            // Output: Handled: DivideByZeroException
            // Output: Handled: IndexOutOfRangeException
            // Output: Handled: NullReferenceException
            // Output: Handling exception: Division task failed
            // Output: Handling exception: Index task failed
            // Output: Handling exception: Null task failed
        }

        static void FlattenAggregateExceptions()
        {
            Console.WriteLine("3. Flattening Nested AggregateExceptions:");
            Console.WriteLine("-----------------------------------");

            var inner1 = new System.AggregateException(
                "Inner 1",
                new List<Exception> { new ArgumentException("Arg error 1"), new FormatException("Format error 1") }
            );

            var agg = new System.AggregateException(
                "Outer aggregate",
                new List<Exception>
                {
                    new InvalidOperationException("Op error"),
                    inner1
                }
            );

            Console.WriteLine($"Before Flatten - InnerExceptions.Count: {agg.InnerExceptions.Count}");

            var flattened = agg.Flatten();

            Console.WriteLine($"After Flatten - InnerExceptions.Count: {flattened.InnerExceptions.Count}");

            foreach (var ex in flattened.InnerExceptions)
            {
                Console.WriteLine($"  - {ex.GetType().Name}: {ex.Message}");
            }

            // Output: Before Flatten - InnerExceptions.Count: 2
            // Output: After Flatten - InnerExceptions.Count: 3
            // Output:   - InvalidOperationException: Op error
            // Output:   - ArgumentException: Arg error 1
            // Output:   - FormatException: Format error 1
        }

        static void ExtractSpecificExceptions()
        {
            Console.WriteLine("4. Extracting Specific Exception Types:");
            Console.WriteLine("---------------------------------------");

            var agg = new System.AggregateException(
                "Mixed exceptions",
                new List<Exception>
                {
                    new ArgumentException("Bad argument"),
                    new DivideByZeroException("Division by zero"),
                    new ArgumentNullException("Null argument"),
                    new FormatException("Bad format")
                }
            );

            var arguments = agg.InnerExceptions.OfType<ArgumentException>();
            Console.WriteLine($"ArgumentException count: {arguments.Count()}");

            var divideByZero = agg.InnerExceptions.OfType<DivideByZeroException>();
            Console.WriteLine($"DivideByZeroException count: {divideByZero.Count()}");

            Console.WriteLine("\nHandling by type:");

            agg.Handle(ex =>
            {
                if (ex is ArgumentException)
                {
                    Console.WriteLine($"  Handled ArgumentException: {ex.Message}");
                    return true;
                }
                return false;
            });

            // Output: ArgumentException count: 2
            // Output: DivideByZeroException count: 1
            // Output: 
            // Output: Handling by type:
            // Output:   Handled ArgumentException: Bad argument
            // Output:   Handled ArgumentException: Null argument
        }
    }
}