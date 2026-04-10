/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Attributes - Real-World Applications
 * FILE      : 06_Attributes_RealWorld.cs
 * PURPOSE   : Demonstrates practical real-world applications of attributes - logging, validation, serialization
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for collections
using System.Reflection; // needed for reflection

namespace CSharp_MasterGuide._10_ReflectionMetadata._02_Attributes
{
    /// <summary>
    /// Real-world applications of custom attributes
    /// </summary>
    public class Attributes_RealWorld
    {
        /// <summary>
        /// Entry point for real-world attribute examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Real-World Attributes Demo ===
            Console.WriteLine("=== Real-World Attributes Demo ===\n");

            // ── REAL-WORLD: JSON Serialization ────────────────────────────────
            // Output: 1. JSON Serialization with Attributes:
            Console.WriteLine("1. JSON Serialization with Attributes:");
            
            // Create object with serialization attributes
            var user = new SerializedUser
            {
                // User properties
                Id = 123,
                UserName = "alice",
                Email = "alice@example.com",
                Password = "secret123",
                InternalCode = "ABC123"
            };
            
            // SerializeToJson converts using attributes
            string json = SerializeToJson(user);
            
            // Output: JSON: [serialized string]
            Console.WriteLine($"   JSON: {json}");

            // ── REAL-WORLD: Property Validation ────────────────────────────────
            // Output: 2. Property Validation with Attributes:
            Console.WriteLine("\n2. Property Validation with Attributes:");
            
            // Validate object using validation attributes
            var person = new ValidatedPerson
            {
                Name = "",  // Invalid - empty
                Age = -5,   // Invalid - negative
                Email = "not-an-email"  // Invalid - not email format
            };
            
            // ValidateObject returns list of errors
            List<string> errors = ValidateObject(person);
            
            // foreach = iterate errors
            foreach (string error in errors)
            {
                // Output: Validation error: [error message]
                Console.WriteLine($"   Validation error: {error}");
            }

            // ── REAL-WORLD: Database Mapping ─────────────────────────────────
            // Output: 3. Database Mapping with Attributes:
            Console.WriteLine("\n3. Database Mapping with Attributes:");
            
            // Get database column mapping
            Type entityType = typeof(DatabaseEntity);
            
            // GetProperty finds property
            PropertyInfo prop = entityType.GetProperty("UserId");
            
            if (prop != null)
            {
                // GetCustomAttribute retrieves column attribute
                // ColumnAttribute = database column mapping
                ColumnAttribute colAttr = prop.GetCustomAttribute<ColumnAttribute>();
                
                if (colAttr != null)
                {
                    // colAttr.Name = database column name
                    // colAttr.IsPrimaryKey = whether it's PK
                    // Output: Column: [name], PK: [True/False]
                    Console.WriteLine($"   Column: {colAttr.Name}, PK: {colAttr.IsPrimaryKey}");
                }
            }

            // ── REAL-WORLD: API Route Mapping ────────────────────────────────
            // Output: 4. API Route Mapping with Attributes:
            Console.WriteLine("\n4. API Route Mapping with Attributes:");
            
            // Find all methods with route attributes
            // GetMethods returns public methods
            MethodInfo[] methods = typeof(ApiController).GetMethods();
            
            // foreach = iterate methods
            foreach (MethodInfo method in methods)
            {
                // GetCustomAttribute checks for route attribute
                RouteAttribute routeAttr = method.GetCustomAttribute<RouteAttribute>();
                
                if (routeAttr != null)
                {
                    // method.Name = method name
                    // routeAttr.Template = route template
                    // routeAttr.HttpMethod = HTTP method (GET, POST, etc.)
                    // Output: [HTTP]: [route] -> [method]
                    Console.WriteLine($"   {routeAttr.HttpMethod}: {routeAttr.Template} -> {method.Name}");
                }
            }

            // ── REAL-WORLD: Logging Decorator ────────────────────────────────
            // Output: 5. Logging with Attributes:
            Console.WriteLine("\n5. Logging with Attributes:");
            
            // Create service instance
            var service = new BusinessService();
            
