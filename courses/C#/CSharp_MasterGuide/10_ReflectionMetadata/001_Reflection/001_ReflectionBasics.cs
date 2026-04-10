/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Reflection Basics
 * FILE      : 01_ReflectionBasics.cs
 * PURPOSE   : Demonstrates basic reflection - retrieving type information at runtime
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Reflection; // needed for Type, MemberInfo classes

namespace CSharp_MasterGuide._10_ReflectionMetadata._01_Reflection
{
    /// <summary>
    /// Demonstrates runtime type discovery through reflection
    /// </summary>
    public class ReflectionBasics
    {
        /// <summary>
        /// Entry point for reflection basics
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Reflection Basics Demo ===
            Console.WriteLine("=== Reflection Basics Demo ===\n");

            // ── CONCEPT: Getting Type Information ────────────────────────────
            // Type class provides metadata about any type at runtime

            // Example 1: GetType from object instance
            // Output: 1. GetType from Instance:
            Console.WriteLine("1. GetType from Instance:");
            
            // object = base type for all reference types
            string str = "Hello";
            int num = 42;
            
            // obj.GetType() = returns runtime Type object
            // Type = metadata about the type (name, namespace, members)
            // Output: string type: System.String
            Console.WriteLine($"   string: {str.GetType()}");
            // Output: int type: System.Int32
            Console.WriteLine($"   int: {num.GetType()}");

            // Example 2: GetType using typeof operator
            // Output: 2. typeof Operator:
            Console.WriteLine("\n2. typeof Operator:");
            
            // typeof(TypeName) = gets Type at compile time
            // Output: typeof(string): System.String
            Console.WriteLine($"   typeof(string): {typeof(string)}");
            // Output: typeof(List<int>): System.Collections.Generic.List`1[System.Int32]
            Console.WriteLine($"   typeof(List<int>): {typeof(System.Collections.Generic.List<int>)}");

            // Example 3: Type properties
            // Output: 3. Type Properties:
            Console.WriteLine("\n3. Type Properties:");
            
            // Type t = get type for Person class
            Type t = typeof(Person);
            
            // Type.Name = simple type name without namespace
            // Output: Type Name: Person
            Console.WriteLine($"   Type Name: {t.Name}");
            // Type.FullName = full type name with namespace
            // Output: Full Name: CSharp_MasterGuide...Person
            Console.WriteLine($"   Full Name: {t.FullName}");
            // Type.Namespace = namespace containing the type
            // Output: Namespace: CSharp_MasterGuide...
            Console.WriteLine($"   Namespace: {t.Namespace}");
            // Type.IsClass = true for classes (not structs/enums)
            // Output: Is Class: True
            Console.WriteLine($"   Is Class: {t.IsClass}");
            // Type.IsValueType = true for structs and enums
            // Output: Is Value Type: False
            Console.WriteLine($"   Is Value Type: {t.IsValueType}");

            // ── CONCEPT: Type Members ────────────────────────────────────────
            // Reflection can discover all members (methods, properties, fields)

            // Example 4: Get Members
            // Output: 4. Get Members:
            Console.WriteLine("\n4. Get Members:");
            
            // Type.GetMembers() = returns all public members
            // MemberInfo[] = array of member metadata
            MemberInfo[] members = t.GetMembers();
            
            // foreach = iterate through each member
            foreach (MemberInfo member in members)
            {
                // MemberInfo.MemberType = type of member (Method, Property, Field, etc.)
                // Output: [MemberType]: [MemberName]
                Console.WriteLine($"   {member.MemberType}: {member.Name}");
            }

            // ── CONCEPT: Get Methods ─────────────────────────────────────────
            // GetMethods returns public methods

            // Example 5: Get Methods
            // Output: 5. Get Methods:
            Console.WriteLine("\n5. Get Methods:");
            
            // BindingFlags = filter which members to retrieve
            // Instance = non-static methods only
            // Public = public methods only
            MethodInfo[] methods = t.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            
            // MethodInfo.Name = method name
            // Output: [MethodName]([ParameterTypes])
            foreach (MethodInfo method in methods)
            {
                // GetParameters = returns parameter list
                // string.Join = concatenates parameter types with commas
                string parameters = string.Join(", ", 
                    Array.ConvertAll(method.GetParameters(), p => p.ParameterType.Name));
                
                // Output: MethodName(ParamTypes)
                Console.WriteLine($"   {method.Name}({parameters})");
            }

            // ── CONCEPT: Get Properties ──────────────────────────────────────
            // GetProperties returns public properties

            // Example 6: Get Properties
            // Output: 6. Get Properties:
            Console.WriteLine("\n6. Get Properties:");
            
            // PropertyInfo[] = array of property metadata
            PropertyInfo[] properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (PropertyInfo prop in properties)
            {
                // PropertyInfo.PropertyType = type of the property value
                // Output: [PropertyName]: [PropertyType]
                Console.WriteLine($"   {prop.Name}: {prop.PropertyType.Name}");
            }

            // ── REAL-WORLD EXAMPLE: Generic Serializer ────────────────────────
            // Output: --- Real-World: Object Inspector ---
            Console.WriteLine("\n--- Real-World: Object Inspector ---");
            
            // Inspect any object using reflection
            var person = new Person { Name = "Alice", Age = 30 };
            
            // InspectObject returns string description
            string description = InspectObject(person);
            
            // Output: [inspection result]
            Console.WriteLine(description);

            Console.WriteLine("\n=== Reflection Basics Complete ===");
        }

        /// <summary>
        /// Inspects object properties using reflection
        /// </summary>
        public static string InspectObject(object obj)
        {
            // Guard against null
            if (obj == null)
                return "Object is null";
            
            // GetType returns runtime type
            Type type = obj.GetType();
            
            // StringBuilder for building output
            var sb = new System.Text.StringBuilder();
            
            // Append type name
            sb.AppendLine($"Type: {type.Name}");
            sb.AppendLine("Properties:");
            
            // GetProperties returns public instance properties
            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            // foreach = iterate properties
            foreach (PropertyInfo prop in props)
            {
                try
                {
                    // PropertyInfo.GetValue(obj) = gets property value from instance
                    // object value = the current value of the property
                    object value = prop.GetValue(obj);
                    
                    // Append formatted property info
                    // Output:   [Name]: [Value]
                    sb.AppendLine($"   {prop.Name}: {value}");
                }
                catch
                {
                    // Skip properties that can't be read
                }
            }
            
            return sb.ToString();
        }
    }

    /// <summary>
    /// Simple person class for demonstration
    /// </summary>
    public class Person
    {
        // string = reference type for name
        public string Name { get; set; }
        
        // int = value type for age
        public int Age { get; set; }

        /// <summary>
        /// Returns greeting message
        /// </summary>
        public string SayHello()
        {
            return $"Hello, I'm {Name}";
        }
    }
}
