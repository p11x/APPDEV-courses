/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : Mocking
 * FILE      : 02_Mocking.cs
 * PURPOSE   : Demonstrates mocking in unit testing
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._14_Testing._02_Mocking
{
    /// <summary>
    /// Demonstrates mocking
    /// </summary>
    public class MockingDemo
    {
        /// <summary>
        /// Entry point for mocking examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Mocking ===
            Console.WriteLine("=== Mocking ===\n");

            // ── CONCEPT: What is Mocking? ─────────────────────────────────────
            // Creating fake objects to replace real dependencies

            // Example 1: Manual Mock
            // Output: 1. Manual Mock:
            Console.WriteLine("1. Manual Mock:");
            
            // Use mock instead of real implementation
            var mockRepo = new MockUserRepository();
            var service = new UserService(mockRepo);
            
            var user = service.GetUser(1);
            // Output: Mock: Returning user ID 1
            Console.WriteLine($"   User: {user.Name}");

            // Example 2: Mock Behavior
            // Output: 2. Mock Behavior:
            Console.WriteLine("\n2. Mock Behavior:");
            
            // Configure mock to return specific values
            var mockPayment = new MockPaymentGateway();
            mockPayment.ShouldSucceed = true;
            
            var orderService = new OrderService(mockPayment);
            var result = orderService.Process(100);
            // Output: Payment successful: True
            Console.WriteLine($"   Payment successful: {result}");

            // Example 3: Verifying Interactions
            // Output: 3. Verifying Interactions:
            Console.WriteLine("\n3. Verifying Interactions:");
            
            // Check that methods were called
            var mockLogger = new MockLogger();
            var processor = new DataProcessor(mockLogger);
            
            processor.Process("test data");
            // Output: Process called, Log called
            Console.WriteLine($"   Log call count: {mockLogger.CallCount}");

            Console.WriteLine("\n=== Mocking Complete ===");
        }
    }

    /// <summary>
    /// User repository interface
    /// </summary>
    public interface IUserRepository
    {
        User GetUser(int id); // method: gets user
        void Save(User user); // method: saves user
    }

    /// <summary>
    /// User class
    /// </summary>
    public class User
    {
        public int Id { get; set; } // property: user ID
        public string Name { get; set; } // property: user name
    }

    /// <summary>
    /// Manual mock for user repository
    /// </summary>
    public class MockUserRepository : IUserRepository
    {
        public User GetUser(int id)
        {
            Console.WriteLine($"   Mock: Returning user ID {id}");
            return new User { Id = id, Name = "Mock User" };
        }
        
        public void Save(User user) { }
    }

    /// <summary>
    /// User service
    /// </summary>
    public class UserService
    {
        private IUserRepository _repository;
        
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        
        public User GetUser(int id) => _repository.GetUser(id);
    }

    /// <summary>
    /// Payment gateway interface
    /// </summary>
    public interface IPaymentGateway
    {
        bool ProcessPayment(decimal amount); // method: processes payment
    }

    /// <summary>
    /// Mock payment gateway
    /// </summary>
    public class MockPaymentGateway : IPaymentGateway
    {
        public bool ShouldSucceed { get; set; }
        
        public bool ProcessPayment(decimal amount)
        {
            return ShouldSucceed;
        }
    }

    /// <summary>
    /// Order service
    /// </summary>
    public class OrderService
    {
        private IPaymentGateway _paymentGateway;
        
        public OrderService(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;
        }
        
        public bool Process(decimal amount)
        {
            return _paymentGateway.ProcessPayment(amount);
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
    /// Mock logger
    /// </summary>
    public class MockLogger : ILogger
    {
        public int CallCount { get; private set; }
        
        public void Log(string message)
        {
            CallCount++;
            Console.WriteLine($"   Mock: {message}");
        }
    }

    /// <summary>
    /// Data processor
    /// </summary>
    public class DataProcessor
    {
        private ILogger _logger;
        
        public DataProcessor(ILogger logger)
        {
            _logger = logger;
        }
        
        public void Process(string data)
        {
            Console.WriteLine($"   Process called");
            _logger.Log("Processed data");
        }
    }
}