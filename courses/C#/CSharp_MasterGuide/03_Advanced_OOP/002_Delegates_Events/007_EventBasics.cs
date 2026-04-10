/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Delegates and Events - Event Basics
 * FILE      : EventBasics.cs
 * PURPOSE   : Teaches event fundamentals in C#, declaring events,
 *            subscribing/unsubscribing with += and -=, event
 *            invocation, and event patterns
 * ============================================================
 */

using System;

namespace CSharp_MasterGuide._03_Advanced_OOP._02_Delegates_Events
{
    class EventBasics
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Event Basics in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: What are Events?
            // ═══════════════════════════════════════════════════════════

            // Events are a mechanism for communication between objects
            // They implement the Observer design pattern
            // Events enable loose coupling between publishers and subscribers

            // ── EXAMPLE 1: Basic Event Declaration ──────────────────────
            Console.WriteLine("--- Basic Event Declaration ---");
            
            var publisher = new StockPublisher();
            
            // Subscribe to event using +=
            publisher.PriceChanged += OnPriceChanged;
            publisher.PriceChanged += OnPriceChangedDetailed;
            
            // Trigger event by changing price
            publisher.SetPrice(100);
            publisher.SetPrice(150);
            publisher.SetPrice(200);
            
            // Unsubscribe using -=
            publisher.PriceChanged -= OnPriceChangedDetailed;
            
            Console.WriteLine("After unsubscribing OnPriceChangedDetailed:");
            publisher.SetPrice(250);

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Event Declaration Syntax
            // ═══════════════════════════════════════════════════════════

            // Events are declared using the 'event' keyword
            // They must be declared within a class
            // Event type is typically a delegate

            // ── EXAMPLE 1: Simple Event ───────────────────────────────────
            Console.WriteLine("\n--- Simple Event ---");
            
            var notifier = new SimpleNotifier();
            notifier.Notify += () => Console.WriteLine("  Notification received!");
            notifier.Trigger();

            // ── EXAMPLE 2: Event with Custom Delegate ─────────────────────
            Console.WriteLine("\n--- Event with Custom Delegate ---");
            
            var messenger = new MessageMessenger();
            messenger.MessageSent += HandleMessageSent;
            messenger.Send("Hello, World!");

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Event Subscription (+=) and Unsubscription (-=)
            // ═══════════════════════════════════════════════════════════

            // Use += to subscribe (add handler)
            // Use -= to unsubscribe (remove handler)
            // Multiple handlers can subscribe to the same event

            // ── EXAMPLE 1: Multiple Subscribers ───────────────────────────
            Console.WriteLine("\n--- Multiple Subscribers ---");
            
            var newsChannel = new NewsChannel();
            
            // Subscribe multiple handlers
            newsChannel.BreakNews += TelevisionReceiver.Receive;
            newsChannel.BreakNews += RadioReceiver.Receive;
            newsChannel.BreakNews += WebReceiver.Receive;
            
            // Broadcast news
            newsChannel.Broadcast("Breaking: New technology announced!");

            Console.WriteLine("\n--- After removing Radio ---");
            
            // Unsubscribe one handler
            newsChannel.BreakNews -= RadioReceiver.Receive;
            newsChannel.Broadcast("Update: Market trends change");

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Event Invocation
            // ═══════════════════════════════════════════════════════════

            // Use null-conditional operator ?. before invoking
            // Always invoke with Invoke() or direct call
            // Pass appropriate arguments to event handlers

            // ── EXAMPLE 1: Safe Event Invocation ─────────────────────────
            Console.WriteLine("\n--- Safe Event Invocation ---");
            
            var calculator = new Calculator();
            calculator.ResultCalculated += (result) =>
                Console.WriteLine($"  Result calculated: {result}");
            
            calculator.Add(10, 20);

            // ── EXAMPLE 2: Event with No Arguments ───────────────────────
            Console.WriteLine("\n--- Event with No Arguments ---");
            
