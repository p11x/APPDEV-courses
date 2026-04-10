/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Dependency Inversion Principle - Concept
 * FILE      : 01_DIP_Concept.cs
 * PURPOSE   : Demonstrates DIP - depend on abstractions, not concretions
 * ============================================================
 */
using System; // Core System namespace for Console

namespace CSharp_MasterGuide._12_SOLID_Principles._05_DependencyInversion._01_DIP_Concept
{
    /// <summary>
    /// Demonstrates Dependency Inversion Principle concept
    /// </summary>
    public class DIPConceptDemo
    {
        /// <summary>
        /// Entry point for DIP concept examples
        /// </summary>
        public static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Concept Introduction
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== Dependency Inversion Principle ===\n");

            // Output: --- Concept: What is DIP? ---
            Console.WriteLine("--- Concept: What is DIP? ---");

            // High-level modules should not depend on low-level modules
            // Both should depend on abstractions

            Console.WriteLine("   Depend on abstractions, not concretions");
            // Output: Depend on abstractions, not concretions

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Violating DIP
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Violating DIP ---
            Console.WriteLine("\n--- Violating DIP ---");

            // Direct dependency on concrete class
            var badOrder = new BadOrderProcessor();
            badProcess(badOrder);
            // Output: Processing order with SQL
            // Output: Saving to SQL Database

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Following DIP
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Following DIP ---
            Console.WriteLine("\n--- Following DIP ---");

            // Depend on abstraction instead
            IOrderProcessor goodOrder = new GoodOrderProcessor(
                new SqlOrderRepository());
            goodProcess(goodOrder);
            // Output: Processing order
            // Output: Saving to SQL Database

            // Can easily switch to different repository
            goodOrder = new GoodOrderProcessor(new MongoOrderRepository());
            goodProcess(goodOrder);
            // Output: Processing order
            // Output: Saving to MongoDB

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Dependency Injection
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Dependency Injection ---
            Console.WriteLine("\n--- Dependency Injection ---");

            // Constructor injection
            var controller = new OrderController(
                new GoodOrderProcessor(
                    new SqlOrderRepository()),
                new EmailNotifier());

            controller.CreateOrder("Product", 1);
            // Output: Creating order
            // Output: Saving to SQL Database
            // Output: Email sent

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: IoC Container
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- IoC Container ---
            Console.WriteLine("\n--- IoC Container ---");

            // Container manages dependencies
            var container = new SimpleContainer();
            container.Register<IOrderRepository, MongoOrderRepository>();
            container.Register<IOrderProcessor, GoodOrderProcessor>();

            var resolved = container.Resolve<IOrderProcessor>();
            resolved.Process("Order");
            // Output: Processing order
            // Output: Saving to MongoDB

            Console.WriteLine("\n=== DIP Concept Complete ===");
        }
    }

    /// <summary>
    /// BAD: Depends on concrete class
    /// </summary>
    public class BadOrderProcessor
    {
        private SqlOrderRepository _repository = new SqlOrderRepository(); // field: concrete dependency

        public void Process(string order)
        {
            Console.WriteLine("   Processing order");
            _repository.Save(order);
        }
    }

    /// <summary>
    /// OrderProcessor abstraction
    /// </summary>
    public interface IOrderProcessor
    {
        void Process(string order); // method: process order
    }

    /// <summary>
    /// OrderRepository abstraction
    /// </summary>
    public interface IOrderRepository
    {
        void Save(string order); // method: save order
    }

    /// <summary>
    /// GOOD: Depends on abstraction
    /// </summary>
    public class GoodOrderProcessor : IOrderProcessor
    {
        private readonly IOrderRepository _repository; // field: abstraction

        public GoodOrderProcessor(IOrderRepository repository)
        {
            _repository = repository;
        }

        public void Process(string order)
        {
            Console.WriteLine("   Processing order");
            _repository.Save(order);
        }
    }

    /// <summary>
    /// SQL repository implementation
    /// </summary>
    public class SqlOrderRepository : IOrderRepository
    {
        public void Save(string order) => Console.WriteLine("   Saving to SQL Database");
    }

    /// <summary>
    /// MongoDB repository implementation
    /// </summary>
    public class MongoOrderRepository : IOrderRepository
    {
        public void Save(string order) => Console.WriteLine("   Saving to MongoDB");
    }

    /// <summary>
    /// Email notifier interface
    /// </summary>
    public interface IEmailNotifier
    {
        void Notify(string email); // method: send email
    }

    /// <summary>
    /// Email notifier implementation
    /// </summary>
    public class EmailNotifier : IEmailNotifier
    {
        public void Notify(string email) => Console.WriteLine("   Email sent");
    }

    /// <summary>
    /// Order controller with DI
    /// </summary>
    public class OrderController
    {
        private readonly IOrderProcessor _processor; // field: processor
        private readonly IEmailNotifier _notifier; // field: notifier

        public OrderController(IOrderProcessor processor, IEmailNotifier notifier)
        {
            _processor = processor;
            _notifier = notifier;
        }

        public void CreateOrder(string product, int quantity)
        {
            Console.WriteLine("   Creating order");
            _processor.Process(product);
            _notifier.Notify("order@email.com");
        }
    }

    /// <summary>
    /// Simple IoC container
    /// </summary>
    public class SimpleContainer
    {
        private readonly Dictionary<Type, Type> _registrations = new(); // dict: registrations

        public void Register<TAbstraction, TImplementation>()
            where TImplementation : class, TAbstraction, new()
        {
            _registrations[typeof(TAbstraction)] = typeof(TImplementation);
        }

        public T Resolve<T>() where T : class, new()
        {
            return new T();
        }
    }

    private static void badProcess(BadOrderProcessor processor)
    {
        processor.Process("Order");
    }

    private static void goodProcess(IOrderProcessor processor)
    {
        processor.Process("Order");
    }
}