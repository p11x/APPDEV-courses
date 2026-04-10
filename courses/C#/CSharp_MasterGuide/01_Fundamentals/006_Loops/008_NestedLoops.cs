/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Loops - Nested Loops
 * FILE      : NestedLoops.cs
 * PURPOSE   : This file covers nested loops in C#, including common patterns,
 *             performance considerations, and practical applications.
 * ============================================================
 */

// --- SECTION: Nested Loops ---
// Nested loops are loops within loops - used for multi-dimensional data,
// matrices, combinations, and more complex iterations

using System;

namespace CSharp_MasterGuide._01_Fundamentals._06_Loops
{
    class NestedLoops
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Basic Nested Loops
            // ═══════════════════════════════════════════════════════════════
            
            // 2x3 table - outer loop for rows, inner for columns
            Console.WriteLine("=== 2x3 Multiplication Table ===");
            
            for (int i = 1; i <= 2; i++) // outer - rows
            {
                for (int j = 1; j <= 3; j++) // inner - columns
                {
                    Console.Write($"{i * j}\t");
                }
                Console.WriteLine(); // newline after each row
            }
            // Output:
            // 1	2	3	
            // 2	4	6
            
            // ── Triangle pattern ───────────────────────────────────────────
            Console.WriteLine("\n=== Triangle Pattern ===");
            
