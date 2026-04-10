/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Single Responsibility Principle - Part 2
 * FILE      : 02_SRP_Part2.cs
 * PURPOSE   : Advanced SRP examples with practical refactoring
 * ============================================================
 */
using System; // Core System namespace for Console
using System.Collections.Generic; // Generic collections

namespace CSharp_MasterGuide._12_SOLID_Principles._01_SingleResponsibility._02_SRP_Part2
{
    /// <summary>
    /// Demonstrates SRP advanced examples and refactoring
    /// </summary>
    public class SRPPart2Demo
    {
        /// <summary>
        /// Entry point for SRP Part 2 examples
        /// </summary>
        public static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Refactoring Example
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== SRP Part 2 ===\n");

            // Output: --- Refactoring Example ---
            Console.WriteLine("--- Refactoring Example ---");

            // Before: God class does everything
            var badUserService = new BadUserService();
            badUserService.RegisterUser("alice", "alice@email.com");
            // Output: Validating user
            // Output: Registering user
            // Output: Sending welcome email
            // Output: Logging action

            // After: Separated services
            var validator = new UserValidator();
            var repository = new UserRepository();
            var emailService = new EmailService();
            var logger = new AppLogger();

            var goodUserService = new GoodUserService(
                validator, repository, emailService, logger);

            goodUserService.RegisterUser("bob", "bob@email.com");
            // Output: Validating user
            // Output: Registering user in database
            // Output: Sending welcome email
            // Output: Logging: User registered

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Separation Checklist
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Separation Checklist ---
            Console.WriteLine("\n--- Separation Checklist ---");

            // Check each responsibility
            var responsibilities = new List<string>
            {
                "Data persistence",
                "Business logic",
                "Validation",
                "Notification",
                "Logging",
                "Formatting"
            };

            foreach (var resp in responsibilities)
            {
                Console.WriteLine($"   {resp}");
            }
            // Output: Data persistence
            // Output: Business logic
            // Output: Validation
            // Output: Notification
            // Output: Logging
            // Output: Formatting

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Testing Benefits
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Testing Benefits ---
            Console.WriteLine("\n--- Testing Benefits ---");

            // Mock dependencies - test in isolation
            var mockRepo = new MockUserRepository();
            var mockEmail = new MockEmailService();
            var testService = new GoodUserService(
                new UserValidator(), mockRepo, mockEmail, new AppLogger());

            var result = testService.RegisterUser("test", "test@email.com");
            Console.WriteLine($"   Test result: {result}");
            // Output: Test result: Success

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Code Organization
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Code Organization ---
            Console.WriteLine("\n--- Code Organization ---");

            // Organized classes by responsibility
            Console.WriteLine("   Services/ -> Business logic");
            Console.WriteLine("   Repositories/ -> Data access");
            Console.WriteLine("   Validators/ -> Validation rules");
            Console.WriteLine("   Notifiers/ -> Notifications");
            // Output: Services/ -> Business logic
            // Output: Repositories/ -> Data access
            // Output: Validators/ -> Validation rules
            // Output: Notifiers/ -> Notifications

            Console.WriteLine("\n=== SRP Part 2 Complete ===");
        }
    }

    /// <summary>
    /// BAD: Combines all responsibilities
    /// </summary>
    public class BadUserService
    {
        public void RegisterUser(string username, string email)
        {
            // Validating - should be separate
            Console.WriteLine("   Validating user");
            // Registering - should be separate
            Console.WriteLine("   Registering user");
            // Emailing - should be separate
            Console.WriteLine("   Sending welcome email");
            // Logging - should be separate
            Console.WriteLine("   Logging action");
        }
    }

    /// <summary>
    /// GOOD: Only validates users
    /// </summary>
    public class UserValidator
    {
        public bool Validate(string username, string email)
        {
            Console.WriteLine("   Validating user");
            return true;
        }
    }

    /// <summary>
    /// GOOD: Only persists users
    /// </summary>
    public class UserRepository
    {
        public void Save(string username, string email)
        {
            Console.WriteLine("   Registering user in database");
        }
    }

    /// <summary>
    /// GOOD: Only sends emails
    /// </summary>
    public class EmailService
    {
        public void SendWelcome(string email)
        {
            Console.WriteLine("   Sending welcome email");
        }
    }

    /// <summary>
    /// GOOD: Only logs
    /// </summary>
    public class AppLogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"   Logging: {message}");
        }
    }

    /// <summary>
    /// GOOD: Coordinates other services - single responsibility
    /// </summary>
    public class GoodUserService
    {
        private readonly UserValidator _validator;
        private readonly UserRepository _repository;
        private readonly EmailService _emailService;
        private readonly AppLogger _logger;

        public GoodUserService(
            UserValidator validator,
            UserRepository repository,
            EmailService emailService,
            AppLogger logger)
        {
            _validator = validator;
            _repository = repository;
            _emailService = emailService;
            _logger = logger;
        }

        public string RegisterUser(string username, string email)
        {
            // Coordinates: delegates to specialized services
            _validator.Validate(username, email);
            _repository.Save(username, email);
            _emailService.SendWelcome(email);
            _logger.Log("User registered");
            return "Success";
        }
    }

    /// <summary>
    /// Mock repository for testing
    /// </summary>
    public class MockUserRepository
    {
        public void Save(string username, string email)
        {
            // Mock implementation - no real DB
        }
    }

    /// <summary>
    /// Mock email service for testing
    /// </summary>
    public class MockEmailService
    {
        public void SendWelcome(string email)
        {
            // Mock implementation - no real email
        }
    }
}