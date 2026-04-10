/*
 * TOPIC: CSharp_MasterGuide/03_Advanced_OOP/03_LambdaExpressions
 * SUBTOPIC: Real-World Lambda Expressions
 * FILE: Lambda_RealWorld.cs
 * PURPOSE: Practical applications: LINQ queries, event handlers, predicates in real scenarios
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._03_LambdaExpressions
{
    public class RWCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public decimal TotalPurchases { get; set; }
        public DateTime LastPurchaseDate { get; set; }
        public string Tier { get; set; }
    }

    public class RWOrder
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class LambdaRealWorld
    {
        public static void RunMain(string[] args)
        {
            Console.WriteLine("=== Real-World Lambda Expressions ===\n");

            // Sample data
            var customers = new List<RWCustomer>
            {
                new RWCustomer { Id = 1, Name = "Alice", Email = "alice@email.com", TotalPurchases = 5000, LastPurchaseDate = DateTime.Now.AddDays(-5), Tier = "Gold" },
                new RWCustomer { Id = 2, Name = "Bob", Email = "bob@email.com", TotalPurchases = 1500, LastPurchaseDate = DateTime.Now.AddDays(-30), Tier = "Silver" },
                new RWCustomer { Id = 3, Name = "Charlie", Email = "charlie@email.com", TotalPurchases = 8000, LastPurchaseDate = DateTime.Now.AddDays(-2), Tier = "Platinum" },
                new RWCustomer { Id = 4, Name = "Diana", Email = "diana@email.com", TotalPurchases = 500, LastPurchaseDate = DateTime.Now.AddDays(-60), Tier = "Bronze" },
                new RWCustomer { Id = 5, Name = "Eve", Email = "eve@email.com", TotalPurchases = 3000, LastPurchaseDate = DateTime.Now.AddDays(-10), Tier = "Gold" }
            };

            var orders = new List<RWOrder>
            {
                new RWOrder { OrderId = 1, CustomerId = 1, Amount = 100, Status = "Completed", CreatedDate = DateTime.Now.AddDays(-5) },
                new RWOrder { OrderId = 2, CustomerId = 1, Amount = 200, Status = "Pending", CreatedDate = DateTime.Now.AddDays(-2) },
                new RWOrder { OrderId = 3, CustomerId = 2, Amount = 50, Status = "Completed", CreatedDate = DateTime.Now.AddDays(-30) },
                new RWOrder { OrderId = 4, CustomerId = 3, Amount = 500, Status = "Completed", CreatedDate = DateTime.Now.AddDays(-2) },
                new RWOrder { OrderId = 5, CustomerId = 4, Amount = 25, Status = "Cancelled", CreatedDate = DateTime.Now.AddDays(-60) }
            };

            // ============================================
            // LINQ QUERIES WITH LAMBDA
            // ============================================

            // Example 1: Filter with Where + lambda
            var goldCustomers = customers.Where(c => c.Tier == "Gold").ToList();
            Console.WriteLine($"Gold customers: {string.Join(", ", goldCustomers.Select(c => c.Name))}");
            // Output: Alice, Eve

            // Example 2: Transform with Select + lambda
            var customerSummary = customers.Select(c => $"{c.Name} ({c.Tier}): ${c.TotalPurchases}").ToList();
            Console.WriteLine($"\nCustomer summary:");
            foreach (var summary in customerSummary)
            {
                Console.WriteLine(summary);
            }

            // Example 3: Complex filtering with multiple conditions
            var vipCustomers = customers
                .Where(c => c.Tier == "Platinum" || c.TotalPurchases > 4000)
                .OrderByDescending(c => c.TotalPurchases)
                .ToList();
            Console.WriteLine($"\nVIP customers: {string.Join(", ", vipCustomers.Select(c => c.Name))}");
            // Output: Charlie, Alice, Eve

            // Example 4: GroupBy with lambda
            var customersByTier = customers.GroupBy(c => c.Tier)
                .Select(g => $"{g.Key}: {g.Count()} customers, total: ${g.Sum(c => c.TotalPurchases)}")
                .ToList();
            Console.WriteLine($"\nCustomers by tier:");
            foreach (var line in customersByTier)
            {
                Console.WriteLine(line);
            }

            // Example 5: Any and All with lambda
            bool hasPlatinum = customers.Any(c => c.Tier == "Platinum");
            bool allActive = customers.All(c => c.TotalPurchases > 0);
            Console.WriteLine($"\nHas Platinum: {hasPlatinum}"); // Output: True
            Console.WriteLine($"All active: {allActive}"); // Output: True

            // Example 6: First/FirstOrDefault with lambda
            var topSpender = customers.FirstOrDefault(c => c.TotalPurchases == customers.Max(x => x.TotalPurchases));
            Console.WriteLine($"\nTop spender: {topSpender?.Name}"); // Output: Charlie

            // ============================================
            // EVENT HANDLERS WITH LAMBDA
            // ============================================

            Console.WriteLine($"\n=== Event Handlers with Lambda ===");

            // Example 7: Simple event subscription (simulated)
            var button = new Button("Submit");
            button.Click += (sender, e) => Console.WriteLine($"Button {sender} was clicked!");
            button.Click += (sender, e) => Console.WriteLine($"Click handler 2: Processing...");
            button.RaiseClick(); // Output: Two lines of output

            // Example 8: Event with conditional handler
            button.Click += (sender, e) =>
            {
                if (sender is Button b && b.Name == "Submit")
                {
                    Console.WriteLine("Processing submit...");
                }
            };

            // ============================================
            // PREDICATES IN REAL SCENARIOS
            // ============================================

            Console.WriteLine($"\n=== Predicates in Real Scenarios ===");

            // Example 9: Validation predicates
            Predicate<RWOrder> isValidOrder = o =>
                o.Amount > 0 &&
                o.Status != "Cancelled" &&
                o.CustomerId > 0;

            var validOrders = orders.Where(o => isValidOrder(o)).ToList();
            Console.WriteLine($"Valid orders: {validOrders.Count}"); // Output: 4

            // Example 10: Complex search predicate
            Predicate<RWCustomer> searchPredicate = c =>
                c.TotalPurchases >= 1000 &&
                c.Tier != "Bronze" &&
                (DateTime.Now - c.LastPurchaseDate).Days < 30;

            var recentActiveCustomers = customers.Where(c => searchPredicate(c)).ToList();
            Console.WriteLine($"Recent active customers: {string.Join(", ", recentActiveCustomers.Select(c => c.Name))}");
            // Output: Alice, Eve

            // Example 11: Dynamic filtering
            var filter = CreateFilter<Order>(100, "Completed");
            var filteredOrders = orders.Where(o => filter(o)).ToList();
            Console.WriteLine($"Filtered orders (>" + "$100, Completed): {filteredOrders.Count}"); // Output: 2
        }

        // Factory method for creating dynamic filters
        public static Predicate<T> CreateFilter<T>(decimal minAmount, string status)
        {
            return o =>
            {
                var order = o as Order;
                return order != null && order.Amount > minAmount && order.Status == status;
            };
        }
    }

    // Simple Button class for event examples
    public class Button
    {
        public string Name { get; }

        public event EventHandler Click;

        public Button(string name)
        {
            Name = name;
        }

        public void RaiseClick()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }
    }
}