/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : Integration Testing Part 2
 * FILE      : IntegrationTest_Part2.cs
 * PURPOSE   : Advanced integration testing
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._14_Testing._03_IntegrationTesting
{
    /// <summary>
    /// Integration testing Part 2
    /// </summary>
    public class IntegrationTestPart2Demo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Integration Test Part 2 ===\n");

            // Output: --- Database Integration ---
            Console.WriteLine("--- Database Integration ---");

            var db = new InMemoryDatabase();
            db.Connect();
            db.Execute("CREATE TABLE users");
            Console.WriteLine("   Database connected");
            // Output: Database connected

            // Output: --- API Integration ---
            Console.WriteLine("\n--- API Integration ---");

            var api = new RestApiClient();
            var response = api.Get("users/1");
            Console.WriteLine($"   Response: {response}");
            // Output: Response: {"id":1,"name":"John"}

            // Output: --- Service Integration ---
            Console.WriteLine("\n--- Service Integration ---");

            var service = new UserService(new InMemoryUserStore());
            service.CreateUser("Alice");
            var user = service.GetUser("Alice");
            Console.WriteLine($"   User: {user.Name}");
            // Output: User: Alice

            Console.WriteLine("\n=== Part 2 Complete ===");
        }
    }

    /// <summary>
    /// In-memory database
    /// </summary>
    public class InMemoryDatabase
    {
        public void Connect() => Console.WriteLine("   Connecting to database");
        public void Execute(string sql) => Console.WriteLine($"   Executing: {sql}");
    }

    /// <summary>
    /// REST API client
    /// </summary>
    public class RestApiClient
    {
        public string Get(string endpoint) => $"{{\"id\":1,\"name\":\"John\"}}";
    }

    /// <summary>
    /// User store
    /// </summary>
    public class InMemoryUserStore
    {
        public void Save(string name) { }
        public string Get(string name) => name;
    }

    /// <summary>
    /// User service
    /// </summary>
    public class UserService
    {
        private readonly InMemoryUserStore _store;

        public UserService(InMemoryUserStore store)
        {
            _store = store;
        }

        public void CreateUser(string name)
        {
            _store.Save(name);
        }

        public User GetUser(string name)
        {
            return new User { Name = _store.Get(name) };
        }
    }

    /// <summary>
    /// User entity
    /// </summary>
    public class User
    {
        public string Name { get; set; } // property: name
    }
}