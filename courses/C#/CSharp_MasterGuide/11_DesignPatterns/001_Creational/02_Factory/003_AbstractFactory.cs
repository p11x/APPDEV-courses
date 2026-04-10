/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Abstract Factory Pattern
 * FILE      : 02_AbstractFactory.cs
 * PURPOSE   : Demonstrates Abstract Factory design pattern in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational._02_Factory
{
    /// <summary>
    /// Demonstrates Abstract Factory pattern
    /// </summary>
    public class AbstractFactory
    {
        /// <summary>
        /// Entry point for Abstract Factory examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Abstract Factory Pattern ===
            Console.WriteLine("=== Abstract Factory Pattern ===\n");

            // ── CONCEPT: What is Abstract Factory? ─────────────────────────────
            // Creates families of related objects without specifying concrete types

            // Example 1: Related Product Families
            // Output: 1. Related Product Families:
            Console.WriteLine("1. Related Product Families:");
            
            // Create UI family for Windows
            var windowsFactory = new WindowsUIFactory();
            var windowsButton = windowsFactory.CreateButton();
            var windowsTextBox = windowsFactory.CreateTextBox();
            
            // Output: Windows Button created
            // Output: Windows TextBox created
            Console.WriteLine($"   {windowsButton.GetType().Name} created");
            Console.WriteLine($"   {windowsTextBox.GetType().Name} created");
            
            // Create UI family for Mac
            var macFactory = new MacUIFactory();
            var macButton = macFactory.CreateButton();
            var macTextBox = macFactory.CreateTextBox();
            
            // Output: Mac Button created
            // Output: Mac TextBox created
            Console.WriteLine($"   {macButton.GetType().Name} created");
            Console.WriteLine($"   {macTextBox.GetType().Name} created");

            // ── CONCEPT: Consistent Object Families ──────────────────────────
            // Ensures products are compatible

            // Example 2: Consistent Families
            // Output: 2. Consistent Families:
            Console.WriteLine("\n2. Consistent Families:");
            
            // Both from same factory = compatible
            var factory = new ModernThemeFactory();
            var card = factory.CreateCard();
            var button = factory.CreateButton();
            
            // Output: Modern Card and Modern Button are consistent
            Console.WriteLine($"   {card.GetType().Name} and {button.GetType().Name} are consistent");

            // ── CONCEPT: Swappable Factories ─────────────────────────────────
            // Change entire product family at once

            // Example 3: Swappable Families
            // Output: 3. Swappable Families:
            Console.WriteLine("\n3. Swappable Families:");
            
            // Switch between themes
            RenderTheme(new DarkThemeFactory());
            RenderTheme(new LightThemeFactory());
            
            // Output: Dark Theme: DarkButton, DarkCard
            // Output: Light Theme: LightButton, LightCard

            // ── REAL-WORLD EXAMPLE: Database Abstraction ─────────────────────
            // Output: --- Real-World: Database Abstraction ---
            Console.WriteLine("\n--- Real-World: Database Abstraction ---");
            
            // SQL Server family
            var sqlFactory = new SQLServerFactory();
            var sqlConn = sqlFactory.CreateConnection();
            var sqlCmd = sqlFactory.CreateCommand();
            
            // Output: SQLServer Connection created
            // Output: SQLServer Command created
            Console.WriteLine($"   {sqlConn.GetType().Name} created");
            Console.WriteLine($"   {sqlCmd.GetType().Name} created");
            
            // MySQL family
            var mysqlFactory = new MySQLFactory();
            var mysqlConn = mysqlFactory.CreateConnection();
            var mysqlCmd = mysqlFactory.CreateCommand();
            
            // Output: MySQL Connection created
            // Output: MySQL Command created
            Console.WriteLine($"   {mysqlConn.GetType().Name} created");
            Console.WriteLine($"   {mysqlCmd.GetType().Name} created");

            Console.WriteLine("\n=== Abstract Factory Complete ===");
        }

        /// <summary>
        /// Renders UI with given factory
        /// </summary>
        static void RenderTheme(IUIFactory factory)
        {
            var btn = factory.CreateButton();
            var card = factory.CreateCard();
            Console.WriteLine($"   {btn.GetType().Name}, {card.GetType().Name}");
        }
    }

    // Abstract Factory and Products
    public interface IUIFactory
    {
        IButton CreateButton(); // method: creates button
        ITextBox CreateTextBox(); // method: creates textbox
        ICard CreateCard(); // method: creates card
    }

    public interface IButton { }
    public interface ITextBox { }
    public interface ICard { }

    // Windows Factory
    public class WindowsUIFactory : IUIFactory
    {
        public IButton CreateButton() => new WindowsButton();
        public ITextBox CreateTextBox() => new WindowsTextBox();
        public ICard CreateCard() => new WindowsCard();
    }

    public class WindowsButton : IButton { }
    public class WindowsTextBox : ITextBox { }
    public class WindowsCard : ICard { }

    // Mac Factory
    public class MacUIFactory : IUIFactory
    {
        public IButton CreateButton() => new MacButton();
        public ITextBox CreateTextBox() => new MacTextBox();
        public ICard CreateCard() => new MacCard();
    }

    public class MacButton : IButton { }
    public class MacTextBox : ITextBox { }
    public class MacCard : ICard { }

    // Modern Theme Factory
    public class ModernThemeFactory : IUIFactory
    {
        public IButton CreateButton() => new ModernButton();
        public ITextBox CreateTextBox() => new ModernTextBox();
        public ICard CreateCard() => new ModernCard();
    }

    public class ModernButton : IButton { }
    public class ModernTextBox : ITextBox { }
    public class ModernCard : ICard { }

    // Dark Theme Factory
    public class DarkThemeFactory : IUIFactory
    {
        public IButton CreateButton() => new DarkButton();
        public ITextBox CreateTextBox() => new DarkTextBox();
        public ICard CreateCard() => new DarkCard();
    }

    public class DarkButton : IButton { }
    public class DarkTextBox : ITextBox { }
    public class DarkCard : ICard { }

    // Light Theme Factory
    public class LightThemeFactory : IUIFactory
    {
        public IButton CreateButton() => new LightButton();
        public ITextBox CreateTextBox() => new LightTextBox();
        public ICard CreateCard() => new LightCard();
    }

    public class LightButton : IButton { }
    public class LightTextBox : ITextBox { }
    public class LightCard : ICard { }

    // Database Abstract Factory
    public interface IDatabaseFactory
    {
        IConnection CreateConnection(); // method: creates connection
        ICommand CreateCommand(); // method: creates command
    }

    public interface IConnection { void Connect(); }
    public interface ICommand { void Execute(); string Query { get; set; } }

    public class SQLServerFactory : IDatabaseFactory
    {
        public IConnection CreateConnection() => new SQLServerConnection();
        public ICommand CreateCommand() => new SQLServerCommand();
    }

    public class SQLServerConnection : IConnection { public void Connect() => Console.WriteLine("   Connected to SQL Server"); }
    public class SQLServerCommand : ICommand { public void Execute() { } public string Query { get; set; } }

    public class MySQLFactory : IDatabaseFactory
    {
        public IConnection CreateConnection() => new MySQLConnection();
        public ICommand CreateCommand() => new MySQLCommand();
    }

    public class MySQLConnection : IConnection { public void Connect() => Console.WriteLine("   Connected to MySQL"); }
    public class MySQLCommand : ICommand { public void Execute() { } public string Query { get; set; } }
}