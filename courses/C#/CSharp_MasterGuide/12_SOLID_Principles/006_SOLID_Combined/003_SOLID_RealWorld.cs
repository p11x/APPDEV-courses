/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : SOLID Combined - Real-World Application
 * FILE      : 03_SOLID_RealWorld.cs
 * PURPOSE   : Real-world SOLID application example
 * ============================================================
 */
using System; // Core System namespace for Console
using System.Collections.Generic; // Generic collections

namespace CSharp_MasterGuide._12_SOLID_Principles._06_SOLID_Combined._03_SOLID_RealWorld
{
    /// <summary>
    /// Real-world E-commerce system applying SOLID
    /// </summary>
    public class SOLIDRealWorldDemo
    {
        /// <summary>
        /// Entry point for real-world example
        /// </summary>
        public static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // E-commerce System Example
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== SOLID Real-World: E-Commerce ===\n");

            // Output: --- Product Catalog ---
            Console.WriteLine("--- Product Catalog ---");

            var catalog = new ProductCatalog();
            catalog.Add(new PhysicalProduct { Name = "Book", Price = 20m });
            catalog.Add(new DigitalProduct { Name = "E-Book", Price = 10m });

            foreach (var product in catalog.GetAll())
            {
                Console.WriteLine($"   {product.Name}: {product.Price:C}");
            }
            // Output: Book: $20.00
            // Output: E-Book: $10.00

            // Output: --- Shopping Cart ---
            Console.WriteLine("\n--- Shopping Cart ---");

            var cart = new ShoppingCart(catalog);
            cart.AddItem("Book", 2);
            cart.AddItem("E-Book", 1);

            Console.WriteLine($"   Total: {cart.Total():C}");
            // Output: Total: $50.00

            // Output: --- Checkout ---
            Console.WriteLine("\n--- Checkout ---");

            var checkout = new CheckoutService(
                new CreditCardPaymentGateway(),
                new InventoryService(),
                new ShippingService());

            checkout.Checkout(cart, "123 Main St");
            // Output: Payment: $50.00
            // Output: Inventory checked
            // Output: Shipping to: 123 Main St

            Console.WriteLine("\n=== Real-World Complete ===");
        }
    }

    // SRP: Product classes - single responsibility
    public interface IProduct
    {
        string Name { get; } // property: name
        decimal Price { get; } // property: price
    }

    public class PhysicalProduct : IProduct
    {
        public string Name { get; set; } // property: name
        public decimal Price { get; set; } // property: price
    }

    public class DigitalProduct : IProduct
    {
        public string Name { get; set; } // property: name
        public decimal Price { get; set; } // property: price
    }

    // SRP: Catalog - single responsibility
    public class ProductCatalog
    {
        private readonly List<IProduct> _products = new(); // list: products

        public void Add(IProduct product) => _products.Add(product);
        public IEnumerable<IProduct> GetAll() => _products;
    }

    // SRP: Cart - single responsibility
    public class ShoppingCart
    {
        private readonly ProductCatalog _catalog; // field: catalog
        private readonly Dictionary<string, int> _items = new(); // dict: items

        public ShoppingCart(ProductCatalog catalog)
        {
            _catalog = catalog;
        }

        public void AddItem(string productName, int quantity)
        {
            _items[productName] = quantity;
        }

        public decimal Total()
        {
            decimal total = 0;
            foreach (var item in _items)
            {
                var product = _catalog.GetAll().FirstOrDefault(p => p.Name == item.Key);
                if (product != null)
                    total += product.Price * item.Value;
            }
            return total;
        }
    }

    // OCP: Payment gateway - open for extension
    public interface IPaymentGateway
    {
        void ProcessPayment(decimal amount); // method: process payment
    }

    public class CreditCardPaymentGateway : IPaymentGateway
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"   Payment: {amount:C}");
        }
    }

    public class PayPalPaymentGateway : IPaymentGateway
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"   PayPal: {amount:C}");
        }
    }

    // LSP: Inventory service - substitutable
    public interface IInventoryService
    {
        bool CheckAvailability(string product); // method: check availability
    }

    public class InventoryService : IInventoryService
    {
        public bool CheckAvailability(string product) => true;
    }

    // ISP: Small interfaces
    public interface IShippingService
    {
        void ShipTo(string address); // method: ship to address
    }

    public class ShippingService : IShippingService
    {
        public void ShipTo(string address) => Console.WriteLine($"   Shipping to: {address}");
    }

    // DIP: Depends on abstractions
    public class CheckoutService
    {
        private readonly IPaymentGateway _paymentGateway; // field: payment abstraction
        private readonly IInventoryService _inventoryService; // field: inventory abstraction
        private readonly IShippingService _shippingService; // field: shipping abstraction

        public CheckoutService(
            IPaymentGateway paymentGateway,
            IInventoryService inventoryService,
            IShippingService shippingService)
        {
            _paymentGateway = paymentGateway;
            _inventoryService = inventoryService;
            _shippingService = shippingService;
        }

        public void Checkout(ShoppingCart cart, string shippingAddress)
        {
            _paymentGateway.ProcessPayment(cart.Total());
            Console.WriteLine("   Inventory checked");
            _shippingService.ShipTo(shippingAddress);
        }
    }
}