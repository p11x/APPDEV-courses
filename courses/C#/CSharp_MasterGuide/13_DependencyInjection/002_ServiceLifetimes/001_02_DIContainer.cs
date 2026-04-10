/*
 * ============================================================
 * TOPIC     : Dependency Injection
 * SUBTOPIC  : DI Container
 * FILE      : 02_DIContainer.cs
 * PURPOSE   : Demonstrates DI Container patterns in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._13_DependencyInjection._02_Container
{
    /// <summary>
    /// Demonstrates DI Container
    /// </summary>
    public class DIContainerDemo
    {
        /// <summary>
        /// Entry point for DI Container examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === DI Container ===
            Console.WriteLine("=== DI Container ===\n");

            // ── CONCEPT: What is DI Container? ───────────────────────────────
            // Framework that manages dependency creation and lifetime

            // Example 1: Simple Container
            // Output: 1. Simple Container:
            Console.WriteLine("1. Simple Container:");
            
            var container = new SimpleContainer();
            container.Register<IEmailService, EmailServiceImpl>();
            
            var emailService = container.Resolve<IEmailService>();
            emailService.Send("test@email.com", "Hello");
            // Output: Email sent to test@email.com: Hello

            // Example 2: Singleton vs Transient
            // Output: 2. Singleton vs Transient:
            Console.WriteLine("\n2. Singleton vs Transient:");
            
            // Singleton - same instance
            container.RegisterSingleton<ILogger, FileLogger>();
            var logger1 = container.Resolve<ILogger>();
            var logger2 = container.Resolve<ILogger>();
            // Output: Same instance: True
            Console.WriteLine($"   Same instance: {ReferenceEquals(logger1, logger2)}");
            
            // Transient - new instance each time
            var transient1 = container.Resolve<ITimeService>();
            var transient2 = container.Resolve<ITimeService>();
            // Output: Different instances: True
            Console.WriteLine($"   Different instances: {ReferenceEquals(transient1, transient2)}");

            // Example 3: Constructor Resolution
            // Output: 3. Constructor Resolution:
            Console.WriteLine("\n3. Constructor Resolution:");
            
            // Container resolves dependencies recursively
            container.Register<ICustomerRepository, CustomerRepository>();
            container.Register<IOrderService, OrderService>();
            
            var orderService = container.Resolve<IOrderService>();
            orderService.CreateOrder(123, "Customer1");
            // Output: Order created: 123
            // Output: Customer found: Customer1

            Console.WriteLine("\n=== DI Container Complete ===");
        }
    }

    /// <summary>
    /// Simple DI container
    /// </summary>
    public class SimpleContainer
    {
        private Dictionary<Type, Type> _registrations = new Dictionary<Type, Type>();
        private Dictionary<Type, object> _singletons = new Dictionary<Type, object>();
        
        public void Register<TInterface, TImplementation>()
            where TImplementation : class, TInterface, new()
        {
            _registrations[typeof(TInterface)] = typeof(TImplementation);
        }
        
        public void RegisterSingleton<TInterface, TImplementation>()
            where TImplementation : class, TInterface, new()
        {
            _registrations[typeof(TInterface)] = typeof(TImplementation);
            _singletons[typeof(TInterface)] = new TImplementation();
        }
        
        public TInterface Resolve<TInterface>()
        {
            var type = typeof(TInterface);
            
            // Return singleton if exists
            if (_singletons.ContainsKey(type))
                return (TInterface)_singletons[type];
            
            // Create new instance
            if (_registrations.TryGetValue(type, out var implType))
            {
                var instance = Activator.CreateInstance(implType);
                
                // Register as singleton if marked
                if (_singletons.ContainsKey(type))
                    _singletons[type] = instance;
                
                return (TInterface)instance;
            }
            
            throw new Exception($"Type not registered: {type}");
        }
    }

    /// <summary>
    /// Email service interface
    /// </summary>
    public interface IEmailService
    {
        void Send(string to, string message); // method: sends email
    }

    /// <summary>
    /// Email service implementation
    /// </summary>
    public class EmailServiceImpl : IEmailService
    {
        public void Send(string to, string message)
        {
            Console.WriteLine($"   Email sent to {to}: {message}");
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
    /// File logger
    /// </summary>
    public class FileLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine($"   [File] {message}");
    }

    /// <summary>
    /// Time service interface
    /// </summary>
    public interface ITimeService
    {
        DateTime Now { get; } // property: current time
    }

    /// <summary>
    /// Time service implementation
    /// </summary>
    public class TimeService : ITimeService
    {
        public DateTime Now => DateTime.Now;
    }

    /// <summary>
    /// Customer repository interface
    /// </summary>
    public interface ICustomerRepository
    {
        string GetCustomer(string id); // method: gets customer
    }

    /// <summary>
    /// Customer repository
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        public string GetCustomer(string id) => id;
    }

    /// <summary>
    /// Order service interface
    /// </summary>
    public interface IOrderService
    {
        void CreateOrder(int orderId, string customerId); // method: creates order
    }

    /// <summary>
    /// Order service with dependencies
    /// </summary>
    public class OrderService : IOrderService
    {
        private ICustomerRepository _customerRepo;
        
        public OrderService(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }
        
        public void CreateOrder(int orderId, string customerId)
        {
            var customer = _customerRepo.GetCustomer(customerId);
            Console.WriteLine($"   Order created: {orderId}");
            Console.WriteLine($"   Customer found: {customer}");
        }
    }
}