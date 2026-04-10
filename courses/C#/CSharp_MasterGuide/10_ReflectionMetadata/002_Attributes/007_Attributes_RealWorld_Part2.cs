/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Attributes - Real-World Part 2
 * FILE      : 07_Attributes_RealWorld_Part2.cs
 * PURPOSE   : More real-world attribute applications - caching, authorization, dependency injection
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for collections
using System.Reflection; // needed for reflection

namespace CSharp_MasterGuide._10_ReflectionMetadata._02_Attributes
{
    /// <summary>
    /// More real-world attribute applications
    /// </summary>
    public class Attributes_RealWorld_Part2
    {
        /// <summary>
        /// Entry point for more real-world attribute examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Real-World Attributes Part 2 ===
            Console.WriteLine("=== Real-World Attributes Part 2 ===\n");

            // ── REAL-WORLD: Caching ─────────────────────────────────────────
            // Output: 1. Caching with Attributes:
            Console.WriteLine("1. Caching with Attributes:");
            
            // Check method for cache attribute
            MethodInfo method = typeof(CacheService).GetMethod("GetData");
            
            if (method != null)
            {
                // GetCustomAttribute checks for cache attribute
                // CacheAttribute = marks method result as cacheable
                CacheAttribute cacheAttr = method.GetCustomAttribute<CacheAttribute>();
                
                if (cacheAttr != null)
                {
                    // cacheAttr.DurationSeconds = cache duration
                    // cacheAttr.Key = cache key prefix
                    // Output: Cache: [key], Duration: [seconds]s
                    Console.WriteLine($"   Cache: {cacheAttr.Key}, Duration: {cacheAttr.DurationSeconds}s");
                }
            }

            // ── REAL-WORLD: Authorization ───────────────────────────────────
            // Output: 2. Authorization with Attributes:
            Console.WriteLine("\n2. Authorization with Attributes:");
            
            // Get method with authorization
            MethodInfo adminMethod = typeof(SecureService).GetMethod("AdminOnly");
            
            if (adminMethod != null)
            {
                // GetCustomAttribute retrieves authorize attribute
                // AuthorizeAttribute = requires specific role or permission
                AuthorizeAttribute authAttr = adminMethod.GetCustomAttribute<AuthorizeAttribute>();
                
                if (authAttr != null)
                {
                    // authAttr.Roles = required roles
                    // authAttr.Policy = authorization policy name
                    // Output: Requires role: [role]
                    Console.WriteLine($"   Requires role: {authAttr.Roles}");
                }
            }

            // ── REAL-WORLD: Dependency Injection ────────────────────────────
            // Output: 3. Dependency Injection with Attributes:
            Console.WriteLine("\n3. Dependency Injection with Attributes:");
            
            // Get properties with inject attribute
            PropertyInfo[] properties = typeof(ConstructorDi).GetProperties();
            
            // foreach = iterate properties
            foreach (PropertyInfo prop in properties)
            {
                // GetCustomAttribute checks for inject attribute
                // InjectAttribute = marks property for DI
                InjectAttribute injectAttr = prop.GetCustomAttribute<InjectAttribute>();
                
                if (injectAttr != null)
                {
                    // prop.Name = property name
                    // injectAttr.ServiceType = type to inject
                    // Output: Inject: [property] -> [type]
                    Console.WriteLine($"   Inject: {prop.Name} -> {injectAttr.ServiceType.Name}");
                }
            }

            // ── REAL-WORLD: Display Formatting ───────────────────────────────
            // Output: 4. Display Formatting with Attributes:
            Console.WriteLine("\n4. Display Formatting with Attributes:");
            
            // Get property with display attribute
            PropertyInfo displayProp = typeof(DisplayModel).GetProperty("FullName");
            
            if (displayProp != null)
            {
                // GetCustomAttribute retrieves display attribute
                // DisplayAttribute = UI display formatting
                DisplayAttribute displayAttr = displayProp.GetCustomAttribute<DisplayAttribute>();
                
                if (displayAttr != null)
                {
                    // displayAttr.Name = display name
                    // displayAttr.Format = format string
                    // Output: Display: [name], Format: [format]
                    Console.WriteLine($"   Display: {displayAttr.Name}, Format: {displayAttr.Format}");
                }
            }

            // ── REAL-WORLD: Test Data Generation ────────────────────────────
            // Output: 5. Test Data Generation:
            Console.WriteLine("\n5. Test Data Generation:");
            
            // Generate test data using attributes
            var generator = new TestDataGenerator();
            var testUser = generator.Generate<UserTestData>();
            
            // testUser properties populated from attributes
            // Output: Generated: [name], Age: [age], Email: [email]
            Console.WriteLine($"   Generated: {testUser.Name}, Age: {testUser.Age}, Email: {testUser.Email}");

