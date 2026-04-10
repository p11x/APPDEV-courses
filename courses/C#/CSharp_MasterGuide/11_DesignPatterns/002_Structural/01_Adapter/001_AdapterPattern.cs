/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Adapter Pattern
 * FILE      : 01_AdapterPattern.cs
 * PURPOSE   : Demonstrates Adapter design pattern in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural._01_Adapter
{
    /// <summary>
    /// Demonstrates Adapter pattern
    /// </summary>
    public class AdapterPattern
    {
        /// <summary>
        /// Entry point for Adapter pattern examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Adapter Pattern ===
            Console.WriteLine("=== Adapter Pattern ===\n");

            // ── CONCEPT: What is Adapter? ──────────────────────────────────────
            // Converts interface of a class to another interface

            // Example 1: Basic Adapter
            // Output: 1. Basic Adapter:
            Console.WriteLine("1. Basic Adapter:");
            
            // Client expects ITarget interface
            ITarget target = new Adapter(new Adaptee());
            
            // Request() is converted to SpecificRequest()
            var result = target.Request();
            // Output: Adapter converted: SpecificRequest result
            Console.WriteLine($"   Adapter converted: {result}");

            // ── CONCEPT: Object Adapter ───────────────────────────────────────
            // Uses composition to adapt interface

            // Example 2: Object Adapter
            // Output: 2. Object Adapter:
            Console.WriteLine("\n2. Object Adapter:");
            
            // Adapter wraps Adaptee object
            var objectAdapter = new ObjectAdapter(new LegacyPaymentProcessor());
            
            // ProcessPayment maps to legacy ProcessCreditCard
            objectAdapter.ProcessPayment(100.00m);
            // Output: Processed payment: $100.00

            // ── CONCEPT: Class Adapter ───────────────────────────────────────
            // Uses inheritance (C# limitation: multiple inheritance not supported)

            // Example 3: Class Adapter (via composition)
            // Output: 3. Class Adapter:
            Console.WriteLine("\n3. Class Adapter:");
            
            // Library adapter wraps third-party library
            var libraryAdapter = new ThirdPartyLibraryAdapter();
            
            // Convert our interface to library format
            var userData = new UserData { Name = "John", Email = "john@email.com" };
            libraryAdapter.SaveUser(userData);
            // Output: Saved to third-party: John (john@email.com)

            // ── REAL-WORLD EXAMPLE: Data Adapter ─────────────────────────────
            // Output: --- Real-World: Data Adapter ---
            Console.WriteLine("\n--- Real-World: Data Adapter ---");
            
            // Old system uses different interface
            var legacyDatabase = new LegacyDatabaseAdapter();
            
            // Connect works the same way despite different legacy interface
            legacyDatabase.Connect("Server=localhost;Database=MyDB");
            // Output: Connected to legacy database
            
            // Query works with new interface
            var results = legacyDatabase.Query("SELECT * FROM Users");
            // Output: Query returned: 3 records

            Console.WriteLine("\n=== Adapter Pattern Complete ===");
        }
    }

    /// <summary>
    /// Target interface - what client expects
    /// </summary>
    public interface ITarget
    {
        string Request(); // method: returns processed result
    }

    /// <summary>
    /// Adaptee - existing incompatible interface
    /// </summary>
    public class Adaptee
    {
        /// <summary>
        /// Specific method that needs adapting
        /// </summary>
        public string SpecificRequest()
        {
            return "SpecificRequest result";
        }
    }

    /// <summary>
    /// Adapter - converts Adaptee to ITarget
    /// </summary>
    public class Adapter : ITarget
    {
        private Adaptee _adaptee; // wraps adaptee
        
        public Adapter(Adaptee adaptee)
        {
            _adaptee = adaptee;
        }
        
        /// <summary>
        /// Adapts SpecificRequest to Request
        /// </summary>
        public string Request()
        {
            // Convert interface here
            return "Adapter converted: " + _adaptee.SpecificRequest();
        }
    }

    /// <summary>
    /// Legacy payment processor
    /// </summary>
    public class LegacyPaymentProcessor
    {
        /// <summary>
        /// Legacy method with different signature
        /// </summary>
        public void ProcessCreditCard(decimal amount, string cardNumber)
        {
            Console.WriteLine($"   Processed payment: ${amount}");
        }
    }

    /// <summary>
    /// Target for payments
    /// </summary>
    public interface IPaymentGateway
    {
        void ProcessPayment(decimal amount); // method: processes payment
    }

    /// <summary>
    /// Object adapter for payments
    /// </summary>
    public class ObjectAdapter : IPaymentGateway
    {
        private LegacyPaymentProcessor _legacy; // wraps legacy processor
        
        public ObjectAdapter(LegacyPaymentProcessor legacy)
        {
            _legacy = legacy;
        }
        
        /// <summary>
        /// Adapts ProcessPayment to ProcessCreditCard
        /// </summary>
        public void ProcessPayment(decimal amount)
        {
            // Convert call to legacy format
            _legacy.ProcessCreditCard(amount, "****-****-****-1234");
        }
    }

    /// <summary>
    /// Third-party library interface
    /// </summary>
    public interface IThirdPartyLibrary
    {
        void Save(string jsonData); // method: saves data as JSON
    }

    /// <summary>
    /// Third-party library implementation
    /// </summary>
    public class ThirdPartyLibrary : IThirdPartyLibrary
    {
        public void Save(string jsonData)
        {
            Console.WriteLine($"   Saved to third-party: {jsonData}");
        }
    }

    /// <summary>
    /// Our user data
    /// </summary>
    public class UserData
    {
        public string Name { get; set; } // property: user's name
        public string Email { get; set; } // property: user's email
    }

    /// <summary>
    /// Adapter for third-party library
    /// </summary>
    public class ThirdPartyLibraryAdapter
    {
        private IThirdPartyLibrary _library; // wraps third-party
        
        public ThirdPartyLibraryAdapter()
        {
            _library = new ThirdPartyLibrary();
        }
        
        /// <summary>
        /// Adapts our UserData to library's JSON
        /// </summary>
        public void SaveUser(UserData user)
        {
            // Convert our data to JSON format
            var json = $"{user.Name} ({user.Email})";
            _library.Save(json);
        }
    }

    /// <summary>
    /// Legacy database interface
    /// </summary>
    public class LegacyDatabase
    {
        /// <summary>
        /// Legacy connect method
        /// </summary>
        public void ConnectToDatabase(string connectionString)
        {
            Console.WriteLine($"   Connected to legacy database");
        }
        
        /// <summary>
        /// Legacy execute method
        /// </summary>
        public string ExecuteQuery(string sql)
        {
            return "3 records"; // simulated result
        }
    }

    /// <summary>
    /// Modern database interface
    /// </summary>
    public interface IModernDatabase
    {
        void Connect(string connectionString); // method: connects to database
        List<string> Query(string sql); // method: executes query
    }

    /// <summary>
    /// Adapter for legacy database
    /// </summary>
    public class LegacyDatabaseAdapter : IModernDatabase
    {
        private LegacyDatabase _legacy; // wraps legacy database
        
        public LegacyDatabaseAdapter()
        {
            _legacy = new LegacyDatabase();
        }
        
        /// <summary>
        /// Adapts ConnectToDatabase to Connect
        /// </summary>
        public void Connect(string connectionString)
        {
            _legacy.ConnectToDatabase(connectionString);
        }
        
        /// <summary>
        /// Adapts ExecuteQuery to Query
        /// </summary>
        public List<string> Query(string sql)
        {
            var result = _legacy.ExecuteQuery(sql);
            // Convert to modern format
            return new List<string> { result };
        }
    }
}