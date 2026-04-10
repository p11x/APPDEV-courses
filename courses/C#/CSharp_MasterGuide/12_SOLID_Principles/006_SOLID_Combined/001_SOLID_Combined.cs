/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : SOLID Combined - Putting It All Together
 * FILE      : 01_SOLID_Combined.cs
 * PURPOSE   : Demonstrates all five SOLID principles working together
 * ============================================================
 */
using System; // Core System namespace for Console
using System.Collections.Generic; // Generic collections

namespace CSharp_MasterGuide._12_SOLID_Principles._06_SOLID_Combined._01_SOLID_Combined
{
    /// <summary>
    /// Demonstrates combined SOLID principles
    /// </summary>
    public class SOLIDCombinedDemo
    {
        /// <summary>
        /// Entry point for SOLID combined examples
        /// </summary>
        public static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Single Responsibility in Practice
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== SOLID Combined ===\n");

            // Output: --- S: Single Responsibility ---
            Console.WriteLine("--- S: Single Responsibility ---");

            // Each class has one responsibility
            var orderRepository = new OrderRepository();
            var orderValidator = new OrderValidator();
            var orderNotifier = new OrderNotifier();

            var orderService = new OrderService(
                orderRepository, orderValidator, orderNotifier);

            var valid = orderService.PlaceOrder("Product", 2);
            Console.WriteLine($"   Order valid: {valid}");
            // Output: Order valid: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Open-Closed in Practice
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- O: Open-Closed ---
            Console.WriteLine("\n--- O: Open-Closed ---");

            // Add new payment types without modification
            var payments = new List<IPaymentMethod>
            {
                new CreditCardPayment(),
                new PayPalPayment(),
                new BankPayment()
            };

            var checkout = new CheckoutService();
            foreach (var payment in payments)
            {
                checkout.Process(payment, 100m);
            }
            // Output: Credit card: $100.00
            // Output: PayPal: $100.00
            // Output: Bank: $100.00

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Liskov Substitution in Practice
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- L: Liskov Substitution ---
            Console.WriteLine("\n--- L: Liskov Substitution ---");

            // Works with any shape
            var shapes = new List<IShape>
            {
                new Rectangle { Width = 4, Height = 5 },
                new Square2 { Side = 5 }
            };

            var areaCalc = new AreaCalculator();
            foreach (var shape in shapes)
            {
                Console.WriteLine($"   {shape.GetType().Name}: {areaCalc.Calculate(shape)}");
            }
            // Output: Rectangle2: 20
            // Output: Square2: 25

            // ═══════════════════════════════════════════════════════════════════
            // SECTION 4: Interface Segregation in Practice
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- I: Interface Segregation ---
            Console.WriteLine("\n--- I: Interface Segregation ---");

            // Use small, focused interfaces
            var printer = new BasicPrinter();
            var scanner = new BasicScanner();

            if (printer is IPrintable p) p.Print();
            if (scanner is IScannable s) s.Scan();
            // Output: Printing
            // Output: Scanning

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Dependency Inversion in Practice
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- D: Dependency Inversion ---
            Console.WriteLine("\n--- D: Dependency Inversion ---");

            // Depend on abstractions, not concretions
            IUserRepository userRepo = new InMemoryUserRepository();
            var userService = new UserService(userRepo);

            userService.Register("Alice");
            // Output: User Alice registered

            // Swap implementation
            userRepo = new SqlUserRepository();
            userService = new UserService(userRepo);
            userService.Register("Bob");
            // Output: User Bob registered

            Console.WriteLine("\n=== SOLID Combined Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 1: SRP Implementations
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Order validator - single responsibility
    /// </summary>
    public class OrderValidator
    {
        public bool Validate(string product, int quantity)
        {
            return !string.IsNullOrEmpty(product) && quantity > 0;
        }
    }

    /// <summary>
    /// Order repository - single responsibility
    /// </summary>
    public class OrderRepository
    {
        public void Save(string order) => Console.WriteLine("   Order saved");
    }

    /// <summary>
    /// Order notifier - single responsibility
    /// </summary>
    public class OrderNotifier
    {
        public void Notify(string order) => Console.WriteLine("   Notification sent");
    }

    /// <summary>
    /// Order service - orchestrates single-responsibility components
    /// </summary>
    public class OrderService
    {
        private readonly OrderRepository _repository; // field: repository
        private readonly OrderValidator _validator; // field: validator
        private readonly OrderNotifier _notifier; // field: notifier

        public OrderService(OrderRepository repository, OrderValidator validator, OrderNotifier notifier)
        {
            _repository = repository;
            _validator = validator;
            _notifier = notifier;
        }

        public bool PlaceOrder(string product, int quantity)
        {
            if (!_validator.Validate(product, quantity)) return false;
            _repository.Save(product);
            _notifier.Notify(product);
            return true;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 2: OCP Implementations
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Payment method abstraction - open for extension
    /// </summary>
    public interface IPaymentMethod
    {
        void Process(decimal amount); // method: process payment
    }

    /// <summary>
    /// Payment method implementations - closed for modification
    /// </summary>
    public class CreditCardPayment : IPaymentMethod
    {
        public void Process(decimal amount) => Console.WriteLine($"   Credit card: {amount:C}");
    }

    public class PayPalPayment : IPaymentMethod
    {
        public void Process(decimal amount) => Console.WriteLine($"   PayPal: {amount:C}");
    }

    public class BankPayment : IPaymentMethod
    {
        public void Process(decimal amount) => Console.WriteLine($"   Bank: {amount:C}");
    }

    /// <summary>
    /// Checkout service - closed for modification
    /// </summary>
    public class CheckoutService
    {
        public void Process(IPaymentMethod payment, decimal amount)
        {
            payment.Process(amount);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 3: LSP Implementations
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Shape abstraction - substitutable
    /// </summary>
    public interface IShape
    {
        int Area { get; } // property: area
    }

    public class Rectangle2 : IShape
    {
        public int Width { get; set; } // property: width
        public int Height { get; set; } // property: height
        public int Area => Width * Height; // property: area
    }

    public class Square2 : IShape
    {
        public int Side { get; set; } // property: side
        public int Area => Side * Side; // property: area
    }

    public class AreaCalculator
    {
        public int Calculate(IShape shape) => shape.Area;
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 4: ISP Implementations
    // ═══════════════════════════════════════════════════════════

    public interface IPrintable
    {
        void Print(); // method: print
    }

    public interface IScannable
    {
        void Scan(); // method: scan
    }

    public class BasicPrinter : IPrintable
    {
        public void Print() => Console.WriteLine("   Printing");
    }

    public class BasicScanner : IScannable
    {
        public void Scan() => Console.WriteLine("   Scanning");
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 5: DIP Implementations
    // ═══════════════════════════════════════════════════════════

    public interface IUserRepository
    {
        void Add(string user); // method: add user
    }

    public class SqlUserRepository : IUserRepository
    {
        public void Add(string user) => Console.WriteLine($"   Database: User {user}");
    }

    public class InMemoryUserRepository : IUserRepository
    {
        public void Add(string user) => Console.WriteLine($"   Memory: User {user}");
    }

    public class UserService
    {
        private readonly IUserRepository _repository; // field: abstraction

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public void Register(string user)
        {
            _repository.Add(user);
            Console.WriteLine($"   User {user} registered");
        }
    }
}