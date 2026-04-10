/*
 * ============================================================
 * TOPIC     : Modern C#
 * SUBTOPIC  : Records
 * FILE      : 01_RecordsDemo.cs
 * PURPOSE   : Demonstrates C# records
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._19_ModernCSharp._01_Records
{
    public class RecordsDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Records Demo ===\n");
            Console.WriteLine("1. Record Declaration:");
            var user = new UserRecord(1, "John", "john@email.com");
            Console.WriteLine($"   Created: {user.Name}");
            Console.WriteLine("\n=== Records Complete ===");
        }
    }

    public record UserRecord(int Id, string Name, string Email);
}