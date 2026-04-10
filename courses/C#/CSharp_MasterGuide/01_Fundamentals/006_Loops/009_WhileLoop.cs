/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Loops - While Loop
 * FILE      : WhileLoop.cs
 * PURPOSE   : This file covers the while loop in C#, including basic usage, condition checking,
 *             and common patterns for indefinite iteration.
 * ============================================================
 */

// --- SECTION: While Loops ---
// While loops repeat as long as a condition is true
// Condition is checked before each iteration

using System;

namespace CSharp_MasterGuide._01_Fundamentals._06_Loops
{
    class WhileLoop
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Basic While Loop
            // ═══════════════════════════════════════════════════════════════
            
            // Basic while - runs while condition is true
            int count = 0;
            
            while (count < 5)
            {
                Console.WriteLine($"Count: {count}");
                count++; // Must update to avoid infinite loop
            }
            // Output: 0, 1, 2, 3, 4
            
            // ── Count up ──────────────────────────────────────────────────
            int number = 1;
            
            while (number <= 10)
            {
                Console.WriteLine(number);
                number++;
            }
            // Output: 1 through 10
            
            // ── Count down ─────────────────────────────────────────────────
            int countdown = 5;
            
            while (countdown > 0)
            {
                Console.WriteLine($"T-minus {countdown}");
                countdown--;
            }
            
            Console.WriteLine("Liftoff!");
            // Output: T-minus 5,4,3,2,1, Liftoff!

            // ═══════════════════════════════════════════════════════════════
            // SECTION: While with Complex Conditions
            // ═══════════════════════════════════════════════════════════════
            
            // ── Multiple conditions ─────────────────────────────────────────
            int a = 0, b = 0;
            
            while (a < 3 && b < 5)
            {
                Console.WriteLine($"a={a}, b={b}");
                a++;
                b += 2;
            }
            // Output: a=0,b=0; a=1,b=2; a=2,b=4
            
            // ── Break-based loop ───────────────────────────────────────────
            int value = 1;
            
            while (true)
            {
                Console.WriteLine(value);
                
                if (value >= 10)
                {
                    Console.WriteLine("Breaking at 10");
                    break;
                }
                
                value++;
            }
            // Output: 1 through 10, Breaking at 10

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World While Patterns
            // ═══════════════════════════════════════════════════════════════
            
            // ── Waiting for condition ─────────────────────────────────────
            Console.WriteLine("\n=== Waiting for Condition ===");
            
            int attempts = 0;
            int maxAttempts = 5;
            bool resourceAvailable = false;
            
            while (!resourceAvailable && attempts < maxAttempts)
            {
                attempts++;
                Console.WriteLine($"Attempt {attempts}...");
                
                // Simulate waiting (in real code, this might be:
                // await Task.Delay(1000);
                // or checking if resource is ready)
                
                if (attempts >= 3)
                    resourceAvailable = true;
            }
            
            if (resourceAvailable)
                Console.WriteLine("Resource available!");
            else
                Console.WriteLine("Timed out waiting");
            
            // ── Process items until empty ──────────────────────────────────
            var queue = new Queue<string>();
            queue.Enqueue("Task 1");
            queue.Enqueue("Task 2");
            queue.Enqueue("Task 3");
            
            Console.WriteLine("\n=== Processing Queue ===");
            
            while (queue.Count > 0)
            {
                var task = queue.Dequeue();
                Console.WriteLine($"Processing: {task}");
            }
            
            Console.WriteLine("Queue empty!");
            
            // ── Read file until end ───────────────────────────────────────
            // Simulated line-by-line reading
            var lines = new[] { "Line 1", "Line 2", "Line 3" };
            int lineIndex = 0;
            
            Console.WriteLine("\n=== Reading Lines ===");
            
            while (lineIndex < lines.Length)
            {
                Console.WriteLine(lines[lineIndex]);
                lineIndex++;
            }
            
            // ── Generate sequence ─────────────────────────────────────────
            double balance = 1000;
            double rate = 0.05;
            int year = 0;
            
            Console.WriteLine("\n=== Compound Interest ===");
            
            while (balance < 2000)
            {
                year++;
                balance *= (1 + rate);
                Console.WriteLine($"Year {year}: ${balance:F2}");
            }
            
            Console.WriteLine($"Reached ${balance:F2} in {year} years");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: While with Collections
            // ═══════════════════════════════════════════════════════════════
            
            // ── Traverse linked list ───────────────────────────────────────
            var linkedList = new LinkedList<int>();
            linkedList.AddLast(1);
            linkedList.AddLast(2);
            linkedList.AddLast(3);
            
            var current = linkedList.First;
            
            Console.WriteLine("\n=== Traverse LinkedList ===");
            
            while (current != null)
            {
                Console.WriteLine(current.Value);
                current = current.Next;
            }
            
            // ── Iterate dictionary ─────────────────────────────────────────
            var settings = new Dictionary<string, string>
            {
                ["host"] = "localhost",
                ["port"] = "8080",
                ["mode"] = "debug"
            };
            
            Console.WriteLine("\n=== Dictionary Keys ===");
            
            var keys = new List<string>(settings.Keys);
            int index = 0;
            
            while (index < keys.Count)
            {
                string key = keys[index];
                Console.WriteLine($"{key} = {settings[key]}");
                index++;
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: While with User Input
            // ═══════════════════════════════════════════════════════════════
            
            // Simulated user menu
            int choice = 0;
            
            Console.WriteLine("\n=== Menu System ===");
            
            while (choice != 3)
            {
                Console.WriteLine("1. New Game");
                Console.WriteLine("2. Load Game");
                Console.WriteLine("3. Exit");
                Console.Write("Choice: ");
                
                // Simulate input (would use Console.ReadLine in real code)
                choice = (choice + 1) % 4; // Just for demo
                
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Starting new game...");
                        break;
                    case 2:
                        Console.WriteLine("Loading game...");
                        break;
                    case 3:
                        Console.WriteLine("Goodbye!");
                        break;
                }
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Performance Considerations
            // ═══════════════════════════════════════════════════════════════
            
            // Don't modify collection while iterating
            // This would cause problems:
            // var list = new List<int> { 1, 2, 3 };
            // while (list.Count > 0)
            // {
            //     list.RemoveAt(0); // OK but inefficient
            // }
            
            // Instead, use a copy or clear directly
            var toProcess = new List<int> { 1, 2, 3, 4, 5 };
            
            while (toProcess.Count > 0)
            {
                int item = toProcess[0];
                toProcess.RemoveAt(0);
                Console.WriteLine($"Processed: {item}");
            }
        }
    }
}
