/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Reflection Real-World Applications
 * FILE      : 08_Reflection_RealWorld.cs
 * PURPOSE   : Real-world applications of reflection - serialization, DI, plugins
 * ============================================================
 */
using System; // needed for Console
using System.Collections.Generic; // needed for collections
using System.Reflection; // needed for reflection

namespace CSharp_MasterGuide._10_ReflectionMetadata._01_Reflection
{
    /// <summary>
    /// Real-world reflection applications
    /// </summary>
    public class Reflection_RealWorld
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Real-World Reflection Demo ===\n");

            // Example 1: AutoMapper-style property copying
            Console.WriteLine("1. AutoMapper-style Copy:");
            var source = new SourceClass { Name = "Alice", Age = 30, Email = "alice@email.com" };
            var dest = new DestClass();
            CopyProperties(source, dest);
            Console.WriteLine($"   Copied: {dest.Name}, {dest.Age}");

            // Example 2: DI Container
            Console.WriteLine("\n2. Simple DI Container:");
            var container = new SimpleContainer();
            container.Register<Service>();
            var svc = container.Resolve<Service>();
            Console.WriteLine($"   Resolved: {svc.GetType().Name}");

            // Example 3: Property Validator
            Console.WriteLine("\n3. Property Validator:");
            var person = new PersonValidate { Name = "", Age = -5 };
            var errors = ValidateObject(person);
            Console.WriteLine($"   Errors: {string.Join(", ", errors)}");

            Console.WriteLine("\n=== Real-World Reflection Complete ===");
        }

        public static void CopyProperties(object source, object dest)
        {
            Type srcType = source.GetType();
            Type dstType = dest.GetType();
            foreach (var prop in srcType.GetProperties())
            {
                var dstProp = dstType.GetProperty(prop.Name);
                if (dstProp != null && dstProp.CanWrite)
                {
                    dstProp.SetValue(dest, prop.GetValue(source));
                }
            }
        }

        public static List<string> ValidateObject(object obj)
        {
            var errors = new List<string>();
            Type type = obj.GetType();
            foreach (var prop in type.GetProperties())
            {
                var value = prop.GetValue(obj);
                if (value is string str && string.IsNullOrEmpty(str))
                    errors.Add($"{prop.Name} is empty");
                if (prop.Name == "Age" && value is int age && age < 0)
                    errors.Add($"{prop.Name} is negative");
            }
            return errors;
        }
    }

    public class SourceClass { public string Name { get; set; } public int Age { get; set; } public string Email { get; set; } }
    public class DestClass { public string Name { get; set; } public int Age { get; set; } }
    public class SimpleContainer { private Dictionary<Type, Type> _ registrations = new Dictionary<Type, Type>(); public void Register<T>() { _registrations[typeof(T)] = typeof(T); } public T Resolve<T>() { return (T)Activator.CreateInstance(_registrations[typeof(T)]); } }
    public class Service { }
    public class PersonValidate { public string Name { get; set; } public int Age { get; set; } }
}
