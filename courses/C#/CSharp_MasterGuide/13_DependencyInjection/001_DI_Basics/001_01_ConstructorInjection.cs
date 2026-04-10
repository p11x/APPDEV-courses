/*
 * ============================================================
 * TOPIC     : Dependency Injection
 * SUBTOPIC  : DI Basics - Constructor Injection
 * FILE      : 01_ConstructorInjection.cs
 * PURPOSE   : Constructor injection pattern in C#
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._13_DependencyInjection._01_DI_Basics
{
    /// <summary>
    /// Demonstrates constructor injection
    /// </summary>
    public class ConstructorInjection
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Constructor Injection ===\n");

            // Dependencies injected via constructor
            var logger = new ConsoleLogger();
            var service = new UserService(logger);
            
            // Output: User saved, Log: User saved
            service.SaveUser("john");

            Console.WriteLine("\n=== Constructor Injection Complete ===");
        }
    }

    /// <summary>
    /// Logger interface
    /// </summary>
    public interface ILogger
    {
        void Log(string message);
    }

    /// <summary>
    /// Console logger implementation
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine($"   Log: {message}");
    }

    /// <summary>
    /// UserService with injected logger
    /// </summary>
    public class UserService
    {
        private readonly ILogger _logger;
        
        // Constructor injection
        public UserService(ILogger logger)
        {
            _logger = logger;
        }
        
        public void SaveUser(string name)
        {
            Console.WriteLine($"   User saved");
            _logger.Log($"User saved: {name}");
        }
    }
}