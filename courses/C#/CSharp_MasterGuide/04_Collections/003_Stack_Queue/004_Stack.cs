/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : Stack<T> - LIFO Operations
 * FILE      : Stack.cs
 * PURPOSE   : Demonstrates Stack<T> fundamental operations
 *            including Push, Pop, Peek, and Contains
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._04_Collections._03_Stack_Queue
{
    class StackDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Stack<T> LIFO Operations ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Creating a Stack
            // ═══════════════════════════════════════════════════════════

            // Empty stack of integers
            var emptyStack = new Stack<int>();
            Console.WriteLine($"Empty stack count: {emptyStack.Count}");
            // Output: Empty stack count: 0

            // Stack from array initialization
            var stackFromArray = new Stack<string>(new[] { "First", "Second", "Third" });
            Console.WriteLine($"Stack from array count: {stackFromArray.Count}");
            // Output: Stack from array count: 3

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Push - Adding Elements
            // ═══════════════════════════════════════════════════════════

            var browserStack = new Stack<string>();

            // Push adds elements to the TOP of the stack
            browserStack.Push("google.com");
            browserStack.Push("youtube.com");
            browserStack.Push("github.com");
            browserStack.Push("stackoverflow.com");

            Console.WriteLine($"After Push operations: {browserStack.Count} pages");
            // Output: After Push operations: 4 pages

            // Peek to see top element without removing it
            Console.WriteLine($"Top page (Peek): {browserStack.Peek()}");
            // Output: Top page (Peek): stackoverflow.com

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Pop - Removing Elements
            // ═══════════════════════════════════════════════════════════

            // Pop removes and returns the TOP element (LIFO order)
            string lastPage = browserStack.Pop();
            Console.WriteLine($"Popped: {lastPage}");
            // Output: Popped: stackoverflow.com

            Console.WriteLine($"Stack count after pop: {browserStack.Count}");
            // Output: Stack count after pop: 3

            // Pop another element
            string secondLast = browserStack.Pop();
            Console.WriteLine($"Popped: {secondLast}");
            // Output: Popped: github.com

            // Peek again to see what's on top now
            Console.WriteLine($"Now on top: {browserStack.Peek()}");
            // Output: Now on top: youtube.com

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Contains - Checking Elements
            // ═══════════════════════════════════════════════════════════

            bool hasGoogle = browserStack.Contains("google.com");
            bool hasTwitter = browserStack.Contains("twitter.com");

            Console.WriteLine($"Contains 'google.com': {hasGoogle}");
            // Output: Contains 'google.com': True
            Console.WriteLine($"Contains 'twitter.com': {hasTwitter}");
            // Output: Contains 'twitter.com': False

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Iterating Without Removing
            // ═══════════════════════════════════════════════════════════

            // Push more elements for iteration demo
            var numberStack = new Stack<int>();
            numberStack.Push(10);
            numberStack.Push(20);
            numberStack.Push(30);
            numberStack.Push(40);
            numberStack.Push(50);

            Console.WriteLine("\nIterating stack (top to bottom):");
            foreach (int num in numberStack)
            {
                Console.WriteLine($"  {num}");
            }
            // Output:
            //   50
            //   40
            //   30
            //   20
            //   10

            // Copy to array (preserves order from top to bottom)
            int[] stackArray = numberStack.ToArray();
            Console.WriteLine("\nAs array (top to bottom): " + string.Join(", ", stackArray));
            // Output: As array (top to bottom): 50, 40, 30, 20, 10

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World Example - Undo/Redo System
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Real-World: Text Editor Undo System ===");

            var undoStack = new Stack<string>();

            // Simulate text editing operations
            string currentDocument = "";

            // Initial document
            currentDocument = "Hello";
            undoStack.Push(currentDocument); // Save state before change
            Console.WriteLine($"Document: '{currentDocument}'");

            currentDocument = "Hello World";
            undoStack.Push(currentDocument);
            Console.WriteLine($"Document: '{currentDocument}'");

            currentDocument = "Hello World!";
            undoStack.Push(currentDocument);
            Console.WriteLine($"Document: '{currentDocument}'");

            currentDocument = "Hello World!!";
            undoStack.Push(currentDocument);
            Console.WriteLine($"Document: '{currentDocument}'");

            // Undo operation - pop the last change
            Console.WriteLine("\n--- Undo ---");
            if (undoStack.Count > 0)
            {
                currentDocument = undoStack.Pop();
                Console.WriteLine($"After undo: '{currentDocument}'");
                // Output: After undo: 'Hello World!'
            }

            if (undoStack.Count > 0)
            {
                currentDocument = undoStack.Pop();
                Console.WriteLine($"After undo: '{currentDocument}'");
                // Output: After undo: 'Hello World'
            }

            Console.WriteLine($"\nUndo stack has {undoStack.Count} states left");
            // Output: Undo stack has 2 states left

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Real-World Example - Call Stack (Recursive)
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Real-World: Function Call Tracking ===");

            var callStack = new Stack<string>();

            // Simulate function call hierarchy
            callStack.Push("Main()");
            callStack.Push("ProcessOrder()");
            callStack.Push("ValidateInput()");
            callStack.Push("ParseData()");

            Console.WriteLine("Current call stack (bottom to top):");
            foreach (string method in callStack)
            {
                Console.WriteLine($"  {method}");
            }
            // Output:
            //   Main()
            //   ProcessOrder()
            //   ValidateInput()
            //   ParseData()

            Console.WriteLine($"\nDeepest call: {callStack.Peek()}");
            // Output: Deepest call: ParseData()

            Console.WriteLine("\n=== Stack<T> Operations Complete ===");
        }
    }
}