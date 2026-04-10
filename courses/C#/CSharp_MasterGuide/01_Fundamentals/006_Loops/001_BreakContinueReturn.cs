/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Loops - Break, Continue, and Return
 * FILE      : BreakContinueReturn.cs
 * PURPOSE   : This file covers control flow statements that alter loop execution:
 *             break (exit loop), continue (skip iteration), and return (exit method).
 * ============================================================
 */

// --- SECTION: Loop Control Statements ---
// Break, continue, and return control how loops execute
// They allow early exit, skipping iterations, and method return

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._01_Fundamentals._06_Loops
{
    class BreakContinueReturn
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Break Statement
            // ═══════════════════════════════════════════════════════════════
            
            // Break - immediately exits the innermost loop
            Console.WriteLine("=== Break Examples ===");
            
            // Find first even number
            int[] numbers = { 1, 3, 5, 8, 9, 11 };
            int? firstEven = null;
            
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] % 2 == 0)
                {
                    firstEven = numbers[i];
                    Console.WriteLine($"First even: {firstEven}");
                    break; // Exit loop once found
                }
            }
            // Output: First even: 8
            
            // Break in nested loops - exits only inner loop by default
            int[,] matrix = {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            };
            
            Console.WriteLine("\n=== Nested Break ===");
            
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Console.Write($"{matrix[row, col]} ");
                    
                    if (matrix[row, col] == 5)
                    {
                        Console.WriteLine("(found 5!)");
                        break; // Only exits inner loop
                    }
                }
            }
            // Output shows row 1 stops at 5, row 2 still processes
            
            // Exit outer loop using flag
            bool found = false;
            
            for (int i = 0; i < 3 && !found; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[i, j] == 7)
                    {
                        Console.WriteLine($"\nFound 7 at [{i},{j}]");
                        found = true;
                        break;
                    }
                }
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Continue Statement
            // ═══════════════════════════════════════════════════════════════
            
            // Continue - skips to next iteration
            Console.WriteLine("\n=== Continue Examples ===");
            
            // Skip even numbers
            for (int i = 1; i <= 10; i++)
            {
                if (i % 2 == 0)
                    continue; // Skip even numbers
                
                Console.WriteLine($"Odd: {i}");
            }
            // Output: 1, 3, 5, 7, 9
            
            // Filter collection
            var items = new[] { "apple", "banana", "CHERRY", "date", "elderberry" };
            var filtered = new List<string>();
            
            foreach (string item in items)
            {
                if (item.Length < 6)
                    continue; // Skip short names
                
                filtered.Add(item);
                Console.WriteLine($"Added: {item}");
            }
            // Output: Added: CHERRY, Added: elderberry
            
            // Continue with nested loops
            Console.WriteLine("\n=== Continue in Nested ===");
            
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    if (j == 2)
                        continue; // Skip j=2
                    
                    Console.WriteLine($"({i},{j})");
                }
            }
            // Output shows pairs except where j=2

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Return in Loops
            // ═══════════════════════════════════════════════════════════════
            
            // Return - exits the entire method
            Console.WriteLine("\n=== Return Examples ===");
            
            // Find in method and return early
            int result = FindFirstDivisibleBy(3, new[] { 2, 4, 6, 9, 12 });
            Console.WriteLine($"First divisible by 3: {result}"); // Output: 6
            
            result = FindFirstDivisibleBy(7, new[] { 2, 4, 6, 9, 12 });
            Console.WriteLine($"First divisible by 7: {result}"); // Output: -1 (not found)
            
            // Return from lambda in loop
            var numbers = new[] { 1, 2, 3, 4, 5 };
            
            // Note: Can't use return in foreach lambda easily
            // Instead use FirstOrDefault
            int first = numbers.FirstOrDefault(n => n > 3);
            Console.WriteLine($"First > 3: {first}"); // Output: 4

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Goto (Alternative to Break)
            // ═══════════════════════════════════════════════════════════════
            
            // Goto can exit multiple loops (not recommended but possible)
            Console.WriteLine("\n=== Goto Example ===");
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i * j > 4)
                    {
                        Console.WriteLine($"Breaking with goto at i={i}, j={j}");
                        goto exitLoops; // Jump outside both loops
                    }
                }
            }
            
            exitLoops:
            Console.WriteLine("Exited loops");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── Validation loop ───────────────────────────────────────────
            Console.WriteLine("\n=== Validation ===");
            
            var formData = new Dictionary<string, string>
            {
                ["username"] = "john",
                ["email"] = "john@example.com",
                ["age"] = "25"
            };
            
            bool isValid = true;
            string firstError = null;
            
            foreach (var field in formData)
            {
                if (string.IsNullOrWhiteSpace(field.Value))
                {
                    isValid = false;
                    firstError = $"{field.Key} is required";
                    break; // Stop at first validation error
                }
            }
            
            Console.WriteLine(isValid ? "Form is valid" : $"Error: {firstError}");
            
            // ── Processing with skip ────────────────────────────────────────
            Console.WriteLine("\n=== Processing ===");
            
            var orders = new[] {
                (Id: 1, Status: "complete"),
                (Id: 2, Status: "pending"),
                (Id: 3, Status: "complete"),
                (Id: 4, Status: "cancelled")
            };
            
            int processed = 0;
            
            foreach (var order in orders)
            {
                if (order.Status != "complete")
                    continue; // Skip non-complete orders
                
                // Process order
                processed++;
                Console.WriteLine($"Processing order {order.Id}");
            }
            
            Console.WriteLine($"Processed {processed} orders");
            
            // ── Early termination in search ────────────────────────────────
            var database = new List<string> { 
                "user1", "user2", "target", "user4", "user5" 
            };
            
            string target = "target";
            bool exists = false;
            
            foreach (string user in database)
            {
                if (user == target)
                {
                    exists = true;
                    break; // No need to continue
                }
            }
            
            Console.WriteLine($"User exists: {exists}");
            
            // ── Menu with return ──────────────────────────────────────────
            ProcessMenu();
        }
        
        // ═══════════════════════════════════════════════════════════════
        // Methods demonstrating return in loops
        // ═══════════════════════════════════════════════════════════════
        
        // Return from method when condition met
        static int FindFirstDivisibleBy(int divisor, int[] numbers)
        {
            foreach (int num in numbers)
            {
                if (num % divisor == 0)
                    return num; // Return immediately when found
            }
            
            return -1; // Not found
        }
        
        // Menu processing with return
        static void ProcessMenu()
        {
            Console.WriteLine("\n=== Menu ===");
            
            int choice = 1; // Simulated input
            
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Processing option 1");
                    return;
                case 2:
                    Console.WriteLine("Processing option 2");
                    return;
                default:
                    Console.WriteLine("Unknown option");
                    return;
            }
        }
    }
}
