/*
 * ============================================================
 * TOPIC     : Dependency Injection
 * SUBTOPIC  : DI Real-World
 * FILE      : 03_DI_RealWorld.cs
 * PURPOSE   : Real-world Dependency Injection examples
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._13_DependencyInjection._03_RealWorld
{
    /// <summary>
    /// Real-world DI examples
    /// </summary>
    public class DIRealWorldDemo
    {
        /// <summary>
        /// Entry point for DI real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === DI Real-World ===
            Console.WriteLine("=== DI Real-World ===\n");

            // ── REAL-WORLD 1: Web API Controller ───────────────────────────────
            // Controllers receive services via DI

            // Example 1: Web API Controller
            // Output: 1. Web API Controller:
            Console.WriteLine("1. Web API Controller:");
            
            // Simulate HTTP request handling
            var userController = new UserApiController(
                new UserService(
                    new UserRepository(),
                    new EmailSender(),
                    new Logger()));
            
            userController.GetUser(123);
            // Output: API: GET /users/123
            // Output: User retrieved: John

            // ── REAL-WORLD 2: Business Logic ───────────────────────────────
            // Services with multiple dependencies

            // Example 2: Business Logic
            // Output: 2. Business Logic:
            Console.WriteLine("\n2. Business Logic:");
            
            var orderProcessor = new OrderProcessor(
                new PaymentService(),
                new InventoryService(),
                new ShippingService(),
                new NotificationService());
            
            orderProcessor.ProcessOrder(1001);
            // Output: Order 1001: Payment successful
            // Output: Order 1001: Inventory reserved
            // Output: Order 1001: Shipping arranged
            // Output: Order 1001: Customer notified

            // ── REAL-WORLD 3: Unit Testing ───────────────────────────────────
            // DI enables easy mocking

            // Example 3: Unit Testing
            // Output: 3. Unit Testing:
            Console.WriteLine("\n3. Unit Testing:");
            
            // Test with mock dependencies
            var mockRepo = new MockUserRepository();
            var mockEmail = new MockEmailService();
            
            var testService = new UserService(mockRepo, mockEmail, new MockLogger());
            testService.RegisterUser("testuser", "test@email.com");
            
            // Output: Test: User saved
            // Output: Test: Verification email sent

            Console.WriteLine("\n=== DI Real-World Complete ===");
        }
    }

    /// <summary>
    /// User API controller
    /// </summary>
    public class UserApiController
    {
        private IUserService _userService;
        
        public UserApiController(IUserService userService)
        {
            _userService = userService;
        }
        
        public void GetUser(int id)
        {
            Console.WriteLine($"   API: GET /users/{id}");
            var user = _userService.GetUser(id);
            Console.WriteLine($"   User retrieved: {user.Name}");
        }
    }

    /// <summary>
    /// User service interface
    /// </summary>
    public interface IUserService
    {
        User GetUser(int id); // method: gets user
        void RegisterUser(string name, string email); // method: registers user
    }

    /// <summary>
    /// User
    /// </summary>
    public class User
    {
        public int Id { get; set; } // property: user ID
        public string Name { get; set; } // property: user name
        public string Email { get; set; } // property: user email
    }

    /// <summary>
    /// User service
    /// </summary>
    public class UserService : IUserService
    {
        private IUserRepository _repository;
        private IEmailSender _emailSender;
        private ILogger _logger;
        
        public UserService(IUserRepository repository, IEmailSender emailSender, ILogger logger)
        {
            _repository = repository;
            _emailSender = emailSender;
            _logger = logger;
        }
        
        public User GetUser(int id)
        {
            _logger.Log($"Fetching user {id}");
            return _repository.GetById(id);
        }
        
        public void RegisterUser(string name, string email)
        {
            _repository.Save(new User { Name = name, Email = email });
            _emailSender.Send(email, "Welcome!");
            Console.WriteLine($"   User registered: {name}");
        }
    }

    /// <summary>
    /// User repository interface
    /// </summary>
    public interface IUserRepository
    {
        User GetById(int id); // method: gets user by ID
        void Save(User user); // method: saves user
    }

    /// <summary>
    /// Email sender interface
    /// </summary>
    public interface IEmailSender
    {
        void Send(string to, string message); // method: sends email
    }

    /// <summary>
    /// Logger interface
    /// </summary>
    public interface ILogger
    {
        void Log(string message); // method: logs message
    }

    /// <summary>
    /// User repository implementation
    /// </summary>
    public class UserRepository : IUserRepository
    {
        public User GetById(int id) => new User { Id = id, Name = "John" };
        public void Save(User user) { }
    }

    /// <summary>
    /// Email sender implementation
    /// </summary>
    public class EmailSender : IEmailSender
    {
        public void Send(string to, string message) => Console.WriteLine($"   Email sent to {to}");
    }

    /// <summary>
    /// Logger implementation
    /// </summary>
    public class Logger : ILogger
    {
        public void Log(string message) => Console.WriteLine($"   [Log] {message}");
    }

    /// <summary>
    /// Order processor
    /// </summary>
    public class OrderProcessor
    {
        private IPaymentService _payment;
        private IInventoryService _inventory;
        private IShippingService _shipping;
        private INotificationService _notification;
        
        public OrderProcessor(
            IPaymentService payment,
            IInventoryService inventory,
            IShippingService shipping,
            INotificationService notification)
        {
            _payment = payment;
            _inventory = inventory;
            _shipping = shipping;
            _notification = notification;
        }
        
        public void ProcessOrder(int orderId)
        {
            _payment.ProcessPayment(orderId);
            _inventory.ReserveItems(orderId);
            _shipping.ArrangeDelivery(orderId);
            _notification.NotifyCustomer(orderId);
            Console.WriteLine($"   Order {orderId}: Processed successfully");
        }
    }

    public interface IPaymentService { void ProcessPayment(int orderId); }
    public interface IInventoryService { void ReserveItems(int orderId); }
    public interface IShippingService { void ArrangeDelivery(int orderId); }
    public interface INotificationService { void NotifyCustomer(int orderId); }

    public class PaymentService : IPaymentService { public void ProcessPayment(int id) => Console.WriteLine($"   Order {id}: Payment successful"); }
    public class InventoryService : IInventoryService { public void ReserveItems(int id) => Console.WriteLine($"   Order {id}: Inventory reserved"); }
    public class ShippingService : IShippingService { public void ArrangeDelivery(int id) => Console.WriteLine($"   Order {id}: Shipping arranged"); }
    public class NotificationService : INotificationService { public void NotifyCustomer(int id) => Console.WriteLine($"   Order {id}: Customer notified"); }

    /// <summary>
    /// Mock implementations for testing
    /// </summary>
    public class MockUserRepository : IUserRepository
    {
        public User GetById(int id) => new User { Id = id, Name = "TestUser" };
        public void Save(User user) => Console.WriteLine("   Test: User saved");
    }

    public class MockEmailService : IEmailSender
    {
        public void Send(string to, string message) => Console.WriteLine("   Test: Verification email sent");
    }

    public class MockLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine($"   Test: {message}");
    }
}