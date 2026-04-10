/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Control Flow - Ternary Operator
 * FILE      : TernaryOperator.cs
 * PURPOSE   : This file covers the conditional/ternary operator (? :) in C#.
 *             It's a concise way to make binary choices in expressions.
 * ============================================================
 */

// --- SECTION: Ternary Operator (? :) ---
// The ternary operator is a compact if-else that returns a value
// Syntax: condition ? trueValue : falseValue

using System;

namespace CSharp_MasterGuide._01_Fundamentals._05_ControlFlow
{
    class TernaryOperator
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Basic Ternary Operator
            // ═══════════════════════════════════════════════════════════════
            
            // Simple ternary
            int age = 20;
            string type = age >= 18 ? "Adult" : "Minor";
            Console.WriteLine($"Age {age}: {type}"); // Output: Age 20: Adult
            
            // With string values
            string status = age >= 18 ? "Eligible to vote" : "Not eligible";
            Console.WriteLine(status); // Output: Eligible to vote
            
            // Ternary returns a value - can be used anywhere
            Console.WriteLine(age >= 18 ? "Adult" : "Minor"); // Direct output

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Nested Ternary Operators
            // ═══════════════════════════════════════════════════════════════
            
            // Multiple conditions - nest carefully!
            int score = 75;
            string grade = score >= 90 ? "A" :
                          score >= 80 ? "B" :
                          score >= 70 ? "C" :
                          score >= 60 ? "D" : "F";
            
            Console.WriteLine($"Score {score}: Grade {grade}"); // Output: Score 75: Grade C
            
            // More readable version with intermediate variables
            string result;
            if (score >= 90)
                result = "A";
            else if (score >= 80)
                result = "B";
            else if (score >= 70)
                result = "C";
            else if (score >= 60)
                result = "D";
            else
                result = "F";
            
            Console.WriteLine($"Using if-else: {result}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Ternary with Different Types
            // ═══════════════════════════════════════════════════════════════
            
            // Both branches must be same type or compatible
            int value = 10;
            
            // Return numeric
            int max = value > 5 ? value : 5;
            Console.WriteLine($"Max: {max}"); // Output: Max: 10
            
            // Return string
            string label = value > 5 ? "High" : "Low";
            Console.WriteLine($"Label: {label}"); // Output: Label: High
            
            // Return bool
            bool isPositive = value > 0 ? true : false;
            Console.WriteLine($"Is positive: {isPositive}"); // Output: Is positive: True
            
            // Return object (different types - uses common base)
            object obj = value > 0 ? "positive" : "negative";
            Console.WriteLine($"Object: {obj}"); // Output: Object: positive

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Ternary in Expressions
            // ═══════════════════════════════════════════════════════════════
            
            // In method arguments
            PrintMessage(true);
            PrintMessage(false);
            
            // In string concatenation
            int a = 10, b = 20;
            Console.WriteLine($"Max of {a} and {b} is {a > b ? a : b}");
            
            // In array initialization
            int[] values = { 1, 2, 3, 4, 5 };
            int[] parity = values.Select(x => x % 2 == 0 ? 1 : -1).ToArray();
            Console.WriteLine($"Parity: {string.Join(", ", parity)}");
            
            // In LINQ
            var names = new List<string> { "Alice", "Bob", null, "Charlie" };
            var processed = names.Select(n => n == null ? "(null)" : n.ToUpper()).ToList();
            Console.WriteLine($"Processed: {string.Join(", ", processed)}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Ternary with Null Coalescing
            // ═══════════════════════════════════════════════════════════════
            
            // Combine with null-coalescing
            string? nullable = null;
            string output = nullable ?? "default";
            
            // Ternary with null check
            string? input = null;
            int length = input != null ? input.Length : -1;
            Console.WriteLine($"Length: {length}"); // Output: -1
            
            // Same result with null-conditional
            length = input?.Length ?? -1;
            Console.WriteLine($"Length with ?.: {length}"); // Output: -1

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Ternary in Assignment
            // ═══════════════════════════════════════════════════════════════
            
            // Simple assignment
            int x = 5, y = 10;
            int bigger = x > y ? x : y;
            
            // In for loop initialization
            for (int i = 0; i < (true ? 5 : 10); i++)
            {
                // Would iterate 5 times
            }
            
            // In object initialization
            var person = new 
            {
                Name = "John",
                IsAdult = 25 >= 18 ? true : false
            };
            Console.WriteLine($"IsAdult: {person.IsAdult}"); // Output: True

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── Formatting ────────────────────────────────────────────────
            int count = 1;
            string itemText = count == 1 ? "item" : "items";
            Console.WriteLine($"You have {count} {itemText}"); // Output: items (if count=1 outputs item)
            
            // With pluralization helper
            count = 5;
            Console.WriteLine($"Found {count} {Pluralize(count, "match", "matches")}");
            
            // ── Status messages ───────────────────────────────────────────
            bool isActive = true;
            string statusDisplay = isActive ? "Active" : "Inactive";
            Console.WriteLine($"Status: {statusDisplay}");
            
            // ── UI conditional rendering ─────────────────────────────────
            int userRole = 2; // 1=admin, 2=user, 3=guest
            string menuItems = userRole == 1 ? "All menu items" : 
                              userRole == 2 ? "Standard menu" : 
                              "Limited menu";
            Console.WriteLine($"Menu: {menuItems}");
            
            // ── Business logic ──────────────────────────────────────────
            decimal price = 100m;
            decimal discount = 0.15m;
            bool isPremium = false;
            
            decimal finalPrice = isPremium ? price * (1 - discount) : price;
            Console.WriteLine($"Final price: {finalPrice:C2}"); // Output: $100.00 (not premium)
            
            // ── Configuration selection ─────────────────────────────────
            string? env = Environment.GetEnvironmentVariable("MODE");
            string mode = env ?? "development";
            bool isDebug = mode == "development" ? true : false;
            Console.WriteLine($"Mode: {mode}, Debug: {isDebug}");
        }
        
        static void PrintMessage(bool condition)
        {
            Console.WriteLine(condition ? "Condition is true!" : "Condition is false");
        }
        
        // Helper for pluralization
        static string Pluralize(int count, string singular, string plural)
        {
            return count == 1 ? singular : plural;
        }
    }
}
