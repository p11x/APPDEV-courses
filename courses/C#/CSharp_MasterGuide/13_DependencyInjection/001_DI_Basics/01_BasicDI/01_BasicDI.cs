/*
 * ============================================================
 * TOPIC     : Dependency Injection
 * SUBTOPIC  : Basic DI Concepts
 * FILE      : 01_BasicDI.cs
 * PURPOSE   : Demonstrates basic Dependency Injection in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._13_DependencyInjection._01_BasicDI
{
    /// <summary>
    /// Demonstrates basic Dependency Injection
    /// </summary>
    public class BasicDIDemo
    {
        /// <summary>
        /// Entry point for DI examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Basic Dependency Injection ===
            Console.WriteLine("=== Basic Dependency Injection ===\n");

            // ── CONCEPT: What is DI? ────────────────────────────────────────
            // Dependencies are provided from outside

            // Example 1: Without DI (Tight Coupling)
            // Output: 1. Without DI:
            Console.WriteLine("1. Without DI:");
            
            // Class creates its own dependency
            var badService = new BadCustomerService();
            badService.AddCustomer("John");
            // Output: Customer added: John

            // Example 2: With DI (Loose Coupling)
            // Output: 2. With DI:
            Console.WriteLine("\n2. With DI:");
            
            // Dependency injected from outside
            var goodService = new GoodCustomerService(new EmailService());
            goodService.AddCustomer("John");
            // Output: Customer added: John
            // Output: Email sent to John

            // ── CONCEPT: DI Patterns ────────────────────────────────────────
            // Constructor, Property, Method injection

            // Example 3: Constructor Injection
            // Output: 3. Constructor Injection:
            Console.WriteLine("\n3. Constructor Injection:");
            
            // Dependencies in constructor
            var orderService = new OrderService(
                new PaymentGateway(),
                new ShippingService(),
                new NotificationService());
            
            orderService.ProcessOrder(123);
            // Output: Order 123: Payment processed
            // Output: Order 123: Shipping arranged
            // Output: Order 123: Notification sent

            // Example 4: Property Injection
            // Output: 4. Property Injection:
            Console.WriteLine("\n4. Property Injection:");
            
            // Dependencies via properties
            var logger = new PropertyLogger();
            logger.LogService = new ConsoleLogService();
            logger.Log("Message");
            // Output: [Console] Message

            // Example 5: Method Injection
            // Output: 5. Method Injection:
            Console.WriteLine("\n5. Method Injection:");
            
            // Dependency passed to method
            var calculator = new ShippingCalculator();
            var domestic = calculator.Calculate(new DomesticRate(), 10);
            var international = calculator.Calculate(new InternationalRate(), 10);
            // Output: Domestic: $20.00
            // Output: International: $50.00

            Console.WriteLine("\n=== Basic DI Complete ===");
        }
    }

    /// <summary>
    /// BAD: Tightly coupled - creates its own dependency
    /// </summary>
    public class BadCustomerService
    {
        private EmailService _emailService = new EmailService();
        
        public void AddCustomer(string name)
        {
            Console.WriteLine($"   Customer added: {name}");
            _emailService.Send(name);
        }
    }

    /// <summary>
    /// Email service
    /// </summary>
    public class EmailService
    {
        public void Send(string name)
        {
            Console.WriteLine($"   Email sent to {name}");
        }
    }

    /// <summary>
    /// GOOD: Depends on abstraction, injected externally
    /// </summary>
    public class GoodCustomerService
    {
        private IEmailService _emailService;
        
        public GoodCustomerService(IEmailService emailService)
        {
            _emailService = emailService;
        }
        
        public void AddCustomer(string name)
        {
            Console.WriteLine($"   Customer added: {name}");
            _emailService.Send(name);
        }
    }

    /// <summary>
    /// Email service interface
    /// </summary>
    public interface IEmailService
    {
        void Send(string name); // method: sends email
    }

    /// <summary>
    /// Payment gateway
    /// </summary>
    public class PaymentGateway
    {
        public void ProcessPayment(decimal amount) => Console.WriteLine($"   Payment processed: ${amount}");
    }

    /// <summary>
    /// Shipping service
    /// </summary>
    public class ShippingService
    {
        public void ArrangeShipping(int orderId) => Console.WriteLine($"   Shipping arranged for order {orderId}");
    }

    /// <summary>
    /// Notification service
    /// </summary>
    public class NotificationService
    {
        public void Notify(int orderId) => Console.WriteLine($"   Notification sent for order {orderId}");
    }

    /// <summary>
    /// Order service with constructor injection
    /// </summary>
    public class OrderService
    {
        private PaymentGateway _payment;
        private ShippingService _shipping;
        private NotificationService _notification;
        
        public OrderService(PaymentGateway payment, ShippingService shipping, NotificationService notification)
        {
            _payment = payment;
            _shipping = shipping;
            _notification = notification;
        }
        
        public void ProcessOrder(int orderId)
        {
            _payment.ProcessPayment(100.00m);
            _shipping.ArrangeShipping(orderId);
            _notification.Notify(orderId);
            Console.WriteLine($"   Order {orderId}: All services completed");
        }
    }

    /// <summary>
    /// Log service interface
    /// </summary>
    public interface ILogService
    {
        void Log(string message); // method: logs message
    }

    /// <summary>
    /// Console log service
    /// </summary>
    public class ConsoleLogService : ILogService
    {
        public void Log(string message) => Console.WriteLine($"   [Console] {message}");
    }

    /// <summary>
    /// Property injection example
    /// </summary>
    public class PropertyLogger
    {
        public ILogService LogService { get; set; }
        
        public void Log(string message)
        {
            LogService?.Log(message);
        }
    }

    /// <summary>
    /// Shipping rate interface
    /// </summary>
    public interface IShippingRate
    {
        decimal Calculate(decimal weight); // method: calculates shipping cost
    }

    /// <summary>
    /// Domestic rate
    /// </summary>
    public class DomesticRate : IShippingRate
    {
        public decimal Calculate(decimal weight) => weight * 2.0m;
    }

    /// <summary>
    /// International rate
    /// </summary>
    public class InternationalRate : IShippingRate
    {
        public decimal Calculate(decimal weight) => weight * 5.0m;
    }

    /// <summary>
    /// Method injection example
    /// </summary>
    public class ShippingCalculator
    {
        public decimal Calculate(IShippingRate rate, decimal weight)
        {
            var cost = rate.Calculate(weight);
            Console.WriteLine($"   Shipping cost: ${cost:F2}");
            return cost;
        }
    }
}