            // GetMethod finds method to invoke
            MethodInfo executeMethod = typeof(BusinessService).GetMethod("ExecuteOperation");
            
            if (executeMethod != null)
            {
                // Check for logging attribute
                LogAttribute logAttr = executeMethod.GetCustomAttribute<LogAttribute>();
                
                if (logAttr != null)
                {
                    // logAttr.Level = logging level
                    // logAttr.Message = log message template
                    // Output: Would log: [level]: [message]
                    Console.WriteLine($"   Would log: {logAttr.Level}: {logAttr.Message}");
                }
            }

            Console.WriteLine("\n=== Real-World Attributes Complete ===");
        }

        // ── REAL-WORLD EXAMPLE: JSON Serialization ─────────────────────────
        /// <summary>
        /// Serializes object to JSON using attributes
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>JSON string representation</returns>
        public static string SerializeToJson(object obj)
        {
            // StringBuilder for building JSON output
            var json = new System.Text.StringBuilder();
            
            // GetType returns object's runtime type
            Type type = obj.GetType();
            
            // GetProperties returns public properties
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            // Append opening brace
            json.Append("{");
            
            // List to store serialized properties
            var pairs = new List<string>();
            
            // foreach = iterate properties
            foreach (PropertyInfo prop in properties)
            {
                // GetCustomAttribute checks for JsonIgnore attribute
                // JsonIgnoreAttribute = marks property to exclude
                JsonIgnoreAttribute ignore = prop.GetCustomAttribute<JsonIgnoreAttribute>();
                
                // Skip if marked to ignore
                if (ignore != null)
                    continue;
                
                // Get value from property
                object value = prop.GetValue(obj);
                
                // GetJsonName gets custom name if present
                JsonPropertyAttribute jsonAttr = prop.GetCustomAttribute<JsonPropertyAttribute>();
                
                // string name = use custom name or property name
                string name = jsonAttr != null ? jsonAttr.PropertyName : prop.Name;
                
                // Format as JSON property
                // $"\"{name}\": \"{value}\"" = JSON key-value pair
                pairs.Add($"\"{name}\": \"{value}\"");
            }
            
            // Join properties with comma
            json.Append(string.Join(", ", pairs));
            
            // Append closing brace
            json.Append("}");
            
            return json.ToString();
        }

