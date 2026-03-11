/*
================================================================================
TOPIC 34: ENTITY FRAMEWORK CORE
================================================================================

Entity Framework Core is the cross-platform version of Entity Framework.

TABLE OF CONTENTS:
1. What is EF Core?
2. DbContext
3. DbSet
4. Migrations
5. Relationships
================================================================================
*/

namespace EFCoreConcepts
{
    // ====================================================================
    // ENTITY CLASS
    // ====================================================================
    
    // Example entity:
    /*
    public class Product
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        public int CategoryId { get; set; }
        
        // Navigation property
        public Category Category { get; set; }
    }
    
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        // Collection navigation
        public ICollection<Product> Products { get; set; }
    }
    */
    
    // ====================================================================
    // DB CONTEXT
    // ====================================================================
    
    // Example DbContext:
    /*
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("connection-string");
            // Or UseSqlite("connection-string");
            // Or UseInMemoryDatabase("database-name");
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API configuration
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
        }
    }
    */
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Entity Framework Core ===");
            
            // Install: Microsoft.EntityFrameworkCore.SqlServer
            // Install: Microsoft.EntityFrameworkCore.Tools
            
            Console.WriteLine("\nNuGet Packages:");
            Console.WriteLine("Microsoft.EntityFrameworkCore.SqlServer - SQL Server");
            Console.WriteLine("Microsoft.EntityFrameworkCore.Sqlite - SQLite");
            Console.WriteLine("Microsoft.EntityFrameworkCore.InMemory - In-memory");
            
            Console.WriteLine("\nCLI Commands:");
            Console.WriteLine("dotnet ef migrations add InitialCreate");
            Console.WriteLine("dotnet ef database update");
            
            Console.WriteLine("\nProviders:");
            Console.WriteLine("SQL Server, SQLite, PostgreSQL, MySQL, Oracle");
        }
    }
}

/*
EF CORE FEATURES:
-----------------
- Cross-platform
- No object tracking optimization
- Batch operations
- Shadow properties
- Field mapping
- Better performance

MIGRATIONS:
-----------
1. Add-Migration MigrationName
2. Update-Database
3. Remove-Migration
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 35 covers Blazor.
*/
