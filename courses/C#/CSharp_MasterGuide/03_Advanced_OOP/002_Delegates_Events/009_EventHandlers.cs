/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Delegates and Events - Event Handlers
 * FILE      : EventHandlers.cs
 * PURPOSE   : Standard event handlers, EventHandler<T>,
 *            custom event handlers, and handler patterns
 * ============================================================
 */

using System;
using System.Threading;

namespace CSharp_MasterGuide._03_Advanced_OOP._02_Delegates_Events
{
    class EventHandlers
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Event Handlers in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Understanding Event Handlers
            // ═══════════════════════════════════════════════════════════

            // Event handler is a method that responds to an event
            // Must match delegate signature of the event
            // Two parameters: sender (object) and event args (EventArgs)

            // ── EXAMPLE 1: Basic EventHandler Delegate ──────────────────
            Console.WriteLine("--- Basic EventHandler Delegate ---");
            
            var server = new Server();
            
            // Subscribe with standard EventHandler
            server.RequestReceived += (sender, e) =>
                Console.WriteLine($"  Request received from {e.ClientIP}");
            
            server.AcceptRequest("192.168.1.100");

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: EventHandler<T> Generic Delegate
            // ═══════════════════════════════════════════════════════════

            // EventHandler<T> is generic - T must derive from EventArgs
            // Reduces boilerplate code for custom events
            // Built into .NET framework

            // ── EXAMPLE 1: EventHandler<T> Usage ────────────────────────
            Console.WriteLine("\n--- EventHandler<T> Generic ---");
            
            var logger = new Logger();
            
            // Subscribe with EventHandler<LogEventArgs>
            logger.MessageLogged += (sender, e) =>
            {
                Console.WriteLine($"  [{e.Level}] {e.Message}");
            };
            
            logger.Log("Application started", LogLevel.Info);
            logger.Log("Error occurred", LogLevel.Error);
            logger.Log("Warning: Low memory", LogLevel.Warning);

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Custom Event Handlers
            // ═══════════════════════════════════════════════════════════

            // Can create custom delegate types for events
            // Useful when standard patterns don't fit

            // ── EXAMPLE 1: Custom Delegate Signature ─────────────────────
            Console.WriteLine("\n--- Custom Event Handlers ---");
            
            var downloader = new FileDownloaderCustom();
            
            // Custom handler with specific parameters
            downloader.DownloadProgress += OnDownloadProgress;
            downloader.DownloadComplete += OnDownloadComplete;
            
            downloader.StartDownload("data.zip");

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Handler Methods vs Lambdas
            // ═══════════════════════════════════════════════════════════

            // Handlers can be defined as named methods or lambdas
            // Named methods: better for reuse, debugging
            // Lambdas: concise, inline, for simple logic

            // ── EXAMPLE 1: Named Method Handler ──────────────────────────
            Console.WriteLine("\n--- Named Method vs Lambda ---");
            
            var publisher = new EventPublisherNamed();
            
            // Named method handler
            publisher.EventFired += HandleEventFired;
            
            // Lambda handler
            publisher.EventFired += (sender, e) =>
                Console.WriteLine($"  Lambda handler: {e.Message}");
            
            publisher.RaiseEvent("Test message");

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Multiple Handlers with Different Signatures
            // ═══════════════════════════════════════════════════════════

            // Events can work with different delegate types
            // Can combine multiple subscriptions

            // ── EXAMPLE 1: Handler with Sender ────────────────────────────
            Console.WriteLine("\n--- Handler with Sender ---");
            
            var button = new ButtonHandler();
            
            // Handler that uses sender
            button.Click += Button_Click;
            
            button.ClickMe();

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Async Event Handlers
            // ═══════════════════════════════════════════════════════════

            // Event handlers can be async
            // Must be async void for event handlers
            // Useful for I/O operations

            // ── EXAMPLE 1: Async Event Handler ───────────────────────────
            Console.WriteLine("\n--- Async Event Handlers ---");
            
            var dataService = new DataService();
            
            dataService.DataLoaded += async (sender, e) =>
            {
                Console.WriteLine($"  Loading data for: {e.Category}");
                await Task.Delay(100);  // Simulate async work
                Console.WriteLine($"  Data loaded: {e.Data.Count} items");
            };
            
            dataService.LoadData("products");

            Thread.Sleep(200);  // Wait for async to complete

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Real-World: Game Event System
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Game Events ───────────────────────────────────
            Console.WriteLine("\n--- Real-World: Game Events ---");
            
            var game = new Game();
            
            game.PlayerDied += (sender, e) =>
                Console.WriteLine($"  Game Over! Player died. Score: {e.Score}");
            
            game.ScoreChanged += (sender, e) =>
                Console.WriteLine($"  Score: {e.Score}");
            
            game.AddScore(100);
            game.AddScore(50);
            game.PlayerDies();

            // ── EXAMPLE 2: Multiple Game Events ──────────────────────────
            Console.WriteLine("\n--- Multiple Game Events ---");
            
            var game2 = new Game2();
            
            game2.LevelCompleted += (sender, e) =>
                Console.WriteLine($"  Level {e.Level} completed! Bonus: {e.Bonus}");
            
            game2.EnemyKilled += (sender, e) =>
                Console.WriteLine($"  Enemy killed! XP: {e.XP}");
            
            game2.CompleteLevel(1, 500);
            game2.KillEnemy(10);

