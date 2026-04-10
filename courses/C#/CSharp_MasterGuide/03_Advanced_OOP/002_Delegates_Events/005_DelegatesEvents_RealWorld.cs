/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Delegates and Events - Real-World Examples
 * FILE      : DelegatesEvents_RealWorld.cs
 * PURPOSE   : Real-world applications of delegates and events
 *            including observer pattern, callback systems,
 *            and event bus implementations
 * ============================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._02_Delegates_Events
{
    class DelegatesEvents_RealWorld
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Delegates & Events: Real-World Examples ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Observer Pattern with Events
            // ═══════════════════════════════════════════════════════════

            // Observer pattern allows one-to-many notification
            // Subject notifies all observers when state changes
            // Events provide built-in support for this pattern

            // ── EXAMPLE 1: Stock Market Observer ───────────────────────────
            Console.WriteLine("--- Observer Pattern: Stock Market ---");
            
            var stock = new Stock("AAPL", 150.00m);
            
            // Multiple observers subscribe to stock changes
            stock.PriceChanged += new PriceAlertObserver().OnPriceChanged;
            stock.PriceChanged += new ChartObserver().OnPriceChanged;
            stock.PriceChanged += new NewsObserver().OnPriceChanged;
            
            // Simulate price changes
            stock.Price = 152.50m;
            stock.Price = 148.00m;
            stock.Price = 155.00m;

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Callback System
            // ═══════════════════════════════════════════════════════════

            // Callbacks enable asynchronous operations
            // Delegates passed as parameters for completion handling
            // Common in I/O, networking, and long-running operations

            // ── EXAMPLE 1: File Download with Callbacks ────────────────────
            Console.WriteLine("\n--- Callback System: File Download ---");
            
            var downloader = new FileDownloaderCallback();
            
            // Set up callbacks
            downloader.OnProgressChanged = (percent) =>
                Console.WriteLine($"  Download: {percent}%");
            
            downloader.OnDownloadComplete = (success, error) =>
            {
                if (success)
                    Console.WriteLine("  Download completed successfully");
                else
                    Console.WriteLine($"  Download failed: {error}");
            };
            
            downloader.Download("http://example.com/file.zip");

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Event Bus / Message Center
            // ═══════════════════════════════════════════════════════════

            // Event bus provides loose coupling between components
            // Centralized event handling
            // Useful for cross-cutting concerns

            // ── EXAMPLE 1: Simple Event Bus ────────────────────────────────
            Console.WriteLine("\n--- Event Bus Pattern ---");
            
            var eventBus = EventBus.Instance;
            
            // Subscribe to specific event types
            eventBus.Subscribe<UserRegisteredEvent>(OnUserRegistered);
            eventBus.Subscribe<UserLoggedInEvent>(OnUserLoggedIn);
            eventBus.Subscribe<OrderPlacedEvent>(OnOrderPlaced);
            
            // Publish events
            eventBus.Publish(new UserRegisteredEvent("alice", "alice@example.com"));
            eventBus.Publish(new UserLoggedInEvent("alice"));
            eventBus.Publish(new OrderPlacedEvent("ORD-001", 99.99m));

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Dependency Injection with Delegates
            // ═══════════════════════════════════════════════════════════

            // Use delegates for strategy pattern
            // Enables runtime selection of algorithms
            // Common in validation, sorting, transformation

            // ── EXAMPLE 1: Strategy Pattern with Delegates ────────────────
            Console.WriteLine("\n--- Strategy Pattern: Sorting ---");
            
            var sorter = new SmartSorter();
            
            // Using lambda for custom sorting
            sorter.Sort(new[] { 5, 2, 8, 1, 9 }, (a, b) => a.CompareTo(b));
            // Output: 1 2 5 8 9
            
            // Reverse sorting
            sorter.Sort(new[] { 5, 2, 8, 1, 9 }, (a, b) => b.CompareTo(a));
            // Output: 9 8 5 2 1

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Pipeline Pattern
            // ═══════════════════════════════════════════════════════════

            // Chain processing steps using delegates
            // Each step transforms data
            // Common in data processing, validation chains

            // ── EXAMPLE 1: Processing Pipeline ─────────────────────────────
            Console.WriteLine("\n--- Pipeline Pattern ---");
            
            var pipeline = new ProcessingPipeline();
            
            // Build pipeline with middleware
            pipeline.Use(input => input.Trim());
            pipeline.Use(input => input.ToUpper());
            pipeline.Use(input => $"PROCESSED: {input}");
            
            // Execute pipeline
            var result = pipeline.Execute("  hello world  ");
            Console.WriteLine($"  Result: {result}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Command Pattern with Delegates
            // ═══════════════════════════════════════════════════════════

            // Encapsulate operations as objects
            // Delegates can represent commands
            // Supports undo/redo, queuing

            // ── EXAMPLE 1: Command Queue ───────────────────────────────────
            Console.WriteLine("\n--- Command Pattern: Command Queue ---");
            
            var commandQueue = new CommandQueue();
            
            // Enqueue commands
            commandQueue.Enqueue(() => Console.WriteLine("  Command 1: Open file"));
            commandQueue.Enqueue(() => Console.WriteLine("  Command 2: Read data"));
            commandQueue.Enqueue(() => Console.WriteLine("  Command 3: Close file"));
            
            // Execute all
            commandQueue.ExecuteAll();

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Real-World: Notification Service
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Multi-Channel Notification ────────────────────
            Console.WriteLine("\n--- Real-World: Notification Service ---");
            
            var notificationService = new NotificationServiceRealWorld();
            
            // Register handlers for different channels
            notificationService.RegisterHandler(Channel.Email, msg =>
                Console.WriteLine($"  EMAIL to {msg.To}: {msg.Subject}"));
            
            notificationService.RegisterHandler(Channel.SMS, msg =>
                Console.WriteLine($"  SMS to {msg.To}: {msg.Body}"));
            
            notificationService.RegisterHandler(Channel.Push, msg =>
                Console.WriteLine($"  PUSH to {msg.To}: {msg.Body}"));
            
            // Send notifications
            notificationService.Send(new NotificationMessage
            {
                To = "user@example.com",
                Subject = "Welcome!",
                Body = "Welcome to our service"
            });

            Console.WriteLine("\n=== Real-World Examples Complete ===");
        }

        // Event handlers for event bus
        static void OnUserRegistered(UserRegisteredEvent e) =>
            Console.WriteLine($"  [Observer] New user: {e.Email}");

        static void OnUserLoggedIn(UserLoggedInEvent e) =>
            Console.WriteLine($"  [Observer] User logged in: {e.Username}");

        static void OnOrderPlaced(OrderPlacedEvent e) =>
            Console.WriteLine($"  [Observer] Order placed: {e.OrderId} - ${e.Amount}");
    }

    // ═══════════════════════════════════════════════════════════
    // Observer Pattern: Stock
    // ═══════════════════════════════════════════════════════════

    class StockPriceChangedEventArgs : EventArgs
    {
        public string Symbol { get; }
        public decimal OldPrice { get; }
        public decimal NewPrice { get; }
        public decimal Change => NewPrice - OldPrice;
        public decimal ChangePercent => (Change / OldPrice) * 100;

        public StockPriceChangedEventArgs(string symbol, decimal oldPrice, decimal newPrice)
        {
            Symbol = symbol;
            OldPrice = oldPrice;
            NewPrice = newPrice;
        }
    }

    class Stock
    {
        private decimal _price;

        public string Symbol { get; }
        
        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    var oldPrice = _price;
                    _price = value;
                    OnPriceChanged(new StockPriceChangedEventArgs(Symbol, oldPrice, value));
                }
            }
        }

        public event EventHandler<StockPriceChangedEventArgs> PriceChanged;

        public Stock(string symbol, decimal price)
        {
            Symbol = symbol;
            _price = price;
        }

        protected virtual void OnPriceChanged(StockPriceChangedEventArgs e)
        {
            PriceChanged?.Invoke(this, e);
        }
    }

    // Observer classes
    class PriceAlertObserver
    {
        public void OnPriceChanged(object sender, StockPriceChangedEventArgs e)
        {
            if (Math.Abs(e.ChangePercent) > 5)
            {
                Console.WriteLine($"  [ALERT] {e.Symbol} changed {e.ChangePercent:F2}%");
            }
        }
    }

    class ChartObserver
    {
        public void OnPriceChanged(object sender, StockPriceChangedEventArgs e)
        {
            Console.WriteLine($"  [CHART] {e.Symbol}: ${e.OldPrice} -> ${e.NewPrice}");
        }
    }

    class NewsObserver
    {
        public void OnPriceChanged(object sender, StockPriceChangedEventArgs e)
        {
            if (e.ChangePercent > 3)
                Console.WriteLine($"  [NEWS] {e.Symbol} is up! Trading volume high.");
            else if (e.ChangePercent < -3)
                Console.WriteLine($"  [NEWS] {e.Symbol} drops. Market concern.");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Callback System: File Downloader
    // ═══════════════════════════════════════════════════════════

    class FileDownloaderCallback
    {
        public Action<int> OnProgressChanged { get; set; }
        public Action<bool, string> OnDownloadComplete { get; set; }

        public void Download(string url)
        {
            // Simulate download with progress
            for (int i = 0; i <= 100; i += 20)
            {
                OnProgressChanged?.Invoke(i);
                System.Threading.Thread.Sleep(50);
            }
            
            OnDownloadComplete?.Invoke(true, null);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Event Bus Implementation
    // ═══════════════════════════════════════════════════════════

    interface IEvent { }

    class UserRegisteredEvent : IEvent
    {
        public string Username { get; }
        public string Email { get; }
        
        public UserRegisteredEvent(string username, string email)
        {
            Username = username;
            Email = email;
        }
    }

    class UserLoggedInEvent : IEvent
    {
        public string Username { get; }
        
        public UserLoggedInEvent(string username)
        {
            Username = username;
        }
    }

    class OrderPlacedEvent : IEvent
    {
        public string OrderId { get; }
        public decimal Amount { get; }
        
        public OrderPlacedEvent(string orderId, decimal amount)
        {
            OrderId = orderId;
            Amount = amount;
        }
    }

    class EventBus
    {
        private static EventBus _instance;
        private static readonly object _lock = new object();
        
        private Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();

        public static EventBus Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new EventBus();
                    }
                }
                return _instance;
            }
        }

        public void Subscribe<T>(Action<T> handler) where T : IEvent
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
            {
                _subscribers[type] = new List<Delegate>();
            }
            _subscribers[type].Add(handler);
        }

        public void Publish<T>(T @event) where T : IEvent
        {
            var type = typeof(T);
            if (_subscribers.ContainsKey(type))
            {
                foreach (var handler in _subscribers[type])
                {
                    ((Action<T>)handler)(@event);
                }
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Strategy Pattern: Smart Sorter
    // ═══════════════════════════════════════════════════════════

    class SmartSorter
    {
        public void Sort(int[] numbers, Comparison<int> comparison)
        {
            Array.Sort(numbers, comparison);
            Console.WriteLine($"  Sorted: {string.Join(", ", numbers)}");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Pipeline Pattern
    // ═══════════════════════════════════════════════════════════

    class ProcessingPipeline
    {
        private List<Func<string, string>> _middleware = new List<Func<string, string>>();

        public void Use(Func<string, string> middleware)
        {
            _middleware.Add(middleware);
        }

        public string Execute(string input)
        {
            var result = input;
            foreach (var middleware in _middleware)
            {
                result = middleware(result);
            }
            return result;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Command Pattern: Command Queue
    // ═══════════════════════════════════════════════════════════

    class CommandQueue
    {
        private Queue<Action> _commands = new Queue<Action>();

        public void Enqueue(Action command)
        {
            _commands.Enqueue(command);
        }

        public void ExecuteAll()
        {
            while (_commands.Count > 0)
            {
                var command = _commands.Dequeue();
                command();
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: Notification Service
    // ═══════════════════════════════════════════════════════════

    enum Channel { Email, SMS, Push }

    class NotificationMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    class NotificationServiceRealWorld
    {
        private Dictionary<Channel, Action<NotificationMessage>> _handlers = new Dictionary<Channel, Action<NotificationMessage>>();

        public void RegisterHandler(Channel channel, Action<NotificationMessage> handler)
        {
            _handlers[channel] = handler;
        }

        public void Send(NotificationMessage message)
        {
            foreach (var handler in _handlers.Values)
            {
                handler(message);
            }
        }
    }
}
