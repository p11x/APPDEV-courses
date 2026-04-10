/*
 * ============================================================
 * TOPIC     : Modern C#
 * SUBTOPIC  : Record Structs
 * FILE      : RecordStructs.cs
 * PURPOSE   : Using record structs in C# 10+
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._19_ModernCSharp._04_CSharp12_Features
{
    /// <summary>
    /// Record struct demonstration
    /// </summary>
    public class RecordStructsDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Record Structs ===\n");

            // Output: --- Value Type Records ---
            Console.WriteLine("--- Value Type Records ---");

            var point = new Point2D(10, 20);
            Console.WriteLine($"   Point: {point.X}, {point.Y}");
            // Output: Point: 10, 20

            // Output: --- With Expression ---
            Console.WriteLine("\n--- With Expression ---");

            var moved = point with { X = 15 };
            Console.WriteLine($"   Moved: {moved.X}, {moved.Y}");
            // Output: Moved: 15, 20

            // Output: --- Equality ---
            Console.WriteLine("\n--- Equality ---");

            var p1 = new Point2D(10, 20);
            var p2 = new Point2D(10, 20);
            Console.WriteLine($"   Equal: {p1 == p2}");
            // Output: Equal: True

            // Output: --- ToString ---
            Console.WriteLine("\n--- ToString ---");

            Console.WriteLine($"   {point}");
            // Output: Point2D { X = 10, Y = 20 }

            Console.WriteLine("\n=== Record Structs Complete ===");
        }
    }

    /// <summary>
    /// Point record struct (C# 10+)
    /// </summary>
    public record struct Point2D(int X, int Y); // record struct: value type with built-in equality
}