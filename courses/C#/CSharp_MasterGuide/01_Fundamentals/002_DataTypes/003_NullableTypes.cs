/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Data Types - Nullable Types
 * FILE      : NullableTypes.cs
 * PURPOSE   : This file provides comprehensive coverage of nullable value types in C#.
 *             Explains how to declare, access, and work with nullable types.
 * ============================================================
 */

// --- SECTION: Nullable Value Types Overview ---
// Value types cannot be null by default, but nullable types allow them to hold null
// This is essential for representing "no value" or "missing" in databases and APIs
// Syntax: Add ? after the type (e.g., int?)

// System.Nullable<T> is the underlying struct for nullable types
// It wraps the value type in a struct that HasValue and Value properties

using System;

namespace CSharp_MasterGuide._01_Fundamentals._02_DataTypes
{
    class NullableTypes
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Declaring Nullable Types
            // ═══════════════════════════════════════════════════════════════
            
            // ── Basic nullable declarations ──────────────────────────────────
            // Nullable int can hold an integer OR null (no value)
            int? nullableAge = null;           // Explicit nullable syntax
            Nullable<int> explicitNullable = null; // Longhand form (rarely used)
            
            Console.WriteLine($"nullableAge: {nullableAge}"); // Output: nullableAge: 
            Console.WriteLine($"explicitNullable: {explicitNullable}"); // Output: explicitNullable: 
            
            // Nullable double for scientific calculations that might fail
            double? temperature = null; // No temperature reading available
            Console.WriteLine($"Temperature: {temperature ?? "Not available"}"); 
            // Output: Temperature: Not available
            
            // Nullable bool - useful for tri-state logic
            bool? hasPermission = null; // Permission not yet determined (unknown)
            Console.WriteLine($"Has permission: {hasPermission}");
            // Output: Has permission: 
            
            // Nullable for database fields (common use case)
            int? customerId = null; // Customer not assigned yet
            DateTime? orderDate = null; // Order not yet placed
            decimal? discount = null; // No discount applied

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Checking and Accessing Nullable Values
            // ═══════════════════════════════════════════════════════════════
            
            // ── HasValue property ───────────────────────────────────────────
            // HasValue returns true if the nullable has a value, false if null
            int? score = 85;
            if (score.HasValue) // Check if score has a value
            {
                Console.WriteLine($"Score has value: {score.Value}"); // Output: Score has value: 85
            }
            
            score = null; // Set to null
            if (!score.HasValue) // Check if null
            {
                Console.WriteLine("Score has no value (null)"); // Output: Score has no value (null)
            }
            
            // ── Value property ──────────────────────────────────────────────
            // Value returns the actual value - throws InvalidOperationException if null!
            int? count = 100;
            int actualCount = count.Value; // Safe because we know it has a value
            Console.WriteLine($"Count: {actualCount}"); // Output: Count: 100
            
            // DANGER: count = null; int bad = count.Value; // Would throw InvalidOperationException!
            
            // ── Null-conditional operator (?.) ──────────────────────────────
            // Safely access Value with ?. operator - returns null if nullable is null
            int? nullableInt = null;
            int? result = nullableInt?.Value; // Returns null instead of throwing
            Console.WriteLine($"Result with ?. : {result ?? "was null"}"); // Output: Result with ?. : was null
            
            nullableInt = 42;
            result = nullableInt?.Value; // Returns 42
            Console.WriteLine($"Result with ?. : {result}"); // Output: Result with ?. : 42

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Null-Coalescing Operators
            // ═══════════════════════════════════════════════════════════════
            
            // ── Null-coalescing operator (??) ──────────────────────────────
            // Returns left operand if not null, otherwise returns right operand
            int? optionalId = null;
            int id = optionalId ?? 0; // Use 0 as default
            Console.WriteLine($"ID with default: {id}"); // Output: ID with default: 0
            
            optionalId = 42;
            id = optionalId ?? 0; // Uses the actual value
            Console.WriteLine($"ID with value: {id}"); // Output: ID with value: 42
            
            // ── Null-coalescing assignment (??=) - C# 8.0+ ─────────────────
            // Assigns right operand only if left is null
            double? price = null;
            price ??= 9.99; // Assign default if null
            Console.WriteLine($"Price: {price}"); // Output: Price: 9.99
            
            price = 19.99; // Set to non-null value
            price ??= 9.99; // Does nothing because price is not null
            Console.WriteLine($"Price after ??=: {price}"); // Output: Price after ??=: 19.99
            
            // ── Chained null-coalescing ──────────────────────────────────────
            string? first = null;
            string? second = null;
            string? third = "Hello";
            
            string result2 = first ?? second ?? third ?? "Default";
            Console.WriteLine($"Chained coalescing: {result2}"); // Output: Chained coalescing: Hello

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Nullable Arithmetic Operations
            // ═══════════════════════════════════════════════════════════════
            
            // ── Arithmetic with nullable ────────────────────────────────────
            // Operations with at least one null operand result in null
            int? a = 10;
            int? b = 5;
            int? c = null;
            
