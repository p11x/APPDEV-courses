/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Observer Real-World
 * FILE      : 03_Observer_RealWorld.cs
 * PURPOSE   : Real-world Observer pattern applications
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List<T>

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral._01_Observer
{
    /// <summary>
    /// Real-world Observer pattern examples
    /// </summary>
    public class ObserverRealWorld
    {
        /// <summary>
        /// Entry point for real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Observer Real-World ===
            Console.WriteLine("=== Observer Real-World ===\n");

            // ── REAL-WORLD 1: Stock Price Updates ────────────────────────────
            // Real-time financial data

            // Example 1: Stock Price Updates
            // Output: 1. Stock Price Updates:
            Console.WriteLine("1. Stock Price Updates:");
            
            // Stock ticker notifies multiple displays
            var ticker = new StockTicker();
            
            // Mobile app subscriber
            ticker.Subscribe(stock => 
                Console.WriteLine($"   Mobile: {stock.Symbol} = ${stock.Price}"));
            
            // Web dashboard subscriber
            ticker.Subscribe(stock => 
                Console.WriteLine($"   Web: {stock.Symbol} = ${stock.Price}"));
            
            // Alert system subscriber
            ticker.Subscribe(stock => 
            {
                if (stock.Price > 150)
                    Console.WriteLine($"   ALERT: {stock.Symbol} above $150!");
            });
            
            // Update stock
            ticker.Update("AAPL", 152.50m);
            // Output: Mobile: AAPL = $152.50
            // Output: Web: AAPL = $152.50
            // Output: ALERT: AAPL above $150!

            // ── REAL-WORLD 2: Event-Driven Architecture ───────────────────────
            // Decoupled component communication

            // Example 2: Event-Driven Architecture
            // Output: 2. Event-Driven Architecture:
            Console.WriteLine("\n2. Event-Driven Architecture:");
            
            // Event bus for component communication
            var eventBus = new EventBus();
            
            // Analytics component
            eventBus.Subscribe("UserLoggedIn", data => 
                Console.WriteLine($"   Analytics: User login tracked"));
            
            // Security component
            eventBus.Subscribe("UserLoggedIn", data => 
                Console.WriteLine($"   Security: Login attempt recorded"));
            
            // Notification component
            eventBus.Subscribe("UserLoggedIn", data => 
                Console.WriteLine($"   Notification: Welcome email sent"));
            
            // Fire event
            eventBus.Publish("UserLoggedIn", new { UserId = 123, Time = DateTime.Now });
            // Output: Analytics: User login tracked
            // Output: Security: Login attempt recorded
            // Output: Notification: Welcome email sent

            // ── REAL-WORLD 3: UI Data Binding ────────────────────────────────
            // MVVM pattern with observers

            // Example 3: UI Data Binding
            // Output: 3. UI Data Binding:
            Console.WriteLine("\n3. UI Data Binding:");
            
            // View model with change notification
            var viewModel = new UserViewModel();
            
            // View subscribes to changes
            viewModel.PropertyChanged += (s, e) => 
                Console.WriteLine($"   View updated: {e.PropertyName}");
            
            // Model change triggers view update
            viewModel.Name = "John";
            viewModel.Email = "john@email.com";
            // Output: View updated: Name
            // Output: View updated: Email

            // ── REAL-WORLD 4: IoT Sensor Network ─────────────────────────────
            // Multiple sensor processing

            // Example 4: IoT Sensor Network
            // Output: 4. IoT Sensor Network:
            Console.WriteLine("\n4. IoT Sensor Network:");
            
            // Central hub collects sensor data
            var hub = new IoTHub();
            
            // Temperature processor
            hub.Subscribe("Temperature", data => 
                Console.WriteLine($"   HVAC: Temperature reading: {data.Value}°C"));
            
            // Motion processor
            hub.Subscribe("Motion", data => 
                Console.WriteLine($"   Security: Motion detected in: {data.Location}"));
            
            // Light processor
            hub.Subscribe("Light", data => 
                Console.WriteLine($"   Lighting: Adjusting brightness to: {data.Value}%"));
            
            // Sensor events
            hub.ProcessSensor("Temperature", new SensorData { Value = 24.0f, Location = "Living Room" });
            hub.ProcessSensor("Motion", new SensorData { Value = 1.0f, Location = "Entrance" });
            // Output: HVAC: Temperature reading: 24°C
            // Output: Security: Motion detected in: Entrance

            // ── REAL-WORLD 5: Game Event System ───────────────────────────────
            // Game state changes

            // Example 5: Game Event System
            // Output: 5. Game Event System:
            Console.WriteLine("\n5. Game Event System:");
            
            // Game event manager
            var gameEvents = new GameEventManager();
            
            // Score system
            gameEvents.Subscribe("EnemyKilled", data => 
                Console.WriteLine($"   Score: +100 points"));
            
            // Achievement system
            gameEvents.Subscribe("EnemyKilled", data => 
                Console.WriteLine($"   Achievement: 10 kills reached"));
            
            // Audio system
            gameEvents.Subscribe("EnemyKilled", data => 
                Console.WriteLine($"   Audio: Play kill sound"));
            
            // Trigger game event
            gameEvents.Trigger("EnemyKilled");
            // Output: Score: +100 points
            // Output: Achievement: 10 kills reached
            // Output: Audio: Play kill sound

            Console.WriteLine("\n=== Observer Real-World Complete ===");
        }
    }

    /// <summary>
    /// Stock data
    /// </summary>
    public class Stock
    {
        public string Symbol { get; set; } // property: stock symbol
        public decimal Price { get; set; } // property: stock price
    }

    /// <summary>
    /// Stock ticker
    /// </summary>
    public class StockTicker
    {
        private List<Action<Stock>> _subscribers = new List<Action<Stock>>();
        
        public void Subscribe(Action<Stock> callback)
        {
            _subscribers.Add(callback);
        }
        
        public void Update(string symbol, decimal price)
        {
            var stock = new Stock { Symbol = symbol, Price = price };
            foreach (var sub in _subscribers)
            {
                sub(stock);
            }
        }
    }

    /// <summary>
    /// Event bus for decoupled communication
    /// </summary>
    public class EventBus
    {
        private Dictionary<string, List<Action<object>>> _handlers = new Dictionary<string, List<Action<object>>>();
        
        public void Subscribe(string eventType, Action<object> handler)
        {
            if (!_handlers.ContainsKey(eventType))
            {
                _handlers[eventType] = new List<Action<object>>();
            }
            _handlers[eventType].Add(handler);
        }
        
        public void Publish(string eventType, object data)
        {
            if (_handlers.ContainsKey(eventType))
            {
                foreach (var handler in _handlers[eventType])
                {
                    handler(data);
                }
            }
        }
    }

    /// <summary>
    /// View model with INotifyPropertyChanged
    /// </summary>
    public class UserViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }
        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Property changed event args
    /// </summary>
    public class PropertyChangedEventArgs : EventArgs
    {
        public string PropertyName { get; }
        
        public PropertyChangedEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }
    }

    public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);

    /// <summary>
    /// Sensor data
    /// </summary>
    public class SensorData
    {
        public float Value { get; set; } // property: sensor value
        public string Location { get; set; } // property: sensor location
    }

    /// <summary>
    /// IoT hub
    /// </summary>
    public class IoTHub
    {
        private Dictionary<string, List<Action<SensorData>>> _handlers = new Dictionary<string, List<Action<SensorData>>>();
        
        public void Subscribe(string sensorType, Action<SensorData> handler)
        {
            if (!_handlers.ContainsKey(sensorType))
            {
                _handlers[sensorType] = new List<Action<SensorData>>();
            }
            _handlers[sensorType].Add(handler);
        }
        
        public void ProcessSensor(string sensorType, SensorData data)
        {
            if (_handlers.ContainsKey(sensorType))
            {
                foreach (var handler in _handlers[sensorType])
                {
                    handler(data);
                }
            }
        }
    }

    /// <summary>
    /// Game event manager
    /// </summary>
    public class GameEventManager
    {
        private Dictionary<string, List<Action>> _events = new Dictionary<string, List<Action>>();
        
        public void Subscribe(string eventName, Action handler)
        {
            if (!_events.ContainsKey(eventName))
            {
                _events[eventName] = new List<Action>();
            }
            _events[eventName].Add(handler);
        }
        
        public void Trigger(string eventName)
        {
            if (_events.ContainsKey(eventName))
            {
                foreach (var handler in _events[eventName])
                {
                    handler();
                }
            }
        }
    }
}