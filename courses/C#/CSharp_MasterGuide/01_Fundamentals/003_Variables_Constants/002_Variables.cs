/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Variables and Constants - Variables (Part 1)
 * FILE      : Variables.cs
 * PURPOSE   : This file covers variable declaration, initialization, scoping, and basic usage in C#.
 *             Variables are containers for storing data values.
 * ============================================================
 */

// --- SECTION: Variable Fundamentals ---
// Variables are named storage locations in memory that hold values which can be modified during program execution.
// Each variable has a specific type that determines what kind of data it can store.

using System;

namespace CSharp_MasterGuide._01_Fundamentals._03_Variables_Constants
{
    class Variables
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Variable Declaration and Initialization
            // ═══════════════════════════════════════════════════════════════
            
            // ── Declaration only ────────────────────────────────────────────
            // Declare variable without initial value
            int age; // Declares integer variable named 'age' - default not initialized (could be any value!)
            // Console.WriteLine(age); // ERROR: Use of unassigned local variable!
            
            // ── Declaration with initialization ─────────────────────────────
            // Declare and assign value in one statement
            int count = 10; // Declares and initializes count to 10
            Console.WriteLine($"Count: {count}"); // Output: Count: 10
            
            // ── Multiple variables in one line ───────────────────────────────
            int a = 1, b = 2, c = 3; // Declare multiple same-type variables
            Console.WriteLine($"a={a}, b={b}, c={c}"); // Output: a=1, b=2, c=3
            
            // ── Type-specific variables ─────────────────────────────────────
            string name = "Alice"; // String - reference type for text
            double price = 19.99; // Double - floating-point
            bool isActive = true; // Boolean - true/false
            char grade = 'A'; // Character - single Unicode
            decimal salary = 75000.50m; // Decimal - high precision
            
            Console.WriteLine($"Name: {name}"); // Output: Name: Alice
            Console.WriteLine($"Price: {price:C}"); // Output: Price: $19.99 (Currency format)
            Console.WriteLine($"Active: {isActive}"); // Output: Active: True
            Console.WriteLine($"Grade: {grade}"); // Output: Grade: A
            Console.WriteLine($"Salary: {salary:N2}"); // Output: Salary: 75,000.50 (Number format)

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Variable Scope
            // ═══════════════════════════════════════════════════════════════
            
            // ── Method-level scope (local variables) ────────────────────────
            int methodLevel = 100; // Accessible anywhere in Main method
            Console.WriteLine($"Method level: {methodLevel}"); // Output: Method level: 100
            
            // Block scope with braces
            if (true)
            {
                int blockLevel = 200; // Only accessible in this block
                Console.WriteLine($"Block level: {blockLevel}"); // Output: Block level: 200
                
                // Can access outer variables
                Console.WriteLine($"Outer: {methodLevel}"); // Output: Outer: 100
            }
            // Console.WriteLine(blockLevel); // ERROR: Out of scope!
            
