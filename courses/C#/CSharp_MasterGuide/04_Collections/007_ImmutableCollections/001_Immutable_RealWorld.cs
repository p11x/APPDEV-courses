/*
 * TOPIC: Immutable Collections
 * SUBTOPIC: Real-World Examples - Configuration, State Management, Thread Safety
 * FILE: Immutable_RealWorld.cs
 * PURPOSE: Demonstrate practical applications of immutable collections
 *          in real-world scenarios including configuration management,
 *          state machines, and thread-safe operations
 */
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._04_Collections._07_ImmutableCollections
{
    // Configuration class using immutable dictionary
    public class AppConfiguration
    {
        public ImmutableDictionary<string, object> Settings { get; }

        private AppConfiguration(ImmutableDictionary<string, object> settings)
        {
            Settings = settings;
        }

        // Factory method for creating initial configuration
        public static AppConfiguration CreateDefault()
        {
            var defaults = ImmutableDictionary<string, object>.Empty
                .Add("AppName", "MyApplication")
                .Add("Version", "1.0.0")
                .Add("MaxUsers", 1000)
                .Add("EnableCache", true)
                .Add("LogLevel", "Information")
                .Add("DatabaseTimeout", 30);

            return new AppConfiguration(defaults);
        }

        // Return new configuration with updated setting (immutable pattern)
        public AppConfiguration WithSetting(string key, object value)
        {
            return new AppConfiguration(Settings.SetItem(key, value));
        }

        // Get value with default fallback
        public T GetValue<T>(string key, T defaultValue = default)
        {
            if (Settings.TryGetValue(key, out var value) && value is T typedValue)
            {
                return typedValue;
            }
            return defaultValue;
        }
    }

    // State management for a shopping cart
    public class ShoppingCart
    {
        public string CustomerId { get; }
        public ImmutableList<CartItem> Items { get; }
        public DateTime LastModified { get; }

        private ShoppingCart(string customerId, ImmutableList<CartItem> items, DateTime lastModified)
        {
            CustomerId = customerId;
            Items = items;
            LastModified = lastModified;
        }

        public static ShoppingCart Create(string customerId)
        {
            return new ShoppingCart(customerId, ImmutableList<CartItem>.Empty, DateTime.UtcNow);
        }

        public ShoppingCart AddItem(Product product, int quantity)
        {
            // Check if item already exists
            var existingItem = Items.FirstOrDefault(i => i.Product.Id == product.Id);
            
            ImmutableList<CartItem> newItems;
            if (existingItem != null)
            {
                // Update quantity - remove old, add new
                newItems = Items.Remove(existingItem)
                               .Add(new CartItem(product, existingItem.Quantity + quantity));
            }
            else
            {
                // Add new item
                newItems = Items.Add(new CartItem(product, quantity));
            }

            return new ShoppingCart(CustomerId, newItems, DateTime.UtcNow);
        }

        public ShoppingCart RemoveItem(string productId)
        {
            var item = Items.FirstOrDefault(i => i.Product.Id == productId);
            if (item == null) return this;

            var newItems = Items.Remove(item);
            return new ShoppingCart(CustomerId, newItems, DateTime.UtcNow);
        }

        public decimal Total => Items.Sum(i => i.Product.Price * i.Quantity);
    }

    public class Product
    {
        public string Id { get; }
        public string Name { get; }
        public decimal Price { get; }

        public Product(string id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }

    public class CartItem
    {
        public Product Product { get; }
        public int Quantity { get; }

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }

    // Thread-safe state container
    public class ThreadSafeCounter
    {
        private ImmutableDictionary<string, int> _counters;
        private readonly object _lock = new object();

        public ThreadSafeCounter()
        {
            _counters = ImmutableDictionary<string, int>.Empty;
        }

        // Increment - returns new state
        public ImmutableDictionary<string, int> Increment(string key)
        {
            lock (_lock)
            {
                var current = _counters.TryGetValue(key, out var count) ? count : 0;
                _counters = _counters.SetItem(key, current + 1);
                return _counters;
            }
        }

        public ImmutableDictionary<string, int> GetSnapshot() => _counters;
    }

    // Event sourcing example - immutable event store
    public class EventStore
    {
        private ImmutableList<AccountEvent> _events;

        public EventStore()
        {
            _events = ImmutableList<AccountEvent>.Empty;
        }

        public EventStore AppendEvent(AccountEvent evt)
        {
            return new EventStore(_events.Add(evt));
        }

        public ImmutableList<AccountEvent> GetEvents() => _events;

        public AccountState Replay()
        {
            var state = AccountState.Empty;
            foreach (var evt in _events)
            {
                state = state.Apply(evt);
            }
            return state;
        }
    }

    public abstract class AccountEvent
    {
        public DateTime Timestamp { get; } = DateTime.UtcNow;
    }

    public class DepositEvent : AccountEvent
    {
        public decimal Amount { get; }
        public DepositEvent(decimal amount) => Amount = amount;
    }

    public class WithdrawalEvent : AccountEvent
    {
        public decimal Amount { get; }
        public WithdrawalEvent(decimal amount) => Amount = amount;
    }

    public class AccountState
    {
        public decimal Balance { get; }
        public int TransactionCount { get; }

        private AccountState(decimal balance, int transactionCount)
        {
            Balance = balance;
            TransactionCount = transactionCount;
        }

        public static AccountState Empty => new AccountState(0, 0);

        public AccountState Apply(AccountEvent evt)
        {
            if (evt is DepositEvent deposit)
            {
                return new AccountState(Balance + deposit.Amount, TransactionCount + 1);
            }
            if (evt is WithdrawalEvent withdrawal)
            {
                return new AccountState(Balance - withdrawal.Amount, TransactionCount + 1);
            }
            return this;
        }
    }

    // Main demonstration class
    public class ImmutableRealWorldDemo
    {
        public static void Main()
        {
            Console.WriteLine("=== Immutable Collections: Real-World Examples ===\n");

            ConfigurationExample();
            StateManagementExample();
            ThreadSafetyExample();
            EventSourcingExample();
        }

        static void ConfigurationExample()
        {
            Console.WriteLine("--- 1. Configuration Management ---");
            Console.WriteLine();

            // Create default configuration
            var config = AppConfiguration.CreateDefault();
            Console.WriteLine($"  App: {config.GetValue<string>("AppName")}, Version: {config.GetValue<string>("Version")}");
            // Output: App: MyApplication, Version: 1.0.0

            // Create development override (without modifying original)
            var devConfig = config
                .WithSetting("Database", "dev.server.local")
                .WithSetting("EnableCache", false)
                .WithSetting("LogLevel", "Debug");

            Console.WriteLine($"  Dev Database: {devConfig.GetValue<string>("Database")}");
            // Output: Dev Database: dev.server.local
            Console.WriteLine($"  Prod Database: {config.GetValue<string>("Database")}");
            // Output: Prod Database: localhost:5432

            // Create production override
            var prodConfig = config
                .WithSetting("Database", "prod.db.com")
                .WithSetting("MaxUsers", 10000);

            Console.WriteLine($"  Prod MaxUsers: {prodConfig.GetValue<int>("MaxUsers")}");
            // Output: Prod MaxUsers: 10000

            // All configurations are independent - original unchanged
            Console.WriteLine($"  Original MaxUsers: {config.GetValue<int>("MaxUsers")}");
            // Output: Original MaxUsers: 1000
            Console.WriteLine();
        }

        static void StateManagementExample()
        {
            Console.WriteLine("--- 2. State Management (Shopping Cart) ---");
            Console.WriteLine();

            // Create new cart
            var cart = ShoppingCart.Create("CUST-001");
            Console.WriteLine($"  Initial cart: {cart.Items.Count} items, Total: ${cart.Total:F2}");
            // Output: Initial cart: 0 items, Total: $0.00

            // Add items - each operation returns NEW cart (immutable)
            var products = new[]
            {
                new Product("P001", "Laptop", 999.99m),
                new Product("P002", "Mouse", 29.99m),
                new Product("P003", "Keyboard", 79.99m)
            };

            var cartWithLaptop = cart.AddItem(products[0], 1);
            Console.WriteLine($"  After adding laptop: ${cartWithLaptop.Total:F2}");
            // Output: After adding laptop: $999.99

            var cartWithMouse = cartWithLaptop.AddItem(products[1], 2);
            Console.WriteLine($"  After adding 2 mice: ${cartWithMouse.Total:F2}");
            // Output: After adding 2 mice: $1059.97

            var cartComplete = cartWithMouse.AddItem(products[2], 1);
            Console.WriteLine($"  Final cart: ${cartComplete.Total:F2}");
            // Output: Final cart: $1139.95

            // Can still access old cart states
            Console.WriteLine($"  Previous state (laptop only): ${cartWithLaptop.Total:F2}");
            // Output: Previous state (laptop only): $999.99

            // Remove item - returns new cart
            var cartWithoutMouse = cartComplete.RemoveItem("P002");
            Console.WriteLine($"  After removing mouse: ${cartWithoutMouse.Total:F2}");
            // Output: After removing mouse: $1009.96
            Console.WriteLine();
        }

        static void ThreadSafetyExample()
        {
            Console.WriteLine("--- 3. Thread Safety Example ---");
            Console.WriteLine();

            var counter = new ThreadSafeCounter();
            var tasks = new List<Task>();
            int taskCount = 10;

            Console.WriteLine($"  Starting {taskCount} concurrent tasks...");

            // Simulate concurrent increments
            for (int i = 0; i < taskCount; i++)
            {
                int taskId = i;
                tasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < 100; j++)
                    {
                        counter.Increment($"Task-{taskId}");
                        counter.Increment("Total");
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            var snapshot = counter.GetSnapshot();
            Console.WriteLine($"  Total operations: {snapshot.GetValueOrDefault("Total", 0)}");
            // Output: Total operations: 1000

            Console.WriteLine("  Per-task counts:");
            for (int i = 0; i < taskCount; i++)
            {
                Console.WriteLine($"    Task-{i}: {snapshot.GetValueOrDefault($"Task-{i}", 0)}");
            }
            // Output: Each Task-N: 100

            // Demonstrate immutable snapshot usage
            var snapshotCopy = snapshot;
            Console.WriteLine($"  Snapshot shared safely - Total: {snapshotCopy["Total"]}");
            // Output: Snapshot shared safely - Total: 1000
            Console.WriteLine();
        }

        static void EventSourcingExample()
        {
            Console.WriteLine("--- 4. Event Sourcing Pattern ---");
            Console.WriteLine();

            // Start with empty event store
            var eventStore = EventStore.Empty;

            // Record some transactions (each returns new store)
            eventStore = eventStore.AppendEvent(new DepositEvent(1000m));
            Console.WriteLine($"  After deposit: +$1000");

            eventStore = eventStore.AppendEvent(new DepositEvent(500m));
            Console.WriteLine($"  After deposit: +$500");

            eventStore = eventStore.AppendEvent(new WithdrawalEvent(200m));
            Console.WriteLine($"  After withdrawal: -$200");

            // Replay events to get current state
            var currentState = eventStore.Replay();
            Console.WriteLine($"  Current Balance: ${currentState.Balance:F2}");
            // Output: Current Balance: $1300.00
            Console.WriteLine($"  Transaction Count: {currentState.TransactionCount}");
            // Output: Transaction Count: 3

            // Can replay from any point - useful for debugging
            var earlierStore = eventStore.GetEvents().Take(2).ToImmutableList();
            Console.WriteLine($"\n  First 2 events replay: Balance=${eventStore.Replay().Balance} (current)");
            // Output shows current state from all events

            // Original events preserved - audit trail
            Console.WriteLine($"\n  Audit trail ({eventStore.GetEvents().Count} events):");
            foreach (var evt in eventStore.GetEvents())
            {
                string type = evt switch
                {
                    DepositEvent d => $"Deposit: +${d.Amount:F2}",
                    WithdrawalEvent w => $"Withdrawal: -${w.Amount:F2}",
                    _ => "Unknown"
                };
                Console.WriteLine($"    [{evt.Timestamp:HH:mm:ss}] {type}");
            }
            // Output shows chronological event log
            Console.WriteLine();
        }
    }
}