            var timer = new Timer();
            timer.Tick += () => Console.WriteLine("  Tick occurred!");
            timer.Start();
            timer.Stop();

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Event Access Modifiers
            // ═══════════════════════════════════════════════════════════

            // Events can have different access levels
            // Public events allow external subscription
            // Private events restrict access to containing class

            // ── EXAMPLE 1: Public vs Private Events ──────────────────────
            Console.WriteLine("\n--- Event Access Modifiers ---");
            
            var processor = new DataProcessor();
            
            // Public event - can subscribe from outside
            processor.ProcessingComplete += () => 
                Console.WriteLine("  Processing complete (public event)!");
            
            // Trigger processing
            processor.Process("test data");

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Event Naming Conventions
            // ═══════════════════════════════════════════════════════════

            // Events should be named using verb or verb-noun pattern
            // Common patterns: Click, Changed, Created, Updated, Deleted

            // ── EXAMPLE 1: Event Naming Examples ──────────────────────────
            Console.WriteLine("\n--- Event Naming Conventions ---");
            
            var userManager = new UserManager();
            
            // Events follow naming convention (past tense for completed actions)
            userManager.UserRegistered += (user) => 
                Console.WriteLine($"  User registered: {user}");
            userManager.UserLoggedIn += (user) => 
                Console.WriteLine($"  User logged in: {user}");
            userManager.UserLoggedOut += (user) => 
                Console.WriteLine($"  User logged out: {user}");
            
            userManager.RegisterUser("alice");
            userManager.LoginUser("alice");
            userManager.LogoutUser("alice");

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Real-World: Notification System
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Email Notification System ─────────────────────
            Console.WriteLine("\n--- Real-World: Notification System ---");
            
            var notificationService = new NotificationService();
            
            // Subscribe different notification handlers
            notificationService.Notification += EmailNotification.Send;
            notificationService.Notification += SMSNotification.Send;
            notificationService.Notification += PushNotification.Send;
            
            // Send notification
            notificationService.Notify("New message received");

            // ── EXAMPLE 2: Progress Reporting ──────────────────────────────
            Console.WriteLine("\n--- Real-World: Progress Reporting ---");
            
            var downloader = new FileDownloader();
            
            downloader.ProgressChanged += (percent) =>
                Console.WriteLine($"  Download progress: {percent}%");
            
            downloader.Download("document.pdf");

