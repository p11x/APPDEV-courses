/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Variables and Constants - Constants and Readonly
 * FILE      : Constants_Readonly.cs
 * PURPOSE   : This file covers const, readonly, and static readonly in C#.
 *             These modifiers create immutable values in different ways.
 * ============================================================
 */

// --- SECTION: Constants and Readonly ---
// Constants create immutable values at compile time (const) or runtime (readonly)
// Both prevent accidental modification but work differently

using System;

namespace CSharp_MasterGuide._01_Fundamentals._03_Variables_Constants
{
    // ── Constants at class level ────────────────────────────────────────────
    // const values are compile-time constants - inlined by compiler
    
    class ConstantsDemo
    {
        // ── const: compile-time constant ───────────────────────────────────
        // Must be initialized with compile-time constant expression
        // Cannot be changed, no storage allocated - inlined everywhere
        
        // Numeric literals
        public const int MaxRetries = 3;
        public const double TaxRate = 0.08;
        
        // String literals (but NOT dynamic strings)
        public const string AppName = "MyApplication";
        public const string Version = "1.0.0";
        
        // null is valid const
        public const string DefaultValue = null;
        
        // Can use const in expressions (all must be compile-time constants)
        public const int BufferSize = MaxRetries * 10; // 30
        
        // Cannot use: const DateTime dt = DateTime.Now; // Not compile-time!
        // Cannot use: const object obj = new object(); // Not compile-time!
        
        // ── readonly: runtime constant ──────────────────────────────────────
        // Set once at runtime (in constructor or initializer), then immutable
        // Has storage - memory allocated for the value
        
        // readonly field - initialized inline or in constructor
        public readonly int MaxConnections;
        public readonly string ConfigPath;
        public readonly DateTime StartTime;
        
        // Static readonly - same as readonly but for static members
        public static readonly string LogPath;
        public static readonly int ProcessId;
        
        // ── readonly static (with initialization) ─────────────────────────
        // Static constructor for complex initialization
        static ConstantsDemo()
        {
            // Static readonly can only be initialized here or inline
            LogPath = @"C:\Logs\app.log"; // Example path
            ProcessId = Environment.ProcessId;
        }
        
        // Instance constructor for instance readonly
        public ConstantsDemo()
        {
            // readonly fields can be initialized in constructor
            MaxConnections = 100;
            ConfigPath = @"C:\Config\app.config";
            StartTime = DateTime.Now;
        }
        
        // Constructor with parameters to set readonly fields
        public ConstantsDemo(int maxConn, string config)
        {
            MaxConnections = maxConn;
            ConfigPath = config;
            StartTime = DateTime.Now;
        }
        
        // Cannot assign to readonly after construction
        // MaxConnections = 200; // ERROR!
        
        // ── const vs readonly comparison ──────────────────────────────────
        public const int ConstValue = 100;
        public readonly int ReadonlyValue; // = 100; // Can also inline
        
        public ConstantsDemo() : this(100, "")
        {
            // Can set readonly in constructor chain
        }
        
        public ConstantsDemo(int val) // Different constructor
        {
            ReadonlyValue = val; // Set to different value
        }
    }

    class Constants_Readonly
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Using const
            // ═══════════════════════════════════════════════════════════════
            
            // const is compile-time constant - no memory allocation
            Console.WriteLine($"MaxRetries: {ConstantsDemo.MaxRetries}"); // Output: MaxRetries: 3
            Console.WriteLine($"TaxRate: {ConstantsDemo.TaxRate}"); // Output: TaxRate: 0.08
            Console.WriteLine($"AppName: {ConstantsDemo.AppName}"); // Output: AppName: MyApplication
            
            // const in calculations
            int bufferSize = ConstantsDemo.BufferSize;
            Console.WriteLine($"BufferSize: {bufferSize}"); // Output: BufferSize: 30 (inlined by compiler)
            
            // const with enum - common pattern
            // Enums are implicitly const in C#
            int statusCode = (int)HttpStatusCode.OK;
            Console.WriteLine($"HTTP OK: {statusCode}"); // Output: HTTP OK: 200

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Using readonly
            // ═══════════════════════════════════════════════════════════════
            
            // readonly is runtime constant - set in constructor
            var demo = new ConstantsDemo();
            Console.WriteLine($"MaxConnections: {demo.MaxConnections}"); // Output: MaxConnections: 100
            Console.WriteLine($"ConfigPath: {demo.ConfigPath}"); // Output: ConfigPath: C:\Config\app.config
            Console.WriteLine($"StartTime: {demo.StartTime}"); // Output: StartTime: (current date/time)
            
