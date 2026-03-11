/*
================================================================================
TOPIC 22: LINQ (Language Integrated Query)
================================================================================

LINQ provides SQL-like querying capabilities in C#.

TABLE OF CONTENTS:
1. LINQ Basics
2. Query Syntax
3. Method Syntax
4. Common Operations
================================================================================
*/

using System;
using System.Linq;
using System.Collections.Generic;

namespace LINQExamples
{
    class Program
    {
        static void Main()
        {
            // Sample data
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            List<string> names = new List<string> { "Alice", "Bob", "Charlie", "David", "Eve" };
            
            // WHERE - Filter
            Console.WriteLine("=== WHERE ===");
            var evens = numbers.Where(n => n % 2 == 0);
            Console.WriteLine(string.Join(", ", evens));
            
            // SELECT - Transform
            Console.WriteLine("\n=== SELECT ===");
            var doubled = numbers.Select(n => n * 2);
            Console.WriteLine(string.Join(", ", doubled));
            
            // ORDER BY
            Console.WriteLine("\n=== ORDER BY ===");
            var sortedDesc = numbers.OrderByDescending(n => n);
            Console.WriteLine(string.Join(", ", sortedDesc));
            
            // FIRST / FIRST OR DEFAULT
            Console.WriteLine("\n=== FIRST ===");
            Console.WriteLine(numbers.First(n => n > 5));
            
            // COUNT / SUM / AVERAGE / MIN / MAX
            Console.WriteLine("\n=== Aggregates ===");
            Console.WriteLine($"Count: {numbers.Count}");
            Console.WriteLine($"Sum: {numbers.Sum()}");
            Console.WriteLine($"Average: {numbers.Average():F2}");
            Console.WriteLine($"Min: {numbers.Min()}");
            Console.WriteLine($"Max: {numbers.Max()}");
            
            // TAKE / SKIP
            Console.WriteLine("\n=== TAKE / SKIP ===");
            var first3 = numbers.Take(3);
            var skip3 = numbers.Skip(3);
            Console.WriteLine($"First 3: {string.Join(", ", first3)}");
            Console.WriteLine($"Skip 3: {string.Join(", ", skip3)}");
            
            // ANY / ALL
            Console.WriteLine("\n=== ANY / ALL ===");
            Console.WriteLine($"Any > 5: {numbers.Any(n => n > 5)}");
            Console.WriteLine($"All > 0: {numbers.All(n => n > 0)}");
            
            // Query syntax
            Console.WriteLine("\n=== Query Syntax ===");
            var query = from n in numbers
                        where n > 5
                        orderby n descending
                        select n * 2;
            Console.WriteLine(string.Join(", ", query));
        }
    }
}

/*
LINQ METHODS:
-------------
Where()       - Filter
Select()      - Transform
OrderBy()     - Sort ascending
OrderByDescending() - Sort descending
First()       - First element
FirstOrDefault() - First or default
Single()      - Only element
Any()         - Any match
All()         - All match
Count()       - Count
Sum()         - Sum
Average()     - Average
Min()         - Minimum
Max()         - Maximum
Take()        - Take first n
Skip()        - Skip first n
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 23 covers Delegates.
*/
