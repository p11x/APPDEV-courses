/*
 * TOPIC: CSharp_MasterGuide/03_Advanced_OOP/03_LambdaExpressions
 * SUBTOPIC: Real-World Lambda Expressions - Part 2
 * FILE: Lambda_RealWorld_Part2.cs
 * PURPOSE: Advanced real-world patterns: strategy pattern, factory pattern, processing pipelines
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._03_LambdaExpressions
{
    public interface IPaymentProcessor
    {
        decimal Process(decimal amount);
    }

    public interface IDiscountStrategy
    {
        decimal Apply(decimal price);
    }

    public interface IValidationRule<T>
    {
        bool IsValid(T item);
        string ErrorMessage { get; }
    }

    public class LambdaRealWorldPart2
    {
        public static void RunMain(string[] args)
        {
            Console.WriteLine("=== Real-World Lambda Part 2: Advanced Patterns ===\n");

            // ============================================
            // STRATEGY PATTERN WITH LAMBDA
            // ============================================

            Console.WriteLine("=== Strategy Pattern ===");

            // Example 1: Payment processing strategies
            var creditCardPayment = new PaymentProcessor((amount) =>
            {
                var fee = amount * 0.029m; // 2.9%
                return amount + fee;
            });

            var paypalPayment = new PaymentProcessor((amount) =>
            {
                var fee = amount * 0.034m + 0.30m;
                return amount + fee;
            });

            var bankTransferPayment = new PaymentProcessor((amount) =>
            {
                return amount + 5.00m; // Flat fee
            });

            decimal purchaseAmount = 1000m;
            Console.WriteLine($"Amount: ${purchaseAmount}");
            Console.WriteLine($"Credit Card: ${creditCardPayment.Process(purchaseAmount):F2}");
            Console.WriteLine($"PayPal: ${paypalPayment.Process(purchaseAmount):F2}");
            Console.WriteLine($"Bank Transfer: ${bankTransferPayment.Process(purchaseAmount):F2}");

            // Example 2: Discount strategies
            var discountStrategies = new Dictionary<string, IDiscountStrategy>
            {
                { "NONE", new DiscountStrategy(p => p) },
                { "TEN_PERCENT", new DiscountStrategy(p => p * 0.9m) },
                { "TWENTY_PERCENT", new DiscountStrategy(p => p * 0.8m) },
                { "BULK", new DiscountStrategy(p => p > 1000 ? p * 0.75m : p) },
                { "LOYALTY", new DiscountStrategy(p => p * 0.85m) }
            };

            Console.WriteLine($"\nDiscount strategies:");
            var originalPrice = 500m;
            foreach (var strategy in discountStrategies)
            {
                var discounted = strategy.Value.Apply(originalPrice);
                Console.WriteLine($"  {strategy.Key}: ${originalPrice} -> ${discounted}");
            }

            // ============================================
            // FACTORY PATTERN WITH LAMBDA
            // ============================================

            Console.WriteLine($"\n=== Factory Pattern ===");

            // Example 3: Factory for creating objects
            var shapeFactory = new Factory<string,IShape>
            {
                { "CIRCLE", () => new Circle() },
                { "RECTANGLE", () => new Rectangle() },
                { "TRIANGLE", () => new Triangle() }
            };

            var circle = shapeFactory.Create("CIRCLE");
            var rectangle = shapeFactory.Create("RECTANGLE");
            Console.WriteLine($"Created shapes: {circle.GetType().Name}, {rectangle.GetType().Name}");

            // Example 4: Service factory
            var loggerFactory = new Factory<string, ILogger>
            {
                { "CONSOLE", () => new ConsoleLogger() },
                { "FILE", () => new FileLogger() },
                { "DATABASE", () => new DatabaseLogger() }
            };

            var consoleLogger = loggerFactory.Create("CONSOLE");
            consoleLogger.Log("Application started");

            // ============================================
            // PROCESSING PIPELINE WITH LAMBDA
            // ============================================

            Console.WriteLine($"\n=== Processing Pipeline ===");

            // Example 5: Data processing pipeline
            var rawData = new List<decimal> { 100, 250, 50, -10, 300, -50, 75, 200 };

            var pipeline = Pipeline.Create<decimal>()
                .AddStep(data => data.Where(x => x > 0)) // Remove negatives
                .AddStep(data => data.Select(x => x * 1.1m)) // Add 10% tax
                .AddStep(data => data.Where(x => x > 50)) // Minimum $50
                .AddStep(data => data.Sum()); // Sum all

            var result = pipeline.Process(rawData);
            Console.WriteLine($"Pipeline result: ${result}"); // Output: sum of valid processed values

            // Example 6: Validation pipeline
var validators = new List<IValidationRule<RW2Order>>
            {
                new ValidationRule<RW2Order>(o => o.Amount > 0, "Amount must be positive"),
                new ValidationRule<RW2Order>(o => o.CustomerId > 0, "Invalid customer"),
                new ValidationRule<RW2Order>(o => !string.IsNullOrEmpty(o.Status), "Status required")
            };

            var testOrder = new RW2Order { OrderId = 1, CustomerId = 1, Amount = 100, Status = "Pending" };
            var invalidOrder = new RW2Order { OrderId = 2, CustomerId = 0, Amount = -50, Status = "" };
            var errors = validators.Where(v => !v.IsValid(invalidOrder)).Select(v => v.ErrorMessage);
            Console.WriteLine($"Invalid order errors: {string.Join(", ", errors)}");

            // ============================================
            // CALLBACK PATTERNS WITH LAMBDA
            // ============================================

            Console.WriteLine($"\n=== Callback Patterns ===");

            // Example 7: Async callback style
            var orderProcessor = new OrderProcessor();
            orderProcessor.ProcessOrder(new RW2Order { OrderId = 1, Amount = 500 }, 
                success => 
                {
                    if (success)
                        Console.WriteLine("Order processed successfully!");
                    else
                        Console.WriteLine("Order processing failed!");
                });

            // Example 8: Event aggregator pattern
            var eventAggregator = new EventAggregator();
            eventAggregator.Subscribe<RW2Order>("OrderCreated", order => 
                Console.WriteLine($"Order {order.OrderId} was created for ${order.Amount}"));
            eventAggregator.Subscribe<RW2Order>("OrderCreated", order =>
                Console.WriteLine($"Sending notification for order {order.OrderId}..."));

            eventAggregator.Publish("OrderCreated", new RW2Order { OrderId = 100, Amount = 250 });
        }
    }

    // Class for discount strategy example
    public class DiscountStrategy : IDiscountStrategy
    {
        private readonly Func<decimal, decimal> _applyDiscount;

        public DiscountStrategy(Func<decimal, decimal> applyDiscount)
        {
            _applyDiscount = applyDiscount;
        }

        public decimal Apply(decimal price) => _applyDiscount(price);
    }

    // Class for payment processor example
    public class PaymentProcessor : IPaymentProcessor
    {
        private readonly Func<decimal, decimal> _process;

        public PaymentProcessor(Func<decimal, decimal> process)
        {
            _process = process;
        }

        public decimal Process(decimal amount) => _process(amount);
    }

    // Generic factory implementation
    public class Factory<TKey, TProduct>
    {
        private readonly Dictionary<TKey, Func<TProduct>> _creators = new Dictionary<TKey, Func<TProduct>>();

        public void Add(TKey key, Func<TProduct> creator)
        {
            _creators[key] = creator;
        }

        public TProduct Create(TKey key)
        {
            return _creators[key]();
        }
    }

    // Shape interfaces and classes
    public interface IShape { }
    public class Circle : IShape { }
    public class Rectangle : IShape { }
    public class Triangle : IShape { }

    // Logger interfaces
    public interface ILogger
    {
        void Log(string message);
    }
    public class ConsoleLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine($"[CONSOLE] {message}");
    }
    public class FileLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine($"[FILE] {message}");
    }
    public class DatabaseLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine($"[DATABASE] {message}");
    }

    // Pipeline implementation
    public class Pipeline<T>
    {
        private readonly List<Func<IEnumerable<T>, IEnumerable<T>>> _steps = new List<Func<IEnumerable<T>, IEnumerable<T>>>();

        public Pipeline<T> AddStep(Func<IEnumerable<T>, IEnumerable<T>> step)
        {
            _steps.Add(step);
            return this;
        }

        public IEnumerable<T> Process(IEnumerable<T> input)
        {
            var result = input;
            foreach (var step in _steps)
            {
                result = step(result);
            }
            return result;
        }

        public static Pipeline<T> Create()
        {
            return new Pipeline<T>();
        }
    }

    public class Pipeline
    {
        public static Pipeline<T> Create<T>()
        {
            return new Pipeline<T>();
        }
    }

    // Validation rule implementation
    public class ValidationRule<T> : IValidationRule<T>
    {
        private readonly Func<T, bool> _validate;

        public ValidationRule(Func<T, bool> validate, string errorMessage)
        {
            _validate = validate;
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }
        public bool IsValid(T item) => _validate(item);
    }

// Order and event aggregator for callback examples
    public class RW2Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }

    public class RW2OrderProcessor
    {
        public void ProcessOrder(RW2Order order, Action<bool> callback)
        {
            // Simulate processing
            bool success = order.Amount > 0;
            callback(success);
        }
    }

    public class EventAggregator
    {
        private readonly Dictionary<string, List<Delegate>> _subscribers = new Dictionary<string, List<Delegate>>();

        public void Subscribe<T>(string eventName, Action<T> handler)
        {
            if (!_subscribers.ContainsKey(eventName))
                _subscribers[eventName] = new List<Delegate>();
            _subscribers[eventName].Add(handler);
        }

        public void Publish<T>(string eventName, T eventData)
        {
            if (_subscribers.ContainsKey(eventName))
            {
                foreach (var handler in _subscribers[eventName])
                {
                    ((Action<T>)handler)(eventData);
                }
            }
        }
    }
}