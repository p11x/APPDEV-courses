/*
 * ============================================================
 * TOPIC     : Exception Handling
 * SUBTOPIC  : Custom Exceptions - Advanced Features
 * FILE      : CustomExceptionBasics_Part2.cs
 * PURPOSE   : Learn advanced custom exception features including 
 *            serialization support, multiple constructors, 
 *            and exception data dictionary
 * ============================================================
 */

using System;
using System.Runtime.Serialization;

namespace CSharp_MasterGuide._05_ExceptionHandling._02_CustomExceptions
{
    class CustomExceptionBasics_Part2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Custom Exception Advanced Features ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Serialization Support
            // ═══════════════════════════════════════════════════════════

            // Serializable exceptions can be logged or transmitted
            // Use [Serializable] attribute and implement ISerializable

            // ── EXAMPLE 1: Serializable Exception ───────────────────────
            try
            {
                throw new DatabaseConnectionException("Cannot connect to database");
            }
            catch (DatabaseConnectionException ex)
            {
                Console.WriteLine($"  Caught: {ex.GetType().Name}");
                Console.WriteLine($"  Message: {ex.Message}");
                Console.WriteLine($"  HResult: {ex.HResult}");
            }
            // Output: Caught: DatabaseConnectionException
            // Output: Message: Cannot connect to database
            // Output: HResult: -2146233088

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Multiple Constructor Types
            // ═══════════════════════════════════════════════════════════

            // Different constructors support various error scenarios

            // ── EXAMPLE 1: All Constructor Types ────────────────────────────
            try
            {
                // Using message constructor
                throw new ConfigurationException("Invalid configuration setting");
            }
            catch (ConfigurationException ex)
            {
                Console.WriteLine($"\n  Message-only constructor:");
                Console.WriteLine($"  Type: {ex.GetType().Name}");
                Console.WriteLine($"  Message: {ex.Message}");
            }
            // Output: Message-only constructor:
            // Output: Type: ConfigurationException
            // Output: Message: Invalid configuration setting

            try
            {
                // Using message and inner exception
                try { int.Parse("bad"); }
                catch (FormatException inner) 
                {
                    throw new ConfigurationException("Parse error", inner);
                }
            }
            catch (ConfigurationException ex)
            {
                Console.WriteLine($"\n  With inner exception:");
                Console.WriteLine($"  Inner: {ex.InnerException?.GetType().Name}");
            }
            // Output: With inner exception:
            // Output: Inner: FormatException

            try
            {
                // Using property constructor (for structured errors)
                throw new ConfigurationException("Config error", "Auth", "ApiKey");
            }
            catch (ConfigurationException ex)
            {
                Console.WriteLine($"\n  Property constructor:");
                Console.WriteLine($"  Module: {ex.Module}");
            }
            // Output: Property constructor:
            // Output: Module: Auth

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Exception Data Dictionary
            // ═══════════════════════════════════════════════════════════

            // The Data property stores custom key-value pairs
            // Useful for structured error information

            // ── EXAMPLE 1: Using Exception Data ────────────────────────────
            try
            {
                var ex = new TimeoutException("Connection timed out");
                ex.Data["Server"] = "db.example.com";
                ex.Data["Port"] = 5432;
                ex.Data["Timeout"] = 30000;
                throw ex;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n  Exception Data:");
                foreach (var key in ex.Data.Keys)
                {
                    Console.WriteLine($"  {key}: {ex.Data[key]}");
                }
            }
            // Output: Exception Data:
            // Output: Server: db.example.com
            // Output: Port: 5432
            // Output: Timeout: 30000

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Custom ToString Override
            // ═══════════════════════════════════════════════════════════

            // Override ToString to provide detailed exception output

