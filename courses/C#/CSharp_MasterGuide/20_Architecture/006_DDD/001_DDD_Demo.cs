/*
 * ============================================================
 * TOPIC     : Architecture
 * SUBTOPIC  : Domain-Driven Design
 * FILE      : DDD_Demo.cs
 * PURPOSE   : DDD patterns and concepts
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._20_Architecture._05_DDD
{
    /// <summary>
    /// DDD demonstration
    /// </summary>
    public class DDDDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== DDD Demo ===\n");

            // Output: --- Entities ---
            Console.WriteLine("--- Entities ---");

            var order = new OrderEntity("Order1");
            Console.WriteLine($"   Order: {order.Id}");
            // Output: Order: Order1

            // Output: --- Value Objects ---
            Console.WriteLine("\n--- Value Objects ---");

            var address = new Address("123 Main St", "City");
            Console.WriteLine($"   Address: {address}");
            // Output: Address: 123 Main St, City

            // Output: --- Aggregates ---
            Console.WriteLine("\n--- Aggregates ---");

            var aggregate = new OrderAggregate();
            aggregate.AddItem(new OrderItem { Product = "Widget" });
            Console.WriteLine($"   Items: {aggregate.Items.Count}");
            // Output: Items: 1

            // Output: --- Domain Services ---
            Console.WriteLine("\n--- Domain Services ---");

            var pricing = new PricingService();
            var price = pricing.CalculatePrice(aggregate.Items);
            Console.WriteLine($"   Price: {price:C}");
            // Output: Price: $10.00

            // Output: --- Repositories ---
            Console.WriteLine("\n--- Repositories ---");

            var repo = new OrderRepository();
            repo.Save(aggregate);
            Console.WriteLine("   Order saved");
            // Output: Order saved

            Console.WriteLine("\n=== DDD Complete ===");
        }
    }

    /// <summary>
    /// Entity with identity
    /// </summary>
    public class OrderEntity
    {
        public string Id { get; } // property: id

        public OrderEntity(string id) => Id = id;
    }

    /// <summary>
    /// Value object - immutable
    /// </summary>
    public class Address
    {
        public string Street { get; } // property: street
        public string City { get; } // property: city

        public Address(string street, string city)
        {
            Street = street;
            City = city;
        }

        public override string ToString() => $"{Street}, {City}";
    }

    /// <summary>
    /// Aggregate root
    /// </summary>
    public class OrderAggregate
    {
        public System.Collections.Generic.List<OrderItem> Items { get; } = new();

        public void AddItem(OrderItem item) => Items.Add(item);
    }

    /// <summary>
    /// Order item
    /// </summary>
    public class OrderItem
    {
        public string Product { get; set; } // property: product
        public decimal Price { get; set; } = 10m; // property: price
    }

    /// <summary>
    /// Domain service
    /// </summary>
    public class PricingService
    {
        public decimal CalculatePrice(System.Collections.Generic.List<OrderItem> items)
        {
            decimal total = 0;
            foreach (var item in items)
                total += item.Price;
            return total;
        }
    }

    /// <summary>
    /// Repository interface
    /// </summary>
    public interface IOrderRepository
    {
        void Save(OrderAggregate order); // method: save
    }

    /// <summary>
    /// Repository implementation
    /// </summary>
    public class OrderRepository : IOrderRepository
    {
        public void Save(OrderAggregate order) { }
    }
}