            Console.WriteLine("\n=== Event Basics Complete ===");
        }

        // Event handlers
        static void OnPriceChanged(decimal oldPrice, decimal newPrice)
        {
            Console.WriteLine($"  Price changed: {oldPrice} -> {newPrice}");
        }

        static void OnPriceChangedDetailed(object sender, PriceChangedEventArgs e)
        {
            Console.WriteLine($"  Detailed change: {e.OldPrice} -> {e.NewPrice} (by {e.Symbol})");
        }

        static void HandleMessageSent(string message, string recipient)
        {
            Console.WriteLine($"  Message sent to {recipient}: {message}");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Event with custom delegate and EventArgs
    // ═══════════════════════════════════════════════════════════

    class PriceChangedEventArgs : EventArgs
    {
        public string Symbol { get; }
        public decimal OldPrice { get; }
        public decimal NewPrice { get; }

        public PriceChangedEventArgs(string symbol, decimal oldPrice, decimal newPrice)
        {
            Symbol = symbol;
            OldPrice = oldPrice;
            NewPrice = newPrice;
        }
    }

    class StockPublisher
    {
        // Event with custom delegate type
        public event Action<decimal, decimal> PriceChanged;
        
        // Alternative: using EventHandler<T>
        public event EventHandler<PriceChangedEventArgs> PriceChangedDetailed;

        private decimal _price;

        public void SetPrice(decimal newPrice)
        {
            if (_price != newPrice)
            {
                decimal oldPrice = _price;
                _price = newPrice;
                
                // Invoke events
                PriceChanged?.Invoke(oldPrice, newPrice);
                PriceChangedDetailed?.Invoke(this, 
                    new PriceChangedEventArgs("STOCK", oldPrice, newPrice));
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Simple event classes
    // ═══════════════════════════════════════════════════════════

    class SimpleNotifier
    {
        public event Action Notify;

        public void Trigger()
        {
            Notify?.Invoke();
        }
    }

    class MessageMessenger
    {
        public delegate void MessageSentHandler(string message, string recipient);
        public event MessageSentHandler MessageSent;

        public void Send(string message)
        {
            Console.WriteLine($"  Sending: {message}");
            MessageSent?.Invoke(message, "recipient@example.com");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // News channel example
    // ═══════════════════════════════════════════════════════════

    class NewsChannel
    {
        public event Action<string> BreakNews;

        public void Broadcast(string news)
        {
            Console.WriteLine($"  Broadcasting: {news}");
            BreakNews?.Invoke(news);
        }
    }

    class TelevisionReceiver
    {
        public static void Receive(string news) =>
            Console.WriteLine($"  TV: Showing {news}");
    }

    class RadioReceiver
    {
        public static void Receive(string news) =>
            Console.WriteLine($"  Radio: Playing {news}");
    }

    class WebReceiver
    {
        public static void Receive(string news) =>
            Console.WriteLine($"  Web: Posting {news}");
    }

    // ═══════════════════════════════════════════════════════════
    // Calculator with result event
    // ═══════════════════════════════════════════════════════════

    class Calculator
    {
        public event Action<int> ResultCalculated;

        public void Add(int a, int b)
        {
            int result = a + b;
            Console.WriteLine($"  Calculating: {a} + {b} = {result}");
            ResultCalculated?.Invoke(result);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Timer class
    // ═══════════════════════════════════════════════════════════

    class Timer
    {
        public event Action Tick;

        public void Start()
        {
            Console.WriteLine("  Timer started");
            Tick?.Invoke();
        }

        public void Stop()
        {
            Console.WriteLine("  Timer stopped");
            Tick?.Invoke();
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Data processor
    // ═══════════════════════════════════════════════════════════

    class DataProcessor
    {
        public event Action ProcessingComplete;  // Public event
        
        public void Process(string data)
        {
            Console.WriteLine($"  Processing: {data}");
            ProcessingComplete?.Invoke();
        }
    }

    // ═══════════════════════════════════════════════════════════
    // User manager
    // ═══════════════════════════════════════════════════════════

    class UserManager
    {
        public event Action<string> UserRegistered;
        public event Action<string> UserLoggedIn;
        public event Action<string> UserLoggedOut;

        public void RegisterUser(string username)
        {
            Console.WriteLine($"  Registering user: {username}");
            UserRegistered?.Invoke(username);
        }

        public void LoginUser(string username)
        {
            Console.WriteLine($"  User logging in: {username}");
            UserLoggedIn?.Invoke(username);
        }

        public void LogoutUser(string username)
        {
            Console.WriteLine($"  User logging out: {username}");
            UserLoggedOut?.Invoke(username);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: Notification classes
    // ═══════════════════════════════════════════════════════════

    class NotificationService
    {
        public event Action<string> Notification;

        public void Notify(string message)
        {
            Console.WriteLine($"  Service: {message}");
            Notification?.Invoke(message);
        }
    }

    class EmailNotification
    {
        public static void Send(string message) =>
            Console.WriteLine($"  EMAIL: {message}");
    }

    class SMSNotification
    {
        public static void Send(string message) =>
            Console.WriteLine($"  SMS: {message}");
    }

    class PushNotification
    {
        public static void Send(string message) =>
            Console.WriteLine($"  PUSH: {message}");
    }

    // ═══════════════════════════════════════════════════════════
    // File downloader
    // ═══════════════════════════════════════════════════════════

    class FileDownloader
    {
        public event Action<int> ProgressChanged;

        public void Download(string filename)
        {
            Console.WriteLine($"  Downloading: {filename}");
            
            for (int i = 0; i <= 100; i += 25)
            {
                ProgressChanged?.Invoke(i);
            }
            
            Console.WriteLine("  Download complete!");
        }
    }
}
