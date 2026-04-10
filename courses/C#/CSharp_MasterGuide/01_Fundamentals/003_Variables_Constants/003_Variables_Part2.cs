/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Variables and Constants - Variables (Part 2)
 * FILE      : Variables_Part2.cs
 * PURPOSE   : This file covers advanced variable topics including stack vs heap,
 *             ref and out parameters, and variable lifetime.
 * ============================================================
 */

// --- SECTION: Advanced Variable Topics ---
// This file covers advanced variable concepts like memory location, parameters, and lifetime

using System;

namespace CSharp_MasterGuide._01_Fundamentals._03_Variables_Constants
{
    class Variables_Part2
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Stack vs Heap Memory
            // ═══════════════════════════════════════════════════════════════
            
            // ── Value types: stored on stack (for local variables) ───────────
            int localInt = 42; // Stored on stack - fast access, automatic cleanup
            double localDouble = 3.14; // Also on stack
            bool localBool = true;
            
            Console.WriteLine($"Stack value: {localInt}"); // Output: Stack value: 42
            
            // ── Reference types: stored on heap ───────────────────────────
            // Object itself on heap, reference on stack
            object boxed = new object(); // Object on heap, boxed ref on stack
            string heapString = "hello"; // String data on heap, heapString ref on stack
            
            // ── Value type in a class (boxed to heap) ──────────────────────
            var person = new Person(); // Person object on heap
            person.Age = 30; // Age (value type) is part of object on heap
            
            // When value type is passed to method, copy is made
            // See ref/out section below

            // ═══════════════════════════════════════════════════════════════
            // SECTION: ref and out Parameters
            // ═══════════════════════════════════════════════════════════════
            
            // ── ref: pass by reference (both read and write) ───────────────
            int number = 10;
            Console.WriteLine($"Before ref: {number}"); // Output: Before ref: 10
            DoubleValue(ref number); // Pass reference, not copy
            Console.WriteLine($"After ref: {number}"); // Output: After ref: 20
            
            // ── out: for output parameters (must be set in method) ──────────
            int result;
            Calculate(out result); // Must assign result in Calculate
            Console.WriteLine($"Out parameter: {result}"); // Output: Out parameter: 100
            
            // ── out with inline declaration (C# 7.0+) ──────────────────────
            if (TryParseNumber("42", out int parsed))
            {
                Console.WriteLine($"Parsed: {parsed}"); // Output: Parsed: 42
            }
            
            // ── in: read-only reference (C# 7.2+) ─────────────────────────
            // Prevents copying but also prevents modification
            int readonlyValue = 50;
            PrintValue(in readonlyValue); // Can read but not modify
            // PrintValue(in 100); // Cannot use literal - needs variable
            
            // ── ref returns (C# 7.0+) ───────────────────────────────────────
            var arr = new int[] { 1, 2, 3 };
            ref int foundRef = ref FindRef(arr, 2); // Get reference to element
            foundRef = 99; // Modify through reference
            Console.WriteLine($"Array modified: {arr[1]}"); // Output: Array modified: 99

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Variable Lifetime and Scope
            // ═══════════════════════════════════════════════════════════════
            
            // ── Local variables: lifetime is method execution ─────────────
            for (int i = 0; i < 3; i++)
            {
                int loopVar = i; // New variable each iteration
                Console.WriteLine($"Loop var: {loopVar}"); // Output: 0, 1, 2
            }
            // loopVar is out of scope here
            
            // ── Static variables: lifetime is application ──────────────────
            // See StaticClasses section - survives across calls
            
            // ── Instance variables: lifetime is object lifetime ─────────────
            var obj = new LifetimeDemo();
            obj.SetValue(42);
            Console.WriteLine($"Instance var: {obj.GetValue()"); // Output: Instance var: 42
            // Value persists as long as obj exists

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Parameter Passing Mechanisms
            // ═══════════════════════════════════════════════════════════════
            
            // ── Pass by value (default) - copies the value ────────────────
            int val = 5;
            ModifyValue(val); // Pass copy - original unchanged
            Console.WriteLine($"After value pass: {val}"); // Output: After value pass: 5
            
