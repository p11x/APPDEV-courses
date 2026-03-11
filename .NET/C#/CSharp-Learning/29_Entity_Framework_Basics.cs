/*
================================================================================
TOPIC 29: ENTITY FRAMEWORK BASICS
================================================================================

Entity Framework is an ORM (Object-Relational Mapper) for database operations.

TABLE OF CONTENTS:
1. What is EF?
2. DbContext
3. DbSet
4. CRUD Operations
5. LINQ with EF
================================================================================
*/

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

// Note: Requires NuGet package: Microsoft.EntityFrameworkCore

namespace EntityFrameworkExamples
{
    // Entity class (maps to table)
    public class Product
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public decimal Price { get; set; }
        
        public string Category { get; set; }
    }
    
    // DbContext (database connection)
    public class AppDbContext
    {
        public System.Collections.Generic.List<Product> Products { get; set; }
        
        // Simulated database for demo
        public AppDbContext()
        {
            Products = new System.Collections.Generic.List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 999.99m, Category = "Electronics" },
                new Product { Id = 2, Name = "Phone", Price = 699.99m, Category = "Electronics" },
                new Product { Id = 3, Name = "Book", Price = 19.99m, Category = "Books" }
            };
        }
    }
    
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Entity Framework Basics ===");
            
            using var context = new AppDbContext();
            
            // READ - Query all
            Console.WriteLine("\n--- All Products ---");
            foreach (var p in context.Products)
            {
                Console.WriteLine($"{p.Id}: {p.Name} - ${p.Price}");
            }
            
            // READ - Query with LINQ
            Console.WriteLine("\n--- Electronics ---");
            var electronics = context.Products
                .Where(p => p.Category == "Electronics");
            
            foreach (var p in electronics)
            {
                Console.WriteLine($"{p.Name}: ${p.Price}");
            }
            
            // CREATE
            Console.WriteLine("\n--- Adding Product ---");
            var newProduct = new Product 
            { 
                Name = "Tablet", 
                Price = 499.99m, 
                Category = "Electronics" 
            };
            context.Products.Add(newProduct);
            Console.WriteLine("Added: Tablet");
            
            // UPDATE
            Console.WriteLine("\n--- Updating Product ---");
            var product = context.Products.FirstOrDefault(p => p.Name == "Laptop");
            if (product != null)
            {
                product.Price = 1099.99m;
                Console.WriteLine("Updated: Laptop price to $1099.99");
            }
            
            // DELETE
            Console.WriteLine("\n--- Deleting Product ---");
            var toDelete = context.Products.FirstOrDefault(p => p.Name == "Phone");
            if (toDelete != null)
            {
                context.Products.Remove(toDelete);
                Console.WriteLine("Deleted: Phone");
            }
        }
    }
}

/*
ENTITY FRAMEWORK:
-----------------
- ORM for .NET
- Code First approach
- LINQ for queries
- CRUD operations
- Migrations

KEY CLASSES:
------------
DbContext     - Database connection
DbSet<T>      - Table representation
Entity        - Row in table
Migration     - Database schema changes
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 30 covers Best Practices.
*/
