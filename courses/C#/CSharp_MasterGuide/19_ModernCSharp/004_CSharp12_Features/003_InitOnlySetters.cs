/*
 * ============================================================
 * TOPIC     : Modern C#
 * SUBTOPIC  : Init Only Setters
 * FILE      : InitOnlySetters.cs
 * PURPOSE   : Using init setters in C# 9+
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._19_ModernCSharp._04_CSharp12_Features
{
    /// <summary>
    /// Init only setters demonstration
    /// </summary>
    public class InitOnlySettersDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Init Only Setters ===\n");

            // Output: --- Basic Init ---
            Console.WriteLine("--- Basic Init ---");

            var person = new Person
            {
                Name = "Alice",
                Age = 30
            };
            Console.WriteLine($"   Name: {person.Name}");
            // Output: Name: Alice

            // Output: --- Init After Creation ---
            Console.WriteLine("\n--- Init After Creation ---");

            person.Age = 31;
            Console.WriteLine($"   Updated age: {person.Age}");
            // Output: Updated age: 31

            // Output: --- With Expression ---
            Console.WriteLine("\n--- Expression-bodied Init ---");

            var person2 = new Person() { Name = "Bob" };
            Console.WriteLine($"   Person2: {person2.Name}");
            // Output: Person2: Bob

            Console.WriteLine("\n=== Init Only Complete ===");
        }
    }

    /// <summary>
    /// Person with init setters (C# 9+)
    /// </summary>
    public class Person
    {
        public string Name { get; init; } = "Unknown"; // property: name - init only
        public int Age { get; init; } = 0; // property: age - init only
    }
}