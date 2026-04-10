/*
 * ============================================================
 * TOPIC     : Pattern Matching
 * SUBTOPIC  : Property Patterns (Continued)
 * FILE      : 02_PropertyPattern_Part2.cs
 * PURPOSE   : Continues property patterns with init patterns, nullable, and complex scenarios
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._09_PatternMatching._03_PropertyPatterns
{
    /// <summary>
    /// Continues property pattern demonstrations
    /// </summary>
    public class PropertyPattern_Part2
    {
        /// <summary>
        /// Entry point for advanced property patterns
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Property Pattern Part 2 ===
            Console.WriteLine("=== Property Pattern Part 2 ===\n");

            // ── CONCEPT: Init-Only Property Patterns ───────────────────────
            // Property patterns work with init-only properties

            // Example 1: Init-only properties
            // Output: 1. Init-Only Property Patterns:
            Console.WriteLine("1. Init-Only Property Patterns:");
            
            // Create init-only records
            var point1 = new Point(10, 20);
            var point2 = new Point(0, 0);
            var point3 = new Point(-5, -5);
            
            // ClassifyPoint returns point classification
            Console.WriteLine($"   (10, 20): {ClassifyPoint(point1)}");
            Console.WriteLine($"   (0, 0): {ClassifyPoint(point2)}");
            Console.WriteLine($"   (-5, -5): {ClassifyPoint(point3)}");

            // ── CONCEPT: Nullable Property Patterns ────────────────────────
            // Match on nullable properties

            // Example 2: Nullable property patterns
            // Output: 2. Nullable Property Patterns:
            Console.WriteLine("\n2. Nullable Property Patterns:");
            
            // Employee with optional Manager
            var withManager = new Employee("Alice", "Engineer", "Bob");
            var withoutManager = new Employee("Charlie", "Developer", null);
            var CEO = new Employee("Diana", "CEO", null);
            
            // GetEmployeeInfo returns info string
            Console.WriteLine($"   {withManager.Name}: {GetEmployeeInfo(withManager)}");
            Console.WriteLine($"   {withoutManager.Name}: {GetEmployeeInfo(withoutManager)}");
            Console.WriteLine($"   {CEO.Name}: {GetEmployeeInfo(CEO)}");

            // ── CONCEPT: Property Pattern with 'when' ───────────────────────
            // Add conditions to property patterns

            // Example 3: Property pattern with when clause
            // Output: 3. Property Pattern with when:
            Console.WriteLine("\n3. Property Pattern with when:");
            
            // Product with various properties
            var electronics = new Product("Laptop", "Electronics", 999.99m, true);
            var food = new Product("Apple", "Food", 1.99m, false);
            var furniture = new Product("Chair", "Furniture", 149.99m, false);
            var discounted = new Product("Phone", "Electronics", 499.99m, true);
            
            // GetProductCategory returns category
            Console.WriteLine($"   {electronics.Name}: {GetProductCategory(electronics)}");
            Console.WriteLine($"   {food.Name}: {GetProductCategory(food)}");
            Console.WriteLine($"   {furniture.Name}: {GetProductCategory(furniture)}");
            Console.WriteLine($"   {discounted.Name}: {GetProductCategory(discounted)}");

            // ── CONCEPT: Multiple Property Matching ─────────────────────────
            // Match on multiple properties simultaneously

            // Example 4: Multiple property matching
            // Output: 4. Multiple Property Matching:
            Console.WriteLine("\n4. Multiple Property Matching:");
            
            // Different user accounts
            var admin = new UserAccount("admin", "admin@co.com", true, true);
            var regular = new UserAccount("user", "user@co.com", false, true);
            var inactive = new UserAccount("inactive", "inactive@co.com", false, false);
            var locked = new UserAccount("locked", "locked@co.com", true, false);
            
            // GetAccountStatus returns status
            Console.WriteLine($"   {admin.Username}: {GetAccountStatus(admin)}");
            Console.WriteLine($"   {regular.Username}: {GetAccountStatus(regular)}");
            Console.WriteLine($"   {inactive.Username}: {GetAccountStatus(inactive)}");
            Console.WriteLine($"   {locked.Username}: {GetAccountStatus(locked)}");

            // ── REAL-WORLD EXAMPLE: Shipping Classifier ─────────────────────
            // Output: --- Real-World: Shipping Classifier ---
            Console.WriteLine("\n--- Real-World: Shipping Classifier ---");
            
            // Classify packages for shipping
            var small = new Package("PKG001", 1, 10, "standard");
            var large = new Package("PKG002", 20, 50, "standard");
            var heavy = new Package("PKG003", 15, 200, "standard");
            var expedited = new Package("PKG004", 5, 30, "express");
            
            // ClassifyPackage returns shipping class
            Console.WriteLine($"   {small.Id}: {ClassifyPackage(small)}");
            Console.WriteLine($"   {large.Id}: {ClassifyPackage(large)}");
            Console.WriteLine($"   {heavy.Id}: {ClassifyPackage(heavy)}");
            Console.WriteLine($"   {expedited.Id}: {ClassifyPackage(expedited)}");

            Console.WriteLine("\n=== Property Pattern Part 2 Complete ===");
        }

        /// <summary>
        /// Classifies point location
        /// </summary>
        public static string ClassifyPoint(Point point)
        {
            // Property pattern matching on X and Y
            return point switch
            {
                // Both zero = origin
                { X: 0, Y: 0 } => "Origin",
                
                // X is zero = on Y axis
                { X: 0, Y: _ } => "On Y-axis",
                
                // Y is zero = on X axis
                { X: _, Y: 0 } => "On X-axis",
                
                // Both positive = quadrant I
                { X: > 0, Y: > 0 } => "Quadrant I",
                
                // X negative, Y positive = quadrant II
                { X: < 0, Y: > 0 } => "Quadrant II",
                
                // Both negative = quadrant III
                { X: < 0, Y: < 0 } => "Quadrant III",
                
                // X positive, Y negative = quadrant IV
                { X: > 0, Y: < 0 } => "Quadrant IV",
                
                // Default
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Gets employee info based on properties
        /// </summary>
        public static string GetEmployeeInfo(Employee emp)
        {
            // Pattern match on Name and optional Manager
            return emp.ManagerName switch
            {
                // Manager is null and role = CEO = top of hierarchy
                null when emp.Role == "CEO" => "Executive (No Manager)",
                
                // Manager is null = no manager (entry level or executive)
                null => "No Manager",
                
                // Has manager = reports to manager
                _ => $"Reports to {emp.ManagerName}"
            };
        }

        /// <summary>
        /// Gets product category using property + when
        /// </summary>
        public static string GetProductCategory(Product product)
        {
            // Property pattern with when clause
            return product switch
            {
                // Electronics, in stock = standard electronics
                { Category: "Electronics", InStock: true } => "Available Electronics",
                
                // Electronics, out of stock = backordered
                { Category: "Electronics", InStock: false } => "Backordered Electronics",
                
                // Food with price > 10 = premium food
                { Category: "Food", Price: > 10 } => "Premium Food",
                
                // Food = regular food
                { Category: "Food", _ } => "Regular Food",
                
                // Any category with price > 500 = premium item
                { Price: > 500 } => "Premium Item",
                
                // Default = standard item
                _ => "Standard Item"
            };
        }

        /// <summary>
        /// Gets account status using multiple properties
        /// </summary>
        public static string GetAccountStatus(UserAccount account)
        {
            // Tuple pattern with multiple property checks
            return (account.IsAdmin, account.IsActive, account.Email) switch
            {
                // Admin, active, has email = full access
                (true, true, { Length: > 0 }) => "Admin (Full Access)",
                
                // Not admin, active = regular user
                (false, true, { Length: > 0 }) => "Regular User",
                
                // Any, inactive = account disabled
                (_, false, _) => "Account Disabled",
                
                // No email = unverified
                (_, _, null) => "Unverified",
                
                // No email string = unverified
                (_, _, { Length: 0 }) => "Unverified",
                
                // Default = unknown
                _ => "Unknown Status"
            };
        }

        /// <summary>
        /// Real-world: Classifies package for shipping
        /// </summary>
        public static string ClassifyPackage(Package package)
        {
            // Multiple property patterns
            return (package.WeightKg, package.ShippingCost, package.ServiceType) switch
            {
                // Very light package = letter rate
                (<= 0.5, _, _) => "Letter",
                
                // Light package, standard = small parcel
                (<= 2, < 20, "standard") => "Small Parcel",
                
                // Medium weight, express = express medium
                (>= 5 and <= 15, _, "express") => "Express Medium",
                
                // Heavy package = freight
                (> 15, _, _) => "Freight",
                
                // High shipping cost = valuable
                (_, > 100, _) => "Valuable",
                
                // Express service = expedited
                (_, _, "express") => "Expedited",
                
                // Default = standard parcel
                _ => "Standard Parcel"
            };
        }
    }

    // ── EXAMPLE CLASSES ───────────────────────────────────────────────────
    /// <summary>
    /// Point with init-only properties
    /// </summary>
    public class Point
    {
        public int X { get; init; }
        public int Y { get; init; }
        
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// Employee with optional manager
    /// </summary>
    public class Employee
    {
        public string Name { get; }
        public string Role { get; }
        public string? ManagerName { get; }
        
        public Employee(string name, string role, string? manager)
        {
            Name = name;
            Role = role;
            ManagerName = manager;
        }
    }

    /// <summary>
    /// Product with multiple properties
    /// </summary>
    public class Product
    {
        public string Name { get; }
        public string Category { get; }
        public decimal Price { get; }
        public bool InStock { get; }
        
        public Product(string name, string category, decimal price, bool inStock)
        {
            Name = name;
            Category = category;
            Price = price;
            InStock = inStock;
        }
    }

    /// <summary>
    /// User account with multiple boolean properties
    /// </summary>
    public class UserAccount
    {
        public string Username { get; }
        public string Email { get; }
        public bool IsAdmin { get; }
        public bool IsActive { get; }
        
        public UserAccount(string username, string email, bool isAdmin, bool isActive)
        {
            Username = username;
            Email = email;
            IsAdmin = isAdmin;
            IsActive = isActive;
        }
    }

    /// <summary>
    /// Package for shipping classification
    /// </summary>
    public class Package
    {
        public string Id { get; }
        public double WeightKg { get; }
        public double ShippingCost { get; }
        public string ServiceType { get; }
        
        public Package(string id, double weight, double cost, string service)
        {
            Id = id;
            WeightKg = weight;
            ShippingCost = cost;
            ServiceType = service;
        }
    }
}
