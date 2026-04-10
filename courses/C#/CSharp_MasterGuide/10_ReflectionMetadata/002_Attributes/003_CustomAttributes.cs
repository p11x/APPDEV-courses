/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Custom Attributes
 * FILE      : 03_CustomAttributes.cs
 * PURPOSE   : Creating custom attributes with properties and validation
 * ============================================================
 */
using System; // needed for Console
using System.Reflection; // needed for reflection

namespace CSharp_MasterGuide._10_ReflectionMetadata._02_Attributes
{
    /// <summary>
    /// Demonstrates custom attribute creation
    /// </summary>
    public class CustomAttributes
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Custom Attributes Demo ===\n");

            // Create custom attribute
            Console.WriteLine("1. Custom Attribute:");
            var attr = new VersionAttribute(1, 0);
            Console.WriteLine($"   Version: {attr.Major}.{attr.Minor}");

            // Validate attributes
            Console.WriteLine("\n2. Validate Attributes:");
            var type = typeof(ValidatedClass);
            Validate(type);

            Console.WriteLine("\n=== Custom Attributes Complete ===");
        }

        public static void Validate(Type type)
        {
            var required = type.GetCustomAttribute<RequiredAttribute>();
            if (required != null)
                Console.WriteLine($"   {type.Name} is required");
            else
                Console.WriteLine($"   {type.Name} is optional");
        }
    }

    public class VersionAttribute : Attribute
    {
        public int Major { get; }
        public int Minor { get; }
        public VersionAttribute(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }
    }

    [Required]
    public class ValidatedClass { }

    [AttributeUsage(AttributeTargets.Class)]
    public class RequiredAttribute : Attribute { }
}
