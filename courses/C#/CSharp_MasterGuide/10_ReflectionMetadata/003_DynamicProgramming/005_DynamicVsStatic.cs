/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Dynamic Programming - Dynamic vs Static
 * FILE      : 05_DynamicVsStatic.cs
 * PURPOSE   : Compares dynamic binding with static typing in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._10_ReflectionMetadata._03_DynamicProgramming
{
    /// <summary>
    /// Demonstrates differences between dynamic and static typing
    /// </summary>
    public class DynamicVsStatic
    {
        /// <summary>
        /// Entry point for dynamic vs static comparison
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Dynamic vs Static Typing ===
            Console.WriteLine("=== Dynamic vs Static Typing ===\n");

            // ── CONCEPT: Compile-Time vs Runtime ───────────────────────────────
            // Static = type checked at compile time
            // Dynamic = type checked at runtime

            // Example 1: Static typing
            // Output: 1. Static Typing:
            Console.WriteLine("1. Static Typing:");
            
            // string = compile-time type, compiler enforces type safety
            string staticValue = "Hello";
            
            // Length = property of string, checked at compile time
            // Output: Length: 5
            Console.WriteLine($"   Length: {staticValue.Length}");
            
            // ToUpper = method of string, compiler knows it exists
            // Output: Upper: HELLO
            Console.WriteLine($"   Upper: {staticValue.ToUpper()}");

            // Example 2: Dynamic typing
            // Output: 2. Dynamic Typing:
            Console.WriteLine("\n2. Dynamic Typing:");
            
            // dynamic = type resolved at runtime
            dynamic dynamicValue = "Hello";
            
            // Method resolution happens at runtime
            // Output: Length: 5
            Console.WriteLine($"   Length: {dynamicValue.Length}");
            
            // Can change type at runtime
            dynamicValue = 42;
            // No Int32 has Length property - runtime error if called
            // Output: Value: 42, Type: Int32
            Console.WriteLine($"   Value: {dynamicValue}, Type: {dynamicValue.GetType()}");

            // ── CONCEPT: Performance Considerations ────────────────────────────
            // Static: optimized at compile time
            // Dynamic: runtime overhead

            // Example 3: Performance comparison setup
            // Output: 3. Performance Comparison:
            Console.WriteLine("\n3. Performance Comparison:");
            
            // Static method call - compiler generates direct call
            int staticResult = StaticCalculator.Add(10, 20);
            // Output: Static result: 30
            Console.WriteLine($"   Static result: {staticResult}");
            
            // Dynamic method call - uses runtime binding
            dynamic calculator = new DynamicCalculator();
            int dynamicResult = calculator.Add(10, 20);
            // Output: Dynamic result: 30
            Console.WriteLine($"   Dynamic result: {dynamicResult}");

            // ── CONCEPT: Type Safety ─────────────────────────────────────────
            // Static: catches errors at compile time
            // Dynamic: errors at runtime

            // Example 4: Type safety demonstration
            // Output: 4. Type Safety:
            Console.WriteLine("\n4. Type Safety:");
            
            // Static - compiler catches type mismatch
            // This would NOT compile: int x = "hello";
            
            // Dynamic - allows any assignment
            dynamic anything = "hello";
            // Output: Dynamic can hold: hello
            Console.WriteLine($"   Dynamic can hold: {anything}");
            
            anything = 100;
            // Output: Now holds: 100
            Console.WriteLine($"   Now holds: {anything}");

            // ── CONCEPT: IntelliSense and Refactoring ────────────────────────
            // Static: full IDE support
            // Dynamic: limited IDE support

            // Example 5: IDE support differences
            // Output: 5. IDE Support:
            Console.WriteLine("\n5. IDE Support:");
            
            // Static - IntelliSense shows all available methods
            string test = "test";
            // compiler can suggest: ToLower, ToUpper, Trim, etc.
            // Output: Static: test (IntelliSense available)
            Console.WriteLine($"   Static: test (IntelliSense available)");
            
            // Dynamic - no IntelliSense for unknown types
            dynamic dynamicTest = GetDynamicObject();
            // No compile-time suggestions available
            // Output: Dynamic: test (No IntelliSense)
            Console.WriteLine($"   Dynamic: test (No IntelliSense)");

            // ── REAL-WORLD EXAMPLE: API Response Handling ─────────────────────
            // Output: --- Real-World: API Response Handling ---
            Console.WriteLine("\n--- Real-World: API Response Handling ---");
            
            // Static approach - requires predefined classes
            var staticApi = new StaticApiResponse();
            var staticData = staticApi.GetData();
            // Type known at compile time
            // Output: Static API - Name: John, Age: 30
            Console.WriteLine($"   Static API - Name: {staticData.Name}, Age: {staticData.Age}");
            
            // Dynamic approach - flexible, no predefined class
            var dynamicApi = new DynamicApiResponse();
            dynamic dynamicData = dynamicApi.GetData();
            // Works with any JSON structure
            // Output: Dynamic API - Name: John, Age: 30
            Console.WriteLine($"   Dynamic API - Name: {dynamicData.Name}, Age: {dynamicData.Age}");

            Console.WriteLine("\n=== Dynamic vs Static Complete ===");
        }

        /// <summary>
        /// Gets a dynamic object for demonstration
        /// </summary>
        /// <returns>Dynamic object</returns>
        static dynamic GetDynamicObject()
        {
            return "test";
        }
    }

    /// <summary>
    /// Static calculator with compile-time binding
    /// </summary>
    public static class StaticCalculator
    {
        /// <summary>
        /// Adds two integers
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <returns>Sum</returns>
        public static int Add(int a, int b)
        {
            return a + b;
        }
    }

    /// <summary>
    /// Dynamic calculator for runtime binding
    /// </summary>
    public class DynamicCalculator
    {
        /// <summary>
        /// Adds two numbers
        /// </summary>
        public dynamic Add(dynamic a, dynamic b)
        {
            return a + b;
        }
    }

    /// <summary>
    /// Static API response with predefined structure
    /// </summary>
    public class StaticApiResponse
    {
        /// <summary>
        /// Gets static typed response data
        /// </summary>
        /// <returns>Predefined data object</returns>
        public StaticPerson GetData()
        {
            return new StaticPerson { Name = "John", Age = 30 };
        }
    }

    /// <summary>
    /// Static person class
    /// </summary>
    public class StaticPerson
    {
        public string Name { get; set; } // property: person's name
        public int Age { get; set; } // property: person's age
    }

    /// <summary>
    /// Dynamic API response returning dynamic object
    /// </summary>
    public class DynamicApiResponse
    {
        /// <summary>
        /// Gets dynamic response data
        /// </summary>
        /// <returns>Dynamic object</returns>
        public dynamic GetData()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.Name = "John";
            data.Age = 30;
            return data;
        }
    }
}