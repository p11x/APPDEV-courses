/*
 * ============================================================
 * TOPIC     : Exception Handling
 * SUBTOPIC  : Custom Exceptions - Real-World Examples
 * FILE      : CustomExceptions_RealWorld.cs
 * PURPOSE   : Apply custom exceptions to real-world scenarios
 *            including domain exceptions and validation exceptions
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._05_ExceptionHandling._02_CustomExceptions
{
    class CustomExceptions_RealWorld
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Custom Exceptions - Real World ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Domain Exceptions - Business Rules
            // ═══════════════════════════════════════════════════════════

            // Domain exceptions represent business rule violations
            // These are meaningful to business logic

            // ── EXAMPLE 1: Banking Domain ────────────────────────────────
            var account = new BankAccount("ACC-001", 1000);
            
            // Successful withdrawal
            try
            {
                account.Withdraw(500);
                Console.WriteLine($"  Withdraw $500: Success, Balance: ${account.Balance}");
            }
            catch (InsufficientFundsException ex)
            {
                Console.WriteLine($"  Withdraw $500: {ex.Message}");
            }
            // Output: Withdraw $500: Success, Balance: $500

            // Failed withdrawal - insufficient funds
            try
            {
                account.Withdraw(1000);
            }
            catch (InsufficientFundsException ex)
            {
                Console.WriteLine($"  Withdraw $1000: {ex.GetType().Name}");
                Console.WriteLine($"  Available: ${ex.AvailableBalance}, Requested: ${ex.RequestedAmount}");
            }
            // Output: Withdraw $1000: InsufficientFundsException
            // Output: Available: $500, Requested: $1000

            // ── EXAMPLE 2: E-Commerce Domain ────────────────────────
            var store = new OnlineStore();
            var cart = new ShoppingCart();

            cart.AddItem(new Product("Laptop", 999.99m), 1);
            
            try
            {
                store.Checkout(cart, new PaymentInfo("EXPired"));
            }
            catch (PaymentFailedException ex)
            {
                Console.WriteLine($"\n  Checkout: {ex.GetType().Name}");
                Console.WriteLine($"  Reason: {ex.FailureReason}");
            }
            // Output: Checkout: PaymentFailedException
            // Output: Reason: Card expired

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Validation Exceptions - Input Validation
            // ═══════════════════════════════════════════════════════════

            // Validation exceptions handle invalid input
            // Provide clear error messages for users

            // ── EXAMPLE 1: User Registration Validation ────────────────
            var userService = new UserService();

            try
            {
                userService.RegisterUser("", "john@email.com", 25);
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"\n  Registration (empty name):");
                Console.WriteLine($"  Errors: {string.Join(", ", ex.Errors)}");
            }
            // Output: Registration (empty name):
            // Output: Errors: Name is required, Email is required

            try
            {
                userService.RegisterUser("John", "invalid-email", 25);
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"  Registration (bad email):");
                Console.WriteLine($"  Errors: {string.Join(", ", ex.Errors)}");
            }
            // Output: Registration (bad email):
            // Output: Errors: Invalid email format

            try
            {
                userService.RegisterUser("John", "john@email.com", -5);
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"  Registration (bad age):");
                Console.WriteLine($"  Errors: {string.Join(", ", ex.Errors)}");
            }
            // Output: Registration (bad age):
            // Output: Errors: Age must be between 0 and 150

            // Successful registration
            try
            {
                var user = userService.RegisterUser("John", "john@email.com", 25);
                Console.WriteLine($"  Registration (valid): User created - {user.Name}");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"  Registration: {ex.Errors[0]}");
            }
            // Output: Registration (valid): User created - John

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Domain-Driven Design Exceptions
            // ═══════════════════════════════════════════════════════════

            // DDD-style exceptions represent domain events
            // They are specific to your business domain

            // ── EXAMPLE 1: Inventory Domain ─────────────────────────────
            var warehouse = new Warehouse();
            
            try
            {
                warehouse.ReserveInventory("SKU-001", 50);
            }
            catch (InsufficientInventoryException ex)
            {
                Console.WriteLine($"\n  Reserve (over stock): {ex.GetType().Name}");
                Console.WriteLine($"  Requested: {ex.RequestedQty}, Available: {ex.AvailableQty}");
            }
            // Output: Reserve (over stock): InsufficientInventoryException
            // Output: Requested: 50, Available: 30

            warehouse.ReserveInventory("SKU-001", 20);
            Console.WriteLine($"  Reserve (valid): Reserved {20} units");
            // Output: Reserve (valid): Reserved 20 units

            // ═══════════════════════════════════════════════════════��═══
            // SECTION 4: API Exception Handling
            // ═══════════════════════════════════════════════════════════

            // API exceptions should return proper status codes or error info

            // ── EXAMPLE 1: REST API Exceptions ─────────────────────────────
            var api = new UserApi();

            var response1 = api.GetUser(1);
            Console.WriteLine($"\n  GET /users/1: {response1.StatusCode}");
            // Output: GET /users/1: 200 OK

            var response2 = api.GetUser(999);
            Console.WriteLine($"  GET /users/999: {response2.StatusCode}");
            // Output: GET /users/999: 404 NotFound
            // Output: Error: User not found

            var response3 = api.CreateUser(new { name = "" });
            Console.WriteLine($"  POST /users: {response3.StatusCode}");
            // Output: POST /users: 400 BadRequest
            // Output: Error: Validation failed

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Exception Handling in Layers
            // ═══════════════════════════════════════════════════════════

            // Different layers handle exceptions differently
            // UI/Web: Convert to user-friendly messages
            // Business: Log and transform if needed
            // Data: Handle storage errors

            // ── EXAMPLE 1: Layered Exception Handling ──────────────────
            var orderService = new OrderService();

            try
            {
                orderService.CreateOrder(null);
            }
            catch (DomainException ex)
            {
                Console.WriteLine($"\n  Layer 1 - Business: {ex.GetType().Name}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Layer 2 - Caught outer: {ex.GetType().Name}");
            }
            // Output: Layer 1 - Business: OrderValidationException
            // Output: Layer 2 - Caught outer: OrderValidationException

            Console.WriteLine("\n=== Custom Exceptions Real World Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    // DOMAIN EXCEPTIONS - BANKING
    // ═══════════════════════════════════════════════════════════

    class InsufficientFundsException : DomainException
    {
        public decimal AvailableBalance { get; }
        public decimal RequestedAmount { get; }

        public InsufficientFundsException(decimal available, decimal requested)
            : base($"Insufficient funds. Available: ${available:F2}, Requested: ${requested:F2}")
        {
            AvailableBalance = available;
            RequestedAmount = requested;
        }
    }

    class BankAccount
    {
        public string AccountId { get; }
        public decimal Balance { get; private set; }

        public BankAccount(string accountId, decimal initialBalance)
        {
            AccountId = accountId;
            Balance = initialBalance;
        }

        public void Withdraw(decimal amount)
        {
            if (amount > Balance)
            {
                throw new InsufficientFundsException(Balance, amount);
            }
            Balance -= amount;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // DOMAIN EXCEPTIONS - E-COMMERCE
    // ═══════════════════════════════════════════════════════════

    class PaymentFailedException : DomainException
    {
        public string FailureReason { get; }

        public PaymentFailedException(string reason)
            : base($"Payment failed: {reason}")
        {
            FailureReason = reason;
        }
    }

    class PaymentInfo
    {
        public string CardNumber { get; }

        public PaymentInfo(string cardNumber)
        {
            CardNumber = cardNumber;
        }
    }

    class Product
    {
        public string Name { get; }
        public decimal Price { get; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }

    class ShoppingCart
    {
        private readonly List<(Product product, int quantity)> _items = new();

        public void AddItem(Product product, int quantity)
        {
            _items.Add((product, quantity));
        }

        public IReadOnlyList<(Product product, int quantity)> Items => _items;
    }

    class OnlineStore
    {
        public void Checkout(ShoppingCart cart, PaymentInfo payment)
        {
            if (payment.CardNumber == "EXPired")
            {
                throw new PaymentFailedException("Card expired");
            }
            if (payment.CardNumber == "declined")
            {
                throw new PaymentFailedException("Card declined");
            }
            Console.WriteLine($"  Processing {cart.Items.Count} items...");
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    // VALIDATION EXCEPTIONS
    // ═══════════════════════════════════════════════════════════

    class ValidationException : DomainException
    {
        public List<string> Errors { get; } = new();

        public ValidationException() : base("Validation failed") { }

        public ValidationException(string message) : base(message) { }
    }

    class UserService
    {
        public User RegisterUser(string name, string email, int age)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(name))
            {
                errors.Add("Name is required");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add("Email is required");
            }
            else if (!email.Contains("@"))
            {
                errors.Add("Invalid email format");
            }

            if (age < 0 || age > 150)
            {
                errors.Add("Age must be between 0 and 150");
            }

            if (errors.Count > 0)
            {
                var ex = new ValidationException();
                ex.Errors.AddRange(errors);
                throw ex;
            }

            return new User(name, email, age);
        }
    }

    class User
    {
        public string Name { get; }
        public string Email { get; }
        public int Age { get; }

        public User(string name, string email, int age)
        {
            Name = name;
            Email = email;
            Age = age;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // DOMAIN EXCEPTIONS - INVENTORY
    // ═══════════════════════════════════════════════════════════

    class InsufficientInventoryException : DomainException
    {
        public int RequestedQty { get; }
        public int AvailableQty { get; }

        public InsufficientInventoryException(int requested, int available)
            : base($"Insufficient inventory. Requested: {requested}, Available: {available}")
        {
            RequestedQty = requested;
            AvailableQty = available;
        }
    }

    class Warehouse
    {
        private int _stockQty = 30;

        public void ReserveInventory(string sku, int qty)
        {
            if (qty > _stockQty)
            {
                throw new InsufficientInventoryException(qty, _stockQty);
            }
            _stockQty -= qty;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // BASE DOMAIN EXCEPTION
    // ═══════════════════════════════════════════════════════════

    class DomainException : Exception
    {
        public DomainException() : base() { }
        public DomainException(string message) : base(message) { }
        public DomainException(string message, Exception inner) : base(message, inner) { }
    }

    // ════════════════════════════════════════════════��═��════════════════
    // API EXCEPTIONS
    // ═══════════════════════════════════════════════════════════

    class ApiException : Exception
    {
        public int StatusCode { get; }

        public ApiException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }

    class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Body { get; set; }
    }

    class UserApi
    {
        private readonly Dictionary<int, User> _users = new()
        {
            { 1, new User("John", "john@email.com", 25) }
        };

        public ApiResponse GetUser(int id)
        {
            if (!_users.ContainsKey(id))
            {
                throw new ApiException(404, "User not found");
            }
            return new ApiResponse { StatusCode = 200, Body = $"{{\"id\": {id}, \"name\": \"{_users[id].Name}\"}}" };
        }

        public ApiResponse CreateUser(object userData)
        {
            throw new ApiException(400, "Validation failed");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SERVICE LAYER EXCEPTIONS
    // ═══════════════════════════════════════════════════════════

    class OrderValidationException : DomainException
    {
        public OrderValidationException(string message) : base(message) { }
    }

    class OrderService
    {
        public void CreateOrder(ShoppingCart cart)
        {
            if (cart == null || cart.Items.Count == 0)
            {
                throw new OrderValidationException("Order must have at least one item");
            }
        }
    }
}