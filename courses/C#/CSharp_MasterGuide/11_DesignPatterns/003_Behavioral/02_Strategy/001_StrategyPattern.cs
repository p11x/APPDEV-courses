/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Strategy Pattern
 * FILE      : 01_StrategyPattern.cs
 * PURPOSE   : Demonstrates Strategy design pattern in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral._02_Strategy
{
    /// <summary>
    /// Demonstrates Strategy pattern
    /// </summary>
    public class StrategyPattern
    {
        /// <summary>
        /// Entry point for Strategy pattern examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Strategy Pattern ===
            Console.WriteLine("=== Strategy Pattern ===\n");

            // ── CONCEPT: What is Strategy? ─────────────────────────────────────
            // Defines family of algorithms, makes them interchangeable

            // Example 1: Basic Strategy
            // Output: 1. Basic Strategy:
            Console.WriteLine("1. Basic Strategy:");
            
            // Context uses different strategies
            var sorter = new Sorter();
            
            // Bubble sort strategy
            sorter.SetStrategy(new BubbleSortStrategy());
            var result1 = sorter.Sort(new int[] { 5, 3, 8, 1 });
            // Output: Sorted with BubbleSort: 1, 3, 5, 8
            
            // Quick sort strategy
            sorter.SetStrategy(new QuickSortStrategy());
            var result2 = sorter.Sort(new int[] { 5, 3, 8, 1 });
            // Output: Sorted with QuickSort: 1, 3, 5, 8

            // ── CONCEPT: Strategy Selection ───────────────────────────────────
            // Choose algorithm at runtime

            // Example 2: Strategy Selection
            // Output: 2. Strategy Selection:
            Console.WriteLine("\n2. Strategy Selection:");
            
            // Payment processing with different strategies
            var paymentContext = new PaymentContext();
            
            // Choose credit card
            paymentContext.SetStrategy(new CreditCardStrategy());
            paymentContext.ProcessPayment(100.00m);
            // Output: Credit card payment: $100.00
            
            // Switch to PayPal
            paymentContext.SetStrategy(new PayPalStrategy());
            paymentContext.ProcessPayment(100.00m);
            // Output: PayPal payment: $100.00

            // ── CONCEPT: Strategy with State ──────────────────────────────────
            // Combine with state pattern

            // Example 3: Strategy with State
            // Output: 3. Strategy with State:
            Console.WriteLine("\n3. Strategy with State:");
            
            // Shipping calculator with state-dependent strategy
            var shipping = new ShippingCalculator();
            
            // Domestic shipping
            shipping.SetDestination(Destination.Domestic);
            var domesticCost = shipping.CalculateShipping(10.0m);
            // Output: Domestic shipping: $15.00
            
            // International shipping
            shipping.SetDestination(Destination.International);
            var internationalCost = shipping.CalculateShipping(10.0m);
            // Output: International shipping: $45.00

            // ── REAL-WORLD EXAMPLE: Validation ────────────────────────────────
            // Output: --- Real-World: Validation ---
            Console.WriteLine("\n--- Real-World: Validation ---");
            
            // Input validation with different strategies
            var validator = new InputValidator();
            
            // Email validation
            validator.SetStrategy(new EmailValidationStrategy());
            var isEmailValid = validator.Validate("test@email.com");
            // Output: Email valid: True
            
            // Phone validation
            validator.SetStrategy(new PhoneValidationStrategy());
            var isPhoneValid = validator.Validate("+1234567890");
            // Output: Phone valid: True
            
            // URL validation
            validator.SetStrategy(new URLValidationStrategy());
            var isUrlValid = validator.Validate("https://example.com");
            // Output: URL valid: True

            Console.WriteLine("\n=== Strategy Pattern Complete ===");
        }
    }

    /// <summary>
    /// Sorting strategy interface
    /// </summary>
    public interface ISortStrategy
    {
        int[] Sort(int[] array); // method: sorts array
    }

    /// <summary>
    /// Bubble sort implementation
    /// </summary>
    public class BubbleSortStrategy : ISortStrategy
    {
        public int[] Sort(int[] array)
        {
            var sorted = (int[])array.Clone();
            for (int i = 0; i < sorted.Length; i++)
            {
                for (int j = 0; j < sorted.Length - 1; j++)
                {
                    if (sorted[j] > sorted[j + 1])
                    {
                        var temp = sorted[j];
                        sorted[j] = sorted[j + 1];
                        sorted[j + 1] = temp;
                    }
                }
            }
            Console.WriteLine($"   Sorted with BubbleSort: {string.Join(", ", sorted)}");
            return sorted;
        }
    }

    /// <summary>
    /// Quick sort implementation
    /// </summary>
    public class QuickSortStrategy : ISortStrategy
    {
        public int[] Sort(int[] array)
        {
            var sorted = (int[])array.Clone();
            Array.Sort(sorted);
            Console.WriteLine($"   Sorted with QuickSort: {string.Join(", ", sorted)}");
            return sorted;
        }
    }

    /// <summary>
    /// Context using strategy
    /// </summary>
    public class Sorter
    {
        private ISortStrategy _strategy;
        
        public void SetStrategy(ISortStrategy strategy)
        {
            _strategy = strategy;
        }
        
        public int[] Sort(int[] array)
        {
            return _strategy.Sort(array);
        }
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
        public void Pay(decimal amount)
        {
            Console.WriteLine($"   Credit card payment: ${amount:F2}");
        }
    }

    /// <summary>
    /// PayPal payment
    /// </summary>
    public class PayPalStrategy : IPaymentStrategy
    {
        public void Pay(decimal amount)
        {
            Console.WriteLine($"   PayPal payment: ${amount:F2}");
        }
    }

    /// <summary>
    /// Payment context
    /// </summary>
    public class PaymentContext
    {
        private IPaymentStrategy _strategy;
        
        public void SetStrategy(IPaymentStrategy strategy)
        {
            _strategy = strategy;
        }
        
        public void ProcessPayment(decimal amount)
        {
            _strategy.Pay(amount);
        }
    }

    /// <summary>
    /// Destination type
    /// </summary>
    public enum Destination { Domestic, International }

    /// <summary>
    /// Shipping strategy interface
    /// </summary>
    public interface IShippingStrategy
    {
        decimal Calculate(decimal weight); // method: calculates shipping cost
    }

    /// <summary>
    /// Domestic shipping
    /// </summary>
    public class DomesticShippingStrategy : IShippingStrategy
    {
        public decimal Calculate(decimal weight)
        {
            return weight * 1.5m + 5.0m; // $1.50/lb + $5 base
        }
    }

    /// <summary>
    /// International shipping
    /// </summary>
    public class InternationalShippingStrategy : IShippingStrategy
    {
        public decimal Calculate(decimal weight)
        {
            return weight * 4.5m + 15.0m; // $4.50/lb + $15 base
        }
    }

    /// <summary>
    /// Shipping calculator
    /// </summary>
    public class ShippingCalculator
    {
        private IShippingStrategy _strategy;
        private Destination _destination;
        
        public void SetDestination(Destination destination)
        {
            _destination = destination;
            _strategy = destination switch
            {
                Destination.Domestic => new DomesticShippingStrategy(),
                Destination.International => new InternationalShippingStrategy(),
                _ => new DomesticShippingStrategy()
            };
        }
        
        public decimal CalculateShipping(decimal weight)
        {
            var cost = _strategy.Calculate(weight);
            var label = _destination == Destination.Domestic ? "Domestic" : "International";
            Console.WriteLine($"   {label} shipping: ${cost:F2}");
            return cost;
        }
    }

    /// <summary>
    /// Validation strategy interface
    /// </summary>
    public interface IValidationStrategy
    {
        bool Validate(string input); // method: validates input
    }

    /// <summary>
    /// Email validation
    /// </summary>
    public class EmailValidationStrategy : IValidationStrategy
    {
        public bool Validate(string input)
        {
            var isValid = input.Contains("@") && input.Contains(".");
            Console.WriteLine($"   Email valid: {isValid}");
            return isValid;
        }
    }

    /// <summary>
    /// Phone validation
    /// </summary>
    public class PhoneValidationStrategy : IValidationStrategy
    {
        public bool Validate(string input)
        {
            var isValid = input.Length >= 10 && input.StartsWith("+");
            Console.WriteLine($"   Phone valid: {isValid}");
            return isValid;
        }
    }

    /// <summary>
    /// URL validation
    /// </summary>
    public class URLValidationStrategy : IValidationStrategy
    {
        public bool Validate(string input)
        {
            var isValid = input.StartsWith("http://") || input.StartsWith("https://");
            Console.WriteLine($"   URL valid: {isValid}");
            return isValid;
        }
    }

    /// <summary>
    /// Input validator
    /// </summary>
    public class InputValidator
    {
        private IValidationStrategy _strategy;
        
        public void SetStrategy(IValidationStrategy strategy)
        {
            _strategy = strategy;
        }
        
        public bool Validate(string input)
        {
            return _strategy.Validate(input);
        }
    }
}