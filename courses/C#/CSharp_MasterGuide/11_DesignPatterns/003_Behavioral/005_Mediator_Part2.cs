/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Mediator Part 2
 * FILE      : 07_Mediator_Part2.cs
 * PURPOSE   : Demonstrates advanced Mediator patterns in C#
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral
{
    /// <summary>
    /// Advanced Mediator patterns
    /// </summary>
    public class MediatorPart2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Mediator Part 2 ===\n");

            Console.WriteLine("1. Event Aggregator:");
            var eventAggregator = new EventAggregator();
            eventAggregator.Subscribe("OrderPlaced", msg => Console.WriteLine($"   Handler 1: {msg}"));
            eventAggregator.Subscribe("OrderPlaced", msg => Console.WriteLine($"   Handler 2: {msg}"));
            eventAggregator.Publish("OrderPlaced", "Order #123");
            // Output: Handler 1: Order #123
            // Output: Handler 2: Order #123

            Console.WriteLine("\n=== Mediator Part 2 Complete ===");
        }
    }

    public class EventAggregator
    {
        private Dictionary<string, List<Action<string>>> _handlers = new();
        
        public void Subscribe(string eventName, Action<string> handler)
        {
            if (!_handlers.ContainsKey(eventName))
                _handlers[eventName] = new List<Action<string>>();
            _handlers[eventName].Add(handler);
        }
        
        public void Publish(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                foreach (var handler in _handlers[eventName])
                    handler(message);
            }
        }
    }
}