            Console.WriteLine("\n=== Event Handlers Complete ===");
        }

        // Named handler methods
        static void OnDownloadProgress(int percent, string file)
        {
            Console.WriteLine($"  Download: {file} - {percent}%");
        }

        static void OnDownloadComplete(string file, bool success)
        {
            Console.WriteLine($"  Download {file} - {(success ? "SUCCESS" : "FAILED")}");
        }

        static void HandleEventFired(object sender, EventFiredArgs e)
        {
            Console.WriteLine($"  Named handler: {e.Message}");
        }

        static void Button_Click(object sender, EventArgs e)
        {
            var button = sender as ButtonHandler;
            Console.WriteLine($"  Button clicked! Button text: {button?.Text}");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Basic EventHandler example
    // ═══════════════════════════════════════════════════════════

    class RequestEventArgs : EventArgs
    {
        public string ClientIP { get; }
        
        public RequestEventArgs(string clientIP)
        {
            ClientIP = clientIP;
        }
    }

    class Server
    {
        public event EventHandler<RequestEventArgs> RequestReceived;

        public void AcceptRequest(string clientIP)
        {
            RequestReceived?.Invoke(this, new RequestEventArgs(clientIP));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Logger with EventHandler<T>
    // ═══════════════════════════════════════════════════════════

    enum LogLevel
    {
        Info,
        Warning,
        Error,
        Debug
    }

    class LogEventArgs : EventArgs
    {
        public string Message { get; }
        public LogLevel Level { get; }
        public DateTime Timestamp { get; }

        public LogEventArgs(string message, LogLevel level)
        {
            Message = message;
            Level = level;
            Timestamp = DateTime.Now;
        }
    }

    class Logger
    {
        public event EventHandler<LogEventArgs> MessageLogged;

        public void Log(string message, LogLevel level)
        {
            MessageLogged?.Invoke(this, new LogEventArgs(message, level));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Custom event handlers
    // ═══════════════════════════════════════════════════════════

    class FileDownloaderCustom
    {
        // Custom delegate for progress (no EventArgs needed)
        public delegate void ProgressHandler(int percent, string file);
        
        // Custom delegate for completion
        public delegate void CompleteHandler(string file, bool success);

        public event ProgressHandler DownloadProgress;
        public event CompleteHandler DownloadComplete;

        public void StartDownload(string filename)
        {
            for (int i = 0; i <= 100; i += 20)
            {
                DownloadProgress?.Invoke(i, filename);
                Thread.Sleep(50);
            }
            
            DownloadComplete?.Invoke(filename, true);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Named handler example
    // ═══════════════════════════════════════════════════════════

    class EventFiredArgs : EventArgs
    {
        public string Message { get; }
        
        public EventFiredArgs(string message)
        {
            Message = message;
        }
    }

    class EventPublisherNamed
    {
        public event EventHandler<EventFiredArgs> EventFired;

        public void RaiseEvent(string message)
        {
            EventFired?.Invoke(this, new EventFiredArgs(message));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Button with text property
    // ═══════════════════════════════════════════════════════════

    class ButtonHandler
    {
        public string Text { get; set; } = "Click Me";

        public event EventHandler Click;

        public void ClickMe()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Data service with async
    // ═══════════════════════════════════════════════════════════

    class DataLoadedArgs : EventArgs
    {
        public string Category { get; }
        public List<string> Data { get; }

        public DataLoadedArgs(string category, List<string> data)
        {
            Category = category;
            Data = data;
        }
    }

    class DataService
    {
        public event EventHandler<DataLoadedArgs> DataLoaded;

        public async void LoadData(string category)
        {
            await Task.Delay(100);
            var data = new List<string> { "Item1", "Item2", "Item3" };
            DataLoaded?.Invoke(this, new DataLoadedArgs(category, data));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: Game events
    // ═══════════════════════════════════════════════════════════

    class PlayerDiedArgs : EventArgs
    {
        public int Score { get; }
        
        public PlayerDiedArgs(int score)
        {
            Score = score;
        }
    }

    class ScoreChangedArgs : EventArgs
    {
        public int Score { get; }
        
        public ScoreChangedArgs(int score)
        {
            Score = score;
        }
    }

    class Game
    {
        private int _score;

        public event EventHandler<PlayerDiedArgs> PlayerDied;
        public event EventHandler<ScoreChangedArgs> ScoreChanged;

        public void AddScore(int points)
        {
            _score += points;
            ScoreChanged?.Invoke(this, new ScoreChangedArgs(_score));
        }

        public void PlayerDies()
        {
            PlayerDied?.Invoke(this, new PlayerDiedArgs(_score));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: Game events (level 2)
    // ═══════════════════════════════════════════════════════════

    class LevelCompletedArgs : EventArgs
    {
        public int Level { get; }
        public int Bonus { get; }

        public LevelCompletedArgs(int level, int bonus)
        {
            Level = level;
            Bonus = bonus;
        }
    }

    class EnemyKilledArgs : EventArgs
    {
        public int XP { get; }

        public EnemyKilledArgs(int xp)
        {
            XP = xp;
        }
    }

    class Game2
    {
        public event EventHandler<LevelCompletedArgs> LevelCompleted;
        public event EventHandler<EnemyKilledArgs> EnemyKilled;

        public void CompleteLevel(int level, int bonus)
        {
            LevelCompleted?.Invoke(this, new LevelCompletedArgs(level, bonus));
        }

        public void KillEnemy(int xp)
        {
            EnemyKilled?.Invoke(this, new EnemyKilledArgs(xp));
        }
    }
}
