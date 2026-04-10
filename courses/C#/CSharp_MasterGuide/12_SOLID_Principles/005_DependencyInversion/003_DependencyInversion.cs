/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Dependency Inversion Principle
 * FILE      : 05_DependencyInversion.cs
 * PURPOSE   : Demonstrates DIP - depend on abstractions, not concretions
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._12_SOLID_Principles._05_DependencyInversion
{
    /// <summary>
    /// Demonstrates Dependency Inversion Principle
    /// </summary>
    public class DependencyInversionDemo
    {
        /// <summary>
        /// Entry point for DIP examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Dependency Inversion Principle ===
            Console.WriteLine("=== Dependency Inversion Principle ===\n");

            // ── CONCEPT: What is DIP? ────────────────────────────────────────
            // High-level modules should not depend on low-level modules

            // Example 1: Violating DIP
            // Output: 1. Violating DIP:
            Console.WriteLine("1. Violating DIP:");
            
            // Tightly coupled - hard to change data source
            var badService = new BadOrderService();
            badService.CreateOrder("Product", 1);
            // Output: Order created with SQL Database

            // Example 2: Following DIP
            // Output: 2. Following DIP:
            Console.WriteLine("\n2. Following DIP:");
            
            // Depend on abstraction (interface), not concrete class
            var goodService = new GoodOrderService(new SQLOrderRepository());
            goodService.CreateOrder("Product", 1);
            // Output: Order created with SQL Database
            
            // Can easily switch to different repository
            goodService = new GoodOrderService(new MongoOrderRepository());
            goodService.CreateOrder("Product", 1);
            // Output: Order created with MongoDB

            // ── CONCEPT: Dependency Injection ───────────────────────────────
            // Pass dependencies through constructor

            // Example 3: Dependency Injection
            // Output: 3. Dependency Injection:
            Console.WriteLine("\n3. Dependency Injection:");
            
            // Constructor injection
            var controller = new UserController(
                new UserService(
                    new UserRepository(),
                    new EmailSender()));
            
            controller.CreateUser("john", "john@email.com");
            // Output: User created and email sent

            // ── REAL-WORLD EXAMPLE: Logger Abstraction ──────────────────────
            // Output: --- Real-World: Logger Abstraction ---
            Console.WriteLine("\n--- Real-World: Logger Abstraction ---");
            
            var appWithFileLogger = new Application(new FileLogger());
            appWithFileLogger.Run();
            // Output: App running with FileLogger
            
            var appWithCloudLogger = new Application(new CloudLogger());
            appWithCloudLogger.Run();
            // Output: App running with CloudLogger

            Console.WriteLine("\n=== DIP Complete ===");
        }
    }

    /// <summary>
    /// BAD: Direct dependency on concrete class
    /// </summary>
    public class BadOrderService
    {
        private SQLOrderRepository _repository = new SQLOrderRepository();
        
        public void CreateOrder(string product, int quantity)
        {
            _repository.Save(product, quantity);
            Console.WriteLine("   Order created with SQL Database");
        }
    }

    /// <summary>
    /// BAD: Concrete implementation
    /// </summary>
    public class SQLOrderRepository
    {
        public void Save(string product, int quantity)
        {
            Console.WriteLine("   Order created with SQL Database");
        }
    }

    /// <summary>
    /// GOOD: Abstract interface
    /// </summary>
    public interface IOrderRepository
    {
        void Save(string product, int quantity); // method: saves order
    }

    /// <summary>
    /// GOOD: Depends on abstraction
    /// </summary>
    public class GoodOrderService
    {
        private IOrderRepository _repository;
        
        public GoodOrderService(IOrderRepository repository)
        {
            _repository = repository;
        }
        
        public void CreateOrder(string product, int quantity)
        {
            _repository.Save(product, quantity);
        }
    }

    /// <summary>
    /// SQL implementation
    /// </summary>
    public class SQLOrderRepository : IOrderRepository
    {
        public void Save(string product, int quantity)
        {
            Console.WriteLine("   Order created with SQL Database");
        }
    }

    /// <summary>
    /// MongoDB implementation
    /// </summary>
    public class MongoOrderRepository : IOrderRepository
    {
        public void Save(string product, int quantity)
        {
            Console.WriteLine("   Order created with MongoDB");
        }
    }

    /// <summary>
    /// User repository interface
    /// </summary>
    public interface IUserRepository
    {
        void Save(string username, string email); // method: saves user
    }

    /// <summary>
    /// User repository implementation
    /// </summary>
    public class UserRepository : IUserRepository
    {
        public void Save(string username, string email)
        {
            Console.WriteLine("   User saved to database");
        }
    }

    /// <summary>
    /// Email sender interface
    /// </summary>
    public interface IEmailSender
    {
        void Send(string to, string message); // method: sends email
    }

    /// <summary>
    /// Email sender implementation
    /// </summary>
    public class EmailSender : IEmailSender
    {
        public void Send(string to, string message)
        {
            Console.WriteLine("   Email sent");
        }
    }

    /// <summary>
    /// User service with DI
    /// </summary>
    public class UserService
    {
        private IUserRepository _repository;
        private IEmailSender _emailSender;
        
        public UserService(IUserRepository repository, IEmailSender emailSender)
        {
            _repository = repository;
            _emailSender = emailSender;
        }
        
        public void CreateUser(string username, string email)
        {
            _repository.Save(username, email);
            _emailSender.Send(email, "Welcome!");
            Console.WriteLine("   User created and email sent");
        }
    }

    /// <summary>
    /// User controller with DI
    /// </summary>
    public class UserController
    {
        private UserService _userService;
        
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        
        public void CreateUser(string username, string email)
        {
            _userService.CreateUser(username, email);
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
    /// File logger implementation
    /// </summary>
    public class FileLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine("   App running with FileLogger");
    }

    /// <summary>
    /// Cloud logger implementation
    /// </summary>
    public class CloudLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine("   App running with CloudLogger");
    }

    /// <summary>
    /// Application depending on abstraction
    /// </summary>
    public class Application
    {
        private ILogger _logger;
        
        public Application(ILogger logger)
        {
            _logger = logger;
        }
        
        public void Run()
        {
            _logger.Log("Application running");
        }
    }
}