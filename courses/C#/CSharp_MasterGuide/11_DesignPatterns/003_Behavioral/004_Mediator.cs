/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Mediator Pattern
 * FILE      : 06_Mediator.cs
 * PURPOSE   : Demonstrates Mediator design pattern in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral
{
    /// <summary>
    /// Demonstrates Mediator pattern
    /// </summary>
    public class MediatorDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Mediator Pattern ===\n");

            Console.WriteLine("1. Mediator - Centralized Communication:");
            var chatroom = new ChatRoom();
            var user1 = new User("Alice", chatroom);
            var user2 = new User("Bob", chatroom);
            
            user1.Send("Hello Bob!");
            user2.Send("Hi Alice!");
            // Output: Alice: Hello Bob!
            // Output: Bob: Hi Alice!

            Console.WriteLine("\n=== Mediator Complete ===");
        }
    }

    public interface IChatMediator
    {
        void SendMessage(string message, User sender);
        void AddUser(User user);
    }

    public class ChatRoom : IChatMediator
    {
        private List<User> _users = new();
        
        public void AddUser(User user) => _users.Add(user);
        
        public void SendMessage(string message, User sender)
        {
            foreach (var user in _users)
            {
                if (user != sender)
                    user.Receive(message, sender.Name);
            }
        }
    }

    public class User
    {
        public string Name { get; }
        private IChatMediator _mediator;
        
        public User(string name, IChatMediator mediator)
        {
            Name = name;
            _mediator = mediator;
            mediator.AddUser(this);
        }
        
        public void Send(string message)
        {
            Console.WriteLine($"   {Name}: {message}");
            _mediator.SendMessage(message, this);
        }
        
        public void Receive(string message, string from)
        {
            Console.WriteLine($"   {from} to {Name}: {message}");
        }
    }
}