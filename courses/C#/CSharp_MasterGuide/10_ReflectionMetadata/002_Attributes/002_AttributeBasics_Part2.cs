/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Attributes - Continued
 * FILE      : 02_AttributeBasics_Part2.cs
 * PURPOSE   : More attribute patterns - flags, multiple attributes, inheritance
 * ============================================================
 */
using System; // needed for Console

namespace CSharp_MasterGuide._10_ReflectionMetadata._02_Attributes
{
    public class AttributeBasics_Part2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Attribute Basics Part 2 ===\n");

            // Multiple attributes
            Console.WriteLine("1. Multiple Attributes:");
            var t = typeof(MultiAttrClass);
            var attrs = t.GetCustomAttributes(typeof(AttributeBase), false);
            Console.WriteLine($"   Count: {attrs.Length}");

            // Flags enum attribute
            Console.WriteLine("\n2. Flags Attribute:");
            var flags = FeatureFlags.Read | FeatureFlags.Write;
            Console.WriteLine($"   {flags} has Write: {flags.HasFlag(FeatureFlags.Write)}");

            Console.WriteLine("\n=== Part 2 Complete ===");
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AttributeBase : Attribute
    {
        public string Name { get; }
        public AttributeBase(string name) { Name = name; }
    }

    [AttributeBase("First")]
    [AttributeBase("Second")]
    public class MultiAttrClass { }

    [Flags]
    public enum FeatureFlags { None = 0, Read = 1, Write = 2, Execute = 4 }
}