        // ── REAL-WORLD EXAMPLE: Validation ──────────────────────────────────
        /// <summary>
        /// Validates object using validation attributes
        /// </summary>
        /// <param name="obj">Object to validate</param>
        /// <returns>List of validation error messages</returns>
        public static List<string> ValidateObject(object obj)
        {
            // List to store errors
            var errors = new List<string>();
            
            // GetType returns runtime type
            Type type = obj.GetType();
            
            // GetProperties returns public properties
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            // foreach = iterate properties
            foreach (PropertyInfo prop in properties)
            {
                // GetCustomAttribute retrieves Required attribute
                RequiredAttribute required = prop.GetCustomAttribute<RequiredAttribute>();
                
                // Get value from property
                object value = prop.GetValue(obj);
                
                // Check if required and null/empty
                if (required != null)
                {
                    // string.IsNullOrEmpty checks for null or empty string
                    if (value == null || (value is string s && string.IsNullOrEmpty(s)))
                    {
                        // prop.Name = property name
                        // required.ErrorMessage = custom error message
                        // Output: [property] is required
                        errors.Add($"{prop.Name} is required");
                    }
                }
                
                // GetCustomAttribute retrieves Range attribute
                RangeAttribute range = prop.GetCustomAttribute<RangeAttribute>();
                
                if (range != null && value != null)
                {
                    // range.IsValid checks value is in range
                    if (!range.IsValid(value))
                    {
                        // prop.Name = property name
                        // range.Minimum, range.Maximum = range bounds
                        // Output: [property] must be between [min] and [max]
                        errors.Add($"{prop.Name} must be between {range.Minimum} and {range.Maximum}");
                    }
                }
                
                // GetCustomAttribute retrieves Email attribute
                EmailAttribute email = prop.GetCustomAttribute<EmailAttribute>();
                
                if (email != null && value is string emailValue)
                {
                    // Simple email validation (contains @)
                    if (!emailValue.Contains("@"))
                    {
                        // prop.Name = property name
                        // Output: [property] must be a valid email
                        errors.Add($"{prop.Name} must be a valid email");
                    }
                }
            }
            
            return errors;
        }
    }

    // ── JSON SERIALIZATION ATTRIBUTES ─────────────────────────────────────
    /// <summary>
    /// Marks property to exclude from JSON serialization
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class JsonIgnoreAttribute : Attribute { }

    /// <summary>
    /// Specifies custom name for JSON property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class JsonPropertyAttribute : Attribute
    {
        // string PropertyName = custom name in JSON
        public string PropertyName { get; set; }
        
        public JsonPropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }

    /// <summary>
    /// Example class with JSON attributes
    /// </summary>
    public class SerializedUser
    {
        public int Id { get; set; }
        
        [JsonProperty("username")]
        public string UserName { get; set; }
        
        public string Email { get; set; }
        
        [JsonIgnore]
        public string Password { get; set; }
        
        [JsonIgnore]
        public string InternalCode { get; set; }
    }

    // ── VALIDATION ATTRIBUTES ────────────────────────────────────────────
    /// <summary>
    /// Marks property as required
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredAttribute : Attribute
    {
        // string ErrorMessage = custom error message
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// Validates value is within range
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RangeAttribute : Attribute
    {
        // int Minimum = lower bound
        public int Minimum { get; set; }
        
        // int Maximum = upper bound
        public int Maximum { get; set; }
        
        public bool IsValid(object value)
        {
            // int.TryParse checks if value can be parsed as integer
            if (value is int intVal)
            {
                return intVal >= Minimum && intVal <= Maximum;
            }
            return false;
        }
    }

    /// <summary>
    /// Validates email format
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EmailAttribute : Attribute { }

    /// <summary>
    /// Example class with validation attributes
    /// </summary>
    public class ValidatedPerson
    {
        [Required]
        public string Name { get; set; }
        
        [Range(0, 150)]
        public int Age { get; set; }
        
        [Email]
        public string Email { get; set; }
    }

    // ── DATABASE ATTRIBUTES ───────────────────────────────────────────────
    /// <summary>
    /// Maps property to database column
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        // string Name = database column name
        public string Name { get; set; }
        
        // bool IsPrimaryKey = whether column is primary key
        public bool IsPrimaryKey { get; set; }
    }

    /// <summary>
    /// Example entity with database mapping
    /// </summary>
    public class DatabaseEntity
    {
        [Column(Name = "user_id", IsPrimaryKey = true)]
        public int UserId { get; set; }
        
        [Column(Name = "user_name")]
        public string UserName { get; set; }
        
        [Column(Name = "created_at")]
        public DateTime CreatedAt { get; set; }
    }

    // ── API ROUTING ATTRIBUTES ─────────────────────────────────────────────
    /// <summary>
    /// Maps method to API route
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RouteAttribute : Attribute
    {
        // string Template = route template (URL pattern)
        public string Template { get; set; }
        
        // string HttpMethod = HTTP method (GET, POST, etc.)
        public string HttpMethod { get; set; }
    }

    /// <summary>
    /// Example API controller with routes
    /// </summary>
    public class ApiController
    {
        [RouteTemplate("api/users", HttpMethod = "GET")]
        public void GetUsers() { }
        
        [RouteTemplate("api/users/{id}", HttpMethod = "GET")]
        public void GetUserById(int id) { }
        
        [RouteTemplate("api/users", HttpMethod = "POST")]
        public void CreateUser() { }
    }

    // ── LOGGING ATTRIBUTES ─────────────────────────────────────────────────
    /// <summary>
    /// Marks method for logging
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class LogAttribute : Attribute
    {
        // string Level = logging level (Info, Warning, Error)
        public string Level { get; set; }
        
        // string Message = log message template
        public string Message { get; set; }
    }

    /// <summary>
    /// Example service with logging
    /// </summary>
    public class BusinessService
    {
        [Log(Level = "Info", Message = "Operation executed")]
        public void ExecuteOperation()
        {
            // Method body - placeholder
        }
    }
}
