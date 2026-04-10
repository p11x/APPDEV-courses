/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Reflection Real-World (Continued)
 * FILE      : 09_Reflection_RealWorld_Part2.cs
 * PURPOSE   : More real-world reflection patterns - caching, validation, code generation
 * ============================================================
 */
using System; // needed for Console
using System.Collections.Generic; // needed for collections
using System.Reflection; // needed for reflection
using System.Linq; // needed for LINQ

namespace CSharp_MasterGuide._10_ReflectionMetadata._01_Reflection
{
    /// <summary>
    /// More real-world reflection applications
    /// </summary>
    public class Reflection_RealWorld_Part2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Real-World Reflection Part 2 ===\n");

            // Example 1: Object Factory
            Console.WriteLine("1. Object Factory:");
            var factory = new ObjectFactory();
            var product = factory.Create("Product", "Laptop", 999.99m);
            var order = factory.Create("Order", "ORD-001", 150.00m);
            Console.WriteLine($"   Created: {product.Name}, {order.Id}");

            // Example 2: Clone via Reflection
            Console.WriteLine("\n2. Clone via Reflection:");
            var original = new Cloneable { Name = "Original", Value = 100 };
            var clone = Clone(original);
            clone.Name = "Clone";
            Console.WriteLine($"   Original: {original.Name}, Clone: {clone.Name}");

            // Example 3: Member Sorter
            Console.WriteLine("\n3. Member Sorter:");
            var members = GetMemberNames(typeof(Reflection_RealWorld_Part2));
            Console.WriteLine($"   Members: {string.Join(", ", members.Take(5))}");

            Console.WriteLine("\n=== Real-World Reflection Part 2 Complete ===");
        }

        public class ObjectFactory
        {
            public object Create(string typeName, params object[] args)
            {
                Type type = Type.GetType($"Reflection_RealWorld_Part2+{typeName}");
                return Activator.CreateInstance(type, args);
            }
        }

        public class Product { public string Name { get; set; } public decimal Price { get; set; } public Product(string n, decimal p) { Name = n; Price = p; } }
        public class Order { public string Id { get; set; } public decimal Total { get; set; } public Order(string id, decimal t) { Id = id; Total = t; } }

        public static T Clone<T>(T source) where T : new()
        {
            var clone = new T();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.CanWrite) prop.SetValue(clone, prop.GetValue(source));
            }
            return clone;
        }

        public class Cloneable { public string Name { get; set; } public int Value { get; set; } }

        public static List<string> GetMemberNames(Type type)
        {
            return type.GetMembers().Select(m => m.Name).ToList();
        }
    }
}
