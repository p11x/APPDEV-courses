/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Single Responsibility - Real-World Example
 * FILE      : 02_SRP_RealWorld.cs
 * PURPOSE   : Real-world SRP demonstration - User service split
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._12_SOLID_Principles._01_SingleResponsibility._02_SRP_RealWorld
{
    /// <summary>
    /// Demonstrates SRP with real-world user management
    /// </summary>
    public class SRPRealWorld
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== SRP Real-World ===\n");

            // Each class has ONE responsibility
            var validator = new UserValidator();
            var repository = new UserRepository();
            var logger = new UserLogger();
            
            var user = new User("john@example.com", "John Doe");
            
            // Validation handles validation
            if (validator.Validate(user))
            {
                // Repository handles persistence
                repository.Save(user);
                // Logger handles logging
                logger.Log($"User created: {user.Email}");
            }

            Console.WriteLine("\n=== SRP Real-World Complete ===");
        }
    }

    /// <summary>
    /// User entity
    /// </summary>
    public class User
    {
        public string Email { get; }
        public string Name { get; }
        
        public User(string email, string name)
        {
            Email = email;
            Name = name;
        }
    }

    /// <summary>
    /// UserValidator - ONLY validates users
    /// </summary>
    public class UserValidator
    {
        public bool Validate(User user)
        {
            Console.WriteLine("   Validating user...");
            return !string.IsNullOrEmpty(user.Email);
        }
    }

    /// <summary>
    /// UserRepository - ONLY persists users
    /// </summary>
    public class UserRepository
    {
        public void Save(User user)
        {
            Console.WriteLine($"   Saving user: {user.Name}");
        }
    }

    /// <summary>
    /// UserLogger - ONLY logs user events
    /// </summary>
    public class UserLogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"   LOG: {message}");
        }
    }
}