/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Attributes Basics
 * FILE      : 01_AttributeBasics.cs
 * PURPOSE   : Introduction to custom attributes - declaring and applying attributes
 * ============================================================
 */
using System; // needed for Console
using System.Reflection; // needed for attribute inspection

namespace CSharp_MasterGuide._10_ReflectionMetadata._02_Attributes
{
    /// <summary>
    /// Demonstrates attribute basics in C#
    /// </summary>
    public class AttributeBasics
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Attribute Basics Demo ===\n");

            // Example 1: Reading applied attributes
            Console.WriteLine("1. Reading Attributes:");
            var type = typeof(AttributedClass);
            var attrs = type.GetCustomAttributes(typeof(DescriptionAttr), false);
            Console.WriteLine($"   Attributes: {attrs.Length}");

            // Example 2: Checking method attributes
            Console.WriteLine("\n2. Method Attributes:");
            var methods = typeof(AttributedClass).GetMethods();
            foreach (var m in methods)
            {
                var mattrs = m.GetCustomAttributes(typeof(ImportantAttribute), false);
                if (mattrs.Length > 0)
                    Console.WriteLine($"   {m.Name} is important");
            }

            // Example 3: Attribute with parameters
            Console.WriteLine("\n3. Attribute Parameters:");
            var props = typeof(AttributedClass).GetProperties();
            foreach (var p in props)
            {
                var cattr = p.GetCustomAttribute<CategoryAttribute>();
                if (cattr != null)
                    Console.WriteLine($"   {p.Name}: {cattr.Category}");
            }

            Console.WriteLine("\n=== Attribute Basics Complete ===");
        }
    }

    [DescriptionAttr("This class does something useful")]
    public class AttributedClass
    {
        [Important]
        public string Name { get; set; }

        [Category("Personal")]
        public int Age { get; set; }

        [Important]
        public void DoWork() { }
        
        public void OtherMethod() { }
    }

    // Basic attribute
    [AttributeUsage(AttributeTargets.Class)]
    public class DescriptionAttr : Attribute
    {
        public string Text { get; }
        public DescriptionAttr(string text) { Text = text; }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class ImportantAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class CategoryAttribute : Attribute
    {
        public string Category { get; }
        public CategoryAttribute(string category) { Category = category; }
    }
}
