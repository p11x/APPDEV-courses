/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Attribute Targets
 * FILE      : 05_AttributeTargets.cs
 * PURPOSE   : Demonstrates which program elements can be decorated with attributes
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Reflection; // needed for member inspection

namespace CSharp_MasterGuide._10_ReflectionMetadata._02_Attributes
{
    /// <summary>
    /// Demonstrates AttributeTargets - what can be decorated with attributes
    /// </summary>
    public class AttributeTargets
    {
        /// <summary>
        /// Entry point for attribute targets examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Attribute Targets Demo ===
            Console.WriteLine("=== Attribute Targets Demo ===\n");

            // ── CONCEPT: Class-Level Attributes ────────────────────────────────
            // AttributeTargets.Class = attributes on classes

            // Example 1: Class attributes
            // Output: 1. Class Attributes:
            Console.WriteLine("1. Class Attributes:");
            
            // Get type for class with attribute
            Type classType = typeof(AnnotatedClass);
            
            // GetCustomAttribute retrieves class-level attribute
            // ClassDescriptionAttribute = attribute defined for classes
            ClassDescriptionAttribute classAttr = classType.GetCustomAttribute<ClassDescriptionAttribute>();
            
            if (classAttr != null)
            {
                // classAttr.Description returns description text
                // Output: Class: [description]
                Console.WriteLine($"   Class: {classAttr.Description}");
            }

            // ── CONCEPT: Method-Level Attributes ─────────────────────────────
            // AttributeTargets.Method = attributes on methods

            // Example 2: Method attributes
            // Output: 2. Method Attributes:
            Console.WriteLine("\n2. Method Attributes:");
            
            // GetMethod finds specific method by name
            // MethodInfo = method metadata
            MethodInfo doSomethingMethod = classType.GetMethod("DoSomething");
            
            if (doSomethingMethod != null)
            {
                // GetCustomAttribute checks for method attribute
                // MethodDescriptionAttribute = method-level attribute
                MethodDescriptionAttribute methodAttr = doSomethingMethod.GetCustomAttribute<MethodDescriptionAttribute>();
                
                if (methodAttr != null)
                {
                    // methodAttr.Description = description text
                    // Output: Method: [description]
                    Console.WriteLine($"   Method: {methodAttr.Description}");
                }
            }

            // ── CONCEPT: Property-Level Attributes ───────────────────────────
            // AttributeTargets.Property = attributes on properties

            // Example 3: Property attributes
            // Output: 3. Property Attributes:
            Console.WriteLine("\n3. Property Attributes:");
            
            // GetProperty finds property by name
            // PropertyInfo = property metadata
            PropertyInfo nameProperty = classType.GetProperty("Name");
            
            if (nameProperty != null)
            {
                // Property attribute example
                // PropertyDescriptionAttribute = property-level attribute
                PropertyDescriptionAttribute propAttr = nameProperty.GetCustomAttribute<PropertyDescriptionAttribute>();
                
                if (propAttr != null)
                {
                    // propAttr.Description = property description
                    // Output: Property: [description]
                    Console.WriteLine($"   Property: {propAttr.Description}");
                }
            }

            // ── CONCEPT: Parameter Attributes ────────────────────────────────
            // AttributeTargets.Parameter = attributes on method parameters

            // Example 4: Parameter attributes
            // Output: 4. Parameter Attributes:
            Console.WriteLine("\n4. Parameter Attributes:");
            
            // GetMethod finds parameterized method
            MethodInfo processMethod = classType.GetMethod("ProcessData");
            
            if (processMethod != null)
            {
                // GetParameters returns parameter array
                // ParameterInfo[] = array of parameter metadata
                ParameterInfo[] parameters = processMethod.GetParameters();
                
                // foreach = iterate through parameters
                foreach (ParameterInfo param in parameters)
                {
                    // GetCustomAttribute checks for parameter attribute
                    // ParameterDescriptionAttribute = parameter-level attribute
                    ParameterDescriptionAttribute paramAttr = param.GetCustomAttribute<ParameterDescriptionAttribute>();
                    
                    if (paramAttr != null)
                    {
                        // paramAttr.Description = parameter description
                        // param.Name = parameter name
                        // Output: Parameter [name]: [description]
                        Console.WriteLine($"   Parameter {param.Name}: {paramAttr.Description}");
                    }
                }
            }

            // ── CONCEPT: Field-Level Attributes ────────────────────────────────
            // AttributeTargets.Field = attributes on fields

            // Example 5: Field attributes
            // Output: 5. Field Attributes:
            Console.WriteLine("\n5. Field Attributes:");
            
            // GetField finds field by name
            // FieldInfo = field metadata
            FieldInfo secretField = classType.GetField("_secretValue");
            
            if (secretField != null)
            {
                // Field attribute example
                // FieldDescriptionAttribute = field-level attribute
                FieldDescriptionAttribute fieldAttr = secretField.GetCustomAttribute<FieldDescriptionAttribute>();
                
                if (fieldAttr != null)
                {
                    // fieldAttr.Description = field description
                    // Output: Field: [description]
                    Console.WriteLine($"   Field: {fieldAttr.Description}");
                }
            }

