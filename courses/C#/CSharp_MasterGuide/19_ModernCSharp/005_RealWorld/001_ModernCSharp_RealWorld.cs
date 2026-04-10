/*
 * ============================================================
 * TOPIC     : Modern C#
 * SUBTOPIC  : Real-World Modern C#
 * FILE      : 04_ModernCSharp_RealWorld.cs
 * PURPOSE   : Real-world modern C# examples
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._19_ModernCSharp._04_RealWorld
{
    public class ModernCSharpRealWorldDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Modern C# Real-World ===\n");
            Console.WriteLine("1. With Expressions:");
            var person = new Person(1, "John", "john@email.com");
            var updated = person with { Name = "Jane" };
            Console.WriteLine($"   Original: {person.Name}, Updated: {updated.Name}");
            Console.WriteLine("\n=== Modern C# Real-World Complete ===");
        }
    }

    public record Person(int Id, string Name, string Email);
}