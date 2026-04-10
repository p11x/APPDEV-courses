/*
 * ============================================================
 * TOPIC     : Database
 * SUBTOPIC  : Real-World Database
 * FILE      : 04_Database_RealWorld.cs
 * PURPOSE   : Real-world database examples
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List<T>

namespace CSharp_MasterGuide._16_Database._04_RealWorld
{
    /// <summary>
    /// Real-world database examples
    /// </summary>
    public class DatabaseRealWorldDemo
    {
        /// <summary>
        /// Entry point for database real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Database Real-World ===
            Console.WriteLine("=== Database Real-World ===\n");

            // ── REAL-WORLD 1: E-commerce Database ─────────────────────────────
            // Complete data model

            // Example 1: E-commerce Model
            // Output: 1. E-commerce Database:
            Console.WriteLine("1. E-commerce Database:");
            
            var order = new Order
            {
                Id = 1001,
                CustomerId = 1,
                Total = 299.99m,
                Status = "Shipped"
            };
            
            // Output: Order: 1001, Customer: 1, Total: $299.99, Status: Shipped
            Console.WriteLine($"   Order: {order.Id}, Customer: {order.CustomerId}, Total: ${order.Total}, Status: {order.Status}");

            // ── REAL-WORLD 2: Repository Pattern ───────────────────────────────
            // Data access abstraction

            // Example 2: Repository Pattern
            // Output: 2. Repository Pattern:
            Console.WriteLine("\n2. Repository Pattern:");
            
            var userRepo = new UserRepository();
            var user = userRepo.GetById(1);
            // Output: Retrieved: John (john@email.com)
            Console.WriteLine($"   Retrieved: {user.Name} ({user.Email})");

            // ── REAL-WORLD 3: Transaction ─────────────────────────────────────
            // ACID transactions

            // Example 3: Transaction
            // Output: 3. Transaction:
            Console.WriteLine("\n3. Transaction:");
            
            var transaction = new DatabaseTransaction();
            transaction.Begin();
            try
            {
                transaction.Execute("UPDATE accounts SET balance = balance - 100 WHERE id = 1");
                transaction.Execute("UPDATE accounts SET balance = balance + 100 WHERE id = 2");
                transaction.Commit();
                // Output: Transaction committed
                Console.WriteLine("   Transaction committed");
            }
            catch
            {
                transaction.Rollback();
                // Output: Transaction rolled back
                Console.WriteLine("   Transaction rolled back");
            }

            Console.WriteLine("\n=== Database Real-World Complete ===");
        }
    }

    /// <summary>
    /// Order entity
    /// </summary>
    public class Order
    {
        public int Id { get; set; } // property: order ID
        public int CustomerId { get; set; } // property: customer ID
        public decimal Total { get; set; } // property: order total
        public string Status { get; set; } // property: order status
    }

    /// <summary>
    /// User entity
    /// </summary>
    public class UserEntity
    {
        public int Id { get; set; } // property: user ID
        public string Name { get; set; } // property: user name
        public string Email { get; set; } // property: user email
    }

    /// <summary>
    /// User repository
    /// </summary>
    public class UserRepository
    {
        private List<UserEntity> _users = new List<UserEntity>
        {
            new UserEntity { Id = 1, Name = "John", Email = "john@email.com" }
        };
        
        public UserEntity GetById(int id)
        {
            return _users.Find(u => u.Id == id);
        }
    }

    /// <summary>
    /// Database transaction
    /// </summary>
    public class DatabaseTransaction
    {
        public void Begin() => Console.WriteLine("   Transaction started");
        public void Execute(string sql) => Console.WriteLine($"   Executed: {sql}");
        public void Commit() { }
        public void Rollback() { }
    }
}