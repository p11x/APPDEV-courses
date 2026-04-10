/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Dynamic Programming - ExpandoObject
 * FILE      : 03_ExpandoObject.cs
 * PURPOSE   : Demonstrates ExpandoObject - a dynamic object for building object models at runtime
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Dynamic; // needed for ExpandoObject

namespace CSharp_MasterGuide._10_ReflectionMetadata._03_DynamicProgramming
{
    /// <summary>
    /// Demonstrates ExpandoObject - dynamically expandable objects
    /// </summary>
    public class ExpandoObject
    {
        /// <summary>
        /// Entry point for ExpandoObject examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === ExpandoObject Demo ===
            Console.WriteLine("=== ExpandoObject Demo ===\n");

            // ── CONCEPT: Creating ExpandoObject ───────────────────────────────
            // ExpandoObject allows dynamic property addition

            // Example 1: Basic ExpandoObject
            // Output: 1. Basic ExpandoObject:
            Console.WriteLine("1. Basic ExpandoObject:");
            
            // ExpandoObject = dynamically expandable object
            // dynamic = bypasses compile-time checking
            dynamic person = new ExpandoObject();
            
            // Add properties dynamically - no compile-time definition
            person.Name = "Alice";
            person.Age = 30;
            person.Email = "alice@example.com";
            
            // Access properties dynamically
            // Output: Name: Alice, Age: 30, Email: [email]
            Console.WriteLine($"   Name: {person.Name}, Age: {person.Age}, Email: {person.Email}");

            // ── CONCEPT: ExpandoObject as Dictionary ─────────────────────────
            // ExpandoObject implements IDictionary<string, object>

            // Example 2: ExpandoObject as Dictionary
            // Output: 2. ExpandoObject as Dictionary:
            Console.WriteLine("\n2. ExpandoObject as Dictionary:");
            
            // Cast to IDictionary for dictionary operations
            dynamic employee = new ExpandoObject();
            employee.Title = "Engineer";
            employee.Department = "Engineering";
            
            // IDictionary<string, object> allows dictionary access
            var dict = (System.Collections.Generic.IDictionary<string, object>)employee;
            
            // Count = number of properties
            // Output: Properties count: [count]
            Console.WriteLine($"   Properties count: {dict.Count}");
            
            // ContainsKey checks if property exists
            // Output: Has Title: [True/False]
            Console.WriteLine($"   Has Title: {dict.ContainsKey("Title")}");
            
            // Remove property
            dict.Remove("Department");
            // Output: After remove: [count]
            Console.WriteLine($"   After remove: {dict.Count}");

            // ── CONCEPT: Adding Methods to ExpandoObject ────────────────────
            // ExpandoObject can hold delegate properties (methods)

            // Example 3: Adding Methods
            // Output: 3. Adding Methods to ExpandoObject:
            Console.WriteLine("\n3. Adding Methods to ExpandoObject:");
            
            // Create dynamic object with method
            dynamic calculator = new ExpandoObject();
            
            // Assign delegate to property - acts as method
            // Func<int, int, int> = function taking 2 ints, returning int
            calculator.Add = (Func<int, int, int>)((a, b) => a + b);
            calculator.Multiply = (Func<int, int, int>)((a, b) => a * b);
            
            // Call methods like properties
            // Output: Add(5, 3) = [result]
            Console.WriteLine($"   Add(5, 3) = {calculator.Add(5, 3)}");
            // Output: Multiply(4, 7) = [result]
            Console.WriteLine($"   Multiply(4, 7) = {calculator.Multiply(4, 7)}");

            // ── CONCEPT: ExpandoObject Events ─────────────────────────────────
            // ExpandoObject can have events

            // Example 4: ExpandoObject Events
            // Output: 4. ExpandoObject Events:
            Console.WriteLine("\n4. ExpandoObject Events:");
            
            // Create expando with event
            dynamic notifier = new ExpandoObject();
            
            // EventHandler delegate type for events
            // += adds event handler
            notifier.Notification += (sender, message) => 
                Console.WriteLine($"   Event received: {message}");
            
            // Get event and invoke
            var eventHandler = notifier.Notification as System.EventHandler<string>;
            eventHandler?.Invoke(notifier, "Hello Event!");

            // ── CONCEPT: Recursive Dynamic Objects ────────────────────────────
            // Nested ExpandoObject objects

            // Example 5: Nested Objects
            // Output: 5. Nested Objects:
            Console.WriteLine("\n5. Nested Objects:");
            
            // Create object with nested object
            dynamic company = new ExpandoObject();
            company.Name = "Tech Corp";
            
            // Create nested ExpandoObject
            dynamic address = new ExpandoObject();
            address.Street = "123 Main St";
            address.City = "Seattle";
            address.State = "WA";
            
            // Assign nested to property
            company.Address = address;
            
            // Access nested properties
            // company.Address.Street = nested property
            // Output: Company: Tech Corp, City: [city]
            Console.WriteLine($"   Company: {company.Name}, City: {company.Address.City}");

            // ── REAL-WORLD EXAMPLE: JSON to Object ───────────────────────────
            // Output: --- Real-World: JSON to Object ---
            Console.WriteLine("\n--- Real-World: JSON to Object ---");
            
            // Simulate JSON parsing to dynamic
            dynamic jsonResult = ParseJsonToExpando(@"{""user"":{""name"":""Bob"",""age"":25}}");
            
            // Access nested JSON properties
            // Output: User: [name], Age: [age]
            Console.WriteLine($"   User: {jsonResult.user.name}, Age: {jsonResult.user.age}");

            Console.WriteLine("\n=== ExpandoObject Complete ===");
        }

        /// <summary>
        /// Parses JSON string to dynamic ExpandoObject
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <returns>Dynamic object with JSON properties</returns>
        public static dynamic ParseJsonToExpando(string json)
        {
            // Simple JSON parser for demonstration
            // In production, use JSON library
            dynamic result = new ExpandoObject();
            
            // Parse simple nested JSON
            if (json.Contains("user"))
            {
                dynamic user = new ExpandoObject();
                user.name = "Bob";
                user.age = 25;
                result.user = user;
            }
            
            return result;
        }
    }
}
