/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Delegates and Events - Custom EventArgs
 * FILE      : CustomEventArgs.cs
 * PURPOSE   : Creating custom EventArgs classes, reusable
 *            event data classes, and best practices for
 *            event data design
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._02_Delegates_Events
{
    class CustomEventArgs
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Custom EventArgs in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: What is EventArgs?
            // ═══════════════════════════════════════════════════════════

            // EventArgs is the base class for event data
            // Empty EventArgs used for events with no data
            // Custom EventArgs derived classes for specific data

            // ── EXAMPLE 1: Using Empty EventArgs ─────────────────────────
            Console.WriteLine("--- Empty EventArgs Usage ---");
            
            var button = new SimpleButtonEventArgs();
            
            button.Clicked += (sender, e) =>
                Console.WriteLine("  Button clicked!");
            
            button.Click();

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Creating Custom EventArgs
            // ═══════════════════════════════════════════════════════════

            // Custom EventArgs should derive from EventArgs
            // Include only read-only properties for immutable data
            // Use constructors to set required values

            // ── EXAMPLE 1: Basic Custom EventArgs ────────────────────────
            Console.WriteLine("\n--- Basic Custom EventArgs ---");
            
            var progress = new ProgressReporter();
            
            progress.TaskProgress += (sender, e) =>
            {
                Console.WriteLine($"  Task: {e.TaskName} - {e.PercentComplete}%");
                if (e.IsComplete)
                    Console.WriteLine("  Task completed!");
            };
            
            progress.UpdateTask("Downloading", 25);
            progress.UpdateTask("Downloading", 50);
            progress.UpdateTask("Downloading", 75);
            progress.UpdateTask("Downloading", 100);

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: EventArgs with Multiple Properties
            // ═══════════════════════════════════════════════════════════

            // Include all relevant data in EventArgs
            // Include source/timestamp for logging
            // Consider read-only for thread safety

            // ── EXAMPLE 1: Comprehensive EventArgs ─────────────────────
            Console.WriteLine("\n--- Comprehensive EventArgs ---");
            
            var order = new OrderProcessor();
            
            order.OrderProcessed += (sender, e) =>
            {
                Console.WriteLine($"  Order #{e.OrderId}");
                Console.WriteLine($"    Customer: {e.CustomerName}");
                Console.WriteLine($"    Total: ${e.TotalAmount:F2}");
                Console.WriteLine($"    Status: {e.Status}");
                Console.WriteLine($"    Processed at: {e.ProcessedAt}");
            };
            
            order.ProcessOrder("ORD-12345", "John Doe", 299.99m);

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Reusable EventArgs Classes
            // ═══════════════════════════════════════════════════════════

            // Design reusable EventArgs for common scenarios
            // Generic types can make them more flexible

            // ── EXAMPLE 1: Generic EventArgs<T> ─────────────────────────
            Console.WriteLine("\n--- Generic EventArgs ---");
            
            var cache = new CacheManager<string>();
            
            cache.ItemAdded += (sender, e) =>
                Console.WriteLine($"  Added: {e.Item}");
            
            cache.ItemRemoved += (sender, e) =>
                Console.WriteLine($"  Removed: {e.Item}");
            
            cache.Add("user1", "Alice");
            cache.Add("user2", "Bob");
            cache.Remove("user1");

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: EventArgs with Collection Data
            // ═══════════════════════════════════════════════════════════

            // Can include collections in EventArgs
            // Use defensive copying for thread safety

            // ── EXAMPLE 1: EventArgs with Collection ───────────────────
            Console.WriteLine("\n--- EventArgs with Collection ---");
            
            var fileProcessor = new FileProcessor();
            
            fileProcessor.FilesProcessed += (sender, e) =>
            {
                Console.WriteLine($"  Processed {e.ProcessedFiles.Count} files:");
                foreach (var file in e.ProcessedFiles)
                {
                    Console.WriteLine($"    - {file}");
                }
            };
            
            fileProcessor.ProcessFiles(new[] { "a.txt", "b.txt", "c.txt" });

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: EventArgs for Error Handling
            // ═══════════════════════════════════════════════════════════

            // Include error information in EventArgs
            // Can allow cancellation of operations

            // ── EXAMPLE 1: Error EventArgs ─────────────────────────────
            Console.WriteLine("\n--- Error EventArgs ---");
            
            var validator = new InputValidator();
            
            validator.ValidationFailed += (sender, e) =>
            {
                Console.WriteLine($"  Validation failed: {e.ErrorMessage}");
                Console.WriteLine($"    Field: {e.FieldName}");
                Console.WriteLine($"    Value: {e.InvalidValue}");
            };
            
            validator.Validate("email", "");
            validator.Validate("email", "invalid");
            validator.Validate("age", "-5");

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Real-World: E-Commerce Events
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Shopping Cart Events ──────────────────────────
            Console.WriteLine("\n--- Real-World: Shopping Cart ---");
            
            var cart = new ShoppingCart();
            
            cart.ItemAdded += (sender, e) =>
                Console.WriteLine($"  Added {e.Quantity}x {e.ProductName} (${e.UnitPrice})");
            
            cart.ItemRemoved += (sender, e) =>
                Console.WriteLine($"  Removed {e.ProductName}");
            
            cart.CartTotal += (sender, e) =>
                Console.WriteLine($"  Cart Total: ${e.Total:F2}");
            
            cart.AddItem("Laptop", 1, 999.99m);
            cart.AddItem("Mouse", 2, 29.99m);
            cart.RemoveItem("Mouse");
            cart.UpdateTotal(1029.98m);

            // ── EXAMPLE 2: Inventory Events ──────────────────────────────
            Console.WriteLine("\n--- Real-World: Inventory ---");
            
            var inventory = new InventoryManager();
            
            inventory.LowStock += (sender, e) =>
                Console.WriteLine($"  LOW STOCK: {e.ProductName} - Only {e.Quantity} left!");
            
            inventory.StockUpdated += (sender, e) =>
                Console.WriteLine($"  Updated {e.ProductName}: {e.OldQuantity} -> {e.NewQuantity}");
            
            inventory.UpdateStock("Widget", 100, 5);
            inventory.UpdateStock("Widget", 5, 3);

            Console.WriteLine("\n=== Custom EventArgs Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Simple button with EventArgs
    // ═══════════════════════════════════════════════════════════

    class SimpleButtonEventArgs
    {
        public event EventHandler Clicked;

        public void Click()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Progress EventArgs
    // ═══════════════════════════════════════════════════════════

    class ProgressChangedEventArgs : EventArgs
    {
        public string TaskName { get; }
        public int PercentComplete { get; }
        public bool IsComplete { get; }

        public ProgressChangedEventArgs(string taskName, int percent)
        {
            TaskName = taskName;
            PercentComplete = percent;
            IsComplete = percent >= 100;
        }
    }

    class ProgressReporter
    {
        public event EventHandler<ProgressChangedEventArgs> TaskProgress;

        public void UpdateTask(string taskName, int percent)
        {
            TaskProgress?.Invoke(this, new ProgressChangedEventArgs(taskName, percent));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Order EventArgs
    // ═══════════════════════════════════════════════════════════

    class OrderProcessedEventArgs : EventArgs
    {
        public string OrderId { get; }
        public string CustomerName { get; }
        public decimal TotalAmount { get; }
        public string Status { get; }
        public DateTime ProcessedAt { get; }

        public OrderProcessedEventArgs(string orderId, string customer, decimal total, string status)
        {
            OrderId = orderId;
            CustomerName = customer;
            TotalAmount = total;
            Status = status;
            ProcessedAt = DateTime.Now;
        }
    }

    class OrderProcessor
    {
        public event EventHandler<OrderProcessedEventArgs> OrderProcessed;

        public void ProcessOrder(string orderId, string customer, decimal total)
        {
            OrderProcessed?.Invoke(this, new OrderProcessedEventArgs(
                orderId, customer, total, "Completed"));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic EventArgs<T>
    // ═══════════════════════════════════════════════════════════

    class ItemEventArgs<T> : EventArgs
    {
        public string Key { get; }
        public T Item { get; }

        public ItemEventArgs(string key, T item)
        {
            Key = key;
            Item = item;
        }
    }

    class CacheManager<T>
    {
        private Dictionary<string, T> _cache = new Dictionary<string, T>();

        public event EventHandler<ItemEventArgs<T>> ItemAdded;
        public event EventHandler<ItemEventArgs<T>> ItemRemoved;

        public void Add(string key, T item)
        {
            _cache[key] = item;
            ItemAdded?.Invoke(this, new ItemEventArgs<T>(key, item));
        }

        public void Remove(string key)
        {
            if (_cache.TryGetValue(key, out var item))
            {
                _cache.Remove(key);
                ItemRemoved?.Invoke(this, new ItemEventArgs<T>(key, item));
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Files Processed EventArgs
    // ═══════════════════════════════════════════════════════════

    class FilesProcessedEventArgs : EventArgs
    {
        public List<string> ProcessedFiles { get; }
        public DateTime ProcessedAt { get; }

        public FilesProcessedEventArgs(IEnumerable<string> files)
        {
            // Create a defensive copy
            ProcessedFiles = new List<string>(files);
            ProcessedAt = DateTime.Now;
        }
    }

    class FileProcessor
    {
        public event EventHandler<FilesProcessedEventArgs> FilesProcessed;

        public void ProcessFiles(string[] files)
        {
            FilesProcessed?.Invoke(this, new FilesProcessedEventArgs(files));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Validation Failed EventArgs
    // ═══════════════════════════════════════════════════════════

    class ValidationFailedEventArgs : EventArgs
    {
        public string FieldName { get; }
        public string InvalidValue { get; }
        public string ErrorMessage { get; }

        public ValidationFailedEventArgs(string field, string value, string message)
        {
            FieldName = field;
            InvalidValue = value;
            ErrorMessage = message;
        }
    }

    class InputValidator
    {
        public event EventHandler<ValidationFailedEventArgs> ValidationFailed;

        public void Validate(string field, string value)
        {
            switch (field)
            {
                case "email":
                    if (string.IsNullOrEmpty(value) || !value.Contains("@"))
                    {
                        ValidationFailed?.Invoke(this, new ValidationFailedEventArgs(
                            field, value, "Invalid email address"));
                    }
                    break;
                case "age":
                    if (!int.TryParse(value, out var age) || age < 0)
                    {
                        ValidationFailed?.Invoke(this, new ValidationFailedEventArgs(
                            field, value, "Age must be a positive number"));
                    }
                    break;
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Shopping Cart EventArgs
    // ═══════════════════════════════════════════════════════════

    class CartItemEventArgs : EventArgs
    {
        public string ProductName { get; }
        public int Quantity { get; }
        public decimal UnitPrice { get; }

        public CartItemEventArgs(string product, int qty, decimal price)
        {
            ProductName = product;
            Quantity = qty;
            UnitPrice = price;
        }
    }

    class CartItemRemovedEventArgs : EventArgs
    {
        public string ProductName { get; }

        public CartItemRemovedEventArgs(string product)
        {
            ProductName = product;
        }
    }

    class CartTotalEventArgs : EventArgs
    {
        public decimal Total { get; }

        public CartTotalEventArgs(decimal total)
        {
            Total = total;
        }
    }

    class ShoppingCart
    {
        public event EventHandler<CartItemEventArgs> ItemAdded;
        public event EventHandler<CartItemRemovedEventArgs> ItemRemoved;
        public event EventHandler<CartTotalEventArgs> CartTotal;

        public void AddItem(string product, int qty, decimal price)
        {
            ItemAdded?.Invoke(this, new CartItemEventArgs(product, qty, price));
        }

        public void RemoveItem(string product)
        {
            ItemRemoved?.Invoke(this, new CartItemRemovedEventArgs(product));
        }

        public void UpdateTotal(decimal total)
        {
            CartTotal?.Invoke(this, new CartTotalEventArgs(total));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Inventory EventArgs
    // ═══════════════════════════════════════════════════════════

    class StockUpdatedEventArgs : EventArgs
    {
        public string ProductName { get; }
        public int OldQuantity { get; }
        public int NewQuantity { get; }

        public StockUpdatedEventArgs(string product, int oldQty, int newQty)
        {
            ProductName = product;
            OldQuantity = oldQty;
            NewQuantity = newQty;
        }
    }

    class LowStockEventArgs : EventArgs
    {
        public string ProductName { get; }
        public int Quantity { get; }

        public LowStockEventArgs(string product, int qty)
        {
            ProductName = product;
            Quantity = qty;
        }
    }

    class InventoryManager
    {
        private const int LowStockThreshold = 10;

        public event EventHandler<StockUpdatedEventArgs> StockUpdated;
        public event EventHandler<LowStockEventArgs> LowStock;

        public void UpdateStock(string product, int oldQty, int newQty)
        {
            StockUpdated?.Invoke(this, new StockUpdatedEventArgs(product, oldQty, newQty));
            
            if (newQty <= LowStockThreshold)
            {
                LowStock?.Invoke(this, new LowStockEventArgs(product, newQty));
            }
        }
    }
}
