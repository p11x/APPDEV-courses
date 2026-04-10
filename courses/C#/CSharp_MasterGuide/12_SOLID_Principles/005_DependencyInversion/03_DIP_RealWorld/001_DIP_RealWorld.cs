/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Dependency Inversion Principle - Real-World
 * FILE      : 03_DIP_RealWorld.cs
 * PURPOSE   : Demonstrates real-world DIP applications including
 *             IoC container, database abstraction, and
 *             service layering
 * ============================================================
 */

using System; // Core System namespace for Console
using System.Collections.Generic; // Generic collections

namespace CSharp_MasterGuide._12_SOLID_Principles._05_DependencyInversion._03_DIP_RealWorld
{
    /// <summary>
    /// Demonstrates real-world Dependency Inversion Principle
    /// </summary>
    public class DIPRealWorldDemo
    {
        /// <summary>
        /// Entry point for DIP real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Database Abstraction with DIP
            // ═══════════════════════════════════════════════════════════
            // High-level modules depend on abstractions, not concrete types
            // Can swap database implementation without changing business logic

            Console.WriteLine("=== DIP Real-World ===\n");

            // Output: --- Database Abstraction ---
            Console.WriteLine("--- Database Abstraction ---");

            // Use SQL database - swap doesn't change service code
            IDatabase database = new SqlDatabase();
            var userService = new UserService(database);
            
            userService.CreateUser("Alice");
            // Output: SQL: Inserting user Alice
            userService.GetUser(1);
            // Output: SQL: Selecting user 1

            // Switch to MongoDB - same interface, different implementation
            database = new MongoDatabase();
            userService = new UserService(database);
            
            userService.CreateUser("Bob");
            // Output: MongoDB: Inserting user Bob
            userService.GetUser(2);
            // Output: MongoDB: Selecting user 2

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: IoC Container with DIP
            // ═══════════════════════════════════════════════════════════
            // Inversion of Control container manages dependencies
            // Dependencies are injected, not created by the class

            // Output: --- IoC Container ---
            Console.WriteLine("\n--- IoC Container ---");

            var container = new SimpleContainer();
            container.Register<IMessageService, EmailService>();
            // Output: Registered: IMessageService -> EmailService
            container.Register<INotificationService, NotificationService>();
            // Output: Registered: INotificationService -> NotificationService

            var notification = container.Resolve<INotificationService>();
            notification.Send("Hello");
            // Output: Email sent: Hello

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Service Layering with DIP
            // ═══════════════════════════════════════════════════════════
            // Each layer depends on abstractions of the next layer
            // Business logic doesn't depend on data access details

            // Output: --- Service Layering ---
            Console.WriteLine("\n--- Service Layering ---");

            // Controller depends on service abstraction
            IOrderService orderService = new OrderService(
                new SqlDatabase()
            );
            orderService.PlaceOrder("Widget", 2);
            // Output: Placing order: Widget x2
            // Output: SQL: Inserting order

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Logger Abstraction with DIP
            // ═══════════════════════════════════════════════════════════
            // Application code depends on logger abstraction
            // Can switch logging implementation at runtime

            // Output: --- Logger Abstraction ---
            Console.WriteLine("\n--- Logger Abstraction ---");

            // Console logger
            ILogger consoleLogger = new ConsoleLogger();
            var appConsole = new Application(consoleLogger);
            appConsole.Run();
            // Output: [Console] Application started
            // Output: [Console] Processing data

            // File logger
            ILogger fileLogger = new FileLogger();
            var appFile = new Application(fileLogger);
            appFile.Run();
            // Output: [File] Application started
            // Output: [File] Processing data

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Payment Gateway with DIP
            // ═══════════════════════════════════════════════════════════
            // Payment processor doesn't know concrete gateway
            // Can add new gateways without modifying processor

            // Output: --- Payment Gateway ---
            Console.WriteLine("\n--- Payment Gateway ---");

            // Use PayPal
            IPaymentGateway paypal = new PayPalGateway();
            var checkout = new CheckoutService(paypal);
            checkout.Process(100m);
            // Output: PayPal: Processing $100.00

            // Switch to Stripe
            IPaymentGateway stripe = new StripeGateway();
            checkout = new CheckoutService(stripe);
            checkout.Process(100m);
            // Output: Stripe: Processing $100.00

