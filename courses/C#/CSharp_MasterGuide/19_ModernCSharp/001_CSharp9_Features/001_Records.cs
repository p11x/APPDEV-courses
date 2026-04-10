/*
 * ============================================================
 * TOPIC     : Modern C#
 * SUBTOPIC  : C# 9 Features - Records
 * FILE      : 01_Records.cs
 * PURPOSE   : Record types in C# 9
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._19_ModernCSharp._01_CSharp9_Features
{
    /// <summary>
    /// C# 9 Records
    /// </summary>
    public class CSharp9Records
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== C# 9 Records ===\n");

            // Create record
            var person = new Person("John", "Doe");
            Console.WriteLine($"   Created: {person.FirstName} {person.LastName}");
            
            // With expression (immutable)
            var person2 = person with { LastName = "Smith" };
            Console.WriteLine($"   Modified: {person2.LastName}");
            
            // Value equality
            var person3 = new Person("John", "Doe");
            Console.WriteLine($"   Equal: {person == person3}");

            Console.WriteLine("\n=== C# 9 Records Complete ===");
        }
    }

    /// <summary>
    /// Record type - immutable, value equality
    /// </summary>
    public record Person(string FirstName, string LastName);
}