/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Real-World Applications
 * FILE      : 17_Behavioral_RealWorld.cs
 * PURPOSE   : Real-world examples of Behavioral patterns
 * ============================================================
 */
using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral._08_RealWorld
{
    /// <summary>
    /// Real-world Behavioral pattern demonstrations
    /// </summary>
    public class BehavioralRealWorld
    {
        /// <summary>
        /// Entry point for real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Behavioral Patterns Real-World ===\n");

            // --- Observer: Event System ---
            Console.WriteLine("--- Observer: Event System ---");
            var eventBus = new EventBus();
            var logger = new EventLogger();
            var metrics = new MetricsCollector();
            
            eventBus.Subscribe("order.created", logger);
            eventBus.Subscribe("order.created", metrics);
            eventBus.Publish("order.created", new { OrderId = "123" });
            
            // Output: Logged: order.created, Metrics recorded

            // --- Strategy: Payment Processing ---
            Console.WriteLine("\n--- Strategy: Payment Processing ---");
            var paymentContext = new PaymentContext();
            paymentContext.SetStrategy(new CreditCardPayment());
            paymentContext.Pay(100);
            
            paymentContext.SetStrategy(new PayPalPayment());
            paymentContext.Pay(50);
            
            // Output: Paid $100 via Credit Card, Paid $50 via PayPal

            // --- Command: Action Queue ---
            Console.WriteLine("\n--- Command: Action Queue ---");
            var queue = new CommandQueue();
            queue.Enqueue(new PrintCommand("Document 1"));
            queue.Enqueue(new PrintCommand("Document 2"));
            queue.ExecuteAll();
            
            // Output: Printed Document 1, Printed Document 2

            // --- State: Order Lifecycle ---
            Console.WriteLine("\n--- State: Order Lifecycle ---");
            var order = new Order();
            order.Submit();
            order.Pay();
            order.Ship();
            order.Deliver();
            
            // Output: Submitted, Paid, Shipped, Delivered

            // --- Chain of Responsibility: Middleware ---
            Console.WriteLine("\n--- Chain of Responsibility: Middleware ---");
            var pipeline = new MiddlewarePipeline();
            pipeline.Use(new AuthMiddleware());
            pipeline.Use(new LoggingMiddleware());
            pipeline.Use(new ValidationMiddleware());
            pipeline.Execute(new Request { Path = "/api/users" });
            
            // Output: Auth -> Log -> Validate -> Handler

            // --- Mediator: Chat Room ---
            Console.WriteLine("\n--- Mediator: Chat Room ---");
            var chat = new ChatRoom();
            var user1 = new ChatUser("Alice", chat);
            var user2 = new ChatUser("Bob", chat);
            
            user1.Send("Hello Bob!");
            // Output: Alice: Hello Bob!

            // --- Iterator: Collection Traversal ---
            Console.WriteLine("\n--- Iterator: Collection Traversal ---");
            var collection = new ProductCollection();
            collection.Add(new Product("Laptop", 999));
            collection.Add(new Product("Phone", 599));
            
            foreach (var product in collection)
            {
                Console.WriteLine($"   {product.Name}: ${product.Price}");
            }
            
            // Output: Laptop: $999, Phone: $599

            Console.WriteLine("\n=== Behavioral Real-World Complete ===");
        }
    }

    // Observer - Event Bus
    public class EventBus
    {
        private readonly Dictionary<string, List<IEventHandler>> _handlers = new();
        
        public void Subscribe(string eventType, IEventHandler handler)
        {
            if (!_handlers.ContainsKey(eventType))
                _handlers[eventType] = new List<IEventHandler>();
            _handlers[eventType].Add(handler);
        }
        
        public void Publish(string eventType, object data)
        {
            if (_handlers.ContainsKey(eventType))
            {
                foreach (var handler in _handlers[eventType])
                    handler.Handle(data);
            }
        }
    }

    public interface IEventHandler
    {
        void Handle(object data);
    }

    public class EventLogger : IEventHandler
    {
        public void Handle(object data) => Console.WriteLine("   Logged: order.created");
    }

    public class MetricsCollector : IEventHandler
    {
        public void Handle(object data) => Console.WriteLine("   Metrics recorded");
    }

    // Strategy - Payment
    public interface IPaymentStrategy
    {
        void Pay(decimal amount);
    }

    public class CreditCardPayment : IPaymentStrategy
    {
        public void Pay(decimal amount) => Console.WriteLine($"   Paid ${amount} via Credit Card");
    }

    public class PayPalPayment : IPaymentStrategy
    {
        public void Pay(decimal amount) => Console.WriteLine($"   Paid ${amount} via PayPal");
    }

    public class PaymentContext
    {
        private IPaymentStrategy _strategy;
        
        public void SetStrategy(IPaymentStrategy strategy) => _strategy = strategy;
        public void Pay(decimal amount) => _strategy.Pay(amount);
    }

    // Command - Action Queue
    public interface ICommand
    {
        void Execute();
    }

    public class PrintCommand : ICommand
    {
        private readonly string _document;
        
        public PrintCommand(string document) => _document = document;
        
        public void Execute() => Console.WriteLine($"   Printed {_document}");
    }

    public class CommandQueue
    {
        private readonly Queue<ICommand> _queue = new();
        
        public void Enqueue(ICommand command) => _queue.Enqueue(command);
        
        public void ExecuteAll()
        {
            while (_queue.Count > 0)
                _queue.Dequeue().Execute();
        }
    }

    // State - Order Lifecycle
    public interface IOrderState
    {
        void Submit(Order order);
        void Pay(Order order);
        void Ship(Order order);
        void Deliver(Order order);
    }

    public class Order
    {
        public IOrderState State { get; set; } = new SubmittedState();
        
        public void Submit() => State.Submit(this);
        public void Pay() => State.Pay(this);
        public void Ship() => State.Ship(this);
        public void Deliver() => State.Deliver(this);
    }

    public class SubmittedState : IOrderState
    {
        public void Submit(Order o) => Console.WriteLine("   Submitted");
        public void Pay(Order o) { Console.WriteLine("   Paid"); o.State = new PaidState(); }
        public void Ship(Order o) => Console.WriteLine("   Pay first");
        public void Deliver(Order o) => Console.WriteLine("   Ship first");
    }

    public class PaidState : IOrderState
    {
        public void Submit(Order o) => Console.WriteLine("   Already submitted");
        public void Pay(Order o) => Console.WriteLine("   Already paid");
        public void Ship(Order o) { Console.WriteLine("   Shipped"); o.State = new ShippedState(); }
        public void Deliver(Order o) => Console.WriteLine("   Ship first");
    }

    public class ShippedState : IOrderState
    {
        public void Submit(Order o) => Console.WriteLine("   Already submitted");
        public void Pay(Order o) => Console.WriteLine("   Already paid");
        public void Ship(Order o) => Console.WriteLine("   Already shipped");
        public void Deliver(Order o) { Console.WriteLine("   Delivered"); o.State = new DeliveredState(); }
    }

    public class DeliveredState : IOrderState
    {
        public void Submit(Order o) => Console.WriteLine("   Completed");
        public void Pay(Order o) => Console.WriteLine("   Completed");
        public void Ship(Order o) => Console.WriteLine("   Completed");
        public void Deliver(Order o) => Console.WriteLine("   Completed");
    }

    // Chain of Responsibility - Middleware
    public class Request
    {
        public string Path { get; set; }
    }

    public interface IMiddleware
    {
        void Handle(Request req, Action next);
    }

    public class MiddlewarePipeline
    {
        private readonly List<IMiddleware> _middleware = new();
        
        public void Use(IMiddleware m) => _middleware.Add(m);
        
        public void Execute(Request req)
        {
            Action next = () => Console.WriteLine("   Handler executed");
            for (int i = _middleware.Count - 1; i >= 0; i--)
            {
                var m = _middleware[i];
                var tempNext = next;
                next = () => m.Handle(req, tempNext);
            }
            next();
        }
    }

    public class AuthMiddleware : IMiddleware
    {
        public void Handle(Request req, Action next) 
        { 
            Console.WriteLine("   Auth checked"); 
            next(); 
        }
    }

    public class LoggingMiddleware : IMiddleware
    {
        public void Handle(Request req, Action next) 
        { 
            Console.WriteLine("   Logged"); 
            next(); 
        }
    }

    public class ValidationMiddleware : IMiddleware
    {
        public void Handle(Request req, Action next) 
        { 
            Console.WriteLine("   Validated"); 
            next(); 
        }
    }

    // Mediator - Chat Room
    public interface IChatMediator
    {
        void SendMessage(string from, string message);
        void AddUser(ChatUser user);
    }

    public class ChatRoom : IChatMediator
    {
        private readonly List<ChatUser> _users = new();
        
        public void AddUser(ChatUser user) => _users.Add(user);
        
        public void SendMessage(string from, string message)
        {
            foreach (var user in _users)
                if (user.Name != from)
                    user.Receive(from, message);
        }
    }

    public class ChatUser
    {
        public string Name { get; }
        private readonly IChatMediator _mediator;
        
        public ChatUser(string name, IChatMediator mediator)
        {
            Name = name;
            _mediator = mediator;
            _mediator.AddUser(this);
        }
        
        public void Send(string message) => _mediator.SendMessage(Name, message);
        
        public void Receive(string from, string message) => 
            Console.WriteLine($"   {from}: {message}");
    }

    // Iterator - Product Collection
    public class Product
    {
        public string Name { get; }
        public decimal Price { get; }
        
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }

    public class ProductCollection : IEnumerable<Product>
    {
        private readonly List<Product> _products = new();
        
        public void Add(Product product) => _products.Add(product);
        
        public IEnumerator<Product> GetEnumerator() => _products.GetEnumerator();
        
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => 
            GetEnumerator();
    }
}