/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Dependency Inversion Principle - Part 2
 * FILE      : 02_DIP_Part2.cs
 * PURPOSE   : Advanced DIP with real-world patterns
 * ============================================================
 */
using System; // Core System namespace for Console
using System.Collections.Generic; // Generic collections

namespace CSharp_MasterGuide._12_SOLID_Principles._05_DependencyInversion._02_DIP_Part2
{
    /// <summary>
    /// Demonstrates DIP advanced examples
    /// </summary>
    public class DIPPart2Demo
    {
        /// <summary>
        /// Entry point for DIP Part 2 examples
        /// </summary>
        public static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Strategy Pattern + DIP
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== DIP Part 2 ===\n");

            // Output: --- Strategy + DI ---
            Console.WriteLine("--- Strategy + DI ---");

            // Inject strategy implementations
            var calculator = new ShippingCalculator(
                new FedExStrategy());
            
            Console.WriteLine($"   FedEx: {calculator.Calculate(10m):C}");
            // Output: FedEx: $15.00

            calculator = new ShippingCalculator(new UPSStrategy());
            Console.WriteLine($"   UPS: {calculator.Calculate(10m):C}");
            // Output: UPS: $12.00

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Repository Pattern + DIP
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Repository + DI ---
            Console.WriteLine("\n--- Repository + DI ---");

            // Different data sources, same interface
            IUserRepository repo = new SqlUserRepository();
            var service = new UserService(repo);
            service.Get(1);
            // Output: SQL: User 1

            repo = new MongoUserRepository();
            service = new UserService(repo);
            service.Get(1);
            // Output: Mongo: User 1

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Service Layer + DIP
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Service Layer + DI ---
            Console.WriteLine("\n--- Service Layer + DI ---");

            // Layers depend on abstractions
            var orderController = new OrderController(
                new OrderService(
                    new OrderRepository(),
                    new PaymentService(
                        new PaymentGateway())));

            orderController.PlaceOrder("Widget", 2);
            // Output: Order placed: Widget x2
            // Output: Payment processed
            // Output: Order saved

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Decorator Pattern + DIP
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Decorator + DI ---
            Console.WriteLine("\n--- Decorator + DI ---");

            // Wrap decorators around base
            ICache baseCache = new SimpleCache();
            ICache loggingCache = new LoggingCache(baseCache);
            ICache metricsCache = new MetricsCache(loggingCache);

            metricsCache.Get("key");
            metricsCache.Set("key", "value");
            // Output: Cache get: key
            // Output: Cache set: key
            // Output: Metrics: get+set

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Factory + DI
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Factory + DI ---
            Console.WriteLine("\n--- Factory + DI ---");

            // Factory creates dependencies
            var factory = new ServiceFactory();
            var service1 = factory.Create<IShippingService>("fedex");
            var service2 = factory.Create<IShippingService>("ups");

            service1.Calculate(5m);
            service2.Calculate(5m);
            // Output: FedEx: $7.50
            // Output: UPS: $6.00