            // ── Nested blocks ────────────────────────────────────────────────
            int outer = 1;
            {
                int middle = 2;
                {
                    int inner = 3;
                    Console.WriteLine($"Outer: {outer}, Middle: {middle}, Inner: {inner}");
                    // Output: Outer: 1, Middle: 2, Inner: 3
                }
                // Console.WriteLine(inner); // ERROR: inner is out of scope
                Console.WriteLine($"Outer: {outer}, Middle: {middle}"); // OK: middle in scope
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Variable Initialization Requirements
            // ═══════════════════════════════════════════════════════════════
            
            // ── Local variables must be initialized before use ─────────────
            int uninitialized; // Not initialized
            // Cannot use: Console.WriteLine(uninitialized); // Compiler error!
            
            // Must assign before first read
            uninitialized = 42; // Now initialized
            Console.WriteLine($"Initialized: {uninitialized}"); // Output: Initialized: 42
            
            // ── Class-level fields have default values ─────────────────────
            // ClassMemberExample class (defined below) demonstrates this
            
            // ── Array elements are initialized by default ─────────────────
            int[] numbers = new int[3]; // All elements = 0
            Console.WriteLine($"Array[0]: {numbers[0]}"); // Output: Array[0]: 0
            string[] names = new string[3]; // All elements = null
            Console.WriteLine($"Names[0]: {names[0] ?? "null"}"); // Output: Names[0]: null

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Dynamic vs Static Typing
            // ═══════════════════════════════════════════════════════════════
            
            // ── Statically typed (explicit type) ───────────────────────────
            string explicitType = "Hello"; // Type known at compile time
            int explicitInt = 42;
            Console.WriteLine($"Explicit: {explicitType}, {explicitInt}");
            
            // ── Implicit typing with var keyword ───────────────────────────
            // Compiler infers type from right side
            var inferredString = "World"; // Compiler sees string literal
            var inferredInt = 100; // Compiler sees int literal
            Console.WriteLine($"Inferred: {inferredString}, {inferredInt}");
            
            // var still requires initialization - cannot do: var noInit;
            
            // ── When to use var vs explicit ─────────────────────────────────
            // var useful when type is obvious from right side
            var list = new List<int>(); // Clear from constructor
            var dict = new Dictionary<string, List<Customer>>(); // Complex type
            
            // Explicit useful for clarity or when type differs from initializer
            object obj = "string"; // Explicit shows it's boxed
            int explicitInt2 = (int)3.14; // Explicit shows conversion

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Readonly Variables
            // ═══════════════════════════════════════════════════════════════
            
            // ── const - compile-time constant ──────────────────────────────
            const int MaxConnections = 100; // Must be initialized with compile-time constant
            // MaxConnections = 200; // ERROR: Cannot modify const
            Console.WriteLine($"Const: {MaxConnections}"); // Output: Const: 100
            
            // const values are inlined by compiler
            const double Pi = 3.14159; // Inline this value everywhere
            double radius = 5;
            double area = Pi * radius * radius;
            Console.WriteLine($"Circle area: {area}"); // Output: Circle area: 78.53975
            
            // ── readonly - runtime constant (see Constants file for more)
            // readonly can be initialized in constructor
            var instance = new ReadOnlyExample(50);
            Console.WriteLine($"Readonly field: {instance.Value}"); // Output: Readonly field: 50

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Variable Naming Best Practices
            // ═══════════════════════════════════════════════════════════════
            
            // ── CamelCase for local variables ─────────────────────────────
            int customerCount = 0; // Clear, descriptive
            string firstName = "John"; // Easy to read
            DateTime orderDate = DateTime.Now;
            
            // ── Meaningful names over brevity ─────────────────────────────
            int x = 0; // Poor: what is x?
            int numberOfOrders = 0; // Better: describes content
            int orderCount = 0; // Even better: specific
            
            // ── Avoid single letters except in loops ───────────────────────
            for (int i = 0; i < 10; i++) // i is OK for loop counter
            {
                // But for complex logic, use descriptive names
            }
            
            // ── Underscore prefix for private fields (convention) ──────────
            // See OOP section - this is field naming convention
        }
    }
    
    // Class demonstrating field default values
    class ClassMemberExample
    {
        // Class-level fields get default values automatically
        public int intField; // Default: 0
        public bool boolField; // Default: false
        public string stringField; // Default: null
        public double doubleField; // Default: 0
        public object objectField; // Default: null
        
        // Method to display defaults
        public void ShowDefaults()
        {
            Console.WriteLine($"int: {intField}"); // Output: int: 0
            Console.WriteLine($"bool: {boolField}"); // Output: bool: False
            Console.WriteLine($"string: {stringField ?? "null"}"); // Output: string: null
            Console.WriteLine($"double: {doubleField}"); // Output: double: 0
        }
    }
    
    // Class demonstrating readonly
    class ReadOnlyExample
    {
        public readonly int Value; // Can be set in constructor or initializer
        
        public ReadOnlyExample(int value)
        {
            Value = value; // OK: can set in constructor
        }
        
        // Value cannot be modified after construction
    }
}
