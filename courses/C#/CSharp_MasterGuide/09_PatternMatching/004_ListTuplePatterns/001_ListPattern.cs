/*
 * ============================================================
 * TOPIC     : Pattern Matching
 * SUBTOPIC  : List Patterns
 * FILE      : 01_ListPattern.cs
 * PURPOSE   : Demonstrates list patterns in C# 11+ for matching array and list elements
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._09_PatternMatching._04_ListTuplePatterns
{
    /// <summary>
    /// Demonstrates list pattern matching in C#
    /// </summary>
    public class ListPattern
    {
        /// <summary>
        /// Entry point for list pattern examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === List Pattern Matching Demo ===
            Console.WriteLine("=== List Pattern Matching Demo ===\n");

            // ── CONCEPT: Basic List Patterns ────────────────────────────────
            // List patterns match elements in arrays/lists

            // Example 1: Basic list pattern with underscore
            // Output: 1. Basic List Patterns:
            Console.WriteLine("1. Basic List Patterns:");
            
            // int[] = array of integers
            int[] empty = { };
            int[] single = { 1 };
            int[] two = { 1, 2 };
            int[] many = { 1, 2, 3, 4, 5 };
            
            // DescribeList returns description
            Console.WriteLine($"   Empty: {DescribeList(empty)}");
            Console.WriteLine($"   Single: {DescribeList(single)}");
            Console.WriteLine($"   Two: {DescribeList(two)}");
            Console.WriteLine($"   Many: {DescribeList(many)}");

            // ── CONCEPT: Var-Rest Pattern ────────────────────────────────────
            // .. (var-rests) matches any number of elements

            // Example 2: Var-rest pattern
            // Output: 2. Var-Rest Pattern:
            Console.WriteLine("\n2. Var-Rest Pattern:");
            
            // Test lists of various lengths
            var list1 = new[] { 1, 2, 3 };
            var list2 = new[] { 1, 2, 3, 4, 5, 6, 7 };
            var list3 = new[] { 99 };
            
            // MatchFirstLast returns description
            Console.WriteLine($"   [1,2,3]: {MatchFirstLast(list1)}");
            Console.WriteLine($"   [1-7]: {MatchFirstLast(list2)}");
            Console.WriteLine($"   [99]: {MatchFirstLast(list3)}");

            // ── CONCEPT: Range Patterns ─────────────────────────────────────
            // Match specific positions in list

            // Example 3: Range and slice patterns
            // Output: 3. Range and Slice Patterns:
            Console.WriteLine("\n3. Range and Slice Patterns:");
            
            // Test list with known elements
            var nums = new[] { 1, 2, 3, 4, 5 };
            
            // GetMiddleElements returns description
            // Output: [1,2,3,4,5] - Middle: [2,3,4]
            Console.WriteLine($"   {string.Join(",", nums)} - Middle: {GetMiddleElements(nums)}");

            // ── CONCEPT: Discards in List Patterns ──────────────────────────
            // _ matches single element, discards value

            // Example 4: Discards in list pattern
            // Output: 4. Discards in List Patterns:
            Console.WriteLine("\n4. Discards in List Patterns:");
            
            // Test different length lists
            var discard1 = new[] { 1, 2, 3 };
            var discard2 = new[] { 1, 2 };
            var discard3 = new[] { 1 };
            
            // FindCenter returns center element or message
            Console.WriteLine($"   [1,2,3]: {FindCenter(discard1)}");
            Console.WriteLine($"   [1,2]: {FindCenter(discard2)}");
            Console.WriteLine($"   [1]: {FindCenter(discard3)}");

            // ── CONCEPT: List Pattern with Conditions ────────────────────────
            // Combine with when clause for conditions

            // Example 5: List pattern with when
            // Output: 5. List Pattern with when:
            Console.WriteLine("\n5. List Pattern with when:");
            
            // Test lists with sum conditions
            var sum1 = new[] { 1, 2, 3 };      // sum = 6
            var sum2 = new[] { 1, 2, 3, 4 };  // sum = 10
            var sum3 = new[] { 5 };           // sum = 5
            
            // ClassifyBySum returns classification
            Console.WriteLine($"   [1,2,3]: {ClassifyBySum(sum1)}");
            Console.WriteLine($"   [1,2,3,4]: {ClassifyBySum(sum2)}");
            Console.WriteLine($"   [5]: {ClassifyBySum(sum3)}");

            // ── REAL-WORLD EXAMPLE: Command Parser ───────────────────────────
            // Output: --- Real-World: Command Parser ---
            Console.WriteLine("\n--- Real-World: Command Parser ---");
            
            // Parse different command formats
            var cmd1 = new[] { "CREATE", "user", "alice" };
            var cmd2 = new[] { "DELETE", "user", "123" };
            var cmd3 = new[] { "UPDATE", "config", "key", "value" };
            var cmd4 = new[] { "LIST" };
            var cmd5 = new[] { "UNKNOWN", "args" };
            
            // ParseCommand returns parsed result
            Console.WriteLine($"   CREATE: {ParseCommand(cmd1)}");
            Console.WriteLine($"   DELETE: {ParseCommand(cmd2)}");
            Console.WriteLine($"   UPDATE: {ParseCommand(cmd3)}");
            Console.WriteLine($"   LIST: {ParseCommand(cmd4)}");
            Console.WriteLine($"   UNKNOWN: {ParseCommand(cmd5)}");

            Console.WriteLine("\n=== List Pattern Complete ===");
        }

        /// <summary>
        /// Describes list using basic list pattern
        /// </summary>
        public static string DescribeList(int[] list)
        {
            // Match on list length using array pattern
            return list switch
            {
                // Empty list
                [] => "Empty list",
                
                // Single element
                [_] => "Single element",
                
                // Two elements
                [_, _] => "Two elements",
                
                // More than two
                [_, _, ..] => "Three or more elements"
            };
        }

        /// <summary>
        /// Matches first and last elements using var-rest pattern
        /// </summary>
        public static string MatchFirstLast(int[] list)
        {
            // .. matches any number of elements (slice)
            return list switch
            {
                // Single element (first = last)
                [var only] => $"Only element: {only}",
                
                // First is 1, last is 3, any middle
                [1, .., 3] => "Starts with 1, ends with 3",
                
                // First is 1, any middle elements
                [1, ..] => "Starts with 1",
                
                // Last is 99
                [.., 99] => "Ends with 99",
                
                // Has elements, any first and last
                [var first, .., var last] => $"First: {first}, Last: {last}",
                
                // Empty
                _ => "Empty list"
            };
        }

        /// <summary>
        /// Gets middle elements from list
        /// </summary>
        public static string GetMiddleElements(int[] list)
        {
            // Slice pattern for middle elements
            return list switch
            {
                // Exactly 5 elements: extract middle 3
                [var a, var b, var c, var d, var e] => 
                    $"{b}, {c}, {d}",
                    
                // 3 elements
                [var a, var b, var c] => $"{b}",
                
                // Any other
                _ => "Use different method"
            };
        }

        /// <summary>
        /// Finds center element using discard pattern
        /// </summary>
        public static string FindCenter(int[] list)
        {
            // _ discards matched value
            return list switch
            {
                // Odd length: center is middle
                [_, _, _] => "Center element",
                
                // Even length: no single center
                [_, _] => "No center (even length)",
                
                // Single element
                [var only] => $"Center: {only}",
                
                // Empty
                _ => "List too short or empty"
            };
        }

        /// <summary>
        /// Classifies list by sum using when clause
        /// </summary>
        public static string ClassifyBySum(int[] list)
        {
            // List pattern with when condition
            return list switch
            {
                // Sum less than 7
                [var a, var b, var c] when a + b + c < 7 => "Low sum",
                
                // Sum equals 6 exactly
                [var a, var b, var c] when a + b + c == 6 => "Sum is 6",
                
                // Sum greater than 7
                [var a, var b, var c, var d] when a + b + c + d > 7 => "High sum",
                
                // Default
                _ => "Other"
            };
        }

        /// <summary>
        /// Real-world: Parses command arguments
        /// </summary>
        public static string ParseCommand(string[] args)
        {
            // Command pattern matching
            return args switch
            {
                // CREATE command with 2 args
                ["CREATE", var resource, var name] => $"Create {resource} '{name}'",
                
                // DELETE command with 2 args
                ["DELETE", var resource, var id] => $"Delete {resource} ID:{id}",
                
                // UPDATE command with 3 args
                ["UPDATE", var resource, var key, var value] => $"Update {resource}.{key}={value}",
                
                // LIST with no args
                ["LIST"] => "List all resources",
                
                // Any unknown command
                [var cmd, ..] => $"Unknown command: {cmd}",
                
                // Empty
                _ => "No command"
            };
        }
    }
}
