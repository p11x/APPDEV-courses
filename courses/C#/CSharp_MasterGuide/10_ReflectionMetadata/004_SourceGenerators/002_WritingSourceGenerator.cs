/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Source Generators - Writing Generators
 * FILE      : 02_WritingSourceGenerator.cs
 * PURPOSE   : Demonstrates how to write a basic Source Generator
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._10_ReflectionMetadata._04_SourceGenerators
{
    /// <summary>
    /// Demonstrates the structure of a Source Generator
    /// </summary>
    public class WritingSourceGenerator
    {
        /// <summary>
        /// Entry point for writing Source Generator examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Writing a Source Generator ===
            Console.WriteLine("=== Writing a Source Generator ===\n");

            // ── CONCEPT: Generator Structure ───────────────────────────────────
            // Source Generators implement ISourceGenerator interface

            // Example 1: Generator Components
            // Output: 1. Generator Components:
            Console.WriteLine("1. Generator Components:");
            
            // A Source Generator needs:
            // 1. Reference to Compilation (read existing code)
            // Output: - Compilation reference (to read existing code)
            Console.WriteLine("   - Compilation reference");
            
            // 2. Generator attribute (registers the generator)
            // Output: - [Generator] attribute
            Console.WriteLine("   - [Generator] attribute");
            
            // 3. Execute method (produces new code)
            // Output: - Execute method (produces code)
            Console.WriteLine("   - Execute method");

            // ── CONCEPT: What Generators Can Analyze ─────────────────────────
            // Syntax trees, attributes, types

            // Example 2: Analysis Capabilities
            // Output: 2. Analysis Capabilities:
            Console.WriteLine("\n2. Analysis Capabilities:");
            
            // Can inspect syntax trees - parse code structure
            // Output: - Inspect syntax trees (parse code structure)
            Console.WriteLine("   - Inspect syntax trees");
            
            // Can find classes with specific attributes
            // Output: - Find classes with specific attributes
            Console.WriteLine("   - Find classes with attributes");
            
            // Can analyze type declarations and members
            // Output: - Analyze type declarations and members
            Console.WriteLine("   - Analyze type declarations");
            
            // Can read metadata from existing code
            // Output: - Read metadata from code
            Console.WriteLine("   - Read metadata from code");

            // ── CONCEPT: Code Emission ───────────────────────────────────────
            // Adding new source files to compilation

            // Example 3: Code Emission
            // Output: 3. Code Emission:
            Console.WriteLine("\n3. Code Emission:");
            
            // context.AddSource() adds code to compilation
            // Output: - AddSource(fileName, code)
            Console.WriteLine("   - AddSource(fileName, code)");
            
            // Generated code is compiled together with original
            // Output: - Generated code compiles with your code
            Console.WriteLine("   - Generated code compiles with your code");
            
            // Code can be in any .NET language (C#, VB, F#)
            // Output: - Can generate C#, VB, or F# code
            Console.WriteLine("   - Can generate C#, VB, or F# code");

            // ── CONCEPT: Example Generator Pattern ───────────────────────────
            // Pattern: Find attribute → generate code

            // Example 4: Generator Pattern
            // Output: 4. Generator Pattern:
            Console.WriteLine("\n4. Generator Pattern:");
            
            // Step 1: Define attribute class for users to apply
            // Output: Step 1: Define [YourAttribute]
            Console.WriteLine("   Step 1: Define [YourAttribute]");
            
            // Step 2: Find all uses of the attribute
            // Output: Step 2: Find all [YourAttribute] usages
            Console.WriteLine("   Step 2: Find [YourAttribute] usages");
            
            // Step 3: Extract information from annotated code
            // Output: Step 3: Extract info from annotated code
            Console.WriteLine("   Step 3: Extract info from code");
            
            // Step 4: Generate new code based on extracted info
            // Output: Step 4: Generate new code
            Console.WriteLine("   Step 4: Generate new code");

            // ── REAL-WORLD EXAMPLE: Auto-Wiring Example ──────────────────────
            // Output: --- Real-World: Auto-Wiring Example ---
            Console.WriteLine("\n--- Real-World: Auto-Wiring Example ---");
            
            // Demonstrate what auto-wiring generator produces
            // Simulates generated service registration
            
            // Example: User marks class with [Service] attribute
            // Output: User marks: [Service] class MyService
            Console.WriteLine("   User marks: [Service] class MyService");
            
            // Generator produces:
            // Output: Generator produces: serviceCollection.Add<MyService>()
            Console.WriteLine("   Generator produces: serviceCollection.Add<MyService>()");
            
            // The actual generated code
            var registration = new ServiceRegistration();
            // Register services (simulated generated code)
            registration.RegisterServices();
            // Output: Services registered successfully
            Console.WriteLine("   Services registered successfully");

            Console.WriteLine("\n=== Writing Source Generator Complete ===");
        }
    }

    /// <summary>
    /// Demonstrates service registration that would be generated
    /// </summary>
    public class ServiceRegistration
    {
        /// <summary>
        /// Simulates generated service registration
        /// </summary>
        public void RegisterServices()
        {
            // In real scenario, this would be:
            // services.AddTransient<IMyService, MyService>();
            // services.AddSingleton<IMyRepository, MyRepository>();
            Console.WriteLine("   Registered: IMyService -> MyService");
            Console.WriteLine("   Registered: IMyRepository -> MyRepository");
        }
    }

    /// <summary>
    /// Example attribute that would trigger code generation
    /// In real scenario, this would be used with a Source Generator
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ServiceAttribute : Attribute
    {
        // Attribute to mark classes for auto-registration
        // Usage: [Service] class MyService { }
    }

    /// <summary>
    /// Example attribute for interface implementation
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class ImplementationForAttribute : Attribute
    {
        /// <summary>
        /// Specifies the interface to implement
        /// </summary>
        public Type InterfaceType { get; } // property: the interface type
        
        /// <summary>
        /// Constructor taking interface type
        /// </summary>
        public ImplementationForAttribute(Type interfaceType)
        {
            InterfaceType = interfaceType; // store the interface type
        }
    }
}