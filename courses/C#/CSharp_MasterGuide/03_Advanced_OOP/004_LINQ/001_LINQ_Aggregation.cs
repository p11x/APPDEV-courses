/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Aggregation Operations
 * FILE: LINQ_Aggregation.cs
 * PURPOSE: Sum, Average, Count, Min, Max, Aggregate
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_Aggregation
    {
        public static void Main()
        {
            // Sum - add all values
            Console.WriteLine("=== Sum ===");
            
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            int total = numbers.Sum();
            Console.WriteLine($"Sum of 1-10: {total}"); // Output: 55
            
            // Sum with selector
            var products = new List<Product>
            {
                new Product { Name = "Laptop", Price = 999.99m },
                new Product { Name = "Mouse", Price = 29.99m },
                new Product { Name = "Keyboard", Price = 79.99m }
            };
            
            decimal totalPrice = products.Sum(p => p.Price);
            Console.WriteLine($"Total product price: ${totalPrice:F2}"); // Output: $1109.97
            
            // Sum with condition
            decimal expensiveTotal = products.Sum(p => p.Price > 100 ? p.Price : 0);
            Console.WriteLine($"Expensive products total: ${expensiveTotal:F2}"); // Output: $999.99

            // Average - mean of values
            Console.WriteLine("\n=== Average ===");
            
            var scores = new List<int> { 85, 90, 78, 92, 88 };
            
            double avg = scores.Average();
            Console.WriteLine($"Average score: {avg:F1}"); // Output: 86.6
            
            // Average with selector
            decimal avgPrice = products.Average(p => p.Price);
            Console.WriteLine($"Average product price: ${avgPrice:F2}"); // Output: $369.99

            // Count - number of elements
            Console.WriteLine("\n=== Count ===");
            
            int count = numbers.Count();
            Console.WriteLine($"Count of numbers: {count}"); // Output: 10
            
            // Count with predicate
            int evenCount = numbers.Count(n => n % 2 == 0);
            Console.WriteLine($"Even numbers: {evenCount}"); // Output: 5
            
            // Count grouped
            var categories = new List<string> { "A", "B", "A", "C", "B", "A" };
            var categoryCounts = categories
                .GroupBy(c => c)
                .Select(g => new { Category = g.Key, Count = g.Count() });
            
            Console.WriteLine("Category counts:");
            foreach (var item in categoryCounts)
            {
                Console.WriteLine($"  {item.Category}: {item.Count}");
                // Output: A: 3, B: 2, C: 1
            }

            // Min - smallest value
            Console.WriteLine("\n=== Min ===");
            
            int min = numbers.Min();
            Console.WriteLine($"Min: {min}"); // Output: 1
            
            // Min with selector
            decimal minPrice = products.Min(p => p.Price);
            Console.WriteLine($"Cheapest product: ${minPrice:F2}"); // Output: $29.99
            
            // Min with anonymous type
            var cheapest = products.Min(p => p.Price);
            var cheapestProduct = products.First(p => p.Price == cheapest);
            Console.WriteLine($"Cheapest product name: {cheapestProduct.Name}"); // Output: Mouse

            // Max - largest value
            Console.WriteLine("\n=== Max ===");
            
            int max = numbers.Max();
            Console.WriteLine($"Max: {max}"); // Output: 10
            
            // Max with selector
            decimal maxPrice = products.Max(p => p.Price);
            Console.WriteLine($"Most expensive: ${maxPrice:F2}"); // Output: $999.99

            // Aggregate - custom accumulation
            Console.WriteLine("\n=== Aggregate ===");
            
            // Simple aggregate - product of all numbers
            int product = numbers.Aggregate((acc, n) => acc * n);
            Console.WriteLine($"Product of 1-10: {product}"); // Output: 3628800
            
            // Aggregate with seed value
            string concatenated = numbers.Aggregate("Numbers: ", (acc, n) => acc + n + ", ");
            Console.WriteLine(concatenated); // Output: Numbers: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 
            
            // Aggregate with result selector
            var stats = numbers.Aggregate(
                seed: new { Sum = 0, Count = 0, Min = int.MaxValue, Max = int.MinValue },
                func: (acc, n) => new 
                { 
                    Sum = acc.Sum + n, 
                    Count = acc.Count + 1, 
                    Min = Math.Min(acc.Min, n), 
                    Max = Math.Max(acc.Max, n) 
                },
                resultSelector: acc => $"Sum: {acc.Sum}, Count: {acc.Count}, Min: {acc.Min}, Max: {acc.Max}"
            );
            Console.WriteLine(stats); // Output: Sum: 55, Count: 10, Min: 1, Max: 10

            // REAL WORLD EXAMPLE: Sales statistics
            Console.WriteLine("\n=== Real World: Sales Statistics ===");
            
            var sales = new List<Sale>
            {
                new Sale { Product = "Laptop", Region = "North", Amount = 5000 },
                new Sale { Product = "Mouse", Region = "South", Amount = 1500 },
                new Sale { Product = "Laptop", Region = "South", Amount = 3000 },
                new Sale { Product = "Keyboard", Region = "North", Amount = 2000 },
                new Sale { Product = "Mouse", Region = "North", Amount = 1000 }
            };
            
            decimal totalSales = sales.Sum(s => s.Amount);
            decimal avgSale = sales.Average(s => s.Amount);
            decimal minSale = sales.Min(s => s.Amount);
            decimal maxSale = sales.Max(s => s.Amount);
            int saleCount = sales.Count;
            
            Console.WriteLine($"Total Sales: ${totalSales:N0}");
            Console.WriteLine($"Average Sale: ${avgSale:N0}");
            Console.WriteLine($"Min Sale: ${minSale:N0}");
            Console.WriteLine($"Max Sale: ${maxSale:N0}");
            Console.WriteLine($"Number of Sales: {saleCount}");
            // Output: Total: $12500, Avg: $2500, Min: $1000, Max: $5000, Count: 5

            // REAL WORLD EXAMPLE: Order statistics by customer
            Console.WriteLine("\n=== Real World: Customer Order Stats ===");
            
            var orders = new List<Order>
            {
                new Order { CustomerId = 1, Total = 150.00m },
                new Order { CustomerId = 1, Total = 200.00m },
                new Order { CustomerId = 2, Total = 75.00m },
                new Order { CustomerId = 2, Total = 300.00m },
                new Order { CustomerId = 3, Total = 50.00m }
            };
            
            var customerStats = orders
                .GroupBy(o => o.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    OrderCount = g.Count(),
                    TotalSpent = g.Sum(o => o.Total),
                    AvgOrder = g.Average(o => o.Total),
                    MaxOrder = g.Max(o => o.Total),
                    MinOrder = g.Min(o => o.Total)
                });
            
            Console.WriteLine("Customer Statistics:");
            foreach (var stat in customerStats)
            {
                Console.WriteLine($"  Customer {stat.CustomerId}: {stat.OrderCount} orders, " +
                    $"Total ${stat.TotalSpent:F2}, Avg ${stat.AvgOrder:F2}, " +
                    $"Max ${stat.MaxOrder:F2}, Min ${stat.MinOrder:F2}");
                // Output: Customer 1: 2 orders, Total $350, Avg $175, Max $200, Min $150
                // Output: Customer 2: 2 orders, Total $375, Avg $187.50, Max $300, Min $75
                // Output: Customer 3: 1 orders, Total $50, Avg $50, Max $50, Min $50
            }
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class Sale
    {
        public string Product { get; set; }
        public string Region { get; set; }
        public decimal Amount { get; set; }
    }

    public class Order
    {
        public int CustomerId { get; set; }
        public decimal Total { get; set; }
    }
}