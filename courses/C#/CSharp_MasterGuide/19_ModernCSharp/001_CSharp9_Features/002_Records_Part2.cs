/*
 * ============================================================
 * TOPIC     : Modern C#
 * SUBTOPIC  : C# 9 - Records Part 2
 * FILE      : 02_Records_Part2.cs
 * PURPOSE   : Advanced records features
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._19_ModernCSharp._01_CSharp9_Features
{
    /// <summary>
    /// Advanced records
    /// </summary>
    public class RecordsPart2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Records Part 2 ===\n");

            var person = new Person("John", "Doe");
            var updated = person with { LastName = "Smith" };
            Console.WriteLine($"   Original: {person.LastName}, Modified: {updated.LastName}");

            Console.WriteLine("\n=== Records Part 2 Complete ===");
        }
    }

    public record Person(string FirstName, string LastName);
}