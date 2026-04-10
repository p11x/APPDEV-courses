/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Factory Pattern (Part 2)
 * FILE      : 04_Factory_Part2.cs
 * PURPOSE   : Continues factory patterns with generic factories,
 *             factory registry, dependency injection, and
 *             real-world payment factory examples
 * ============================================================
 */

using System; // Core System namespace for Console
using System.Collections.Generic; // Generic collections for dictionary
using System.Linq; // Linq for string manipulation

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational._02_Factory
{
    /// <summary>
    /// Demonstrates advanced factory pattern techniques including
    /// generic factories, registries, and dependency injection
    /// </summary>
    public class FactoryAdvancedDemo
    {
        /// <summary>
        /// Entry point for factory advanced examples
        /// </summary>
        public static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Generic Factory
            // ═══════════════════════════════════════════════════════════
            // Generic factory reduces code duplication across different types
            // Uses constraint: class (reference type), new() (parameterless constructor)
            // Works with any type matching these constraints

            Console.WriteLine("=== Factory Advanced (Part 2) ===\n");

            // Output: --- Generic Factory ---
            Console.WriteLine("--- Generic Factory ---");

            // GenericFactory<T> creates instances without knowing concrete type at compile time
            var genericFactory = new GenericFactory<Logger>();
            var logger1 = genericFactory.Create();
            // Output: Logger instance created
            Console.WriteLine($"  Type: {logger1.GetType().Name}");
            // Output: Type: Logger

            // Same factory works with different types
            var stringFactory = new GenericFactory<StringWriter>();
            var stringWriter = stringFactory.Create();
            // Output: StringWriter instance created
            Console.WriteLine($"  Type: {stringWriter.GetType().Name}");
            // Output: Type: StringWriter

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Factory with Registration
            // ═══════════════════════════════════════════════════════════
            // Registry pattern allows dynamic registration of factories
            // Enables plugin architecture and runtime type selection
            // Useful when types are not known at compile time

            // Output: --- Factory with Registration ---
            Console.WriteLine("\n--- Factory with Registration ---");

            // NotificationRegistry stores factory functions keyed by string
            var registry = new NotificationRegistry();
            registry.Register("email", () => new EmailNotification());
            // Output: Registered: email
            registry.Register("sms", () => new SmsNotification());
            // Output: Registered: sms
            registry.Register("push", () => new PushNotification());
            // Output: Registered: push

            // Create notifications through registry at runtime
            var email = registry.Create("email");
            // Output: Creating: Email
            email.Send("Hello via Email");
            // Output: Email sent: Hello via Email

            var sms = registry.Create("sms");
            // Output: Creating: SMS
            sms.Send("Hello via SMS");
            // Output: SMS sent: Hello via SMS

            var push = registry.Create("push");
            // Output: Creating: Push
            push.Send("Hello via Push");
            // Output: Push sent: Hello via Push

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Factory with Parameters
            // ═══════════════════════════════════════════════════════════
            // Factories can accept parameters to customize object creation
            // Parameters allow configuration at creation time

            // Output: --- Factory with Parameters ---
            Console.WriteLine("\n--- Factory with Parameters ---");

            // ParameterizedVehicleFactory accepts type and name parameters
            var factory = new ParameterizedVehicleFactory();
            
            var car = factory.Create("car", "Tesla Model 3");
            // Output: Created: Car - Tesla Model 3
            Console.WriteLine($"  {car.GetType().Name}: {car.Name}");
            // Output: Car: Tesla Model 3

            var bike = factory.Create("motorcycle", "Harley Davidson");
            // Output: Created: Motorcycle - Harley Davidson
            Console.WriteLine($"  {bike.GetType().Name}: {bike.Name}");
            // Output: Motorcycle: Harley Davidson

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Factory with Dependency Injection
            // ═══════════════════════════════════════════════════════════
            // Combining factory with DI improves testability
            // Dependencies are injected rather than created internally
            // Enables mocking and unit testing

            // Output: --- Factory with Dependencies ---
            Console.WriteLine("\n--- Factory with Dependencies ---");

            // FileLogger is injected as dependency
            var logger = new FileLogger(); // Dependency - could be mock in tests
            var diFactory = new ServiceFactory(logger);
            
            var processor = diFactory.CreateProcessor();
            // Output: Created: DataProcessor
            Console.WriteLine($"  Processor type: {processor.GetType().Name}");
            // Output: Processor type: DataProcessor

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Extended Abstract Factory
            // ═══════════════════════════════════════════════════════════
            // Abstract factory creates families of related objects
            // Different factories for different product families (themes, regions)
            // Ensures products are compatible with each other

            // Output: --- Extended Abstract Factory ---
            Console.WriteLine("\n--- Extended Abstract Factory ---");

            // Different factories for different UI themes
            IUIFactory lightFactory = new LightThemeFactory();
            IUIFactory darkFactory = new DarkThemeFactory();

            // Light theme products
            var lightButton = lightFactory.CreateButton();
            // Output: Light Button created
            Console.WriteLine($"  {lightButton.Render()}");
            // Output: Light Button: [Button]

            var lightInput = lightFactory.CreateInput();
            // Output: Light Input created
            Console.WriteLine($"  {lightInput.Render()}");
            // Output: Light Input: [Input]

            // Dark theme products
            var darkButton = darkFactory.CreateButton();
            // Output: Dark Button created
            Console.WriteLine($"  {darkButton.Render()}");
            // Output: Dark Button: [Button]

            var darkInput = darkFactory.CreateInput();
            // Output: Dark Input created
            Console.WriteLine($"  {darkInput.Render()}");
            // Output: Dark Input: [Input]

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World: Payment Factory
            // ═══════════════════════════════════════════════════════════
            // Common real-world use: payment method selection
            // Factory determines which payment handler to use based on type
            // Keeps payment processing logic centralized

            // Output: --- Real-World: Payment Factory ---
            Console.WriteLine("\n--- Real-World: Payment Factory ---");

            // PaymentFactory creates appropriate payment handler
            var paymentFactory = new PaymentFactory();
            
            var creditPayment = paymentFactory.CreatePayment("creditcard", 100m);
            // Output: Credit Card payment created: $100.00
            creditPayment.Process();
            // Output: Processed: Credit Card ***1234

            var paypalPayment = paymentFactory.CreatePayment("paypal", 50m);
            // Output: PayPal payment created: $50.00
            paypalPayment.Process();
            // Output: Processed: PayPal user@email.com

            var bankPayment = paymentFactory.CreatePayment("bank", 200m);
            // Output: Bank payment created: $200.00
            bankPayment.Process();
            // Output: Processed: Bank ***5678

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Real-World: Document Factory
            // ═══════════════════════════════════════════════════════════
            // Document creation is another common factory use case
            // Different exporters for different formats

            // Output: --- Real-World: Document Factory ---
            Console.WriteLine("\n--- Real-World: Document Factory ---");

            var docFactory = new DocumentExporterFactory();
            
            var pdfExporter = docFactory.CreateExporter("pdf");
            // Output: PDF exporter created
            var pdfDoc = pdfExporter.Export("report");
            // Output: Exported: report.pdf

            var csvExporter = docFactory.CreateExporter("csv");
            // Output: CSV exporter created
            var csvDoc = csvExporter.Export("data");
            // Output: Exported: data.csv

            var jsonExporter = docFactory.CreateExporter("json");
            // Output: JSON exporter created
            var jsonDoc = jsonExporter.Export("config");
            // Output: Exported: config.json

            Console.WriteLine("\n=== Factory Advanced Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 1: Generic Factory Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Generic factory using Activator.CreateInstance
    /// Constraint: class (reference type), new() (parameterless constructor)
    /// </summary>
    /// <typeparam name="T">Type to create - must have parameterless constructor</typeparam>
    public class GenericFactory<T> where T : class, new()
    {
        /// <summary>
        /// Creates instance using parameterless constructor
        /// </summary>
        public T Create()
        {
            var instance = new T();
            Console.WriteLine($"   {typeof(T).Name} instance created");
            return instance;
        }
    }

    /// <summary>
    /// Test class for generic factory demonstration
    /// </summary>
    public class Logger
    {
        // Parameterless constructor required by generic factory constraint
    }

    /// <summary>
    /// Another test class for generic factory demonstration
    /// </summary>
    public class StringWriter
    {
        // Parameterless constructor required by generic factory constraint
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 2: Factory Registry Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Base notification interface for factory registry demo
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// Sends notification with given message
        /// </summary>
        void Send(string message);
    }

    /// <summary>
    /// Email notification implementation
    /// </summary>
    public class EmailNotification : INotification
    {
        public void Send(string message)
        {
            Console.WriteLine($"   Email sent: {message}");
        }
    }

    /// <summary>
    /// SMS notification implementation
    /// </summary>
    public class SmsNotification : INotification
    {
        public void Send(string message)
        {
            Console.WriteLine($"   SMS sent: {message}");
        }
    }

    /// <summary>
    /// Push notification implementation
    /// </summary>
    public class PushNotification : INotification
    {
        public void Send(string message)
        {
            Console.WriteLine($"   Push sent: {message}");
        }
    }

    /// <summary>
    /// Factory registry - stores factory functions keyed by string
    /// Allows dynamic registration and runtime type selection
    /// </summary>
    public class NotificationRegistry
    {
        private readonly Dictionary<string, Func<INotification>> _factories = new();

        /// <summary>
        /// Registers factory function for given key
        /// </summary>
        public void Register(string key, Func<INotification> factory)
        {
            _factories[key] = factory;
            Console.WriteLine($"   Registered: {key}");
        }

        /// <summary>
        /// Creates notification using registered factory
        /// </summary>
        public INotification Create(string key)
        {
            if (!_factories.ContainsKey(key))
            {
                throw new ArgumentException($"Unknown notification type: {key}");
            }
            Console.WriteLine($"   Creating: {key.First().ToString().ToUpper() + key.Substring(1)}");
            return _factories[key]();
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 3: Parameterized Factory Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Base vehicle class for parameterized factory demo
    /// </summary>
    public abstract class Vehicle
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// Concrete car implementation
    /// </summary>
    public class Car : Vehicle
    {
        public Car(string name) { Name = name; }
    }

    /// <summary>
    /// Concrete motorcycle implementation
    /// </summary>
    public class Motorcycle : Vehicle
    {
        public Motorcycle(string name) { Name = name; }
    }

    /// <summary>
    /// Factory that accepts parameters for creation
    /// Parameters customize the created object
    /// </summary>
    public class ParameterizedVehicleFactory
    {
        /// <summary>
        /// Creates vehicle based on type with given name
        /// </summary>
        public Vehicle Create(string type, string name)
        {
            Vehicle vehicle = type switch
            {
                "car" => new Car(name),
                "motorcycle" => new Motorcycle(name),
                _ => throw new ArgumentException($"Unknown type: {type}")
            };
            Console.WriteLine($"   Created: {vehicle.GetType().Name} - {name}");
            return vehicle;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 4: Factory with Dependencies
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Logger interface for dependency injection demo
    /// </summary>
    public interface ILogger
    {
        void Log(string message);
    }

    /// <summary>
    /// File logger implementation - actual dependency
    /// </summary>
    public class FileLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine($"   Logged: {message}");
    }

    /// <summary>
    /// Data processor depends on logger interface
    /// </summary>
    public class DataProcessor
    {
        private readonly ILogger _logger;

        public DataProcessor(ILogger logger)
        {
            _logger = logger;
            Console.WriteLine("   Created: DataProcessor");
        }
    }

    /// <summary>
    /// Factory that injects dependencies into created objects
    /// Enables testability through dependency injection
    /// </summary>
    public class ServiceFactory
    {
        private readonly ILogger _logger;

        public ServiceFactory(ILogger logger)
        {
            _logger = logger;
        }

        public DataProcessor CreateProcessor()
        {
            return new DataProcessor(_logger);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 5: Abstract Factory Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Abstract factory interface for UI components
    /// Creates families of related UI objects
    /// </summary>
    public interface IUIFactory
    {
        IButton CreateButton();
        IInput CreateInput();
    }

    /// <summary>
    /// Button interface for abstract factory
    /// </summary>
    public interface IButton
    {
        string Render();
    }

    /// <summary>
    /// Input interface for abstract factory
    /// </summary>
    public interface IInput
    {
        string Render();
    }

    /// <summary>
    /// Light theme button implementation
    /// </summary>
    public class LightButton : IButton
    {
        public string Render() => "Light Button: [Button]";
    }

    /// <summary>
    /// Dark theme button implementation
    /// </summary>
    public class DarkButton : IButton
    {
        public string Render() => "Dark Button: [Button]";
    }

    /// <summary>
    /// Light theme input implementation
    /// </summary>
    public class LightInput : IInput
    {
        public string Render() => "Light Input: [Input]";
    }

    /// <summary>
    /// Dark theme input implementation
    /// </summary>
    public class DarkInput : IInput
    {
        public string Render() => "Dark Input: [Input]";
    }

    /// <summary>
    /// Light theme factory - creates light-themed UI components
    /// </summary>
    public class LightThemeFactory : IUIFactory
    {
        public IButton CreateButton()
        {
            Console.WriteLine("   Light Button created");
            return new LightButton();
        }

        public IInput CreateInput()
        {
            Console.WriteLine("   Light Input created");
            return new LightInput();
        }
    }

    /// <summary>
    /// Dark theme factory - creates dark-themed UI components
    /// </summary>
    public class DarkThemeFactory : IUIFactory
    {
        public IButton CreateButton()
        {
            Console.WriteLine("   Dark Button created");
            return new DarkButton();
        }

        public IInput CreateInput()
        {
            Console.WriteLine("   Dark Input created");
            return new DarkInput();
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 6: Real-World Payment Factory
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Payment interface for real-world factory demo
    /// </summary>
    public interface IPayment
    {
        void Process();
    }

    /// <summary>
    /// Credit card payment implementation
    /// </summary>
    public class CreditCardPayment : IPayment
    {
        private readonly decimal _amount;

        public CreditCardPayment(decimal amount)
        {
            _amount = amount;
            Console.WriteLine($"   Credit Card payment created: ${_amount:F2}");
        }

        public void Process()
        {
            Console.WriteLine($"   Processed: Credit Card ***1234");
        }
    }

    /// <summary>
    /// PayPal payment implementation
    /// </summary>
    public class PayPalPayment : IPayment
    {
        private readonly decimal _amount;

        public PayPalPayment(decimal amount)
        {
            _amount = amount;
            Console.WriteLine($"   PayPal payment created: ${_amount:F2}");
        }

        public void Process()
        {
            Console.WriteLine($"   Processed: PayPal user@email.com");
        }
    }

    /// <summary>
    /// Bank payment implementation
    /// </summary>
    public class BankPayment : IPayment
    {
        private readonly decimal _amount;

        public BankPayment(decimal amount)
        {
            _amount = amount;
            Console.WriteLine($"   Bank payment created: ${_amount:F2}");
        }

        public void Process()
        {
            Console.WriteLine($"   Processed: Bank ***5678");
        }
    }

    /// <summary>
    /// Payment factory for real-world scenario
    /// Centralizes payment method creation logic
    /// </summary>
    public class PaymentFactory
    {
        /// <summary>
        /// Creates payment method based on type and amount
        /// </summary>
        public IPayment CreatePayment(string type, decimal amount)
        {
            return type.ToLower() switch
            {
                "creditcard" => new CreditCardPayment(amount),
                "paypal" => new PayPalPayment(amount),
                "bank" => new BankPayment(amount),
                _ => throw new ArgumentException($"Unknown payment type: {type}")
            };
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 7: Real-World Document Factory
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Document exporter interface
    /// </summary>
    public interface IDocumentExporter
    {
        string Export(string filename);
    }

    /// <summary>
    /// PDF exporter implementation
    /// </summary>
    public class PdfExporter : IDocumentExporter
    {
        public string Export(string filename) => $"   Exported: {filename}.pdf";
    }

    /// <summary>
    /// CSV exporter implementation
    /// </summary>
    public class CsvExporter : IDocumentExporter
    {
        public string Export(string filename) => $"   Exported: {filename}.csv";
    }

    /// <summary>
    /// JSON exporter implementation
    /// </summary>
    public class JsonExporter : IDocumentExporter
    {
        public string Export(string filename) => $"   Exported: {filename}.json";
    }

    /// <summary>
    /// Document exporter factory
    /// </summary>
    public class DocumentExporterFactory
    {
        public IDocumentExporter CreateExporter(string format)
        {
            return format.ToLower() switch
            {
                "pdf" => new PdfExporter(),
                "csv" => new CsvExporter(),
                "json" => new JsonExporter(),
                _ => throw new ArgumentException($"Unknown format: {format}")
            };
        }
    }
}
