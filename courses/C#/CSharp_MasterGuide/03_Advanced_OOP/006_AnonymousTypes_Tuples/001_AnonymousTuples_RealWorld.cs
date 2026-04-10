/*
 * TOPIC: Real-World Tuple Applications
 * SUBTOPIC: Real-world: returning multiple values, swapping, LINQ
 * FILE: AnonymousTuples_RealWorld.cs
 * PURPOSE: Demonstrate practical real-world applications of tuples in C#
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._06_AnonymousTypes_Tuples
{
    public class AnonymousTuples_RealWorld
    {
        public static void Main()
        {
            // ============================================================
            // Example 1: Returning multiple values from a method
            // ============================================================
            Console.WriteLine("=== Returning Multiple Values ===");

            // Instead of out parameters or custom classes, use ValueTuple
            var (isValid, errors, warnings) = ValidateUserInput("john@example", "password123");
            Console.WriteLine($"Valid: {isValid}, Errors: {errors.Count}, Warnings: {warnings.Count}");
            // Output: Valid: True, Errors: 0, Warnings: 1

            // Multiple return values - search results
            var searchResult = SearchProducts("laptop");
            Console.WriteLine($"Found {searchResult.TotalCount} products, showing page {searchResult.PageNumber} of {searchResult.TotalPages}");
            // Output: Found 25 products, showing page 1 of 3

            // ============================================================
            // Example 2: Variable swapping without temp variable
            // ============================================================
            Console.WriteLine();
            Console.WriteLine("=== Variable Swapping ===");

            int x = 5, y = 10;
            Console.WriteLine($"Before swap: x={x}, y={y}");    // Output: Before swap: x=5, y=10
            (x, y) = (y, x);  // Single line swap!
            Console.WriteLine($"After swap: x={x}, y={y}");    // Output: After swap: x=10, y=5

            // Swapping strings
            string first = "Hello", second = "World";
            (first, second) = (second, first);
            Console.WriteLine($"{first} {second}");    // Output: World Hello

            // ============================================================
            // Example 3: LINQ with tuples for grouping and aggregation
            // ============================================================
            Console.WriteLine();
            Console.WriteLine("=== LINQ with Tuples ===");

            var sales = new[]
            {
                new { Product = "Laptop", Category = "Electronics", Amount = 1200, Region = "North" },
                new { Product = "Mouse", Category = "Electronics", Amount = 25, Region = "South" },
                new { Product = "Chair", Category = "Furniture", Amount = 300, Region = "North" },
                new { Product = "Desk", Category = "Furniture", Amount = 500, Region = "South" },
                new { Product = "Monitor", Category = "Electronics", Amount = 400, Region = "North" },
                new { Product = "Keyboard", Category = "Electronics", Amount = 80, Region = "South" }
            };

            // Group by category and sum amounts using tuples
            var categorySales = sales
                .GroupBy(s => s.Category)
                .Select(g => (Category: g.Key, Total: g.Sum(s => s.Amount), Count: g.Count()))
                .OrderByDescending(c => c.Total);

            foreach (var cat in categorySales)
            {
                Console.WriteLine($"{cat.Category}: ${cat.Total:N0} ({cat.Count} items)");
                // Output:
                // Electronics: $1,705 (4 items)
                // Furniture: $800 (2 items)
            }

            // Region-wise analysis with tuple projection
            var regionStats = sales
                .GroupBy(s => s.Region)
                .Select(g => (
                    Region: g.Key,
                    TotalRevenue: g.Sum(s => s.Amount),
                    AverageSale: g.Average(s => s.Amount),
                    TopProduct: g.OrderByDescending(s => s.Amount).First().Product
                ));

            foreach (var region in regionStats)
            {
                Console.WriteLine($"{region.Region}: ${region.TotalRevenue:N0} total, avg ${region.AverageSale:N2}, top: {region.TopProduct}");
                // Output:
                // North: $1,900 total, avg $633.33, top: Laptop
                // South: $605 total, avg $151.25, top: Desk
            }

            // ============================================================
            // Example 4: Tuple in data transformation (ETL pipeline)
            // ============================================================
            Console.WriteLine();
            Console.WriteLine("=== Data Transformation ===");

            var rawData = new[]
            {
                "2024-01-15|Electronics|Laptop|1200",
                "2024-01-16|Furniture|Chair|300",
                "2024-01-17|Electronics|Mouse|25"
            };

            var parsedData = rawData.Select(line =>
            {
                var parts = line.Split('|');
                return (Date: DateTime.Parse(parts[0]), Category: parts[1], Product: parts[2], Amount: decimal.Parse(parts[3]));
            }).ToList();

            foreach (var row in parsedData)
            {
                Console.WriteLine($"{row.Date:yyyy-MM-dd} | {row.Category,-12} | {row.Product,-10} | ${row.Amount}");
                // Output:
                // 2024-01-15 | Electronics  | Laptop     | $1200
                // 2024-01-16 | Furniture    | Chair      | $300
                // 2024-01-17 | Electronics  | Mouse      | $25
            }

            // ============================================================
            // Example 5: Dictionary with composite keys
            // ============================================================
            Console.WriteLine();
            Console.WriteLine("=== Composite Keys ===");

            var priceLookup = new Dictionary<(string Product, string Region), decimal>
            {
                { ("Laptop", "North"), 1200m },
                { ("Laptop", "South"), 1150m },
                { ("Mouse", "North"), 25m },
                { ("Mouse", "South"), 22m }
            };

            var key = (Product: "Laptop", Region: "North");
            Console.WriteLine($"Price in {key.Product} for {key.Region}: ${priceLookup[key]}");    // Output: Price in Laptop for North: $1200

            // ============================================================
            // Example 6: Async/await with tuples (simulated)
            // ============================================================
            Console.WriteLine();
            Console.WriteLine("=== Multiple Async Results ===");

            var userTask = GetUserAsync(1);
            var profileTask = GetProfileAsync(1);
            
            // In real async code, you'd use Task.WhenAll
            // Simulating synchronous access for demo
            var (userId, username, email) = GetUserAsync(1);
            var (profileId, bio, avatar) = GetProfileAsync(1);

            Console.WriteLine($"User: {username} ({email})");
            Console.WriteLine($"Profile: {bio}");
            Console.WriteLine($"Avatar: {avatar}");
            // Output:
            // User: john_doe (john@example.com)
            // Profile: Software developer and tech enthusiast
            // Avatar: /avatars/john_doe.png

            // ============================================================
            // Example 7: Method with multiple out parameters using tuple
            // ============================================================
            Console.WriteLine();
            Console.WriteLine("=== Parsing and Validation ===");

            var parseResult = TryParseDateTime("2024-12-25");
            if (parseResult.Success)
            {
                Console.WriteLine($"Parsed: {parseResult.Value:yyyy-MM-dd}");    // Output: Parsed: 2024-12-25
            }
            else
            {
                Console.WriteLine($"Error: {parseResult.Error}");
            }

            // Invalid date
            var badParse = TryParseDateTime("not-a-date");
            Console.WriteLine(badParse.Success ? "Success" : $"Failed: {badParse.Error}");    // Output: Failed: Invalid date format

            // ============================================================
            // Example 8: Tuple for caching with composite key
            // ============================================================
            Console.WriteLine();
            Console.WriteLine("=== Caching with Tuples ===");

            var cache = new Dictionary<(string UserId, string CacheKey), (DateTime Timestamp, object Data)>();
            
            void CacheSet(string userId, string key, object data)
            {
                cache[(userId, key)] = (DateTime.Now, data);
            }
            
            (bool Found, object Data) CacheGet(string userId, string key)
            {
                var cacheKey = (userId, key);
                if (cache.TryGetValue(cacheKey, out var entry))
                {
                    return (true, entry.Data);
                }
                return (false, null);
            }

            CacheSet("user123", "profile", "User Profile Data");
            CacheSet("user123", "settings", "User Settings Data");
            
            var profile = CacheGet("user123", "profile");
            var settings = CacheGet("user123", "settings");
            var invalid = CacheGet("user999", "profile");

            Console.WriteLine($"Profile cached: {profile.Found}");    // Output: Profile cached: True
            Console.WriteLine($"Settings cached: {settings.Found}");    // Output: Settings cached: True
            Console.WriteLine($"Invalid cached: {invalid.Found}");    // Output: Invalid cached: False

            // ============================================================
            // Example 9: Sorting by multiple fields
            // ============================================================
            Console.WriteLine();
            Console.WriteLine("=== Multi-field Sorting ===");

            var employees = new[]
            {
                (Name: "Charlie", Department: "IT", Salary: 70000),
                (Name: "Alice", Department: "HR", Salary: 65000),
                (Name: "Bob", Department: "IT", Salary: 75000),
                (Name: "Diana", Department: "HR", Salary: 70000)
            };

            // Sort by Department, then by Salary descending
            var sorted = employees
                .OrderBy(e => e.Department)
                .ThenByDescending(e => e.Salary)
                .ThenBy(e => e.Name);

            Console.WriteLine("Sorted employees:");
            foreach (var emp in sorted)
            {
                Console.WriteLine($"  {emp.Department}: {emp.Name} - ${emp.Salary:N0}");
                // Output:
                //   HR: Diana - $70,000
                //   HR: Alice - $65,000
                //   IT: Bob - $75,000
                //   IT: Charlie - $70,000
            }
        }

        // ============================================================
        // Method returning multiple values using ValueTuple
        // ============================================================
        static (bool IsValid, List<string> Errors, List<string> Warnings) ValidateUserInput(string email, string password)
        {
            var errors = new List<string>();
            var warnings = new List<string>();

            // Email validation
            if (string.IsNullOrEmpty(email) || !email.Contains('@'))
            {
                errors.Add("Invalid email address");
            }

            // Password validation
            if (string.IsNullOrEmpty(password) || password.Length < 8)
            {
                errors.Add("Password must be at least 8 characters");
            }

            // Warnings (not critical)
            if (password.Length < 12)
            {
                warnings.Add("Consider using a longer password");
            }

            return (errors.Count == 0, errors, warnings);
        }

        // Search with pagination info
        static (List<string> Products, int TotalCount, int PageNumber, int PageSize, int TotalPages) SearchProducts(string query)
        {
            // Simulated search
            var allProducts = new List<string> { "Laptop", "Gaming Laptop", "Laptop Bag", "Laptop Stand", "Laptop Sleeve" };
            var matches = allProducts.Where(p => p.ToLower().Contains(query.ToLower())).ToList();
            
            int pageSize = 10;
            int totalCount = matches.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            
            return (matches, totalCount, 1, pageSize, totalPages);
        }

        // Simulated async methods (returning ValueTuple instead of custom objects)
        static (int UserId, string Username, string Email) GetUserAsync(int userId)
        {
            return (1, "john_doe", "john@example.com");
        }

        static (int ProfileId, string Bio, string Avatar) GetProfileAsync(int userId)
        {
            return (1, "Software developer and tech enthusiast", "/avatars/john_doe.png");
        }

        // TryParse pattern with ValueTuple
        static (bool Success, DateTime Value, string Error) TryParseDateTime(string input)
        {
            if (DateTime.TryParse(input, out var result))
            {
                return (true, result, null);
            }
            return (false, default(DateTime), "Invalid date format");
        }
    }
}