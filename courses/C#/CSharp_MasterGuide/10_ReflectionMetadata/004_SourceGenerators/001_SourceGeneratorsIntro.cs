/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Source Generators - Introduction
 * FILE      : 01_SourceGeneratorsIntro.cs
 * PURPOSE   : Introduces Source Generators in C# and their use cases
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._10_ReflectionMetadata._04_SourceGenerators
{
    /// <summary>
    /// Introduction to Source Generators concept
    /// </summary>
    public class SourceGeneratorsIntro
    {
        /// <summary>
        /// Entry point for Source Generator introduction
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Source Generators Introduction ===
            Console.WriteLine("=== Source Generators Introduction ===\n");

            // ── CONCEPT: What are Source Generators? ──────────────────────────
            // Source Generators add code during compilation

            // Example 1: Understanding Source Generators
            // Output: 1. What are Source Generators?
            Console.WriteLine("1. What are Source Generators?");
            
            // Source Generators = compile-time code generation
            // They analyze code and emit additional source files
            // Output: Source Generators add code at compile time
            Console.WriteLine("   Source Generators add code at compile time");
            
            // Unlike reflection (runtime), Source Generators work at build
            // Output: Unlike reflection, they run during compilation
            Console.WriteLine("   Unlike reflection, they run during compilation");

            // ── CONCEPT: Benefits of Source Generators ────────────────────────
            // Performance + type safety + reduced boilerplate

            // Example 2: Benefits
            // Output: 2. Benefits:
            Console.WriteLine("\n2. Benefits:");
            
            // No runtime overhead - code is generated at compile time
            // Output: - No runtime reflection overhead
            Console.WriteLine("   - No runtime reflection overhead");
            
            // Type safety - generated code is fully typed
            // Output: - Full compile-time type safety
            Console.WriteLine("   - Full compile-time type safety");
            
            // Reduced boilerplate - less manual code to write
            // Output: - Less boilerplate code
            Console.WriteLine("   - Less boilerplate code");
            
            // IntelliSense support - IDE knows about generated code
            // Output: - Full IntelliSense support
            Console.WriteLine("   - Full IntelliSense support");

            // ── CONCEPT: Common Use Cases ───────────────────────────────────
            // Serialization, dependency injection, code generation

            // Example 3: Common Use Cases
            // Output: 3. Common Use Cases:
            Console.WriteLine("\n3. Common Use Cases:");
            
            // JSON serialization - auto-generate serialization code
            // Output: - JSON/XML serialization (System.Text.Json, Newtonsoft)
            Console.WriteLine("   - JSON/XML serialization");
            
            // Dependency Injection - generate service registrations
            // Output: - Dependency Injection container registration
            Console.WriteLine("   - Dependency Injection");
            
            // Record source generators - enhance record types
            // Output: - Record equality and cloning
            Console.WriteLine("   - Record equality and cloning");
            
            // API clients - generate from OpenAPI specs
            // Output: - API client generation from specs
            Console.WriteLine("   - API client generation");

            // ── CONCEPT: How Source Generators Work ──────────────────────────
            // Compilation pipeline integration

            // Example 4: How They Work
            // Output: 4. How Source Generators Work:
            Console.WriteLine("\n4. How Source Generators Work:");
            
            // Step 1: Analyzer reads source code
            // Output: Step 1: Analyzer reads your source code
            Console.WriteLine("   Step 1: Analyzer reads source code");
            
            // Step 2: Generator produces additional code
            // Output: Step 2: Generator produces additional code
            Console.WriteLine("   Step 2: Generator produces additional code");
            
            // Step 3: Generated code is compiled together
            // Output: Step 3: Generated code is compiled together
            Console.WriteLine("   Step 3: Generated code is compiled together");
            
            // Step 4: Final assembly contains all code
            // Output: Step 4: Final assembly contains all code
            Console.WriteLine("   Step 4: Final assembly contains all code");

            // ── REAL-WORLD EXAMPLE: Generated Code Demo ─────────────────────
            // Output: --- Real-World: Generated Code Demo ---
            Console.WriteLine("\n--- Real-World: Generated Code Demo ---");
            
            // Demonstrate what generated code looks like
            // In real scenario, this would be auto-generated
            var generated = new GeneratedPerson
            {
                FirstName = "John", // property: person's first name
                LastName = "Doe" // property: person's last name
            };
            
            // Generated Equals method
            // Output: Person: John Doe
            Console.WriteLine($"   Person: {generated.FirstName} {generated.LastName}");
            
            // Generated ToString override
            // Output: ToString: Person(FirstName=John, LastName=Doe)
            Console.WriteLine($"   ToString: {generated.ToString()}");
            
            // Generated equality
            var same = new GeneratedPerson { FirstName = "John", LastName = "Doe" };
            // Output: Equal: True
            Console.WriteLine($"   Equal: {generated.Equals(same)}");

            Console.WriteLine("\n=== Source Generators Intro Complete ===");
        }
    }

    /// <summary>
    /// Simulates a class that would be generated by a Source Generator
    /// Shows what automatic code generation produces
    /// </summary>
    public class GeneratedPerson
    {
        // Properties that a generator would create
        public string FirstName { get; set; } // property: person's first name
        public string LastName { get; set; } // property: person's last name

        // Generated constructor
        public GeneratedPerson(string firstName, string lastName)
        {
            FirstName = firstName; // assign parameter to field
            LastName = lastName; // assign parameter to field
        }

        // Parameterless constructor (generated)
        public GeneratedPerson() { } // default constructor

        // Generated Equals method
        public override bool Equals(object obj)
        {
            if (obj is GeneratedPerson other) // pattern match to type
            {
                return FirstName == other.FirstName && LastName == other.LastName;
            }
            return false; // not equal if wrong type
        }

        // Generated GetHashCode
        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName); // hash based on properties
        }

        // Generated ToString
        public override string ToString()
        {
            return $"Person(FirstName={FirstName}, LastName={LastName})";
        }

        // Generated == operator
        public static bool operator ==(GeneratedPerson left, GeneratedPerson right)
        {
            if (left is null) return right is null; // null check
            return left.Equals(right); // use Equals method
        }

        // Generated != operator
        public static bool operator !=(GeneratedPerson left, GeneratedPerson right)
        {
            return !(left == right); // negate == operator
        }
    }
}