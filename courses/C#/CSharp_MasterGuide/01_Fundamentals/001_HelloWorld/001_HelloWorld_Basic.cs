/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Hello World - Basic
 * FILE      : HelloWorld_Basic.cs
 * PURPOSE   : This file covers the most basic C# program - the traditional Hello World.
 *             It demonstrates the minimal structure required to write and run a C# console application.
 * ============================================================
 */

// --- SECTION: Basic Hello World Program ---
// The simplest C# program consists of a class with a Main method.
// This is the entry point for any C# console application.

using System;
// System namespace contains fundamental classes like Console, which we use for input/output

namespace CSharp_MasterGuide._01_Fundamentals._01_HelloWorld
{
    // Class definition - every C# program must have at least one class
    // The class can be named anything, but conventionally matches the filename
    class HelloWorld_Basic
    {
        // Main method - this is the entry point of the application
        // The runtime searches for this method to start execution
        // static: method can be called without creating an instance of the class
        // void: method returns nothing
        // string[] args: command-line arguments passed to the program
        static void Main(string[] args)
        {
            // ── EXAMPLE 1: Simple Hello World ──────────────────────
            // This is the most basic way to display text on the console
            // Console.WriteLine writes text followed by a new line character
            
            Console.WriteLine("Hello, World!"); // Output: Hello, World!

            // ── EXAMPLE 2: Using Write vs WriteLine ──────────────────────
            // WriteLine adds a newline after printing, Write does not
            
            Console.Write("Hello, ");  // Prints "Hello, " without newline - cursor stays on same line
            Console.WriteLine("World!"); // Prints "World!" and moves to new line
            // Output: 
            // Hello, World!

            // ── EXAMPLE 3: Multiple WriteLine calls ──────────────────────
            // Each call produces a new line in the output
            
            Console.WriteLine("Line 1"); // Output: Line 1
            Console.WriteLine("Line 2"); // Output: Line 2
            Console.WriteLine("Line 3"); // Output: Line 3

            // ── EXAMPLE 4: Empty lines ──────────────────────
            // Calling WriteLine with no arguments outputs an empty line
            
            Console.WriteLine("Before empty line"); // Output: Before empty line
            Console.WriteLine(); // Output: (empty line)
            Console.WriteLine("After empty line"); // Output: After empty line
        }
    }
}
