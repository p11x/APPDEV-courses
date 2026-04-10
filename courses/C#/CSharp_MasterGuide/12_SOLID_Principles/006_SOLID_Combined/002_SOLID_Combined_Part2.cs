/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : SOLID Combined - Part 2
 * FILE      : 02_SOLID_Combined_Part2.cs
 * PURPOSE   : Advanced SOLID examples combining all principles
 * ============================================================
 */
using System; // Core System namespace for Console

namespace CSharp_MasterGuide._12_SOLID_Principles._06_SOLID_Combined._02_SOLID_Combined_Part2
{
    /// <summary>
    /// Demonstrates advanced SOLID combinations
    /// </summary>
    public class SOLIDCombinedPart2Demo
    {
        /// <summary>
        /// Entry point for SOLID combined Part 2
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== SOLID Combined Part 2 ===\n");

            // Output: --- Notification System ---
            Console.WriteLine("--- Notification System ---");

            // Combine all 5 principles
            var notifier = new NotificationService(
                new EmailProvider(),
                new MessageQueue());

            notifier.Send("Hello");
            // Output: Email sent: Hello

            Console.WriteLine("\n=== Part 2 Complete ===");
        }
    }

    public interface IMessageSender
    {
        void Send(string message); // method: send message
    }

    public interface IMessageQueue
    {
        void Enqueue(string message); // method: queue message
    }

    public class EmailProvider : IMessageSender
    {
        public void Send(string message) => Console.WriteLine($"   Email sent: {message}");
    }

    public class MessageQueue : IMessageQueue
    {
        public void Enqueue(string message) => Console.WriteLine($"   Queued: {message}");
    }

    public class NotificationService
    {
        private readonly IMessageSender _sender; // field: sender abstraction
        private readonly IMessageQueue _queue; // field: queue abstraction

        public NotificationService(IMessageSender sender, IMessageQueue queue)
        {
            _sender = sender;
            _queue = queue;
        }

        public void Send(string message)
        {
            _queue.Enqueue(message);
            _sender.Send(message);
        }
    }
}