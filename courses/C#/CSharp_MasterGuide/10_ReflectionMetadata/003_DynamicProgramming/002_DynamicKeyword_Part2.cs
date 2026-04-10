/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Dynamic Programming - Continued
 * FILE      : 02_DynamicKeyword_Part2.cs
 * PURPOSE   : Continues dynamic keyword with COM interop, dynamic expressions, and best practices
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._10_ReflectionMetadata._03_DynamicProgramming
{
    /// <summary>
    /// Continues dynamic keyword demonstrations
    /// </summary>
    public class DynamicKeyword_Part2
    {
        /// <summary>
        /// Entry point for dynamic keyword part 2
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Dynamic Keyword Part 2 ===
            Console.WriteLine("=== Dynamic Keyword Part 2 ===\n");

            // ── CONCEPT: Dynamic in Expression Trees ──────────────────────────
            // Output: 1. Dynamic with LINQ:
            Console.WriteLine("1. Dynamic with LINQ:");
            
            // Create list with dynamic objects
            var list = new System.Collections.ArrayList();
            list.Add(new { Name = "Alice", Score = 90 });
            list.Add(new { Name = "Bob", Score = 75 });
            list.Add(new { Name = "Charlie", Score = 85 });
            
            // Cast to IEnumerable for LINQ
            // var = implicitly typed local variable
            var query = list.Cast<dynamic>().Where(x => x.Score > 80);
            
            // foreach = iterate query results
            foreach (var item in query)
            {
                // item.Name, item.Score = access dynamic properties
                // Output: [name]: [score]
                Console.WriteLine($"   {item.Name}: {item.Score}");
            }

            // ── CONCEPT: Dynamic Base Type ──────────────────────────────────
            // Output: 2. Dynamic Base Type:
            Console.WriteLine("\n2. Dynamic Base Type:");
            
            // DynamicBase class demonstrates dynamic base reference
            DynamicBase derived = new DynamicDerived();
            
            // Cast to dynamic
            dynamic dyn = derived;
            
            // Call method - resolved at runtime
            // Output: DerivedMethod called
            Console.WriteLine($"   {dyn.GetMessage()}");

            // ── CONCEPT: Duck Typing with Dynamic ────────────────────────────
            // Output: 3. Duck Typing:
            Console.WriteLine("\n3. Duck Typing:");
            
            // Both classes have Write method - can use either
            var writer1 = new ConsoleWriter();
            var writer2 = new FileWriter();
            
            // ProcessWriter accepts dynamic - duck typing
            ProcessWriter(writer1);
            ProcessWriter(writer2);

            // ── CONCEPT: Performance Considerations ────────────────────────────
            // Output: 4. Performance Considerations:
            Console.WriteLine("\n4. Performance Considerations:");
            
            // Measure dynamic vs static performance
            var timer = new System.Diagnostics.Stopwatch();
            
            // static call - baseline
            timer.Start();
            for (int i = 0; i < 1000; i++)
            {
                int result = StaticAdd(1, 2);
            }
            timer.Stop();
            // Output: Static: [elapsed]ms
            Console.WriteLine($"   Static: {timer.ElapsedMilliseconds}ms");
            
            // dynamic call
            timer.Restart();
            dynamic dynAdd = new MathOperations();
            for (int i = 0; i < 1000; i++)
            {
                int result = dynAdd.Add(1, 2);
            }
            timer.Stop();
            // Output: Dynamic: [elapsed]ms
            Console.WriteLine($"   Dynamic: {timer.ElapsedMilliseconds}ms");

            // ── REAL-WORLD EXAMPLE: Configuration Reader ─────────────────────
            // Output: --- Real-World: Configuration Reader ---
            Console.WriteLine("\n--- Real-World: Configuration Reader ---");
            
            // Read config with dynamic
            var config = ReadConfig("app.config");
            
            // Access config values dynamically
            // Output: Database: [connection string]
            Console.WriteLine($"   Database: {config.Database}");
            // Output: Cache: [enabled], TTL: [seconds]
            Console.WriteLine($"   Cache: {config.CacheEnabled}, TTL: {config.CacheTTL}");

            Console.WriteLine("\n=== Dynamic Keyword Part 2 Complete ===");
        }

        /// <summary>
        /// Static method for comparison
        /// </summary>
        static int StaticAdd(int a, int b) => a + b;

        /// <summary>
        /// Process any object with Write method (duck typing)
        /// </summary>
        static void ProcessWriter(dynamic writer)
        {
            // writer.Write called at runtime
            writer.Write("Hello, World!");
        }
    }

    /// <summary>
    /// Base class with virtual method
    /// </summary>
    public class DynamicBase
    {
        public virtual string GetMessage()
        {
            return "Base method";
        }
    }

    /// <summary>
    /// Derived class overrides method
    /// </summary>
    public class DynamicDerived : DynamicBase
    {
        public override string GetMessage()
        {
            return "Derived method";
        }
    }

    /// <summary>
    /// Console writer implementation
    /// </summary>
    public class ConsoleWriter
    {
        public void Write(string message)
        {
            Console.WriteLine($"   Console: {message}");
        }
    }

    /// <summary>
    /// File writer implementation
    /// </summary>
    public class FileWriter
    {
        public void Write(string message)
        {
            // In real scenario, write to file
            Console.WriteLine($"   File: {message}");
        }
    }

    /// <summary>
    /// Math operations class
    /// </summary>
    public class MathOperations
    {
        public int Add(int a, int b) => a + b;
        public int Subtract(int a, int b) => a - b;
    }

    /// <summary>
    /// Configuration reader - real-world example
    /// </summary>
    public class ConfigReader
    {
        /// <summary>
        /// Reads configuration file and returns dynamic object
        /// </summary>
        /// <param name="filePath">Path to config file</param>
        /// <returns>Dynamic object with config values</returns>
        public static dynamic ReadConfig(string filePath)
        {
            // In real scenario, parse config file
            // Create dynamic object using ExpandoObject
            dynamic config = new System.Dynamic.ExpandoObject();
            
            // Set properties dynamically
            config.Database = "Server=localhost;Database=MyApp";
            config.CacheEnabled = true;
            config.CacheTTL = 300;
            config.MaxConnections = 100;
            
            return config;
        }
    }
}
