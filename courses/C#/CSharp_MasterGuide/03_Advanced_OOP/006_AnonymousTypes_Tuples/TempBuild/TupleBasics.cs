/*
 * TOPIC: Tuples
 * SUBTOPIC: Tuple basics (Tuple<T1,...>, Item1, Item2)
 * FILE: TupleBasics.cs
 * PURPOSE: Demonstrate basic tuple usage in C# including creation, access, and common patterns
 */
using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._06_AnonymousTypes_Tuples
{
    public class TupleBasics
    {
        public static void Main()
        {
            // Basic Tuple<T1, T2> creation using Tuple class
            // Tuple<T1, T2> is a generic class that holds two values of specified types
            Tuple<string, int> person = Tuple.Create("Alice", 30);
            
            Console.WriteLine(person.Item1);    // Output: Alice
            Console.WriteLine(person.Item2);    // Output: 30

            // Tuple<T1, T2, T3> - three elements
            Tuple<string, int, string> employee = Tuple.Create("Bob", 25, "Engineering");
            
            Console.WriteLine($"{employee.Item1} works in {employee.Item3}");    // Output: Bob works in Engineering

            // Tuple with up to 8 elements (Tuple<T1,...,T8>)
            // The 8th element can be another tuple (nesting)
            var complexData = Tuple.Create("Product", 100, 25.50m, true, "Category", DateTime.Now, "Supplier", Tuple.Create(5));
            
            Console.WriteLine($"Product: {complexData.Item1}, Qty: {complexData.Item2}, Price: {complexData.Item3}");    // Output: Product: Product, Qty: 100, Price: 25.50
            Console.WriteLine($"Supplier ID: {complexData.Item8.Item1}");    // Output: Supplier ID: 5

            // Accessing tuple elements via Item1, Item2, etc.
            // Note: Tuple elements are read-only once created
            var coordinates = Tuple.Create(10, 20);
            int x = coordinates.Item1;
            int y = coordinates.Item2;
            
            Console.WriteLine($"X: {x}, Y: {y}");    // Output: X: 10, Y: 20

            // Using tuples as dictionary keys (they implement equality)
            var scoreLookup = new Dictionary<Tuple<string, string>, int>
            {
                { Tuple.Create("Alice", "Math"), 95 },
                { Tuple.Create("Alice", "Science"), 88 },
                { Tuple.Create("Bob", "Math"), 92 }
            };

            Console.WriteLine($"Alice Math Score: {scoreLookup[Tuple.Create("Alice", "Math")]}");    // Output: Alice Math Score: 95

            // Nested tuples
            var nested = Tuple.Create(1, Tuple.Create(2, 3));
            
            Console.WriteLine(nested.Item1);                              // Output: 1
            Console.WriteLine(nested.Item2.Item1);                      // Output: 2
            Console.WriteLine(nested.Item2.Item2);                      // Output: 3

            // Returning tuple from a method (multiple return values)
            var divisionResult = Divide(10, 3);
            Console.WriteLine($"Quotient: {divisionResult.Item1}, Remainder: {divisionResult.Item2}");    // Output: Quotient: 3, Remainder: 1

            // Tuple as generic type parameter
            var listOfPairs = new List<Tuple<string, int>>
            {
                Tuple.Create("One", 1),
                Tuple.Create("Two", 2),
                Tuple.Create("Three", 3)
            };

            foreach (var pair in listOfPairs)
            {
                Console.WriteLine($"{pair.Item1} = {pair.Item2}");
                // Output:
                // One = 1
                // Two = 2
                // Three = 3
            }

            // Tuple element types can be different
            var mixedTypes = Tuple.Create("String", 42, 3.14, true, 'A');
            
            Console.WriteLine($"{mixedTypes.Item1} ({mixedTypes.Item1.GetType().Name})");    // Output: String (String)
            Console.WriteLine($"{mixedTypes.Item2} ({mixedTypes.Item2.GetType().Name})");    // Output: 42 (Int32)
            Console.WriteLine($"{mixedTypes.Item3} ({mixedTypes.Item3.GetType().Name})");    // Output: 3.14 (Double)
            Console.WriteLine($"{mixedTypes.Item4} ({mixedTypes.Item4.GetType().Name})");    // Output: True (Boolean)
            Console.WriteLine($"{mixedTypes.Item5} ({mixedTypes.Item5.GetType().Name})");    // Output: A (Char)

            Console.WriteLine();
            Console.WriteLine("=== Real-World Examples ===");
            Console.WriteLine();

            // Real-world Example 1: HTTP response status
            // Returning both status code and message from a method
            var httpResponse = GetHttpResponse(200);
            Console.WriteLine($"Status: {httpResponse.Item1} - {httpResponse.Item2}");    // Output: Status: 200 - OK
            
            var errorResponse = GetHttpResponse(404);
            Console.WriteLine($"Status: {errorResponse.Item1} - {errorResponse.Item2}");    // Output: Status: 404 - Not Found

            // Real-world Example 2: Date range representation
            var fiscalQuarter = Tuple.Create(new DateTime(2024, 1, 1), new DateTime(2024, 3, 31));
            
            Console.WriteLine($"Q1 2024: {fiscalQuarter.Item1:yyyy-MM-dd} to {fiscalQuarter.Item2:yyyy-MM-dd}");    // Output: Q1 2024: 2024-01-01 to 2024-03-31

            // Real-world Example 3: Key-value pair with metadata
            var configSetting = Tuple.Create("MaxConnections", 100, "Maximum concurrent connections allowed", true);
            
            Console.WriteLine($"Setting: {configSetting.Item1}");
            Console.WriteLine($"  Value: {configSetting.Item2}");    // Output: Value: 100
            Console.WriteLine($"  Description: {configSetting.Item3}");    // Output: Description: Maximum concurrent connections allowed
            Console.WriteLine($"  IsEditable: {configSetting.Item4}");    // Output: IsEditable: True
        }

        // Method returning a tuple for multiple return values
        static Tuple<int, int> Divide(int dividend, int divisor)
        {
            int quotient = dividend / divisor;
            int remainder = dividend % divisor;
            return Tuple.Create(quotient, remainder);
        }

        // Simulated HTTP response method
        static Tuple<int, string> GetHttpResponse(int statusCode)
        {
            string message;
            switch (statusCode)
            {
                case 200: message = "OK"; break;
                case 404: message = "Not Found"; break;
                case 500: message = "Internal Server Error"; break;
                default: message = "Unknown"; break;
            }
            return Tuple.Create(statusCode, message);
        }
    }
}