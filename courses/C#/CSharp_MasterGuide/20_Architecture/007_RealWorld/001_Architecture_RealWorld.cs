/*
 * ============================================================
 * TOPIC     : Architecture
 * SUBTOPIC  : Real-World Architecture
 * FILE      : 03_Architecture_RealWorld.cs
 * PURPOSE   : Real-world architecture examples
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._20_Architecture._03_RealWorld
{
    public class ArchitectureRealWorldDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Architecture Real-World ===\n");
            Console.WriteLine("1. Clean Architecture Layers:");
            var service = new UserService(new UserRepository());
            service.CreateUser("John", "john@email.com");
            Console.WriteLine("\n=== Architecture Real-World Complete ===");
        }
    }

    public class UserService
    {
        private IUserRepository _repo;
        public UserService(IUserRepository repo) => _repo = repo;
        public void CreateUser(string name, string email) => _repo.Save(name, email);
    }
    public interface IUserRepository { void Save(string name, string email); }
    public class UserRepository : IUserRepository { public void Save(string name, string email) => Console.WriteLine($"   Saved: {name}"); }
}