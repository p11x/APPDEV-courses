/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Liskov Substitution Principle - Real-World
 * FILE      : 03_LSP_RealWorld.cs
 * PURPOSE   : Demonstrates real-world LSP applications including
 *             payment processing, shape hierarchy, and
 *             database repositories
 * ============================================================
 */

using System; // Core System namespace for Console
using System.Collections.Generic; // Generic collections

namespace CSharp_MasterGuide._12_SOLID_Principles._03_LiskovSubstitution._03_LSP_RealWorld
{
    /// <summary>
    /// Demonstrates real-world Liskov Substitution Principle
    /// </summary>
    public class LSPRealWorldDemo
    {
        /// <summary>
        /// Entry point for LSP real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Payment Processing with LSP
            // ═══════════════════════════════════════════════════════════
            // Subtypes must be substitutable for base type
            // All payment methods work through common interface

            Console.WriteLine("=== LSP Real-World ===\n");

            // Output: --- Payment Processing ---
            Console.WriteLine("--- Payment Processing ---");

            // Process all payments through base class reference
            var payments = new List<PaymentBase>
            {
                new CreditCard("4111111111111111", 100m),
                new PayPal("user@example.com", 50m),
                new BankTransfer("ACC123456", 200m)
            };

            // Each payment can be processed identically
            foreach (var payment in payments)
            {
                var result = payment.Process();
                // Output: Credit Card ***1111: $100.00
                // Output: PayPal user@example.com: $50.00
                // Output: Bank ACC123456: $200.00
                Console.WriteLine($"  {result}");
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Shape Hierarchy with LSP
            // ═══════════════════════════════════════════════════════════
            // All shapes can be used through Shape base class
            // Area calculation works uniformly for all shapes

            // Output: --- Shape Hierarchy ---
            Console.WriteLine("\n--- Shape Hierarchy ---");

            var shapes = new List<Shape>
            {
                new RectangleArea(5, 3),
                new SquareArea(4),
                new CircleArea(2)
            };

            // Each shape calculates area correctly
            foreach (var shape in shapes)
            {
                var area = shape.GetArea();
                // Output: Rectangle: 15.00
                // Output: Square: 16.00
                // Output: Circle: 12.57
                Console.WriteLine($"  {shape.GetType().Name}: {area:F2}");
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Repository Pattern with LSP
            // ═══════════════════════════════════════════════════════════
            // All repositories implement common interface
            // Can switch implementations without changing client code

            // Output: --- Repository Pattern ---
            Console.WriteLine("\n--- Repository Pattern ---");

            // Use in-memory repository
            IRepository<User> userRepo = new InMemoryRepository<User>();
            userRepo.Add(new User { Id = 1, Name = "Alice" });
            userRepo.Add(new User { Id = 2, Name = "Bob" });
            // Output: Added: Alice
            // Output: Added: Bob

            var users = userRepo.GetAll();
            // Output: Total users: 2
            Console.WriteLine($"  Total users: {users.Count}");

            // Switch to dictionary repository (same interface)
            userRepo = new DictionaryRepository<User>();
            userRepo.Add(new User { Id = 1, Name = "Charlie" });
            userRepo.Add(new User { Id = 2, Name = "Diana" });
            // Output: Added: Charlie
            // Output: Added: Diana

            users = userRepo.GetAll();
            // Output: Total users: 2
            Console.WriteLine($"  Total users: {users.Count}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Notification Service with LSP
            // ═══════════════════════════════════════════════════════════
            // All notifiers can be used interchangeably

            // Output: --- Notification Service ---
            Console.WriteLine("\n--- Notification Service ---");

            var notifications = new List<INotify>
            {
                new EmailNotify("user@example.com"),
                new SmsNotify("+1234567890"),
                new PushNotify("device123")
            };

            foreach (var notify in notifications)
            {
                notify.Send("Hello!");
                // Output: Email sent to user@example.com: Hello!
                // Output: SMS sent to +1234567890: Hello!
                // Output: Push sent to device123: Hello!
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Employee Hierarchy with LSP
            // ═══════════════════════════════════════════════════════════
            // All employee types calculate pay correctly
            // Base class defines contract, subclasses fulfill it

            // Output: --- Employee Hierarchy ---
            Console.WriteLine("\n--- Employee Hierarchy ---");

            var employees = new List<Employee>
            {
                new FullTime("Alice", 60000m),
                new PartTime("Bob", 20m, 40),
                new Contractor("Charlie", 50m, 80)
            };

            decimal totalPay = 0;
            foreach (var emp in employees)
            {
                var pay = emp.GetPay();
                totalPay += pay;
                // Output: Alice: $5000.00/month
                // Output: Bob: $3200.00/month
                // Output: Charlie: $4000.00/month
                Console.WriteLine($"  {emp.Name}: ${pay:F2}/month");
            }

            // Output: Total: $12200.00/month
            Console.WriteLine($"  Total: ${totalPay:F2}/month");

            Console.WriteLine("\n=== LSP Real-World Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 1: Payment Processing Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Base payment class - defines contract for all payment types
    /// </summary>
    public abstract class PaymentBase
    {
        /// <summary>
        /// Processes payment and returns result string
        /// </summary>
        public abstract string Process();
    }

    /// <summary>
    /// Credit card payment - substitutable for PaymentBase
    /// </summary>
    public class CreditCard : PaymentBase
    {
        private readonly string _cardNumber;
        private readonly decimal _amount;

        public CreditCard(string cardNumber, decimal amount)
        {
            _cardNumber = cardNumber;
            _amount = amount;
        }

        public override string Process()
        {
            var last4 = _cardNumber.Substring(_cardNumber.Length - 4);
            return $"Credit Card ***{last4}: ${_amount:F2}";
        }
    }

    /// <summary>
    /// PayPal payment - substitutable for PaymentBase
    /// </summary>
    public class PayPal : PaymentBase
    {
        private readonly string _email;
        private readonly decimal _amount;

        public PayPal(string email, decimal amount)
        {
            _email = email;
            _amount = amount;
        }

        public override string Process()
        {
            return $"PayPal {_email}: ${_amount:F2}";
        }
    }

    /// <summary>
    /// Bank transfer - substitutable for PaymentBase
    /// </summary>
    public class BankTransfer : PaymentBase
    {
        private readonly string _accountNumber;
        private readonly decimal _amount;

        public BankTransfer(string accountNumber, decimal amount)
        {
            _accountNumber = accountNumber;
            _amount = amount;
        }

        public override string Process()
        {
            return $"Bank {_accountNumber}: ${_amount:F2}";
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 2: Shape Hierarchy Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Base shape class - contract for area calculation
    /// </summary>
    public abstract class Shape
    {
        /// <summary>
        /// Calculates area of shape
        /// </summary>
        public abstract double GetArea();
    }

    /// <summary>
    /// Rectangle - substitutable for Shape
    /// </summary>
    public class RectangleArea : Shape
    {
        private readonly double _width;
        private readonly double _height;

        public RectangleArea(double width, double height)
        {
            _width = width;
            _height = height;
        }

        public override double GetArea() => _width * _height;
    }

    /// <summary>
    /// Square - substitutable for Shape
    /// Square is a Rectangle, but LSP requires proper substitution
    /// </summary>
    public class SquareArea : Shape
    {
        private readonly double _side;

        public SquareArea(double side)
        {
            _side = side;
        }

        public override double GetArea() => _side * _side;
    }

    /// <summary>
    /// Circle - substitutable for Shape
    /// </summary>
    public class CircleArea : Shape
    {
        private readonly double _radius;

        public CircleArea(double radius)
        {
            _radius = radius;
        }

        public override double GetArea() => Math.PI * _radius * _radius;
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 3: Repository Pattern Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Generic repository interface
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        List<T> GetAll();
    }

    /// <summary>
    /// In-memory repository - substitutable for IRepository<T>
    /// </summary>
    public class InMemoryRepository<T> : IRepository<T> where T : class
    {
        private readonly List<T> _items = new();

        public void Add(T entity)
        {
            _items.Add(entity);
            Console.WriteLine($"   Added: {entity.GetType().Name}");
        }

        public List<T> GetAll() => new List<T>(_items);
    }

    /// <summary>
    /// Dictionary repository - substitutable for IRepository<T>
    /// </summary>
    public class DictionaryRepository<T> : IRepository<T> where T : class
    {
        private readonly Dictionary<int, T> _items = new();

        public void Add(T entity)
        {
            var id = _items.Count + 1;
            _items[id] = entity;
            Console.WriteLine($"   Added: {entity.GetType().Name}");
        }

        public List<T> GetAll() => new List<T>(_items.Values);
    }

    /// <summary>
    /// User entity for repository demo
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 4: Notification Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Notification interface
    /// </summary>
    public interface INotify
    {
        /// <summary>
        /// Sends notification message
        /// </summary>
        void Send(string message);
    }

    /// <summary>
    /// Email notification - substitutable for INotify
    /// </summary>
    public class EmailNotify : INotify
    {
        private readonly string _email;

        public EmailNotify(string email) => _email = email;

        public void Send(string message)
        {
            Console.WriteLine($"   Email sent to {_email}: {message}");
        }
    }

    /// <summary>
    /// SMS notification - substitutable for INotify
    /// </summary>
    public class SmsNotify : INotify
    {
        private readonly string _phone;

        public SmsNotify(string phone) => _phone = phone;

        public void Send(string message)
        {
            Console.WriteLine($"   SMS sent to {_phone}: {message}");
        }
    }

    /// <summary>
    /// Push notification - substitutable for INotify
    /// </summary>
    public class PushNotify : INotify
    {
        private readonly string _deviceId;

        public PushNotify(string deviceId) => _deviceId = deviceId;

        public void Send(string message)
        {
            Console.WriteLine($"   Push sent to {_deviceId}: {message}");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 5: Employee Hierarchy Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Base employee class - defines pay calculation contract
    /// </summary>
    public abstract class Employee
    {
        public string Name { get; set; }

        protected Employee(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Calculates monthly pay - must be implementable by all subtypes
        /// </summary>
        public abstract decimal GetPay();
    }

    /// <summary>
    /// Full-time employee - substitutable for Employee
    /// </summary>
    public class FullTime : Employee
    {
        private readonly decimal _annualSalary;

        public FullTime(string name, decimal annualSalary) : base(name)
        {
            _annualSalary = annualSalary;
        }

        public override decimal GetPay() => _annualSalary / 12;
    }

    /// <summary>
    /// Part-time employee - substitutable for Employee
    /// </summary>
    public class PartTime : Employee
    {
        private readonly decimal _hourlyRate;
        private readonly int _hoursPerWeek;

        public PartTime(string name, decimal hourlyRate, int hoursPerWeek) : base(name)
        {
            _hourlyRate = hourlyRate;
            _hoursPerWeek = hoursPerWeek;
        }

        public override decimal GetPay() => _hourlyRate * _hoursPerWeek * 4;
    }

    /// <summary>
    /// Contractor - substitutable for Employee
    /// </summary>
    public class Contractor : Employee
    {
        private readonly decimal _hourlyRate;
        private readonly int _hoursBilled;

        public Contractor(string name, decimal hourlyRate, int hoursBilled) : base(name)
        {
            _hourlyRate = hourlyRate;
            _hoursBilled = hoursBilled;
        }

        public override decimal GetPay() => _hourlyRate * _hoursBilled;
    }
}
