/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Chain of Responsibility Part 2
 * FILE      : 02_ChainOfResponsibility_Part2.cs
 * PURPOSE   : Demonstrates advanced Chain of Responsibility patterns
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral
{
    /// <summary>
    /// Advanced Chain of Responsibility patterns
    /// </summary>
    public class ChainOfResponsibilityPart2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Chain Part 2 ===\n");

            Console.WriteLine("1. Multiple Handlers:");
            var chain = new LogHandler().SetNext(new EmailHandler()).SetNext(new SmsHandler());
            chain.Handle("message");
            // Output: Logged
            // Output: Emailed
            // Output: SMS sent

            Console.WriteLine("\n=== Chain Part 2 Complete ===");
        }
    }

    public class LogHandler : RequestHandler
    {
        public override void Handle(string request)
        {
            Console.WriteLine("   Logged");
            _next?.Handle(request);
        }
    }

    public class EmailHandler : RequestHandler
    {
        public override void Handle(string request)
        {
            Console.WriteLine("   Emailed");
            _next?.Handle(request);
        }
    }

    public class SmsHandler : RequestHandler
    {
        public override void Handle(string request)
        {
            Console.WriteLine("   SMS sent");
        }
    }
}