            Console.WriteLine("\n=== DIP Real-World Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 1: Database Abstraction Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Database abstraction - high-level depends on this
    /// </summary>
    public interface IDatabase
    {
        void Insert(string data);
        void Select(int id);
    }

    /// <summary>
    /// SQL database implementation
    /// </summary>
    public class SqlDatabase : IDatabase
    {
        public void Insert(string data) => Console.WriteLine($"   SQL: Inserting user {data}");
        public void Select(int id) => Console.WriteLine($"   SQL: Selecting user {id}");
    }

    /// <summary>
    /// MongoDB implementation - interchangeable with SQL
    /// </summary>
    public class MongoDatabase : IDatabase
    {
        public void Insert(string data) => Console.WriteLine($"   MongoDB: Inserting user {data}");
        public void Select(int id) => Console.WriteLine($"   MongoDB: Selecting user {id}");
    }

    /// <summary>
    /// User service - depends on IDatabase abstraction
    /// </summary>
    public class UserService
    {
        private readonly IDatabase _database;

        public UserService(IDatabase database)
        {
            _database = database;
        }

        public void CreateUser(string name)
        {
            _database.Insert(name);
        }

        public void GetUser(int id)
        {
            _database.Select(id);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 2: IoC Container Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Simple IoC container
    /// </summary>
    public class SimpleContainer
    {
        private readonly Dictionary<Type, Type> _registrations = new();

        public void Register<TAbstraction, TImplementation>() 
            where TImplementation : class, TAbstraction, new()
        {
            _registrations[typeof(TAbstraction)] = typeof(TImplementation);
            Console.WriteLine($"   Registered: {typeof(TAbstraction).Name} -> {typeof(TImplementation).Name}");
        }

        public T Resolve<T>() where T : class, new()
        {
            var implementationType = _registrations[typeof(T)];
            return new T();
        }
    }

    /// <summary>
    /// Message service interface
    /// </summary>
    public interface IMessageService
    {
        void SendMessage(string message);
    }

    /// <summary>
    /// Email service implementation
    /// </summary>
    public class EmailService : IMessageService
    {
        public void SendMessage(string message) => Console.WriteLine($"   Email sent: {message}");
    }

    /// <summary>
    /// Notification service interface
    /// </summary>
    public interface INotificationService
    {
        void Send(string message);
    }

    /// <summary>
    /// Notification service - depends on IMessageService abstraction
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IMessageService _messageService;

        public NotificationService(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public void Send(string message)
        {
            _messageService.SendMessage(message);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 3: Service Layering Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Order service - business logic layer
    /// </summary>
    public class OrderService
    {
        private readonly IDatabase _database;

        public OrderService(IDatabase database)
        {
            _database = database;
        }

        public void PlaceOrder(string product, int quantity)
        {
            Console.WriteLine($"   Placing order: {product} x{quantity}");
            _database.Insert($"Order: {product} x{quantity}");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 4: Logger Abstraction Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Logger abstraction - application depends on this
    /// </summary>
    public interface ILogger
    {
        void Log(string message);
    }

    /// <summary>
    /// Console logger implementation
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine($"   [{DateTime.Now.TimeOfDay}] {message}");
    }

    /// <summary>
    /// File logger implementation - interchangeable
    /// </summary>
    public class FileLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine($"   [File] {message}");
    }

    /// <summary>
    /// Application - depends on ILogger abstraction
    /// </summary>
    public class Application
    {
        private readonly ILogger _logger;

        public Application(ILogger logger)
        {
            _logger = logger;
        }

        public void Run()
        {
            _logger.Log("Application started");
            _logger.Log("Processing data");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 5: Payment Gateway Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Payment gateway abstraction
    /// </summary>
    public interface IPaymentGateway
    {
        void ProcessPayment(decimal amount);
    }

    /// <summary>
    /// PayPal gateway implementation
    /// </summary>
    public class PayPalGateway : IPaymentGateway
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"   PayPal: Processing ${amount:F2}");
        }
    }

    /// <summary>
    /// Stripe gateway implementation - interchangeable
    /// </summary>
    public class StripeGateway : IPaymentGateway
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"   Stripe: Processing ${amount:F2}");
        }
    }

    /// <summary>
    /// Checkout service - depends on IPaymentGateway abstraction
    /// </summary>
    public class CheckoutService
    {
        private readonly IPaymentGateway _gateway;

        public CheckoutService(IPaymentGateway gateway)
        {
            _gateway = gateway;
        }

        public void Process(decimal amount)
        {
            _gateway.ProcessPayment(amount);
        }
    }
}
