/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Delegates and Events - Event Handlers Part 2
 * FILE      : EventHandlers_Part2.cs
 * PURPOSE   : Async events, weak events pattern, and advanced
 *            event handling techniques
 * ============================================================
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._03_Advanced_OOP._02_Delegates_Events
{
    class EventHandlers_Part2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Event Handlers Part 2 ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Async Events Best Practices
            // ═══════════════════════════════════════════════════════════

            // Async event handlers should be careful about exceptions
            // Use try-catch in async handlers
            // Consider fire-and-forget patterns

            // ── EXAMPLE 1: Async Event Handler ───────────────────────────
            Console.WriteLine("--- Async Event Handlers ---");
            
            var apiClient = new ApiClient();
            
            apiClient.ResponseReceived += async (sender, e) =>
            {
                Console.WriteLine($"  Processing response: {e.Status}");
                
                try
                {
                    await Task.Delay(50);  // Simulate async processing
                    Console.WriteLine($"  Response processed successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  Error processing: {ex.Message}");
                }
            };
            
            apiClient.Fetch("api/data");

            Thread.Sleep(200);  // Wait for async operations

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Multiple Async Handlers
            // ═══════════════════════════════════════════════════════════

            // Multiple handlers can be async
            // Each runs independently
            // Use Task.WhenAll for coordinated async

            // ── EXAMPLE 1: Multiple Async Handlers ────────────────────────
            Console.WriteLine("\n--- Multiple Async Handlers ---");
            
            var notifier = new AsyncNotifier();
            
            // Handler 1
            notifier.Notification += async (sender, e) =>
            {
                await Task.Delay(100);
                Console.WriteLine($"  Handler 1 processed: {e.Message}");
            };
            
            // Handler 2
            notifier.Notification += async (sender, e) =>
            {
                await Task.Delay(50);
                Console.WriteLine($"  Handler 2 processed: {e.Message}");
            };
            
            notifier.Notify("Hello Async!");

            Thread.Sleep(200);

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Weak Events Pattern
            // ═══════════════════════════════════════════════════════════

            // Weak events prevent memory leaks
            // Subscriber can be garbage collected even if not unsubscribed
            // Use WeakEventManager for implementation

            // ── EXAMPLE 1: Weak Event Basics ──────────────────────────────
            Console.WriteLine("\n--- Weak Events Pattern ---");
            
            var subject = new WeakEventSubject();
            
            // Create weak subscriber
            var subscriber = new WeakEventSubscriber();
            subscriber.Subscribe(subject);
            
            subject.RaiseEvent("First event");
            subject.RaiseEvent("Second event");
            
            Console.WriteLine("  Subscriber still alive: " + (subscriber != null));
            
            // Force garbage collection
            subscriber = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
            Console.WriteLine("  After GC - raising event:");
            subject.RaiseEvent("Third event (subscriber may be gone)");

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Custom Weak Event Implementation
            // ═══════════════════════════════════════════════════════════

            // Create custom weak event manager
            // Stores weak references to handlers

            // ── EXAMPLE 1: Custom WeakEventManager ──────────────────────
            Console.WriteLine("\n--- Custom WeakEventManager ---");
            
            var publisher = new CustomWeakEventPublisher();
            
            var listener = new CustomWeakListener();
            publisher.Event += listener.HandleEvent;
            
            publisher.Raise("Before GC");
            
            // Remove strong reference
            publisher.Event -= listener.HandleEvent;
            
            // Listener can now be garbage collected
            listener = null;
            GC.Collect();
            
            publisher.Raise("After GC");

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Event Timing and Synchronization
            // ═══════════════════════════════════════════════════════════

            // Events can be raised synchronously or asynchronously
            // Consider thread safety in multi-threaded scenarios

            // ── EXAMPLE 1: Synchronous vs Async Raise ───────────────────
            Console.WriteLine("\n--- Synchronous vs Async Raise ---");
            
            var timer = new TimerEvents();
            
            timer.TimerTick += () => Console.WriteLine("  Tick handler executed");
            
            Console.WriteLine("  Starting timer...");
            timer.Start();
            
            Thread.Sleep(150);
            timer.Stop();
            Console.WriteLine("  Timer stopped");

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Debouncing and Throttling Events
            // ═══════════════════════════════════════════════════════════

            // Debounce: waits for quiet period before firing
            // Throttle: fires at regular intervals

            // ── EXAMPLE 1: Simple Debounce ───────────────────────────────
            Console.WriteLine("\n--- Event Debouncing ---");
            
            var searchBox = new SearchBox();
            
            searchBox.TextChanged += (text) =>
                Console.WriteLine($"  Searching for: {text}");
            
            // Simulate rapid typing
            searchBox.Type("h");
            searchBox.Type("he");
            searchBox.Type("hel");
            searchBox.Type("hell");
            searchBox.Type("hello");

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Real-World: Chat Application
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Chat Message Events ──────────────────────────
            Console.WriteLine("\n--- Real-World: Chat Application ---");
            
            var chat = new ChatRoom();
            
            chat.MessageReceived += (sender, e) =>
                Console.WriteLine($"  [{e.Sender}] {e.Message}");
            
            chat.MessageReceived += async (sender, e) =>
            {
                // Simulate auto-response
                if (e.Message.Contains("hello"))
                {
                    await Task.Delay(100);
                    Console.WriteLine("  [Bot] Hello! How can I help?");
                }
            };
            
            chat.SendMessage("Alice", "Hello there!");
            chat.SendMessage("Bob", "Hi Alice!");

            Thread.Sleep(200);

            // ── EXAMPLE 2: Connection Manager ────────────────────────────
            Console.WriteLine("\n--- Real-World: Connection Events ---");
            
            var connection = new ConnectionManager();
            
            connection.Connected += (endpoint) =>
                Console.WriteLine($"  Connected to: {endpoint}");
            connection.Disconnected += (endpoint) =>
                Console.WriteLine($"  Disconnected from: {endpoint}");
            connection.Error += (ex) =>
                Console.WriteLine($"  Error: {ex.Message}");
            
            connection.Connect("server1.example.com");
            connection.Disconnect();

            Console.WriteLine("\n=== Event Handlers Part 2 Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // API Client with async events
    // ═══════════════════════════════════════════════════════════

    class ResponseEventArgs : EventArgs
    {
        public string Status { get; }
        public string Data { get; }

        public ResponseEventArgs(string status, string data)
        {
            Status = status;
            Data = data;
        }
    }

    class ApiClient
    {
        public event EventHandler<ResponseEventArgs> ResponseReceived;

        public async void Fetch(string endpoint)
        {
            await Task.Delay(100);
            ResponseReceived?.Invoke(this, new ResponseEventArgs("200 OK", "Data"));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Async Notifier
    // ═══════════════════════════════════════════════════════════

    class NotificationEventArgs : EventArgs
    {
        public string Message { get; }
        
        public NotificationEventArgs(string message)
        {
            Message = message;
        }
    }

    class AsyncNotifier
    {
        public event EventHandler<NotificationEventArgs> Notification;

        public void Notify(string message)
        {
            Notification?.Invoke(this, new NotificationEventArgs(message));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Weak Events - Subject
    // ═══════════════════════════════════════════════════════════

    class WeakEventSubject
    {
        // Using WeakEventManager for proper weak event pattern
        public static readonly WeakEventManager<string> EventManager = new WeakEventManager<string>();

        public void RaiseEvent(string message)
        {
            // Check if any listeners exist
            if (EventManager.HasListeners)
            {
                Console.WriteLine($"  Raising event: {message}");
                EventManager.HandleEvent(this, message);
            }
            else
            {
                Console.WriteLine($"  No listeners for: {message}");
            }
        }
    }

    class WeakEventSubscriber
    {
        public void Subscribe(WeakEventSubject subject)
        {
            // Subscribe using weak reference
            WeakEventSubject.EventManager.AddEventHandler(this, (sender, message) =>
            {
                Console.WriteLine($"  Subscriber received: {message}");
            });
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Custom Weak Event Publisher
    // ═══════════════════════════════════════════════════════════

    class CustomWeakEventPublisher
    {
        // Store weak references
        private List<WeakReference<EventHandler<string>>> _handlers = new List<WeakReference<EventHandler<string>>>();

        public event EventHandler<string> Event
        {
            add
            {
                _handlers.Add(new WeakReference<EventHandler<string>>(value));
            }
            remove
            {
                // In real implementation, need to find and remove the weak reference
            }
        }

        public void Raise(string message)
        {
            var deadRefs = new List<WeakReference<EventHandler<string>>>();
            
            foreach (var weakRef in _handlers)
            {
                if (weakRef.TryGetTarget(out var handler))
                {
                    handler(this, message);
                }
                else
                {
                    deadRefs.Add(weakRef);
                }
            }
            
            // Clean up dead references
            foreach (var dead in deadRefs)
            {
                _handlers.Remove(dead);
            }
        }
    }

    class CustomWeakListener
    {
        public void HandleEvent(object sender, string message)
        {
            Console.WriteLine($"  Listener received: {message}");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Timer events
    // ═══════════════════════════════════════════════════════════

    class TimerEvents
    {
        public event Action TimerTick;

        public void Start()
        {
            var timer = new Timer(_ =>
            {
                TimerTick?.Invoke();
            }, null, 0, 50);
        }

        public void Stop()
        {
            // Timer disposal would happen here
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Search box with debounce
    // ═══════════════════════════════════════════════════════════

    class SearchBox
    {
        private System.Timers.Timer _debounceTimer;
        private string _pendingText;

        public event Action<string> TextChanged;

        public void Type(string text)
        {
            _pendingText = text;
            
            // Cancel previous timer
            _debounceTimer?.Stop();
            _debounceTimer?.Dispose();
            
            // Start new debounce timer (300ms delay)
            _debounceTimer = new System.Timers.Timer(300);
            _debounceTimer.Elapsed += (sender, e) =>
            {
                _debounceTimer.Stop();
                TextChanged?.Invoke(_pendingText);
            };
            _debounceTimer.AutoReset = false;
            _debounceTimer.Start();
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Chat Room
    // ═══════════════════════════════════════════════════════════

    class ChatMessageEventArgs : EventArgs
    {
        public string Sender { get; }
        public string Message { get; }

        public ChatMessageEventArgs(string sender, string message)
        {
            Sender = sender;
            Message = message;
        }
    }

    class ChatRoom
    {
        public event EventHandler<ChatMessageEventArgs> MessageReceived;

        public void SendMessage(string sender, string message)
        {
            MessageReceived?.Invoke(this, new ChatMessageEventArgs(sender, message));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Connection Manager
    // ═══════════════════════════════════════════════════════════

    class ConnectionManager
    {
        public event Action<string> Connected;
        public event Action<string> Disconnected;
        public event Action<Exception> Error;

        public void Connect(string endpoint)
        {
            Console.WriteLine($"  Connecting to {endpoint}...");
            Connected?.Invoke(endpoint);
        }

        public void Disconnect()
        {
            Console.WriteLine("  Disconnecting...");
            Disconnected?.Invoke("server1.example.com");
        }

        public void SimulateError(Exception ex)
        {
            Error?.Invoke(ex);
        }
    }
}
