/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : Unit Testing Real-World
 * FILE      : UnitTest_RealWorld.cs
 * PURPOSE   : Real-world unit testing examples
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._14_Testing._01_UnitTesting
{
    /// <summary>
    /// Real-world unit testing demonstration
    /// </summary>
    public class UnitTestRealWorldDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Real-World Unit Testing ===\n");

            // Output: --- Order Processing ---
            Console.WriteLine("--- Order Processing ---");

            var orderService = new OrderService();
            var result = orderService.PlaceOrder("Product", 2, 100m);
            Console.WriteLine($"   Order placed: {result}");
            // Output: Order placed: True

            // Output: --- Validation ---
            Console.WriteLine("\n--- Validation ---");

            var validator = new OrderValidator();
            var isValid = validator.Validate("Product", 2, 100m);
            Console.WriteLine($"   Valid: {isValid}");
            // Output: Valid: True

            isValid = validator.Validate("", 0, 0m);
            Console.WriteLine($"   Invalid: {isValid}");
            // Output: Invalid: False

            // Output: --- Pricing ---
            Console.WriteLine("\n--- Pricing ---");

            var pricing = new PricingService();
            var total = pricing.CalculateTotal(100m, 2, "STANDARD");
            Console.WriteLine($"   Total: {total:C}");
            // Output: Total: $200.00

            total = pricing.CalculateTotal(100m, 2, "EXPRESS");
            Console.WriteLine($"   Express: {total:C}");
            // Output: Express: $240.00

            // Output: --- Inventory ---
            Console.WriteLine("\n--- Inventory ---");

            var inventory = new InventoryService();
            var available = inventory.CheckStock("Product", 2);
            Console.WriteLine($"   Available: {available}");
            // Output: Available: True

            available = inventory.CheckStock("Product", 1000);
            Console.WriteLine($"   Not available: {available}");
            // Output: Not available: False

            Console.WriteLine("\n=== Real-World Complete ===");
        }
    }

    /// <summary>
    /// Order service - real business logic
    /// </summary>
    public class OrderService
    {
        public bool PlaceOrder(string product, int quantity, decimal price)
        {
            var validator = new OrderValidator();
            if (!validator.Validate(product, quantity, price))
                return false;

            var inventory = new InventoryService();
            if (!inventory.CheckStock(product, quantity))
                return false;

            Console.WriteLine($"   Order placed for {quantity} {product}");
            return true;
        }
    }

    /// <summary>
    /// Order validator
    /// </summary>
    public class OrderValidator
    {
        public bool Validate(string product, int quantity, decimal price)
        {
            return !string.IsNullOrEmpty(product) && quantity > 0 && price > 0;
        }
    }

    /// <summary>
    /// Pricing service
    /// </summary>
    public class PricingService
    {
        public decimal CalculateTotal(decimal unitPrice, int quantity, string shippingType)
        {
            var subtotal = unitPrice * quantity;
            var shipping = shippingType switch
            {
                "STANDARD" => 0m,
                "EXPRESS" => 40m,
                _ => 20m
            };
            return subtotal + shipping;
        }
    }

    /// <summary>
    /// Inventory service
    /// </summary>
    public class InventoryService
    {
        private readonly int _stock = 100;

        public bool CheckStock(string product, int quantity)
        {
            return quantity <= _stock;
        }
    }
}