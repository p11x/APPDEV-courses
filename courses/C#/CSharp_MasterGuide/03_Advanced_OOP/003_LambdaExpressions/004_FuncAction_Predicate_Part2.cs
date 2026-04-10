/*
 * TOPIC: CSharp_MasterGuide/03_Advanced_OOP/03_LambdaExpressions
 * SUBTOPIC: Func<T>, Action<T>, Predicate<T> - Advanced Usage
 * FILE: FuncAction_Predicate_Part2.cs
 * PURPOSE: Advanced delegate patterns, combining delegates, real-world scenarios
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._03_LambdaExpressions
{
    public class Part2Order
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }

    public class FuncActionPredicatePart2
    {
        public static void RunMain(string[] args)
        {
            Console.WriteLine("=== Advanced Func, Action, Predicate Patterns ===\n");

            // ============================================
            // DELEGATE COMPOSITION
            // ============================================

            // Example 1: Composing Funcs (function composition)
            // f(g(x)) - apply g first, then f
            Func<int, int> add5 = x => x + 5;
            Func<int, int> multiply3 = x => x * 3;
            Func<int, int> composed = x => multiply3(add5(x));
            Console.WriteLine($"Compose: add5(2) * 3 = {composed(2)}"); // Output: 21

            // Example 2: Composition helper method
            Func<double, double> add10 = x => x + 10;
            Func<double, double> squareRoot = x => Math.Sqrt(x);
            Func<double, double> composed2 = Compose(add10, squareRoot);
            Console.WriteLine($"Compose(sqrt(add10)): sqrt(26) = {composed2(26):F2}"); // Output: 6.00

            // Example 3: Composing predicates (AND logic)
            Predicate<int> isPositive = n => n > 0;
            Predicate<int> isEven = n => n % 2 == 0;
            Predicate<int> isPositiveEven = And(isPositive, isEven);
            Console.WriteLine($"\nPredicate AND: Is 4 positive even? {isPositiveEven(4)}"); // True
            Console.WriteLine($"Is 3 positive even? {isPositiveEven(3)}"); // False

            // Example 4: Composing predicates (OR logic)
            Predicate<int> isNegative = n => n < 0;
            Predicate<int> isPositiveOrNegative = Or(isPositive, isNegative);
            Console.WriteLine($"Predicate OR: Is 5 non-zero? {isPositiveOrNegative(5)}"); // True
            Console.WriteLine($"Is -5 non-zero? {isPositiveOrNegative(-5)}"); // True
            Console.WriteLine($"Is 0 non-zero? {isPositiveOrNegative(0)}"); // False

            // Example 5: Negating predicates
            Predicate<int> isNotPositive = Not(isPositive);
            Console.WriteLine($"Predicate NOT: Is 5 not positive? {isNotPositive(5)}"); // False
            Console.WriteLine($"Is -5 not positive? {isNotPositive(-5)}"); // True

            // ============================================
            // PIPELINE PATTERNS
            // ============================================

            // Example 6: Processing pipeline with Action
            var logs = new List<string>();
            Action<string> addTimestamp = msg => logs.Add($"{DateTime.Now:HH:mm:ss} {msg}");
            Action<string> uppercase = msg => msg.ToUpper();
            Action<string> logWithPrefix = msg => logs.Add($"LOG: {msg}");

            Action<string> pipeline = Chain(addTimestamp, uppercase, logWithPrefix);
            Console.WriteLine("\nAction pipeline:");
            pipeline("system started");

            // Example 7: LINQ-style pipeline with Func
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var result = numbers
                .Where(n => n > 3)               // Filter: > 3
                .Select(n => n * 2)              // Transform: * 2
                .Where(n => n < 15)              // Filter again: < 15
                .OrderByDescending(n => n);       // Sort
            Console.WriteLine($"LINQ pipeline: {string.Join(", ", result)}"); // Output: 14, 12, 10, 8, 6

            // ============================================
            // REAL-WORLD PATTERNS
            // ============================================

            // Example 8: Validation pipeline
            var orders = new List<Part2Order>
            {
                new Part2Order { OrderId = 1, Amount = 100, Customer = "Alice", Status = "Completed" },
                new Part2Order { OrderId = 2, Amount = -50, Customer = "Bob", Status = "Pending" }, // Invalid
                new Part2Order { OrderId = 3, Amount = 200, Customer = "", Status = "Completed" }, // Invalid
                new Part2Order { OrderId = 4, Amount = 50, Customer = "Charlie", Status = "Cancelled" }
            };

            // Complex validation using combined predicates
            Predicate<Part2Order> isValidOrder = ValidateOrder();

            Console.WriteLine("\nValid orders:");
            foreach (var order in orders.Where(o => isValidOrder(o)))
            {
                Console.WriteLine($"Order {order.OrderId}: {order.Customer} - ${order.Amount}");
            }

            // Example 9: Specification pattern (reusable predicates)
            var completedOnly = new Specification<Part2Order>(o => o.Status == "Completed");
            var highValue = new Specification<Part2Order>(o => o.Amount >= 100);
            var validCustomer = new Specification<Part2Order>(o => !string.IsNullOrEmpty(o.Customer));

            var spec = completedOnly.And(highValue).And(validCustomer);
            Console.WriteLine("\nSpecification pattern:");
            foreach (var order in orders.Where(o => spec.IsSatisfiedBy(o)))
            {
                Console.WriteLine($"Order {order.OrderId}: ${order.Amount} - {order.Status}");
            }

            // Example 10: Strategy pattern with Func
            var shoppingCart = new List<decimal> { 100, 50, 75, 200 };
            
            Func<decimal, decimal> noDiscount = subtotal => subtotal;
            Func<decimal, decimal> tenPercentOff = subtotal => subtotal * 0.9m;
            Func<decimal, decimal> flatTenOff = subtotal => Math.Max(0, subtotal - 10);

            Console.WriteLine("\nStrategy pattern - Different discount strategies:");
            Console.WriteLine($"No discount: ${CalculateTotal(shoppingCart, noDiscount):F2}"); // Output: 425.00
            Console.WriteLine($"10% off: ${CalculateTotal(shoppingCart, tenPercentOff):F2}"); // Output: 382.50
            Console.WriteLine($"Flat $10 off: ${CalculateTotal(shoppingCart, flatTenOff):F2}"); // Output: 415.00
        }

        // Helper for composing Funcs
        public static Func<T1, TResult> Compose<T1, TIntermediate, TResult>(
            Func<T1, TIntermediate> first,
            Func<T2, TResult> second)
        {
            return x => second(first(x));
        }

        // Note: This overload is needed for proper type inference in Compose example
        public static T2 Compose<T1, T2>(Func<T1, T2> first, Func<T2, T2> second)
        {
            return second(first);
        }

        // Helper for composing predicates (AND)
        public static Predicate<T> And<T>(Predicate<T> first, Predicate<T> second)
        {
            return t => first(t) && second(t);
        }

        // Helper for composing predicates (OR)
        public static Predicate<T> Or<T>(Predicate<T> first, Predicate<T> second)
        {
            return t => first(t) || second(t);
        }

        // Helper for negating predicates
        public static Predicate<T> Not<T>(Predicate<T> predicate)
        {
            return t => !predicate(t);
        }

        // Helper for chaining Actions
        public static Action<T> Chain<T>(params Action<T>[] actions)
        {
            return t =>
            {
                foreach (var action in actions)
                {
                    action(t);
                }
            };
        }

        // Complex validation predicate
        public static Predicate<Part2Order> ValidateOrder()
        {
            return o => o.Amount > 0 &&
                      !string.IsNullOrEmpty(o.Customer) &&
                      o.Status != "Cancelled";
        }

        // Calculate total using strategy
        public static decimal CalculateTotal(List<decimal> items, Func<decimal, decimal> discountStrategy)
        {
            var subtotal = items.Sum();
            return discountStrategy(subtotal);
        }
    }

    // Specification pattern class - reusable, composable predicates
    public class Specification<T>
    {
        private readonly Predicate<T> _predicate;

        public Specification(Predicate<T> predicate)
        {
            _predicate = predicate;
        }

        public bool IsSatisfiedBy(T item) => _predicate(item);

        public Specification<T> And(Specification<T> other)
        {
            return new Specification<T>(item => this.IsSatisfiedBy(item) && other.IsSatisfiedBy(item));
        }

        public Specification<T> Or(Specification<T> other)
        {
            return new Specification<T>(item => this.IsSatisfiedBy(item) || other.IsSatisfiedBy(item));
        }

        public Specification<T> Not()
        {
            return new Specification<T>(item => !this.IsSatisfiedBy(item));
        }
    }
}