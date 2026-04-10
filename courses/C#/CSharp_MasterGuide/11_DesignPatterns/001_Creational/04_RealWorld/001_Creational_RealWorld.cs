/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Real-World Applications
 * FILE      : 09_Creational_RealWorld.cs
 * PURPOSE   : Real-world examples of Creational patterns
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational._04_RealWorld
{
    /// <summary>
    /// Real-world Creational pattern demonstrations
    /// </summary>
    public class CreationalRealWorld
    {
        /// <summary>
        /// Entry point for real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Creational Patterns Real-World ===
            Console.WriteLine("=== Creational Patterns Real-World ===\n");

            // ── EXAMPLE: Logging System with Singleton ───────────────────────────
            // Centralized logging across the application

            // Output: --- Singleton: Centralized Logger ---
            Console.WriteLine("--- Singleton: Centralized Logger ---");
            
            // Single instance throughout application
            var logger = Logger.Instance;
            logger.Log("Application started");
            logger.Log("User logged in");
            logger.Log("Data loaded");
            
            // Output: Application started, User logged in, Data loaded (logged 3 times)
            Console.WriteLine("   (logged 3 times)");

            // ── EXAMPLE: Document Factory ───────────────────────────────────────
            // Create different document types dynamically

            // Output: --- Factory Method: Document Generator ---
            Console.WriteLine("\n--- Factory Method: Document Generator ---");
            
            // Generate different document types
            var docFactory = new DocumentGenerator();
            
            var invoice = docFactory.Generate("Invoice", "CUSTOMER-001", 1500.00m);
            // Output: Invoice: Generated for CUSTOMER-001 - $1500.00
            Console.WriteLine($"   Invoice: {invoice}");
            
            var report = docFactory.Generate("Report", "Q1-2024", 45);
            // Output: Report: Generated for Q1-2024 - 45 pages
            Console.WriteLine($"   Report: {report}");
            
            var contract = docFactory.Generate("Contract", "EMP-001", "Full-time");
            // Output: Contract: Generated for EMP-001 - Full-time
            Console.WriteLine($"   Contract: {contract}");

            // ── EXAMPLE: UI Component Builder ───────────────────────────────────
            // Build complex UI components fluently

            // Output: --- Builder: UI Component Builder ---
            Console.WriteLine("\n--- Builder: UI Component Builder ---");
            
            // Build button with many properties
            var button = new UIButtonBuilder()
                .SetText("Submit")
                .SetStyle(ButtonStyle.Primary)
                .SetSize(ButtonSize.Large)
                .SetIcon("check")
                .SetEnabled(true)
                .AddEvent("click", "submitForm()")
                .Build();
            
            // Output: Button: Submit [Primary, Large, Enabled]
            Console.WriteLine($"   Button: {button.Text} [{button.Style}, {button.Size}, {(button.Enabled ? "Enabled" : "Disabled")}]");

            // ── EXAMPLE: Configuration Clone ───────────────────────────────────
            // Clone configuration for different environments

            // Output: --- Prototype: Configuration Templates ---
            Console.WriteLine("\n--- Prototype: Configuration Templates ---");
            
            // Base configuration template
            var baseConfig = new AppConfiguration
            {
                Timeout = 30,
                MaxRetries = 3,
                CacheEnabled = true,
                LogLevel = "Info"
            };
            
            // Clone for development
            var devConfig = baseConfig.Clone();
            devConfig.LogLevel = "Debug";
            devConfig.Timeout = 5;
            
            // Output: Base: Timeout=30, Dev: Timeout=5 (independent)
            Console.WriteLine($"   Base: Timeout={baseConfig.Timeout}, Dev: Timeout={devConfig.Timeout} (independent)");

            // ── EXAMPLE: Theme Abstract Factory ─────────────────────────────────
            // Create consistent cross-platform UI themes

            // Output: --- Abstract Factory: UI Theme System ---
            Console.WriteLine("\n--- Abstract Factory: UI Theme System ---");
            
            // Apply light theme
            var lightTheme = new LightThemeFactory();
            var lightButton = lightTheme.CreateButton();
            var lightInput = lightTheme.CreateInput();
            
            // Output: Light Theme: Button=Rounded, Input=Light Border
            Console.WriteLine($"   Light Theme: Button={lightButton.Render()}, Input={lightInput.Render()}");
            
            // Apply dark theme
            var darkTheme = new DarkThemeFactory();
            var darkButton = darkTheme.CreateButton();
            var darkInput = darkTheme.CreateInput();
            
            // Output: Dark Theme: Button=Rounded Dark, Input=Dark Border
            Console.WriteLine($"   Dark Theme: Button={darkButton.Render()}, Input={darkInput.Render()}");

            Console.WriteLine("\n=== Creational Real-World Complete ===");
        }
    }

    /// <summary>
    /// Centralized Logger (Singleton)
    /// </summary>
    public class Logger
    {
        private static readonly Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());
        private readonly List<string> _logs = new List<string>();
        
        private Logger() { }
        
        /// <summary>
        /// Gets singleton instance
        /// </summary>
        public static Logger Instance => _instance.Value;
        
        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="message">Message to log</param>
        public void Log(string message)
        {
            _logs.Add($"{DateTime.Now:HH:mm:ss} - {message}");
        }
    }

    /// <summary>
    /// Document Generator (Factory Method)
    /// </summary>
    public class DocumentGenerator
    {
        /// <summary>
        /// Generates document based on type
        /// </summary>
        public string Generate(string type, string param1, object param2)
        {
            return type switch
            {
                "Invoice" => $"Generated for {param1} - ${param2}",
                "Report" => $"Generated for {param1} - {param2} pages",
                "Contract" => $"Generated for {param1} - {param2}",
                _ => "Unknown document type"
            };
        }
    }

    /// <summary>
    /// UI Button (Builder result)
    /// </summary>
    public class UIButton
    {
        public string Text { get; set; }
        public ButtonStyle Style { get; set; }
        public ButtonSize Size { get; set; }
        public string Icon { get; set; }
        public bool Enabled { get; set; }
        public Dictionary<string, string> Events { get; set; } = new();
    }

    /// <summary>
    /// Button style enum
    /// </summary>
    public enum ButtonStyle { Primary, Secondary, Danger, Success }
    
    /// <summary>
    /// Button size enum
    /// </summary>
    public enum ButtonSize { Small, Medium, Large }

    /// <summary>
    /// UI Button Builder (Fluent)
    /// </summary>
    public class UIButtonBuilder
    {
        private readonly UIButton _button = new UIButton();
        
        public UIButtonBuilder SetText(string text)
        {
            _button.Text = text;
            return this;
        }
        
        public UIButtonBuilder SetStyle(ButtonStyle style)
        {
            _button.Style = style;
            return this;
        }
        
        public UIButtonBuilder SetSize(ButtonSize size)
        {
            _button.Size = size;
            return this;
        }
        
        public UIButtonBuilder SetIcon(string icon)
        {
            _button.Icon = icon;
            return this;
        }
        
        public UIButtonBuilder SetEnabled(bool enabled)
        {
            _button.Enabled = enabled;
            return this;
        }
        
        public UIButtonBuilder AddEvent(string eventName, string handler)
        {
            _button.Events[eventName] = handler;
            return this;
        }
        
        public UIButton Build() => _button;
    }

    /// <summary>
    /// App Configuration (Prototype)
    /// </summary>
    public class AppConfiguration
    {
        public int Timeout { get; set; }
        public int MaxRetries { get; set; }
        public bool CacheEnabled { get; set; }
        public string LogLevel { get; set; }
        
        /// <summary>
        /// Clones configuration
        /// </summary>
        /// <returns>Deep copy</returns>
        public AppConfiguration Clone()
        {
            return new AppConfiguration
            {
                Timeout = Timeout,
                MaxRetries = MaxRetries,
                CacheEnabled = CacheEnabled,
                LogLevel = LogLevel
            };
        }
    }

    /// <summary>
    /// UI Factory interfaces
    /// </summary>
    public interface IThemeFactory
    {
        IThemeButton CreateButton();
        IThemeInput CreateInput();
    }

    public interface IThemeButton
    {
        string Render();
    }

    public interface IThemeInput
    {
        string Render();
    }

    /// <summary>
    /// Light theme factory
    /// </summary>
    public class LightThemeFactory : IThemeFactory
    {
        public IThemeButton CreateButton() => new LightButton();
        public IThemeInput CreateInput() => new LightInput();
    }

    /// <summary>
    /// Dark theme factory
    /// </summary>
    public class DarkThemeFactory : IThemeFactory
    {
        public IThemeButton CreateButton() => new DarkButton();
        public IThemeInput CreateInput() => new DarkInput();
    }

    /// <summary>
    /// Light button
    /// </summary>
    public class LightButton : IThemeButton
    {
        public string Render() => "Rounded";
    }

    /// <summary>
    /// Light input
    /// </summary>
    public class LightInput : IThemeInput
    {
        public string Render() => "Light Border";
    }

    /// <summary>
    /// Dark button
    /// </summary>
    public class DarkButton : IThemeButton
    {
        public string Render() => "Rounded Dark";
    }

    /// <summary>
    /// Dark input
    /// </summary>
    public class DarkInput : IThemeInput
    {
        public string Render() => "Dark Border";
    }
}