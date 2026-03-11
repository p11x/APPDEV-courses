/*
================================================================================
TOPIC 28: DEPENDENCY INJECTION
================================================================================

Dependency Injection is a design pattern for loose coupling.

TABLE OF CONTENTS:
1. What is DI?
2. Constructor Injection
3. Service Container
4. Benefits of DI
================================================================================
*/

using System;

namespace DIExamples
{
    // Service interface
    interface IMessageService
    {
        void Send(string message);
    }
    
    // Implementations
    class EmailService : IMessageService
    {
        public void Send(string message)
        {
            Console.WriteLine($"Email sent: {message}");
        }
    }
    
    class SmsService : IMessageService
    {
        public void Send(string message)
        {
            Console.WriteLine($"SMS sent: {message}");
        }
    }
    
    // Dependent class
    class Notification
    {
        private readonly IMessageService _service;
        
        // Constructor injection
        public Notification(IMessageService service)
        {
            _service = service;
        }
        
        public void Notify(string message)
        {
            _service.Send(message);
        }
    }
    
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Dependency Injection ===");
            
            // Inject different implementations
            IMessageService email = new EmailService();
            Notification n1 = new Notification(email);
            n1.Notify("Hello via Email");
            
            IMessageService sms = new SmsService();
            Notification n2 = new Notification(sms);
            n2.Notify("Hello via SMS");
        }
    }
}

/*
DI BENEFITS:
-----------
- Loose coupling
- Testability
- Flexibility
- Maintainability

TYPES:
------
- Constructor Injection (most common)
- Property Injection
- Method Injection
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 29 covers Entity Framework Basics.
*/