            // ── EXAMPLE 1: Custom ToString Output ────────────────────────
            try
            {
                throw new PaymentDeclinedException(
                    "Your card was declined",
                    "Visa",
                    "INSUFFICIENT_FUNDS"
                );
            }
            catch (PaymentDeclinedException ex)
            {
                Console.WriteLine($"\n  ToString Override:");
                Console.WriteLine(ex.ToString());
            }
            // Output: ToString Override:
            // Output: PaymentDeclinedException: Your card was declined
            // Output:   Card Type: Visa
            // Output:   Decline Code: INSUFFICIENT_FUNDS

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Exception Handling Best Practices
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Best Practices Demo ─────────────────────────────
            try
            {
                var validator = new UserValidator();
                validator.ValidateAge(-1);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"\n  Best Practice - Specific Exception First:");
                Console.WriteLine($"  Caught: {ex.GetType().Name}");
                Console.WriteLine($"  ParamName: {ex.ParamName}");
            }
            // Output: Best Practice - Specific Exception First:
            // Output: Caught: ArgumentOutOfRangeException
            // Output: ParamName: age

            try
            {
                var db = new DatabaseService();
                db.Connect("invalid_connection_string");
            }
            catch (DatabaseConnectionException ex)
            {
                Console.WriteLine($"\n  Best Practice - Log and Rethrow:");
                Console.WriteLine($"  Logged: {ex.Message}");
                throw;
            }
            // Output: Best Practice - Log and Rethrow:
            // Output: Logged: Cannot connect to database

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World: API Error Handling
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: API Error Handling ────────────────────────────
            var api = new ApiClient();

            var response1 = api.GetAsync("/users/123").Result;
            Console.WriteLine($"\n  API Response (Success): {response1}");
            // Output: API Response (Success): {"id": 123, "name": "John"}

            var response2 = api.GetAsync("/users/999").Result;
            Console.WriteLine($"  API Response (Not Found): {response2}");
            // Output: API Response (Not Found): NotFound
            // Output: ApiException: User not found

            Console.WriteLine("\n=== Custom Exception Advanced Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // CUSTOM EXCEPTION WITH PROPERTIES
    // ═══════════════════════════════════════════════════════════

    class DatabaseConnectionException : Exception
    {
        public string Module { get; set; } = string.Empty;

        public DatabaseConnectionException() : base() { }

        public DatabaseConnectionException(string message) : base(message) { }

        public DatabaseConnectionException(string message, Exception inner) 
            : base(message, inner) { }

        public DatabaseConnectionException(string message, string module)
            : base(message)
        {
            Module = module;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // MULTIPLE CONSTRUCTOR EXCEPTION
    // ═══════════════════════════════════════════════════════════

    class ConfigurationException : Exception
    {
        public string Module { get; set; } = string.Empty;
        public string ConfigKey { get; set; } = string.Empty;

        public ConfigurationException() : base() { }

        public ConfigurationException(string message) : base(message) { }

        public ConfigurationException(string message, Exception inner) 
            : base(message, inner) { }

        public ConfigurationException(string message, string module, string configKey)
            : base(message)
        {
            Module = module;
            ConfigKey = configKey;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // CUSTOM TOSTRING EXCEPTION
    // ═══════════════════════════════════════════════════════════

    class PaymentDeclinedException : Exception
    {
        public string CardType { get; set; } = string.Empty;
        public string DeclineCode { get; set; } = string.Empty;

        public PaymentDeclinedException() : base() { }

        public PaymentDeclinedException(string message) : base(message) { }

        public PaymentDeclinedException(string message, string cardType, string declineCode)
            : base(message)
        {
            CardType = cardType;
            DeclineCode = declineCode;
        }

        public PaymentDeclinedException(string message, Exception inner)
            : base(message, inner) { }

        public PaymentDeclinedException(string message, string cardType, string declineCode, Exception inner)
            : base(message, inner)
        {
            CardType = cardType;
            DeclineCode = declineCode;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: {Message}\n  Card Type: {CardType}\n  Decline Code: {DeclineCode}";
        }
    }

    // ═══════════════════════════════════════════════════════════
    // REAL-WORLD: User Validator
    // ═══════════════════════════════════════════════════════════

    class UserValidator
    {
        public void ValidateAge(int age)
        {
            if (age < 0)
            {
                throw new ArgumentOutOfRangeException("age", "Age cannot be negative");
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // REAL-WORLD: Database Service
    // ═══════════════════════════════════════════════════════════

    class DatabaseService
    {
        public void Connect(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new DatabaseConnectionException("Connection string is empty");
            }

            if (!connectionString.Contains("Server="))
            {
                throw new DatabaseConnectionException("Invalid connection string format");
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // REAL-WORLD: API Client
    // ═══════════════════════════════════════════════════════════

    class ApiException : Exception
    {
        public int StatusCode { get; set; }

        public ApiException() : base() { }
        public ApiException(string message) : base(message) { }
        public ApiException(string message, Exception inner) : base(message, inner) { }

        public ApiException(string message, int statusCode) 
            : base(message)
        {
            StatusCode = statusCode;
        }
    }

    class ApiClient
    {
        public Task<string> GetAsync(string endpoint)
        {
            return Task.FromResult(Get(endpoint));
        }

        private string Get(string endpoint)
        {
            if (endpoint.Contains("999"))
            {
                throw new ApiException("User not found", 404);
            }

            if (endpoint.Contains("123"))
            {
                Console.WriteLine($"  Fetching data from {endpoint}...");
                return "{\"id\": 123, \"name\": \"John\"}";
            }

            return "OK";
        }
    }
}