/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : Mocking Real-World
 * FILE      : Mocking_RealWorld.cs
 * PURPOSE   : Real-world mocking examples
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._14_Testing._02_Mocking
{
    /// <summary>
    /// Real-world mocking demonstration
    /// </summary>
    public class MockingRealWorldDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Real-World Mocking ===\n");

            // Output: --- External API ---
            Console.WriteLine("--- External API ---");

            // Mock external service
            var mockApi = new ExternalPaymentApi();
            var result = mockApi.ProcessPayment("order1", 100m);
            Console.WriteLine($"   Payment: {result}");
            // Output: Payment: Success

            // Output: --- Email Service ---
            Console.WriteLine("\n--- Email Service ---");

            // Mock email sender
            var mockEmail = Substitute.For<IEmailSender>();
            mockEmail.Send("test@example.com", "Subject", "Body");

            Console.WriteLine("   Email sent");
            // Output: Email sent

            // Output: --- Repository ---
            Console.WriteLine("\n--- Repository ---");

            // Mock database
            var mockDb = Substitute.For<IUserRepository>();
            mockDb.GetById(1).Returns(new User { Id = 1, Name = "Test User" });
            mockDb.Save(Arg.Any<User>());

            var dbUser = mockDb.GetById(1);
            Console.WriteLine($"   User from mock: {dbUser.Name}");
            // Output: User from mock: Test User

            Console.WriteLine("\n=== Real-World Complete ===");
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
    /// Interface for external payment API
    /// </summary>
    public interface IExternalPaymentApi
    {
        string ProcessPayment(string orderId, decimal amount); // method: process payment
    }

    /// <summary>
    /// External payment API implementation
    /// </summary>
    public class ExternalPaymentApi : IExternalPaymentApi
    {
        public string ProcessPayment(string orderId, decimal amount)
        {
            return "Success";
        }
    }

    /// <summary>
    /// Email sender interface
    /// </summary>
    public interface IEmailSender
    {
        void Send(string to, string subject, string body); // method: send email
    }

    /// <summary>
    /// User repository interface
    /// </summary>
    public interface IUserRepository
    {
        User GetById(int id); // method: get user
        void Save(User user); // method: save user
    }
}