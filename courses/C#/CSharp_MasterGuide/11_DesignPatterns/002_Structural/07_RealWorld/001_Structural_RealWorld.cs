/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Real-World Applications
 * FILE      : 12_Structural_RealWorld.cs
 * PURPOSE   : Real-world examples of Structural patterns
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural._07_RealWorld
{
    /// <summary>
    /// Real-world Structural pattern demonstrations
    /// </summary>
    public class StructuralRealWorld
    {
        /// <summary>
        /// Entry point for real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Structural Patterns Real-World ===
            Console.WriteLine("=== Structural Patterns Real-World ===\n");

            // ── EXAMPLE: Adapter - Legacy Payment Integration ─────────────────
            // Adapt old payment system to new interface

            // Output: --- Adapter: Legacy Payment System ---
            Console.WriteLine("--- Adapter: Legacy Payment System ---");
            
            var legacyPayment = new LegacyPaymentSystem();
            var adapter = new PaymentAdapter(legacyPayment);
            
            // Modern interface works with legacy system
            var paymentProcessor = new ModernPaymentProcessor();
            paymentProcessor.ProcessPayment(adapter, 250.00m);
            
            // Output: Processed: $250.00 via Legacy System

            // ── EXAMPLE: Decorator - Logging Middleware ───────────────────────
            // Add functionality dynamically

            // Output: --- Decorator: Middleware Stack ---
            Console.WriteLine("\n--- Decorator: Middleware Stack ---");
            
            var handler = new AuthHandler();
            var loggedHandler = new LoggingDecorator(handler);
            var cachedHandler = new CachingDecorator(loggedHandler);
            
            cachedHandler.Handle("User Request");
            
            // Output: Auth -> Log -> Cache applied

            // ── EXAMPLE: Proxy - Caching API Client ───────────────────────────
            // Control access to remote service

            // Output: --- Proxy: API Client with Cache ---
            Console.WriteLine("\n--- Proxy: API Client with Cache ---");
            
            var apiClient = new CachedApiClient(new RealApiClient());
            
            // First call - goes to API
            var result1 = apiClient.GetData("users");
            // Output: Fetched from API
            
            // Second call - from cache
            var result2 = apiClient.GetData("users");
            // Output: From cache

            // ── EXAMPLE: Composite - File System ───────────────────────────────
            // Treat individual files and directories uniformly

            // Output: --- Composite: File System ---
            Console.WriteLine("\n--- Composite: File System ---");
            
            var root = new Folder("root");
            var docs = new Folder("documents");
            var file1 = new File("report.pdf", 1024);
            
            docs.Add(file1);
            root.Add(docs);
            root.Add(new File("config.json", 256));
            
            root.Print("");
            
            // Output: root/ -> documents/ -> report.pdf

            // ── EXAMPLE: Facade - Simple API Client ───────────────────────────
            // Hide complexity behind simple interface

            // Output: --- Facade: Order Processing ---
            Console.WriteLine("\n--- Facade: Order Processing ---");
            
            var orderFacade = new OrderFacade();
            orderFacade.PlaceOrder("CUSTOMER-001", "LAPTOP-001");
            
            // Output: Order placed for CUSTOMER-001

            // ── EXAMPLE: Bridge - Multi-Platform Notifications ─────────────────
            // Separate notification type from delivery method

            // Output: --- Bridge: Notification System ---
            Console.WriteLine("\n--- Bridge: Notification System ---");
            
            var emailNotifier = new EmailNotifier(new TextContent());
            var smsNotifier = new SmsNotifier(new TextContent());
            var pushNotifier = new PushNotifier(new HtmlContent());
            
            emailNotifier.Send("Hello");
            smsNotifier.Send("Hello");
            pushNotifier.Send("Hello");
            
            // Output: Email: Text, SMS: Text, Push: HTML

            Console.WriteLine("\n=== Structural Real-World Complete ===");
        }
    }

    /// <summary>
    /// Legacy payment system
    /// </summary>
    public class LegacyPaymentSystem
    {
        public void ExecutePayment(decimal amount) => 
            Console.WriteLine($"   Processed: ${amount:F2} via Legacy System");
    }

    /// <summary>
    /// Adapter for legacy system
    /// </summary>
    public class PaymentAdapter
    {
        private readonly LegacyPaymentSystem _legacy;
        
        public PaymentAdapter(LegacyPaymentSystem legacy) => _legacy = legacy;
        
        public void Pay(decimal amount) => _legacy.ExecutePayment(amount);
    }

    /// <summary>
    /// Modern payment interface
    /// </summary>
    public interface IPayment
    {
        void Pay(decimal amount);
    }

    /// <summary>
    /// Modern payment processor
    /// </summary>
    public class ModernPaymentProcessor
    {
        public void ProcessPayment(IPayment payment, decimal amount) => payment.Pay(amount);
    }

    /// <summary>
    /// Request handler interface
    /// </summary>
    public interface IRequestHandler
    {
        void Handle(string request);
    }

    /// <summary>
    /// Auth handler
    /// </summary>
    public class AuthHandler : IRequestHandler
    {
        public void Handle(string request) => Console.WriteLine("   Auth applied");
    }

    /// <summary>
    /// Logging decorator
    /// </summary>
    public class LoggingDecorator : IRequestHandler
    {
        private readonly IRequestHandler _handler;
        
        public LoggingDecorator(IRequestHandler handler) => _handler = handler;
        
        public void Handle(string request)
        {
            Console.WriteLine("   Logging: " + request);
            _handler.Handle(request);
        }
    }

    /// <summary>
    /// Caching decorator
    /// </summary>
    public class CachingDecorator : IRequestHandler
    {
        private readonly IRequestHandler _handler;
        
        public CachingDecorator(IRequestHandler handler) => _handler = handler;
        
        public void Handle(string request)
        {
            Console.WriteLine("   Cache checked");
            _handler.Handle(request);
        }
    }

    /// <summary>
    /// API client interface
    /// </summary>
    public interface IApiClient
    {
        string GetData(string endpoint);
    }

    /// <summary>
    /// Real API client
    /// </summary>
    public class RealApiClient : IApiClient
    {
        public string GetData(string endpoint) => "Data from API";
    }

    /// <summary>
    /// Caching proxy
    /// </summary>
    public class CachedApiClient : IApiClient
    {
        private readonly IApiClient _real;
        private readonly Dictionary<string, string> _cache = new();
        
        public CachedApiClient(IApiClient real) => _real = real;
        
        public string GetData(string endpoint)
        {
            if (_cache.ContainsKey(endpoint))
            {
                Console.WriteLine("   From cache");
                return _cache[endpoint];
            }
            
            Console.WriteLine("   Fetched from API");
            var data = _real.GetData(endpoint);
            _cache[endpoint] = data;
            return data;
        }
    }

    /// <summary>
    /// File system component interface
    /// </summary>
    public interface IFileSystemItem
    {
        void Print(string indent);
    }

    /// <summary>
    /// File implementation
    /// </summary>
    public class File : IFileSystemItem
    {
        public string Name { get; }
        public int Size { get; }
        
        public File(string name, int size)
        {
            Name = name;
            Size = size;
        }
        
        public void Print(string indent) => 
            Console.WriteLine($"{indent}{Name} ({Size}KB)");
    }

    /// <summary>
    /// Folder - composite
    /// </summary>
    public class Folder : IFileSystemItem
    {
        public string Name { get; }
        private readonly List<IFileSystemItem> _items = new();
        
        public Folder(string name) => Name = name;
        
        public void Add(IFileSystemItem item) => _items.Add(item);
        
        public void Print(string indent)
        {
            Console.WriteLine($"{indent}{Name}/");
            foreach (var item in _items)
                item.Print(indent + "  ");
        }
    }

    /// <summary>
    /// Order facade
    /// </summary>
    public class OrderFacade
    {
        public void PlaceOrder(string customerId, string productId)
        {
            // Hide all complexity behind simple method
            ValidateCustomer(customerId);
            CheckInventory(productId);
            ProcessPayment(customerId, productId);
            ShipProduct(productId);
            
            Console.WriteLine($"   Order placed for {customerId}");
        }
        
        private void ValidateCustomer(string id) { }
        private void CheckInventory(string id) { }
        private void ProcessPayment(string c, string p) { }
        private void ShipProduct(string id) { }
    }

    /// <summary>
    /// Notification content interface
    /// </summary>
    public interface INotificationContent
    {
        string Format(string message);
    }

    /// <summary>
    /// Text content
    /// </summary>
    public class TextContent : INotificationContent
    {
        public string Format(string message) => message;
    }

    /// <summary>
    /// HTML content
    /// </summary>
    public class HtmlContent : INotificationContent
    {
        public string Format(string message) => $"<html><body>{message}</body></html>";
    }

    /// <summary>
    /// Notifier interface
    /// </summary>
    public interface INotifier
    {
        void Send(string message);
    }

    /// <summary>
    /// Email notifier
    /// </summary>
    public class EmailNotifier : INotifier
    {
        private readonly INotificationContent _content;
        
        public EmailNotifier(INotificationContent content) => _content = content;
        
        public void Send(string message) => 
            Console.WriteLine($"   Email: {_content.Format(message)}");
    }

    /// <summary>
    /// SMS notifier
    /// </summary>
    public class SmsNotifier : INotifier
    {
        private readonly INotificationContent _content;
        
        public SmsNotifier(INotificationContent content) => _content = content;
        
        public void Send(string message) => 
            Console.WriteLine($"   SMS: {_content.Format(message)}");
    }

    /// <summary>
    /// Push notifier
    /// </summary>
    public class PushNotifier : INotifier
    {
        private readonly INotificationContent _content;
        
        public PushNotifier(INotificationContent content) => _content = content;
        
        public void Send(string message) => 
            Console.WriteLine($"   Push: {_content.Format(message)}");
    }
}