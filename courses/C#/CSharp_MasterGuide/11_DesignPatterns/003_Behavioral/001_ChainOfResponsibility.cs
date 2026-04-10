/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Chain of Responsibility
 * FILE      : 01_ChainOfResponsibility.cs
 * PURPOSE   : Demonstrates Chain of Responsibility pattern in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral
{
    /// <summary>
    /// Demonstrates Chain of Responsibility pattern
    /// </summary>
    public class ChainOfResponsibilityDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Chain of Responsibility ===\n");

            Console.WriteLine("1. Chain - Request Processing:");
            var handler1 = new AuthHandler();
            var handler2 = new ValidationHandler();
            var handler3 = new BusinessHandler();
            
            handler1.SetNext(handler2).SetNext(handler3);
            handler1.Handle("request");
            // Output: Authenticated
            // Output: Validated
            // Output: Processed

            Console.WriteLine("\n=== Chain Complete ===");
        }
    }

    public abstract class RequestHandler
    {
        protected RequestHandler _next;
        
        public RequestHandler SetNext(RequestHandler handler)
        {
            _next = handler;
            return handler;
        }
        
        public abstract void Handle(string request);
    }

    public class AuthHandler : RequestHandler
    {
        public override void Handle(string request)
        {
            Console.WriteLine("   Authenticated");
            _next?.Handle(request);
        }
    }

    public class ValidationHandler : RequestHandler
    {
        public override void Handle(string request)
        {
            Console.WriteLine("   Validated");
            _next?.Handle(request);
        }
    }

    public class BusinessHandler : RequestHandler
    {
        public override void Handle(string request)
        {
            Console.WriteLine("   Processed");
        }
    }
}