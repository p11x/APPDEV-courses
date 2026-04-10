/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : NSubstitute - Mocking Library
 * FILE      : NSubstitute.cs
 * PURPOSE   : NSubstitute mocking framework usage
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._14_Testing._02_Mocking
{
    /// <summary>
    /// NSubstitute mocking demonstration
    /// </summary>
    public class NSubstituteDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== NSubstitute Testing ===\n");

            // Output: --- Basic Mock ---
            Console.WriteLine("--- Basic Mock ---");

            // Create mock
            var mockRepo = Substitute.For<IUserRepository>();
            mockRepo.Add("Alice");
            mockRepo.Add("Bob");

            // Verify calls
            Console.WriteLine("   Mock created");
            // Output: Mock created

            // Output: --- Stub Return Values ---
            Console.WriteLine("\n--- Stub Return Values ---");

            // Configure return value
            var mockService = Substitute.For<IUserService>();
            mockService.GetUser(1).Returns(new User { Id = 1, Name = "Alice" });

            var user = mockService.GetUser(1);
            Console.WriteLine($"   User: {user.Name}");
            // Output: User: Alice

            // Output: --- Raise Events ---
            Console.WriteLine("\n--- Raise Events ---");

            var notifier = Substitute.For<IEventNotifier>();
            notifier.UserCreated += (s, e) => Console.WriteLine("   Event raised");
            notifier.OnUserCreated();
            // Output: Event raised

            // Output: --- Arguments ---
            Console.WriteLine("\n--- Argument Matching ---");

            var mockCalc = Substitute.For<ICalculator>();
            mockCalc.Add(1, 2).Returns(3);

            var result = mockCalc.Add(1, 2);
            Console.WriteLine($"   Result: {result}");
            // Output: Result: 3

            Console.WriteLine("\n=== NSubstitute Complete ===");
        }
    }

    /// <summary>
    /// User entity
    /// </summary>
    public class User
    {
        public int Id { get; set; } // property: id
        public string Name { get; set; } // property: name
    }

    /// <summary>
    /// User repository interface
    /// </summary>
    public interface IUserRepository
    {
        void Add(string name); // method: add user
    }

    /// <summary>
    /// User service interface
    /// </summary>
    public interface IUserService
    {
        User GetUser(int id); // method: get user
    }

    /// <summary>
    /// Event notifier interface
    /// </summary>
    public interface IEventNotifier
    {
        event EventHandler UserCreated; // event: user created
        void OnUserCreated(); // method: raise event
    }

    /// <summary>
    /// Calculator interface
    /// </summary>
    public interface ICalculator
    {
        int Add(int a, int b); // method: add
    }
}