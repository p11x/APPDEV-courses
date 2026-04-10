/*
 * ============================================================
 * TOPIC     : Architecture
 * SUBTOPIC  : Clean Architecture Layers
 * FILE      : CleanArch_Layers.cs
 * PURPOSE   : Clean Architecture layer implementations
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._20_Architecture._01_CleanArchitecture
{
    /// <summary>
    /// Clean architecture layers demonstration
    /// </summary>
    public class CleanArchLayersDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Clean Architecture Layers ===\n");

            // Output: --- Domain Layer ---
            Console.WriteLine("--- Domain Layer ---");

            var order = new Order();
            order.AddItem("Product", 2);
            Console.WriteLine($"   Order items: {order.Items.Count}");
            // Output: Order items: 1

            // Output: --- Application Layer ---
            Console.WriteLine("\n--- Application Layer ---");

            var createOrderUseCase = new CreateOrderUseCase();
            createOrderUseCase.Execute(order);
            Console.WriteLine("   Order created");
            // Output: Order created

            // Output: --- Infrastructure Layer ---
            Console.WriteLine("\n--- Infrastructure Layer ---");

            var repo = new SqlOrderRepository();
            repo.Save(order);
            Console.WriteLine("   Saved to database");
            // Output: Saved to database

            // Output: --- Presentation Layer ---
            Console.WriteLine("\n--- Presentation Layer ---");

            var controller = new OrderController();
            controller.Create(order);
            Console.WriteLine("   Response sent");
            // Output: Response sent

            Console.WriteLine("\n=== Layers Complete ===");
        }
    }

    /// <summary>
    /// Domain entity
    /// </summary>
    public class Order
    {
        public System.Collections.Generic.List<OrderItem> Items { get; } = new();
        public void AddItem(string product, int qty) => Items.Add(new OrderItem { Product = product, Qty = qty });
    }

    /// <summary>
    /// Order item
    /// </summary>
    public class OrderItem
    {
        public string Product { get; set; } // property: product
        public int Qty { get; set; } // property: quantity
    }

    /// <summary>
    /// Application use case
    /// </summary>
    public class CreateOrderUseCase
    {
        public void Execute(Order order) { }
    }

    /// <summary>
    /// Infrastructure repository
    /// </summary>
    public class SqlOrderRepository
    {
        public void Save(Order order) { }
    }

    /// <summary>
    /// Presentation controller
    /// </summary>
    public class OrderController
    {
        public void Create(Order order) { }
    }
}