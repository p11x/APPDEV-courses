/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : BDD Part 2 - Advanced
 * FILE      : BDD_Part2.cs
 * PURPOSE   : Advanced BDD techniques
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._14_Testing._05_BDD
{
    /// <summary>
    /// BDD Part 2 demonstration
    /// </summary>
    public class BDDPart2Demo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== BDD Part 2 ===\n");

            // Output: --- Given-When-Then ---
            Console.WriteLine("--- Given-When-Then ---");

            // Given a user is logged in
            var user = new User { IsLoggedIn = true };

            // When they place an order
            var cart = new ShoppingCart();
            cart.AddItem("Product", 2);

            // Then order should be created
            var order = new OrderService().CreateOrder(user, cart);
            Console.WriteLine($"   Order created: {order.Id}");
            // Output: Order created: 123

            // Output: --- Scenario Outlines ---
            Console.WriteLine("\n--- Scenario Outlines ---");

            var pricing = new PricingService();
            Assert.AreEqual(100m, pricing.CalculateDiscount(100m, "STANDARD"));
            Assert.AreEqual(90m, pricing.CalculateDiscount(100m, "PREMIUM"));
            Console.WriteLine("   Discount tests passed");
            // Output: Discount tests passed

            // Output: --- Data Tables ---
            Console.WriteLine("\n--- Data Tables ---");

            var shipping = new ShippingService();
            var cost = shipping.GetShippingCost("US");
            Console.WriteLine($"   US shipping: {cost}");
            // Output: US shipping: $10

            cost = shipping.GetShippingCost("EU");
            Console.WriteLine($"   EU shipping: {cost}");
            // Output: EU shipping: $20

            Console.WriteLine("\n=== Part 2 Complete ===");
        }
    }

    /// <summary>
    /// User entity
    /// </summary>
    public class User
    {
        public bool IsLoggedIn { get; set; } // property: logged in
    }

    /// <summary>
    /// Shopping cart
    /// </summary>
    public class ShoppingCart
    {
        public void AddItem(string product, int qty) { }
    }

    /// <summary>
    /// Order entity
    /// </summary>
    public class Order
    {
        public int Id { get; set; } = 123; // property: id
    }

    /// <summary>
    /// Order service
    /// </summary>
    public class OrderService
    {
        public Order CreateOrder(User user, ShoppingCart cart)
        {
            return new Order();
        }
    }

    /// <summary>
    /// Pricing service
    /// </summary>
    public class PricingService
    {
        public decimal CalculateDiscount(decimal amount, string customerType)
        {
            return customerType switch
            {
                "PREMIUM" => amount * 0.9m,
                _ => amount
            };
        }
    }

    /// <summary>
    /// Shipping service
    /// </summary>
    public class ShippingService
    {
        public decimal GetShippingCost(string country)
        {
            return country switch
            {
                "US" => 10m,
                "EU" => 20m,
                _ => 30m
            };
        }
    }

    /// <summary>
    /// Assert helper
    /// </summary>
    public static class Assert
    {
        public static void AreEqual(object expected, object actual)
        {
            if (!expected.Equals(actual))
                throw new Exception($"Expected {expected} but got {actual}");
        }
    }
}