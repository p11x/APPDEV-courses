/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Real-World Examples - Part 2
 * FILE: LINQ_RealWorld_Part2.cs
 * PURPOSE: Data transformation, analytics, and projection
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_RealWorld_Part2
    {
        public static void Main()
        {
            // Real-world scenario: Transform raw data to DTOs
            Console.WriteLine("=== Real World: Data Transformation ===");
            
            var rawProducts = new List<RawProduct>
            {
                new RawProduct { product_id = 1, product_name = "Laptop", price = 1299.99, category = "electronics" },
                new RawProduct { product_id = 2, product_name = "Chair", price = 299.99, category = "furniture" },
                new RawProduct { product_id = 3, product_name = "Mouse", price = 29.99, category = "electronics" }
            };
            
            // Transform to clean DTO
            var productDTOs = rawProducts
                .Select(p => new ProductDTO
                {
                    Id = p.product_id,
                    Name = p.product_name,
                    Price = p.price,
                    Category = p.category.ToUpper(),
                    PriceCategory = p.price > 500 ? "Premium" : "Standard"
                });
            
            Console.WriteLine("Transformed Products:");
            foreach (var p in productDTOs)
            {
                Console.WriteLine($"  {p.Name} ({p.Category}): ${p.Price} - {p.PriceCategory}");
                // Output: Laptop (ELECTRONICS): $1299.99 - Premium
                // Output: Chair (FURNITURE): $299.99 - Standard
            }

            // Real-world scenario: Sales data transformation
            Console.WriteLine("\n=== Real World: Sales Analytics ===");
            
            var sales = new List<SaleRecord>
            {
                new SaleRecord { Date = new DateTime(2024, 1, 1), Region = "North", Product = "Laptop", Quantity = 10, UnitPrice = 1000 },
                new SaleRecord { Date = new DateTime(2024, 1, 1), Region = "South", Product = "Mouse", Quantity = 50, UnitPrice = 30 },
                new SaleRecord { Date = new DateTime(2024, 1, 2), Region = "North", Product = "Mouse", Quantity = 30, UnitPrice = 30 },
                new SaleRecord { Date = new DateTime(2024, 1, 2), Region = "South", Product = "Laptop", Quantity = 5, UnitPrice = 1000 },
                new SaleRecord { Date = new DateTime(2024, 1, 3), Region = "East", Product = "Keyboard", Quantity = 20, UnitPrice = 50 }
            };
            
            // Transform to sales summary
            var salesSummary = sales
                .Select(s => new
                {
                    s.Date,
                    s.Region,
                    s.Product,
                    TotalRevenue = s.Quantity * s.UnitPrice,
                    Commission = s.Quantity * s.UnitPrice * 0.05m
                })
                .OrderByDescending(s => s.TotalRevenue);
            
            Console.WriteLine("Sales Summary (by revenue):");
            foreach (var s in salesSummary)
            {
                Console.WriteLine($"  {s.Date:MMM dd} {s.Region} {s.Product}: ${s.TotalRevenue} (Comm: ${s.Commission})");
                // Output: Jan 01 North Laptop: $10000 (Comm: $500)
            }

            // Real-world scenario: User activity transformation
            Console.WriteLine("\n=== Real World: User Activity Analysis ===");
            
            var userLogs = new List<UserActivity>
            {
                new UserActivity { UserId = 1, Action = "Login", Timestamp = new DateTime(2024, 1, 1, 8, 0, 0) },
                new UserActivity { UserId = 1, Action = "ViewProduct", Timestamp = new DateTime(2024, 1, 1, 8, 5, 0) },
                new UserActivity { UserId = 1, Action = "AddToCart", Timestamp = new DateTime(2024, 1, 1, 8, 10, 0) },
                new UserActivity { UserId = 2, Action = "Login", Timestamp = new DateTime(2024, 1, 1, 9, 0, 0) },
                new UserActivity { UserId = 2, Action = "ViewProduct", Timestamp = new DateTime(2024, 1, 1, 9, 3, 0) },
                new UserActivity { UserId = 1, Action = "Purchase", Timestamp = new DateTime(2024, 1, 1, 8, 15, 0) }
            };
            
            // Transform to user journey
            var userJourneys = userLogs
                .GroupBy(l => l.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    SessionStart = g.Min(l => l.Timestamp),
                    SessionEnd = g.Max(l => l.Timestamp),
                    Actions = string.Join(" -> ", g.OrderBy(l => l.Timestamp).Select(l => l.Action)),
                    ActionCount = g.Count()
                });
            
            Console.WriteLine("User Sessions:");
            foreach (var session in userJourneys)
            {
                Console.WriteLine($"  User {session.UserId}: {session.Actions}");
                Console.WriteLine($"    Duration: {session.SessionEnd - session.SessionStart}");
                // Output: User 1: Login -> ViewProduct -> AddToCart -> Purchase
            }

            // Real-world scenario: Flatten nested data
            Console.WriteLine("\n=== Real World: Flatten Nested Data ===");
            
            var orders = new List<OrderWithItems>
            {
                new OrderWithItems
                {
                    OrderId = 1001,
                    Customer = "John",
                    Items = new List<OrderItem>
                    {
                        new OrderItem { Product = "Laptop", Qty = 1, Price = 1000 },
                        new OrderItem { Product = "Mouse", Qty = 2, Price = 30 }
                    }
                },
                new OrderWithItems
                {
                    OrderId = 1002,
                    Customer = "Jane",
                    Items = new List<OrderItem>
                    {
                        new OrderItem { Product = "Keyboard", Qty = 1, Price = 150 }
                    }
                }
            };
            
            // Flatten to line items
            var lineItems = orders
                .SelectMany(o => o.Items, (o, item) => new
                {
                    OrderId = o.OrderId,
                    Customer = o.Customer,
                    Product = item.Product,
                    Qty = item.Qty,
                    Total = item.Qty * item.Price
                });
            
            Console.WriteLine("All Line Items:");
            foreach (var item in lineItems)
            {
                Console.WriteLine($"  Order #{item.OrderId} ({item.Customer}): {item.Product} x{item.Qty} = ${item.Total}");
                // Output: Order #1001 (John): Laptop x1 = $1000, Mouse x2 = $60, etc.
            }

            // Real-world scenario: Transform to different format
            Console.WriteLine("\n=== Real World: Format Transformation ===");
            
            var temperatures = new List<TemperatureReading>
            {
                new TemperatureReading { City = "New York", Temp = 72, Unit = "F" },
                new TemperatureReading { City = "London", Temp = 20, Unit = "C" },
                new TemperatureReading { City = "Tokyo", Temp = 25, Unit = "C" }
            };
            
            // Convert to standard format
            var standardized = temperatures
                .Select(t => new
                {
                    t.City,
                    Fahrenheit = t.Unit == "F" ? t.Temp : (t.Temp * 9/5) + 32,
                    Celsius = t.Unit == "C" ? t.Temp : (t.Temp - 32) * 5/9
                });
            
            Console.WriteLine("Standardized Temperatures:");
            foreach (var t in standardized)
            {
                Console.WriteLine($"  {t.City}: {t.Fahrenheit:F1}°F / {t.Celsius:F1}°C");
                // Output: New York: 72.0°F / 22.2°C
            }

            // Real-world scenario: KPI calculations
            Console.WriteLine("\n=== Real World: KPI Calculations ===");
            
            var monthlyData = new List<MonthlyMetric>
            {
                new MonthlyMetric { Month = "Jan", Revenue = 50000, Expenses = 35000 },
                new MonthlyMetric { Month = "Feb", Revenue = 55000, Expenses = 38000 },
                new MonthlyMetric { Month = "Mar", Revenue = 48000, Expenses = 32000 },
                new MonthlyMetric { Month = "Apr", Revenue = 62000, Expenses = 40000 }
            };
            
            // Calculate KPIs
            var kpis = monthlyData
                .Select(m => new
                {
                    m.Month,
                    Profit = m.Revenue - m.Expenses,
                    Margin = (m.Revenue - m.Expenses) / m.Revenue * 100,
                    Efficiency = m.Expenses / m.Revenue * 100
                })
                .OrderByDescending(k => k.Profit);
            
            Console.WriteLine("Monthly KPIs:");
            foreach (var k in kpis)
            {
                Console.WriteLine($"  {k.Month}: Profit ${k.Profit}, Margin {k.Margin:F1}%, Efficiency {k.Efficiency:F1}%");
                // Output: Apr: Profit $22000, Margin 35.5%, Efficiency 64.5%
            }
        }
    }

    public class RawProduct
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
        public double price { get; set; }
        public string category { get; set; }
    }

    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string PriceCategory { get; set; }
    }

    public class SaleRecord
    {
        public DateTime Date { get; set; }
        public string Region { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class UserActivity
    {
        public int UserId { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class OrderWithItems
    {
        public int OrderId { get; set; }
        public string Customer { get; set; }
        public List<OrderItem> Items { get; set; }
    }

    public class OrderItem
    {
        public string Product { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }

    public class TemperatureReading
    {
        public string City { get; set; }
        public double Temp { get; set; }
        public string Unit { get; set; }
    }

    public class MonthlyMetric
    {
        public string Month { get; set; }
        public decimal Revenue { get; set; }
        public decimal Expenses { get; set; }
    }
}