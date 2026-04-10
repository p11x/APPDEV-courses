/*
 * ============================================================
 * TOPIC     : Architecture
 * SUBTOPIC  : Microservices
 * FILE      : Microservices_Demo.cs
 * PURPOSE   : Microservices architecture
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._20_Architecture._04_Microservices
{
    /// <summary>
    /// Microservices demonstration
    /// </summary>
    public class MicroservicesDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Microservices ===\n");

            // Output: --- Service Discovery ---
            Console.WriteLine("--- Service Discovery ---");

            var registry = new ServiceRegistry();
            registry.Register("user-service", "http://user:80");
            registry.Register("order-service", "http://order:80");
            var location = registry.Discover("user-service");
            Console.WriteLine($"   Location: {location}");
            // Output: Location: http://user:80

            // Output: --- API Gateway ---
            Console.WriteLine("\n--- API Gateway ---");

            var gateway = new ApiGateway();
            var response = gateway.Route("/api/users");
            Console.WriteLine($"   Routed: {response}");
            // Output: Routed: user-service

            // Output: --- Inter-Service Communication ---
            Console.WriteLine("\n--- Inter-Service ---");

            var client = new ServiceClient();
            client.Call("order-service", "POST", "/orders");
            Console.WriteLine("   Called order service");
            // Output: Called order service

            // Output: --- Message Queue ---
            Console.WriteLine("\n--- Message Queue ---");

            var queue = new MessageQueue();
            queue.Publish("order.created", new { OrderId = 1 });
            Console.WriteLine("   Message published");
            // Output: Message published

            Console.WriteLine("\n=== Microservices Complete ===");
        }
    }

    /// <summary>
    /// Service registry
    /// </summary>
    public class ServiceRegistry
    {
        private readonly System.Collections.Generic.Dictionary<string, string> _services = new();

        public void Register(string name, string url) => _services[name] = url;
        public string Discover(string name) => _services[name];
    }

    /// <summary>
    /// API gateway
    /// </summary>
    public class ApiGateway
    {
        public string Route(string path) => "user-service";
    }

    /// <summary>
    /// HTTP service client
    /// </summary>
    public class ServiceClient
    {
        public void Call(string service, string method, string path) { }
    }

    /// <summary>
    /// Message queue
    /// </summary>
    public class MessageQueue
    {
        public void Publish<T>(string topic, T message) { }
    }
}