            Console.WriteLine("\n=== Real-World Attributes Part 2 Complete ===");
        }
    }

    // ── CACHING ATTRIBUTES ────────────────────────────────────────────────
    /// <summary>
    /// Marks method result as cacheable
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheAttribute : Attribute
    {
        // string Key = cache key prefix
        public string Key { get; set; }
        
        // int DurationSeconds = cache duration in seconds
        public int DurationSeconds { get; set; }
        
        public CacheAttribute(string key, int durationSeconds = 300)
        {
            Key = key;
            DurationSeconds = durationSeconds;
        }
    }

    /// <summary>
    /// Service with cached method
    /// </summary>
    public class CacheService
    {
        [Cache("user_data", DurationSeconds = 60)]
        public object GetData(string id)
        {
            // Method body - would fetch and cache data
            return new { Data = "cached" };
        }
    }

    // ── AUTHORIZATION ATTRIBUTES ─────────────────────────────────────────
    /// <summary>
    /// Requires authorization to access
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AuthorizeAttribute : Attribute
    {
        // string Roles = comma-separated required roles
        public string Roles { get; set; }
        
        // string Policy = authorization policy name
        public string Policy { get; set; }
    }

    /// <summary>
    /// Secure service with authorization
    /// </summary>
    public class SecureService
    {
        [Authorize(Roles = "Admin")]
        public void AdminOnly()
        {
            // Method body - admin only
        }
        
        public void PublicMethod()
        {
            // Method body - anyone can access
        }
    }

    // ── DEPENDENCY INJECTION ATTRIBUTES ────────────────────────────────────
    /// <summary>
    /// Marks property for dependency injection
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class InjectAttribute : Attribute
    {
        // Type ServiceType = type to inject
        public Type ServiceType { get; set; }
        
        public InjectAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }
    }

    /// <summary>
    /// Class using DI attributes
    /// </summary>
    public class ConstructorDi
    {
        [Inject(typeof(ILogger))]
        public ILogger Logger { get; set; }
        
        [Inject(typeof(IRepository))]
        public IRepository Repository { get; set; }
    }

    // Example interfaces and classes for DI
    public interface ILogger { }
    public interface IRepository { }
    public class Logger : ILogger { }
    public class Repository : IRepository { }

    // ── DISPLAY ATTRIBUTES ────────────────────────────────────────────────
    /// <summary>
    /// Controls UI display formatting
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class DisplayAttribute : Attribute
    {
        // string Name = display name
        public string Name { get; set; }
        
        // string Format = format string
        public string Format { get; set; }
        
        // int Order = display order
        public int Order { get; set; }
    }

    /// <summary>
    /// Model with display attributes
    /// </summary>
    public class DisplayModel
    {
        [Display(Name = "Full Name", Format = "TitleCase", Order = 1)]
        public string FullName { get; set; }
        
        [Display(Name = "Created Date", Format = "yyyy-MM-dd", Order = 2)]
        public DateTime CreatedDate { get; set; }
    }

    // ── TEST DATA ATTRIBUTES ─────────────────────────────────────────────
    /// <summary>
    /// Generates test data
    /// </summary>
    [AttributeUsage(PropertyTargets.Field | PropertyTargets.Property)]
    public class TestDataAttribute : Attribute
    {
        // string Generator = test data generator name
        public string Generator { get; set; }
        
        // string Format = format for data
        public string Format { get; set; }
        
        public TestDataAttribute(string generator)
        {
            Generator = generator;
        }
    }

    /// <summary>
    /// Test data generator using attributes
    /// </summary>
    public class TestDataGenerator
    {
        public T Generate<T>() where T : new()
        {
            // Create new instance
            var instance = new T();
            
            // GetType returns runtime type
            Type type = typeof(T);
            
            // GetProperties returns properties
            PropertyInfo[] properties = type.GetProperties();
            
            // foreach = iterate properties
            foreach (PropertyInfo prop in properties)
            {
                // GetCustomAttribute gets test data attribute
                TestDataAttribute attr = prop.GetCustomAttribute<TestDataAttribute>();
                
                if (attr != null)
                {
                    // Generate based on generator type
                    object value = GenerateValue(attr.Generator);
                    
                    // SetValue sets property value
                    prop.SetValue(instance, value);
                }
            }
            
            return instance;
        }
        
        /// <summary>
        /// Generates test value based on generator type
        /// </summary>
        private object GenerateValue(string generator)
        {
            // switch matches generator type
            return generator switch
            {
                "Name" => "Test User",
                "Email" => "test@example.com",
                "Age" => 25,
                _ => "default"
            };
        }
    }

    /// <summary>
    /// Test data class
    /// </summary>
    public class UserTestData
    {
        [TestData("Name")]
        public string Name { get; set; }
        
        [TestData("Email")]
        public string Email { get; set; }
        
        [TestData("Age")]
        public int Age { get; set; }
    }
}
