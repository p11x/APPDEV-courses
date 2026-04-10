/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Dynamic Programming - Dynamic Collections
 * FILE      : 04_DynamicCollections.cs
 * PURPOSE   : Demonstrates working with dynamic collections in C#
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections; // needed for ArrayList, Queue, Stack

namespace CSharp_MasterGuide._10_ReflectionMetadata._03_DynamicProgramming
{
    /// <summary>
    /// Demonstrates dynamic collections and mixed-type handling
    /// </summary>
    public class DynamicCollections
    {
        /// <summary>
        /// Entry point for dynamic collections examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Dynamic Collections Demo ===
            Console.WriteLine("=== Dynamic Collections Demo ===\n");

            // ── CONCEPT: ArrayList with Dynamic ────────────────────────────────
            // ArrayList stores object references, works well with dynamic

            // Example 1: ArrayList with mixed types
            // Output: 1. ArrayList with Mixed Types:
            Console.WriteLine("1. ArrayList with Mixed Types:");
            
            // ArrayList = non-generic collection, accepts any object
            dynamic mixedList = new ArrayList();
            
            // Add = inserts element at end
            mixedList.Add("Hello"); // string added
            mixedList.Add(100); // int added (boxed to object)
            mixedList.Add(3.14); // double added
            mixedList.Add(true); // bool added
            
            // Count = number of elements in collection
            // Output: Count: 4
            Console.WriteLine($"   Count: {mixedList.Count}");
            
            // Access by index - returns object, but dynamic handles it
            // [0] = first element, cast to string for display
            // Output: First: Hello, Last: True
            Console.WriteLine($"   First: {mixedList[0]}, Last: {mixedList[mixedList.Count - 1]}");

            // ── CONCEPT: Queue and Stack with Dynamic ────────────────────────
            // Generic collections also work with dynamic

            // Example 2: Dynamic Queue
            // Output: 2. Dynamic Queue:
            Console.WriteLine("\n2. Dynamic Queue:");
            
            // Queue<T> = FIFO (First-In-First-Out) collection
            dynamic queue = new Queue();
            
            // Enqueue = adds element to end of queue
            queue.Enqueue("First");
            queue.Enqueue("Second");
            queue.Enqueue("Third");
            
            // Count = number of items in queue
            // Output: Queue count: 3
            Console.WriteLine($"   Queue count: {queue.Count}");
            
            // Dequeue = removes and returns first item
            // Output: Dequeued: First
            Console.WriteLine($"   Dequeued: {queue.Dequeue()}");
            // Output: Remaining: 2
            Console.WriteLine($"   Remaining: {queue.Count}");

            // Example 3: Dynamic Stack
            // Output: 3. Dynamic Stack:
            Console.WriteLine("\n3. Dynamic Stack:");
            
            // Stack<T> = LIFO (Last-In-First-Out) collection
            dynamic stack = new Stack();
            
            // Push = adds element to top of stack
            stack.Push("Bottom");
            stack.Push("Middle");
            stack.Push("Top");
            
            // Pop = removes and returns top element
            // Output: Popped: Top
            Console.WriteLine($"   Popped: {stack.Pop()}");
            // Output: Next: Middle
            Console.WriteLine($"   Next: {stack.Peek()}");

            // ── CONCEPT: Hashtable with Dynamic ──────────────────────────────
            // Dictionary-like structure with dynamic access

            // Example 4: Dynamic Hashtable
            // Output: 4. Dynamic Hashtable:
            Console.WriteLine("\n4. Dynamic Hashtable:");
            
            // Hashtable = key-value pairs, keys are hashed
            dynamic dict = new Hashtable();
            
            // Add(key, value) = inserts key-value pair
            dict["name"] = "Alice";
            dict["age"] = 30;
            dict["city"] = "New York";
            
            // Access by key - returns object
            // Output: Name: Alice, Age: 30
            Console.WriteLine($"   Name: {dict["name"]}, Age: {dict["age"]}");
            
            // ContainsKey = checks if key exists
            // Output: Has city key: True
            Console.WriteLine($"   Has city key: {dict.ContainsKey("city")}");

            // ── CONCEPT: Dynamic Type Conversion ──────────────────────────────
            // Automatic type handling with dynamic

            // Example 5: Type conversion with dynamic
            // Output: 5. Type Conversion:
            Console.WriteLine("\n5. Type Conversion:");
            
            // dynamic preserves actual runtime type
            dynamic num = 42;
            // GetType returns runtime Type object
            // Output: Type: System.Int32
            Console.WriteLine($"   Type: {num.GetType()}");
            
            // Can perform type-specific operations
            // + operator works at runtime for int
            // Output: 42 + 8 = 50
            Console.WriteLine($"   {num} + 8 = {num + 8}");
            
            // Change type at runtime
            num = "forty-two";
            // Output: Type: System.String
            Console.WriteLine($"   Type: {num.GetType()}");

            // ── REAL-WORLD EXAMPLE: Configuration Loader ─────────────────────
            // Output: --- Real-World: Configuration Loader ---
            Console.WriteLine("\n--- Real-World: Configuration Loader ---");
            
            // Simulate loading mixed-type configuration
            var configLoader = new ConfigLoader();
            
            // LoadConfig returns dynamic with mixed types
            dynamic config = configLoader.LoadConfig();
            
            // Access different typed values without casting
            // Output: AppName: MyApp, Version: 1.0, Debug: True
            Console.WriteLine($"   AppName: {config.AppName}, Version: {config.Version}, Debug: {config.Debug}");
            
            // Port is int at runtime
            // Output: Port: 8080
            Console.WriteLine($"   Port: {config.Port}");

            Console.WriteLine("\n=== Dynamic Collections Complete ===");
        }
    }

    /// <summary>
    /// Simulates loading configuration with mixed types
    /// </summary>
    public class ConfigLoader
    {
        /// <summary>
        /// Loads configuration as dynamic object
        /// </summary>
        /// <returns>Dynamic configuration object</returns>
        public dynamic LoadConfig()
        {
            // Return anonymous type as dynamic (simulated)
            var config = new System.Dynamic.ExpandoObject();
            
            // ExpandoObject allows dynamic property addition
            config.AppName = "MyApp";
            config.Version = "1.0";
            config.Debug = true;
            config.Port = 8080;
            
            return config; // returns as dynamic
        }
    }
}