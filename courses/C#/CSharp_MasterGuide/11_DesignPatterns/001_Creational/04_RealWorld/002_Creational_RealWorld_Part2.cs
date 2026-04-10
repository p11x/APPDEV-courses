/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Real-World Part 2
 * FILE      : 10_Creational_RealWorld_Part2.cs
 * PURPOSE   : More real-world Creational pattern examples
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational._04_RealWorld
{
    /// <summary>
    /// More real-world Creational pattern examples
    /// </summary>
    public class CreationalRealWorldPart2
    {
        /// <summary>
        /// Entry point for more real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Creational Patterns Real-World Part 2 ===
            Console.WriteLine("=== Creational Patterns Real-World Part 2 ===\n");

            // ── EXAMPLE: Database Connection Singleton ─────────────────────────
            // Manage single database connection pool

            // Output: --- Singleton: Database Connection Pool ---
            Console.WriteLine("--- Singleton: Database Connection Pool ---");
            
            var dbPool = DatabaseConnectionPool.Instance;
            var conn1 = dbPool.GetConnection();
            var conn2 = dbPool.GetConnection();
            
            // Output: Connection 1: Active, Connection 2: Active, Total: 2
            Console.WriteLine($"   Connection 1: {conn1.Status}, Connection 2: {conn2.Status}, Total: {dbPool.ActiveCount}");
            
            dbPool.ReleaseConnection(conn1);
            // Output: After release: 1
            Console.WriteLine($"   After release: {dbPool.ActiveCount}");

            // ── EXAMPLE: Payment Factory ───────────────────────────────────────
            // Process different payment types

            // Output: --- Factory: Payment Processor ---
            Console.WriteLine("\n--- Factory: Payment Processor ---");
            
            var paymentFactory = new PaymentProcessorFactory();
            
            // Credit card payment
            var credit = paymentFactory.Create("CreditCard");
            // Output: CreditCard: Processed $99.99
            Console.WriteLine($"   CreditCard: {credit.Process(99.99m)}");
            
            // PayPal payment
            var paypal = paymentFactory.Create("PayPal");
            // Output: PayPal: Processed $49.99
            Console.WriteLine($"   PayPal: {paypal.Process(49.99m)}");
            
            // Crypto payment
            var crypto = paymentFactory.Create("Crypto");
            // Output: Crypto: Processed 0.0025 BTC
            Console.WriteLine($"   Crypto: {crypto.Process(0.0025m)}");

            // ── EXAMPLE: Email Builder ─────────────────────────────────────────
            // Build complex emails with attachments

            // Output: --- Builder: Email Message Builder ---
            Console.WriteLine("\n--- Builder: Email Message Builder ---");
            
            var email = new EmailBuilder()
                .To("user@example.com")
                .From("noreply@company.com")
                .Subject("Welcome!")
                .Body("Thank you for joining us.")
                .AddAttachment("welcome.pdf")
                .SetPriority(EmailPriority.High)
                .Build();
            
            // Output: To: user@example.com, Subject: Welcome!, Attachments: 1
            Console.WriteLine($"   To: {email.To}, Subject: {email.Subject}, Attachments: {email.Attachments.Count}");

            // ── EXAMPLE: Report Template Prototype ─────────────────────────────
            // Clone report templates

            // Output: --- Prototype: Report Templates ---
            Console.WriteLine("\n--- Prototype: Report Templates ---");
            
            // Sales report template
            var salesTemplate = new ReportTemplate
            {
                Title = "Monthly Sales Report",
                Format = "PDF",
                Sections = new List<string> { "Summary", "Charts", "Details" }
            };
            
            // Clone for different months
            var januaryReport = salesTemplate.Clone();
            januaryReport.Title = "January Sales Report";
            
            var februaryReport = salesTemplate.Clone();
            februaryReport.Title = "February Sales Report";
            
            // Output: Template: Monthly Sales, January: January Sales, February: February Sales
            Console.WriteLine($"   Template: {salesTemplate.Title}, January: {januaryReport.Title}, February: {februaryReport.Title}");

            // ── EXAMPLE: Cross-Platform UI Abstract Factory ───────────────────
            // Create platform-specific UI components

            // Output: --- Abstract Factory: Cross-Platform UI ---
            Console.WriteLine("\n--- Abstract Factory: Cross-Platform UI ---");
            
            // Windows UI
            var windowsFactory = new WindowsUIFactory();
            var winButton = windowsFactory.CreateButton();
            var winMenu = windowsFactory.CreateMenu();
            
            // Output: Windows: Button=Windows Style, Menu=Windows Menu
            Console.WriteLine($"   Windows: Button={winButton.Style}, Menu={winMenu.Style}");
            
            // Mac UI
            var macFactory = new MacUIFactory();
            var macButton = macFactory.CreateButton();
            var macMenu = macFactory.CreateMenu();
            
            // Output: Mac: Button=Mac Style, Menu=Mac Menu
            Console.WriteLine($"   Mac: Button={macButton.Style}, Menu={macMenu.Style}");

            Console.WriteLine("\n=== Creational Real-World Part 2 Complete ===");
        }
    }

    /// <summary>
    /// Database Connection (Singleton)
    /// </summary>
    public class DatabaseConnection
    {
        public string Status { get; set; } = "Active";
    }

    /// <summary>
    /// Database Connection Pool (Singleton)
    /// </summary>
    public class DatabaseConnectionPool
    {
        private static readonly Lazy<DatabaseConnectionPool> _instance = 
            new Lazy<DatabaseConnectionPool>(() => new DatabaseConnectionPool());
        
        private readonly List<DatabaseConnection> _connections = new();
        private int _activeCount;
        
        private DatabaseConnectionPool()
        {
            for (int i = 0; i < 10; i++)
                _connections.Add(new DatabaseConnection());
        }
        
        public static DatabaseConnectionPool Instance => _instance.Value;
        
        /// <summary>
        /// Gets available connection
        /// </summary>
        public DatabaseConnection GetConnection()
        {
            var conn = _connections.Find(c => c.Status == "Available");
            if (conn != null)
            {
                conn.Status = "Active";
                _activeCount++;
            }
            return conn ?? new DatabaseConnection { Status = "New" };
        }
        
        /// <summary>
        /// Releases connection back to pool
        /// </summary>
        public void ReleaseConnection(DatabaseConnection conn)
        {
            conn.Status = "Available";
            _activeCount--;
        }
        
        public int ActiveCount => _activeCount;
    }

    /// <summary>
    /// Payment processor interface
    /// </summary>
    public interface IPaymentProcessor
    {
        string Process(decimal amount);
    }

    /// <summary>
    /// Credit card payment
    /// </summary>
    public class CreditCardProcessor : IPaymentProcessor
    {
        public string Process(decimal amount) => $"Processed ${amount:F2}";
    }

    /// <summary>
    /// PayPal payment
    /// </summary>
    public class PayPalProcessor : IPaymentProcessor
    {
        public string Process(decimal amount) => $"Processed ${amount:F2}";
    }

    /// <summary>
    /// Crypto payment
    /// </summary>
    public class CryptoProcessor : IPaymentProcessor
    {
        public string Process(decimal amount) => $"Processed {amount:F6} BTC";
    }

    /// <summary>
    /// Payment processor factory
    /// </summary>
    public class PaymentProcessorFactory
    {
        public IPaymentProcessor Create(string type)
        {
            return type switch
            {
                "CreditCard" => new CreditCardProcessor(),
                "PayPal" => new PayPalProcessor(),
                "Crypto" => new CryptoProcessor(),
                _ => throw new ArgumentException($"Unknown payment: {type}")
            };
        }
    }

    /// <summary>
    /// Email message class
    /// </summary>
    public class EmailMessage
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> Attachments { get; set; } = new();
        public EmailPriority Priority { get; set; }
    }

    /// <summary>
    /// Email priority enum
    /// </summary>
    public enum EmailPriority { Low, Normal, High }

    /// <summary>
    /// Email builder (Fluent)
    /// </summary>
    public class EmailBuilder
    {
        private readonly EmailMessage _email = new EmailMessage();
        
        public EmailBuilder To(string to) { _email.To = to; return this; }
        public EmailBuilder From(string from) { _email.From = from; return this; }
        public EmailBuilder Subject(string subject) { _email.Subject = subject; return this; }
        public EmailBuilder Body(string body) { _email.Body = body; return this; }
        public EmailBuilder AddAttachment(string file) { _email.Attachments.Add(file); return this; }
        public EmailBuilder SetPriority(EmailPriority priority) { _email.Priority = priority; return this; }
        public EmailMessage Build() => _email;
    }

    /// <summary>
    /// Report template (Prototype)
    /// </summary>
    public class ReportTemplate
    {
        public string Title { get; set; }
        public string Format { get; set; }
        public List<string> Sections { get; set; } = new();
        
        public ReportTemplate Clone()
        {
            return new ReportTemplate
            {
                Title = Title,
                Format = Format,
                Sections = new List<string>(Sections)
            };
        }
    }

    /// <summary>
    /// UI Button interface
    /// </summary>
    public interface IUIButton
    {
        string Style { get; }
    }

    /// <summary>
    /// UI Menu interface
    /// </summary>
    public interface IUIMenu
    {
        string Style { get; }
    }

    /// <summary>
    /// Windows UI Factory
    /// </summary>
    public class WindowsUIFactory : IUIFactory
    {
        public IUIButton CreateButton() => new WindowsButton();
        public IUIMenu CreateMenu() => new WindowsMenu();
    }

    /// <summary>
    /// Mac UI Factory
    /// </summary>
    public class MacUIFactory : IUIFactory
    {
        public IUIButton CreateButton() => new MacButton();
        public IUIMenu CreateMenu() => new MacMenu();
    }

    public class WindowsButton : IUIButton { public string Style => "Windows Style"; }
    public class WindowsMenu : IUIMenu { public string Style => "Windows Menu"; }
    public class MacButton : IUIButton { public string Style => "Mac Style"; }
    public class MacMenu : IUIMenu { public string Style => "Mac Menu"; }
}