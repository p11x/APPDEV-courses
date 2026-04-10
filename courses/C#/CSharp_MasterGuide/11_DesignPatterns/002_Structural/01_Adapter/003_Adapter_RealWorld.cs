/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Adapter Real-World
 * FILE      : 03_Adapter_RealWorld.cs
 * PURPOSE   : Real-world Adapter pattern applications
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural._01_Adapter
{
    /// <summary>
    /// Real-world Adapter pattern examples
    /// </summary>
    public class AdapterRealWorld
    {
        /// <summary>
        /// Entry point for real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Adapter Real-World ===
            Console.WriteLine("=== Adapter Real-World ===\n");

            // ── REAL-WORLD 1: Legacy System Integration ───────────────────────
            // Integrate old inventory system with new e-commerce

            // Example 1: Legacy Inventory Adapter
            // Output: 1. Legacy Inventory Adapter:
            Console.WriteLine("1. Legacy Inventory Adapter:");
            
            // New system expects modern interface
            var inventoryService = new InventoryAdapter();
            
            // Check stock - adapter handles legacy conversion
            var stock = inventoryService.GetStock("SKU-123");
            // Output: Stock for SKU-123: 50 units
            Console.WriteLine($"   Stock for SKU-123: {stock} units");
            
            // Update stock
            inventoryService.UpdateStock("SKU-123", 45);
            // Output: Updated SKU-123 stock: 45 units
            Console.WriteLine($"   Updated SKU-123 stock: {inventoryService.GetStock("SKU-123")} units");

            // ── REAL-WORLD 2: External API Integration ───────────────────────
            // Different APIs unified through adapters

            // Example 2: Weather API Adapter
            // Output: 2. Weather API Adapter:
            Console.WriteLine("\n2. Weather API Adapter:");
            
            // Use weather adapter regardless of underlying API
            var weatherService = new WeatherServiceAdapter();
            
            var temperature = weatherService.GetTemperature("New York");
            var forecast = weatherService.GetForecast("New York", 5);
            
            // Output: New York: 72°F
            Console.WriteLine($"   New York: {temperature}°F");
            // Output: Forecast: 5 days
            Console.WriteLine($"   Forecast: {forecast} days");

            // ── REAL-WORLD 3: File Format Conversion ──────────────────────────
            // Convert between different file formats

            // Example 3: File Format Adapter
            // Output: 3. File Format Adapter:
            Console.WriteLine("\n3. File Format Adapter:");
            
            // Read any format with same interface
            var csvImporter = new FileFormatAdapter("CSV");
            var jsonImporter = new FileFormatAdapter("JSON");
            var xmlImporter = new FileFormatAdapter("XML");
            
            // All return same data structure
            var csvData = csvImporter.Import("data.csv");
            var jsonData = jsonImporter.Import("data.json");
            var xmlData = xmlImporter.Import("data.xml");
            
            // Output: CSV: 100 rows imported
            Console.WriteLine($"   CSV: {csvData} rows imported");
            // Output: JSON: 100 rows imported
            Console.WriteLine($"   JSON: {jsonData} rows imported");
            // Output: XML: 100 rows imported
            Console.WriteLine($"   XML: {xmlData} rows imported");

            // ── REAL-WORLD 4: Authentication Providers ────────────────────────
            // Different auth systems with unified interface

            // Example 4: Auth Provider Adapter
            // Output: 4. Auth Provider Adapter:
            Console.WriteLine("\n4. Auth Provider Adapter:");
            
            // Use any auth provider through adapter
            var googleAuth = new AuthProviderAdapter("Google");
            var facebookAuth = new AuthProviderAdapter("Facebook");
            var customAuth = new AuthProviderAdapter("Custom");
            
            // Same interface for all
            var googleUser = googleAuth.Authenticate("user@gmail.com");
            var facebookUser = facebookAuth.Authenticate("user@facebook.com");
            var customUser = customAuth.Authenticate("user@custom.com");
            
            // Output: Google authenticated: user@gmail.com
            Console.WriteLine($"   Google authenticated: {googleUser}");
            // Output: Facebook authenticated: user@facebook.com
            Console.WriteLine($"   Facebook authenticated: {facebookUser}");
            // Output: Custom authenticated: user@custom.com
            Console.WriteLine($"   Custom authenticated: {customUser}");

            // ── REAL-WORLD 5: Notification System ───────────────────────────
            // Different notification channels unified

            // Example 5: Notification Adapter
            // Output: 5. Notification Adapter:
            Console.WriteLine("\n5. Notification Adapter:");
            
            // Send via any channel
            var emailNotifier = new NotificationAdapter("Email");
            var smsNotifier = new NotificationAdapter("SMS");
            var pushNotifier = new NotificationAdapter("Push");
            
            // Same interface
            emailNotifier.Send("user@example.com", "Welcome!");
            smsNotifier.Send("+1234567890", "Your code: 1234");
            pushNotifier.Send("device123", "New message");
            
            // Output: Email sent to user@example.com: Welcome!
            // Output: SMS sent to +1234567890: Your code: 1234
            // Output: Push sent to device123: New message

            Console.WriteLine("\n=== Adapter Real-World Complete ===");
        }
    }

    /// <summary>
    /// Legacy inventory system (old interface)
    /// </summary>
    public class LegacyInventorySystem
    {
        /// <summary>
        /// Legacy method - different naming
        /// </summary>
        public int GetItemQuantity(string sku)
        {
            return 50; // simulated
        }
        
        /// <summary>
        /// Legacy method - different naming
        /// </summary>
        public void SetItemQuantity(string sku, int qty)
        {
            Console.WriteLine($"   Legacy inventory updated: {sku} = {qty}");
        }
    }

    /// <summary>
    /// Modern inventory interface
    /// </summary>
    public interface IInventoryService
    {
        int GetStock(string productId); // method: gets stock level
        void UpdateStock(string productId, int quantity); // method: updates stock
    }

    /// <summary>
    /// Adapter for legacy inventory
    /// </summary>
    public class InventoryAdapter : IInventoryService
    {
        private LegacyInventorySystem _legacy = new LegacyInventorySystem();
        
        public int GetStock(string productId)
        {
            // Map productId to SKU
            return _legacy.GetItemQuantity(productId);
        }
        
        public void UpdateStock(string productId, int quantity)
        {
            _legacy.SetItemQuantity(productId, quantity);
        }
    }

    /// <summary>
    /// Weather API interfaces (different providers)
    /// </summary>
    public interface IWeatherProvider
    {
        double GetTemp(string city); // method: gets temperature
    }

    public class OpenWeatherMap : IWeatherProvider
    {
        public double GetTemp(string city) => 72.0;
    }

    public class WeatherUnderground : IWeatherProvider
    {
        public double GetTemp(string city) => 71.5;
    }

    /// <summary>
    /// Weather service interface
    /// </summary>
    public interface IWeatherService
    {
        double GetTemperature(string city); // method: gets temperature
        int GetForecast(string city, int days); // method: gets forecast
    }

    /// <summary>
    /// Weather adapter
    /// </summary>
    public class WeatherServiceAdapter : IWeatherService
    {
        private IWeatherProvider _provider = new OpenWeatherMap();
        
        public double GetTemperature(string city)
        {
            return _provider.GetTemp(city);
        }
        
        public int GetForecast(string city, int days)
        {
            return days;
        }
    }

    /// <summary>
    /// File importer interface
    /// </summary>
    public interface IFileImporter
    {
        int Import(string filePath); // method: imports file
    }

    /// <summary>
    /// Format-specific importers
    /// </summary>
    public class CSVImporter : IFileImporter
    {
        public int Import(string filePath) => 100;
    }

    public class JSONImporter : IFileImporter
    {
        public int Import(string filePath) => 100;
    }

    public class XMLImporter : IFileImporter
    {
        public int Import(string filePath) => 100;
    }

    /// <summary>
    /// File format adapter
    /// </summary>
    public class FileFormatAdapter : IFileImporter
    {
        private IFileImporter _importer;
        
        public FileFormatAdapter(string format)
        {
            _importer = format switch
            {
                "CSV" => new CSVImporter(),
                "JSON" => new JSONImporter(),
                "XML" => new XMLImporter(),
                _ => throw new ArgumentException($"Unknown format: {format}")
            };
        }
        
        public int Import(string filePath)
        {
            return _importer.Import(filePath);
        }
    }

    /// <summary>
    /// Auth provider interface
    /// </summary>
    public interface IAuthProvider
    {
        string Auth(string userId); // method: authenticates user
    }

    public class GoogleAuth : IAuthProvider
    {
        public string Auth(string userId) => userId;
    }

    public class FacebookAuth : IAuthProvider
    {
        public string Auth(string userId) => userId;
    }

    public class CustomAuth : IAuthProvider
    {
        public string Auth(string userId) => userId;
    }

    /// <summary>
    /// Auth adapter
    /// </summary>
    public class AuthProviderAdapter
    {
        private IAuthProvider _provider;
        
        public AuthProviderAdapter(string providerName)
        {
            _provider = providerName switch
            {
                "Google" => new GoogleAuth(),
                "Facebook" => new FacebookAuth(),
                "Custom" => new CustomAuth(),
                _ => throw new ArgumentException($"Unknown provider: {providerName}")
            };
        }
        
        public string Authenticate(string userId)
        {
            return _provider.Auth(userId);
        }
    }

    /// <summary>
    /// Notifier interface
    /// </summary>
    public interface INotifier
    {
        void Notify(string target, string message); // method: sends notification
    }

    public class EmailService : INotifier
    {
        public void Notify(string target, string message)
        {
            Console.WriteLine($"   Email sent to {target}: {message}");
        }
    }

    public class SMSService : INotifier
    {
        public void Notify(string target, string message)
        {
            Console.WriteLine($"   SMS sent to {target}: {message}");
        }
    }

    public class PushService : INotifier
    {
        public void Notify(string target, string message)
        {
            Console.WriteLine($"   Push sent to {target}: {message}");
        }
    }

    /// <summary>
    /// Notification adapter
    /// </summary>
    public class NotificationAdapter
    {
        private INotifier _notifier;
        
        public NotificationAdapter(string channel)
        {
            _notifier = channel switch
            {
                "Email" => new EmailService(),
                "SMS" => new SMSService(),
                "Push" => new PushService(),
                _ => throw new ArgumentException($"Unknown channel: {channel}")
            };
        }
        
        public void Send(string target, string message)
        {
            _notifier.Notify(target, message);
        }
    }
}