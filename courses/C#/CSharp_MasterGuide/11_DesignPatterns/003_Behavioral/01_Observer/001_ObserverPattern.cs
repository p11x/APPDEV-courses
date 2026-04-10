/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Observer Pattern
 * FILE      : 01_ObserverPattern.cs
 * PURPOSE   : Demonstrates Observer design pattern in C#
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List<T>

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral._01_Observer
{
    /// <summary>
    /// Demonstrates Observer pattern
    /// </summary>
    public class ObserverPattern
    {
        /// <summary>
        /// Entry point for Observer pattern examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Observer Pattern ===
            Console.WriteLine("=== Observer Pattern ===\n");

            // ── CONCEPT: What is Observer? ─────────────────────────────────────
            // Defines one-to-many dependency between objects

            // Example 1: Basic Observer
            // Output: 1. Basic Observer:
            Console.WriteLine("1. Basic Observer:");
            
            // Subject maintains list of observers
            var stock = new Stock("AAPL", 150.00m);
            
            // Observers subscribe to subject
            stock.Attach(new InvestorObserver("John"));
            stock.Attach(new InvestorObserver("Jane"));
            
            // Price change notifies all observers
            stock.Price = 155.00m;
            // Output: John notified: AAPL changed to $155.00
            // Output: Jane notified: AAPL changed to $155.00

            // ── CONCEPT: Event-Based Observer ────────────────────────────────
            // Using C# events for observer pattern

            // Example 2: Event-Based Observer
            // Output: 2. Event-Based Observer:
            Console.WriteLine("\n2. Event-Based Observer:");
            
            // Publisher uses events
            var newsChannel = new NewsChannel();
            
            // Subscribers register for events
            newsChannel.OnNewsPublished += (sender, news) => 
                Console.WriteLine($"   Subscriber 1 received: {news}");
            newsChannel.OnNewsPublished += (sender, news) => 
                Console.WriteLine($"   Subscriber 2 received: {news}");
            
            // Publish news
            newsChannel.Publish("Breaking: Stock market up!");
            // Output: Subscriber 1 received: Breaking: Stock market up!
            // Output: Subscriber 2 received: Breaking: Stock market up!

            // ── CONCEPT: Multiple Subjects ───────────────────────────────────
            // Observers can subscribe to multiple subjects

            // Example 3: Multiple Subjects
            // Output: 3. Multiple Subjects:
            Console.WriteLine("\n3. Multiple Subjects:");
            
            // Observer subscribes to multiple publishers
            var dashboard = new DashboardObserver();
            
            var sensor1 = new TemperatureSensor();
            var sensor2 = new HumiditySensor();
            
            sensor1.OnTemperatureChanged += dashboard.Update;
            sensor2.OnHumidityChanged += dashboard.Update;
            
            // Both trigger same observer
            sensor1.Temperature = 25.5f;
            sensor2.Humidity = 60.0f;
            // Output: Dashboard: Temperature = 25.5°C
            // Output: Dashboard: Humidity = 60.0%

            // ── REAL-WORLD EXAMPLE: Order Processing ────────────────────────
            // Output: --- Real-World: Order Processing ---
            Console.WriteLine("\n--- Real-World: Order Processing ---");
            
            // Order state changes trigger notifications
            var order = new Order(123);
            
            // Multiple services react to order changes
            order.StatusChanged += (s, status) => Console.WriteLine($"   Email sent: Order {status}");
            order.StatusChanged += (s, status) => Console.WriteLine($"   Inventory updated: Order {status}");
            order.StatusChanged += (s, status) => Console.WriteLine($"   Analytics tracked: Order {status}");
            
            // Order status changes
            order.Status = "Processing";
            // Output: Email sent: Order Processing
            // Output: Inventory updated: Order Processing
            // Output: Analytics tracked: Order Processing
            
            order.Status = "Shipped";
            // Output: Email sent: Order Shipped
            // Output: Inventory updated: Order Shipped
            // Output: Analytics tracked: Order Shipped

            Console.WriteLine("\n=== Observer Pattern Complete ===");
        }
    }

    /// <summary>
    /// Observer interface
    /// </summary>
    public interface IObserver
    {
        void Update(decimal price); // method: receives update
    }

    /// <summary>
    /// Subject interface
    /// </summary>
    public interface ISubject
    {
        void Attach(IObserver observer); // method: adds observer
        void Detach(IObserver observer); // method: removes observer
        void Notify(); // method: notifies all observers
    }

    /// <summary>
    /// Stock - concrete subject
    /// </summary>
    public class Stock : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();
        private string _symbol;
        private decimal _price;
        
        public Stock(string symbol, decimal price)
        {
            _symbol = symbol;
            _price = price;
        }
        
        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                Notify(); // notify on change
            }
        }
        
        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }
        
        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }
        
        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(_price);
            }
        }
    }

    /// <summary>
    /// Investor observer
    /// </summary>
    public class InvestorObserver : IObserver
    {
        private string _name;
        
        public InvestorObserver(string name)
        {
            _name = name;
        }
        
        public void Update(decimal price)
        {
            Console.WriteLine($"   {_name} notified: AAPL changed to ${price:F2}");
        }
    }

    /// <summary>
    /// News channel using events
    /// </summary>
    public class NewsChannel
    {
        public event EventHandler<string> OnNewsPublished;
        
        public void Publish(string news)
        {
            OnNewsPublished?.Invoke(this, news);
        }
    }

    /// <summary>
    /// Temperature sensor
    /// </summary>
    public class TemperatureSensor
    {
        public event EventHandler<float> OnTemperatureChanged;
        private float _temperature;
        
        public float Temperature
        {
            get => _temperature;
            set
            {
                _temperature = value;
                OnTemperatureChanged?.Invoke(this, value);
            }
        }
    }

    /// <summary>
    /// Humidity sensor
    /// </summary>
    public class HumiditySensor
    {
        public event EventHandler<float> OnHumidityChanged;
        private float _humidity;
        
        public float Humidity
        {
            get => _humidity;
            set
            {
                _humidity = value;
                OnHumidityChanged?.Invoke(this, value);
            }
        }
    }

    /// <summary>
    /// Dashboard observer
    /// </summary>
    public class DashboardObserver
    {
        public void Update(object sender, float value)
        {
            if (sender is TemperatureSensor)
            {
                Console.WriteLine($"   Dashboard: Temperature = {value}°C");
            }
            else if (sender is HumiditySensor)
            {
                Console.WriteLine($"   Dashboard: Humidity = {value}%");
            }
        }
    }

    /// <summary>
    /// Order with events
    /// </summary>
    public class Order
    {
        public event EventHandler<string> StatusChanged;
        
        private int _orderId;
        private string _status;
        
        public Order(int orderId)
        {
            _orderId = orderId;
        }
        
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                StatusChanged?.Invoke(this, value);
            }
        }
    }
}