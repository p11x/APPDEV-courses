/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Data Types - Reference Types (Part 1)
 * FILE      : ReferenceTypes.cs
 * PURPOSE   : This file covers C# reference types including class, interface, delegate, and dynamic types.
 *             Reference types store a reference to the data location, not the data itself.
 * ============================================================
 */

// --- SECTION: Introduction to Reference Types ---
// Reference types store a reference (pointer) to the actual data, not the data itself.
// When assigned to another variable, both variables point to the same object.
// Stored on the heap, not the stack. Include: class, interface, delegate, arrays, string, dynamic.

using System;

namespace CSharp_MasterGuide._01_Fundamentals._02_DataTypes
{
    // ── CLASS: Reference Type Basics ────────────────────────────────────────
    // Classes are reference types - the variable holds a reference to the object
    
    // Simple Person class for demonstrating reference type behavior
    class Person
    {
        public string Name { get; set; } // Auto-property for name
        public int Age { get; set; }     // Auto-property for age
        
        // Constructor to initialize properties
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
        
        // Override ToString for display
        public override string ToString() => $"Person: {Name}, Age: {Age}";
    }

    class ReferenceTypes
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Reference vs Value Type Behavior
            // ═══════════════════════════════════════════════════════════════
            
            // ── Value type behavior: COPY made on assignment ─────────────────
            int x = 10; // Create value type variable
            int y = x;  // COPY the value to y - two independent copies
            y = 20;    // Modify y does NOT affect x
            Console.WriteLine($"x = {x}, y = {y}"); // Output: x = 10, y = 20
            
            // ── Reference type behavior: REFERENCE shared ──────────────────
            Person person1 = new Person("Alice", 30); // Create object on heap
            Person person2 = person1; // Copy REFERENCE - both point to same object
            person2.Name = "Bob";    // Modify object through person2
            Console.WriteLine($"person1: {person1}"); // Output: person1: Person: Bob, Age: 30
            Console.WriteLine($"person2: {person2}"); // Output: person2: Person: Bob, Age: 30
            // Both show "Bob" because they reference the same object!
            
            // ── Checking reference equality ─────────────────────────────────
            // Use ReferenceEquals to check if two references point to same object
            Person person3 = new Person("Charlie", 25); // New object
            bool sameReference = ReferenceEquals(person1, person2); // True - same object
            bool differentReference = ReferenceEquals(person1, person3); // False - different objects
            Console.WriteLine($"person1 == person2: {sameReference}"); // Output: person1 == person2: True
            Console.WriteLine($"person1 == person3: {differentReference}"); // Output: person1 == person3: False

            // ═══════════════════════════════════════════════════════════════
            // SECTION: String Type (Special Reference Type)
            // ═══════════════════════════════════════════════════════════════
            // Strings are reference types but behave like value types (immutable)
            
            // ── String immutability ─────────────────────────────────────────
            string str1 = "Hello"; // Create string
            string str2 = str1;    // Both point to same string in intern pool
            str2 = "World";       // Creates NEW string - str1 unchanged
            
            Console.WriteLine($"str1: {str1}"); // Output: str1: Hello
            Console.WriteLine($"str2: {str2}"); // Output: str2: World
            // str1 is still "Hello" because strings are immutable!
            
            // ── String intern pool ───────────────────────────────────────────
            // C# interns string literals for performance
            string a = "hello"; // From intern pool
            string b = "hello"; // Same intern pool reference
            bool internedSame = ReferenceEquals(a, b); // True - same reference!
            Console.WriteLine($"Interned strings same reference: {internedSame}");
            // Output: Interned strings same reference: True
            
            // But dynamically created strings are different
            string c = new string("hello".ToCharArray()); // New object
            bool newStringSame = ReferenceEquals(a, c); // False - different objects
            Console.WriteLine($"New string same reference: {newStringSame}");
            // Output: New string same reference: False

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Arrays as Reference Types
            // ═══════════════════════════════════════════════════════════════
            // Arrays in C# are reference types (even if element type is value type)
            
            // ── Array reference behavior ──────────────────────────────────────
            int[] numbers1 = { 1, 2, 3 }; // Create array
            int[] numbers2 = numbers1;    // Copy reference
            numbers2[0] = 99;             // Modify through numbers2
            
            Console.WriteLine($"numbers1[0]: {numbers1[0]}"); // Output: numbers1[0]: 99
            Console.WriteLine($"numbers2[0]: {numbers2[0]}"); // Output: numbers2[0]: 99
            // Both show 99 because they reference same array!
            
