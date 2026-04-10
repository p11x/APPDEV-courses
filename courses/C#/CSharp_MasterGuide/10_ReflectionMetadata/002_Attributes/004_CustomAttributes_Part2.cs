/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Custom Attributes - Continued
 * FILE      : 04_CustomAttributes_Part2.cs
 * PURPOSE   : Demonstrates advanced custom attribute patterns - constructors, validation, and usage
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Reflection; // needed for attribute inspection

namespace CSharp_MasterGuide._10_ReflectionMetadata._02_Attributes
{
    /// <summary>
    /// Demonstrates advanced custom attribute patterns with constructors and validation
    /// </summary>
    public class CustomAttributes_Part2
    {
        /// <summary>
        /// Entry point for custom attribute part 2 examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Custom Attributes Part 2 Demo ===
            Console.WriteLine("=== Custom Attributes Part 2 Demo ===\n");

            // ── CONCEPT: Attribute Constructor Parameters ───────────────────
            // Attributes can have required and optional constructor parameters

            // Example 1: Required vs Optional parameters
            // Output: 1. Constructor Parameters:
            Console.WriteLine("1. Constructor Parameters:");
            
            // Get type with attribute
            Type t = typeof(DataField);
            
            // GetCustomAttribute retrieves attribute instance
            // DataFieldAttribute = attribute type
            // returns null if not found
            DataFieldAttribute attr = t.GetProperty("Value").GetCustomAttribute<DataFieldAttribute>();
            
            if (attr != null)
            {
                // attr.FieldName = required parameter value
                // attr.IsRequired = optional parameter value (has default)
                // Output: Field: [name], Required: [bool]
                Console.WriteLine($"   Field: {attr.FieldName}, Required: {attr.IsRequired}");
            }

            // ── CONCEPT: Attribute with Validation Logic ──────────────────────
            // Attributes can include validation methods

            // Example 2: Validation attribute
            // Output: 2. Validation Attribute:
            Console.WriteLine("\n2. Validation Attribute:");
            
            // Test range validation
            var validator = new RangeAttribute(0, 100);
            // RangeAttribute.IsValid checks value within range
            // Output: 50 is valid: True
            Console.WriteLine($"   50 is valid: {validator.IsValid(50)}");
            // Output: 150 is valid: False
            Console.WriteLine($"   150 is valid: {validator.IsValid(150)}");

            // ── CONCEPT: Multiple Attribute Instances ─────────────────────────
            // AllowMultiple = true allows multiple attributes

            // Example 3: Multiple attributes
            // Output: 3. Multiple Attributes:
            Console.WriteLine("\n3. Multiple Attributes:");
            
            Type multiType = typeof(MultiAttributedClass);
            // GetCustomAttributes returns all attribute instances (allows multiple)
            object[] attrs = multiType.GetCustomAttributes(typeof(AuthorAttribute), false);
            
            // foreach = iterate through attribute array
            foreach (AuthorAttribute author in attrs)
            {
                // Output: Author: [name], Version: [version]
                Console.WriteLine($"   Author: {author.Name}, Version: {author.Version}");
            }

            // ── CONCEPT: Attribute Inheritance ─────────────────────────────────
            // Attributes can be applied to derived classes

            // Example 4: Attribute inheritance
            // Output: 4. Attribute Inheritance:
            Console.WriteLine("\n4. Attribute Inheritance:");
            
            // Check child class for parent's attributes
            Type childType = typeof(ChildClass);
            // GetCustomAttribute(inherit: true) searches base classes
            // InheritedAttribute = attribute that supports inheritance
            InheritedAttribute inherited = childType.GetCustomAttribute<InheritedAttribute>(true);
            
            // Output: Child has inherited attr: [True/False]
            Console.WriteLine($"   Child has inherited attr: {inherited != null}");

            // ── REAL-WORLD EXAMPLE: API Versioning ───────────────────────────
            // Output: --- Real-World: API Versioning ---
            Console.WriteLine("\n--- Real-World: API Versioning ---");
            
            // Check controller version
            var apiController = new ApiController();
            Type apiType = apiController.GetType();
            
            // GetCustomAttribute checks for API version attribute
            ApiVersionAttribute apiVersion = apiType.GetCustomAttribute<ApiVersionAttribute>();
            
            if (apiVersion != null)
            {
                // apiVersion.Version returns version string
                // Output: API Version: v[version]
                Console.WriteLine($"   API Version: v{apiVersion.Version}");
            }

            Console.WriteLine("\n=== Custom Attributes Part 2 Complete ===");
        }
    }

    // ── EXAMPLE CLASSES ───────────────────────────────────────────────────
    /// <summary>
    /// Data field attribute with constructor parameters
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DataFieldAttribute : Attribute
    {
        // string FieldName = required parameter (no default value)
        public string FieldName { get; }
        
        // bool IsRequired = optional parameter (has default value)
        public bool IsRequired { get; set; }
        
        // Constructor with required parameter
        public DataFieldAttribute(string fieldName)
        {
            FieldName = fieldName;
            // IsRequired defaults to false
            IsRequired = false;
        }
    }

    /// <summary>
    /// Data class with annotated properties
    /// </summary>
    public class DataField
    {
        // [DataField] marks property as data field
        [DataField("user_name", IsRequired = true)]
        public string Value { get; set; }
    }

    /// <summary>
    /// Range validation attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RangeAttribute : Attribute
    {
        // int Minimum = lower bound of valid range
        public int Minimum { get; }
        
        // int Maximum = upper bound of valid range
        public int Maximum { get; }
        
        public RangeAttribute(int min, int max)
        {
            Minimum = min;
            Maximum = max;
        }
        
        /// <summary>
        /// Validates that value is within range
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        public bool IsValid(object value)
        {
            // int.TryParse attempts to parse value as integer
            // returns false if not convertible
            if (value is int intValue)
            {
                // return true if between min and max (inclusive)
                return intValue >= Minimum && intValue <= Maximum;
            }
            return false;
        }
    }

    /// <summary>
    /// Author attribute - allows multiple instances
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AuthorAttribute : Attribute
    {
        // string Name = author name (required)
        public string Name { get; }
        
        // string Version = version number (optional)
        public string Version { get; set; }
        
        public AuthorAttribute(string name)
        {
            Name = name;
            Version = "1.0";
        }
    }

    // Multiple author attributes on same class
    [Author("Alice", Version = "1.0")]
    [Author("Bob", Version = "2.0")]
    public class MultiAttributedClass { }

    /// <summary>
    /// Inherited attribute - can be inherited by derived classes
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class InheritedAttribute : Attribute
    {
        public string Value { get; set; }
    }

    // Base class with attribute
    [Inherited(Value = "Base")]
    public class BaseClass { }

    // Derived class inherits attribute
    public class ChildClass : BaseClass { }

    /// <summary>
    /// API Version attribute for real-world example
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ApiVersionAttribute : Attribute
    {
        // string Version = API version number
        public string Version { get; }
        
        public ApiVersionAttribute(string version)
        {
            Version = version;
        }
    }

    // Apply API version attribute
    [ApiVersion("1.0")]
    public class ApiController { }
}
