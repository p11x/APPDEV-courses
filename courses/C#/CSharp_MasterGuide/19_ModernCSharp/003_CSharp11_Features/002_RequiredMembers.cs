/*
 * ============================================================
 * TOPIC     : Modern C#
 * SUBTOPIC  : Required Members
 * FILE      : RequiredMembers.cs
 * PURPOSE   : Using required members in C# 11
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._19_ModernCSharp._03_CSharp11_Features
{
    /// <summary>
    /// Required members demonstration
    /// </summary>
    public class RequiredMembersDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Required Members ===\n");

            // Output: --- Required Property ---
            Console.WriteLine("--- Required Property ---");

            var user = new User2 { Name = "Alice" };
            Console.WriteLine($"   User: {user.Name}");
            // Output: User: Alice

            // Output: --- Compilation Error If Missing ---
            Console.WriteLine("\n--- Must Provide ---");

            Console.WriteLine("   Name is required at compile time");
            // Output: Name is required at compile time

            Console.WriteLine("\n=== Required Complete ===");
        }
    }

    /// <summary>
    /// User with required member (C# 11)
    /// </summary>
    public class User2
    {
        public required string Name { get; set; } = "Unknown"; // required: must be initialized
    }
}