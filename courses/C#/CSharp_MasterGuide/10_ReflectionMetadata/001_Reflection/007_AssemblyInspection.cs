/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Assembly Inspection
 * FILE      : 07_AssemblyInspection.cs
 * PURPOSE   : Demonstrates loading and inspecting assemblies at runtime
 * ============================================================
 */
using System; // needed for Console
using System.Reflection; // needed for assembly types

namespace CSharp_MasterGuide._10_ReflectionMetadata._01_Reflection
{
    /// <summary>
    /// Demonstrates assembly inspection using reflection
    /// </summary>
    public class AssemblyInspection
    {
        /// <summary>
        /// Entry point for assembly inspection
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Assembly Inspection Demo ===
            Console.WriteLine("=== Assembly Inspection Demo ===\n");

            // ── CONCEPT: Get Current Assembly ────────────────────────────────
            // Assembly.GetExecutingAssembly returns current assembly

            // Example 1: Get Current Assembly:
            // Output: 1. Get Current Assembly:
            Console.WriteLine("1. Get Current Assembly:");
            
            // Assembly = metadata about loaded assembly
            Assembly current = Assembly.GetExecutingAssembly();
            
            // Assembly.FullName = full qualified name
            // Output: Current: [AssemblyName], Version: [Version]
            Console.WriteLine($"   Current: {current.GetName().Name}");
            Console.WriteLine($"   Version: {current.GetName().Version}");

            // ── CONCEPT: Get Types from Assembly ───────────────────────────────
            // Assembly.GetTypes returns all types defined in assembly

            // Example 2: Get Types:
            // Output: 2. Get Types:
            Console.WriteLine("\n2. Get Types:");
            
            // GetTypes returns all public types in assembly
            Type[] allTypes = current.GetTypes();
            
            // Type = metadata about a type
            // Output: Total types: [Count]
            Console.WriteLine($"   Total types: {allTypes.Length}");

            // ── CONCEPT: Get Referenced Assemblies ────────────────────────────
            // GetReferencedAssemblies returns dependencies

            // Example 3: Referenced Assemblies:
            // Output: 3. Referenced Assemblies:
            Console.WriteLine("\n3. Referenced Assemblies:");
            
            // GetReferencedAssemblies = all assemblies this assembly references
            var refs = current.GetReferencedAssemblies();
            
            foreach (var name in refs.Take(5))
            {
                // AssemblyName.Name = simple name
                // Output:   - [AssemblyName]
                Console.WriteLine($"   - {name.Name}");
            }

            // ── REAL-WORLD EXAMPLE: Type Scanner ────────────────────────────
            // Output: --- Real-World: Type Scanner ---
            Console.WriteLine("\n--- Real-World: Type Scanner ---");
            
            // Find all classes implementing specific interface
            var scanner = new TypeScanner();
            
            // Output: Classes implementing IComparable:
            Console.WriteLine("   Classes implementing IComparable:");
            var comparables = scanner.FindImplementingTypes<IComparable>();
            
            foreach (var type in comparables.Take(5))
            {
                // Output:   - [TypeName]
                Console.WriteLine($"   - {type.Name}");
            }

            Console.WriteLine("\n=== Assembly Inspection Complete ===");
        }
    }

    /// <summary>
    /// Scans assemblies for types
    /// </summary>
    public class TypeScanner
    {
        public List<Type> FindImplementingTypes<T>()
        {
            // List<Type> = result collection
            var result = new List<Type>();
            
            // GetExecutingAssembly gets current assembly
            Assembly asm = Assembly.GetExecutingAssembly();
            
            // GetTypes = all types in assembly
            foreach (Type type in asm.GetTypes())
            {
                // IsClass = true for classes
                // IsAssignableFrom checks interface implementation
                if (type.IsClass && typeof(T).IsAssignableFrom(type))
                {
                    result.Add(type);
                }
            }
            
            return result;
        }
    }
}