            Console.WriteLine("\n=== DIP Part 2 Complete ===");
        }
    }

    /// <summary>
    /// Shipping strategy interface
    /// </summary>
    public interface IShippingStrategy
    {
        decimal Calculate(decimal weight); // method: calculate shipping
    }

    /// <summary>
    /// FedEx strategy
    /// </summary>
    public class FedExStrategy : IShippingStrategy
    {
        public decimal Calculate(decimal weight) => weight * 1.5m;
    }

    /// <summary>
    /// UPS strategy
    /// </summary>
    public class UPSStrategy : IShippingStrategy
    {
        public decimal Calculate(decimal weight) => weight * 1.2m;
    }

    /// <summary>
    /// Shipping calculator - depends on abstraction
    /// </summary>
    public class ShippingCalculator
    {
        private readonly IShippingStrategy _strategy; // field: strategy

        public ShippingCalculator(IShippingStrategy strategy)
        {
            _strategy = strategy;
        }

        public decimal Calculate(decimal weight) => _strategy.Calculate(weight);
    }

    /// <summary>
    /// User repository abstraction
    /// </summary>
    public interface IUserRepository
    {
        void Get(int id); // method: get user
    }

    /// <summary>
    /// SQL user repository
    /// </summary>
    public class SqlUserRepository : IUserRepository
    {
        public void Get(int id) => Console.WriteLine($"   SQL: User {id}");
    }

    /// <summary>
    /// Mongo user repository
    /// </summary>
    public class MongoUserRepository : IUserRepository
    {
        public void Get(int id) => Console.WriteLine($"   Mongo: User {id}");
    }

    /// <summary>
    /// User service - depends on abstraction
    /// </summary>
    public class UserService
    {
        private readonly IUserRepository _repository; // field: repository

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public void Get(int id) => _repository.Get(id);
    }

    /// <summary>
    /// Order repository interface
    /// </summary>
    public interface IOrderRepository
    {
        void Save(string order); // method: save order
    }

    /// <summary>
    /// Payment gateway interface
    /// </summary>
    public interface IPaymentGateway
    {
        void Process(decimal amount); // method: process payment
    }

    /// <summary>
    /// Order repository
    /// </summary>
    public class OrderRepository : IOrderRepository
    {
        public void Save(string order) => Console.WriteLine("   Order saved");
    }

    /// <summary>
    /// Payment service - depends on abstraction
    /// </summary>
    public class PaymentService
    {
        private readonly IPaymentGateway _gateway; // field: gateway

        public PaymentService(IPaymentGateway gateway)
        {
            _gateway = gateway;
        }

        public void Process(decimal amount) => _gateway.Process(amount);
    }

    /// <summary>
    /// Payment gateway
    /// </summary>
    public class PaymentGateway : IPaymentGateway
    {
        public void Process(decimal amount) => Console.WriteLine("   Payment processed");
    }

    /// <summary>
    /// Order service - depends on abstractions
    /// </summary>
    public class OrderService
    {
        private readonly IOrderRepository _repository; // field: repository
        private readonly PaymentService _paymentService; // field: payment service

        public OrderService(IOrderRepository repository, PaymentService paymentService)
        {
            _repository = repository;
            _paymentService = paymentService;
        }

        public void PlaceOrder(string product, int quantity)
        {
            Console.WriteLine($"   Order placed: {product} x{quantity}");
            _paymentService.Process(0);
            _repository.Save(product);
        }
    }

    /// <summary>
    /// Order controller
    /// </summary>
    public class OrderController
    {
        private readonly OrderService _service; // field: service

        public OrderController(OrderService service)
        {
            _service = service;
        }

        public void PlaceOrder(string product, int quantity)
        {
            _service.PlaceOrder(product, quantity);
        }
    }

    /// <summary>
    /// Cache abstraction
    /// </summary>
    public interface ICache
    {
        object Get(string key); // method: get value
        void Set(string key, object value); // method: set value
    }

    /// <summary>
    /// Simple cache
    /// </summary>
    public class SimpleCache : ICache
    {
        public object Get(string key) => null;
        public void Set(string key, object value) { }
    }

    /// <summary>
    /// Logging decorator
    /// </summary>
    public class LoggingCache : ICache
    {
        private readonly ICache _inner; // field: inner cache

        public LoggingCache(ICache inner) => _inner = inner;

        public object Get(string key)
        {
            Console.WriteLine($"   Cache get: {key}");
            return _inner.Get(key);
        }

        public void Set(string key, object value)
        {
            Console.WriteLine($"   Cache set: {key}");
            _inner.Set(key, value);
        }
    }

    /// <summary>
    /// Metrics decorator
    /// </summary>
    public class MetricsCache : ICache
    {
        private readonly ICache _inner; // field: inner cache

        public MetricsCache(ICache inner) => _inner = inner;

        public object Get(string key)
        {
            Console.WriteLine("   Metrics: get+set");
            return _inner.Get(key);
        }

        public void Set(string key, object value) => _inner.Set(key, value);
    }

    /// <summary>
    /// Shipping service interface
    /// </summary>
    public interface IShippingService
    {
        decimal Calculate(decimal weight); // method: calculate
    }

    /// <summary>
    /// Service factory
    /// </summary>
    public class ServiceFactory
    {
        public T Create<T>(string type) where T : class, new()
        {
            return new T();
        }
    }
}