            // Different constructor with different values
            var demo2 = new ConstantsDemo(500, "/custom/config");
            Console.WriteLine($"MaxConnections(500): {demo2.MaxConnections}"); // Output: MaxConnections(500): 500
            
            // static readonly
            Console.WriteLine($"LogPath: {ConstantsDemo.LogPath}"); // Output: LogPath: C:\Logs\app.log
            Console.WriteLine($"ProcessId: {ConstantsDemo.ProcessId}"); // Output: ProcessId: (current process ID)

            // ═══════════════════════════════════════════════════════════════
            // SECTION: const vs readonly - When to use which
            // ═══════════════════════════════════════════════════════════════
            
            // ── Use const when: ─────────────────────────────────────────────
            // - Value is known at compile time
            // - Value will never change
            // - You want inlining for performance
            // - Works with primitive types, string literals, enums
            
            // ── Use readonly when: ─────────────────────────────────────────
            // - Value determined at runtime
            // - Value could potentially change (e.g., loaded from config)
            // - Need complex initialization
            // - Need to use with non-const expressions
            
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World readonly Scenarios
            // ═══════════════════════════════════════════════════════════════
            
            // ── Configuration loaded at runtime ───────────────────────────
            var config = new AppConfig("production");
            Console.WriteLine($"Environment: {config.Environment}"); // Output: Environment: production
            Console.WriteLine($"ConnectionTimeout: {config.ConnectionTimeout}"); // Output: ConnectionTimeout: 30
            
            // ── Singleton pattern with readonly ───────────────────────────
            var singleton = Singleton.Instance;
            Console.WriteLine($"Singleton ID: {singleton.Id}"); // Output: Singleton ID: 1

            // ── Thread-safe counter with Interlocked (advanced) ───────────
            // Interlocked provides atomic operations for certain types
            int counter = 0;
            Interlocked.Increment(ref counter); // Thread-safe increment
            Interlocked.Add(ref counter, 10); // Thread-safe add
            Console.WriteLine($"Thread-safe counter: {counter}"); // Output: Thread-safe counter: 11

            // ═══════════════════════════════════════════════════════════════
            // SECTION: enum with const and readonly
            // ═══════════════════════════════════════════════════════════════
            
            // Enums are compile-time constants
            LogLevel level = LogLevel.Warning;
            Console.WriteLine($"Log level: {level}"); // Output: Log level: Warning
            
            // Can use switch on const enum values
            switch (level)
            {
                case LogLevel.Debug:
                    Console.WriteLine("Detailed debug info");
                    break;
                case LogLevel.Info:
                    Console.WriteLine("General info");
                    break;
                case LogLevel.Warning:
                    Console.WriteLine("Warning message"); // Output: Warning message
                    break;
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: const in mathematical calculations
            // ═══════════════════════════════════════════════════════════════
            
            const double PI = 3.14159;
            const double E = 2.71828;
            
            double radius = 5;
            double circleArea = PI * radius * radius;
            double circleCircumference = 2 * PI * radius;
            
            Console.WriteLine($"Circle area (r=5): {circleArea:F2}"); // Output: Circle area (r=5): 78.54
            Console.WriteLine($"Circle circumference: {circleCircumference:F2}"); // Output: Circle circumference: 31.42
        }
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // Supporting classes and enums
    // ═══════════════════════════════════════════════════════════════════════
    
    // Simple enum - values are implicitly const
    enum HttpStatusCode
    {
        OK = 200,
        Created = 201,
        BadRequest = 400,
        Unauthorized = 401,
        NotFound = 404,
        InternalServerError = 500
    }
    
    enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3,
        Critical = 4
    }
    
    // Configuration class with readonly fields
    class AppConfig
    {
        public readonly string Environment;
        public readonly int ConnectionTimeout;
        
        public AppConfig(string environment)
        {
            // Determine values at runtime based on environment
            Environment = environment;
            
            // Different timeout based on environment
            ConnectionTimeout = environment.ToLower() == "production" ? 30 : 10;
        }
    }
    
    // Singleton pattern - uses readonly for instance
    class Singleton
    {
        // readonly ensures single instance - can only set once
        public static readonly Singleton Instance;
        
        public int Id { get; }
        
        static Singleton()
        {
            // Static constructor - runs once before first use
            Instance = new Singleton(1);
        }
        
        private Singleton(int id)
        {
            Id = id;
        }
    }
}
