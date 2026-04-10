/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : Dictionary<TKey,TValue> - Real-World Examples
 * FILE      : Dictionary_RealWorld.cs
 * PURPOSE   : Demonstrates practical real-world applications of
 *            Dictionary including caching, lookup tables,
 *            frequency counters, and more
 * ============================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._04_Collections._02_Dictionary_Hashtable
{
    class DictionaryRealWorld
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Dictionary<TKey,TValue> Real-World Examples ===\n");

            // ═══════════════════════════════════════════════════════════
            // Example 1: In-Memory Cache
            // ═══════════════════════════════════════════════════════════

            var productCache = new Dictionary<int, Product>();

            // Simulate caching products
            productCache[1] = new Product { Id = 1, Name = "Laptop", Price = 999.99m };
            productCache[2] = new Product { Id = 2, Name = "Mouse", Price = 29.99m };
            productCache[3] = new Product { Id = 3, Name = "Keyboard", Price = 79.99m };

            // Get from cache (simulated lookup)
            int lookupId = 2;
            if (productCache.TryGetValue(lookupId, out Product cachedProduct))
            {
                Console.WriteLine($"[Cache Hit] Product: {cachedProduct.Name}, Price: {cachedProduct.Price:C}");
                // Output: [Cache Hit] Product: Mouse, Price: $29.99
            }
            else
            {
                // Would fetch from database if not in cache
                Console.WriteLine($"[Cache Miss] Would fetch product {lookupId} from database");
            }

            // ═══════════════════════════════════════════════════════════
            // Example 2: Lookup Table
            // ═══════════════════════════════════════════════════════════

            // Country code to name mapping
            var countryCodes = new Dictionary<string, string>
            {
                { "US", "United States" },
                { "UK", "United Kingdom" },
                { "CA", "Canada" },
                { "AU", "Australia" },
                { "DE", "Germany" },
                { "JP", "Japan" },
                { "IN", "India" }
            };

            // Fast lookup
            string code = "DE";
            if (countryCodes.TryGetValue(code, out string countryName))
            {
                Console.WriteLine($"\n[{code}] {countryName}");
                // Output: [DE] Germany
            }

            // Check if country code exists
            string searchCode = "FR";
            Console.WriteLine($"Code '{searchCode}' exists: {countryCodes.ContainsKey(searchCode)}");
            // Output: Code 'FR' exists: False

            // ═══════════════════════════════════════════════════════════
            // Example 3: Word Frequency Counter
            // ═══════════════════════════════════════════════════════════

            string text = "the quick brown fox jumps over the lazy dog the fox is quick";
            string[] words = text.ToLower().Split(' ');

            var wordFrequency = new Dictionary<string, int>();

            foreach (string word in words)
            {
                if (wordFrequency.TryGetValue(word, out int count))
                {
                    wordFrequency[word] = count + 1;
                }
                else
                {
                    wordFrequency[word] = 1;
                }
            }

            Console.WriteLine("\n=== Word Frequency ===");
            foreach (var kvp in wordFrequency.OrderByDescending(w => w.Value))
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            // Output:
            //   the: 3
            //   quick: 2
            //   fox: 2
            //   brown: 1
            //   jumps: 1
            //   over: 1
            //   lazy: 1
            //   dog: 1
            //   is: 1

            // ═══════════════════════════════════════════════════════════
            // Example 4: Inventory Management
            // ═══════════════════════════════════════════════════════════

            var inventory = new Dictionary<string, int>
            {
                { "Laptop", 50 },
                { "Mouse", 200 },
                { "Keyboard", 150 },
                { "Monitor", 75 },
                { "Headphones", 100 }
            };

            // Check stock
            string itemToCheck = "Mouse";
            if (inventory.TryGetValue(itemToCheck, out int stock))
            {
                if (stock > 0)
                {
                    Console.WriteLine($"\n[{itemToCheck}] In stock: {stock} units");
                    // Output: [Mouse] In stock: 200 units
                }
                else
                {
                    Console.WriteLine($"\n[{itemToCheck}] Out of stock");
                }
            }

            // Process order (decrement stock)
            string orderItem = "Laptop";
            int orderQuantity = 5;

            if (inventory.TryGetValue(orderItem, out int currentStock))
            {
                if (currentStock >= orderQuantity)
                {
                    inventory[orderItem] = currentStock - orderQuantity;
                    Console.WriteLine($"Order fulfilled: {orderQuantity} {orderItem}(s)");
                    // Output: Order fulfilled: 5 Laptop(s)
                    Console.WriteLine($"Remaining stock: {inventory[orderItem]}");
                    // Output: Remaining stock: 45
                }
                else
                {
                    Console.WriteLine($"Insufficient stock. Available: {currentStock}");
                }
            }

            // ═══════════════════════════════════════════════════════════
            // Example 5: Phonebook/Contact Lookup
            // ═══════════════════════════════════════════════════════════

            var phonebook = new Dictionary<string, string>
            {
                { "John Smith", "555-0101" },
                { "Jane Doe", "555-0102" },
                { "Bob Johnson", "555-0103" },
                { "Alice Williams", "555-0104" }
            };

            // Search by name
            string searchName = "Jane Doe";
            if (phonebook.TryGetValue(searchName, out string phoneNumber))
            {
                Console.WriteLine($"\n{searchName}: {phoneNumber}");
                // Output: Jane Doe: 555-0102
            }

            // ═══════════════════════════════════════════════════════════
            // Example 6: Configuration Dictionary
            // ═══════════════════════════════════════════════════════════

            var appConfig = new Dictionary<string, string>
            {
                { "DatabaseHost", "localhost" },
                { "DatabasePort", "5432" },
                { "DatabaseName", "myapp" },
                { "MaxConnections", "100" },
                { "EnableCache", "true" },
                { "LogLevel", "Information" }
            };

            // Get with default fallback
            string GetConfig(string key, string defaultValue = "")
            {
                return appConfig.TryGetValue(key, out string value) ? value : defaultValue;
            }

            Console.WriteLine("\n=== Application Configuration ===");
            Console.WriteLine($"DB Host: {GetConfig("DatabaseHost")}");
            // Output: DB Host: localhost
            Console.WriteLine($"DB Port: {GetConfig("DatabasePort")}");
            // Output: DB Port: 5432
            Console.WriteLine($"Cache: {GetConfig("EnableCache", "false")}");
            // Output: Cache: true
            Console.WriteLine($"Unknown: {GetConfig("UnknownKey", "default_value")}");
            // Output: Unknown: default_value

            // ═══════════════════════════════════════════════════════════
            // Example 7: Grouping by Key
            // ═══════════════════════════════════════════════════════════

            var employees = new List<Employee>
            {
                new Employee { Name = "Alice", Department = "Engineering" },
                new Employee { Name = "Bob", Department = "Marketing" },
                new Employee { Name = "Charlie", Department = "Engineering" },
                new Employee { Name = "Diana", Department = "HR" },
                new Employee { Name = "Eve", Department = "Marketing" }
            };

            // Group employees by department
            var employeesByDept = new Dictionary<string, List<Employee>>();

            foreach (var emp in employees)
            {
                if (!employeesByDept.ContainsKey(emp.Department))
                {
                    employeesByDept[emp.Department] = new List<Employee>();
                }
                employeesByDept[emp.Department].Add(emp);
            }

            Console.WriteLine("\n=== Employees by Department ===");
            foreach (var dept in employeesByDept)
            {
                Console.WriteLine($"\n{dept.Key}:");
                foreach (var emp in dept.Value)
                {
                    Console.WriteLine($"  - {emp.Name}");
                }
            }
            // Output:
            //   Engineering:
            //     - Alice
            //     - Charlie
            //   Marketing:
            //     - Bob
            //     - Eve
            //   HR:
            //     - Diana

            // ═══════════════════════════════════════════════════════════
            // Example 8: Price Lookup with Quantity Discounts
            // ═══════════════════════════════════════════════════════════

            var pricing = new Dictionary<string, decimal>
            {
                { "Widget", 10.00m },
                { "Gadget", 25.00m },
                { "Gizmo", 50.00m }
            };

            var discounts = new Dictionary<string, Dictionary<int, decimal>>
            {
                { "Widget", new Dictionary<int, decimal> { { 10, 0.10m }, { 50, 0.20m } } },
                { "Gadget", new Dictionary<int, decimal> { { 5, 0.05m }, { 20, 0.15m } } },
                { "Gizmo", new Dictionary<int, decimal> { { 3, 0.05m }, { 10, 0.10m } } }
            };

            // Calculate order total with volume discounts
            string product = "Widget";
            int quantity = 25;

            decimal unitPrice = pricing[product];
            decimal discount = 0m;

            if (discounts.TryGetValue(product, out var tieredDiscounts))
            {
                foreach (var tier in tieredDiscounts.OrderByDescending(t => t.Key))
                {
                    if (quantity >= tier.Key)
                    {
                        discount = tier.Value;
                        break;
                    }
                }
            }

            decimal discountedPrice = unitPrice * (1 - discount);
            decimal total = discountedPrice * quantity;

            Console.WriteLine($"\n=== Price Calculation ===");
            Console.WriteLine($"Product: {product}");
            Console.WriteLine($"Quantity: {quantity}");
            Console.WriteLine($"Unit Price: {unitPrice:C}");
            Console.WriteLine($"Discount: {discount:P0}");
            Console.WriteLine($"Total: {total:C}");
            // Output:
            //   Product: Widget
            //   Quantity: 25
            //   Unit Price: $10.00
            //   Discount: 20%
            //   Total: $200.00

            Console.WriteLine("\n=== Dictionary Real-World Examples Complete ===");
        }
    }

    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    class Employee
    {
        public string Name { get; set; }
        public string Department { get; set; }
    }
}
