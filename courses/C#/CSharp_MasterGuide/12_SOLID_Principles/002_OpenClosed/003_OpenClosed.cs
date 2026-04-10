/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Open/Closed Principle
 * FILE      : 02_OpenClosed.cs
 * PURPOSE   : Demonstrates OCP - open for extension, closed for modification
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List<T>

namespace CSharp_MasterGuide._12_SOLID_Principles._02_OpenClosed
{
    /// <summary>
    /// Demonstrates Open/Closed Principle
    /// </summary>
    public class OpenClosedDemo
    {
        /// <summary>
        /// Entry point for OCP examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Open/Closed Principle ===
            Console.WriteLine("=== Open/Closed Principle ===\n");

            // ── CONCEPT: What is OCP? ────────────────────────────────────────
            // Open for extension, closed for modification

            // Example 1: Violating OCP
            // Output: 1. Violating OCP:
            Console.WriteLine("1. Violating OCP:");
            
            // Adding new discount type requires modifying existing code
            var calculator = new BadDiscountCalculator();
            var regular = calculator.Calculate(100.00m, "Regular");
            var silver = calculator.Calculate(100.00m, "Silver");
            var gold = calculator.Calculate(100.00m, "Gold");
            
            // Output: Regular discount: $90.00
            // Output: Silver discount: $80.00
            // Output: Gold discount: $70.00
            
            // Problem: Must modify Calculator to add new types

            // Example 2: Following OCP
            // Output: 2. Following OCP:
            Console.WriteLine("\n2. Following OCP:");
            
            // Use inheritance - add new discounts without modifying existing
            var discounts = new List<IDiscount>
            {
                new RegularDiscount(),
                new SilverDiscount(),
                new GoldDiscount()
            };
            
            foreach (var discount in discounts)
            {
                var price = discount.Apply(100.00m);
                // Output: Discount applied: $90.00
                Console.WriteLine($"   {discount.GetType().Name}: ${price:F2}");
            }

            // ── CONCEPT: Strategy Pattern with OCP ──────────────────────────
            // Use strategy for extensions

            // Example 3: Strategy Pattern
            // Output: 3. Strategy Pattern:
            Console.WriteLine("\n3. Strategy Pattern:");
            
            var paymentProcessor = new PaymentProcessor();
            
            paymentProcessor.SetStrategy(new CreditCardStrategy());
            paymentProcessor.Process(50.00m);
            // Output: Credit card: $50.00
            
            paymentProcessor.SetStrategy(new PayPalStrategy());
            paymentProcessor.Process(50.00m);
            // Output: PayPal: $50.00
            
            // Add new payment types without modifying PaymentProcessor

            // ── REAL-WORLD EXAMPLE: Shipping Calculator ──────────────────────
            // Output: --- Real-World: Shipping Calculator ---
            Console.WriteLine("\n--- Real-World: Shipping Calculator ---");
            
            var shipping = new ShippingCalculator();
            
            shipping.AddStrategy(new DomesticShipping());
            shipping.AddStrategy(new InternationalShipping());
            shipping.AddStrategy(new ExpressShipping());
            
            var domestic = shipping.Calculate(10.0m, "Domestic");
            var international = shipping.Calculate(10.0m, "International");
            var express = shipping.Calculate(10.0m, "Express");
            
            // Output: Domestic: $15.00
            // Output: International: $45.00
            // Output: Express: $75.00

            Console.WriteLine("\n=== OCP Complete ===");
        }
    }

    /// <summary>
    /// BAD: Must modify to add new discount types
    /// </summary>
    public class BadDiscountCalculator
    {
        public decimal Calculate(decimal price, string customerType)
        {
            if (customerType == "Regular")
                return price * 0.9m;
            if (customerType == "Silver")
                return price * 0.8m;
            if (customerType == "Gold")
                return price * 0.7m;
            return price;
        }
    }

    /// <summary>
    /// GOOD: Open for extension via interface
    /// </summary>
    public interface IDiscount
    {
        decimal Apply(decimal price); // method: applies discount
    }

    /// <summary>
    /// Regular discount
    /// </summary>
    public class RegularDiscount : IDiscount
    {
        public decimal Apply(decimal price) => price * 0.9m;
    }

    /// <summary>
    /// Silver discount
    /// </summary>
    public class SilverDiscount : IDiscount
    {
        public decimal Apply(decimal price) => price * 0.8m;
    }

    /// <summary>
    /// Gold discount
    /// </summary>
    public class GoldDiscount : IDiscount
    {
        public decimal Apply(decimal price) => price * 0.7m;
    }

    /// <summary>
    /// Payment strategy interface
    /// </summary>
    public interface IPaymentStrategy
    {
        void Pay(decimal amount); // method: processes payment
    }

    /// <summary>
    /// Credit card payment
    /// </summary>
    public class CreditCardStrategy : IPaymentStrategy
    {
        public void Pay(decimal amount) => Console.WriteLine($"   Credit card: ${amount:F2}");
    }

    /// <summary>
    /// PayPal payment
    /// </summary>
    public class PayPalStrategy : IPaymentStrategy
    {
        public void Pay(decimal amount) => Console.WriteLine($"   PayPal: ${amount:F2}");
    }

    /// <summary>
    /// Payment processor - closed for modification
    /// </summary>
    public class PaymentProcessor
    {
        private IPaymentStrategy _strategy;
        
        public void SetStrategy(IPaymentStrategy strategy)
        {
            _strategy = strategy;
        }
        
        public void Process(decimal amount)
        {
            _strategy.Pay(amount);
        }
    }

    /// <summary>
    /// Shipping strategy interface
    /// </summary>
    public interface IShippingStrategy
    {
        decimal Calculate(decimal weight); // method: calculates shipping
        bool Supports(string type); // method: checks if supports type
    }

    /// <summary>
    /// Domestic shipping
    /// </summary>
    public class DomesticShipping : IShippingStrategy
    {
        public decimal Calculate(decimal weight) => weight * 1.5m + 5.0m;
        public bool Supports(string type) => type == "Domestic";
    }

    /// <summary>
    /// International shipping
    /// </summary>
    public class InternationalShipping : IShippingStrategy
    {
        public decimal Calculate(decimal weight) => weight * 4.5m + 15.0m;
        public bool Supports(string type) => type == "International";
    }

    /// <summary>
    /// Express shipping
    /// </summary>
    public class ExpressShipping : IShippingStrategy
    {
        public decimal Calculate(decimal weight) => weight * 7.5m + 25.0m;
        public bool Supports(string type) => type == "Express";
    }

    /// <summary>
    /// Shipping calculator - closed for modification
    /// </summary>
    public class ShippingCalculator
    {
        private List<IShippingStrategy> _strategies = new List<IShippingStrategy>();
        
        public void AddStrategy(IShippingStrategy strategy)
        {
            _strategies.Add(strategy);
        }
        
        public decimal Calculate(decimal weight, string type)
        {
            foreach (var strategy in _strategies)
            {
                if (strategy.Supports(type))
                {
                    var cost = strategy.Calculate(weight);
                    Console.WriteLine($"   {type}: ${cost:F2}");
                    return cost;
                }
            }
            return 0;
        }
    }
}