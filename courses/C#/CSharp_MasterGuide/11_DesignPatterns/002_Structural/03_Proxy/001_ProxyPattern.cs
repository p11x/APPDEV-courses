/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Proxy Pattern
 * FILE      : 01_ProxyPattern.cs
 * PURPOSE   : Demonstrates Proxy design pattern in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural._03_Proxy
{
    /// <summary>
    /// Demonstrates Proxy pattern
    /// </summary>
    public class ProxyPattern
    {
        /// <summary>
        /// Entry point for Proxy pattern examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Proxy Pattern ===
            Console.WriteLine("=== Proxy Pattern ===\n");

            // ── CONCEPT: What is Proxy? ───────────────────────────────────────
            // Provides a surrogate for another object

            // Example 1: Basic Proxy
            // Output: 1. Basic Proxy:
            Console.WriteLine("1. Basic Proxy:");
            
            // Client uses proxy instead of real object
            IImage imageProxy = new ImageProxy("photo.jpg");
            
            // Display triggers loading (lazy)
            imageProxy.Display();
            // Output: Loading: photo.jpg
            // Output: Displaying: photo.jpg

            // ── CONCEPT: Protection Proxy ─────────────────────────────────────
            // Controls access to real object

            // Example 2: Protection Proxy
            // Output: 2. Protection Proxy:
            Console.WriteLine("\n2. Protection Proxy:");
            
            // Proxy checks permissions before allowing access
            var protectedDocument = new ProtectedDocumentProxy("secret.txt", "admin");
            
            // Admin can access
            protectedDocument.Read();
            // Output: Admin reading: secret.txt
            
            // Different user would be denied

            // ── CONCEPT: Virtual Proxy ───────────────────────────────────────
            // Delays expensive object creation

            // Example 3: Virtual Proxy
            // Output: 3. Virtual Proxy:
            Console.WriteLine("\n3. Virtual Proxy:");
            
            // Heavy object not created until needed
            var heavyService = new HeavyServiceProxy();
            
            // First call creates object
            var result1 = heavyService.Process("data1");
            // Output: Creating heavy object...
            // Output: Processing: data1
            
            // Subsequent calls use existing object
            var result2 = heavyService.Process("data2");
            // Output: Processing: data2

            // ── REAL-WORLD EXAMPLE: Caching Proxy ───────────────────────────
            // Output: --- Real-World: Caching Proxy ---
            Console.WriteLine("\n--- Real-World: Caching Proxy ---");
            
            // Caching service proxy
            var weatherService = new WeatherServiceProxy();
            
            // First call - fetches from API
            var temp1 = weatherService.GetTemperature("New York");
            // Output: Fetching from API: New York, 72°F
            
            // Second call - returns cached
            var temp2 = weatherService.GetTemperature("New York");
            // Output: Returning cached: New York, 72°F

            Console.WriteLine("\n=== Proxy Pattern Complete ===");
        }
    }

    /// <summary>
    /// Image interface
    /// </summary>
    public interface IImage
    {
        void Display(); // method: displays image
    }

    /// <summary>
    /// Real image (expensive to create)
    /// </summary>
    public class RealImage : IImage
    {
        private string _filename;
        
        public RealImage(string filename)
        {
            _filename = filename;
            LoadFromDisk(); // expensive operation
        }
        
        private void LoadFromDisk()
        {
            Console.WriteLine($"   Loading: {_filename}");
        }
        
        public void Display()
        {
            Console.WriteLine($"   Displaying: {_filename}");
        }
    }

    /// <summary>
    /// Virtual proxy - delays real image creation
    /// </summary>
    public class ImageProxy : IImage
    {
        private string _filename;
        private RealImage _realImage;
        
        public ImageProxy(string filename)
        {
            _filename = filename;
        }
        
        public void Display()
        {
            // Create real image only when needed
            if (_realImage == null)
            {
                _realImage = new RealImage(_filename);
            }
            _realImage.Display();
        }
    }

    /// <summary>
    /// Document interface
    /// </summary>
    public interface IDocument
    {
        void Read(); // method: reads document
    }

    /// <summary>
    /// Real document
    /// </summary>
    public class RealDocument : IDocument
    {
        private string _filename;
        
        public RealDocument(string filename)
        {
            _filename = filename;
        }
        
        public void Read()
        {
            Console.WriteLine($"   Reading: {_filename}");
        }
    }

    /// <summary>
    /// Protection proxy - controls access
    /// </summary>
    public class ProtectedDocumentProxy : IDocument
    {
        private string _filename;
        private string _userRole;
        private RealDocument _realDocument;
        
        public ProtectedDocumentProxy(string filename, string userRole)
        {
            _filename = filename;
            _userRole = userRole;
        }
        
        public void Read()
        {
            // Check permissions
            if (_userRole == "admin")
            {
                // Create real document only if authorized
                if (_realDocument == null)
                {
                    _realDocument = new RealDocument(_filename);
                }
                Console.WriteLine($"   {_userRole} reading: {_filename}");
                _realDocument.Read();
            }
            else
            {
                Console.WriteLine($"   Access denied for {_userRole}");
            }
        }
    }

    /// <summary>
    /// Heavy service interface
    /// </summary>
    public interface IHeavyService
    {
        string Process(string data); // method: processes data
    }

    /// <summary>
    /// Real heavy service (expensive to create)
    /// </summary>
    public class RealHeavyService : IHeavyService
    {
        public RealHeavyService()
        {
            Console.WriteLine("   Creating heavy object...");
        }
        
        public string Process(string data)
        {
            return $"Processed: {data}";
        }
    }

    /// <summary>
    /// Virtual proxy for heavy service
    /// </summary>
    public class HeavyServiceProxy : IHeavyService
    {
        private RealHeavyService _realService;
        
        public string Process(string data)
        {
            // Create real service on first use
            if (_realService == null)
            {
                _realService = new RealHeavyService();
            }
            Console.WriteLine($"   Processing: {data}");
            return _realService.Process(data);
        }
    }

    /// <summary>
    /// Weather service interface
    /// </summary>
    public interface IWeatherService
    {
        double GetTemperature(string city); // method: gets temperature
    }

    /// <summary>
    /// Real weather service (calls external API)
    /// </summary>
    public class RealWeatherService : IWeatherService
    {
        public double GetTemperature(string city)
        {
            Console.WriteLine($"   Fetching from API: {city}");
            return 72.0; // simulated
        }
    }

    /// <summary>
    /// Caching proxy for weather service
    /// </summary>
    public class WeatherServiceProxy : IWeatherService
    {
        private RealWeatherService _realService;
        private Dictionary<string, double> _cache = new Dictionary<string, double>();
        
        public double GetTemperature(string city)
        {
            // Return cached if available
            if (_cache.ContainsKey(city))
            {
                Console.WriteLine($"   Returning cached: {city}, {_cache[city]}°F");
                return _cache[city];
            }
            
            // Fetch from real service
            if (_realService == null)
            {
                _realService = new RealWeatherService();
            }
            
            var temp = _realService.GetTemperature(city);
            _cache[city] = temp;
            
            Console.WriteLine($"   {city}, {temp}°F");
            return temp;
        }
    }
}