/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Control Flow - Real World Applications
 * FILE      : ControlFlow_RealWorld.cs
 * PURPOSE   : This file demonstrates practical, real-world uses of control flow statements
 *             in production applications.
 * ============================================================
 */

// --- SECTION: Real-World Control Flow ---
// This file demonstrates control flow in real production scenarios

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._01_Fundamentals._05_ControlFlow
{
    class ControlFlow_RealWorld
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: User Authentication Flow
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("=== Authentication ===");
            
            var loginResult = AuthenticateUser("admin", "password123", false);
            Console.WriteLine($"Login result: {loginResult}");
            
            loginResult = AuthenticateUser("user", "wrongpass", true);
            Console.WriteLine($"Login result: {loginResult}");
            
            loginResult = AuthenticateUser("locked", "pass", true);
            Console.WriteLine($"Login result: {loginResult}");
            
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Order Processing Pipeline
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Order Processing ===");
            
            ProcessOrder(100m, "Pending", "credit_card");
            ProcessOrder(50m, "Pending", "cash");
            ProcessOrder(1000m, "Pending", "credit_card");
            ProcessOrder(100m, "Cancelled", "credit_card");
            
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Business Rule Engine
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Business Rules ===");
            
            // Calculate shipping cost
            var shippingOptions = new[] 
            {
                (Weight: 5.0, Distance: 100, Express: false),
                (Weight: 25.0, Distance: 500, Express: true),
                (Weight: 0.5, Distance: 50, Express: false)
            };
            
            foreach (var option in shippingOptions)
            {
                decimal cost = CalculateShipping(option.Weight, option.Distance, option.Express);
                Console.WriteLine($"Weight: {option.Weight}kg, Distance: {option.Distance}mi, Express: {option.Express} => ${cost:F2}");
            }
            
            // Calculate discount
            var customers = new[]
            {
                (Level: "Gold", Years: 5, PurchaseAmount: 500m),
                (Level: "Silver", Years: 2, PurchaseAmount: 200m),
                (Level: "Bronze", Years: 0, PurchaseAmount: 50m),
                (Level: "Gold", Years: 10, PurchaseAmount: 1000m)
            };
            
            foreach (var customer in customers)
            {
                decimal discount = CalculateDiscount(customer.Level, customer.Years, customer.PurchaseAmount);
                Console.WriteLine($"{customer.Level} customer with ${customer.PurchaseAmount}: {discount:F1}% discount");
            }
            
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Input Validation
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Validation ===");
            
            ValidateUserInput("john@example.com", "password123", 25);
            ValidateUserInput("invalid", "short", 15);
            ValidateUserInput("", "password123", 30);
            ValidateUserInput("test@test.com", "", 20);
            
            // ═══════════════════════════════════════════════════════════════
            // SECTION: State Machine
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Order State Machine ===");
            
            var order = new OrderState { Status = "Created", Amount = 100m };
            ProcessOrderState(order);
            
            order.Status = "Approved";
            order.Amount = 500m;
            ProcessOrderState(order);
            
            order.Status = "Approved";
            order.Amount = 10000m;
            ProcessOrderState(order);
            
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Game Logic
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Game Logic ===");
            
            // Player action processing
            ProcessPlayerAction("attack", 50, 30, true);
            ProcessPlayerAction("defend", 50, 30, true);
            ProcessPlayerAction("heal", 10, 30, true);
            ProcessPlayerAction("use_potion", 100, 5, false);
            ProcessPlayerAction("flee", 50, 30, false);
            
            // ═══════════════════════════════════════════════════════════════
            // SECTION: API Response Handling
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== API Handling ===");
            
            int[] statusCodes = { 200, 201, 400, 401, 403, 404, 500, 503 };
            
            foreach (var code in statusCodes)
            {
                string action = HandleHttpStatus(code);
                Console.WriteLine($"Status {code}: {action}");
            }
            
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Data Processing
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Data Processing ===");
            
            var records = new[] 
            {
                (Id: 1, Value: 150, Type: "sale"),
                (Id: 2, Value: 50, Type: "refund"),
                (Id: 3, Value: 200, Type: "sale"),
                (Id: 4, Value: 75, Type: "discount")
            };
            
            decimal totalSales = 0;
            decimal totalRefunds = 0;
            
            foreach (var record in records)
            {
                if (record.Type == "sale")
                    totalSales += record.Value;
                else if (record.Type == "refund")
                    totalRefunds += record.Value;
                else
                    Console.WriteLine($"Skipping {record.Type} record");
            }
            
            Console.WriteLine($"Total Sales: ${totalSales}, Refunds: ${totalRefunds}");
            Console.WriteLine($"Net: ${totalSales - totalRefunds}");
        }
        
        // ═══════════════════════════════════════════════════════════════
        // Helper methods
        // ═══════════════════════════════════════════════════════════════
        
        static string AuthenticateUser(string username, string password, bool isLocked)
        {
            if (isLocked)
                return "Account locked";
            
            if (string.IsNullOrWhiteSpace(username))
                return "Username required";
            
            if (username == "admin" && password == "password123")
                return "Login successful";
            
            return "Invalid credentials";
        }
        
        static void ProcessOrder(decimal amount, string status, string paymentMethod)
        {
            // Process based on order status
            if (status == "Cancelled")
            {
                Console.WriteLine($"Order cancelled - no processing needed");
                return;
            }
            
            if (status != "Pending")
            {
                Console.WriteLine($"Unknown status: {status}");
                return;
            }
            
            // Check payment method
            if (paymentMethod == "credit_card")
            {
                decimal fee = amount * 0.029m + 0.30m;
                Console.WriteLine($"Processing credit card: ${amount} + ${fee:F2} fee = ${amount + fee:F2}");
            }
            else if (paymentMethod == "debit_card")
            {
                decimal fee = amount * 0.019m + 0.25m;
                Console.WriteLine($"Processing debit card: ${amount} + ${fee:F2} fee = ${amount + fee:F2}");
            }
            else
            {
                Console.WriteLine($"Processing {paymentMethod}: ${amount}");
            }
        }
        
        static decimal CalculateShipping(double weight, int distance, bool express)
        {
            decimal baseCost = (decimal)(weight * 0.50 + distance * 0.10);
            
            if (weight > 100)
                baseCost *= 1.5m;
            else if (weight > 50)
                baseCost *= 1.25m;
            
            if (express)
                baseCost *= 1.5m;
            
            return baseCost;
        }
        
        static decimal CalculateDiscount(string level, int years, decimal purchaseAmount)
        {
            decimal discount = level switch
            {
                "Gold" => 20m,
                "Silver" => 10m,
                "Bronze" => 5m,
                _ => 0m
            };
            
            // Add loyalty bonus
            if (years > 5)
                discount += 5m;
            else if (years > 2)
                discount += 2m;
            
            // Add purchase bonus
            if (purchaseAmount > 500)
                discount += 5m;
            else if (purchaseAmount > 100)
                discount += 2m;
            
            return discount;
        }
        
        static void ValidateUserInput(string email, string password, int age)
        {
            List<string> errors = new List<string>();
            
            if (string.IsNullOrWhiteSpace(email))
                errors.Add("Email is required");
            else if (!email.Contains("@") || !email.Contains("."))
                errors.Add("Invalid email format");
            
            if (string.IsNullOrWhiteSpace(password))
                errors.Add("Password is required");
            else if (password.Length < 8)
                errors.Add("Password must be at least 8 characters");
            
            if (age < 13)
                errors.Add("Must be at least 13 years old");
            else if (age > 120)
                errors.Add("Invalid age");
            
            if (errors.Count == 0)
                Console.WriteLine("Validation passed!");
            else
                Console.WriteLine($"Errors: {string.Join(", ", errors)}");
        }
        
        static void ProcessOrderState(OrderState order)
        {
            string action = order.Status switch
            {
                "Created" => "Order created - awaiting approval",
                "Approved" => order.Amount > 1000m 
                    ? "High value - manual review required" 
                    : "Approved - processing payment",
                "Processing" => "Payment being processed",
                "Shipped" => "Order shipped to customer",
                "Delivered" => "Order delivered - complete",
                "Cancelled" => "Order cancelled",
                _ => "Unknown state"
            };
            
            Console.WriteLine($"Status: {order.Status}, Action: {action}");
        }
        
        static void ProcessPlayerAction(string action, int playerHealth, int playerMana, bool hasPotion)
        {
            if (action == "attack")
            {
                if (playerMana < 10)
                    Console.WriteLine("Not enough mana for attack");
                else
                    Console.WriteLine("Attack performed!");
            }
            else if (action == "defend")
            {
                Console.WriteLine("Defensive stance taken");
            }
            else if (action == "heal")
            {
                if (playerMana < 20)
                    Console.WriteLine("Not enough mana for heal");
                else
                    Console.WriteLine("Healed 30 HP!");
            }
            else if (action == "use_potion")
            {
                if (hasPotion)
                    Console.WriteLine("Potion used - health restored");
                else
                    Console.WriteLine("No potion available!");
            }
            else if (action == "flee")
            {
                Console.WriteLine("Ran away from battle!");
            }
            else
            {
                Console.WriteLine("Unknown action");
            }
        }
        
        static string HandleHttpStatus(int statusCode) => statusCode switch
        {
            200 => "OK - Process response",
            201 => "Created - Resource created",
            204 => "No Content - Success with no response body",
            400 => "Bad Request - Validate input",
            401 => "Unauthorized - Redirect to login",
            403 => "Forbidden - No permission",
            404 => "Not Found - Resource not found",
            409 => "Conflict - Handle merge conflict",
            422 => "Unprocessable - Validate business rules",
            429 => "Too Many Requests - Rate limit",
            >= 500 and < 600 => "Server Error - Log and alert",
            _ => "Unknown status code"
        };
        
        // Helper class
        class OrderState
        {
            public string Status { get; set; } = "";
            public decimal Amount { get; set; }
        }
    }
}
