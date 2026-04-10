/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Delegates and Events - Events
 * FILE      : EventHandling.cs
 * PURPOSE   : Teaches event handling in C#, declaring events,
 *            subscribing/unsubscribing, event patterns,
 *            and built-in event args
 * ============================================================
 */

using System; // Core System namespace

namespace CSharp_MasterGuide._03_Advanced_OOP._02_Delegates_Events
{
    class EventHandling
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Event Handling in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: What are Events?
            // ═══════════════════════════════════════════════════════════

            // Events are a way to implement the observer pattern
            // They allow a class to notify other classes when something happens
            
            // ── EXAMPLE 1: Basic Event ───────────────────────────────────
            var button = new Button();
            
            // Subscribe to event
            button.Click += OnButtonClick;
            
            // Click the button - triggers event
            button.ClickButton();
            
            // Unsubscribe
            button.Click -= OnButtonClick;

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Event Declaration
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Custom EventArgs ─────────────────────────────
            var publisher = new EventPublisher();
            publisher.ValueChanged += OnValueChanged;
            publisher.ValueChanged += OnValueChangedDetailed;
            
            publisher.SetValue(10);
            publisher.SetValue(20);
            publisher.SetValue(30);

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Event Subscription
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Multiple Subscribers ────────────────────────
            var newsAgency = new NewsAgency();
            
            var tvChannel = new TVChannel();
            var webPortal = new WebPortal();
            var mobileApp = new MobileApp();
            
            // Multiple subscribers
            newsAgency.NewsPublished += tvChannel.OnNewsPublished;
            newsAgency.NewsPublished += webPortal.OnNewsPublished;
            newsAgency.NewsPublished += mobileApp.OnNewsPublished;
            
            // Publish news
            newsAgency.PublishNews("Breaking: New C# features announced!");

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Event Patterns
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Standard .NET Event Pattern ─────────────────
            var counter = new Counter();
            counter.OnCount += (sender, e) => Console.WriteLine($"  Count: {e.Count}");
            
            counter.Start();
            counter.Start();
            counter.Start();

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Built-in Event Handlers
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Using EventHandler ───────────────────────────
            var document = new Document();
            
            // Using EventHandler<T>
            document.Saved += (sender, e) => 
                Console.WriteLine($"  Document saved: {e.FileName}");
            
            document.Save("report.pdf");

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Custom Event Args
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: EventArgs Derived Class ─────────────────────
            var orderSystem = new OrderSystem();
            
            orderSystem.OrderPlaced += (sender, e) =>
            {
                Console.WriteLine($"  Order {e.OrderId} placed by {e.CustomerName}");
                Console.WriteLine($"  Items: {e.ItemCount}, Total: ${e.Total}");
            };
            
            orderSystem.PlaceOrder("John", 3, 150m);

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Real-World: UI Events
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Form with Multiple Controls ─────────────────
            var form = new Form();
            
            form.ButtonClicked += () => Console.WriteLine("  Button clicked!");
            form.TextChanged += (text) => Console.WriteLine($"  Text changed: {text}");
            form.SelectedIndexChanged += (index) => Console.WriteLine($"  Selected: {index}");
            
            form.SimulateButtonClick();
            form.SimulateTextChange("Hello");
            form.SimulateSelection(2);

            Console.WriteLine("\n=== Event Handling Complete ===");
        }

        // Event handlers
        static void OnButtonClick()
        {
            Console.WriteLine("  Button was clicked!");
        }

        static void OnValueChanged(int oldValue, int newValue)
        {
            Console.WriteLine($"  Value changed from {oldValue} to {newValue}");
        }

        static void OnValueChangedDetailed(object sender, ValueChangedEventArgs e)
        {
            Console.WriteLine($"  Detailed: {e.OldValue} -> {e.NewValue}");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Simple Event Example
    // ═══════════════════════════════════════════════════════════

    class Button
    {
        // Event declaration - uses a delegate
        public event Action Click;
        
        public void ClickButton()
        {
            // Raise the event
            Click?.Invoke();
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Custom Event with EventArgs
    // ═══════════════════════════════════════════════════════════

    class ValueChangedEventArgs : EventArgs
    {
        public int OldValue { get; }
        public int NewValue { get; }
        
        public ValueChangedEventArgs(int oldValue, int newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    class EventPublisher
    {
        private int _value;
        
        // Event with custom delegate and EventArgs
        public event EventHandler<ValueChangedEventArgs> ValueChanged;
        
        public void SetValue(int newValue)
        {
            int oldValue = _value;
            _value = newValue;
            
            // Raise event
            ValueChanged?.Invoke(this, new ValueChangedEventArgs(oldValue, newValue));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: News Agency
    // ═══════════════════════════════════════════════════════════

    class NewsAgency
    {
        public event Action<string> NewsPublished;
        
        public void PublishNews(string news)
        {
            Console.WriteLine($"  Publishing: {news}");
            NewsPublished?.Invoke(news);
        }
    }

    class TVChannel
    {
        public void OnNewsPublished(string news)
        {
            Console.WriteLine($"  TV: Broadcasting - {news}");
        }
    }

    class WebPortal
    {
        public void OnNewsPublished(string news)
        {
            Console.WriteLine($"  Web: Posted - {news}");
        }
    }

    class MobileApp
    {
        public void OnNewsPublished(string news)
        {
            Console.WriteLine($"  App: Pushed - {news}");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Standard Event Pattern
    // ═══════════════════════════════════════════════════════════

    class CounterEventArgs : EventArgs
    {
        public int Count { get; }
        public CounterEventArgs(int count) => Count = count;
    }

    class Counter
    {
        private int _count;
        
        public event EventHandler<CounterEventArgs> OnCount;
        
        public void Start()
        {
            _count++;
            OnCount?.Invoke(this, new CounterEventArgs(_count));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Built-in EventHandler<T>
    // ═══════════════════════════════════════════════════════════

    class FileSavedEventArgs : EventArgs
    {
        public string FileName { get; }
        public FileSavedEventArgs(string fileName) => FileName = fileName;
    }

    class Document
    {
        public event EventHandler<FileSavedEventArgs> Saved;
        
        public void Save(string filename)
        {
            Console.WriteLine($"  Saving {filename}...");
            Saved?.Invoke(this, new FileSavedEventArgs(filename));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Custom Event Args for Business
    // ═══════════════════════════════════════════════════════════

    class OrderPlacedEventArgs : EventArgs
    {
        public string OrderId { get; }
        public string CustomerName { get; }
        public int ItemCount { get; }
        public decimal Total { get; }
        
        public OrderPlacedEventArgs(string orderId, string customer, int items, decimal total)
        {
            OrderId = orderId;
            CustomerName = customer;
            ItemCount = items;
            Total = total;
        }
    }

    class OrderSystem
    {
        private int _orderId = 1000;
        
        public event EventHandler<OrderPlacedEventArgs> OrderPlaced;
        
        public void PlaceOrder(string customer, int items, decimal total)
        {
            _orderId++;
            Console.WriteLine($"  Order created: ORD-{_orderId}");
            OrderPlaced?.Invoke(this, new OrderPlacedEventArgs($"ORD-{_orderId}", customer, items, total));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // UI Form Events
    // ═══════════════════════════════════════════════════════════

    class Form
    {
        public event Action ButtonClicked;
        public event Action<string> TextChanged;
        public event Action<int> SelectedIndexChanged;
        
        public void SimulateButtonClick() => ButtonClicked?.Invoke();
        public void SimulateTextChange(string text) => TextChanged?.Invoke(text);
        public void SimulateSelection(int index) => SelectedIndexChanged?.Invoke(index);
    }
}