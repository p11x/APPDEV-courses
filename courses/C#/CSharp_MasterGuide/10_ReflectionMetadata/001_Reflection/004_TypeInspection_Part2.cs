/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Type Inspection (Continued)
 * FILE      : 04_TypeInspection_Part2.cs
 * PURPOSE   : Continues type inspection with late-binding, type comparison, and more scenarios
 * ============================================================
 */
using System; // needed for Console, types
using System.Reflection; // needed for reflection

namespace CSharp_MasterGuide._10_ReflectionMetadata._01_Reflection
{
    /// <summary>
    /// Continues type inspection demonstrations
    /// </summary>
    public class TypeInspection_Part2
    {
        /// <summary>
        /// Entry point for advanced type inspection
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Type Inspection Part 2 ===
            Console.WriteLine("=== Type Inspection Part 2 ===\n");

            // ── CONCEPT: Late Binding with Generics ──────────────────────────
            // Create generic types at runtime

            // Example 1: Late Binding with Generics:
            // Output: 1. Late Binding with Generics:
            Console.WriteLine("1. Late Binding with Generics:");
            
            // Create List<T> where T = string at runtime
            // Type listType = typeof(List<>)
            Type listType = typeof(List<>);
            
            // MakeGenericType = constructs specific type from generic definition
            // Type stringListType = List<string>
            Type stringListType = listType.MakeGenericType(typeof(string));
            
            // Output: Created Type: List`1[System.String]
            Console.WriteLine($"   Created Type: {stringListType}");
            
            // Activator.CreateInstance = create instance of late-bound type
            // object = new List<string>()
            object list = Activator.CreateInstance(stringListType);
            
            // Output: Instance Type: [TypeName]
            Console.WriteLine($"   Instance Type: {list.GetType()}");

            // ── CONCEPT: Check if Type is Primitive/Numeric ──────────────────
            // Type.IsPrimitive for basic types

            // Example 2: Primitive Type Check:
            // Output: 2. Primitive Type Check:
            Console.WriteLine("\n2. Primitive Type Check:");
            
            // object[] testTypes = types to check
            object[] testTypes = { 1, "test", 3.14, true, new int[0] };
            
            foreach (object obj in testTypes)
            {
                Type t = obj.GetType();
                
                // IsPrimitive = true for int, bool, char, byte, etc.
                // IsValueType = true for structs and enums (including primitives)
                // Output: [TypeName]: Primitive=[bool], ValueType=[bool]
                Console.WriteLine($"   {t.Name}: Primitive={t.IsPrimitive}, ValueType={t.IsValueType}");
            }

            // ── CONCEPT: Compare Types ────────────────────────────────────────
            // Type equality using == and GetType()

            // Example 3: Type Comparison:
            // Output: 3. Type Comparison:
            Console.WriteLine("\n3. Type Comparison:");
            
            // object = integer value
            object intObj = 42;
            object strObj = "42";
            
            // GetType() returns Type object
            // == compares types for equality
            // Output: int and string types equal: False
            Console.WriteLine($"   int and string types equal: {intObj.GetType() == strObj.GetType()}");
            
            // typeof gets type at compile time
            // Output: int and typeof(int) equal: True
            Console.WriteLine($"   int and typeof(int) equal: {intObj.GetType() == typeof(int)}");

            // ── CONCEPT: Check Array Types ───────────────────────────────────
            // IsArray, GetElementType, GetArrayRank

            // Example 4: Array Type Check:
            // Output: 4. Array Type Check:
            Console.WriteLine("\n4. Array Type Check:");
            
            // Array types
            object[] arr1 = new int[10];
            object[,] arr2 = new int[10, 10];
            object[][] arr3 = new int[10][];
            
            // Check each array
            object[] arrays = { arr1, arr2, arr3 };
            
            foreach (object arr in arrays)
            {
                Type t = arr.GetType();
                
                // IsArray = true for array types
                // GetArrayRank = number of dimensions
                // Output: [TypeName]: IsArray=[bool], Rank=[n]
                Console.WriteLine($"   {t.Name}: IsArray={t.IsArray}, Rank={t.GetArrayRank()}");
            }

            // ── CONCEPT: Check Nullable Types ────────────────────────────────
            // Nullable.GetUnderlyingType

            // Example 5: Nullable Type Check:
            // Output: 5. Nullable Type Check:
            Console.WriteLine("\n5. Nullable Type Check:");
            
            // int? = Nullable<int>
            int? nullableInt = 42;
            int normalInt = 42;
            
            // GetUnderlyingType returns underlying type from Nullable<T>
            // Output: int? underlying: Int32
            Console.WriteLine($"   int? underlying: {Nullable.GetUnderlyingType(typeof(int?)).Name}");
            
            // IsNullable = check if type is Nullable<T>
            // Output: int?: IsNullable=False (wrong approach)
            // Output: int? is Nullable: True
            Console.WriteLine($"   int? is Nullable: {nullableInt.GetType().IsNullable()}");

            // ── REAL-WORLD EXAMPLE: Serialization Helper ─────────────────────
            // Output: --- Real-World: Serialization Helper ---
            Console.WriteLine("\n--- Real-World: Serialization Helper ---");
            
            // Check various types for serialization
            CheckSerializability(typeof(string));
            CheckSerializability(typeof(int));
            CheckSerializability(typeof(List<int>));
            CheckSerializability(typeof(object));

            Console.WriteLine("\n=== Type Inspection Part 2 Complete ===");
        }

        /// <summary>
        /// Checks if a type is nullable
        /// </summary>
        public static bool IsNullable(this Type type)
        {
            // Nullable<T> is a generic type with one type argument
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Checks if type is serializable
        /// </summary>
        public static void CheckSerializability(Type type)
        {
            // IsSerializable = true for types marked with [Serializable]
            bool serializable = type.IsSerializable;
            
            // IsValueType = value types are serializable
            // string is special - not technically serializable but works
            bool recommended = type.IsValueType || type == typeof(string);
            
            // Output: [TypeName]: [bool]
            Console.WriteLine($"   {type.Name}: Serializable={serializable}, Recommended={recommended}");
        }
    }
}
