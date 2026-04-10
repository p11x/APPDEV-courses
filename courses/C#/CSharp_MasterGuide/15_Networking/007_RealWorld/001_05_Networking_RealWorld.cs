/*
 * ============================================================
 * TOPIC     : Networking
 * SUBTOPIC  : Real-World Networking
 * FILE      : 05_Networking_RealWorld.cs
 * PURPOSE   : Real-world networking examples
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._15_Networking._05_RealWorld
{
    /// <summary>
    /// Real-world networking examples
    /// </summary>
    public class NetworkingRealWorldDemo
    {
        /// <summary>
        /// Entry point for networking real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Networking Real-World ===
            Console.WriteLine("=== Networking Real-World ===\n");

            // ── REAL-WORLD 1: Weather API Client ───────────────────────────────
            // External API integration

            // Example 1: Weather API
            // Output: 1. Weather API:
            Console.WriteLine("1. Weather API:");
            
            var weatherClient = new WeatherAPIClient();
            var weather = weatherClient.GetWeather("New York");
            // Output: Weather in New York: 72°F, Sunny
            Console.WriteLine($"   Weather in {weather.City}: {weather.Temperature}°F, {weather.Condition}");

            // ── REAL-WORLD 2: File Upload ─────────────────────────────────────
            // Multipart file upload

            // Example 2: File Upload
            // Output: 2. File Upload:
            Console.WriteLine("\n2. File Upload:");
            
            var uploadService = new FileUploadService();
            var uploadResult = uploadService.UploadFile("document.pdf", 1024);
            // Output: Uploaded: document.pdf (1024 KB)
            Console.WriteLine($"   Uploaded: {uploadResult.FileName} ({uploadResult.Size} KB)");

            // ── REAL-WORLD 3: Authentication ───────────────────────────────────
            // JWT token handling

            // Example 3: Authentication
            // Output: 3. Authentication:
            Console.WriteLine("\n3. Authentication:");
            
            var authService = new AuthService();
            var token = authService.Login("user@email.com", "password");
            // Output: Login successful, token: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
            Console.WriteLine($"   Login successful, token: {token.Substring(0, 20)}...");

            Console.WriteLine("\n=== Networking Real-World Complete ===");
        }
    }

    /// <summary>
    /// Weather data
    /// </summary>
    public class WeatherInfo
    {
        public string City { get; set; } // property: city name
        public int Temperature { get; set; } // property: temperature
        public string Condition { get; set; } // property: weather condition
    }

    /// <summary>
    /// Weather API client
    /// </summary>
    public class WeatherAPIClient
    {
        public WeatherInfo GetWeather(string city)
        {
            return new WeatherInfo { City = city, Temperature = 72, Condition = "Sunny" };
        }
    }

    /// <summary>
    /// Upload result
    /// </summary>
    public class UploadResult
    {
        public string FileName { get; set; } // property: uploaded file name
        public int Size { get; set; } // property: file size in KB
    }

    /// <summary>
    /// File upload service
    /// </summary>
    public class FileUploadService
    {
        public UploadResult UploadFile(string fileName, int sizeKB)
        {
            return new UploadResult { FileName = fileName, Size = sizeKB };
        }
    }

    /// <summary>
    /// Auth service
    /// </summary>
    public class AuthService
    {
        public string Login(string email, string password)
        {
            return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";
        }
    }
}