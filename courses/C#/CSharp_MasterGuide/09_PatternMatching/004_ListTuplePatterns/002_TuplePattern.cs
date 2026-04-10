/*
 * ============================================================
 * TOPIC     : Pattern Matching
 * SUBTOPIC  : Tuple Patterns
 * FILE      : 02_TuplePattern.cs
 * PURPOSE   : Demonstrates tuple patterns for matching multiple values simultaneously
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._09_PatternMatching._04_ListTuplePatterns
{
    /// <summary>
    /// Demonstrates tuple pattern matching in C#
    /// </summary>
    public class TuplePattern
    {
        /// <summary>
        /// Entry point for tuple pattern examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Tuple Pattern Matching Demo ===
            Console.WriteLine("=== Tuple Pattern Matching Demo ===\n");

            // ── CONCEPT: Basic Tuple Patterns ────────────────────────────────
            // Match on tuple values directly

            // Example 1: Basic tuple pattern
            // Output: 1. Basic Tuple Patterns:
            Console.WriteLine("1. Basic Tuple Patterns:");
            
            // (int, int) = tuple with two integers
            var point1 = (10, 20);
            var point2 = (0, 0);
            var point3 = (-5, 15);
            
            // GetQuadrant returns quadrant description
            Console.WriteLine($"   (10, 20): {GetQuadrant(point1)}");
            Console.WriteLine($"   (0, 0): {GetQuadrant(point2)}");
            Console.WriteLine($"   (-5, 15): {GetQuadrant(point3)}");

            // ── CONCEPT: Tuple with Type Patterns ────────────────────────────
            // Combine tuple with type patterns

            // Example 2: Tuple with type patterns
            // Output: 2. Tuple with Type Patterns:
            Console.WriteLine("\n2. Tuple with Type Patterns:");
            
            // (object, object) = tuple with mixed types
            var mixed1 = (42, "hello");
            var mixed2 = (3.14, true);
            var mixed3 = ("test", 100);
            
            // DescribeTuple returns type description
            Console.WriteLine($"   (42, \"hello\"): {DescribeTuple(mixed1)}");
            Console.WriteLine($"   (3.14, true): {DescribeTuple(mixed2)}");
            Console.WriteLine($"   (\"test\", 100): {DescribeTuple(mixed3)}");

            // ── CONCEPT: Tuple with Relational Patterns ──────────────────────
            // Use relational patterns on tuple values

            // Example 3: Tuple with relational patterns
            // Output: 3. Tuple with Relational Patterns:
            Console.WriteLine("\n3. Tuple with Relational Patterns:");
            
            // Test various score combinations
            var score1 = (80, 90);  // Good, Good
            var score2 = (50, 60);  // Low, Low
            var score3 = (95, 45); // High, Low
            
            // GetGradeSummary returns grade description
            Console.WriteLine($"   (80, 90): {GetGradeSummary(score1)}");
            Console.WriteLine($"   (50, 60): {GetGradeSummary(score2)}");
            Console.WriteLine($"   (95, 45): {GetGradeSummary(score3)}");

            // ── CONCEPT: Discard Pattern in Tuples ───────────────────────────
            // Use _ to ignore specific tuple positions

            // Example 4: Discard patterns in tuples
            // Output: 4. Discard Patterns in Tuples:
            Console.WriteLine("\n4. Discard Patterns in Tuples:");
            
            // (string, int, string) = name, age, city
            var person1 = ("Alice", 30, "NYC");
            var person2 = ("Bob", 25, "LA");
            var person3 = ("Charlie", 35, "SF");
            
            // GetPersonInfo returns info
            Console.WriteLine($"   {person1}: {GetPersonInfo(person1)}");
            Console.WriteLine($"   {person2}: {GetPersonInfo(person2)}");
            Console.WriteLine($"   {person3}: {GetPersonInfo(person3)}");

            // ── CONCEPT: Named Tuple Patterns ────────────────────────────────
            // Match named tuple elements

            // Example 5: Named tuple patterns
            // Output: 5. Named Tuple Patterns:
            Console.WriteLine("\n5. Named Tuple Patterns:");
            
            // (int X, int Y) = named tuple
            var coords1 = (X: 10, Y: 20);
            var coords2 = (X: 0, Y: 100);
            var coords3 = (X: -5, Y: -10);
            
            // DescribeCoords returns description
            Console.WriteLine($"   (X:10, Y:20): {DescribeCoords(coords1)}");
            Console.WriteLine($"   (X:0, Y:100): {DescribeCoords(coords2)}");
            Console.WriteLine($"   (X:-5, Y:-10): {DescribeCoords(coords3)}");

            // ── REAL-WORLD EXAMPLE: Shipping Calculator ─────────────────────
            // Output: --- Real-World: Shipping Calculator ---
            Console.WriteLine("\n--- Real-World: Shipping Calculator ---");
            
            // (weight, distance, express) = shipping parameters
            var ship1 = (Weight: 2.0, Distance: 50, Express: false);
            var ship2 = (Weight: 15.0, Distance: 500, Express: false);
            var ship3 = (Weight: 1.0, Distance: 100, Express: true);
            var ship4 = (Weight: 30.0, Distance: 1000, Express: true);
            
            // CalculateShipping returns cost
            Console.WriteLine($"   2kg, 50km: ${CalculateShipping(ship1):F2}");
            Console.WriteLine($"   15kg, 500km: ${CalculateShipping(ship2):F2}");
            Console.WriteLine($"   1kg, 100km (express): ${CalculateShipping(ship3):F2}");
            Console.WriteLine($"   30kg, 1000km (express): ${CalculateShipping(ship4):F2}");

            Console.WriteLine("\n=== Tuple Pattern Complete ===");
        }

        /// <summary>
        /// Gets quadrant using basic tuple pattern
        /// </summary>
        public static string GetQuadrant((int X, int Y) point)
        {
            // Match on tuple values
            return point switch
            {
                // Both positive
                (> 0, > 0) => "Quadrant I",
                
                // X negative, Y positive
                (< 0, > 0) => "Quadrant II",
                
                // Both negative
                (< 0, < 0) => "Quadrant III",
                
                // X positive, Y negative
                (> 0, < 0) => "Quadrant IV",
                
                // Origin or on axis
                (0, 0) => "Origin",
                (0, _) => "On Y-axis",
                (_, 0) => "On X-axis"
            };
        }

        /// <summary>
        /// Describes tuple using type patterns
        /// </summary>
        public static string DescribeTuple((object First, object Second) tuple)
        {
            // Type pattern on tuple elements
            return tuple switch
            {
                // Both integers
                (int, int) => "Two integers",
                
                // Int and string
                (int, string) => "Integer and string",
                
                // String and int
                (string, int) => "String and integer",
                
                // Two strings
                (string, string) => "Two strings",
                
                // Double and bool
                (double, bool) => "Double and boolean",
                
                // Default
                _ => "Mixed types"
            };
        }

        /// <summary>
        /// Gets grade summary using relational patterns
        /// </summary>
        public static string GetGradeSummary((int Math, int English) scores)
        {
            // Relational patterns on tuple values
            return scores switch
            {
                // Both >= 70 = pass
                (>= 70, >= 70) => "Pass (Good)",
                
                // Either < 70 = needs improvement
                _ when scores.Math < 70 || scores.English < 70 => "Needs Improvement",
                
                // Default
                _ => "Other"
            };
        }

        /// <summary>
        /// Gets person info using discard pattern
        /// </summary>
        public static string GetPersonInfo((string Name, int Age, string City) person)
        {
            // Discard unused city
            return person switch
            {
                // Name starts with A = special
                (var name, _, _) when name.StartsWith("A") => $"Special: {name}",
                
                // Age > 30 = senior
                (_, > 30, _) => $"Senior: {person.Name}",
                
                // Default
                (var name, var age, _) => $"{name} ({age} years)"
            };
        }

        /// <summary>
        /// Describes coordinates using named tuple pattern
        /// </summary>
        public static string DescribeCoords((int X, int Y) coord)
        {
            // Named tuple pattern matching
            return coord switch
            {
                // X and Y are zero = origin
                (X: 0, Y: 0) => "Origin",
                
                // X is zero = on Y axis
                (X: 0, Y: _) => "On Y-axis",
                
                // Y is zero = on X axis
                (X: _, Y: 0) => "On X-axis",
                
                // Both positive = quadrant I
                (X: > 0, Y: > 0) => "Quadrant I",
                
                // X negative, Y positive = quadrant II
                (X: < 0, Y: > 0) => "Quadrant II",
                
                // Both negative = quadrant III
                (X: < 0, Y: < 0) => "Quadrant III",
                
                // X positive, Y negative = quadrant IV
                (X: > 0, Y: < 0) => "Quadrant IV"
            };
        }

        /// <summary>
        /// Real-world: Calculates shipping cost
        /// </summary>
        public static double CalculateShipping((double Weight, double Distance, bool Express) shipment)
        {
            // Tuple pattern with conditions
            return shipment switch
            {
                // Light and close, not express = cheap
                (<= 2, < 100, false) => 5.00,
                
                // Light and close, express = moderate
                (<= 2, < 100, true) => 12.00,
                
                // Medium weight, standard = medium
                (> 2 and <= 10, < 500, false) => 15.00,
                
                // Heavy = expensive
                (> 10, _, false) => 25.00,
                
                // Any express = premium
                (_, _, true) => 35.00,
                
                // Default
                _ => 10.00
            };
        }
    }
}
