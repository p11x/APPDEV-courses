/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Generics - Real-World Examples Part 2
 * FILE      : Generics_RealWorld_Part2.cs
 * PURPOSE   : Teaches more practical generic patterns: validators,
 *            mappers, builders, and event systems
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._01_Generics
{
    class Generics_RealWorld_Part2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Generics Real-World Part 2 ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Generic Validator
            // ═══════════════════════════════════════════════════════════

            // Using generic validator
            var validator = new Validator8();

            var product = new Product8 { Name = "Laptop", Price = 999.99m };
            var validationResult = validator.Validate(product, p => 
                !string.IsNullOrEmpty(p.Name) && p.Price > 0);

            Console.WriteLine($"Product valid: {validationResult.IsValid}");
            // Output: Product valid: True

            var invalidProduct = new Product8 { Name = "", Price = -10 };
            var invalidResult = validator.Validate(invalidProduct, p => 
                !string.IsNullOrEmpty(p.Name) && p.Price > 0);

            Console.WriteLine($"Invalid product valid: {invalidResult.IsValid}");
            // Output: Invalid product valid: False

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Generic Object Mapper
            // ═══════════════════════════════════════════════════════════

            // Using generic mapper
            var mapper = new ObjectMapper();

            // Map from DTO to Entity
            var productDto = new ProductDto { Id = 1, ProductName = "Phone", ProductPrice = 699.99 };
            var productEntity = mapper.Map<ProductDto, Product8>(productDto);
            Console.WriteLine($"Mapped product: {productEntity.Name}, Price: {productEntity.Price}");
            // Output: Mapped product: Phone, Price: 699.99

            // Map from Entity to ViewModel
            var productViewModel = mapper.Map<Product8, ProductViewModel>(productEntity);
            Console.WriteLine($"ViewModel: {productViewModel.DisplayName}, Formatted: {productViewModel.FormattedPrice}");
            // Output: ViewModel: Phone, Formatted: $699.99

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Generic Builder Pattern
            // ═══════════════════════════════════════════════════════════

            // Using fluent builder
            var emailBuilder = new EmailBuilder()
                .To("user@example.com")
                .Subject("Hello")
                .Body("Welcome to our service!");

            var email = emailBuilder.Build();
            Console.WriteLine($"Email - To: {email.To}, Subject: {email.Subject}");
            // Output: Email - To: user@example.com, Subject: Hello

            var orderBuilder = new OrderBuilder()
                .AddItem("Laptop", 1, 999.99m)
                .AddItem("Mouse", 2, 29.99m)
                .SetCustomer("John Doe")
                .SetShippingAddress("123 Main St");

            var order = orderBuilder.Build();
            Console.WriteLine($"Order - Customer: {order.Customer}, Total: ${order.Total}");
            // Output: Order - Customer: John Doe, Total: $1059.97

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Generic Event Aggregator
            // ═══════════════════════════════════════════════════════════

            // Using event aggregator
            var eventAggregator = new EventAggregator();

            // Subscribe to events
            eventAggregator.Subscribe<OrderCreatedEvent>(e => 
                Console.WriteLine($"Order created: {e.OrderId}"));

            eventAggregator.Subscribe<UserRegisteredEvent>(e => 
                Console.WriteLine($"User registered: {e.UserName}"));

            // Publish events
            eventAggregator.Publish(new OrderCreatedEvent { OrderId = 123, Total = 99.99m });
            // Output: Order created: 123

            eventAggregator.Publish(new UserRegisteredEvent { UserName = "alice" });
            // Output: User registered: alice

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Generic Dependency Injection Container
            // ═══════════════════════════════════════════════════════════

            // Simple DI container
            var container = new SimpleContainer();

            // Register services
            container.Register<ILogger8, ConsoleLogger>();
            container.Register<IStorage, FileStorage>();

            // Resolve services
            var logger = container.Resolve<ILogger8>();
            logger.Log("Application started");
            // Output: [CONSOLE] Application started

            var storage = container.Resolve<IStorage>();
            storage.Save("data.json", "content");
            // Output: [FILE] Saved data.json

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Generic Pipeline/Middleware
            // ═══════════════════════════════════════════════════════════

            // Using pipeline
            var pipeline = new RequestPipeline<RequestContext>();

            pipeline.Use((context, next) => {
                Console.WriteLine($"  [Auth] Processing request");
                context.Items["Authenticated"] = true;
                next();
            });

            pipeline.Use((context, next) => {
                Console.WriteLine($"  [Logging] Request processed");
                context.Items["Logged"] = true;
                next();
            });

            pipeline.Use((context, next) => {
                Console.WriteLine($"  [Final] Request completed");
                context.Items["Completed"] = true;
            });

            var context = new RequestContext();
            pipeline.Execute(context);
            // Output:
            //   [Auth] Processing request
            //   [Logging] Request processed
            //   [Final] Request completed

            Console.WriteLine("\n=== Generics Real-World Part 2 Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Validator
    // ═══════════════════════════════════════════════════════════

    class Validator8
    {
        public ValidationResult Validate<T>(T entity, Func<T, bool> validationFunc)
        {
            bool isValid = validationFunc(entity);
            return new ValidationResult 
            { 
                IsValid = isValid,
                ErrorMessage = isValid ? null : "Validation failed"
            };
        }
    }

    class ValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }

    class Product8
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Object Mapper
    // ═══════════════════════════════════════════════════════════

    class ObjectMapper
    {
        public TTarget Map<TSource, TTarget>(TSource source) 
            where TSource : class
            where TTarget : class, new()
        {
            var target = new TTarget();
            var sourceType = typeof(TSource);
            var targetType = typeof(TTarget);

            // Simple property mapping by convention
            foreach (var targetProp in targetType.GetProperties())
            {
                var sourceProp = sourceType.GetProperty(
                    targetProp.Name.Replace("Name", "ProductName")
                        .Replace("Price", "ProductPrice")
                );
                
                if (sourceProp != null && targetProp.CanWrite)
                {
                    targetProp.SetValue(target, sourceProp.GetValue(source));
                }
            }

            return target;
        }
    }

    class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
    }

    class ProductViewModel
    {
        public string DisplayName { get; set; }
        public string FormattedPrice { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Builder Pattern
    // ═══════════════════════════════════════════════════════════

    class Email
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    class EmailBuilder
    {
        private Email _email = new Email();

        public EmailBuilder To(string to) { _email.To = to; return this; }
        public EmailBuilder From(string from) { _email.From = from; return this; }
        public EmailBuilder Subject(string subject) { _email.Subject = subject; return this; }
        public EmailBuilder Body(string body) { _email.Body = body; return this; }
        public Email Build() => _email;
    }

    class OrderLine
    {
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    class Order
    {
        public string Customer { get; set; }
        public string ShippingAddress { get; set; }
        public List<OrderLine> Items { get; set; } = new List<OrderLine>();
        public decimal Total => Items.Sum(i => i.Quantity * i.UnitPrice);
    }

    class OrderBuilder
    {
        private Order _order = new Order();

        public OrderBuilder AddItem(string product, int quantity, decimal price)
        {
            _order.Items.Add(new OrderLine { Product = product, Quantity = quantity, UnitPrice = price });
            return this;
        }

        public OrderBuilder SetCustomer(string customer) { _order.Customer = customer; return this; }
        public OrderBuilder SetShippingAddress(string address) { _order.ShippingAddress = address; return this; }
        public Order Build() => _order;
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Event Aggregator
    // ═══════════════════════════════════════════════════════════

    interface IEvent { }

    class OrderCreatedEvent : IEvent
    {
        public int OrderId { get; set; }
        public decimal Total { get; set; }
    }

    class UserRegisteredEvent : IEvent
    {
        public string UserName { get; set; }
    }

    class EventAggregator
    {
        private Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();

        public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
        {
            var eventType = typeof(TEvent);
            if (!_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType] = new List<Delegate>();
            }
            _subscribers[eventType].Add(handler);
        }

        public void Publish<TEvent>(TEvent eventToPublish) where TEvent : IEvent
        {
            var eventType = typeof(TEvent);
            if (_subscribers.ContainsKey(eventType))
            {
                foreach (var handler in _subscribers[eventType])
                {
                    ((Action<TEvent>)handler)(eventToPublish);
                }
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Dependency Injection Container
    // ═══════════════════════════════════════════════════════════

    interface ILogger8 { void Log(string message); }
    interface IStorage { void Save(string filename, string content); }

    class ConsoleLogger : ILogger8
    {
        public void Log(string message) => Console.WriteLine($"[CONSOLE] {message}");
    }

    class FileStorage : IStorage
    {
        public void Save(string filename, string content) => 
            Console.WriteLine($"[FILE] Saved {filename}");
    }

    class SimpleContainer
    {
        private Dictionary<Type, Type> _registrations = new Dictionary<Type, Type>();

        public void Register<TInterface, TImplementation>() 
            where TImplementation : class, TInterface, new()
        {
            _registrations[typeof(TInterface)] = typeof(TImplementation);
        }

        public TInterface Resolve<TInterface>() where TInterface : class
        {
            if (_registrations.TryGetValue(typeof(TInterface), out var implType))
            {
                return (TInterface)Activator.CreateInstance(implType);
            }
            throw new InvalidOperationException($"Type {typeof(TInterface)} not registered");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Pipeline/Middleware
    // ═══════════════════════════════════════════════════════════

    class RequestContext
    {
        public Dictionary<string, object> Items { get; set; } = new Dictionary<string, object>();
    }

    class RequestPipeline<TContext> where TContext : class, new()
    {
        private List<Action<TContext, Action>> _middlewares = new List<Action<TContext, Action>>();

        public void Use(Action<TContext, Action> middleware)
        {
            _middlewares.Add(middleware);
        }

        public void Execute(TContext context)
        {
            int index = 0;

            Action next = () =>
            {
                if (index < _middlewares.Count)
                {
                    var middleware = _middlewares[index++];
                    middleware(context, next);
                }
            };

            next();
        }
    }
}