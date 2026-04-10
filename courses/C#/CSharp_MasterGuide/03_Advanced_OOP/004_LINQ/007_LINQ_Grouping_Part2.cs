/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Grouping Operations - Part 2
 * FILE: LINQ_Grouping_Part2.cs
 * PURPOSE: More grouping patterns and advanced grouping
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_Grouping_Part2
    {
        public static void Main()
        {
            // Group by with custom key (computed)
            Console.WriteLine("=== GroupBy with Computed Keys ===");
            
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Amount = 150.00m, Date = new DateTime(2024, 1, 15) },
                new Transaction { Id = 2, Amount = 200.00m, Date = new DateTime(2024, 1, 20) },
                new Transaction { Id = 3, Amount = 75.00m, Date = new DateTime(2024, 2, 5) },
                new Transaction { Id = 4, Amount = 300.00m, Date = new DateTime(2024, 2, 10) },
                new Transaction { Id = 5, Amount = 50.00m, Date = new DateTime(2024, 2, 25) }
            };
            
            // Group by month
            var byMonth = transactions.GroupBy(t => t.Date.Month);
            
            foreach (var group in byMonth)
            {
                Console.WriteLine($"\nMonth {group.Key}:");
                foreach (var t in group)
                {
                    Console.WriteLine($"  ${t.Amount} on {t.Date:MMM dd}");
                    // Output: Month 1: $150 on Jan 15, $200 on Jan 20
                    // Output: Month 2: $75 on Feb 05, $300 on Feb 10, $50 on Feb 25
                }
            }
            
            // Group by year-month string
            Console.WriteLine("\nGroup by Year-Month:");
            var byYearMonth = transactions.GroupBy(t => t.Date.ToString("yyyy-MM"));
            
            foreach (var group in byYearMonth)
            {
                decimal total = group.Sum(t => t.Amount);
                Console.WriteLine($"{group.Key}: ${total}");
                // Output: 2024-01: $350, 2024-02: $425
            }

            // GroupBy with nested grouping
            Console.WriteLine("\n=== Nested Grouping ===");
            
            var sales = new List<Sale>
            {
                new Sale { Product = "Laptop", Region = "North", Quarter = 1, Amount = 5000 },
                new Sale { Product = "Laptop", Region = "North", Quarter = 2, Amount = 3000 },
                new Sale { Product = "Mouse", Region = "North", Quarter = 1, Amount = 1500 },
                new Sale { Product = "Laptop", Region = "South", Quarter = 1, Amount = 4000 },
                new Sale { Product = "Mouse", Region = "South", Quarter = 2, Amount = 2000 }
            };
            
            // Group by product, then by region
            var nested = sales.GroupBy(s => s.Product)
                .Select(productGroup => new
                {
                    Product = productGroup.Key,
                    ByRegion = productGroup.GroupBy(s => s.Region)
                        .Select(regionGroup => new
                        {
                            Region = regionGroup.Key,
                            Total = regionGroup.Sum(s => s.Amount)
                        })
                });
            
            foreach (var product in nested)
            {
                Console.WriteLine($"\nProduct: {product.Product}");
                foreach (var region in product.ByRegion)
                {
                    Console.WriteLine($"  {region.Region}: ${region.Total}");
                    // Output: Laptop: North: $8000, South: $4000
                    // Output: Mouse: North: $1500, South: $2000
                }
            }

            // GroupBy with compound key and ordering
            Console.WriteLine("\n=== GroupBy with Ordered Results ===");
            
            var data = new List<Item>
            {
                new Item { Category = "Electronics", Name = "Laptop", Value = 100 },
                new Item { Category = "Furniture", Name = "Chair", Value = 50 },
                new Item { Category = "Electronics", Name = "Mouse", Value = 30 },
                new Item { Category = "Furniture", Name = "Desk", Value = 80 },
                new Item { Category = "Electronics", Name = "Keyboard", Value = 40 }
            };
            
            var groupedOrdered = data
                .GroupBy(i => i.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Items = g.OrderByDescending(i => i.Value).ToList()
                });
            
            foreach (var group in groupedOrdered)
            {
                Console.WriteLine($"\n{group.Category}:");
                foreach (var item in group.Items)
                {
                    Console.WriteLine($"  {item.Name}: {item.Value}");
                    // Output: Electronics: Laptop 100, Keyboard 40, Mouse 30
                    // Output: Furniture: Desk 80, Chair 50
                }
            }

            // REAL WORLD EXAMPLE: Sales by category and year
            Console.WriteLine("\n=== Real World: Sales Analysis ===");
            
            var salesData = new List<SalesRecord>
            {
                new SalesRecord { Category = "Electronics", Year = 2023, Sales = 150000 },
                new SalesRecord { Category = "Electronics", Year = 2024, Sales = 180000 },
                new SalesRecord { Category = "Furniture", Year = 2023, Sales = 80000 },
                new SalesRecord { Category = "Furniture", Year = 2024, Sales = 95000 },
                new SalesRecord { Category = "Clothing", Year = 2023, Sales = 60000 },
                new SalesRecord { Category = "Clothing", Year = 2024, Sales = 75000 }
            };
            
            var yearlyAnalysis = salesData
                .GroupBy(r => new { r.Category, r.Year })
                .OrderBy(g => g.Key.Year)
                .ThenByDescending(g => g.Sum(r => r.Sales));
            
            Console.WriteLine("Sales by Category and Year:");
            foreach (var group in yearlyAnalysis)
            {
                Console.WriteLine($"  {group.Key.Year} - {group.Key.Category}: ${group.Sum(r => r.Sales):N0}");
                // Output: 2023 - Electronics: $150,000, etc.
            }

            // REAL WORLD EXAMPLE: Inventory by warehouse
            Console.WriteLine("\n=== Real World: Inventory by Warehouse ===");
            
            var inventory = new List<InventoryItem>
            {
                new InventoryItem { Product = "Laptop", Warehouse = "A", Quantity = 50 },
                new InventoryItem { Product = "Laptop", Warehouse = "B", Quantity = 30 },
                new InventoryItem { Product = "Mouse", Warehouse = "A", Quantity = 200 },
                new InventoryItem { Product = "Mouse", Warehouse = "B", Quantity = 150 },
                new InventoryItem { Product = "Keyboard", Warehouse = "A", Quantity = 100 }
            };
            
            var byWarehouse = inventory
                .GroupBy(i => i.Warehouse)
                .Select(g => new
                {
                    Warehouse = g.Key,
                    TotalItems = g.Sum(i => i.Quantity),
                    Products = g.Select(i => $"{i.Product} ({i.Quantity})").ToList()
                });
            
            foreach (var w in byWarehouse)
            {
                Console.WriteLine($"\nWarehouse {w.Warehouse}: {w.TotalItems} items");
                Console.WriteLine($"  Products: {string.Join(", ", w.Products)}");
                // Output: A: 350 items, B: 180 items
            }
        }
    }

    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

    public class Sale
    {
        public string Product { get; set; }
        public string Region { get; set; }
        public int Quarter { get; set; }
        public decimal Amount { get; set; }
    }

    public class Item
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class SalesRecord
    {
        public string Category { get; set; }
        public int Year { get; set; }
        public decimal Sales { get; set; }
    }

    public class InventoryItem
    {
        public string Product { get; set; }
        public string Warehouse { get; set; }
        public int Quantity { get; set; }
    }
}