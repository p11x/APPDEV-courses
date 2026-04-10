/*
 * ============================================================
 * TOPIC     : Miscellaneous
 * SUBTOPIC  : Best Practices
 * FILE      : 02_BestPracticesDemo.cs
 * PURPOSE   : Demonstrates C# best practices
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._24_Miscellaneous._02_BestPractices
{
    public class BestPracticesDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Best Practices Demo ===\n");
            
            Console.WriteLine("1. Use Interfaces:");
            var service = new UserService(new UserRepository());
            Console.WriteLine("   Dependency on abstraction, not concretion");
            
            Console.WriteLine("\n2. Use var for Implicit Types:");
            var message = "Hello";
            Console.WriteLine($"   var message = \"Hello\" (type inferred)");
            
            Console.WriteLine("\n=== Best Practices Complete ===");
        }
    }

    public interface IUserRepository { void Save(string name); }
    public class UserRepository : IUserRepository { public void Save(string name) { } }
    public class UserService
    {
        private IUserRepository _repo;
        public UserService(IUserRepository repo) => _repo = repo;
    }
}