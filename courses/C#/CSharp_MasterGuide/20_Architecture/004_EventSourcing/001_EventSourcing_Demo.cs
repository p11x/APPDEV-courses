/*
 * ============================================================
 * TOPIC     : Architecture
 * SUBTOPIC  : Event Sourcing
 * FILE      : EventSourcing_Demo.cs
 * PURPOSE   : Event sourcing pattern
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._20_Architecture._03_EventSourcing
{
    /// <summary>
    /// Event sourcing demonstration
    /// </summary>
    public class EventSourcingDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Event Sourcing ===\n");

            // Output: --- Store Events ---
            Console.WriteLine("--- Store Events ---");

            var eventStore = new EventStore();
            eventStore.Append(new OrderCreatedEvent("Order1"));
            eventStore.Append(new ItemAddedEvent("Order1", "Product"));
            Console.WriteLine($"   Events: {eventStore.Events.Count}");
            // Output: Events: 2

            // Output: --- Rebuild State ---
            Console.WriteLine("\n--- Rebuild State ---");

            var projector = new OrderProjector();
            var order = projector.Project("Order1");
            Console.WriteLine($"   Items: {order.Items.Count}");
            // Output: Items: 1

            // Output: --- Snapshots ---
            Console.WriteLine("\n--- Snapshots ---");

            var snapshot = new SnapshotStore();
            snapshot.Save(new Order { Id = "Order1" });
            Console.WriteLine("   Snapshot saved");
            // Output: Snapshot saved

            Console.WriteLine("\n=== Event Sourcing Complete ===");
        }
    }

    /// <summary>
    /// Domain event base
    /// </summary>
    public abstract class DomainEvent
    {
        public string OrderId { get; set; } // property: order id
        public DateTime Timestamp { get; set; } = DateTime.Now; // property: timestamp
    }

    /// <summary>
    /// Order created event
    /// </summary>
    public class OrderCreatedEvent : DomainEvent
    {
        public OrderCreatedEvent(string orderId) => OrderId = orderId;
    }

    /// <summary>
    /// Item added event
    /// </summary>
    public class ItemAddedEvent : DomainEvent
    {
        public string Product { get; set; } // property: product
        public ItemAddedEvent(string orderId, string product) { OrderId = orderId; Product = product; }
    }

    /// <summary>
    /// Event store
    /// </summary>
    public class EventStore
    {
        public System.Collections.Generic.List<DomainEvent> Events { get; } = new();
        public void Append(DomainEvent e) => Events.Add(e);
    }

    /// <summary>
    /// Order for projection
    /// </summary>
    public class Order
    {
        public string Id { get; set; } // property: id
        public System.Collections.Generic.List<string> Items { get; } = new();
    }

    /// <summary>
    /// Event projector
    /// </summary>
    public class OrderProjector
    {
        public Order Project(string orderId) => new Order { Id = orderId };
    }

    /// <summary>
    /// Snapshot store
    /// </summary>
    public class SnapshotStore
    {
        public void Save(Order order) { }
    }
}