/*
 * TOPIC: Tuples
 * SUBTOPIC: More tuple basics (nested tuples, comparison)
 * FILE: TupleBasics_Part2.cs
 * PURPOSE: Demonstrate nested tuples, tuple comparison, and advanced tuple patterns
 */
using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._06_AnonymousTypes_Tuples
{
    public class TupleBasics_Part2
    {
        public static void Main()
        {
            // Nested tuples - tuple inside another tuple
            // Useful for hierarchical data structures
            var treeNode = Tuple.Create("Root", 
                Tuple.Create("Child1", 
                    Tuple.Create("Grandchild1", Tuple.Create("GreatGrandchild1"))));
            
            Console.WriteLine(treeNode.Item1);                          // Output: Root
            Console.WriteLine(treeNode.Item2.Item1);                   // Output: Child1
            Console.WriteLine(treeNode.Item2.Item2.Item1);             // Output: Grandchild1

            // Deeply nested tuple access
            Console.WriteLine(treeNode.Item2.Item2.Item2.Item1);       // Output: GreatGrandchild1

            // Creating a simple hierarchy with nested tuples
            var company = Tuple.Create("TechCorp",
                Tuple.Create("Engineering", 
                    Tuple.Create("Backend Team", Tuple.Create("Alice", "Bob"))),
                Tuple.Create("Sales", 
                    Tuple.Create("Enterprise", Tuple.Create("Charlie", "Diana"))));
            
            Console.WriteLine($"Company: {company.Item1}");    // Output: Company: TechCorp
            Console.WriteLine($"  Dept: {company.Item2.Item1}");    // Output:   Dept: Engineering
            Console.WriteLine($"    Team: {company.Item2.Item2.Item1}");    // Output:     Team: Backend Team

            // Tuple comparison - tuples implement IComparable
            // Tuple comparison uses lexicographic ordering (Item1, then Item2, etc.)
            var tuple1 = Tuple.Create(1, 2, 3);
            var tuple2 = Tuple.Create(1, 2, 3);
            var tuple3 = Tuple.Create(1, 2, 4);
            var tuple4 = Tuple.Create(2, 1, 1);

            Console.WriteLine(tuple1.Equals(tuple2));    // Output: True (same values)
            Console.WriteLine(tuple1.Equals(tuple3));    // Output: False (Item3 differs)
            Console.WriteLine(CompareTuples(tuple1, tuple4)); // Output: -1 (1 < 2)

            // Comparing tuples with different element counts
            var shortTuple = Tuple.Create(1, 2);
            var longTuple = Tuple.Create(1, 2, 3);
            
            Console.WriteLine(CompareTuples(shortTuple, longTuple));    // Output: -1 (shorter tuple is "less")

            // Tuple equality with different types
            var intStringTuple = Tuple.Create(1, "test");
            var stringIntTuple = Tuple.Create("test", 1);
            
            Console.WriteLine(intStringTuple.Equals(stringIntTuple));    // Output: False (type order differs)

            // Using tuples in sorting
            var unsorted = new List<Tuple<string, int>>
            {
                Tuple.Create("Charlie", 30),
                Tuple.Create("Alice", 25),
                Tuple.Create("Bob", 30),
                Tuple.Create("Alice", 20)
            };

            // Sort by Name (Item1), then by Age (Item2)
            unsorted.Sort();
            
            Console.WriteLine("Sorted tuples:");
            foreach (var t in unsorted)
            {
                Console.WriteLine($"  {t.Item1}, Age: {t.Item2}");
                // Output:
                //   Alice, Age: 20
                //   Alice, Age: 25
                //   Bob, Age: 30
                //   Charlie, Age: 30
            }

            // Tuple as nested key in Dictionary
            var lookup = new Dictionary<Tuple<int, int>, string>
            {
                { Tuple.Create(0, 0), "Origin" },
                { Tuple.Create(1, 0), "Right" },
                { Tuple.Create(0, 1), "Up" },
                { Tuple.Create(1, 1), "Diagonal" }
            };

            var key = Tuple.Create(1, 0);
            Console.WriteLine(lookup[key]);    // Output: Right

            // Multiple nested levels for complex data
            var sensorData = Tuple.Create("Temperature", 
                Tuple.Create("Sensor1", 
                    Tuple.Create(DateTime.Now.AddHours(-1), 22.5),
                    Tuple.Create(DateTime.Now, 23.1)));
            
            Console.WriteLine($"Sensor: {sensorData.Item2.Item1}");    // Output: Sensor: Sensor1
            Console.WriteLine($"  Current: {sensorData.Item2.Item2.Item2}");    // Output:   Current: 23.1
            Console.WriteLine($"  Previous: {sensorData.Item2.Item2.Item1}");    // Output:   Previous: 4/4/2026 8:29:15 AM

            Console.WriteLine();
            Console.WriteLine("=== Real-World Examples ===");
            Console.WriteLine();

            // Real-world Example 1: Geographic coordinates with metadata
            var location = Tuple.Create("Office", 
                Tuple.Create(40.7128, -74.0060),  // Latitude, Longitude
                "New York HQ");
            
            Console.WriteLine($"Location: {location.Item1} ({location.Item3})");
            Console.WriteLine($"  Coordinates: {location.Item2.Item1}, {location.Item2.Item2}");    // Output:   Coordinates: 40.7128, -74.006

            // Real-world Example 2: Product with variants in nested structure
            var product = Tuple.Create("Laptop",
                Tuple.Create("Dell XPS 15",
                    Tuple.Create(" specs ", Tuple.Create("16GB", "512GB SSD", "RTX 3060"))));
            
            Console.WriteLine($"Product Line: {product.Item1}");    // Output: Product Line: Laptop
            Console.WriteLine($"  Model: {product.Item2.Item1}");    // Output:   Model: Dell XPS 15
            Console.WriteLine($"  RAM: {product.Item2.Item2.Item2.Item1}");    // Output:   RAM: 16GB

            // Real-world Example 3: Employee performance review hierarchy
            var q1Review = Tuple.Create("John", 85, "Exceeds Expectations");
            var q2Review = Tuple.Create("John", 88, "Exceeds Expectations");
            var performanceReview = Tuple.Create(2024, Tuple.Create("Q1", q1Review), Tuple.Create("Q2", q2Review));
            
            Console.WriteLine($"Review Year: {performanceReview.Item1}");    // Output: Review Year: 2024
            Console.WriteLine($"  Q1: {performanceReview.Item2.Item2.Item1} - Score: {performanceReview.Item2.Item2.Item2}");    // Output:   Q1: John - Score: 85
        }

        static int CompareTuples<T1, T2, T3>(Tuple<T1, T2, T3> a, Tuple<T1, T2, T3> b) where T1 : IComparable<T1> where T2 : IComparable<T2> where T3 : IComparable<T3>
        {
            int result = a.Item1.CompareTo(b.Item1);
            if (result != 0) return result;
            result = a.Item2.CompareTo(b.Item2);
            if (result != 0) return result;
            return a.Item3.CompareTo(b.Item3);
        }

        static int CompareTuples<T1, T2>(Tuple<T1, T2> a, Tuple<T1, T2> b) where T1 : IComparable<T1> where T2 : IComparable<T2>
        {
            int result = a.Item1.CompareTo(b.Item1);
            if (result != 0) return result;
            return a.Item2.CompareTo(b.Item2);
        }
    }
}