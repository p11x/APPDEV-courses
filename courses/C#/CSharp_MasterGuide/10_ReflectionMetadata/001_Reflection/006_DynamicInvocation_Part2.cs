/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Dynamic Invocation (Continued)
 * FILE      : 06_DynamicInvocation_Part2.cs
 * PURPOSE   : Continues dynamic invocation with delegates, events, and complex scenarios
 * ============================================================
 */
using System; // needed for Console, types
using System.Reflection; // needed for reflection
using System.Linq; // needed for LINQ

namespace CSharp_MasterGuide._10_ReflectionMetadata._01_Reflection
{
    /// <summary>
    /// Continues dynamic invocation demonstrations
    /// </summary>
    public class DynamicInvocation_Part2
    {
        /// <summary>
        /// Entry point for advanced invocation
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Dynamic Invocation Part 2 ===
            Console.WriteLine("=== Dynamic Invocation Part 2 ===\n");

            // ── CONCEPT: Create Delegate from MethodInfo ─────────────────────
            // Delegate.CreateDelegate for type-safe invocation

            // Example 1: Create Delegate:
            // Output: 1. Create Delegate:
            Console.WriteLine("1. Create Delegate:");
            
            // GetMethod for Add
            MethodInfo addMethod = typeof(Math).GetMethod("Max", new[] { typeof(int), typeof(int) });
            
            // CreateDelegate creates delegate from MethodInfo
            // Func<int,int,int> = delegate taking two ints, returning int
            Func<int, int, int> addDelegate = (Func<int, int, int>)Delegate.CreateDelegate(
                typeof(Func<int, int, int>), addMethod);
            
            // Delegate.Invoke calls method
            int max = addDelegate(10, 20);
            
            // Output: Max(10, 20) = 20
            Console.WriteLine($"   Max(10, 20) = {max}");

            // ── CONCEPT: Invoke via Dynamic ──────────────────────────────────
            // Use dynamic keyword for late binding

            // Example 2: Dynamic Invocation:
            // Output: 2. Dynamic Invocation:
            Console.WriteLine("\n2. Dynamic Invocation:");
            
            // dynamic = bypass compile-time type checking
            dynamic calc = new Calculator2();
            
            // No reflection needed - runtime handles it
            // string result = calc.Subtract(50, 10);
            int result = calc.Subtract(50, 10);
            
            // Output: Subtract(50, 10) = 40
            Console.WriteLine($"   Subtract(50, 10) = {result}");

            // ── CONCEPT: Handle Method Overloads ────────────────────────────
            // GetMethod with parameter types

            // Example 3: Handle Overloads:
            // Output: 3. Handle Overloads:
            Console.WriteLine("\n3. Handle Overloads:");
            
            var overloaded = new OverloadedMethods();
            
            // GetMethod with exact parameter types
            // Type[] = array of parameter types
            MethodInfo methodInt = typeof(OverloadedMethods).GetMethod("Process", new[] { typeof(int) });
            MethodInfo methodStr = typeof(OverloadedMethods).GetMethod("Process", new[] { typeof(string) });
            
            // Invoke each overload
            string res1 = (string)methodInt.Invoke(overloaded, new object[] { 42 });
            string res2 = (string)methodStr.Invoke(overloaded, new object[] { "test" });
            
            // Output: Process(int): Processed int: 42
            Console.WriteLine($"   Process(int): {res1}");
            // Output: Process(string): Processed string: test
            Console.WriteLine($"   Process(string): {res2}");

            // ── CONCEPT: Invoke Private Members ──────────────────────────────
            // BindingFlags.NonPublic for private access

            // Example 4: Private Members:
            // Output: 4. Private Members:
            Console.WriteLine("\n4. Private Members:");
            
            var privateClass = new PrivateClass();
            
            // GetMethod with NonPublic flag
            MethodInfo privateMethod = typeof(PrivateClass).GetMethod("SecretMethod", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            // Invoke private method
            string secret = (string)privateMethod.Invoke(privateClass, null);
            
            // Output: SecretMethod returned: My secret
            Console.WriteLine($"   SecretMethod returned: {secret}");

            // ── REAL-WORLD EXAMPLE: Plugin Loader ───────────────────────────
            // Output: --- Real-World: Plugin Loader ---
            Console.WriteLine("\n--- Real-World: Plugin Loader ---");
            
            // Create and invoke plugins
            var loader = new PluginLoader();
            
            // Output: [PluginName]: [Execute result]
            Console.WriteLine($"   Plugin1: {loader.LoadAndExecute<PluginA>()}");
            Console.WriteLine($"   Plugin2: {loader.LoadAndExecute<PluginB>()}");

            Console.WriteLine("\n=== Dynamic Invocation Part 2 Complete ===");
        }
    }

    /// <summary>
    /// Calculator with reflection-friendly methods
    /// </summary>
    public class Calculator2
    {
        public int Add(int a, int b) => a + b;
        public int Subtract(int a, int b) => a - b;
    }

    /// <summary>
    /// Overloaded methods
    /// </summary>
    public class OverloadedMethods
    {
        public string Process(int value) => $"Processed int: {value}";
        public string Process(string value) => $"Processed string: {value}";
    }

    /// <summary>
    /// Class with private method
    /// </summary>
    public class PrivateClass
    {
        private string SecretMethod()
        {
            return "My secret";
        }
    }

    /// <summary>
    /// Plugin interface
    /// </summary>
    public interface IPlugin
    {
        string Execute();
    }

    /// <summary>
    /// Plugin A
    /// </summary>
    public class PluginA : IPlugin
    {
        public string Execute() => "Plugin A executed";
    }

    /// <summary>
    /// Plugin B
    /// </summary>
    public class PluginB : IPlugin
    {
        public string Execute() => "Plugin B executed";
    }

    /// <summary>
    /// Plugin loader
    /// </summary>
    public class PluginLoader
    {
        public string LoadAndExecute<T>() where T : IPlugin, new()
        {
            // new() = create instance using default constructor
            var plugin = new T();
            
            // Invoke interface method
            return plugin.Execute();
        }
    }
}
