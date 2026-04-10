/*
 * TOPIC: Anonymous Types
 * SUBTOPIC: Anonymous types (var, new { }, property inference)
 * FILE: AnonymousTypes.cs
 * PURPOSE: Demonstrate anonymous types in C# including creation, property inference, and usage patterns
 */
using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._06_AnonymousTypes_Tuples
{
    public class AnonymousTypes
    {
        public static void Main()
        {
            // Basic anonymous type creation using 'new { }' syntax
            // The compiler infers property names and types from the initialization expressions
            var person = new { Name = "Alice", Age = 30, City = "New York" };
            
            Console.WriteLine(person.Name);    // Output: Alice
            Console.WriteLine(person.Age);     // Output: 30
            Console.WriteLine(person.City);    // Output: New York

            // Property type inference: compiler determines types automatically
            // Name is inferred as string, Age as int, IsActive as bool
            var employee = new { 
                Name = "Bob", 
                Age = 25, 
                IsActive = true, 
                Salary = 55000.50 
            };
            
            Console.WriteLine($"{employee.Name} - Age: {employee.Age}, Active: {employee.IsActive}");    // Output: Bob - Age: 25, Active: True
            Console.WriteLine($"Salary: {employee.Salary:C}");    // Output: Salary: $55,000.50

            // Anonymous types are read-only - properties cannot be modified after creation
            // The following line would cause a compile error:
            // person.Name = "Charlie";

            // Using anonymous types in collections
            var products = new[]
            {
                new { Name = "Laptop", Price = 999.99m, InStock = true },
                new { Name = "Mouse", Price = 29.99m, InStock = false },
                new { Name = "Keyboard", Price = 79.99m, InStock = true }
            };

            foreach (var product in products)
            {
                var status = product.InStock ? "Available" : "Out of Stock";
                Console.WriteLine($"{product.Name} - ${product.Price} - {status}");
                // Output:
                // Laptop - $999.99 - Available
                // Mouse - $29.99 - Out of Stock
                // Keyboard - $79.99 - Available
            }

            // Anonymous types with inferred names from variables
            string firstName = "Charlie";
            string lastName = "Davis";
            int score = 95;

            // When initializing with variables, property name matches variable name
            var student = new { firstName, lastName, score };
            
            Console.WriteLine($"{student.firstName} {student.lastName}: {student.score}");    // Output: Charlie Davis: 95

            // Nested anonymous types
            var order = new
            {
                OrderId = 1001,
                Customer = new { Name = "Eve", Email = "eve@example.com" },
                Total = 150.00m
            };

            Console.WriteLine($"Order #{order.OrderId} for {order.Customer.Name}");    // Output: Order #1001 for Eve
            Console.WriteLine($"Total: {order.Total:C}");    // Output: Total: $150.00

            // Using anonymous type with method returning anonymous-like data
            dynamic result = GetProductInfo();
            Console.WriteLine($"Product: {result.Name}, Price: {result.Price}");    // Output: Product: Tablet, Price: 499.99

            // Anonymous types are reference types with Equals() overridden
            var person1 = new { Name = "Test", Age = 20 };
            var person2 = new { Name = "Test", Age = 20 };
            
            Console.WriteLine(person1.Equals(person2));    // Output: True (same property values)
            Console.WriteLine(person1 == person2);        // Output: False (different references)

            Console.WriteLine();
            Console.WriteLine("=== Real-World Examples ===");
            Console.WriteLine();

            // Real-world Example 1: Database query result simulation
            // Simulating ORM behavior where anonymous types represent database rows
            var queryResults = new List<object>();
            queryResults.Add(new { Id = 1, FullName = "John Smith", Email = "john@test.com", CreatedDate = new DateTime(2023, 1, 15) });
            queryResults.Add(new { Id = 2, FullName = "Jane Doe", Email = "jane@test.com", CreatedDate = new DateTime(2023, 2, 20) });
            
            foreach (dynamic row in queryResults)
            {
                Console.WriteLine($"User: {row.FullName} ({row.Email}) joined {row.CreatedDate:yyyy-MM-dd}");
                // Output:
                // User: John Smith (john@test.com) joined 2023-01-15
                // User: Jane Doe (jane@test.com) joined 2023-02-20
            }

            // Real-world Example 2: UI Dashboard data aggregation
            // Combining multiple data sources into a single anonymous type
            var salesData = new { Region = "North", Q1Sales = 50000, Q2Sales = 65000 };
            var marketingData = new { Region = "North", Spend = 15000, Campaigns = 5 };
            var inventoryData = new { Region = "North", ItemsInStock = 1200, LowStockItems = 15 };

            // Combining into dashboard view model
            var dashboard = new
            {
                Region = salesData.Region,
                TotalRevenue = salesData.Q1Sales + salesData.Q2Sales,
                MarketingROI = (salesData.Q1Sales + salesData.Q2Sales) / marketingData.Spend,
                InventoryHealth = (inventoryData.ItemsInStock - inventoryData.LowStockItems) * 100.0 / inventoryData.ItemsInStock
            };

            Console.WriteLine($"Dashboard - {dashboard.Region} Region");
            Console.WriteLine($"  Revenue: ${dashboard.TotalRevenue:N0}");    // Output: Revenue: $115,000
            Console.WriteLine($"  Marketing ROI: {dashboard.MarketingROI:F1}x");    // Output: Marketing ROI: 7.7x
            Console.WriteLine($"  Inventory Health: {dashboard.InventoryHealth:F1}%");    // Output: Inventory Health: 98.8%

            // Real-world Example 3: API Response transformation
            // Converting external API data into a cleaner anonymous type
            var rawApiResponse = new { data = new { user_id = 42, user_name = "testuser", account_type = "premium" } };
            
            var apiResponse = new
            {
                UserId = rawApiResponse.data.user_id,
                Username = rawApiResponse.data.user_name,
                AccountType = rawApiResponse.data.account_type.ToString().ToUpper(),
                IsPremium = rawApiResponse.data.account_type == "premium"
            };

            Console.WriteLine($"API Response: {apiResponse.Username} (ID: {apiResponse.UserId})");
            Console.WriteLine($"  Premium: {apiResponse.IsPremium}, Type: {apiResponse.AccountType}");    // Output: Premium: True, Type: PREMIUM
        }

        // Method returning an anonymous type (actually returns object, but conceptually similar)
        static object GetProductInfo()
        {
            // In real scenarios, you'd use concrete types or ValueTuples
            return new { Name = "Tablet", Price = 499.99m };
        }
    }
}