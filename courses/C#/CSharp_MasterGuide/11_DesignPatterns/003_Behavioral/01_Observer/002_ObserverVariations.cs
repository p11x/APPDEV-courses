/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Observer Variations
 * FILE      : 02_ObserverVariations.cs
 * PURPOSE   : Demonstrates different Observer pattern approaches
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List<T>

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral._01_Observer
{
    /// <summary>
    /// Demonstrates Observer variations
    /// </summary>
    public class ObserverVariations
    {
        /// <summary>
        /// Entry point for Observer variations
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Observer Variations ===
            Console.WriteLine("=== Observer Variations ===\n");

            // ── CONCEPT: Weak Event Pattern ───────────────────────────────────
            // Prevents memory leaks with weak references

            // Example 1: Weak Event Pattern
            // Output: 1. Weak Event Pattern:
            Console.WriteLine("1. Weak Event Pattern:");
            
            // Weak events don't prevent GC
            var source = new WeakEventSource();
            var subscriber = new WeakSubscriber();
            
            source.SomethingHappened += subscriber.OnEvent;
            
            // Trigger event
            source.RaiseEvent();
            // Output: Event received
            
            // Subscriber can be garbage collected
            subscriber = null;
            // GC can collect subscriber now

            // ── CONCEPT: Generic Observer ──────────────────────────────────────
            // Type-safe observer pattern

            // Example 2: Generic Observer
            // Output: 2. Generic Observer:
            Console.WriteLine("\n2. Generic Observer:");
            
            // Use generic event args
            var dataPublisher = new DataPublisher<WeatherData>();
            
            dataPublisher.DataChanged += (sender, data) => 
                Console.WriteLine($"   Received: {data.Temperature}°C, {data.Humidity}%");
            
            dataPublisher.Publish(new WeatherData { Temperature = 22.5f, Humidity = 65.0f });
            // Output: Received: 22.5°C, 65%

            // ── CONCEPT: Push vs Pull ─────────────────────────────────────────
            // Different notification styles

            // Example 3: Push vs Pull
            // Output: 3. Push vs Pull:
            Console.WriteLine("\n3. Push vs Pull:");
            
            // Push: subject sends data
            var pushSubject = new PushSubject();
            pushSubject.Subscribe(data => Console.WriteLine($"   Push: {data}"));
            pushSubject.Notify("Important data");
            
            // Pull: observer requests data
            var pullSubject = new PullSubject();
            pullSubject.Subscribe();
            var data = pullSubject.GetData();
            // Output: Pull: data from subject
            Console.WriteLine($"   Pull: {data}");

            // ── REAL-WORLD EXAMPLE: Reactive Extensions ───────────────────────
            // Output: --- Real-World: Reactive Extensions ---
            Console.WriteLine("\n--- Real-World: Reactive Extensions ---");
            
            // Simulate reactive streams
            var stream = new ReactiveStream<int>();
            
            stream.Subscribe(value => Console.WriteLine($"   Value: {value}"));
            stream.Subscribe(value => Console.WriteLine($"   Double: {value * 2}"));
            
            stream.OnNext(10);
            stream.OnNext(20);
            stream.OnNext(30);
            
            // Output: Value: 10
            // Output: Double: 20
            // Output: Value: 20
            // Output: Double: 40

            Console.WriteLine("\n=== Observer Variations Complete ===");
        }
    }

    /// <summary>
    /// Weak event source
    /// </summary>
    public class WeakEventSource
    {
        public event EventHandler SomethingHappened;
        
        public void RaiseEvent()
        {
            SomethingHappened?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Weak subscriber
    /// </summary>
    public class WeakSubscriber
    {
        public void OnEvent(object sender, EventArgs e)
        {
            Console.WriteLine("   Event received");
        }
    }

    /// <summary>
    /// Weather data
    /// </summary>
    public class WeatherData
    {
        public float Temperature { get; set; } // property: temperature
        public float Humidity { get; set; } // property: humidity
    }

    /// <summary>
    /// Generic data publisher
    /// </summary>
    public class DataPublisher<T>
    {
        public event EventHandler<T> DataChanged;
        
        public void Publish(T data)
        {
            DataChanged?.Invoke(this, data);
        }
    }

    /// <summary>
    /// Push-style subject
    /// </summary>
    public class PushSubject
    {
        private List<Action<string>> _subscribers = new List<Action<string>>();
        
        public void Subscribe(Action<string> callback)
        {
            _subscribers.Add(callback);
        }
        
        public void Notify(string data)
        {
            foreach (var sub in _subscribers)
            {
                sub(data);
            }
        }
    }

    /// <summary>
    /// Pull-style subject
    /// </summary>
    public class PullSubject
    {
        private bool _subscribed;
        
        public void Subscribe()
        {
            _subscribed = true;
        }
        
        public string GetData()
        {
            if (_subscribed)
            {
                return "data from subject";
            }
            return null;
        }
    }

    /// <summary>
    /// Reactive stream
    /// </summary>
    public class ReactiveStream<T>
    {
        private List<Action<T>> _observers = new List<Action<T>>();
        
        public void Subscribe(Action<T> observer)
        {
            _observers.Add(observer);
        }
        
        public void OnNext(T value)
        {
            foreach (var observer in _observers)
            {
                observer(value);
            }
        }
    }
}