            // ── Pass reference type by value - copies reference ──────────────
            var personRef = new Person { Name = "Alice", Age = 30 };
            ModifyObject(personRef); // Copies reference, points to same object
            Console.WriteLine($"After ref type pass: {personRef.Age}"); // Output: After ref type pass: 35
            // Object is modified through reference
            
            // But assigning new object doesn't affect original
            personRef = new Person { Name = "Bob", Age = 20 };
            // The reassignment only affects local copy

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Deconstruction (Tuples)
            // ═══════════════════════════════════════════════════════════════
            
            // ── Deconstruct tuple into variables ───────────────────────────
            var tuple = (Name: "John", Age: 30, City: "NYC");
            (string name, int age, string city) = tuple; // Deconstruct
            Console.WriteLine($"Deconstructed: {name}, {age}, {city}");
            // Output: Deconstructed: John, 30, NYC
            
            // ── Discard with underscore ────────────────────────────────────
            var (_, _, city2) = tuple; // Discard name and age
            Console.WriteLine($"City only: {city2}"); // Output: City only: NYC

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Stack Allocation (unsafe)
            // ═══════════════════════════════════════════════════════════════
            
            // ── stackalloc: allocate on stack instead of heap ──────────────
            // Only works with unmanaged types (primitives, structs)
            unsafe
            {
                int* stackArray = stackalloc int[10]; // Allocate 10 ints on stack
                for (int i = 0; i < 10; i++)
                {
                    stackArray[i] = i * 2;
                    Console.WriteLine($"stackalloc[{i}]: {stackArray[i]}");
                }
                // Automatically freed when exiting block - no GC needed!
            }
            
            // Span<T> is the safe alternative to stackalloc
            Span<int> safeStack = stackalloc int[10]; // Can use in safe code
            safeStack[0] = 42;
            Console.WriteLine($"Span stack: {safeStack[0]}"); // Output: Span stack: 42

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Variable Interpolation with String
            // ═══════════════════════════════════════════════════════════════
            
            // ── String interpolation with variables ────────────────────────
            string greet = "Hello";
            string target = "World";
            string full = $"{greet}, {target}!"; // $ prefix enables interpolation
            Console.WriteLine(full); // Output: Hello, World!
            
            // ── Formatting in interpolation ─────────────────────────────────
            double price = 1234.56;
            DateTime date = new DateTime(2024, 1, 15);
            string formatted = $"Price: {price:C2}, Date: {date:yyyy-MM-dd}";
            Console.WriteLine(formatted); // Output: Price: $1,234.56, Date: 2024-01-15
            
            // ── Inline expressions ─────────────────────────────────────────
            int x = 10, y = 20;
            string calc = $"x + y = {x + y}"; // Can include expressions
            Console.WriteLine(calc); // Output: x + y = 30
        }
        
        // Methods demonstrating ref and out
        
        static void DoubleValue(ref int num)
        {
            num *= 2; // Modifies original variable
        }
        
        static void Calculate(out int result)
        {
            result = 100; // Must assign before returning
        }
        
        static bool TryParseNumber(string input, out int result)
        {
            return int.TryParse(input, out result);
        }
        
        static void PrintValue(in int value)
        {
            // value = 100; // ERROR: Cannot modify 'in' parameter
            Console.WriteLine($"In value: {value}");
        }
        
        static ref int FindRef(int[] array, int target)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == target)
                    return ref array[i]; // Return reference to element
            }
            throw new InvalidOperationException("Not found");
        }
        
        static void ModifyValue(int val)
        {
            val = 100; // Only modifies local copy
        }
        
        static void ModifyObject(Person p)
        {
            p.Age = 35; // Modifies object through reference
            // p = new Person(); // This would not affect original
        }
    }
    
    // Simple class for reference type examples
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    
    // Class demonstrating instance variable lifetime
    class LifetimeDemo
    {
        private int _value; // Instance variable - lives with object
        
        public void SetValue(int value)
        {
            _value = value; // Stored in object on heap
        }
        
        public int GetValue() => _value;
    }
}
