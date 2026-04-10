/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Type Inspection
 * FILE      : 03_TypeInspection.cs
 * PURPOSE   : Deep dive into Type inspection - checking interfaces, base types, generics
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List<T>, Dictionary<K,V>
using System.Reflection; // needed for reflection

namespace CSharp_MasterGuide._10_ReflectionMetadata._01_Reflection
{
    /// <summary>
    /// Demonstrates deep type inspection using reflection
    /// </summary>
    public class TypeInspection
    {
        /// <summary>
        /// Entry point for type inspection
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Type Inspection Demo ===
            Console.WriteLine("=== Type Inspection Demo ===\n");

            // ── CONCEPT: Check Interfaces ───────────────────────────────────
            // GetInterfaces returns all implemented interfaces

            // Example 1: Check Interfaces:
            // Output: 1. Check Interfaces:
            Console.WriteLine("1. Check Interfaces:");
            
            // Type listType = List<string> type
            Type listType = typeof(List<string>);
            
            // GetInterfaces returns array of interface types
            Type[] interfaces = listType.GetInterfaces();
            
            // Output: List<string> implements:
            Console.WriteLine("   List<string> implements:");
            foreach (Type iface in interfaces)
            {
                // Output:   - [InterfaceName]
                Console.WriteLine($"   - {iface.Name}");
            }

            // Example 2: IsAssignableFrom
            // Output: 2. IsAssignableFrom:
            Console.WriteLine("\n2. IsAssignableFrom:");
            
            // object[] values = various types to check
            object[] values = { "string", 42, 3.14, true, new List<int>() };
            
            // foreach = iterate values
            foreach (object value in values)
            {
                // GetType() returns runtime type
                // Type.IsAssignableFrom checks compatibility
                bool assignable = typeof(IComparable).IsAssignableFrom(value.GetType());
                
                // Output: [Type] implements IComparable: [True/False]
                Console.WriteLine($"   {value.GetType().Name} implements IComparable: {assignable}");
            }

            // ── CONCEPT: Check Base Type ────────────────────────────────────
            // Type.BaseType returns parent class

            // Example 3: Base Type
            // Output: 3. Base Type:
            Console.WriteLine("\n3. Base Type:");
            
            // Type personType = Person type
            Type personType = typeof(Person2);
            
            // BaseType = parent class in hierarchy
            // Output: Person2 base type: [BaseTypeName]
            Console.WriteLine($"   Person2 base type: {personType.BaseType?.Name ?? "none"}");
            
            // Walk up hierarchy
            Type current = personType;
            // Output: Hierarchy:
            Console.WriteLine("   Hierarchy:");
            while (current != null)
            {
                // Output:   -> [TypeName]
                Console.WriteLine($"   -> {current.Name}");
                current = current.BaseType;
            }

            // ── CONCEPT: Generic Type Information ────────────────────────────
            // GetGenericTypeDefinition, GetGenericArguments

            // Example 4: Generic Types
            // Output: 4. Generic Types:
            Console.WriteLine("\n4. Generic Types:");
            
            // Type dictType = Dictionary<string, int> type
            Type dictType = typeof(Dictionary<string, int>);
            
            // IsGenericType = true for generic types like List<T>, Dictionary<K,V>
            // Output: Is Generic Type: True
            Console.WriteLine($"   Is Generic Type: {dictType.IsGenericType}");
            
            // GetGenericTypeDefinition = returns generic type definition (without type args)
            // Output: Generic Definition: Dictionary`2
            Console.WriteLine($"   Generic Definition: {dictType.GetGenericTypeDefinition()}");
            
            // GetGenericArguments = returns type arguments
            // Output: Type Arguments:
            Console.WriteLine("   Type Arguments:");
            foreach (Type arg in dictType.GetGenericArguments())
            {
                // Output:   - [TypeName]
                Console.WriteLine($"   - {arg.Name}");
            }

            // ── CONCEPT: Check Attributes ────────────────────────────────────
            // GetCustomAttributes returns applied attributes

            // Example 5: Check Attributes:
            // Output: 5. Check Attributes:
            Console.WriteLine("\n5. Check Attributes:");
            
            // Type with attribute = MyClass type
            Type myClassType = typeof(MyClass);
            
            // GetCustomAttributes returns all attributes of type
            // inherit = true to include inherited attributes
            object[] attrs = myClassType.GetCustomAttributes(typeof(DescriptionAttribute), false);
            
            // Output: Attributes on MyClass:
            Console.WriteLine("   Attributes on MyClass:");
            foreach (object attr in attrs)
            {
                // Cast to DescriptionAttribute to get property
                var desc = attr as DescriptionAttribute;
                if (desc != null)
                {
                    // Output:   - [Description]
                    Console.WriteLine($"   - {desc.Text}");
                }
            }

            // ── REAL-WORLD EXAMPLE: Plugin Discovery ───────────────────────
            // Output: --- Real-World: Plugin Discovery ---
            Console.WriteLine("\n--- Real-World: Plugin Discovery ---");
            
            // Discover all types in current assembly implementing IPlugin
            // DiscoverPlugins returns list of plugin types
            var plugins = DiscoverPlugins();
            
            // Output: Found [n] plugins:
            Console.WriteLine($"   Found {plugins.Count} plugins:");
            foreach (Type plugin in plugins)
            {
                // Output:   - [PluginName]
                Console.WriteLine($"   - {plugin.Name}");
            }

            Console.WriteLine("\n=== Type Inspection Complete ===");
        }

        /// <summary>
        /// Discovers plugin types in the current assembly
        /// </summary>
        public static List<Type> DiscoverPlugins()
        {
            // List<Type> = collection of discovered plugin types
            var plugins = new List<Type>();
            
            // GetExecutingAssembly = current assembly
            // GetTypes = all types defined in assembly
            Type[] allTypes = Assembly.GetExecutingAssembly().GetTypes();
            
            // foreach = iterate types
            foreach (Type type in allTypes)
            {
                // IsClass = true for classes (not interfaces/enums)
                // IsAbstract = false for instantiable classes
                // IsAssignableFrom checks if type implements interface
                if (type.IsClass && !type.IsAbstract && typeof(IPlugin).IsAssignableFrom(type))
                {
                    // Add to plugins list
                    plugins.Add(type);
                }
            }
            
            return plugins;
        }
    }

    /// <summary>
    /// Base class for hierarchy demo
    /// </summary>
    public class Person2
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// Custom attribute for demo
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DescriptionAttribute : Attribute
    {
        public string Text { get; }
        
        public DescriptionAttribute(string text)
        {
            Text = text;
        }
    }

    /// <summary>
    /// Class with custom attribute
    /// </summary>
    [DescriptionAttribute("This is a sample class for reflection")]
    public class MyClass
    {
        public int Value { get; set; }
    }

    /// <summary>
    /// Plugin interface for discovery demo
    /// </summary>
    public interface IPlugin
    {
        void Execute();
    }

    /// <summary>
    /// Sample plugin implementation
    /// </summary>
    public class SamplePlugin : IPlugin
    {
        public void Execute() { }
    }
}
