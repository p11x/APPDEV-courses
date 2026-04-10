/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Real-World Examples - Part 3
 * FILE: LINQ_RealWorld_Part3.cs
 * PURPOSE: Aggregation, grouping, and reporting
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_RealWorld_Part3
    {
        public static void Main()
        {
            // Real-world scenario: Sales reporting
            Console.WriteLine("=== Real World: Sales Reports ===");
            
            var sales = new List<Sale>
            {
                new Sale { Date = new DateTime(2024, 1, 1), Region = "North", Product = "Laptop", Amount = 10000 },
                new Sale { Date = new DateTime(2024, 1, 1), Region = "South", Product = "Mouse", Amount = 1500 },
                new Sale { Date = new DateTime(2024, 1, 2), Region = "North", Product = "Mouse", Amount = 900 },
                new Sale { Date = new DateTime(2024, 1, 2), Region = "South", Product = "Laptop", Amount = 5000 },
                new Sale { Date = new DateTime(2024, 1, 3), Region = "East", Product = "Keyboard", Amount = 1000 },
                new Sale { Date = new DateTime(2024, 1, 3), Region = "North", Product = "Keyboard", Amount = 2000 },
                new Sale { Date = new DateTime(2024, 1, 4), Region = "West", Product = "Laptop", Amount = 8000 }
            };
            
            // Total sales by region
            var salesByRegion = sales
                .GroupBy(s => s.Region)
                .Select(g => new
                {
                    Region = g.Key,
                    TotalSales = g.Sum(s => s.Amount),
                    TransactionCount = g.Count(),
                    AverageSale = g.Average(s => s.Amount)
                })
                .OrderByDescending(r => r.TotalSales);
            
            Console.WriteLine("Sales by Region:");
            foreach (var r in salesByRegion)
            {
                Console.WriteLine($"  {r.Region}: ${r.TotalSales:N0} ({r.TransactionCount} deals, Avg: ${r.AverageSale:N0})");
                // Output: North: $12,900, South: $6,500, etc.
            }

            // Real-world scenario: Inventory reporting
            Console.WriteLine("\n=== Real World: Inventory Report ===");
            
            var inventory = new List<InventoryItem>
            {
                new InventoryItem { SKU = "LAP001", Category = "Electronics", Quantity = 50, UnitCost = 800 },
                new InventoryItem { SKU = "MOU001", Category = "Electronics", Quantity = 200, UnitCost = 20 },
                new InventoryItem { SKU = "CHR001", Category = "Furniture", Quantity = 30, UnitCost = 200 },
                new InventoryItem { SKU = "KEY001", Category = "Electronics", Quantity = 100, UnitCost = 100 },
                new InventoryItem { SKU = "DSK001", Category = "Furniture", Quantity = 15, UnitCost = 400 }
            };
            
            // Inventory value by category
            var inventoryValue = inventory
                .GroupBy(i => i.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    ItemCount = g.Count(),
                    TotalQuantity = g.Sum(i => i.Quantity),
                    TotalValue = g.Sum(i => i.Quantity * i.UnitCost),
                    AvgUnitCost = g.Average(i => i.UnitCost)
                });
            
            Console.WriteLine("Inventory by Category:");
            foreach (var cat in inventoryValue)
            {
                Console.WriteLine($"  {cat.Category}: {cat.ItemCount} SKUs, {cat.TotalQuantity} units, " +
                    $"Value: ${cat.TotalValue:N0}, Avg Cost: ${cat.AvgUnitCost:F2}");
                // Output: Electronics: 3 SKUs, 350 units, Value: $66000
            }

            // Real-world scenario: Customer lifetime value
            Console.WriteLine("\n=== Real World: Customer Lifetime Value ===");
            
            var customerOrders = new List<CustomerOrder>
            {
                new CustomerOrder { CustomerId = 1, Name = "Acme Corp", OrderDate = new DateTime(2024, 1, 1), Amount = 5000 },
                new CustomerOrder { CustomerId = 1, Name = "Acme Corp", OrderDate = new DateTime(2024, 2, 1), Amount = 3000 },
                new CustomerOrder { CustomerId = 1, Name = "Acme Corp", OrderDate = new DateTime(2024, 3, 1), Amount = 7000 },
                new CustomerOrder { CustomerId = 2, Name = "TechStart", OrderDate = new DateTime(2024, 1, 15), Amount = 10000 },
                new CustomerOrder { CustomerId = 2, Name = "TechStart", OrderDate = new DateTime(2024, 2, 15), Amount = 5000 },
                new CustomerOrder { CustomerId = 3, Name = "SmallBiz", OrderDate = new DateTime(2024, 3, 1), Amount = 1000 }
            };
            
            // Calculate CLV metrics
            var clv = customerOrders
                .GroupBy(c => new { c.CustomerId, c.Name })
                .Select(g => new
                {
                    Customer = g.Key.Name,
                    OrderCount = g.Count(),
                    TotalRevenue = g.Sum(c => c.Amount),
                    AverageOrder = g.Average(c => c.Amount),
                    FirstOrder = g.Min(c => c.OrderDate),
                    LastOrder = g.Max(c => c.OrderDate),
                    CustomerSpan = (g.Max(c => c.OrderDate) - g.Min(c => c.OrderDate)).Days
                })
                .OrderByDescending(c => c.TotalRevenue);
            
            Console.WriteLine("Customer Lifetime Value:");
            foreach (var c in clv)
            {
                Console.WriteLine($"  {c.Customer}: {c.OrderCount} orders, ${c.TotalRevenue:N0} total, " +
                    $"Avg ${c.AverageOrder:N0}, Span: {c.CustomerSpan} days");
                // Output: TechStart: 2 orders, $15000 total, Avg $7500
            }

            // Real-world scenario: Department performance report
            Console.WriteLine("\n=== Real World: Department Performance ===");
            
            var employees = new List<EmployeePerf>
            {
                new EmployeePerf { Name = "Alice", Department = "Sales", Revenue = 150000, Leads = 50 },
                new EmployeePerf { Name = "Bob", Department = "Sales", Revenue = 180000, Leads = 65 },
                new EmployeePerf { Name = "Charlie", Department = "Engineering", Projects = 8 },
                new EmployeePerf { Name = "Diana", Department = "Engineering", Projects = 12 },
                new EmployeePerf { Name = "Eve", Department = "Sales", Revenue = 120000, Leads = 40 },
                new EmployeePerf { Name = "Frank", Department = "Engineering", Projects = 6 }
            };
            
            // Sales department metrics
            var salesMetrics = employees
                .Where(e => e.Department == "Sales")
                .GroupBy(e => e.Department)
                .Select(g => new
                {
                    Department = g.Key,
                    EmployeeCount = g.Count(),
                    TotalRevenue = g.Sum(e => e.Revenue ?? 0),
                    TotalLeads = g.Sum(e => e.Leads ?? 0),
                    AvgRevenuePerEmployee = g.Average(e => e.Revenue ?? 0),
                    TopPerformer = g.OrderByDescending(e => e.Revenue).First().Name
                });
            
            Console.WriteLine("Sales Department:");
            foreach (var m in salesMetrics)
            {
                Console.WriteLine($"  {m.EmployeeCount} employees, ${m.TotalRevenue:N0} revenue, " +
                    $"{m.TotalLeads} leads");
                Console.WriteLine($"  Avg/employee: ${m.AvgRevenuePerEmployee:N0}, Top: {m.TopPerformer}");
            }
            
            // Engineering department metrics
            var engMetrics = employees
                .Where(e => e.Department == "Engineering")
                .GroupBy(e => e.Department)
                .Select(g => new
                {
                    Department = g.Key,
                    EmployeeCount = g.Count(),
                    TotalProjects = g.Sum(e => e.Projects),
                    AvgProjects = g.Average(e => e.Projects),
                    TopContributor = g.OrderByDescending(e => e.Projects).First().Name
                });
            
            Console.WriteLine("\nEngineering Department:");
            foreach (var m in engMetrics)
            {
                Console.WriteLine($"  {m.EmployeeCount} employees, {m.TotalProjects} total projects");
                Console.WriteLine($"  Avg/projects: {m.AvgProjects:F1}, Top: {m.TopContributor}");
            }

            // Real-world scenario: Product performance matrix
            Console.WriteLine("\n=== Real World: Product Performance ===");
            
            var productSales = new List<ProductSale>
            {
                new ProductSale { Product = "Laptop", Region = "North", Qty = 100, Revenue = 100000 },
                new ProductSale { Product = "Laptop", Region = "South", Qty = 80, Revenue = 80000 },
                new ProductSale { Product = "Mouse", Region = "North", Qty = 500, Revenue = 15000 },
                new ProductSale { Product = "Mouse", Region = "South", Qty = 400, Revenue = 12000 },
                new ProductSale { Product = "Keyboard", Region = "North", Qty = 200, Revenue = 30000 },
                new ProductSale { Product = "Keyboard", Region = "South", Qty = 150, Revenue = 22500 }
            };
            
            // Product performance across regions
            var productPerformance = productSales
                .GroupBy(p => p.Product)
                .Select(g => new
                {
                    Product = g.Key,
                    TotalQty = g.Sum(p => p.Qty),
                    TotalRevenue = g.Sum(p => p.Revenue),
                    AvgPrice = g.Average(p => p.Qty > 0 ? p.Revenue / p.Qty : 0),
                    Regions = string.Join(", ", g.Select(p => p.Region)),
                    TopRegion = g.OrderByDescending(p => p.Revenue).First().Region
                })
                .OrderByDescending(p => p.TotalRevenue);
            
            Console.WriteLine("Product Performance:");
            foreach (var p in productPerformance)
            {
                Console.WriteLine($"  {p.Product}: {p.TotalQty} units, ${p.TotalRevenue:N0} revenue");
                Console.WriteLine($"    Regions: {p.Regions}, Top: {p.TopRegion}, Avg Price: ${p.AvgPrice:F2}");
            }

            // Real-world scenario: Daily summary report
            Console.WriteLine("\n=== Real World: Daily Summary ===");
            
            var dailyTransactions = new List<TransactionData>
            {
                new TransactionData { Date = new DateTime(2024, 1, 1), Type = "Sale", Amount = 1000 },
                new TransactionData { Date = new DateTime(2024, 1, 1), Type = "Refund", Amount = -50 },
                new TransactionData { Date = new DateTime(2024, 1, 1), Type = "Sale", Amount = 500 },
                new TransactionData { Date = new DateTime(2024, 1, 2), Type = "Sale", Amount = 800 },
                new TransactionData { Date = new DateTime(2024, 1, 2), Type = "Sale", Amount = 1200 },
                new TransactionData { Date = new DateTime(2024, 1, 2), Type = "Refund", Amount = -100 }
            };
            
            var dailySummary = dailyTransactions
                .GroupBy(t => t.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Sales = g.Where(t => t.Type == "Sale").Sum(t => t.Amount),
                    Refunds = g.Where(t => t.Type == "Refund").Sum(t => Math.Abs(t.Amount)),
                    TransactionCount = g.Count(),
                    Net = g.Sum(t => t.Amount)
                })
                .OrderBy(d => d.Date);
            
            Console.WriteLine("Daily Summary:");
            foreach (var d in dailySummary)
            {
                Console.WriteLine($"  {d.Date:MMM dd}: Sales ${d.Sales}, Refunds ${d.Refunds}, " +
                    $"Net ${d.Net}, Transactions: {d.TransactionCount}");
                // Output: Jan 01: Sales $1500, Refunds $50, Net $1450, Transactions: 3
            }
        }
    }

    public class Sale
    {
        public DateTime Date { get; set; }
        public string Region { get; set; }
        public string Product { get; set; }
        public decimal Amount { get; set; }
    }

    public class InventoryItem
    {
        public string SKU { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
    }

    public class CustomerOrder
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Amount { get; set; }
    }

    public class EmployeePerf
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal? Revenue { get; set; }
        public int? Leads { get; set; }
        public int? Projects { get; set; }
    }

    public class ProductSale
    {
        public string Product { get; set; }
        public string Region { get; set; }
        public int Qty { get; set; }
        public decimal Revenue { get; set; }
    }

    public class TransactionData
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
    }
}