            Console.WriteLine($"a + b = {a + b}"); // Output: a + b = 15
            Console.WriteLine($"a + c = {a + c}"); // Output: a + c =  (null)
            Console.WriteLine($"a - b = {a - b}"); // Output: a - b = 5
            Console.WriteLine($"a - c = {a - c}"); // Output: a - c =  (null)
            Console.WriteLine($"a * b = {a * b}"); // Output: a * b = 50
            Console.WriteLine($"a * c = {a * c}"); // Output: a * c =  (null)
            
            // Division with nullable
            Console.WriteLine($"a / b = {a / b}"); // Output: a / b = 2
            Console.WriteLine($"c / a = {c / a}"); // Output: c / a =  (null)
            
            // ── Comparison with nullable ─────────────────────────────────────
            // Comparisons with null always result in false (except ?? and == null)
            int? x = 10;
            int? y = null;
            
            Console.WriteLine($"x > 5: {x > 5}"); // Output: x > 5: True
            Console.WriteLine($"y > 5: {y > 5}"); // Output: y > 5: False
            Console.WriteLine($"x == 10: {x == 10}"); // Output: x == 10: True
            Console.WriteLine($"y == 10: {y == 10}"); // Output: y == 10: False
            Console.WriteLine($"y == null: {y == null}"); // Output: y == null: True
            Console.WriteLine($"x != null: {x != null}"); // Output: x != null: True

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Nullable Type Methods (GetValueOrDefault)
            // ═══════════════════════════════════════════════════════════════
            
            // ── GetValueOrDefault ──────────────────────────────────────────
            // Returns the value if present, otherwise returns specified default
            int? count2 = null;
            int countValue = count2.GetValueOrDefault(); // Returns 0 (default for int)
            Console.WriteLine($"GetValueOrDefault(): {countValue}"); // Output: GetValueOrDefault(): 0
            
            int countValue2 = count2.GetValueOrDefault(-1); // Return -1 as default
            Console.WriteLine($"GetValueOrDefault(-1): {countValue2}"); // Output: GetValueOrDefault(-1): -1
            
            count2 = 50;
            int countValue3 = count2.GetValueOrDefault(-1);
            Console.WriteLine($"GetValueOrDefault with value: {countValue3}"); // Output: GetValueOrDefault with value: 50

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Nullable Underlying Type
            // ═══════════════════════════════════════════════════════════════
            
            // ── Getting the underlying type ────────────────────────────────
            // Use Nullable<T>.UnderlyingType to get the wrapped type
            int? nullableInt2 = 42;
            Type underlyingType = typeof(int?); // Get nullable type
            Console.WriteLine($"Nullable type: {underlyingType}"); // Output: Nullable type: System.Nullable`1[System.Int32]
            
            // Check if a type is nullable
            bool isNullable = Nullable.GetUnderlyingType(underlyingType) != null;
            Console.WriteLine($"Is nullable: {isNullable}"); // Output: Is nullable: True
            
            Type regularInt = typeof(int);
            bool isRegularNullable = Nullable.GetUnderlyingType(regularInt) != null;
            Console.WriteLine($"Is int nullable: {isRegularNullable}"); // Output: Is int nullable: False

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Nullable Scenarios
            // ═══════════════════════════════════════════════════════════════
            
            // ── Database nullable fields ───────────────────────────────────
            // When reading from database, nullable columns become nullable C# types
            // Simulated database record
            var userRecord = new 
            {
                Id = 1,
                Name = "Alice",
                Email = "alice@example.com",
                Phone = (string)null, // Phone not provided - represented as null
                Age = (int?)null      // Age not provided - represented as null
            };
            
            // Process with null-coalescing for defaults
            string phone = userRecord.Phone ?? "No phone on file";
            int? age = userRecord.Age;
            int ageOrDefault = age ?? 18; // Default to 18 if not provided
            
            Console.WriteLine($"Phone: {phone}"); // Output: Phone: No phone on file
            Console.WriteLine($"Age (or default 18): {ageOrDefault}"); // Output: Age (or default 18): 18
            
            // ── API optional parameters ──────────────────────────────────────
            // Many API parameters are optional and represented as nullable
            // Simulated API request
            int? pageNumber = null; // Not specified - use default
            int? pageSize = null;   // Not specified - use default
            
            int actualPage = pageNumber ?? 1; // Default to page 1
            int actualSize = pageSize ?? 10;  // Default to 10 items
            
            Console.WriteLine($"Page: {actualPage}, Size: {actualSize}"); // Output: Page: 1, Size: 10
            
            // ── Tri-state logic ──────────────────────────────────────────────
            // Nullable bool allows three states: true, false, unknown
            bool? userChoice = null; // User hasn't made a choice yet
            
            if (userChoice == true) // Explicitly checking for true
            {
                Console.WriteLine("User chose yes");
            }
            else if (userChoice == false) // Explicitly checking for false
            {
                Console.WriteLine("User chose no");
            }
            else // Null - unknown/not specified
            {
                Console.WriteLine("User choice not yet made");
            }
            // Output: User choice not yet made
        }
    }
}
