/*
 * TOPIC: Exception Handling in C#
 * SUBTOPIC: Inner Exceptions - Understanding InnerException Property
 * FILE: InnerExceptions.cs
 * PURPOSE: Demonstrate the InnerException property and how inner exceptions preserve original error context
 */

using System;
using System.IO;

namespace CSharp_MasterGuide._05_ExceptionHandling._03_InnerExceptions
{
    class InnerExceptions
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Inner Exceptions Demo ===\n");

            DemonstrateInnerException();

            Console.WriteLine("\n=== Accessing Inner Exception Details ===\n");

            AccessInnerExceptionDetails();
        }

        static void DemonstrateInnerException()
        {
            Console.WriteLine("1. Basic Inner Exception Concept:");
            Console.WriteLine("-------------------------------");

            try
            {
                OuterMethod();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Outer Exception: {ex.GetType().Name}");
                Console.WriteLine($"Outer Message: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.GetType().Name}");
                    Console.WriteLine($"Inner Message: {ex.InnerException.Message}");
                }
            }

            // Output: Outer Exception: InvalidOperationException
            // Output: Outer Message: Configuration validation failed
            // Output: Inner Exception: FormatException
            // Output: Inner Message: Invalid string format for port number
        }

        static void OuterMethod()
        {
            try
            {
                InnerMethod();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Configuration validation failed", ex);
            }
        }

        static void InnerMethod()
        {
            string portInput = "not-a-number";
            int port = int.Parse(portInput);
        }

        static void AccessInnerExceptionDetails()
        {
            Console.WriteLine("2. Traversing Inner Exception Chain:");
            Console.WriteLine("-----------------------------------");

            try
            {
                Level1Method();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Top-level exception: {ex.Message}");

                Exception? current = ex.InnerException;
                int level = 1;

                while (current is { } )
                {
                    Console.WriteLine($"Level {level} inner exception: {current.Message}");
                    current = current.InnerException;
                    level++;
                }

                Console.WriteLine($"\nTotal chain depth: {level} exceptions");
            }

            // Output: Top-level exception: Data processing failed - see inner exceptions for details
            // Output: Level 1 inner exception: Invalid data format
            // Output: Level 2 inner exception: Input string was not in a correct format
            // Output: Total chain depth: 3 exceptions
        }

        static void Level1Method()
        {
            try
            {
                Level2Method();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Data processing failed - see inner exceptions for details", ex);
            }
        }

        static void Level2Method()
        {
            try
            {
                Level3Method();
            }
            catch (Exception ex)
            {
                throw new FormatException("Invalid data format", ex);
            }
        }

        static void Level3Method()
        {
            decimal value = decimal.Parse("invalid");
        }
    }
}