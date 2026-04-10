/*
 * TOPIC: ObservableCollection<T> Real-World Applications
 * SUBTOPIC: Data Binding, UI Updates, Live Data Feeds
 * FILE: Observable_RealWorld.cs
 * PURPOSE: Demonstrate practical real-world applications of ObservableCollection
 *          including data binding patterns, UI synchronization, and live data streams
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace CSharp_MasterGuide._04_Collections._06_ObservableCollection
{
    public class StockQuote : INotifyPropertyChanged
    {
        private string _symbol;
        private decimal _price;
        private decimal _change;
        private DateTime _lastUpdated;

        public string Symbol
        {
            get => _symbol;
            set { _symbol = value; OnPropertyChanged(nameof(Symbol)); }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(PriceDisplay));
            }
        }

        public decimal Change
        {
            get => _change;
            set
            {
                _change = value;
                OnPropertyChanged(nameof(Change));
                OnPropertyChanged(nameof(ChangeDisplay));
                OnPropertyChanged(nameof(IsPositive));
            }
        }

        public DateTime LastUpdated
        {
            get => _lastUpdated;
            set { _lastUpdated = value; OnPropertyChanged(nameof(LastUpdated)); }
        }

        public string PriceDisplay => $"${Price:F2}";
        public string ChangeDisplay => $"{Change:+0.00;-0.00;0.00}%";
        public bool IsPositive => Change >= 0;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString() => $"{Symbol}: {PriceDisplay} ({ChangeDisplay})";
    }

    public class ChatMessage : INotifyPropertyChanged
    {
        private string _sender;
        private string _content;
        private DateTime _timestamp;
        private bool _isRead;

        public string Sender
        {
            get => _sender;
            set { _sender = value; OnPropertyChanged(nameof(Sender)); }
        }

        public string Content
        {
            get => _content;
            set { _content = value; OnPropertyChanged(nameof(Content)); }
        }

        public DateTime Timestamp
        {
            get => _timestamp;
            set { _timestamp = value; OnPropertyChanged(nameof(Timestamp)); }
        }

        public bool IsRead
        {
            get => _isRead;
            set { _isRead = value; OnPropertyChanged(nameof(IsRead)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString() => $"[{Timestamp:HH:mm}] {Sender}: {Content}";
    }

    public class ShoppingCartItem
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;

        public override string ToString() => $"{ProductName} x{Quantity} = ${Total:F2}";
    }

    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }

        public override string ToString() => $"[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level}] {Message}";
    }

    public class TaskItem : INotifyPropertyChanged
    {
        private string _title;
        private string _priority;
        private bool _isCompleted;
        private DateTime? _completedDate;

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        public string Priority
        {
            get => _priority;
            set { _priority = value; OnPropertyChanged(nameof(Priority)); }
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                _completedDate = value ? DateTime.Now : (DateTime?)null;
                OnPropertyChanged(nameof(IsCompleted));
                OnPropertyChanged(nameof(CompletedDate));
            }
        }

        public DateTime? CompletedDate => _completedDate;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString() => $"{(IsCompleted ? "[x]" : "[ ]")} {Title} ({Priority})";
    }

    public class ObservableRealWorld
    {
        public static void Main()
        {
            Console.WriteLine("=== ObservableCollection Real-World Examples ===\n");

            StockMarketExample();
            ChatApplicationExample();
            ShoppingCartExample();
            LogViewerExample();
            TaskTrackerExample();
            LiveDataFeedSimulation();
        }

        static void StockMarketExample()
        {
            Console.WriteLine("--- Real-World: Stock Market Watcher ---");
            Console.WriteLine();

            var watchList = new ObservableCollection<StockQuote>
            {
                new StockQuote { Symbol = "AAPL", Price = 175.50m, Change = 2.35m, LastUpdated = DateTime.Now },
                new StockQuote { Symbol = "GOOGL", Price = 140.25m, Change = -1.15m, LastUpdated = DateTime.Now },
                new StockQuote { Symbol = "MSFT", Price = 378.90m, Change = 5.20m, LastUpdated = DateTime.Now }
            };

            // Observe price changes
            int changeCount = 0;
            foreach (var quote in watchList)
            {
                quote.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(StockQuote.Price))
                    {
                        changeCount++;
                        Console.WriteLine($"  [UPDATE #{changeCount}] {quote.Symbol} price changed to {quote.PriceDisplay}");
                    }
                };
            }

            Console.WriteLine("  Initial watchlist:");
            foreach (var q in watchList)
            {
                Console.WriteLine($"    {q}");
                Console.WriteLine($"      Status: {(q.IsPositive ? "▲ Positive" : "▼ Negative")}");
            }
            // Output: Initial watchlist:
            //   AAPL: $175.50 (+2.35%)
            //     Status: ▲ Positive
            //   GOOGL: $140.25 (-1.15%)
            //     Status: ▼ Negative
            //   MSFT: $378.90 (+5.20%)
            //     Status: ▲ Positive

            Console.WriteLine("\n  Simulating market updates...");

            // Simulate price updates
            watchList[0].Price = 177.85m;
            watchList[0].Change = 3.70m;
            // Output: [UPDATE #1] AAPL price changed to $177.85

            watchList[1].Price = 141.50m;
            watchList[1].Change = 0.15m;
            // Output: [UPDATE #2] GOOGL price changed to $141.50

            watchList[2].Price = 375.00m;
            watchList[2].Change = 3.15m;
            // Output: [UPDATE #3] MSFT price changed to $375.00

            Console.WriteLine("\n  Update watchlist (sorted by change):");
            foreach (var q in watchList.OrderByDescending(x => x.Change))
            {
                Console.WriteLine($"    {q}");
            }
            // Output: Update watchlist (sorted by change):
            //   AAPL: $177.85 (+3.70%)
            //   MSFT: $375.00 (+3.15%)
            //   GOOGL: $141.50 (+0.15%)
            Console.WriteLine();
        }

        static void ChatApplicationExample()
        {
            Console.WriteLine("--- Real-World: Chat Application ---");
            Console.WriteLine();

            var messages = new ObservableCollection<ChatMessage>();

            // Track new messages
            int messageCount = 0;
            messages.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    var msg = e.NewItems[0] as ChatMessage;
                    messageCount++;
                    Console.WriteLine($"  [Notification] New message #{messageCount}");
                }
            };

            // Simulate incoming messages
            var incomingMessages = new[]
            {
                new ChatMessage { Sender = "Alice", Content = "Hey everyone!", Timestamp = DateTime.Now.AddMinutes(-5) },
                new ChatMessage { Sender = "Bob", Content = "Hi Alice, how are you?", Timestamp = DateTime.Now.AddMinutes(-4) },
                new ChatMessage { Sender = "Charlie", Content = "Working on the new feature.", Timestamp = DateTime.Now.AddMinutes(-3) },
                new ChatMessage { Sender = "Alice", Content = "Same here, almost done!", Timestamp = DateTime.Now.AddMinutes(-2) },
                new ChatMessage { Sender = "Bob", Content = "Great, let's sync up later.", Timestamp = DateTime.Now.AddMinutes(-1) }
            };

            foreach (var msg in incomingMessages)
            {
                messages.Add(msg);
            }

            Console.WriteLine($"  Conversation ({messages.Count} messages):");
            foreach (var m in messages)
            {
                Console.WriteLine($"    {m}");
            }
            // Output: Conversation (5 messages):
            //   [14:55] Alice: Hey everyone!
            //   [14:56] Bob: Hi Alice, how are you?
            //   [14:57] Charlie: Working on the new feature.
            //   [14:58] Alice: Same here, almost done!
            //   [14:59] Bob: Great, let's sync up later.

            // Mark messages as read
            foreach (var m in messages)
            {
                m.IsRead = true;
            }

            var unreadCount = messages.Count(m => !m.IsRead);
            Console.WriteLine($"\n  Unread messages: {unreadCount}");
            // Output: Unread messages: 0
            Console.WriteLine();
        }

        static void ShoppingCartExample()
        {
            Console.WriteLine("--- Real-World: Shopping Cart ---");
            Console.WriteLine();

            var cart = new ObservableCollection<ShoppingCartItem>
            {
                new ShoppingCartItem { ProductName = "Wireless Mouse", Quantity = 2, UnitPrice = 29.99m },
                new ShoppingCartItem { ProductName = "USB Hub", Quantity = 1, UnitPrice = 19.99m },
                new ShoppingCartItem { ProductName = "HDMI Cable", Quantity = 3, UnitPrice = 12.99m }
            };

            // Track cart changes
            cart.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    var item = e.NewItems[0] as ShoppingCartItem;
                    Console.WriteLine($"  [Added to Cart] {item.ProductName}");
                }
            };

            // Calculate totals
            decimal subtotal = cart.Sum(item => item.Total);
            decimal tax = subtotal * 0.08m;
            decimal total = subtotal + tax;

            Console.WriteLine("  Cart contents:");
            foreach (var item in cart)
            {
                Console.WriteLine($"    {item}");
            }
            // Output: Cart contents:
            //   Wireless Mouse x2 = $59.98
            //   USB Hub x1 = $19.99
            //   HDMI Cable x3 = $38.97

            Console.WriteLine($"\n  Subtotal: ${subtotal:F2}");
            // Output: Subtotal: $118.94

            Console.WriteLine($"  Tax (8%): ${tax:F2}");
            // Output: Tax (8%): $9.52

            Console.WriteLine($"  Total: ${total:F2}");
            // Output: Total: $128.46

            // Add new item
            cart.Add(new ShoppingCartItem { ProductName = "Webcam", Quantity = 1, UnitPrice = 49.99m });
            // Output: [Added to Cart] Webcam

            // Recalculate
            var newTotal = cart.Sum(item => item.Total) * 1.08m;
            Console.WriteLine($"\n  Updated Total: ${newTotal:F2}");
            // Output: Updated Total: $178.45
            Console.WriteLine();
        }

        static void LogViewerExample()
        {
            Console.WriteLine("--- Real-World: Log Viewer ---");
            Console.WriteLine();

            var logEntries = new ObservableCollection<LogEntry>
            {
                new LogEntry { Timestamp = DateTime.Now.AddHours(-2), Level = "INFO", Message = "Application started" },
                new LogEntry { Timestamp = DateTime.Now.AddHours(-1), Level = "INFO", Message = "Database connection established" },
                new LogEntry { Timestamp = DateTime.Now.AddMinutes(-30), Level = "WARN", Message = "Cache miss for user session" },
                new LogEntry { Timestamp = DateTime.Now.AddMinutes(-15), Level = "ERROR", Message = "Failed to process request" },
                new LogEntry { Timestamp = DateTime.Now.AddMinutes(-5), Level = "INFO", Message = "Retrying failed operation" }
            };

            // Filter by level
            var errors = logEntries.Where(l => l.Level == "ERROR").ToList();
            var warnings = logEntries.Where(l => l.Level == "WARN").ToList();
            var infos = logEntries.Where(l => l.Level == "INFO").ToList();

            Console.WriteLine($"  Log entries ({logEntries.Count} total):");
            Console.WriteLine($"    {errors.Count} errors, {warnings.Count} warnings, {infos.Count} info");

            // Output: Log entries (5 total):
            //   1 errors, 1 warnings, 3 info

            Console.WriteLine("\n  All logs:");
            foreach (var entry in logEntries)
            {
                string prefix = entry.Level switch
                {
                    "ERROR" => "✗",
                    "WARN" => "⚠",
                    _ => "✓"
                };
                Console.WriteLine($"    {prefix} {entry}");
            }
            // Output: All logs:
            //   ✓ [2024-01-15 14:00] [INFO] Application started
            //   ✓ [2024-01-15 15:00] [INFO] Database connection established
            //   ⚠ [2024-01-15 15:30] [WARN] Cache miss for user session
            //   ✗ [2024-01-15 15:45] [ERROR] Failed to process request
            //   ✓ [2024-01-15 16:55] [INFO] Retrying failed operation

            // Add new log entry in real-time
            logEntries.Add(new LogEntry { Timestamp = DateTime.Now, Level = "INFO", Message = "Operation completed successfully" });
            Console.WriteLine($"\n  Total after new entry: {logEntries.Count}");
            // Output: Total after new entry: 6
            Console.WriteLine();
        }

        static void TaskTrackerExample()
        {
            Console.WriteLine("--- Real-World: Task Tracker ---");
            Console.WriteLine();

            var tasks = new ObservableCollection<TaskItem>
            {
                new TaskItem { Title = "Design new logo", Priority = "High", IsCompleted = true },
                new TaskItem { Title = "Write documentation", Priority = "Medium", IsCompleted = false },
                new TaskItem { Title = "Fix login bug", Priority = "High", IsCompleted = false },
                new TaskItem { Title = "Update dependencies", Priority = "Low", IsCompleted = false },
                new TaskItem { Title = "Code review", Priority = "Medium", IsCompleted = true }
            };

            // Track task completion
            int completedCount = 0;
            foreach (var task in tasks)
            {
                task.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(TaskItem.IsCompleted))
                    {
                        completedCount++;
                        var t = s as TaskItem;
                        Console.WriteLine($"  [Task {completedCount}] '{t.Title}' marked as {(t.IsCompleted ? "completed" : "incomplete")}");
                    }
                };
            }

            Console.WriteLine("  Task list:");
            foreach (var t in tasks)
            {
                Console.WriteLine($"    {t}");
            }
            // Output: Task list:
            //   [x] Design new logo (High)
            //   [ ] Write documentation (Medium)
            //   [ ] Fix login bug (High)
            //   [ ] Update dependencies (Low)
            //   [x] Code review (Medium)

            // Get statistics
            var pendingTasks = tasks.Count(t => !t.IsCompleted);
            var highPriority = tasks.Count(t => t.Priority == "High" && !t.IsCompleted);

            Console.WriteLine($"\n  Summary: {pendingTasks} pending, {highPriority} high priority");

            // Output: Summary: 3 pending, 1 high priority

            // Mark tasks as complete
            tasks[1].IsCompleted = true;
            // Output: [Task 1] 'Write documentation' marked as completed

            tasks[2].IsCompleted = true;
            // Output: [Task 2] 'Fix login bug' marked as completed

            Console.WriteLine("\n  Updated task list:");
            foreach (var t in tasks)
            {
                Console.WriteLine($"    {t}");
            }
            // Output: Updated task list:
            //   [x] Design new logo (High)
            //   [x] Write documentation (Medium)
            //   [x] Fix login bug (High)
            //   [ ] Update dependencies (Low)
            //   [x] Code review (Medium)
            Console.WriteLine();
        }

        static void LiveDataFeedSimulation()
        {
            Console.WriteLine("--- Real-World: Live Sensor Data Feed ---");
            Console.WriteLine();

            var sensorData = new ObservableCollection<SensorReading>();
            var random = new Random(42);

            // Simulate live sensor feed
            Console.WriteLine("  Simulating sensor data stream...");

            for (int i = 0; i < 10; i++)
            {
                var reading = new SensorReading
                {
                    SensorId = "TEMP-001",
                    Value = 20.0 + random.NextDouble() * 10,
                    Unit = "°C",
                    Timestamp = DateTime.Now.AddSeconds(i * 2)
                };

                sensorData.Add(reading);
                Console.WriteLine($"  [Feed] {reading}");
            }

            // Output: [Feed] T: 23.4°C at 10:00:00
            // Output: [Feed] T: 25.1°C at 10:00:02
            // ... (continues for 10 readings)

            // Calculate statistics
            var avgValue = sensorData.Average(s => s.Value);
            var minValue = sensorData.Min(s => s.Value);
            var maxValue = sensorData.Max(s => s.Value);

            Console.WriteLine($"\n  Statistics:");
            Console.WriteLine($"    Average: {avgValue:F1}°C");
            // Output: Average: 24.9°C

            Console.WriteLine($"    Min: {minValue:F1}°C");
            // Output: Min: 20.8°C

            Console.WriteLine($"    Max: {maxValue:F1}°C");
            // Output: Max: 29.3°C

            // Detect anomalies
            var threshold = avgValue + 2.0;
            var anomalies = sensorData.Where(s => s.Value > threshold).ToList();

            Console.WriteLine($"\n  Anomalies (>{threshold:F1}°C): {anomalies.Count}");
            // Output: Anomalies (>26.9°C): 2
            Console.WriteLine();
        }
    }

    public class SensorReading
    {
        public string SensorId { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString() => $"T: {Value:F1}{Unit} at {Timestamp:HH:mm:ss}";
    }
}