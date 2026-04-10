/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : Stack<T> - Advanced Operations
 * FILE      : Stack_Part2.cs
 * PURPOSE   : Demonstrates Stack<T> advanced operations
 *            including TryPeek, TryPop, Clear, and enumerators
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._04_Collections._03_Stack_Queue
{
    class StackAdvancedDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Stack<T> Advanced Operations ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: TryPeek and TryPop - Safe Operations
            // ═══════════════════════════════════════════════════════════

            var safeStack = new Stack<int>();

            // TryPeek - safely retrieves top element without removing
            bool peekSuccess = safeStack.TryPeek(out int peekValue);
            Console.WriteLine($"TryPeek on empty: success={peekSuccess}, value={peekValue}");
            // Output: TryPeek on empty: success=False, value=0

            // Add some elements
            safeStack.Push(100);
            safeStack.Push(200);
            safeStack.Push(300);

            // TryPeek on non-empty stack
            peekSuccess = safeStack.TryPeek(out peekValue);
            Console.WriteLine($"TryPeek with elements: success={peekSuccess}, value={peekValue}");
            // Output: TryPeek with elements: success=True, value=300

            // TryPop - safely removes and returns top element
            bool popSuccess = safeStack.TryPop(out int popValue);
            Console.WriteLine($"TryPop: success={popSuccess}, value={popValue}");
            // Output: TryPop: success=True, value=300

            Console.WriteLine($"Stack count after TryPop: {safeStack.Count}");
            // Output: Stack count after TryPop: 2

            // TryPop on remaining elements
            while (safeStack.TryPop(out int remaining))
            {
                Console.WriteLine($"Popped: {remaining}");
            }
            // Output:
            //   Popped: 200
            //   Popped: 100

            // TryPop on empty stack
            popSuccess = safeStack.TryPop(out popValue);
            Console.WriteLine($"TryPop on empty: success={popSuccess}, value={popValue}");
            // Output: TryPop on empty: success=False, value=0

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Clear - Removing All Elements
            // ═══════════════════════════════════════════════════════════

            var clearStack = new Stack<string>();
            clearStack.Push("A");
            clearStack.Push("B");
            clearStack.Push("C");

            Console.WriteLine($"\nBefore Clear: {clearStack.Count} elements");
            // Output: Before Clear: 3 elements

            clearStack.Clear();
            Console.WriteLine($"After Clear: {clearStack.Count} elements");
            // Output: After Clear: 0 elements

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Enumerator Usage
            // ═══════════════════════════════════════════════════════════

            var enumStack = new Stack<char>();
            enumStack.Push('X');
            enumStack.Push('Y');
            enumStack.Push('Z');

            // Using GetEnumerator directly
            IEnumerator<char> enumerator = enumStack.GetEnumerator();
            Console.WriteLine("\nUsing enumerator:");
            while (enumerator.MoveNext())
            {
                Console.WriteLine($"  Current: {enumerator.Current}");
            }
            // Output:
            //   Current: Z
            //   Current: Y
            //   Current: X

            // Reset the enumerator
            enumerator.Reset();

            // Note: Reset is required before reusing enumerator
            // In C# 9+, foreach is preferred over manual enumeration

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: ToArray - Converting to Array
            // ═══════════════════════════════════════════════════════════

            var arrayStack = new Stack<int>();
            arrayStack.Push(1);
            arrayStack.Push(2);
            arrayStack.Push(3);
            arrayStack.Push(4);

            // ToArray creates array in LIFO order (top first)
            int[] asArray = arrayStack.ToArray();
            Console.WriteLine("\nToArray result: " + string.Join(", ", asArray));
            // Output: ToArray result: 4, 3, 2, 1

            // Array can be used to create new stack
            var newStack = new Stack<int>(asArray);
            Console.WriteLine($"New stack created from array, count: {newStack.Count}");
            // Output: New stack created from array, count: 4

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: TrimExcess - Memory Optimization
            // ═══════════════════════════════════════════════════════════

            var largeStack = new Stack<int>();
            for (int i = 0; i < 1000; i++)
            {
                largeStack.Push(i);
            }

            // Remove most elements
            for (int i = 0; i < 900; i++)
            {
                largeStack.Pop();
            }

            Console.WriteLine($"\nStack count: {largeStack.Count}");
            // Output: Stack count: 100

            // TrimExcess reclaims unused capacity
            largeStack.TrimExcess();
            Console.WriteLine("TrimExcess called - internal capacity optimized");
            // Output: TrimExcess called - internal capacity optimized

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World Example - Expression Evaluator
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Real-World: Postfix Notation Calculator ===");

            // Postfix evaluation: 3 4 + 2 * = (3+4)*2 = 14
            var operandStack = new Stack<int>();
            string[] tokens = { "3", "4", "+", "2", "*" };

            foreach (string token in tokens)
            {
                if (int.TryParse(token, out int number))
                {
                    // Push numbers onto stack
                    operandStack.Push(number);
                    Console.WriteLine($"Pushed number: {number}");
                }
                else
                {
                    // It's an operator - pop two operands
                    int b = operandStack.Pop(); // second operand
                    int a = operandStack.Pop(); // first operand

                    int result = token switch
                    {
                        "+" => a + b,
                        "-" => a - b,
                        "*" => a * b,
                        "/" => a / b,
                        _ => 0
                    };

                    operandStack.Push(result);
                    Console.WriteLine($"Operation: {a} {token} {b} = {result}");
                }
            }

            int finalResult = operandStack.Pop();
            Console.WriteLine($"Final result: {finalResult}");
            // Output: Final result: 14

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Real-World Example - Navigation History
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Real-World: Browser Navigation History ===");

            var navigationHistory = new Stack<NavigationEntry>();

            // Track page visits
            navigationHistory.Push(new NavigationEntry("home.com", DateTime.Now.AddMinutes(-30)));
            navigationHistory.Push(new NavigationEntry("products.html", DateTime.Now.AddMinutes(-25)));
            navigationHistory.Push(new NavigationEntry("product-details.html", DateTime.Now.AddMinutes(-20)));
            navigationHistory.Push(new NavigationEntry("checkout.html", DateTime.Now.AddMinutes(-10)));

            Console.WriteLine("Navigation history (most recent first):");
            foreach (var entry in navigationHistory)
            {
                Console.WriteLine($"  {entry.Url} at {entry.Timestamp:HH:mm}");
            }
            // Output:
            //   checkout.html at 10:20
            //   product-details.html at 10:10
            //   products.html at 10:05
            //   home.com at 09:30

            // Go back functionality
            Console.WriteLine("\n--- Going Back ---");
            var lastPage = navigationHistory.Pop();
            Console.WriteLine($"Navigated back from: {lastPage.Url}");
            // Output: Navigated back from: checkout.html

            Console.WriteLine($"History now has {navigationHistory.Count} pages");
            // Output: History now has 3 pages

            Console.WriteLine("\n=== Stack<T> Advanced Operations Complete ===");
        }
    }

    // Navigation entry class for real-world example
    public class NavigationEntry
    {
        public string Url { get; set; }
        public DateTime Timestamp { get; set; }

        public NavigationEntry(string url, DateTime timestamp)
        {
            Url = url;
            Timestamp = timestamp;
        }
    }
}