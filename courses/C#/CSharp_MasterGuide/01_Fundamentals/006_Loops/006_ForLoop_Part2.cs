/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Loops - For Loop (Part 2)
 * FILE      : ForLoop_Part2.cs
 * PURPOSE   : This file covers advanced for loop topics including foreach, break/continue,
 *             and performance considerations.
 * ============================================================
 */

// --- SECTION: Advanced For Loop Topics ---
// Covers advanced patterns and performance considerations

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._01_Fundamentals._06_Loops
{
    class ForLoop_Part2
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Break and Continue
            // ═══════════════════════════════════════════════════════════════
            
            // ── Break - exit loop immediately ───────────────────────────────
            Console.WriteLine("=== Break Example ===");
            
            for (int i = 0; i < 10; i++)
            {
                if (i == 5)
                {
                    Console.WriteLine("Breaking at i=5");
                    break; // Exit the loop
                }
                Console.WriteLine($"i = {i}");
            }
            // Output: 0,1,2,3,4, Breaking at i=5
            
            // ── Continue - skip current iteration ───────────────────────
            Console.WriteLine("\n=== Continue Example ===");
            
            for (int i = 0; i < 5; i++)
            {
                if (i == 2)
                {
                    Console.WriteLine("Skipping i=2");
                    continue; // Skip to next iteration
                }
                Console.WriteLine($"Processing i = {i}");
            }
            // Output: 0,1,Skipping i=2,3,4
            
            // ── Practical: Find first match ───────────────────────────────
            int[] numbers = { 3, 7, 2, 9, 5, 8 };
            int? firstEven = null;
            
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] % 2 == 0)
                {
                    firstEven = numbers[i];
                    break; // Found what we need
                }
            }
            
            Console.WriteLine($"First even number: {firstEven}"); // Output: First even number: 2
            
            // ── Practical: Skip invalid data ───────────────────────────────
            var data = new[] { "100", "invalid", "200", "abc", "300" };
            int validSum = 0;
            
            for (int i = 0; i < data.Length; i++)
            {
                if (!int.TryParse(data[i], out int value))
                {
                    Console.WriteLine($"Skipping invalid: {data[i]}");
                    continue; // Skip non-numeric
                }
                validSum += value;
            }
            
            Console.WriteLine($"Valid sum: {validSum}"); // Output: Valid sum: 600

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Return in Loops
            // ═══════════════════════════════════════════════════════════════
            
            // Using return to exit early (when in a method)
            int foundIndex = FindFirstNegative(new[] { 1, 2, 3, -1, 5 });
            Console.WriteLine($"First negative at index: {foundIndex}"); // Output: 3
            
            foundIndex = FindFirstNegative(new[] { 1, 2, 3, 4, 5 });
            Console.WriteLine($"First negative at index: {foundIndex}"); // Output: -1 (not found)

            // ═══════════════════════════════════════════════════════════════
            // SECTION: For vs Foreach Performance
            // ═══════════════════════════════════════════════════════════════
            
            // For loop - slightly faster for arrays due to index access
            int[] arr = Enumerable.Range(1, 1000).ToArray();
            long forSum = 0;
            
            var forWatch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < arr.Length; i++)
            {
                forSum += arr[i];
            }
            forWatch.Stop();
            Console.WriteLine($"For loop: {forWatch.ElapsedTicks} ticks, sum={forSum}");
            
            // Foreach - cleaner syntax, works with any collection
            long foreachSum = 0;
            
            var foreachWatch = System.Diagnostics.Stopwatch.StartNew();
            foreach (int val in arr)
            {
                foreachSum += val;
            }
            foreachWatch.Stop();
            Console.WriteLine($"Foreach: {foreachWatch.ElapsedTicks} ticks, sum={foreachSum}");
            
            // For List<T>, use for with index for best performance
            var list = new List<int>(arr);
            long listSum = 0;
            
            for (int i = 0; i < list.Count; i++)
            {
                listSum += list[i];
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: For Loop with LINQ
            // ═══════════════════════════════════════════════════════════════
            
            // While for gives control, LINQ is often cleaner
            var nums = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            // Filter with LINQ
            var evens = nums.Where(n => n % 2 == 0).ToArray();
            Console.WriteLine($"Evens: {string.Join(",", evens)}");
            
            // Transform
            var doubled = nums.Select(n => n * 2).ToArray();
            Console.WriteLine($"Doubled: {string.Join(",", doubled)}");
            
            // Aggregate
            int sum = nums.Aggregate(0, (acc, n) => acc + n);
            Console.WriteLine($"Sum: {sum}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Infinite Loops with Break
            // ═══════════════════════════════════════════════════════════════
            
            // Useful for waiting loops
            Console.WriteLine("\n=== Infinite Loop Example ===");
            
            int attempts = 0;
            bool success = false;
            
            for (; ; ) // Infinite loop
            {
                attempts++;
                
                // Simulate some condition
                if (attempts >= 3)
                {
                    success = true;
                    break;
                }
            }
            
            Console.WriteLine($"Success after {attempts} attempts");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Multiple Break Levels
            // ═══════════════════════════════════════════════════════════════
            
            // Breaking from nested loops - use labeled break
            Console.WriteLine("\n=== Nested Break ===");
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.WriteLine($"i={i}, j={j}");
                    if (i == 1 && j == 1)
                    {
                        Console.WriteLine("Breaking outer");
                        goto breakOuter; // Or use a flag
                    }
                }
            }
            
            breakOuter:
            Console.WriteLine("Broke out of both loops");
            
            // Better: use a flag
            bool found = false;
            for (int i = 0; i < 3 && !found; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == 1 && j == 1)
                    {
                        found = true;
                        break;
                    }
                }
            }
            
            Console.WriteLine("Used flag to break");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── Searching in grid ──────────────────────────────────────────
            int[,] grid = {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            };
            
            int target = 5;
            (int row, int col)? position = null;
            
            for (int i = 0; i < grid.GetLength(0) && position == null; i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == target)
                    {
                        position = (i, j);
                        break;
                    }
                }
            }
            
            Console.WriteLine($"Found {target} at [{position?.row},{position?.col}]");
            
            // ── Building query string ─────────────────────────────────────
            var queryParams = new Dictionary<string, string>
            {
                ["page"] = "1",
                ["limit"] = "10",
                ["sort"] = "name"
            };
            
            string query = "?";
            var keys = queryParams.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                string key = keys[i];
                query += key + "=" + queryParams[key];
                if (i < keys.Length - 1)
                    query += "&";
            }
            
            Console.WriteLine($"Query: {query}"); // Output: ?page=1&limit=10&sort=name
        }
        
        // Method with early return in loop
        static int FindFirstNegative(int[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] < 0)
                    return i; // Early return
            }
            return -1; // Not found
        }
    }
}
