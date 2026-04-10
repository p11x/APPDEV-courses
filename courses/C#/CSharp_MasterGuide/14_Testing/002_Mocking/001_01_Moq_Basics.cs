/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : Mocking Basics with Moq
 * FILE      : 01_Moq_Basics.cs
 * PURPOSE   : Using Moq framework for mocking
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._14_Testing._02_Mocking
{
    /// <summary>
    /// Moq mocking basics
    /// </summary>
    public class MoqBasics
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Moq Basics ===\n");

            // Create mock
            var mockRepo = new Mock<IUserRepository>();
            
            // Setup mock behavior
            mockRepo.Setup(r => r.GetById(1)).Returns(new User(1, "John"));
            
            // Use mock
            var user = mockRepo.Object.GetById(1);
            Console.WriteLine($"   Fetched: {user.Name}");
            
            // Verify calls
            mockRepo.Verify(r => r.GetById(1), Times.Once);

            Console.WriteLine("\n=== Moq Basics Complete ===");
        }
    }

    public interface IUserRepository
    {
        User GetById(int id);
        void Save(User user);
    }

    public class User
    {
        public int Id { get; }
        public string Name { get; }
        public User(int id, string name) { Id = id; Name = name; }
    }
}