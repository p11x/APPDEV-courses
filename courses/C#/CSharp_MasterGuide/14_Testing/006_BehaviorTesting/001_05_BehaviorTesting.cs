/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : Behavior Testing
 * FILE      : 05_BehaviorTesting.cs
 * PURPOSE   : Demonstrates BDD-style behavior testing
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._14_Testing._05_BehaviorTesting
{
    /// <summary>
    /// Demonstrates behavior testing
    /// </summary>
    public class BehaviorTestingDemo
    {
        /// <summary>
        /// Entry point for behavior testing examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Behavior Testing ===
            Console.WriteLine("=== Behavior Testing ===\n");

            // ── CONCEPT: Behavior-Driven Development ──────────────────────────
            // Tests describe behavior, not implementation

            // Example 1: Given-When-Then
            // Output: 1. Given-When-Then:
            Console.WriteLine("1. Given-When-Then:");
            
            // GIVEN: User with empty cart
            var cart = new BehaviorCart();
            
            // WHEN: Adding item to cart
            cart.AddItem("Widget", 10.00m);
            
            // THEN: Cart should have one item
            var itemCount = cart.Items.Count;
            var total = cart.Total;
            // Output: Cart has 1 item, total: $10.00
            Console.WriteLine($"   Cart has {itemCount} item, total: ${total:F2}");

            // Example 2: Scenario Testing
            // Output: 2. Scenario Testing:
            Console.WriteLine("\n2. Scenario Testing:");
            
            var orderSystem = new OrderSystem();
            
            // Scenario: New customer places order
            var result = orderSystem.PlaceOrder("NewCustomer", 100.00m);
            // Output: Scenario: New customer gets discount
            Console.WriteLine($"   Scenario: {result}");

            Console.WriteLine("\n=== Behavior Testing Complete ===");
        }
    }

    /// <summary>
    /// Shopping cart for behavior test
    /// </summary>
    public class BehaviorCart
    {
        public List<CartItem> Items { get; } = new List<CartItem>();
        
        public void AddItem(string name, decimal price)
        {
            Items.Add(new CartItem { Name = name, Price = price });
        }
        
        public decimal Total => Items.Sum(i => i.Price);
    }

    public class CartItem
    {
        public string Name { get; set; } // property: item name
        public decimal Price { get; set; } // property: item price
    }

    /// <summary>
    /// Order system behavior test
    /// </summary>
    public class OrderSystem
    {
        public string PlaceOrder(string customerType, decimal amount)
        {
            if (customerType == "NewCustomer")
            {
                return "New customer gets discount";
            }
            return "Regular order";
        }
    }
}