            // ── CONCEPT: Enum Attributes ─────────────────────────────────────
            // AttributeTargets.Enum = attributes on enums

            // Example 6: Enum attributes
            // Output: 6. Enum Attributes:
            Console.WriteLine("\n6. Enum Attributes:");
            
            // Get type for enum
            Type statusType = typeof(StatusCode);
            
            // GetCustomAttribute checks enum-level attribute
            // EnumDescriptionAttribute = enum-level attribute
            EnumDescriptionAttribute enumAttr = statusType.GetCustomAttribute<EnumDescriptionAttribute>();
            
            if (enumAttr != null)
            {
                // enumAttr.Description = enum description
                // Output: Enum: [description]
                Console.WriteLine($"   Enum: {enumAttr.Description}");
            }

            // ── REAL-WORLD EXAMPLE: API Documentation ─────────────────────
            // Output: --- Real-World: API Documentation ---
            Console.WriteLine("\n--- Real-World: API Documentation ---");
            
            // Generate API documentation
            GenerateApiDocs(typeof(AnnotatedClass));

            Console.WriteLine("\n=== Attribute Targets Complete ===");
        }

        /// <summary>
        /// Generates API documentation from attributes
        /// </summary>
        /// <param name="type">Type to generate docs for</param>
        public static void GenerateApiDocs(Type type)
        {
            // Get class description
            var classAttr = type.GetCustomAttribute<ClassDescriptionAttribute>();
            if (classAttr != null)
            {
                // Output: Class: [description]
                Console.WriteLine($"   Class: {classAttr.Description}");
            }

            // Get all public methods with attributes
            // GetMethods returns public methods
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            
            // foreach = iterate methods
            foreach (MethodInfo method in methods)
            {
                // GetCustomAttribute gets method attribute
                var methodAttr = method.GetCustomAttribute<MethodDescriptionAttribute>();
                
                if (methodAttr != null)
                {
                    // method.Name = method name
                    // Output:   - [method]: [description]
                    Console.WriteLine($"   - {method.Name}: {methodAttr.Description}");
                }
            }
        }
    }

    // ── EXAMPLE ATTRIBUTES ────────────────────────────────────────────────
    /// <summary>
    /// Attribute for classes - demonstrates AttributeTargets.Class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ClassDescriptionAttribute : Attribute
    {
        // string Description = description text for class
        public string Description { get; }
        
        public ClassDescriptionAttribute(string description)
        {
            Description = description;
        }
    }

    /// <summary>
    /// Attribute for methods - demonstrates AttributeTargets.Method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodDescriptionAttribute : Attribute
    {
        // string Description = description text for method
        public string Description { get; }
        
        public MethodDescriptionAttribute(string description)
        {
            Description = description;
        }
    }

    /// <summary>
    /// Attribute for properties - demonstrates AttributeTargets.Property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyDescriptionAttribute : Attribute
    {
        // string Description = description text for property
        public string Description { get; }
        
        public PropertyDescriptionAttribute(string description)
        {
            Description = description;
        }
    }

    /// <summary>
    /// Attribute for parameters - demonstrates AttributeTargets.Parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ParameterDescriptionAttribute : Attribute
    {
        // string Description = description text for parameter
        public string Description { get; }
        
        public ParameterDescriptionAttribute(string description)
        {
            Description = description;
        }
    }

    /// <summary>
    /// Attribute for fields - demonstrates AttributeTargets.Field
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FieldDescriptionAttribute : Attribute
    {
        // string Description = description text for field
        public string Description { get; }
        
        public FieldDescriptionAttribute(string description)
        {
            Description = description;
        }
    }

    /// <summary>
    /// Attribute for enums - demonstrates AttributeTargets.Enum
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class EnumDescriptionAttribute : Attribute
    {
        // string Description = description text for enum
        public string Description { get; }
        
        public EnumDescriptionAttribute(string description)
        {
            Description = description;
        }
    }

    // ── EXAMPLE CLASSES ───────────────────────────────────────────────────
    /// <summary>
    /// Class with multiple attribute types - demonstrates various AttributeTargets
    /// </summary>
    [ClassDescription("This is an annotated class for demonstration")]
    public class AnnotatedClass
    {
        // Field with attribute - demonstrates AttributeTargets.Field
        [FieldDescription("This is a private field")]
        private string _secretValue = "hidden";

        // Property with attribute - demonstrates AttributeTargets.Property
        [PropertyDescription("The name of the entity")]
        public string Name { get; set; }

        // Method with attribute - demonstrates AttributeTargets.Method
        [MethodDescription("This method does something useful")]
        public void DoSomething()
        {
            // Method body - placeholder
        }

        // Method with parameter attributes - demonstrates AttributeTargets.Parameter
        [MethodDescription("Processes data with the given input")]
        public void ProcessData(
            [ParameterDescription("Input data to process")] string inputData,
            [ParameterDescription("Processing options")] int options)
        {
            // Method body - placeholder
        }
    }

    // Enum with attribute - demonstrates AttributeTargets.Enum
    [EnumDescription("Status codes returned by the API")]
    public enum StatusCode
    {
        // Enum members
        Success = 200,
        NotFound = 404,
        Error = 500
    }
}