            // ── Array of reference type objects ──────────────────────────────
            Person[] people = new Person[2]; // Array that holds Person references
            people[0] = new Person("David", 40); // Create Person objects
            people[1] = new Person("Eve", 35);
            
            // Copy array - both arrays reference same Person objects
            Person[] peopleCopy = people;
            peopleCopy[0].Name = "Frank"; // Modify through copy
            
            Console.WriteLine($"Original[0]: {people[0].Name}"); // Output: Original[0]: Frank
            Console.WriteLine($"Copy[0]: {peopleCopy[0].Name}"); // Output: Copy[0]: Frank

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Object Type (Base Type for All)
            // ═══════════════════════════════════════════════════════════════
            // object is the base type for all types in C# (both value and reference)
            
            // ── Boxing: Value type to object ─────────────────────────────────
            // Boxing: Converting value type to object reference
            int num = 42; // Value type on stack
            object boxed = num; // Boxing - wraps value in object on heap
            Console.WriteLine($"Boxed value: {boxed}"); // Output: Boxed value: 42
            
            // ── Unboxing: Object to value type ───────────────────────────────
            // Unboxing: Converting object back to value type
            int unboxed = (int)boxed; // Unboxing - extract value from object
            Console.WriteLine($"Unboxed value: {unboxed}"); // Output: Unboxed value: 42
            
            // ── Boxing with validation ───────────────────────────────────────
            // Always use safe casting to avoid InvalidCastException
            object someValue = 100;
            if (someValue is int intValue) // Pattern matching for safe unboxing
            {
                Console.WriteLine($"Safe unbox: {intValue}"); // Output: Safe unbox: 100
            }
            
            // ── Storing different types in object ───────────────────────────
            // object can hold any type - useful for mixed collections
            object[] mixed = new object[4]; // Can hold any type
            mixed[0] = 42;           // int (value type - boxed)
            mixed[1] = "hello";      // string (reference type)
            mixed[2] = 3.14;         // double (value type - boxed)
            mixed[3] = new Person("Grace", 28); // custom object
            
            foreach (var item in mixed) // Loop through mixed array
            {
                Console.WriteLine($"Type: {item.GetType().Name}, Value: {item}");
            }
            // Output:
            // Type: Int32, Value: 42
            // Type: String, Value: hello
            // Type: Double, Value: 3.14
            // Type: Person, Value: Person: Grace, Age: 28

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Dynamic Type (Late Binding)
            // ═══════════════════════════════════════════════════════════════
            // dynamic bypasses compile-time type checking - resolved at runtime
            
            // ── Dynamic type basics ──────────────────────────────────────────
            dynamic dyn = "Hello"; // dynamic can hold any type
            Console.WriteLine($"Dynamic string: {dyn}"); // Output: Dynamic string: Hello
            
            dyn = 123; // Change to integer - no compile-time error
            Console.WriteLine($"Dynamic int: {dyn}"); // Output: Dynamic int: 123
            
            dyn = new Person("Henry", 50); // Change to custom object
            Console.WriteLine($"Dynamic person: {dyn.Name}"); // Output: Dynamic person: Henry
            
            // ── Dynamic vs object ─────────────────────────────────────────────
            // object requires explicit casting; dynamic resolves automatically
            object obj = "hello"; // object needs cast to call string methods
            // Console.WriteLine(((string)obj).ToUpper()); // Must cast
            
            dynamic dyn2 = "hello"; // dynamic resolves at runtime
            Console.WriteLine(dyn2.ToUpper()); // Output: HELLO
            // No cast needed - but will fail at runtime if method doesn't exist!

            // ── Dynamic in real-world scenarios ───────────────────────────────
            // Used with COM interop, reflection-heavy code, dynamic languages
            // Example: Processing JSON with unknown structure
            dynamic jsonResponse = GetDynamicJson(); // Simulated dynamic JSON
            Console.WriteLine($"User name: {jsonResponse.user.name}"); // Works at runtime
            // Console.WriteLine($"User name: {jsonResponse.user.name}");
            
            // NOTE: Uncomment the above line would work at runtime if jsonResponse has user.name
            // But would fail at compile time - you'd only discover errors at runtime!
        }
        
        // Simulated method returning dynamic (for demonstration)
        static dynamic GetDynamicJson()
        {
            // In real code, this might come from a JSON library or dynamic language
            // Creating a simple dynamic-like behavior using ExpandoObject
            dynamic response = new System.Dynamic.ExpandoObject();
            response.user = new System.Dynamic.ExpandoObject();
            response.user.name = "John";
            response.user.email = "john@example.com";
            return response;
        }
    }
}
