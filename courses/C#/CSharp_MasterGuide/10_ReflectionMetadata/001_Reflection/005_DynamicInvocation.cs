/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Dynamic Invocation
 * FILE      : 05_DynamicInvocation.cs
 * PURPOSE   : Demonstrates dynamically invoking methods at runtime using MethodInfo and PropertyInfo
 * ============================================================
 */
using System; // needed for Console, types
using System.Reflection; // needed for reflection
using System.Collections.Generic; // needed for List

namespace CSharp_MasterGuide._10_ReflectionMetadata._01_Reflection
{
    /// <summary>
    /// Demonstrates dynamic method and property invocation
    /// </summary>
    public class DynamicInvocation
    {
        /// <summary>
        /// Entry point for dynamic invocation
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Dynamic Invocation Demo ===
            Console.WriteLine("=== Dynamic Invocation Demo ===\n");

            // ── CONCEPT: Invoke Method by Name ───────────────────────────────
            // GetMethod finds method, Invoke calls it

            // Example 1: Invoke Method by Name:
            // Output: 1. Invoke Method by Name:
            Console.WriteLine("1. Invoke Method by Name:");
            
            // var calculator = new Calculator()
            var calculator = new Calculator();
            
            // GetMethod finds method by name
            // MethodInfo = method metadata
            MethodInfo addMethod = typeof(Calculator).GetMethod("Add");
            
            // Invoke calls the method
            // object[] args = parameters to pass
            // First param is null for instance method (non-static)
            object result = addMethod.Invoke(calculator, new object[] { 5, 3 });
            
            // Output: Add(5, 3) = 8
            Console.WriteLine($"   Add(5, 3) = {result}");

            // ── CONCEPT: Invoke Generic Method ───────────────────────────────
            // MakeGenericMethod for generic methods

            // Example 2: Invoke Generic Method:
            // Output: 2. Invoke Generic Method:
            Console.WriteLine("\n2. Invoke Generic Method:");
            
            // Get generic method from type
            MethodInfo genericMethod = typeof(Container).GetMethod("GetFirst");
            
            // MakeGenericMethod creates specific version with type argument
            // MethodInfo closedMethod = GetFirst<string>()
            MethodInfo closedMethod = genericMethod.MakeGenericMethod(typeof(string));
            
            // Create instance of Container
            var container = new Container { Items = new List<string> { "First", "Second" } };
            
            // Invoke generic method
            string first = (string)closedMethod.Invoke(container, null);
            
            // Output: GetFirst<string>() = First
            Console.WriteLine($"   GetFirst<string>() = {first}");

            // ── CONCEPT: Get/Set Property Dynamically ────────────────────────
            // PropertyInfo.GetValue/SetValue

            // Example 3: Property Access:
            // Output: 3. Property Access:
            Console.WriteLine("\n3. Property Access:");
            
            var person = new Person3();
            
            // GetProperty finds property by name
            // PropertyInfo = property metadata
            PropertyInfo nameProp = typeof(Person3).GetProperty("Name");
            
            // SetValue writes property value
            nameProp.SetValue(person, "Alice");
            
            // GetValue reads property value
            string name = (string)nameProp.GetValue(person);
            
            // Output: Set Name to Alice, Got: Alice
            Console.WriteLine($"   Set Name to Alice, Got: {name}");

            // ── CONCEPT: Dynamic Method Selection ───────────────────────────
            // Choose method at runtime based on input

            // Example 4: Dynamic Method Selection:
            // Output: 4. Dynamic Method Selection:
            Console.WriteLine("\n4. Dynamic Method Selection:");
            
            // Process using reflection
            ProcessOperation(calculator, "Add", 10, 20);
            ProcessOperation(calculator, "Subtract", 10, 20);
            ProcessOperation(calculator, "Multiply", 10, 20);
            ProcessOperation(calculator, "Divide", 10, 2);

            // ── REAL-WORLD EXAMPLE: Command Dispatcher ───────────────────────
            // Output: --- Real-World: Command Dispatcher ---
            Console.WriteLine("\n--- Real-World: Command Dispatcher ---");
            
            // Execute commands by name
            var dispatcher = new CommandDispatcher();
            
            // Output: [command name]: [result]
            Console.WriteLine($"   greet: {dispatcher.Execute("greet", new object[] { })}");
            Console.WriteLine($"   add: {dispatcher.Execute("add", new object[] { 100, 200 })}");
            Console.WriteLine($"   format: {dispatcher.Execute("format", new object[] { "hello" })}");

            Console.WriteLine("\n=== Dynamic Invocation Complete ===");
        }

        /// <summary>
        /// Dynamically invokes operation on calculator
        /// </summary>
        public static void ProcessOperation(object target, string methodName, int a, int b)
        {
            // GetMethod finds method by name and parameter types
            // BindingFlags = search options
            MethodInfo method = target.GetType().GetMethod(methodName, 
                BindingFlags.Public | BindingFlags.Instance);
            
            if (method != null)
            {
                // Invoke calls method with arguments
                object result = method.Invoke(target, new object[] { a, b });
                
                // Output: [MethodName]([a], [b]) = [result]
                Console.WriteLine($"   {methodName}({a}, {b}) = {result}");
            }
        }
    }

    /// <summary>
    /// Calculator class for demo
    /// </summary>
    public class Calculator
    {
        public int Add(int a, int b) => a + b;
        public int Subtract(int a, int b) => a - b;
        public int Multiply(int a, int b) => a * b;
        public double Divide(int a, int b) => b != 0 ? (double)a / b : 0;
    }

    /// <summary>
    /// Container with generic method
    /// </summary>
    public class Container
    {
        public List<string> Items { get; set; }
        
        public T GetFirst<T>()
        {
            if (Items.Count > 0)
                return (T)(object)Items[0];
            return default;
        }
    }

    /// <summary>
    /// Person with properties
    /// </summary>
    public class Person3
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    /// <summary>
    /// Command dispatcher for real-world example
    /// </summary>
    public class CommandDispatcher
    {
        /// <summary>
        /// Executes command by name
        /// </summary>
        public string Execute(string command, object[] args)
        {
            // GetMethod finds command handler
            MethodInfo method = GetType().GetMethod(command);
            
            if (method != null)
            {
                // Invoke method with arguments
                return method.Invoke(this, args)?.ToString() ?? "Done";
            }
            
            return "Unknown command";
        }

        public string greet() => "Hello!";
        public int add(int a, int b) => a + b;
        public string format(string text) => text.ToUpper();
    }
}
