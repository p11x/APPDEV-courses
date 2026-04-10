/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Dynamic Programming - Dynamic Keyword
 * FILE      : 01_DynamicKeyword.cs
 * PURPOSE   : Demonstrates the dynamic keyword for late-binding in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._10_ReflectionMetadata._03_DynamicProgramming
{
    /// <summary>
    /// Demonstrates the dynamic keyword for runtime binding
    /// </summary>
    public class DynamicKeyword
    {
        /// <summary>
        /// Entry point for dynamic keyword examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Dynamic Keyword Demo ===
            Console.WriteLine("=== Dynamic Keyword Demo ===\n");

            // ── CONCEPT: What is dynamic? ──────────────────────────────────
            // dynamic bypasses compile-time type checking

            // Example 1: Basic dynamic usage
            // Output: 1. Basic Dynamic:
            Console.WriteLine("1. Basic Dynamic:");
            
            // dynamic = compile-time dynamic, runtime type-checked
            dynamic value = "Hello";
            // value can be any type - no compile error
            // Output: value = Hello, Type: [type]
            Console.WriteLine($"   value = {value}, Type: {value.GetType()}");
            
            // Reassign to different type
            value = 42;
            // Output: value = 42, Type: [type]
            Console.WriteLine($"   value = {value}, Type: {value.GetType()}");

            // ── CONCEPT: Dynamic Method Calls ────────────────────────────────
            // Methods resolved at runtime

            // Example 2: Dynamic method calls
            // Output: 2. Dynamic Method Calls:
            Console.WriteLine("\n2. Dynamic Method Calls:");
            
            // dynamic obj = new object
            dynamic dynObj = new DynamicObject();
            
            // Method calls resolved at runtime - no compile-time check
            // This will work if method exists at runtime
            // string result = dynObj.SayHello();
            string result = dynObj.SayHello();
            
            // Output: Result: Hello from dynamic!
            Console.WriteLine($"   Result: {result}");

            // ── CONCEPT: Dynamic Property Access ────────────────────────────
            // Properties also resolved at runtime

            // Example 3: Dynamic property access
            // Output: 3. Dynamic Property Access:
            Console.WriteLine("\n3. Dynamic Property Access:");
            
            // dynamic expando = new ExpandoObject()
            dynamic expando = new SampleDynamic();
            
            // Assign properties dynamically
            expando.Name = "Alice";
            expando.Age = 30;
            
            // Read properties dynamically
            // Output: Name: Alice, Age: 30
            Console.WriteLine($"   Name: {expando.Name}, Age: {expando.Age}");

            // ── CONCEPT: Dynamic with Collections ────────────────────────────
            // Collections work with dynamic

            // Example 4: Dynamic collections
            // Output: 4. Dynamic Collections:
            Console.WriteLine("\n4. Dynamic Collections:");
            
            // dynamic list = new List of mixed types
            dynamic list = new System.Collections.ArrayList();
            
            // Add different types - allowed with dynamic
            list.Add("string");
            list.Add(42);
            list.Add(3.14);
            
            // Access by index - returns object
            // list[0] = first element
            // Output: First: [value], Count: [count]
            Console.WriteLine($"   First: {list[0]}, Count: {list.Count}");

            // ── REAL-WORLD EXAMPLE: Plugin Loader ───────────────────────────
            // Output: --- Real-World: Plugin Loader ---
            Console.WriteLine("\n--- Real-World: Plugin Loader ---");
            
            // Load plugin dynamically
            var pluginLoader = new PluginLoader();
            
            // LoadPlugin returns dynamic reference to plugin
            dynamic plugin = pluginLoader.LoadPlugin("MyPlugin");
            
            // Call plugin methods without compile-time dependency
            // Output: Plugin result: [result]
            Console.WriteLine($"   Plugin result: {plugin.Execute()}");

            Console.WriteLine("\n=== Dynamic Keyword Complete ===");
        }
    }

    /// <summary>
    /// Sample class with dynamic-friendly methods
    /// </summary>
    public class DynamicObject
    {
        /// <summary>
        /// Returns greeting message
        /// </summary>
        /// <returns>Greeting string</returns>
        public string SayHello()
        {
            return "Hello from dynamic!";
        }
        
        /// <summary>
        /// Adds two numbers
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <returns>Sum of numbers</returns>
        public int Add(int a, int b)
        {
            return a + b;
        }
    }

    /// <summary>
    /// Sample class with dynamic properties
    /// </summary>
    public class SampleDynamic
    {
        // Properties will be set dynamically
        public string Name { get; set; }
        public int Age { get; set; }
    }

    /// <summary>
    /// Plugin loader for real-world example
    /// </summary>
    public class PluginLoader
    {
        /// <summary>
        /// Loads plugin dynamically
        /// </summary>
        /// <param name="pluginName">Name of plugin to load</param>
        /// <returns>Dynamic reference to plugin</returns>
        public dynamic LoadPlugin(string pluginName)
        {
            // In real scenario, load from assembly
            // For demo, return simple object with Execute method
            return new SimplePlugin();
        }
    }

    /// <summary>
    /// Simple plugin implementation
    /// </summary>
    public class SimplePlugin
    {
        /// <summary>
        /// Executes plugin
        /// </summary>
        /// <returns>Execution result</returns>
        public string Execute()
        {
            return "Plugin executed successfully";
        }
    }
}
