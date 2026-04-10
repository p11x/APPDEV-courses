/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Strategy Real-World
 * FILE      : 03_Strategy_RealWorld.cs
 * PURPOSE   : Real-world Strategy pattern applications
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral._02_Strategy
{
    /// <summary>
    /// Real-world Strategy pattern examples
    /// </summary>
    public class StrategyRealWorld
    {
        /// <summary>
        /// Entry point for real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Strategy Real-World ===
            Console.WriteLine("=== Strategy Real-World ===\n");

            // ── REAL-WORLD 1: Payment Processing ───────────────────────────────
            // Different payment methods

            // Example 1: Payment Processing
            // Output: 1. Payment Processing:
            Console.WriteLine("1. Payment Processing:");
            
            var checkout = new ShoppingCart();
            
            // Credit card
            checkout.SetPaymentMethod(new CreditCardProcessor("4111111111111111"));
            checkout.Pay(99.99m);
            // Output: Credit card: Charged $99.99
            
            // PayPal
            checkout.SetPaymentMethod(new PayPalProcessor("user@email.com"));
            checkout.Pay(99.99m);
            // Output: PayPal: Paid $99.99
            
            // Crypto
            checkout.SetPaymentMethod(new CryptoProcessor("bc1qxy2kgdygjrsqtzq2n0yrf2493p83kkfjhx0wlh"));
            checkout.Pay(99.99m);
            // Output: Crypto: Sent 0.003 BTC

            // ── REAL-WORLD 2: Caching Strategies ───────────────────────────────
            // Different cache implementations

            // Example 2: Caching Strategies
            // Output: 2. Caching Strategies:
            Console.WriteLine("\n2. Caching Strategies:");
            
            var cacheManager = new CacheManager();
            
            // Memory cache
            cacheManager.SetStrategy(new MemoryCacheStrategy());
            cacheManager.Get("user:1");
            // Output: Memory cache: user:1 = John
            
            // Redis cache
            cacheManager.SetStrategy(new RedisCacheStrategy());
            cacheManager.Get("user:1");
            // Output: Redis cache: user:1 = John
            
            // No cache
            cacheManager.SetStrategy(new NoCacheStrategy());
            cacheManager.Get("user:1");
            // Output: No cache: Fetching from database

            // ── REAL-WORLD 3: Logging Strategies ───────────────────────────────
            // Different logging outputs

            // Example 3: Logging Strategies
            // Output: 3. Logging Strategies:
            Console.WriteLine("\n3. Logging Strategies:");
            
            var logger = new ApplicationLogger();
            
            // Console logging
            logger.SetStrategy(new ConsoleLogStrategy());
            logger.Log("Application started");
            // Output: [Console] Application started
            
            // File logging
            logger.SetStrategy(new FileLogStrategy());
            logger.Log("Application started");
            // Output: [File] Application started
            
            // Cloud logging
            logger.SetStrategy(new CloudLogStrategy());
            logger.Log("Application started");
            // Output: [Cloud] Application started

            // ── REAL-WORLD 4: Notification Strategies ────────────────────────
            // Different notification channels

            // Example 4: Notification Strategies
            // Output: 4. Notification Strategies:
            Console.WriteLine("\n4. Notification Strategies:");
            
            var notifier = new NotificationService();
            
            // Email notification
            notifier.SetStrategy(new EmailNotifierStrategy());
            notifier.Send("user@example.com", "Your order shipped!");
            // Output: Email sent to user@example.com
            
            // SMS notification
            notifier.SetStrategy(new SMSNotifierStrategy());
            notifier.Send("+1234567890", "Your order shipped!");
            // Output: SMS sent to +1234567890
            
            // Push notification
            notifier.SetStrategy(new PushNotifierStrategy());
            notifier.Send("device123", "Your order shipped!");
            // Output: Push sent to device123

            // ── REAL-WORLD 5: Serialization Strategies ───────────────────────
            // Different data formats

            // Example 5: Serialization Strategies
            // Output: 5. Serialization Strategies:
            Console.WriteLine("\n5. Serialization Strategies:");
            
            var serializer = new DataSerializer();
            var data = new { Name = "John", Age = 30 };
            
            // JSON
            serializer.SetStrategy(new JSONSerializerStrategy());
            var json = serializer.Serialize(data);
            // Output: JSON: {"Name":"John","Age":30}
            
            // XML
            serializer.SetStrategy(new XMLSerializerStrategy());
            var xml = serializer.Serialize(data);
            // Output: XML: <Data><Name>John</Name><Age>30</Age></Data>
            
            // Binary
            serializer.SetStrategy(new BinarySerializerStrategy());
            var binary = serializer.Serialize(data);
            // Output: Binary: [bytes]

            Console.WriteLine("\n=== Strategy Real-World Complete ===");
        }
    }

    /// <summary>
    /// Payment processor interface
    /// </summary>
    public interface IPaymentProcessor
    {
        void Process(decimal amount); // method: processes payment
    }

    /// <summary>
    /// Credit card processor
    /// </summary>
    public class CreditCardProcessor : IPaymentProcessor
    {
        private string _cardNumber;
        
        public CreditCardProcessor(string cardNumber)
        {
            _cardNumber = cardNumber;
        }
        
        public void Process(decimal amount)
        {
            Console.WriteLine($"   Credit card: Charged ${amount:F2}");
        }
    }

    /// <summary>
    /// PayPal processor
    /// </summary>
    public class PayPalProcessor : IPaymentProcessor
    {
        private string _email;
        
        public PayPalProcessor(string email)
        {
            _email = email;
        }
        
        public void Process(decimal amount)
        {
            Console.WriteLine($"   PayPal: Paid ${amount:F2}");
        }
    }

    /// <summary>
    /// Crypto processor
    /// </summary>
    public class CryptoProcessor : IPaymentProcessor
    {
        private string _wallet;
        
        public CryptoProcessor(string wallet)
        {
            _wallet = wallet;
        }
        
        public void Process(decimal amount)
        {
            var btc = amount / 33000m;
            Console.WriteLine($"   Crypto: Sent {btc:F4} BTC");
        }
    }

    /// <summary>
    /// Shopping cart with payment
    /// </summary>
    public class ShoppingCart
    {
        private IPaymentProcessor _paymentMethod;
        
        public void SetPaymentMethod(IPaymentProcessor method)
        {
            _paymentMethod = method;
        }
        
        public void Pay(decimal amount)
        {
            _paymentMethod.Process(amount);
        }
    }

    /// <summary>
    /// Cache strategy interface
    /// </summary>
    public interface ICacheStrategy
    {
        object Get(string key); // method: gets cached value
    }

    /// <summary>
    /// Memory cache strategy
    /// </summary>
    public class MemoryCacheStrategy : ICacheStrategy
    {
        public object Get(string key)
        {
            Console.WriteLine($"   Memory cache: {key} = John");
            return "John";
        }
    }

    /// <summary>
    /// Redis cache strategy
    /// </summary>
    public class RedisCacheStrategy : ICacheStrategy
    {
        public object Get(string key)
        {
            Console.WriteLine($"   Redis cache: {key} = John");
            return "John";
        }
    }

    /// <summary>
    /// No cache strategy
    /// </summary>
    public class NoCacheStrategy : ICacheStrategy
    {
        public object Get(string key)
        {
            Console.WriteLine($"   No cache: Fetching from database");
            return null;
        }
    }

    /// <summary>
    /// Cache manager
    /// </summary>
    public class CacheManager
    {
        private ICacheStrategy _strategy;
        
        public void SetStrategy(ICacheStrategy strategy)
        {
            _strategy = strategy;
        }
        
        public object Get(string key)
        {
            return _strategy.Get(key);
        }
    }

    /// <summary>
    /// Log strategy interface
    /// </summary>
    public interface ILogStrategy
    {
        void Log(string message); // method: logs message
    }

    /// <summary>
    /// Console log strategy
    /// </summary>
    public class ConsoleLogStrategy : ILogStrategy
    {
        public void Log(string message)
        {
            Console.WriteLine($"   [Console] {message}");
        }
    }

    /// <summary>
    /// File log strategy
    /// </summary>
    public class FileLogStrategy : ILogStrategy
    {
        public void Log(string message)
        {
            Console.WriteLine($"   [File] {message}");
        }
    }

    /// <summary>
    /// Cloud log strategy
    /// </summary>
    public class CloudLogStrategy : ILogStrategy
    {
        public void Log(string message)
        {
            Console.WriteLine($"   [Cloud] {message}");
        }
    }

    /// <summary>
    /// Application logger
    /// </summary>
    public class ApplicationLogger
    {
        private ILogStrategy _strategy;
        
        public void SetStrategy(ILogStrategy strategy)
        {
            _strategy = strategy;
        }
        
        public void Log(string message)
        {
            _strategy.Log(message);
        }
    }

    /// <summary>
    /// Notifier strategy interface
    /// </summary>
    public interface INotifierStrategy
    {
        void Send(string target, string message); // method: sends notification
    }

    /// <summary>
    /// Email notifier strategy
    /// </summary>
    public class EmailNotifierStrategy : INotifierStrategy
    {
        public void Send(string target, string message)
        {
            Console.WriteLine($"   Email sent to {target}");
        }
    }

    /// <summary>
    /// SMS notifier strategy
    /// </summary>
    public class SMSNotifierStrategy : INotifierStrategy
    {
        public void Send(string target, string message)
        {
            Console.WriteLine($"   SMS sent to {target}");
        }
    }

    /// <summary>
    /// Push notifier strategy
    /// </summary>
    public class PushNotifierStrategy : INotifierStrategy
    {
        public void Send(string target, string message)
        {
            Console.WriteLine($"   Push sent to {target}");
        }
    }

    /// <summary>
    /// Notification service
    /// </summary>
    public class NotificationService
    {
        private INotifierStrategy _strategy;
        
        public void SetStrategy(INotifierStrategy strategy)
        {
            _strategy = strategy;
        }
        
        public void Send(string target, string message)
        {
            _strategy.Send(target, message);
        }
    }

    /// <summary>
    /// Serializer strategy interface
    /// </summary>
    public interface ISerializerStrategy
    {
        string Serialize(object data); // method: serializes data
    }

    /// <summary>
    /// JSON serializer strategy
    /// </summary>
    public class JSONSerializerStrategy : ISerializerStrategy
    {
        public string Serialize(object data)
        {
            Console.WriteLine($"   JSON: {data}");
            return "{json}";
        }
    }

    /// <summary>
    /// XML serializer strategy
    /// </summary>
    public class XMLSerializerStrategy : ISerializerStrategy
    {
        public string Serialize(object data)
        {
            Console.WriteLine($"   XML: {data}");
            return "<xml>";
        }
    }

    /// <summary>
    /// Binary serializer strategy
    /// </summary>
    public class BinarySerializerStrategy : ISerializerStrategy
    {
        public string Serialize(object data)
        {
            Console.WriteLine($"   Binary: [bytes]");
            return "[bytes]";
        }
    }

    /// <summary>
    /// Data serializer
    /// </summary>
    public class DataSerializer
    {
        private ISerializerStrategy _strategy;
        
        public void SetStrategy(ISerializerStrategy strategy)
        {
            _strategy = strategy;
        }
        
        public string Serialize(object data)
        {
            return _strategy.Serialize(data);
        }
    }
}