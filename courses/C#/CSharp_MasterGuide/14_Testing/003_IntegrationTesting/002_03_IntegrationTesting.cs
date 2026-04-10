/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : Integration Testing
 * FILE      : 03_IntegrationTesting.cs
 * PURPOSE   : Demonstrates integration testing in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._14_Testing._03_IntegrationTesting
{
    /// <summary>
    /// Demonstrates integration testing
    /// </summary>
    public class IntegrationTestingDemo
    {
        /// <summary>
        /// Entry point for integration testing examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Integration Testing ===
            Console.WriteLine("=== Integration Testing ===\n");

            // ── CONCEPT: What is Integration Testing? ───────────────────────
            // Testing multiple components together

            // Example 1: Testing Component Integration
            // Output: 1. Testing Component Integration:
            Console.WriteLine("1. Testing Component Integration:");
            
            // Test whole workflow
            var workflow = new OrderWorkflow();
            var result = workflow.CreateOrder("Product", 2, "customer@email.com");
            // Output: Order workflow: Created -> Validated -> Processed -> Notified
            Console.WriteLine($"   Result: {result}");

            // Example 2: Database Integration
            // Output: 2. Database Integration:
            Console.WriteLine("\n2. Database Integration:");
            
            // Use test database
            var repo = new TestUserRepository();
            repo.Save(new User { Name = "John", Email = "john@test.com" });
            var user = repo.GetByEmail("john@test.com");
            // Output: Retrieved from DB: John (john@test.com)
            Console.WriteLine($"   Retrieved from DB: {user.Name} ({user.Email})");

            // Example 3: API Integration
            // Output: 3. API Integration:
            Console.WriteLine("\n3. API Integration:");
            
            // Test API client
            var apiClient = new TestApiClient();
            var response = apiClient.Get("/users/1");
            // Output: API response: {"id":1,"name":"John"}
            Console.WriteLine($"   API response: {response}");

            Console.WriteLine("\n=== Integration Testing Complete ===");
        }
    }

    /// <summary>
    /// Order workflow - multiple components
    /// </summary>
    public class OrderWorkflow
    {
        public string CreateOrder(string product, int quantity, string email)
        {
            // Step 1: Create order
            Console.WriteLine("   Order workflow: Created");
            
            // Step 2: Validate
            Console.WriteLine("   Order workflow: Validated");
            
            // Step 3: Process
            Console.WriteLine("   Order workflow: Processed");
            
            // Step 4: Notify
            Console.WriteLine("   Order workflow: Notified");
            
            return "Success";
        }
    }

    /// <summary>
    /// User
    /// </summary>
    public class User
    {
        public string Name { get; set; } // property: user name
        public string Email { get; set; } // property: user email
    }

    /// <summary>
    /// Test user repository - simulates DB
    /// </summary>
    public class TestUserRepository
    {
        private List<User> _users = new List<User>();
        
        public void Save(User user)
        {
            _users.Add(user);
            Console.WriteLine($"   Saved to DB: {user.Name}");
        }
        
        public User GetByEmail(string email)
        {
            return _users.Find(u => u.Email == email);
        }
    }

    /// <summary>
    /// Test API client - simulates HTTP
    /// </summary>
    public class TestApiClient
    {
        public string Get(string endpoint)
        {
            return $"{{\"id\":1,\"name\":\"John\"}}";
        }
    }
}