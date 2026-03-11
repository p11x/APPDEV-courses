/*
================================================================================
TOPIC 38: CONFIGURATION AND LOGGING
================================================================================

ASP.NET Core provides built-in configuration and logging systems.

TABLE OF CONTENTS:
1. Configuration
2. Options Pattern
3. Logging
4. Environments
================================================================================
*/

namespace ConfigLoggingConcepts
{
    // ====================================================================
    // APPSETTINGS.JSON
    // ====================================================================
    
    /*
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=.;Database=MyDb;Trusted_Connection=True"
      },
      "AppSettings": {
        "SiteName": "My Website",
        "MaxItems": 100
      },
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft": "Warning"
        }
      }
    }
    */
    
    // ====================================================================
    // USING CONFIGURATION
    // ====================================================================
    
    // Example: Program.cs
    /*
    var builder = WebApplication.CreateBuilder(args);
    
    // Read configuration
    var connectionString = builder.Configuration
        .GetConnectionString("DefaultConnection");
    var siteName = builder.Configuration
        ["AppSettings:SiteName"];
    
    // Use options pattern
    builder.Services.Configure<AppSettings>(
        builder.Configuration.GetSection("AppSettings"));
    */
    
    // Example: IOptions
    /*
    public class HomeController : Controller
    {
        private readonly AppSettings _settings;
        
        public HomeController(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }
    }
    
    public class AppSettings
    {
        public string SiteName { get; set; }
        public int MaxItems { get; set; }
    }
    */
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Configuration ===");
            
            Console.WriteLine("\nConfiguration Sources:");
            Console.WriteLine("- appsettings.json");
            Console.WriteLine("- appsettings.Development.json");
            Console.WriteLine("- Environment variables");
            Console.WriteLine("- Command-line arguments");
            Console.WriteLine("- Secrets (UserSecrets)");
            
            Console.WriteLine("\n=== Logging ===");
            
            Console.WriteLine("\nLog Levels:");
            Console.WriteLine("Trace   - Detailed diagnostics");
            Console.WriteLine("Debug   - Development info");
            Console.WriteLine("Information - General info");
            Console.WriteLine("Warning - Issues");
            Console.WriteLine("Error   - Errors");
            Console.WriteLine("Critical - Fatal issues");
            
            // Example logging:
            // _logger.LogInformation("User {UserId} logged in", userId);
            // _logger.LogWarning("Low memory: {Percent}%", percent);
        }
    }
}

/*
CONFIGURATION:
--------------
- Hierarchical configuration
- Strongly typed options
- Environment-specific settings
- Secrets for sensitive data

LOGGING:
--------
- ILogger<T> for typed loggers
- Serilog for file logging
- Application Insights for monitoring
*/

// ================================================================================
// INTERVIEW QUESTIONS
// =============================================================================

/*
Q: What is the Options pattern in ASP.NET Core?
A: A way to strongly-type configuration and inject it via DI.
   Uses IOptions<T> or IOptionsSnapshot<T>.
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 39 covers Authentication and Authorization.
*/
