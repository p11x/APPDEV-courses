/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Loops - For Loop (Part 1)
 * FILE      : ForLoop.cs
 * PURPOSE   : This file covers the for loop in C#, including basic iteration, initialization,
 *             condition, increment, and common patterns.
 * ============================================================
 */

// --- SECTION: For Loops ---
// For loops provide a compact way to iterate a specific number of times
// Syntax: for (init; condition; increment) { statement(s) }

using System;

namespace CSharp_MasterGuide._01_Fundamentals._06_Loops
{
    class ForLoop
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Basic For Loop
            // ═══════════════════════════════════════════════════════════════
            
            // Classic for loop: 0 to 9 (10 iterations)
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Iteration: {i}");
            }
            // Output: 0, 1, 2, 3, 4, 5, 6, 7, 8, 9
            
            // ── Loop from 1 to 10 ─────────────────────────────────────────
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine($"Count: {i}");
            }
            // Output: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            
            // ── Loop with different increments ─────────────────────────────
            // Count by 2
            for (int i = 0; i < 10; i += 2)
            {
                Console.WriteLine($"Even: {i}");
            }
            // Output: 0, 2, 4, 6, 8
            
            // Count backwards
            for (int i = 10; i > 0; i--)
            {
                Console.WriteLine($"Countdown: {i}");
            }
            // Output: 10, 9, 8, 7, 6, 5, 4, 3, 2, 1

            // ═══════════════════════════════════════════════════════════════
            // SECTION: For Loop Components
            // ═══════════════════════════════════════════════════════════════
            
            // ── Multiple initializations ──────────────────────────────────
            for (int i = 0, j = 10; i < j; i++, j--)
            {
                Console.WriteLine($"i={i}, j={j}");
            }
            // Output: i=0,j=10; i=1,j=9; i=2,j=8; etc.
            
            // ── Complex conditions ─────────────────────────────────────────
            for (int i = 0, sum = 0; i <= 100 && sum < 50; i += 10)
            {
                sum += i;
                Console.WriteLine($"i={i}, sum={sum}");
            }
            
            // ── Empty for loop (infinite) ──────────────────────────────────
            // for (; ; ) { } - runs forever unless broken

            // ═══════════════════════════════════════════════════════════════
            // SECTION: For Loop with Arrays
            // ═══════════════════════════════════════════════════════════════
            
            // Iterate array by index
            string[] colors = { "Red", "Green", "Blue" };
            
            for (int i = 0; i < colors.Length; i++)
            {
                Console.WriteLine($"Color {i}: {colors[i]}");
            }
            // Output: Color 0: Red, Color 1: Green, Color 2: Blue
            
            // Reverse array iteration
            for (int i = colors.Length - 1; i >= 0; i--)
            {
                Console.WriteLine($"Reverse: {colors[i]}");
            }
            
            // Iterate with step
            int[] numbers = { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90 };
            for (int i = 0; i < numbers.Length; i += 3)
            {
                Console.WriteLine($"Step 3: {numbers[i]}");
            }
            // Output: 0, 30, 60, 90

            // ═══════════════════════════════════════════════════════════════
            // SECTION: For Loop with Collections
            // ═══════════════════════════════════════════════════════════════
            
            // Using List
            var list = new List<string> { "Apple", "Banana", "Cherry" };
            
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = list[i].ToUpper(); // Modify in place
            }
            
            // Using Dictionary
            var dict = new Dictionary<string, int>
            {
                ["One"] = 1,
                ["Two"] = 2,
                ["Three"] = 3
            };
            
            string[] keys = new string[dict.Count];
            dict.Keys.CopyTo(keys, 0);
            
            for (int i = 0; i < keys.Length; i++)
            {
                string key = keys[i];
                Console.WriteLine($"Key: {key}, Value: {dict[key]}");
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Common For Loop Patterns
            // ═══════════════════════════════════════════════════════════════
            
            // ── Sum calculation ───────────────────────────────────────────
            int[] values = { 10, 20, 30, 40, 50 };
            int sum = 0;
            
            for (int i = 0; i < values.Length; i++)
            {
                sum += values[i];
            }
            
            Console.WriteLine($"Sum: {sum}"); // Output: Sum: 150
            
            // ── Find maximum ───────────────────────────────────────────────
            int[] scores = { 85, 92, 78, 95, 88 };
            int max = scores[0];
            
            for (int i = 1; i < scores.Length; i++)
            {
                if (scores[i] > max)
                    max = scores[i];
            }
            
            Console.WriteLine($"Max: {max}"); // Output: Max: 95
            
            // ── Count matches ─────────────────────────────────────────────
            string[] names = { "Alice", "Bob", "Alice", "Charlie", "Alice" };
            int aliceCount = 0;
            
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i] == "Alice")
                    aliceCount++;
            }
            
            Console.WriteLine($"Alice appears {aliceCount} times"); // Output: 3
            
            // ── Fibonacci sequence ─────────────────────────────────────────
            int n1 = 0, n2 = 1, n3;
            Console.Write("Fibonacci: ");
            
            for (int i = 0; i < 10; i++)
            {
                Console.Write(n1 + " ");
                n3 = n1 + n2;
                n1 = n2;
                n2 = n3;
            }
            Console.WriteLine(); // Output: 0 1 1 2 3 5 8 13 21 34

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Nested For Loops
            // ═══════════════════════════════════════════════════════════════
            
            // Multiplication table
            Console.WriteLine("\nMultiplication Table:");
            
            for (int i = 1; i <= 5; i++)
            {
                string row = "";
                for (int j = 1; j <= 5; j++)
                {
                    row += $"{i * j,4}"; // Format with width
                }
                Console.WriteLine(row);
            }
            // Output: 5x5 multiplication table

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── Pagination ───────────────────────────────────────────────
            int totalRecords = 100;
            int pageSize = 10;
            int currentPage = 3;
            
            int startIndex = (currentPage - 1) * pageSize;
            int endIndex = Math.Min(startIndex + pageSize, totalRecords);
            
            Console.WriteLine($"Showing records {startIndex + 1} to {endIndex} of {totalRecords}");
            
            // ── File processing simulation ───────────────────────────────
            var fileNames = new[] { "file1.txt", "file2.txt", "file3.txt" };
            
            for (int i = 0; i < fileNames.Length; i++)
            {
                Console.WriteLine($"Processing {i + 1}/{fileNames.Length}: {fileNames[i]}");
            }
        }
    }
}
