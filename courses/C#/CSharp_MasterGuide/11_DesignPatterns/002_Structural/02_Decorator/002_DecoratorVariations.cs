/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Decorator Variations
 * FILE      : 02_DecoratorVariations.cs
 * PURPOSE   : Demonstrates different Decorator pattern approaches
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural._02_Decorator
{
    /// <summary>
    /// Demonstrates Decorator variations
    /// </summary>
    public class DecoratorVariations
    {
        /// <summary>
        /// Entry point for Decorator variations
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Decorator Variations ===
            Console.WriteLine("=== Decorator Variations ===\n");

            // ── CONCEPT: Constructor Injection ────────────────────────────────
            // Pass component through constructor

            // Example 1: Constructor Injection
            // Output: 1. Constructor Injection:
            Console.WriteLine("1. Constructor Injection:");
            
            // Inject component in constructor
            var logger = new TimestampDecorator(
                new LogLevelDecorator(
                    new FileLogger()));
            
            logger.Log("Application started");
            // Output: [2024-01-01 10:00:00] [INFO] Application started

            // ── CONCEPT: Property Injection ───────────────────────────────────
            // Set component through property

            // Example 2: Property Injection
            // Output: 2. Property Injection:
            Console.WriteLine("\n2. Property Injection:");
            
            // Set wrapped component via property
            var cache = new CacheDecorator();
            cache.WrappedCache = new MemoryCache();
            
            cache.Set("key1", "value1");
            var value = cache.Get("key1");
            // Output: Cache: key1 = value1

            // ── CONCEPT: Factory-Based Decoration ───────────────────────────
            // Use factory to create decorated objects

            // Example 3: Factory-Based
            // Output: 3. Factory-Based Decoration:
            Console.WriteLine("\n3. Factory-Based Decoration:");
            
            // Factory creates with requested features
            var basicPizza = PizzaFactory.CreatePizza("Basic");
            var deluxePizza = PizzaFactory.CreatePizza("Deluxe");
            var supremePizza = PizzaFactory.CreatePizza("Supreme");
            
            // Output: Basic: Basic Pizza, $8.00
            // Output: Deluxe: Basic Pizza, Cheese, Pepperoni, $12.00
            // Output: Supreme: Basic Pizza, Cheese, Pepperoni, Mushrooms, Olives, $15.00

            // ── REAL-WORLD EXAMPLE: Middleware Pipeline ──────────────────────
            // Output: --- Real-World: Middleware Pipeline ---
            Console.WriteLine("\n--- Real-World: Middleware Pipeline ---");
            
            // Build middleware chain
            var middleware = new LoggingMiddleware(
                new AuthenticationMiddleware(
                    new AuthorizationMiddleware(
                        new CacheMiddleware(null))));
            
            // Process request through chain
            var request = new HttpRequest { Path = "/api/users", User = "admin" };
            middleware.Process(request);
            // Output: Request processed: /api/users by admin

            Console.WriteLine("\n=== Decorator Variations Complete ===");
        }
    }

    /// <summary>
    /// Logger interface
    /// </summary>
    public interface ILogger
    {
        void Log(string message); // method: logs message
    }

    /// <summary>
    /// File logger - base component
    /// </summary>
    public class FileLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"   {message}");
        }
    }

    /// <summary>
    /// Timestamp decorator
    /// </summary>
    public class TimestampDecorator : ILogger
    {
        private ILogger _logger;
        
        public TimestampDecorator(ILogger logger)
        {
            _logger = logger;
        }
        
        public void Log(string message)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _logger.Log($"[{timestamp}] {message}");
        }
    }

    /// <summary>
    /// Log level decorator
    /// </summary>
    public class LogLevelDecorator : ILogger
    {
        private ILogger _logger;
        
        public LogLevelDecorator(ILogger logger)
        {
            _logger = logger;
        }
        
        public void Log(string message)
        {
            _logger.Log($"[INFO] {message}");
        }
    }

    /// <summary>
    /// Cache interface
    /// </summary>
    public interface ICache
    {
        void Set(string key, string value); // method: stores value
        string Get(string key); // method: retrieves value
    }

    /// <summary>
    /// Memory cache - base component
    /// </summary>
    public class MemoryCache : ICache
    {
        public void Set(string key, string value)
        {
            Console.WriteLine($"   Stored in memory: {key} = {value}");
        }
        
        public string Get(string key)
        {
            Console.WriteLine($"   Retrieved from memory: {key}");
            return "value1";
        }
    }

    /// <summary>
    /// Cache decorator with property injection
    /// </summary>
    public class CacheDecorator : ICache
    {
        public ICache WrappedCache { get; set; } // property: wrapped cache
        
        public void Set(string key, string value)
        {
            WrappedCache?.Set(key, value);
        }
        
        public string Get(string key)
        {
            Console.Write($"   Cache: ");
            return WrappedCache?.Get(key);
        }
    }

    /// <summary>
    /// Pizza interface
    /// </summary>
    public interface IPizza
    {
        string GetDescription(); // method: returns description
        decimal GetPrice(); // method: returns price
    }

    /// <summary>
    /// Basic pizza
    /// </summary>
    public class BasicPizza : IPizza
    {
        public string GetDescription() => "Basic Pizza";
        public decimal GetPrice() => 8.00m;
    }

    /// <summary>
    /// Cheese decorator
    /// </summary>
    public class CheeseDecorator : IPizza
    {
        private IPizza _pizza;
        
        public CheeseDecorator(IPizza pizza)
        {
            _pizza = pizza;
        }
        
        public string GetDescription() => _pizza.GetDescription() + ", Cheese";
        public decimal GetPrice() => _pizza.GetPrice() + 1.50m;
    }

    /// <summary>
    /// Pepperoni decorator
    /// </summary>
    public class PepperoniDecorator : IPizza
    {
        private IPizza _pizza;
        
        public PepperoniDecorator(IPizza pizza)
        {
            _pizza = pizza;
        }
        
        public string GetDescription() => _pizza.GetDescription() + ", Pepperoni";
        public decimal GetPrice() => _pizza.GetPrice() + 2.00m;
    }

    /// <summary>
    /// Mushrooms decorator
    /// </summary>
    public class MushroomsDecorator : IPizza
    {
        private IPizza _pizza;
        
        public MushroomsDecorator(IPizza pizza)
        {
            _pizza = pizza;
        }
        
        public string GetDescription() => _pizza.GetDescription() + ", Mushrooms";
        public decimal GetPrice() => _pizza.GetPrice() + 1.00m;
    }

    /// <summary>
    /// Olives decorator
    /// </summary>
    public class OlivesDecorator : IPizza
    {
        private IPizza _pizza;
        
        public OlivesDecorator(IPizza pizza)
        {
            _pizza = pizza;
        }
        
        public string GetDescription() => _pizza.GetDescription() + ", Olives";
        public decimal GetPrice() => _pizza.GetPrice() + 1.00m;
    }

    /// <summary>
    /// Pizza factory for decoration
    /// </summary>
    public static class PizzaFactory
    {
        /// <summary>
        /// Creates pizza with decorations
        /// </summary>
        public static IPizza CreatePizza(string type)
        {
            var pizza = new BasicPizza();
            
            return type switch
            {
                "Basic" => pizza,
                "Deluxe" => new OlivesDecorator(new PepperoniDecorator(new CheeseDecorator(pizza))),
                "Supreme" => new OlivesDecorator(new MushroomsDecorator(new PepperoniDecorator(new CheeseDecorator(pizza)))),
                _ => pizza
            };
        }
    }

    /// <summary>
    /// HTTP request
    /// </summary>
    public class HttpRequest
    {
        public string Path { get; set; } // property: request path
        public string User { get; set; } // property: user
    }

    /// <summary>
    /// Middleware interface
    /// </summary>
    public interface IMiddleware
    {
        void Process(HttpRequest request); // method: processes request
    }

    /// <summary>
    /// Logging middleware
    /// </summary>
    public class LoggingMiddleware : IMiddleware
    {
        private IMiddleware _next;
        
        public LoggingMiddleware(IMiddleware next)
        {
            _next = next;
        }
        
        public void Process(HttpRequest request)
        {
            Console.WriteLine($"   Logging: Request to {request.Path}");
            _next?.Process(request);
        }
    }

    /// <summary>
    /// Authentication middleware
    /// </summary>
    public class AuthenticationMiddleware : IMiddleware
    {
        private IMiddleware _next;
        
        public AuthenticationMiddleware(IMiddleware next)
        {
            _next = next;
        }
        
        public void Process(HttpRequest request)
        {
            Console.WriteLine($"   Authentication: User {request.User} authenticated");
            _next?.Process(request);
        }
    }

    /// <summary>
    /// Authorization middleware
    /// </summary>
    public class AuthorizationMiddleware : IMiddleware
    {
        private IMiddleware _next;
        
        public AuthorizationMiddleware(IMiddleware next)
        {
            _next = next;
        }
        
        public void Process(HttpRequest request)
        {
            Console.WriteLine($"   Authorization: User {request.User} authorized");
            _next?.Process(request);
        }
    }

    /// <summary>
    /// Cache middleware
    /// </summary>
    public class CacheMiddleware : IMiddleware
    {
        private IMiddleware _next;
        
        public CacheMiddleware(IMiddleware next)
        {
            _next = next;
        }
        
        public void Process(HttpRequest request)
        {
            Console.WriteLine($"   Cache: Checking cache for {request.Path}");
        }
    }
}