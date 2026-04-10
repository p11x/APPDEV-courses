/*
 * ============================================================
 * TOPIC     : Pattern Matching
 * SUBTOPIC  : Type Patterns - Is Expression
 * FILE      : 03_IsPattern.cs
 * PURPOSE   : Demonstrates the 'is' expression for type testing and pattern matching
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._09_PatternMatching._01_TypePatterns
{
    /// <summary>
    /// Demonstrates the 'is' expression for runtime type checking
    /// </summary>
    public class IsPattern
    {
        /// <summary>
        /// Entry point demonstrating 'is' pattern matching
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Is Expression Pattern Demo ===
            Console.WriteLine("=== Is Expression Pattern Demo ===\n");

            // ── CONCEPT: Basic 'is' Expression ──────────────────────────────
            // The 'is' operator tests if an object is of a given type

            // Example 1: Simple type check with is
            // Output: 1. Basic 'is' Checks:
            Console.WriteLine("1. Basic 'is' Checks:");
            
            // object = base type for all reference types
            object[] samples = { "hello", 42, 3.14, null, new int[] { 1, 2, 3 } };
            
            // foreach = iterate through array
            foreach (object sample in samples)
            {
                // is string = checks if sample is string type
                // bool = result of type check
                bool isString = sample is string;
                bool isInt = sample is int;
                bool isDouble = sample is double;
                
                // Output: [value] - String: [bool], Int: [bool], Double: [bool]
                Console.WriteLine($"   {sample} - String: {isString}, Int: {isInt}, Double: {isDouble}");
            }

            // ── CONCEPT: 'is' with Pattern Variables ────────────────────────
            // 'is' can capture matched value in a new variable

            // Example 2: Pattern variable capture
            // Output: 2. Pattern Variable Capture:
            Console.WriteLine("\n2. Pattern Variable Capture:");
            
            // object input = test value
            object input = "Pattern Match";
            
            // if (input is string result) = check type AND assign to result
            // This only executes if input IS a string
            if (input is string result)
            {
                // result = the captured string
                // string.Length = character count
                // Output: Captured string: "Pattern Match", Length: 13
                Console.WriteLine($"   Captured string: \"{result}\", Length: {result.Length}");
            }

            // ── CONCEPT: 'is not' Negated Pattern ──────────────────────────
            // 'is not' checks that object is NOT of a type

            // Example 3: Negated pattern
            // Output: 3. Negated Pattern:
            Console.WriteLine("\n3. Negated Pattern:");
            
            // object notInt = string value
            object notInt = "I'm a string";
            
            // is not int = true if NOT an int
            // Output: Is not int: True
            Console.WriteLine($"   Is not int: {notInt is not int}");

            // Example 4: Using negated pattern for filtering
            // Output: 4. Filtering with Negated Pattern:
            Console.WriteLine("\n4. Filtering with Negated Pattern:");
            
            // object[] mixed = collection with various types
            object[] mixed = { 1, "two", 3, "four", null, 5 };
            
            // foreach = iterate through items
            foreach (object item in mixed)
            {
                // is not string = true if item is NOT a string
                // Filter out strings from processing
                if (item is not string)
                {
                    // Output: Non-string item: [value]
                    Console.WriteLine($"   Non-string item: {item}");
                }
            }

            // ── CONCEPT: 'is' with Nullable Types ───────────────────────────
            // Works with nullable value types

            // Example 5: Nullable type check
            // Output: 5. Nullable Type Check:
            Console.WriteLine("\n5. Nullable Type Check:");
            
            // int? = nullable int (can hold null or int value)
            int? nullable = 100;
            int? empty = null;

            // is int = checks if nullable has value AND is int type
            // Output: 100 is int: True
            Console.WriteLine($"   100 is int: {nullable is int}");
            // Output: null is int: False
            Console.WriteLine($"   null is int: {empty is int}");

            // ── CONCEPT: 'is' with Expression Patterns ──────────────────────
            // Can use 'is' in expressions (not just if conditions)

            // Example 6: Expression context usage
            // Output: 6. Expression Context Usage:
            Console.WriteLine("\n6. Expression Context Usage:");
            
            // GetTypeName uses ternary with is pattern
            // string = result type
            object value = 42;
            
            // value is int ? ... : ... = inline type check in expression
            // Output: Type name: Int32
            Console.WriteLine($"   Type name: {(value is int ? "Int32" : "Other")}");

            // ── CONCEPT: 'is' with Property Patterns ────────────────────────
            // Can check properties after type check

            // Example 7: Type + property pattern
            // Output: 7. Type + Property Pattern:
            Console.WriteLine("\n7. Type + Property Pattern:");
            
            // Process different person types
            ProcessPerson(new Employee("Alice", "Engineering"));
            ProcessPerson(new Customer("Bob", "VIP"));
            ProcessPerson(new Manager("Carol", 10));

            // ── REAL-WORLD EXAMPLE: Validation Logic ─────────────────────────
            // Output: --- Real-World Scenario: Input Validation ---
            Console.WriteLine("\n--- Real-World Scenario: Input Validation ---");
            
            // Validate different input types
            ValidateInput("Hello");
            ValidateInput(123);
            ValidateInput(3.14);
            ValidateInput(null);
            ValidateInput(new[] { 1, 2, 3 });

            Console.WriteLine("\n=== Is Pattern Complete ===");
        }

        /// <summary>
        /// Processes person objects using is pattern matching
        /// </summary>
        public static void ProcessPerson(object person)
        {
            // Type check with property access
            if (person is Employee emp)
            {
                // Employee has Department property
                // Output: Employee: Alice works in Engineering
                Console.WriteLine($"   Employee: {emp.Name} works in {emp.Department}");
            }
            else if (person is Customer cust)
            {
                // Customer has Tier property
                // Output: Customer: Bob is a VIP customer
                Console.WriteLine($"   Customer: {cust.Name} is a {cust.Tier} customer");
            }
            else if (person is Manager mgr)
            {
                // Manager has TeamSize property
                // Output: Manager: Carol leads 10 team members
                Console.WriteLine($"   Manager: {mgr.Name} leads {mgr.TeamSize} team members");
            }
        }

        /// <summary>
        /// Real-world: Validates input based on type
        /// </summary>
        public static void ValidateInput(object input)
        {
            // is string = check for string type
            if (input is string str && str.Length > 0)
            {
                // Output: Valid string: [value]
                Console.WriteLine($"   Valid string: {str}");
            }
            // is int and positive = check int with condition
            else if (input is int i && i > 0)
            {
                // Output: Valid positive integer: [value]
                Console.WriteLine($"   Valid positive integer: {i}");
            }
            // is double in range = check double with range
            else if (input is double d && d > 0 && d < 1000)
            {
                // Output: Valid double in range: [value]
                Console.WriteLine($"   Valid double in range: {d}");
            }
            // is null check
            else if (input is null)
            {
                // Output: Input is null
                Console.WriteLine($"   Input is null");
            }
            // is array with elements
            else if (input is Array arr && arr.Length > 0)
            {
                // Output: Valid array with [n] elements
                Console.WriteLine($"   Valid array with {arr.Length} elements");
            }
            else
            {
                // Output: Invalid input
                Console.WriteLine($"   Invalid input: {input}");
            }
        }
    }

    // ── REAL-WORLD EXAMPLE: Person Classes ─────────────────────────────────
    /// <summary>
    /// Employee person type with department
    /// </summary>
    public class Employee
    {
        public string Name { get; }
        public string Department { get; }

        public Employee(string name, string dept)
        {
            Name = name;
            Department = dept;
        }
    }

    /// <summary>
    /// Customer person type with tier
    /// </summary>
    public class Customer
    {
        public string Name { get; }
        public string Tier { get; }

        public Customer(string name, string tier)
        {
            Name = name;
            Tier = tier;
        }
    }

    /// <summary>
    /// Manager person type with team size
    /// </summary>
    public class Manager
    {
        public string Name { get; }
        public int TeamSize { get; }

        public Manager(string name, int teamSize)
        {
            Name = name;
            TeamSize = teamSize;
        }
    }
}
