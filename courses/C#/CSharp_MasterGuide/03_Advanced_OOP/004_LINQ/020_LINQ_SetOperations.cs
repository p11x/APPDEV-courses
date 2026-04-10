/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Set Operations
 * FILE: LINQ_SetOperations.cs
 * PURPOSE: Union, Intersect, Except, Distinct set operations
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_SetOperations
    {
        public static void Main()
        {
            // Union - combine two sequences, removing duplicates
            Console.WriteLine("=== Union ===");
            
            var set1 = new List<int> { 1, 2, 3, 4, 5 };
            var set2 = new List<int> { 4, 5, 6, 7, 8 };
            
            var union = set1.Union(set2);
            
            Console.WriteLine("Union of sets:");
            foreach (var n in union)
            {
                Console.WriteLine(n); // Output: 1, 2, 3, 4, 5, 6, 7, 8
            }
            
            // Union with custom comparer
            var words1 = new List<string> { "apple", "banana", "cherry" };
            var words2 = new List<string> { "APPLE", "BANANA", "date" };
            
            var unionIgnoreCase = words1.Union(words2, StringComparer.OrdinalIgnoreCase);
            
            Console.WriteLine("\nUnion (case-insensitive):");
            foreach (var w in unionIgnoreCase)
            {
                Console.WriteLine(w); // Output: apple, banana, cherry, date
            }

            // Intersect - elements in both sequences
            Console.WriteLine("\n=== Intersect ===");
            
            var intersect = set1.Intersect(set2);
            
            Console.WriteLine("Intersection:");
            foreach (var n in intersect)
            {
                Console.WriteLine(n); // Output: 4, 5
            }
            
            // Intersect with custom comparer
            var commonWords = words1.Intersect(words2, StringComparer.OrdinalIgnoreCase);
            
            Console.WriteLine("\nIntersection (case-insensitive):");
            foreach (var w in commonWords)
            {
                Console.WriteLine(w); // Output: apple, banana
            }

            // Except - elements in first but not in second
            Console.WriteLine("\n=== Except ===");
            
            var except = set1.Except(set2);
            
            Console.WriteLine("Set1 except Set2:");
            foreach (var n in except)
            {
                Console.WriteLine(n); // Output: 1, 2, 3
            }
            
            var except2 = set2.Except(set1);
            
            Console.WriteLine("\nSet2 except Set1:");
            foreach (var n in except2)
            {
                Console.WriteLine(n); // Output: 6, 7, 8
            }

            // Distinct - remove duplicates (already covered but for completeness)
            Console.WriteLine("\n=== Distinct ===");
            
            var withDupes = new List<int> { 1, 2, 2, 3, 3, 3, 4, 4, 4, 4 };
            var unique = withDupes.Distinct();
            
            Console.WriteLine("Distinct values:");
            foreach (var n in unique)
            {
                Console.WriteLine(n); // Output: 1, 2, 3, 4
            }

            // SequenceEqual - check if sequences are equal
            Console.WriteLine("\n=== SequenceEqual ===");
            
            var a = new List<int> { 1, 2, 3 };
            var b = new List<int> { 1, 2, 3 };
            var c = new List<int> { 1, 2, 4 };
            
            Console.WriteLine($"a equals b: {a.SequenceEqual(b)}"); // Output: True
            Console.WriteLine($"a equals c: {a.SequenceEqual(c)}"); // Output: False
            
            // With case-insensitive
            var s1 = new List<string> { "hello" };
            var s2 = new List<string> { "HELLO" };
            Console.WriteLine($"hello equals HELLO: {s1.SequenceEqual(s2, StringComparer.OrdinalIgnoreCase)}"); // Output: True

            // REAL WORLD EXAMPLE: Find common customers between regions
            Console.WriteLine("\n=== Real World: Common Customers ===");
            
            var northCustomers = new List<string> { "John", "Jane", "Bob", "Alice" };
            var southCustomers = new List<string> { "Jane", "Alice", "Charlie", "Diana" };
            var eastCustomers = new List<string> { "Alice", "Bob", "Eve", "Frank" };
            
            // Find customers in all three regions
            var allRegions = northCustomers.Intersect(southCustomers).Intersect(eastCustomers);
            
            Console.WriteLine("Customers in all regions:");
            foreach (var name in allRegions)
            {
                Console.WriteLine($"  {name}"); // Output: Alice
            }

            // REAL WORLD EXAMPLE: Unique products in order
            Console.WriteLine("\n=== Real World: Unique Order Items ===");
            
            var order1Items = new List<string> { "Laptop", "Mouse", "Keyboard" };
            var order2Items = new List<string> { "Mouse", "Keyboard", "Monitor" };
            var order3Items = new List<string> { "Laptop", "Mouse", "Headset" };
            
            // All unique items ordered across all orders
            var allUniqueItems = order1Items.Union(order2Items).Union(order3Items).Distinct();
            
            Console.WriteLine("All unique items ordered:");
            foreach (var item in allUniqueItems)
            {
                Console.WriteLine($"  {item}"); 
                // Output: Laptop, Mouse, Keyboard, Monitor, Headset
            }

            // REAL WORLD EXAMPLE: Products available in one store but not another
            Console.WriteLine("\n=== Real World: Store Inventory Difference ===");
            
            var storeAProducts = new List<string> { "Laptop", "Mouse", "Keyboard", "Monitor" };
            var storeBProducts = new List<string> { "Mouse", "Keyboard", "Headset", "Webcam" };
            
            var onlyInA = storeAProducts.Except(storeBProducts);
            var onlyInB = storeBProducts.Except(storeAProducts);
            var inBoth = storeAProducts.Intersect(storeBProducts);
            
            Console.WriteLine("Store A only:");
            foreach (var p in onlyInA)
            {
                Console.WriteLine($"  {p}"); // Output: Laptop, Monitor
            }
            
            Console.WriteLine("\nStore B only:");
            foreach (var p in onlyInB)
            {
                Console.WriteLine($"  {p}"); // Output: Headset, Webcam
            }
            
            Console.WriteLine("\nBoth stores:");
            foreach (var p in inBoth)
            {
                Console.WriteLine($"  {p}"); // Output: Mouse, Keyboard
            }
        }
    }
}