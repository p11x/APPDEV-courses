/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Factory Real-World
 * FILE      : 03_Factory_RealWorld.cs
 * PURPOSE   : Real-world Factory pattern applications
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational._02_Factory
{
    /// <summary>
    /// Real-world Factory pattern examples
    /// </summary>
    public class FactoryRealWorld
    {
        /// <summary>
        /// Entry point for real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Factory Real-World ===
            Console.WriteLine("=== Factory Real-World ===\n");

            // ── REAL-WORLD 1: Logger Factory ──────────────────────────────────
            // Create appropriate logger based on configuration

            // Example 1: Logger Factory
            // Output: 1. Logger Factory:
            Console.WriteLine("1. Logger Factory:");
            
            // Create loggers via factory
            var consoleLogger = LoggerFactory.CreateLogger("Console");
            var fileLogger = LoggerFactory.CreateLogger("File");
            var databaseLogger = LoggerFactory.CreateLogger("Database");
            
            // Log messages
            consoleLogger.Log("Info message"); // logs to console
            fileLogger.Log("Warning message"); // logs to file
            databaseLogger.Log("Error message"); // logs to database
            
            // Output: Console: Info message
            // Output: File: Warning message
            // Output: Database: Error message

            // ── REAL-WORLD 2: Notification Factory ────────────────────────────
            // Send notifications via different channels

            // Example 2: Notification Factory
            // Output: 2. Notification Factory:
            Console.WriteLine("\n2. Notification Factory:");
            
            // Create notification senders
            var emailNotifier = NotificationFactory.CreateNotifier("Email");
            var smsNotifier = NotificationFactory.CreateNotifier("SMS");
            var pushNotifier = NotificationFactory.CreateNotifier("Push");
            
            // Send notifications
            emailNotifier.Send("user@example.com", "Welcome!"); // sends email
            smsNotifier.Send("+1234567890", "Your code: 1234"); // sends SMS
            pushNotifier.Send("device123", "New message"); // sends push notification
            
            // Output: Email sent to user@example.com
            // Output: SMS sent to +1234567890
            // Output: Push sent to device123

            // ── REAL-WORLD 3: Cache Factory ───────────────────────────────────
            // Create different cache implementations

            // Example 3: Cache Factory
            // Output: 3. Cache Factory:
            Console.WriteLine("\n3. Cache Factory:");
            
            // Create caches based on storage type
            var memoryCache = CacheFactory.CreateCache("Memory");
            var redisCache = CacheFactory.CreateCache("Redis");
            var diskCache = CacheFactory.CreateCache("Disk");
            
            // Use caches
            memoryCache.Set("key1", "value1"); // stores in memory
            redisCache.Set("key2", "value2"); // stores in Redis
            diskCache.Set("key3", "value3"); // stores on disk
            
            // Output: Memory cache: key1=value1
            // Output: Redis cache: key2=value2
            // Output: Disk cache: key3=value3

            // ── REAL-WORLD 4: Serializer Factory ──────────────────────────────
            // Serialize data in different formats

            // Example 4: Serializer Factory
            // Output: 4. Serializer Factory:
            Console.WriteLine("\n4. Serializer Factory:");
            
            // Create serializers
            var jsonSerializer = SerializerFactory.CreateSerializer("JSON");
            var xmlSerializer = SerializerFactory.CreateSerializer("XML");
            var binarySerializer = SerializerFactory.CreateSerializer("Binary");
            
            // Serialize sample object
            var data = new SampleData { Name = "Test", Value = 42 };
            
            var json = jsonSerializer.Serialize(data); // to JSON
            var xml = xmlSerializer.Serialize(data); // to XML
            var binary = binarySerializer.Serialize(data); // to binary
            
            // Output: JSON: {"Name":"Test","Value":42}
            // Output: XML: <SampleData><Name>Test</Name><Value>42</Value></SampleData>
            // Output: Binary: [bytes]
            Console.WriteLine($"   JSON: {json}");
            Console.WriteLine($"   XML: {xml}");
            Console.WriteLine($"   Binary: {binary.Length} bytes");

            // ── REAL-WORLD 5: Handler Factory ─────────────────────────────────
            // Create request handlers dynamically

            // Example 5: Handler Factory
            // Output: 5. Handler Factory:
            Console.WriteLine("\n5. Handler Factory:");
            
            // Create handlers
            var getHandler = HandlerFactory.CreateHandler("GET");
            var postHandler = HandlerFactory.CreateHandler("POST");
            var putHandler = HandlerFactory.CreateHandler("PUT");
            var deleteHandler = HandlerFactory.CreateHandler("DELETE");
            
            // Process requests
            getHandler.Handle("/users"); // handles GET request
            postHandler.Handle("/users"); // handles POST request
            putHandler.Handle("/users/1"); // handles PUT request
            deleteHandler.Handle("/users/1"); // handles DELETE request
            
            // Output: GET handler processed /users
            // Output: POST handler processed /users
            // Output: PUT handler processed /users/1
            // Output: DELETE handler processed /users/1

            Console.WriteLine("\n=== Factory Real-World Complete ===");
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
    /// Console logger
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"   Console: {message}");
        }
    }

    /// <summary>
    /// File logger
    /// </summary>
    public class FileLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"   File: {message}");
        }
    }

    /// <summary>
    /// Database logger
    /// </summary>
    public class DatabaseLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"   Database: {message}");
        }
    }

    /// <summary>
    /// Logger factory
    /// </summary>
    public static class LoggerFactory
    {
        /// <summary>
        /// Creates logger by type
        /// </summary>
        public static ILogger CreateLogger(string type)
        {
            return type switch
            {
                "Console" => new ConsoleLogger(),
                "File" => new FileLogger(),
                "Database" => new DatabaseLogger(),
                _ => throw new ArgumentException($"Unknown logger: {type}")
            };
        }
    }

    /// <summary>
    /// Notifier interface
    /// </summary>
    public interface INotifier
    {
        void Send(string target, string message); // method: sends notification
    }

    /// <summary>
    /// Email notifier
    /// </summary>
    public class EmailNotifier : INotifier
    {
        public void Send(string target, string message)
        {
            Console.WriteLine($"   Email sent to {target}");
        }
    }

    /// <summary>
    /// SMS notifier
    /// </summary>
    public class SMSNotifier : INotifier
    {
        public void Send(string target, string message)
        {
            Console.WriteLine($"   SMS sent to {target}");
        }
    }

    /// <summary>
    /// Push notification notifier
    /// </summary>
    public class PushNotifier : INotifier
    {
        public void Send(string target, string message)
        {
            Console.WriteLine($"   Push sent to {target}");
        }
    }

    /// <summary>
    /// Notification factory
    /// </summary>
    public static class NotificationFactory
    {
        /// <summary>
        /// Creates notifier by type
        /// </summary>
        public static INotifier CreateNotifier(string type)
        {
            return type switch
            {
                "Email" => new EmailNotifier(),
                "SMS" => new SMSNotifier(),
                "Push" => new PushNotifier(),
                _ => throw new ArgumentException($"Unknown notifier: {type}")
            };
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
    /// Memory cache
    /// </summary>
    public class MemoryCache : ICache
    {
        public void Set(string key, string value)
        {
            Console.WriteLine($"   Memory cache: {key}={value}");
        }
        public string Get(string key) => null;
    }

    /// <summary>
    /// Redis cache
    /// </summary>
    public class RedisCache : ICache
    {
        public void Set(string key, string value)
        {
            Console.WriteLine($"   Redis cache: {key}={value}");
        }
        public string Get(string key) => null;
    }

    /// <summary>
    /// Disk cache
    /// </summary>
    public class DiskCache : ICache
    {
        public void Set(string key, string value)
        {
            Console.WriteLine($"   Disk cache: {key}={value}");
        }
        public string Get(string key) => null;
    }

    /// <summary>
    /// Cache factory
    /// </summary>
    public static class CacheFactory
    {
        /// <summary>
        /// Creates cache by type
        /// </summary>
        public static ICache CreateCache(string type)
        {
            return type switch
            {
                "Memory" => new MemoryCache(),
                "Redis" => new RedisCache(),
                "Disk" => new DiskCache(),
                _ => throw new ArgumentException($"Unknown cache: {type}")
            };
        }
    }

    /// <summary>
    /// Sample data for serialization
    /// </summary>
    public class SampleData
    {
        public string Name { get; set; } // property: data name
        public int Value { get; set; } // property: data value
    }

    /// <summary>
    /// Serializer interface
    /// </summary>
    public interface ISerializer
    {
        string Serialize(object obj); // method: serializes object to string
    }

    /// <summary>
    /// JSON serializer
    /// </summary>
    public class JSONSerializer : ISerializer
    {
        public string Serialize(object obj)
        {
            var data = (SampleData)obj; // cast to SampleData
            return $"{{\"Name\":\"{data.Name}\",\"Value\":{data.Value}}}"; // JSON format
        }
    }

    /// <summary>
    /// XML serializer
    /// </summary>
    public class XMLSerializer : ISerializer
    {
        public string Serialize(object obj)
        {
            var data = (SampleData)obj; // cast to SampleData
            return $"<SampleData><Name>{data.Name}</Name><Value>{data.Value}</Value></SampleData>"; // XML format
        }
    }

    /// <summary>
    /// Binary serializer
    /// </summary>
    public class BinarySerializer : ISerializer
    {
        public string Serialize(object obj)
        {
            return "[bytes]"; // placeholder for binary
        }
    }

    /// <summary>
    /// Serializer factory
    /// </summary>
    public static class SerializerFactory
    {
        /// <summary>
        /// Creates serializer by type
        /// </summary>
        public static ISerializer CreateSerializer(string type)
        {
            return type switch
            {
                "JSON" => new JSONSerializer(),
                "XML" => new XMLSerializer(),
                "Binary" => new BinarySerializer(),
                _ => throw new ArgumentException($"Unknown serializer: {type}")
            };
        }
    }

    /// <summary>
    /// Request handler interface
    /// </summary>
    public interface IRequestHandler
    {
        void Handle(string path); // method: handles request
    }

    /// <summary>
    /// GET handler
    /// </summary>
    public class GetHandler : IRequestHandler
    {
        public void Handle(string path)
        {
            Console.WriteLine($"   GET handler processed {path}");
        }
    }

    /// <summary>
    /// POST handler
    /// </summary>
    public class PostHandler : IRequestHandler
    {
        public void Handle(string path)
        {
            Console.WriteLine($"   POST handler processed {path}");
        }
    }

    /// <summary>
    /// PUT handler
    /// </summary>
    public class PutHandler : IRequestHandler
    {
        public void Handle(string path)
        {
            Console.WriteLine($"   PUT handler processed {path}");
        }
    }

    /// <summary>
    /// DELETE handler
    /// </summary>
    public class DeleteHandler : IRequestHandler
    {
        public void Handle(string path)
        {
            Console.WriteLine($"   DELETE handler processed {path}");
        }
    }

    /// <summary>
    /// Handler factory
    /// </summary>
    public static class HandlerFactory
    {
        /// <summary>
        /// Creates handler by HTTP method
        /// </summary>
        public static IRequestHandler CreateHandler(string method)
        {
            return method switch
            {
                "GET" => new GetHandler(),
                "POST" => new PostHandler(),
                "PUT" => new PutHandler(),
                "DELETE" => new DeleteHandler(),
                _ => throw new ArgumentException($"Unknown method: {method}")
            };
        }
    }
}