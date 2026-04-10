/*
 * ============================================================
 * TOPIC     : Pattern Matching
 * SUBTOPIC  : Tuple Patterns (Continued)
 * FILE      : 03_TuplePattern_Part2.cs
 * PURPOSE   : Continues tuple patterns with more complex scenarios and real-world examples
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._09_PatternMatching._04_ListTuplePatterns
{
    /// <summary>
    /// Continues tuple pattern demonstrations
    /// </summary>
    public class TuplePattern_Part2
    {
        /// <summary>
        /// Entry point for advanced tuple patterns
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Tuple Pattern Part 2 ===
            Console.WriteLine("=== Tuple Pattern Part 2 ===\n");

            // ── CONCEPT: Nested Tuple Patterns ──────────────────────────────
            // Match on tuples containing tuples

            // Example 1: Nested tuple patterns
            // Output: 1. Nested Tuple Patterns:
            Console.WriteLine("1. Nested Tuple Patterns:");
            
            // (int, (int, int)) = outer with nested tuple
            var nested1 = (10, (20, 30));
            var nested2 = (5, (0, 0));
            var nested3 = (100, (50, 50));
            
            // DescribeNested returns description
            Console.WriteLine($"   (10, (20, 30)): {DescribeNested(nested1)}");
            Console.WriteLine($"   (5, (0, 0)): {DescribeNested(nested2)}");
            Console.WriteLine($"   (100, (50, 50)): {DescribeNested(nested3)}");

            // ── CONCEPT: Tuple with Property Patterns ────────────────────────
            // Combine tuple and property patterns

            // Example 2: Tuple with property patterns
            // Output: 2. Tuple with Property Patterns:
            Console.WriteLine("\n2. Tuple with Property Patterns:");
            
            // Create date tuple
            var date1 = (Year: 2024, Month: 1, Day: 1);
            var date2 = (Year: 2024, Month: 12, Day: 25);
            var date3 = (Year: 2024, Month: 7, Day: 4);
            
            // GetSeason returns season
            Console.WriteLine($"   Jan 1: {GetSeason(date1)}");
            Console.WriteLine($"   Dec 25: {GetSeason(date2)}");
            Console.WriteLine($"   Jul 4: {GetSeason(date3)}");

            // ── CONCEPT: Switch Statement with Tuples ───────────────────────
            // Traditional switch with tuple patterns

            // Example 3: Switch statement tuple patterns
            // Output: 3. Switch Statement Tuples:
            Console.WriteLine("\n3. Switch Statement Tuples:");
            
            // Test network response codes
            var response1 = (Code: 200, Message: "OK");
            var response2 = (Code: 404, Message: "Not Found");
            var response3 = (Code: 500, Message: "Error");
            var response4 = (Code: 301, Message: "Redirect");
            
            // GetHttpAction returns action
            Console.WriteLine($"   200 OK: {GetHttpAction(response1)}");
            Console.WriteLine($"   404 Not Found: {GetHttpAction(response2)}");
            Console.WriteLine($"   500 Error: {GetHttpAction(response3)}");
            Console.WriteLine($"   301 Redirect: {GetHttpAction(response4)}");

            // ── CONCEPT: Three-Element Tuple Patterns ───────────────────────
            // Match on three-value tuples

            // Example 4: Three-element tuples
            // Output: 4. Three-Element Tuples:
            Console.WriteLine("\n4. Three-Element Tuples:");
            
            // (r, g, b) = color components
            var color1 = (255, 0, 0);     // Red
            var color2 = (0, 255, 0);     // Green
            var color3 = (0, 0, 255);     // Blue
            var color4 = (128, 128, 128); // Gray
            
            // GetColorName returns color name
            Console.WriteLine($"   {color1}: {GetColorName(color1)}");
            Console.WriteLine($"   {color2}: {GetColorName(color2)}");
            Console.WriteLine($"   {color3}: {GetColorName(color3)}");
            Console.WriteLine($"   {color4}: {GetColorName(color4)}");

            // ── REAL-WORLD EXAMPLE: Time Clock Calculator ───────────────────
            // Output: --- Real-World: Time Clock Calculator ---
            Console.WriteLine("\n--- Real-World: Time Clock Calculator ---");
            
            // (hours, minutes, isPM) = time components
            var time1 = (9, 30, false);   // 9:30 AM
            var time2 = (9, 30, true);    // 9:30 PM
            var time3 = (12, 0, false);   // 12:00 PM (noon)
            var time4 = (12, 0, true);    // 12:00 AM (midnight)
            var time5 = (5, 45, true);    // 5:45 PM
            
            // FormatTime returns formatted time string
            Console.WriteLine($"   {FormatTime(time1)}");
            Console.WriteLine($"   {FormatTime(time2)}");
            Console.WriteLine($"   {FormatTime(time3)}");
            Console.WriteLine($"   {FormatTime(time4)}");
            Console.WriteLine($"   {FormatTime(time5)}");

            Console.WriteLine("\n=== Tuple Pattern Part 2 Complete ===");
        }

        /// <summary>
        /// Describes nested tuple
        /// </summary>
        public static string DescribeNested((int Outer, (int, int) Inner) nested)
        {
            // Match outer and inner tuple
            return nested switch
            {
                // Outer is 10, inner has any values
                (10, _) => "Outer is 10",
                
                // Outer is > 50 and inner second is 0
                (> 50, (_, 0)) => "Outer > 50, inner Y is 0",
                
                // Inner both equal (diagonal)
                (_, (var x, var y)) when x == y => $"Inner diagonal ({x}, {y})",
                
                // Default
                (var outer, (var inner1, var inner2)) => $"Outer: {outer}, Inner: ({inner1}, {inner2})"
            };
        }

        /// <summary>
        /// Gets season from date tuple
        /// </summary>
        public static string GetSeason((int Year, int Month, int Day) date)
        {
            // Property pattern combined with tuple
            return date switch
            {
                // Spring: March-May
                (_, 3 or 4 or 5, _) => "Spring",
                
                // Summer: June-August
                (_, 6 or 7 or 8, _) => "Summer",
                
                // Fall: September-November
                (_, 9 or 10 or 11, _) => "Fall",
                
                // Winter: December-February
                (_, 12 or 1 or 2, _) => "Winter",
                
                // Default
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Gets HTTP action from response
        /// </summary>
        public static string GetHttpAction((int Code, string Message) response)
        {
            // Switch statement with tuple pattern
            switch (response)
            {
                // 2xx = success
                (200, _):
                case (201, _):
                case (202, _):
                    return "Success";
                    
                // 3xx = redirect
                (301, _):
                case (302, _):
                case (304, _):
                    return "Redirect";
                    
                // 4xx = client error
                (400, _):
                case (401, _):
                case (403, _):
                case (404, _):
                    return "Client Error";
                    
                // 5xx = server error
                case (500, _):
                case (503, _):
                    return "Server Error";
                    
                default:
                    return "Unknown";
            }
        }

        /// <summary>
        /// Gets color name from RGB tuple
        /// </summary>
        public static string GetColorName((int R, int G, int B) color)
        {
            // Three-element tuple pattern
            return color switch
            {
                // Red = R=255, G=0, B=0
                (255, 0, 0) => "Red",
                
                // Green = R=0, G=255, B=0
                (0, 255, 0) => "Green",
                
                // Blue = R=0, G=0, B=255
                (0, 0, 255) => "Blue",
                
                // White = all max
                (255, 255, 255) => "White",
                
                // Black = all zero
                (0, 0, 0) => "Black",
                
                // Gray = all equal
                (var r, var g, var b) when r == g && g == b => "Gray",
                
                // Default = custom color
                _ => "Custom Color"
            };
        }

        /// <summary>
        /// Formats time from tuple
        /// </summary>
        public static string FormatTime((int Hours, int Minutes, bool IsPM) time)
        {
            // Tuple pattern with conditions
            return time switch
            {
                // Noon (12 PM)
                (12, 0, false) => "Noon",
                
                // Midnight (12 AM)
                (12, 0, true) => "Midnight",
                
                // Morning hours (1-11 AM)
                (var h, var m, false) when h >= 1 && h <= 11 => $"{h}:{m:D2} AM",
                
                // Afternoon hours (1-11 PM)
                (var h, var m, true) when h >= 1 && h <= 11 => $"{h}:{m:D2} PM",
                
                // Default = unknown
                _ => "Invalid time"
            };
        }
    }
}
