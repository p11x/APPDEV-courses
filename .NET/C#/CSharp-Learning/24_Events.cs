/*
================================================================================
TOPIC 24: EVENTS
================================================================================

Events enable the observer pattern for loose coupling between components.

TABLE OF CONTENTS:
1. What are Events?
2. Declaring Events
3. Subscribing to Events
4. Raising Events
================================================================================
*/

using System;

namespace EventExamples
{
    // Publisher class
    class Publisher
    {
        // Event declaration
        public event EventHandler<string> OnMessage;
        
        public void Publish(string message)
        {
            Console.WriteLine($"Publishing: {message}");
            
            // Raise event if anyone subscribed
            OnMessage?.Invoke(this, message);
        }
    }
    
    // Subscriber
    class Subscriber
    {
        public string Name { get; }
        
        public Subscriber(string name) => Name = name;
        
        // Event handler
        public void HandleMessage(object sender, string message)
        {
            Console.WriteLine($"{Name} received: {message}");
        }
    }
    
    class Program
    {
        static void Main()
        {
            Publisher pub = new Publisher();
            Subscriber sub1 = new Subscriber("Subscriber 1");
            Subscriber sub2 = new Subscriber("Subscriber 2");
            
            // Subscribe to event
            pub.OnMessage += sub1.HandleMessage;
            pub.OnMessage += sub2.HandleMessage;
            
            // Trigger event
            pub.Publish("Hello World!");
            
            // Unsubscribe
            pub.OnMessage -= sub2.HandleMessage;
            
            Console.WriteLine("\nAfter unsubscribing:");
            pub.Publish("Second message");
        }
    }
}

/*
EVENT PATTERN:
--------------
- Publisher: Has event, raises it
- Subscriber: Handles the event
- EventHandler: Delegate type for events
- +=: Subscribe
- -=: Unsubscribe
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 25 covers Lambda Expressions.
*/
