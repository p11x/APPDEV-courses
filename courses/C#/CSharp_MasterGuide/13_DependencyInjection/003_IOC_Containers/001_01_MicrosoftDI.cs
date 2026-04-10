/*
 * ============================================================
 * TOPIC     : Dependency Injection
 * SUBTOPIC  : IOC Containers - Microsoft DI
 * FILE      : 01_MicrosoftDI.cs
 * PURPOSE   : Microsoft.Extensions.DependencyInjection
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._13_DependencyInjection._03_IOC_Containers
{
    /// <summary>
    /// Demonstrates Microsoft DI container
    /// </summary>
    public class MicrosoftDIBasics
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Microsoft DI ===\n");

            // Register services
            var services = new ServiceCollection();
            services.AddSingleton<ILogger, ConsoleLogger>();
            services.AddTransient<IUserService, UserService>();
            
            // Build provider
            var provider = services.BuildServiceProvider();
            
            // Resolve services
            var userService = provider.GetService<IUserService>();
            userService.Save("john");

            Console.WriteLine("\n=== Microsoft DI Complete ===");
        }
    }

    public interface ILogger { void Log(string msg); }
    public interface IUserService { void Save(string name); }
    public class ConsoleLogger : ILogger { public void Log(string m) => Console.WriteLine($"   {m}"); }
    public class UserService : IUserService
    {
        private readonly ILogger _logger;
        public UserService(ILogger logger) => _logger = logger;
        public void Save(string name) { Console.WriteLine($"   Saved: {name}"); _logger.Log($"Saved {name}"); }
    }
}