            for (int row = 1; row <= 5; row++)
            {
                for (int col = 1; col <= row; col++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
            // Output:
            // *
            // **
            // ***
            // ****
            // *****
            
            // ── Inverted triangle ─────────────────────────────────────────
            Console.WriteLine("\n=== Inverted Triangle ===");
            
            for (int row = 5; row >= 1; row--)
            {
                for (int col = 1; col <= row; col++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Multi-Dimensional Arrays
            // ═══════════════════════════════════════════════════════════════
            
            // 2D array iteration
            int[,] matrix = {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            };
            
            Console.WriteLine("\n=== 2D Array ===");
            
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    Console.Write($"{matrix[row, col]}\t");
                }
                Console.WriteLine();
            }
            
            // ── 3D array ───────────────────────────────────────────────────
            int[,,] cube = new int[2, 2, 2];
            int value = 1;
            
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int z = 0; z < 2; z++)
                    {
                        cube[x, y, z] = value++;
                    }
                }
            }
            
            Console.WriteLine("\n=== 3D Array ===");
            
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int z = 0; z < 2; z++)
                    {
                        Console.WriteLine($"cube[{x},{y},{z}] = {cube[x, y, z]}");
                    }
                }
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Practical Nested Loop Patterns
            // ═══════════════════════════════════════════════════════════════
            
            // ── Find all pairs ─────────────────────────────────────────────
            int[] numbers = { 1, 2, 3 };
            
            Console.WriteLine("\n=== All Pairs ===");
            
            for (int i = 0; i < numbers.Length; i++)
            {
                for (int j = 0; j < numbers.Length; j++)
                {
                    Console.WriteLine($"({numbers[i]}, {numbers[j]})");
                }
            }
            
            // ── Unique pairs (no duplicates) ─────────────────────────────
            Console.WriteLine("\n=== Unique Pairs ===");
            
            for (int i = 0; i < numbers.Length; i++)
            {
                for (int j = i + 1; j < numbers.Length; j++)
                {
                    Console.WriteLine($"({numbers[i]}, {numbers[j]})");
                }
            }
            // Output: (1,2), (1,3), (2,3)
            
            // ── Find combinations ─────────────────────────────────────────
            string[] colors = { "Red", "Green", "Blue" };
            string[] sizes = { "Small", "Large" };
            
            Console.WriteLine("\n=== Product Combinations ===");
            
            foreach (string color in colors)
            {
                foreach (string size in sizes)
                {
                    Console.WriteLine($"{color} {size}");
                }
            }
            // Output: Red Small, Red Green, etc. (6 combinations)
            
            // ── Cartesian product ──────────────────────────────────────────
            string[] types = { "T-Shirt", "Pants" };
            var combinations = new List<(string, string, string)>();
            
            foreach (string color in colors)
            {
                foreach (string size in sizes)
                {
                    foreach (string type in types)
                    {
                        combinations.Add((color, size, type));
                    }
                }
            }
            
            Console.WriteLine($"\nTotal combinations: {combinations.Count}"); // 12

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Searching in Nested Structures
            // ═══════════════════════════════════════════════════════════════
            
            // ── Matrix search ───────────────────────────────────────────────
            int[,] grid = {
                { 1, 5, 9 },
                { 3, 7, 2 },
                { 8, 4, 6 }
            };
            
            int target = 7;
            (int row, int col)? found = null;
            
            for (int r = 0; r < grid.GetLength(0) && found == null; r++)
            {
                for (int c = 0; c < grid.GetLength(1); c++)
                {
                    if (grid[r, c] == target)
                    {
                        found = (r, c);
                        break;
                    }
                }
            }
            
            Console.WriteLine($"\nFound {target} at [{found?.row}, {found?.col}]"); // [1,1]
            
            // ── Find maximum in matrix ─────────────────────────────────────
            int maxVal = grid[0, 0];
            (int maxRow, int maxCol) = (0, 0);
            
            for (int r = 0; r < grid.GetLength(0); r++)
            {
                for (int c = 0; c < grid.GetLength(1); c++)
                {
                    if (grid[r, c] > maxVal)
                    {
                        maxVal = grid[r, c];
                        maxRow = r;
                        maxCol = c;
                    }
                }
            }
            
            Console.WriteLine($"Max value {maxVal} at [{maxRow}, {maxCol}]"); // 9 at [0,2]

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Break and Continue in Nested Loops
            // ═══════════════════════════════════════════════════════════════
            
            // Breaking out of all loops - use flag
            bool found2 = false;
            
            for (int i = 0; i < 3 && !found2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i * j > 4)
                    {
                        Console.WriteLine($"Found at i={i}, j={j}");
                        found2 = true;
                        break;
                    }
                }
            }
            
            // Continue to next iteration of outer loop
            Console.WriteLine("\n=== Continue in Nested ===");
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (j == 1)
                        continue; // Skip j=1
                    
                    Console.WriteLine($"i={i}, j={j}");
                }
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── Calendar generation ───────────────────────────────────────
            int daysInMonth = 30;
            int startDayOfWeek = 3; // Thursday (0=Sunday)
            
            Console.WriteLine("\n=== Calendar ===");
            Console.WriteLine("Su Mo Tu We Th Fr Sa");
            
            // Print leading spaces for first week
            for (int i = 0; i < startDayOfWeek; i++)
                Console.Write("   ");
            
            for (int day = 1; day <= daysInMonth; day++)
            {
                Console.Write($"{day,2} ");
                
                if ((day + startDayOfWeek) % 7 == 0)
                    Console.WriteLine();
            }
            
            Console.WriteLine();
            
            // ── Score table with totals ─────────────────────────────────────
            string[] subjects = { "Math", "Science", "English" };
            string[] students = { "Alice", "Bob" };
            int[,] scores = {
                { 85, 90, 88 },
                { 78, 82, 91 }
            };
            
            Console.WriteLine("\n=== Score Table ===");
            
            // Print header
            Console.Write("Student\t");
            foreach (string subject in subjects)
                Console.Write($"{subject}\t");
            Console.WriteLine("Total");
            
            // Print student scores
            for (int s = 0; s < students.Length; s++)
            {
                int total = 0;
                
                Console.Write($"{students[s]}\t");
                
                for (int sub = 0; sub < subjects.Length; sub++)
                {
                    Console.Write($"{scores[s, sub]}\t");
                    total += scores[s, sub];
                }
                
                Console.WriteLine(total);
            }
            
            // Print subject averages
            Console.Write("Average\t");
            
            for (int sub = 0; sub < subjects.Length; sub++)
            {
                int sum = 0;
                
                for (int s = 0; s < students.Length; s++)
                    sum += scores[s, sub];
                
                Console.Write($"{sum / students.Length}\t");
            }
            
            Console.WriteLine();
        }
    }
}
