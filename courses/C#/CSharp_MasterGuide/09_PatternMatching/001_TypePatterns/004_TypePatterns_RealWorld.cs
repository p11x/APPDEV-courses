/*
 * ============================================================
 * TOPIC     : Pattern Matching
 * SUBTOPIC  : Type Patterns - Real-World Applications
 * FILE      : 04_TypePatterns_RealWorld.cs
 * PURPOSE   : Demonstrates practical applications of type pattern matching in real-world scenarios
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List, Dictionary

namespace CSharp_MasterGuide._09_PatternMatching._01_TypePatterns
{
    /// <summary>
    /// Real-world applications of type pattern matching
    /// </summary>
    public class TypePatterns_RealWorld
    {
        /// <summary>
        /// Entry point for real-world type pattern examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Real-World Type Patterns Demo ===
            Console.WriteLine("=== Real-World Type Patterns Demo ===\n");

            // ── REAL-WORLD: Event Handler with Different Event Types ──────────
            // Output: 1. Event Processing System:
            Console.WriteLine("1. Event Processing System:");
            
            // Process various event types using pattern matching
            // ProcessEvent handles different event types appropriately
            ProcessEvent(new UserCreatedEvent("Alice", "alice@email.com"));
            ProcessEvent(new OrderPlacedEvent("ORD-123", 150.00m));
            ProcessEvent(new PaymentReceivedEvent("PAY-456", 99.99m));
            ProcessEvent(new EmailSentEvent("Welcome", "alice@email.com"));

            // ── REAL-WORLD: Configuration Parser ─────────────────────────────
            // Output: 2. Configuration Parser:
            Console.WriteLine("\n2. Configuration Parser:");
            
            // Parse different configuration value types
            // ParseConfigValue demonstrates type-based parsing
            ParseConfigValue("timeout", 30);
            ParseConfigValue("debug", true);
            ParseConfigValue("rate", 3.14);
            ParseConfigValue("name", "MyApp");
            ParseConfigValue("tags", new string[] { "web", "api" });

            // ── REAL-WORLD: Message Queue Handler ────────────────────────────
            // Output: 3. Message Queue Processor:
            Console.WriteLine("\n3. Message Queue Processor:");
            
            // Handle different message types from queue
            // ProcessMessage routes based on message type
            ProcessMessage(new TextMessage("user1", "Hello!"));
            ProcessMessage(new ImageMessage("user2", "photo.jpg", 1024));
            ProcessMessage(new FileMessage("user3", "document.pdf", 5120));
            ProcessMessage(new SystemMessage("USER_JOINED", "New user registered"));

            // ── REAL-WORLD: Shape Calculator ─────────────────────────────────
            // Output: 4. Shape Area Calculator:
            Console.WriteLine("\n4. Shape Area Calculator:");
            
            // Calculate area for different shapes
            // CalculateArea demonstrates polymorphic pattern matching
            Console.WriteLine($"   Circle (r=5): {CalculateArea(new Circle(5)):F2}");
            Console.WriteLine($"   Rectangle (4x6): {CalculateArea(new Rectangle(4, 6)):F2}");
            Console.WriteLine($"   Triangle (b=3, h=4): {CalculateArea(new Triangle(3, 4)):F2}");
            Console.WriteLine($"   Square (s=5): {CalculateArea(new Square(5)):F2}");

            // ── REAL-WORLD: Tree Node Processor ─────────────────────────────
            // Output: 5. Binary Tree Processor:
            Console.WriteLine("\n5. Binary Tree Processor:");
            
            // Build and process binary tree
            // TreeNode<T> = generic tree node structure
            var root = new TreeNode<int>(10,
                new TreeNode<int>(5,
                    new TreeNode<int>(3, null, null),
                    new TreeNode<int>(7, null, null)),
                new TreeNode<int>(15,
                    new TreeNode<int>(12, null, null),
                    new TreeNode<int>(20, null, null)));
            
            // TraverseTree performs different operations based on node type
            Console.WriteLine($"   Tree sum: {TraverseTree(root)}");
            Console.WriteLine($"   Node count: {CountNodes(root)}");
            Console.WriteLine($"   Max value: {FindMax(root)}");

            // ── REAL-WORLD: Game Character Processor ────────────────────────
            // Output: 6. Game Character Processor:
            Console.WriteLine("\n6. Game Character Processor:");
            
            // Process different game character types
            // AttackWithCharacter calculates damage based on character type
            var warrior = new Warrior(100, 25);
            var mage = new Mage(80, 50);
            var archer = new Archer(90, 30);
            varhealer = new Healer(70, 15);
            
            // Character[] = array of character types
            Character[] characters = { warrior, mage, archer, healer };
            
            // foreach = iterate through characters
            foreach (var character in characters)
            {
                // Output: [CharacterType]: HP=[hp], Attack=[attack]
                Console.WriteLine($"   {character.GetType().Name}: HP={character.Health}, Attack={character.AttackPower}");
            }

            Console.WriteLine("\n=== Real-World Type Patterns Complete ===");
        }

        /// <summary>
        /// Processes different event types using pattern matching
        /// </summary>
        public static void ProcessEvent(object evt)
        {
            // Pattern match on event type
            switch (evt)
            {
                case UserCreatedEvent user:
                    // Output: [Event] User Created: Alice (alice@email.com)
                    Console.WriteLine($"   [Event] User Created: {user.Name} ({user.Email})");
                    break;
                    
                case OrderPlacedEvent order:
                    // decimal = precise monetary type
                    // Output: [Event] Order Placed: ORD-123 - $150.00
                    Console.WriteLine($"   [Event] Order Placed: {order.OrderId} - ${order.Amount:F2}");
                    break;
                    
                case PaymentReceivedEvent payment:
                    // Output: [Event] Payment: PAY-456 - $99.99
                    Console.WriteLine($"   [Event] Payment: {payment.PaymentId} - ${payment.Amount:F2}");
                    break;
                    
                case EmailSentEvent email:
                    // Output: [Event] Email Sent: "Welcome" to alice@email.com
                    Console.WriteLine($"   [Event] Email Sent: \"{email.Subject}\" to {email.Recipient}");
                    break;
                    
                default:
                    // Output: [Event] Unknown type
                    Console.WriteLine($"   [Event] Unknown type: {evt.GetType().Name}");
                    break;
            }
        }

        /// <summary>
        /// Parses configuration values based on their type
        /// </summary>
        public static void ParseConfigValue(string key, object value)
        {
            // Pattern match on value type
            switch (value)
            {
                case int intVal:
                    // Output: Config [key]: [value] (integer)
                    Console.WriteLine($"   Config {key}: {intVal} (integer)");
                    break;
                    
                case bool boolVal:
                    // Output: Config [key]: [value] (boolean)
                    Console.WriteLine($"   Config {key}: {boolVal} (boolean)");
                    break;
                    
                case double doubleVal:
                    // Output: Config [key]: [value] (double)
                    Console.WriteLine($"   Config {key}: {doubleVal:F2} (double)");
                    break;
                    
                case string strVal:
                    // Output: Config [key]: [value] (string)
                    Console.WriteLine($"   Config {key}: {strVal} (string)");
                    break;
                    
                case string[] arr:
                    // string[] = array of strings
                    // Output: Config [key]: [comma-separated values]
                    Console.WriteLine($"   Config {key}: [{string.Join(", ", arr)}] (string array)");
                    break;
                    
                default:
                    // Output: Config [key]: Unknown type
                    Console.WriteLine($"   Config {key}: {value} (unknown type)");
                    break;
            }
        }

        /// <summary>
        /// Processes messages from a queue
        /// </summary>
        public static void ProcessMessage(object message)
        {
            // Pattern match message type
            // Each message type has different processing logic
            
            string result = message switch
            {
                // TextMessage = simple text message
                TextMessage t => $"[TEXT] {t.Sender}: {t.Content}",
                
                // ImageMessage = message with image attachment
                ImageMessage i => $"[IMAGE] {i.Sender}: {i.FileName} ({i.SizeKB}KB)",
                
                // FileMessage = generic file transfer
                FileMessage f => $"[FILE] {f.Sender}: {f.FileName} ({f.SizeKB}KB)",
                
                // SystemMessage = system notification
                SystemMessage s => $"[SYSTEM] {s.EventType}: {s.Data}",
                
                // Default case
                _ => $"[UNKNOWN] {message.GetType().Name}"
            };
            
            // Output: [processed result]
            Console.WriteLine($"   {result}");
        }

        /// <summary>
        /// Calculates area of different shapes using pattern matching
        /// </summary>
        public static double CalculateArea(object shape)
        {
            // Pattern match on shape type
            // Calculate area based on shape properties
            
            return shape switch
            {
                // Circle: π * r²
                Circle c => Math.PI * c.Radius * c.Radius,
                
                // Rectangle: width * height
                Rectangle r => r.Width * r.Height,
                
                // Triangle: 0.5 * base * height
                Triangle t => 0.5 * t.Base * t.Height,
                
                // Square: side * side (inherits from Rectangle)
                Square s => s.Side * s.Side,
                
                // Default: 0 area
                _ => 0
            };
        }

        /// <summary>
        /// Calculates sum of all values in binary tree
        /// </summary>
        public static int TraverseTree(TreeNode<int> node)
        {
            // Recursive pattern: check if node exists
            if (node is null)
                return 0;
                
            // Recursively sum left and right subtrees
            // TreeNode<T>.Left = left child node
            // TreeNode<T>.Right = right child node
            return node.Value + TraverseTree(node.Left) + TraverseTree(node.Right);
        }

        /// <summary>
        /// Counts total nodes in binary tree
        /// </summary>
        public static int CountNodes(TreeNode<int> node)
        {
            if (node is null)
                return 0;
                
            return 1 + CountNodes(node.Left) + CountNodes(node.Right);
        }

        /// <summary>
        /// Finds maximum value in binary tree
        /// </summary>
        public static int FindMax(TreeNode<int> node)
        {
            if (node is null)
                return int.MinValue;
                
            // Math.Max = returns larger of two values
            int maxLeft = FindMax(node.Left);
            int maxRight = FindMax(node.Right);
            
            return Math.Max(node.Value, Math.Max(maxLeft, maxRight));
        }
    }

    // ── REAL-WORLD EXAMPLE: Event Classes ──────────────────────────────────
    /// <summary>
    /// User creation event
    /// </summary>
    public class UserCreatedEvent
    {
        public string Name { get; }
        public string Email { get; }
        
        public UserCreatedEvent(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }

    /// <summary>
    /// Order placement event
    /// </summary>
    public class OrderPlacedEvent
    {
        public string OrderId { get; }
        public decimal Amount { get; }
        
        public OrderPlacedEvent(string id, decimal amount)
        {
            OrderId = id;
            Amount = amount;
        }
    }

    /// <summary>
    /// Payment received event
    /// </summary>
    public class PaymentReceivedEvent
    {
        public string PaymentId { get; }
        public decimal Amount { get; }
        
        public PaymentReceivedEvent(string id, decimal amount)
        {
            PaymentId = id;
            Amount = amount;
        }
    }

    /// <summary>
    /// Email sent event
    /// </summary>
    public class EmailSentEvent
    {
        public string Subject { get; }
        public string Recipient { get; }
        
        public EmailSentEvent(string subject, string recipient)
        {
            Subject = subject;
            Recipient = recipient;
        }
    }

    // ── REAL-WORLD EXAMPLE: Message Classes ────────────────────────────────
    /// <summary>
    /// Text message type
    /// </summary>
    public class TextMessage
    {
        public string Sender { get; }
        public string Content { get; }
        
        public TextMessage(string sender, string content)
        {
            Sender = sender;
            Content = content;
        }
    }

    /// <summary>
    /// Image message type
    /// </summary>
    public class ImageMessage
    {
        public string Sender { get; }
        public string FileName { get; }
        public int SizeKB { get; }
        
        public ImageMessage(string sender, string fileName, int sizeKB)
        {
            Sender = sender;
            FileName = fileName;
            SizeKB = sizeKB;
        }
    }

    /// <summary>
    /// File message type
    /// </summary>
    public class FileMessage
    {
        public string Sender { get; }
        public string FileName { get; }
        public int SizeKB { get; }
        
        public FileMessage(string sender, string fileName, int sizeKB)
        {
            Sender = sender;
            FileName = fileName;
            SizeKB = sizeKB;
        }
    }

    /// <summary>
    /// System message type
    /// </summary>
    public class SystemMessage
    {
        public string EventType { get; }
        public string Data { get; }
        
        public SystemMessage(string eventType, string data)
        {
            EventType = eventType;
            Data = data;
        }
    }

    // ── REAL-WORLD EXAMPLE: Shape Classes ─────────────────────────────────
    /// <summary>
    /// Circle shape
    /// </summary>
    public class Circle
    {
        public double Radius { get; }
        public Circle(double radius) { Radius = radius; }
    }

    /// <summary>
    /// Rectangle shape
    /// </summary>
    public class Rectangle
    {
        public double Width { get; }
        public double Height { get; }
        public Rectangle(double w, double h) { Width = w; Height = h; }
    }

    /// <summary>
    /// Triangle shape
    /// </summary>
    public class Triangle
    {
        public double Base { get; }
        public double Height { get; }
        public Triangle(double b, double h) { Base = b; Height = h; }
    }

    /// <summary>
    /// Square shape
    /// </summary>
    public class Square
    {
        public double Side { get; }
        public Square(double side) { Side = side; }
    }

    // ── REAL-WORLD EXAMPLE: Tree Node ─────────────────────────────────────
    /// <summary>
    /// Generic binary tree node
    /// </summary>
    /// <typeparam name="T">Type of value stored</typeparam>
    public class TreeNode<T>
    {
        public T Value { get; }
        public TreeNode<T> Left { get; }
        public TreeNode<T> Right { get; }
        
        public TreeNode(T value, TreeNode<T> left, TreeNode<T> right)
        {
            Value = value;
            Left = left;
            Right = right;
        }
    }

    // ── REAL-WORLD EXAMPLE: Game Character Classes ───────────────────────
    /// <summary>
    /// Base character class
    /// </summary>
    public abstract class Character
    {
        public int Health { get; protected set; }
        public int AttackPower { get; protected set; }
    }

    /// <summary>
    /// Warrior character type
    /// </summary>
    public class Warrior : Character
    {
        public Warrior(int hp, int attack)
        {
            Health = hp;
            AttackPower = attack;
        }
    }

    /// <summary>
    /// Mage character type
    /// </summary>
    public class Mage : Character
    {
        public Mage(int hp, int attack)
        {
            Health = hp;
            AttackPower = attack;
        }
    }

    /// <summary>
    /// Archer character type
    /// </summary>
    public class Archer : Character
    {
        public Archer(int hp, int attack)
        {
            Health = hp;
            AttackPower = attack;
        }
    }

    /// <summary>
    /// Healer character type
    /// </summary>
    public class Healer : Character
    {
        public Healer(int hp, int attack)
        {
            Health = hp;
            AttackPower = attack;
        }
    }
}
