/*
 * ============================================================
 * TOPIC     : Pattern Matching
 * SUBTOPIC  : Switch Expressions - Advanced Part 2
 * FILE      : 02_SwitchExpression_Advanced_Part2.cs
 * PURPOSE   : Continues advanced switch expression patterns with tuple patterns, var patterns, and recursion
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._09_PatternMatching._02_SwitchExpressions
{
    /// <summary>
    /// Continues advanced switch expression patterns
    /// </summary>
    public class SwitchExpression_Advanced_Part2
    {
        /// <summary>
        /// Entry point for more advanced patterns
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Advanced Switch Expression Part 2 ===
            Console.WriteLine("=== Advanced Switch Expression Part 2 ===\n");

            // ── CONCEPT: Tuple Patterns ──────────────────────────────────────
            // Match on multiple values using tuple syntax

            // Example 1: Tuple patterns
            // Output: 1. Tuple Patterns:
            Console.WriteLine("1. Tuple Patterns:");
            
            // Test coordinate quadrants
            // GetQuadrant returns which quadrant the point is in
            Console.WriteLine($"   (1, 1): {GetQuadrant(1, 1)}");
            Console.WriteLine($"   (-1, 1): {GetQuadrant(-1, 1)}");
            Console.WriteLine($"   (-1, -1): {GetQuadrant(-1, -1)}");
            Console.WriteLine($"   (1, -1): {GetQuadrant(1, -1)}");
            Console.WriteLine($"   (0, 5): {GetQuadrant(0, 5)}");

            // Example 2: Tuple pattern with range conditions
            // Output: 2. Tuple Patterns with Ranges:
            Console.WriteLine("\n2. Tuple Patterns with Ranges:");
            
            // ClassifyPoint determines point location type
            // ClassifyPointLocation returns location classification
            Console.WriteLine($"   (0, 0): {ClassifyPointLocation(0, 0)}");
            Console.WriteLine($"   (50, 50): {ClassifyPointLocation(50, 50)}");
            Console.WriteLine($"   (150, 50): {ClassifyPointLocation(150, 50)}");
            Console.WriteLine($"   (-50, -50): {ClassifyPointLocation(-50, -50)}");

            // ── CONCEPT: Positional Patterns ─────────────────────────────────
            // Match on tuple Deconstruct positions

            // Example 3: Positional patterns
            // Output: 3. Positional Patterns:
            Console.WriteLine("\n3. Positional Patterns:");
            
            // Test with Deconstruct-enabled types
            // GetColorDescription returns color info
            var rgb = (Red: 255, Green: 128, Blue: 64);
            var gray = (Red: 128, Green: 128, Blue: 128);
            var black = (Red: 0, Green: 0, Blue: 0);
            
            Console.WriteLine($"   {rgb}: {GetColorDescription(rgb)}");
            Console.WriteLine($"   {gray}: {GetColorDescription(gray)}");
            Console.WriteLine($"   {black}: {GetColorDescription(black)}");

            // ── CONCEPT: Var Patterns in Switch ────────────────────────────────
            // Var pattern captures any value

            // Example 4: Var patterns
            // Output: 4. Var Patterns:
            Console.WriteLine("\n4. Var Patterns:");
            
            // Process different value types
            // DescribeValue returns description
            Console.WriteLine($"   42: {DescribeValue(42)}");
            Console.WriteLine($"   \"hello\": {DescribeValue("hello")}");
            Console.WriteLine($"   3.14: {DescribeValue(3.14)}");

            // ── CONCEPT: Recursive Patterns ───────────────────────────────────
            // Nested patterns for complex matching

            // Example 5: Recursive/nested patterns
            // Output: 5. Recursive Patterns:
            Console.WriteLine("\n5. Recursive Patterns:");
            
            // Process nested objects
            // DescribeResult returns nested result description
            var success = (Status: "success", Data: (Value: 100, Message: "OK"));
            var error = (Status: "error", Data: (Value: 0, Message: "Failed"));
            var empty = (Status: "empty", Data: (Value: -1, Message: "No data"));
            
            Console.WriteLine($"   {success.Status}: {DescribeResult(success)}");
            Console.WriteLine($"   {error.Status}: {DescribeResult(error)}");
            Console.WriteLine($"   {empty.Status}: {DescribeResult(empty)}");

            // ── CONCEPT: List Patterns ────────────────────────────────────────
            // Match on array/list elements

            // Example 6: List patterns (C# 11+)
            // Output: 6. List Patterns:
            Console.WriteLine("\n6. List Patterns:");
            
            // Test list patterns (simulated with arrays)
            // MatchFirstLast returns description of list
            int[] single = { 1 };
            int[] two = { 1, 2 };
            int[] three = { 1, 2, 3 };
            int[] many = { 1, 2, 3, 4, 5 };
            
            Console.WriteLine($"   [1]: {MatchFirstLast(single)}");
            Console.WriteLine($"   [1,2]: {MatchFirstLast(two)}");
            Console.WriteLine($"   [1,2,3]: {MatchFirstLast(three)}");
            Console.WriteLine($"   [1,2,3,4,5]: {MatchFirstLast(many)}");

            // ── REAL-WORLD EXAMPLE: FizzBuzz with Switch ─────────────────────
            // Output: --- Real-World: FizzBuzz Calculator ---
            Console.WriteLine("\n--- Real-World: FizzBuzz Calculator ---");
            
            // Classic FizzBuzz using switch expression
            for (int i = 1; i <= 20; i++)
            {
                // FizzBuzz returns "Fizz", "Buzz", "FizzBuzz", or number
                Console.Write($"   {i}: {FizzBuzz(i)}");
                if (i % 5 == 0) Console.WriteLine();
            }

            Console.WriteLine("\n=== Advanced Switch Expression Part 2 Complete ===");
        }

        /// <summary>
        /// Determines quadrant using tuple pattern
        /// </summary>
        public static string GetQuadrant(int x, int y)
        {
            // (x, y) tuple pattern matching
            return (x, y) switch
            {
                // Both positive = quadrant I
                (> 0, > 0) => "Quadrant I",
                
                // x negative, y positive = quadrant II
                (< 0, > 0) => "Quadrant II",
                
                // Both negative = quadrant III
                (< 0, < 0) => "Quadrant III",
                
                // x positive, y negative = quadrant IV
                (> 0, < 0) => "Quadrant IV",
                
                // x = 0 = on Y axis
                (0, _) => "On Y-axis",
                
                // y = 0 = on X axis
                (_, 0) => "On X-axis",
                
                // Both = 0 = origin
                _ => "Origin"
            };
        }

        /// <summary>
        /// Classifies point location using ranges
        /// </summary>
        public static string ClassifyPointLocation(int x, int y)
        {
            // Combine ranges in tuple pattern
            return (x, y) switch
            {
                // Both in center range (50-100)
                (>= 50 and <= 100, >= 50 and <= 100) => "Center",
                
                // x in center, y outside
                (>= 50 and <= 100, _) => "Center-Column",
                
                // y in center, x outside
                (_, >= 50 and <= 100) => "Center-Row",
                
                // All other = outer
                _ => "Outer"
            };
        }

        /// <summary>
        /// Describes color using positional pattern
        /// </summary>
        public static string GetColorDescription((int Red, int Green, int Blue) color)
        {
            // Positional pattern matching on tuple Deconstruct
            return color switch
            {
                // All equal = grayscale
                (var r, var g, var b) when r == g && g == b && r < 128 => "Dark Gray",
                (var r, var g, var b) when r == g && g == b => "Light Gray",
                
                // R=255, G=0, B=0 = red
                (255, 0, 0) => "Red",
                
                // R=0, G=255, B=0 = green
                (0, 255, 0) => "Green",
                
                // R=0, G=0, B=255 = blue
                (0, 0, 255) => "Blue",
                
                // R > G and R > B = warm (mostly red)
                (var r, var g, var b) when r > g && r > b => "Warm Color",
                
                // G > R and G > B = cool (mostly green)
                (var r, var g, var b) when g > r && g > b => "Cool Color",
                
                // B > R and B > G = cool (mostly blue)
                (var r, var g, var b) when b > r && b > g => "Cool Color",
                
                // Default = mixed
                _ => "Mixed Color"
            };
        }

        /// <summary>
        /// Describes value using var pattern
        /// </summary>
        public static string DescribeValue(object value)
        {
            // Var pattern captures anything
            return value switch
            {
                // int i = captures any integer
                int i => $"Integer: {i}",
                
                // string s = captures any string
                string s => $"String: {s} (length {s.Length})",
                
                // double d = captures any double
                double d => $"Double: {d:F2}",
                
                // var v = captures anything else (fallback)
                var v => $"Other: {v?.GetType().Name}"
            };
        }

        /// <summary>
        /// Describes nested result using recursive pattern
        /// </summary>
        public static string DescribeResult((string Status, (int Value, string Message) Data) result)
        {
            // Recursive: match outer tuple, then inner tuple
            return result switch
            {
                // Status = "success" and Value > 0
                ("success", (> 0, _)) => $"Success: {result.Data.Message}",
                
                // Status = "error"
                ("error", _) => $"Error: {result.Data.Message}",
                
                // Status = "empty" or Value < 0
                ("empty", _) => $"Empty: {result.Data.Message}",
                
                // Default
                _ => $"Unknown: {result.Status}"
            };
        }

        /// <summary>
        /// Matches first and last elements using list pattern
        /// </summary>
        public static string MatchFirstLast(int[] numbers)
        {
            // List pattern matches array elements
            return numbers switch
            {
                // Single element
                [_] => "Single element",
                
                // Two elements
                [var first, var last] => $"First: {first}, Last: {last}",
                
                // First is 1, any middle, last is 3
                [1, .., 3] => "Starts with 1, ends with 3",
                
                // First is 1, has more elements
                [1, ..] => "Starts with 1",
                
                // Last is 5
                [.., 5] => "Ends with 5",
                
                // Anything else
                _ => "Other"
            };
        }

        /// <summary>
        /// FizzBuzz using switch expression
        /// </summary>
        public static string FizzBuzz(int n)
        {
            // Classic FizzBuzz logic
            return (n % 3, n % 5) switch
            {
                // Both divisible = FizzBuzz
                (0, 0) => "FizzBuzz",
                
                // Only divisible by 3 = Fizz
                (0, _) => "Fizz",
                
                // Only divisible by 5 = Buzz
                (_, 0) => "Buzz",
                
                // Neither = number as string
                _ => n.ToString()
            };
        }
    }
}
