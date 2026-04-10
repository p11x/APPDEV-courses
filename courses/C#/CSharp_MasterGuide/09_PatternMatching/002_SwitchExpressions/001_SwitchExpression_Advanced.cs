/*
 * ============================================================
 * TOPIC     : Pattern Matching
 * SUBTOPIC  : Switch Expressions - Advanced
 * FILE      : 01_SwitchExpression_Advanced.cs
 * PURPOSE   : Demonstrates advanced switch expression patterns including relational, logical, and combined patterns
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._09_PatternMatching._02_SwitchExpressions
{
    /// <summary>
    /// Demonstrates advanced switch expression patterns
    /// </summary>
    public class SwitchExpression_Advanced
    {
        /// <summary>
        /// Entry point for advanced switch expression examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Advanced Switch Expression Demo ===
            Console.WriteLine("=== Advanced Switch Expression Demo ===\n");

            // ── CONCEPT: Relational Patterns ────────────────────────────────
            // Relational patterns compare against constants: <, >, <=, >=

            // Example 1: Relational patterns with numbers
            // Output: 1. Relational Patterns:
            Console.WriteLine("1. Relational Patterns:");
            
            // Test different temperature values
            // ClassifyTemperature returns temperature category
            Console.WriteLine($"   -5°C: {ClassifyTemperature(-5)}");
            Console.WriteLine($"   0°C: {ClassifyTemperature(0)}");
            Console.WriteLine($"   15°C: {ClassifyTemperature(15)}");
            Console.WriteLine($"   25°C: {ClassifyTemperature(25)}");
            Console.WriteLine($"   40°C: {ClassifyTemperature(40)}");

            // Example 2: Grade classification with relational patterns
            // Output: 2. Grade Classification:
            Console.WriteLine("\n2. Grade Classification:");
            
            // int[] scores = test score values
            int[] scores = { 45, 65, 75, 85, 95 };
            
            // foreach = iterate through scores
            foreach (int score in scores)
            {
                // GetGradeLetter returns letter grade based on score
                // Output: Score [n]: [grade]
                Console.WriteLine($"   Score {score}: {GetGradeLetter(score)}");
            }

            // ── CONCEPT: Logical Patterns ───────────────────────────────────
            // Logical patterns combine conditions: and (&&), or (||), not (!)

            // Example 3: Logical patterns combining conditions
            // Output: 3. Logical Patterns:
            Console.WriteLine("\n3. Logical Patterns:");
            
            // Test number with multiple conditions
            // DescribeNumber returns description of number
            Console.WriteLine($"   0: {DescribeNumber(0)}");
            Console.WriteLine($"   5: {DescribeNumber(5)}");
            Console.WriteLine($"   100: {DescribeNumber(100)}");
            Console.WriteLine($"   -5: {DescribeNumber(-5)}");
            Console.WriteLine($"   50: {DescribeNumber(50)}");

            // ── CONCEPT: Conjunctive (and) Patterns ────────────────────────
            // and pattern requires both conditions to be true

            // Example 4: Conjunctive patterns
            // Output: 4. Conjunctive (and) Patterns:
            Console.WriteLine("\n4. Conjunctive (and) Patterns:");
            
            // Test ranges with both conditions
            // string GetRangeDescription returns range description
            Console.WriteLine($"   50: {GetRangeDescription(50)}");
            Console.WriteLine($"   150: {GetRangeDescription(150)}");
            Console.WriteLine($"   500: {GetRangeDescription(500)}");
            Console.WriteLine($"   1000: {GetRangeDescription(1000)}");

            // ── CONCEPT: Disjunctive (or) Patterns ────────────────────────
            // or pattern matches if either condition is true

            // Example 5: Disjunctive patterns
            // Output: 5. Disjunctive (or) Patterns:
            Console.WriteLine("\n5. Disjunctive (or) Patterns:");
            
            // Test values matching multiple categories
            // GetCategory returns category for values
            Console.WriteLine($"   1: {GetCategory(1)}");
            Console.WriteLine($"   5: {GetCategory(5)}");
            Console.WriteLine($"   10: {GetCategory(10)}");
            Console.WriteLine($"   15: {GetCategory(15)}");
            Console.WriteLine($"   20: {GetCategory(20)}");

            // ── CONCEPT: Negated (!) Patterns ───────────────────────────────
            // !pattern matches when pattern does NOT match

            // Example 6: Negated patterns
            // Output: 6. Negated Patterns:
            Console.WriteLine("\n6. Negated Patterns:");
            
            // Test values excluding certain conditions
            // GetStatus returns status based on conditions
            Console.WriteLine($"   0: {GetStatus(0)}");
            Console.WriteLine($"   50: {GetStatus(50)}");
            Console.WriteLine($"   100: {GetStatus(100)}");

            // ── CONCEPT: Combined Real-World Example ────────────────────────
            // Output: --- Real-World: Shipping Calculator ---
            Console.WriteLine("\n--- Real-World: Shipping Calculator ---");
            
            // Calculate shipping based on weight and distance
            // CalculateShipping returns shipping cost
            Console.WriteLine($"   Small package (2kg, 50km): ${CalculateShipping(2, 50):F2}");
            Console.WriteLine($"   Medium package (5kg, 100km): ${CalculateShipping(5, 100):F2}");
            Console.WriteLine($"   Large package (15kg, 200km): ${CalculateShipping(15, 200):F2}");
            Console.WriteLine($"   Heavy package (50kg, 500km): ${CalculateShipping(50, 500):F2}");

            Console.WriteLine("\n=== Advanced Switch Expression Complete ===");
        }

        /// <summary>
        /// Classifies temperature using relational patterns
        /// </summary>
        public static string ClassifyTemperature(int celsius)
        {
            // switch expression with relational patterns
            return celsius switch
            {
                // < 0 = below freezing
                < 0 => "Freezing",
                
                // 0 = at freezing point
                0 => "At Freezing",
                
                // > 0 and < 10 = cold
                > 0 and < 10 => "Cold",
                
                // >= 10 and < 25 = comfortable
                >= 10 and < 25 => "Comfortable",
                
                // >= 25 and < 35 = warm
                >= 25 and < 35 => "Warm",
                
                // >= 35 = hot
                >= 35 => "Hot"
            };
        }

        /// <summary>
        /// Returns letter grade using relational patterns
        /// </summary>
        public static string GetGradeLetter(int score)
        {
            // Pattern: score >= threshold => grade letter
            return score switch
            {
                >= 90 => "A",
                >= 80 => "B",
                >= 70 => "C",
                >= 60 => "D",
                _ => "F"
            };
        }

        /// <summary>
        /// Describes number properties using logical patterns
        /// </summary>
        public static string DescribeNumber(int n)
        {
            // Combine multiple conditions with and/or
            return n switch
            {
                // == 0 = zero
                0 => "Zero",
                
                // > 0 and <= 10 = small positive
                > 0 and <= 10 => "Small positive",
                
                // > 10 and <= 100 = medium positive
                > 10 and <= 100 => "Medium positive",
                
                // > 100 = large positive
                > 100 => "Large positive",
                
                // < 0 and >= -10 = small negative
                < 0 and >= -10 => "Small negative",
                
                // < -10 = large negative
                < -10 => "Large negative",
                
                // _ = default (should never reach here with int)
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Describes numeric ranges using conjunctive patterns
        /// </summary>
        public static string GetRangeDescription(int value)
        {
            // and pattern requires both conditions
            return value switch
            {
                // >= 0 and < 100 = range 0-99
                >= 0 and < 100 => "Range 0-99",
                
                // >= 100 and < 500 = range 100-499
                >= 100 and < 500 => "Range 100-499",
                
                // >= 500 and < 1000 = range 500-999
                >= 500 and < 1000 => "Range 500-999",
                
                // >= 1000 = 1000 or greater
                >= 1000 => "Range 1000+",
                
                // Default case (negative values)
                _ => "Negative"
            };
        }

        /// <summary>
        /// Categorizes values using disjunctive patterns
        /// </summary>
        public static string GetCategory(int value)
        {
            // or pattern matches if either condition is true
            return value switch
            {
                // 1 or 2 or 3 = low category
                1 or 2 or 3 => "Low",
                
                // 4 or 5 or 6 = medium category
                4 or 5 or 6 => "Medium",
                
                // 7 or 8 or 9 = high category
                7 or 8 or 9 => "High",
                
                // 10 or anything else = extreme
                10 or _ => "Extreme"
            };
        }

        /// <summary>
        /// Gets status using negated patterns
        /// </summary>
        public static string GetStatus(int value)
        {
            // ! pattern negates a condition
            return value switch
            {
                // == 0 = inactive
                0 => "Inactive",
                
                // !0 and <= 50 = active with low value
                // Note: Can't use ! with relational, so use range
                >= 1 and <= 50 => "Active (Low)",
                
                // > 50 and <= 100 = active with high value
                > 50 and <= 100 => "Active (High)",
                
                // _ = unknown
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Real-world: Calculates shipping cost based on weight and distance
        /// </summary>
        public static double CalculateShipping(double weightKg, double distanceKm)
        {
            // Combine weight and distance patterns
            // Base rate + weight fee + distance fee
            return (weightKg, distanceKm) switch
            {
                // Small and close = cheap shipping
                // weight <= 3 and distance <= 100
                (<= 3 and <= 100) => 5.00,
                
                // Small and medium distance
                (<= 3 and > 100 and <= 500) => 10.00,
                
                // Medium weight (3-10kg) and close
                (> 3 and <= 10 and <= 100) => 8.00,
                
                // Medium weight and medium distance
                (> 3 and <= 10 and > 100 and <= 500) => 15.00,
                
                // Large weight (>10kg) or long distance (>500km) = premium
                (> 10 or > 500) => 25.00,
                
                // Default = standard rate
                _ => 12.00
            };
        }
    }
}
