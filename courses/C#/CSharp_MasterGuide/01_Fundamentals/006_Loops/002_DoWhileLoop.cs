/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Loops - Do-While Loop
 * FILE      : DoWhileLoop.cs
 * PURPOSE   : This file covers the do-while loop in C#, which executes at least once
 *             before checking the condition.
 * ============================================================
 */

// --- SECTION: Do-While Loops ---
// Do-while executes the body at least once, then checks condition
// Unlike while, it's guaranteed to run at least one iteration

using System;

namespace CSharp_MasterGuide._01_Fundamentals._06_Loops
{
    class DoWhileLoop
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Basic Do-While Loop
            // ═══════════════════════════════════════════════════════════════
            
            // Basic do-while - always runs at least once
            int count = 0;
            
            do
            {
                Console.WriteLine($"Count: {count}");
                count++;
            }
            while (count < 5);
            // Output: 0,1,2,3,4 (guaranteed at least once!)
            
            // Compare with while - might not run at all
            count = 10;
            
            Console.WriteLine("\n=== While (might not run) ===");
            while (count < 5)
            {
                Console.WriteLine($"While: {count}");
                count++;
            }
            Console.WriteLine("(nothing printed - condition was false from start)");
            
            Console.WriteLine("\n=== Do-While (runs at least once) ===");
            count = 10;
            
            do
            {
                Console.WriteLine($"Do-While: {count}");
                count++;
            }
            while (count < 5);
            // Output: Do-While: 10 (runs once even though condition is false!)

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Do-While with Input Validation
            // ═══════════════════════════════════════════════════════════════
            
            // Classic use: validate input - must get valid input at least once
            Console.WriteLine("\n=== Input Validation ===");
            
            int userAge = 0;
            bool validInput = false;
            
            do
            {
                Console.Write("Enter age (13-120): ");
                // In real code: userAge = int.Parse(Console.ReadLine());
                userAge = 15; // Simulated input
                
                if (userAge >= 13 && userAge <= 120)
                {
                    validInput = true;
                    Console.WriteLine($"Valid age: {userAge}");
                }
                else
                {
                    Console.WriteLine("Invalid age. Try again.");
                }
            }
            while (!validInput);
            
            // Another example: menu selection
            string choice = "";
            
            do
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Start");
                Console.WriteLine("2. Settings");
                Console.WriteLine("3. Quit");
                
                // In real code: choice = Console.ReadLine();
                choice = "3"; // Simulated
                
                if (choice == "1")
                    Console.WriteLine("Starting...");
                else if (choice == "2")
                    Console.WriteLine("Settings...");
            }
            while (choice != "3");
            
            Console.WriteLine("Exited menu");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Do-While with Break
            // ═══════════════════════════════════════════════════════════════
            
            // Using break to exit
            Console.WriteLine("\n=== Do-While with Break ===");
            
            int number = 1;
            
            do
            {
                Console.WriteLine($"Processing {number}");
                number++;
                
                if (number > 5)
                {
                    Console.WriteLine("Reached limit");
                    break;
                }
            }
            while (true); // Infinite loop, but breaks from inside

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── Retry mechanism ───────────────────────────────────────────
            Console.WriteLine("\n=== Retry Mechanism ===");
            
            int attempt = 0;
            int maxAttempts = 3;
            bool success = false;
            
            do
            {
                attempt++;
                Console.WriteLine($"Attempt {attempt} of {maxAttempts}...");
                
                // Simulate operation that might fail
                if (attempt >= 2)
                {
                    success = true;
                    Console.WriteLine("Operation succeeded!");
                }
                else
                {
                    Console.WriteLine("Failed. Retrying...");
                }
            }
            while (!success && attempt < maxAttempts);
            
            if (!success)
                Console.WriteLine("All attempts failed");
            
            // ── Game loop ─────────────────────────────────────────────────
            Console.WriteLine("\n=== Game Loop ===");
            
            bool gameRunning = true;
            int tick = 0;
            
            do
            {
                tick++;
                Console.WriteLine($"Game tick {tick}");
                
                // In real game: update, render
                if (tick >= 3)
                {
                    Console.WriteLine("Game over!");
                    gameRunning = false;
                }
            }
            while (gameRunning);
            
            // ── Batch processing ──────────────────────────────────────────
            Console.WriteLine("\n=== Batch Processing ===");
            
            var batch = new Queue<string>();
            batch.Enqueue("Item A");
            batch.Enqueue("Item B");
            batch.Enqueue("Item C");
            
            int processedCount = 0;
            
            do
            {
                var item = batch.Dequeue();
                Console.WriteLine($"Processing: {item}");
                processedCount++;
            }
            while (batch.Count > 0);
            
            Console.WriteLine($"Processed {processedCount} items");
            
            // ── File read simulation ───────────────────────────────────────
            Console.WriteLine("\n=== File Read ===");
            
            var simulatedLines = new[] { "Header", "Line 1", "Line 2", "Line 3", "EOF" };
            int lineNum = 0;
            
            string line;
            
            do
            {
                line = lineNum < simulatedLines.Length ? simulatedLines[lineNum] : null;
                
                if (line != null && line != "EOF")
                {
                    Console.WriteLine($"Read: {line}");
                }
                
                lineNum++;
            }
            while (line != null && line != "EOF");
            
            Console.WriteLine("End of file");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Do-While vs While Selection
            // ═══════════════════════════════════════════════════════════════
            
            // Use do-when:
            // - Need to execute at least once
            // - Input validation (must validate at least once)
            // - Menu selection (show menu at least once)
            // - Retry operations (try at least once)
            
            // Use while when:
            // - Condition might be false initially
            // - Don't need initial execution
            // - Iterating over collections (might be empty)
        }
    }
}
