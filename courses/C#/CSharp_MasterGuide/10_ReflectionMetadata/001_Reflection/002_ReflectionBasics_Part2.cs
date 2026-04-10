/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Reflection Basics (Continued)
 * FILE      : 02_ReflectionBasics_Part2.cs
 * PURPOSE   : Continues reflection basics - working with constructors, fields, and invoking members
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Reflection; // needed for reflection types

namespace CSharp_MasterGuide._10_ReflectionMetadata._01_Reflection
{
    /// <summary>
    /// Continues reflection demonstrations with more advanced scenarios
    /// </summary>
    public class ReflectionBasics_Part2
    {
        /// <summary>
        /// Entry point for advanced reflection
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Reflection Basics Part 2 ===
            Console.WriteLine("=== Reflection Basics Part 2 ===\n");

            // ── CONCEPT: Getting Constructors ────────────────────────────────
            // GetConstructors returns constructor information

            // Example 1: Get Constructors
            // Output: 1. Get Constructors:
            Console.WriteLine("1. Get Constructors:");
            
            // Type = typeof(Customer)
            Type t = typeof(Customer);
            
            // GetConstructors = returns public constructors
            // ConstructorInfo[] = array of constructor metadata
            ConstructorInfo[] ctors = t.GetConstructors();
            
            foreach (ConstructorInfo ctor in ctors)
            {
                // GetParameters = constructor parameter list
                // ParameterInfo[] = array of parameter metadata
                ParameterInfo[] @params = ctor.GetParameters();
                
                // Output: Constructor with [n] parameters
                Console.WriteLine($"   Constructor with {@params.Length} parameters");
            }

            // Example 2: Create instance via reflection
            // Output: 2. Create Instance via Reflection:
            Console.WriteLine("\n2. Create Instance via Reflection:");
            
            // Activator.CreateInstance = creates instance at runtime
            // object = created instance
            object customer = Activator.CreateInstance(typeof(Customer));
            
            // Cast to Customer for property access
            Customer c = (Customer)customer;
            c.Name = "Bob";
            c.Age = 25;
            
            // Output: Created: Customer Bob, Age 25
            Console.WriteLine($"   Created: {c.Name}, Age {c.Age}");

            // ── CONCEPT: Get Fields ────────────────────────────────────────────
            // GetFields returns public fields

            // Example 3: Get Fields
            // Output: 3. Get Fields:
            Console.WriteLine("\n3. Get Fields:");
            
            // GetFields returns public fields
            // FieldInfo[] = array of field metadata
            FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (FieldInfo field in fields)
            {
                // FieldInfo.FieldType = type of the field
                // Output: [FieldName]: [FieldType]
                Console.WriteLine($"   {field.Name}: {field.FieldType.Name}");
            }

            // ── CONCEPT: Invoke Methods via Reflection ──────────────────────
            // MethodInfo.Invoke calls method at runtime

            // Example 4: Invoke Methods
            // Output: 4. Invoke Methods:
            Console.WriteLine("\n4. Invoke Methods:");
            
            // Create instance for method invocation
            var sample = new SampleClass();
            
            // GetMethod finds specific method by name
            // MethodInfo = method metadata
            MethodInfo greetMethod = typeof(SampleClass).GetMethod("Greet");
            
            // MethodInfo.Invoke = calls method on object
            // First param = object to invoke on (null for static)
            // Second param = object[] of arguments
            // object result = return value from method
            string result = (string)greetMethod.Invoke(sample, null);
            
            // Output: Result: Hello from SampleClass
            Console.WriteLine($"   Result: {result}");

            // Example 5: Invoke with parameters
            // Output: 5. Invoke with Parameters:
            Console.WriteLine("\n5. Invoke with Parameters:");
            
            // GetMethod with parameters
            MethodInfo addMethod = typeof(SampleClass).GetMethod("Add");
            
            // object[] args = arguments to pass to method
            object[] args = { 10, 20 };
            
            // Invoke returns result (sum)
            int sum = (int)addMethod.Invoke(sample, args);
            
            // Output: Sum: 30
            Console.WriteLine($"   Sum: {sum}");

            // ── CONCEPT: Get/Set Field Values ───────────────────────────────
            // FieldInfo.GetValue/SetValue for field access

            // Example 6: Get/Set Field Values
            // Output: 6. Get/Set Field Values:
            Console.WriteLine("\n6. Get/Set Field Values:");
            
            var data = new DataClass();
            
            // GetField finds field by name
            FieldInfo nameField = typeof(DataClass).GetField("Name");
            
            // GetValue = read field value from instance
            // object = current value
            string currentName = (string)nameField.GetValue(data);
            // Output: Initial Name: Default
            Console.WriteLine($"   Initial Name: {currentName}");
            
            // SetValue = write field value to instance
            nameField.SetValue(data, "Updated");
            
            // GetValue again to verify
            string newName = (string)nameField.GetValue(data);
            // Output: Updated Name: Updated
            Console.WriteLine($"   Updated Name: {newName}");

            // ── REAL-WORLD EXAMPLE: Dynamic Factory ───────────────────────────
            // Output: --- Real-World: Dynamic Factory ---
            Console.WriteLine("\n--- Real-World: Dynamic Factory ---");
            
            // Create different types dynamically
            var product = CreateInstance<Product>("Laptop", 999.99m);
            var order = CreateInstance<Order>("ORD-001", 150.00m);
            
            // Output: Product: Laptop - $999.99
            Console.WriteLine($"   Product: {product.Name} - ${product.Price}");
            // Output: Order: ORD-001 - $150.00
            Console.WriteLine($"   Order: {order.Id} - ${order.Total}");

            Console.WriteLine("\n=== Reflection Basics Part 2 Complete ===");
        }

        /// <summary>
        /// Creates instance using reflection with constructor parameters
        /// </summary>
        public static T CreateInstance<T>(params object[] args) where T : class
        {
            // Get constructor that matches argument types
            // Type[] = types of constructor parameters
            Type[] argTypes = Array.ConvertAll(args, a => a.GetType());
            
            // GetConstructor = finds matching constructor
            ConstructorInfo ctor = typeof(T).GetConstructor(argTypes);
            
            // CreateInstance with specific arguments
            return ctor != null ? ctor.Invoke(args) as T : null;
        }
    }

    /// <summary>
    /// Customer class for reflection demo
    /// </summary>
    public class Customer
    {
        public string Name { get; set; }
        public int Age { get; set; }
        
        public Customer() { }
        
        public Customer(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }

    /// <summary>
    /// Sample class with methods for invocation demo
    /// </summary>
    public class SampleClass
    {
        public string Greet()
        {
            return "Hello from SampleClass";
        }
        
        public int Add(int a, int b)
        {
            return a + b;
        }
    }

    /// <summary>
    /// Class with fields for field demo
    /// </summary>
    public class DataClass
    {
        public string Name = "Default";
        public int Value = 0;
    }

    /// <summary>
    /// Product for factory demo
    /// </summary>
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }

    /// <summary>
    /// Order for factory demo
    /// </summary>
    public class Order
    {
        public string Id { get; set; }
        public decimal Total { get; set; }
        
        public Order(string id, decimal total)
        {
            Id = id;
            Total = total;
        }
    }
}
