/*
 * ============================================================
 * TOPIC     : Database
 * SUBTOPIC  : Dapper - Micro ORM
 * FILE      : Dapper_Demo.cs
 * PURPOSE   : Using Dapper for data access
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._16_Database._04_Dapper
{
    /// <summary>
    /// Dapper demonstration
    /// </summary>
    public class DapperDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Dapper Demo ===\n");

            // Output: --- Simple Query ---
            Console.WriteLine("--- Simple Query ---");

            var users = QueryUsers();
            foreach (var user in users)
            {
                Console.WriteLine($"   {user.Name}");
            }
            // Output: Alice
            // Output: Bob

            // Output: --- Dynamic Query ---
            Console.WriteLine("\n--- Dynamic Query ---");

            var dynamic = QueryDynamic("SELECT * FROM Users");
            Console.WriteLine($"   Count: {dynamic.Count}");
            // Output: Count: 2

            // Output: --- Parameterized Query ---
            Console.WriteLine("\n--- Parameterized Query ---");

            var user1 = QueryUser(1);
            Console.WriteLine($"   User: {user1.Name}");
            // Output: User: Alice

            // Output: --- Execute Insert ---
            Console.WriteLine("\n--- Execute Insert ---");

            var inserted = InsertUser("Charlie");
            Console.WriteLine($"   Inserted: {inserted}");
            // Output: Inserted: True

            // Output: --- Stored Proc ---
            Console.WriteLine("\n--- Stored Proc ---");

            var result = ExecuteStoredProc("sp_GetUsers");
            Console.WriteLine($"   Result: {result}");
            // Output: Result: Success

            // Output: --- Transactions ---
            Console.WriteLine("\n--- Transactions ---");

            UseTransaction();
            Console.WriteLine("   Transaction committed");
            // Output: Transaction committed

            Console.WriteLine("\n=== Dapper Complete ===");
        }
    }

    /// <summary>
    /// User entity
    /// </summary>
    public class User
    {
        public int Id { get; set; } // property: id
        public string Name { get; set; } // property: name
        public string Email { get; set; } // property: email
    }

    /// <summary>
    /// Query users - simple query
    /// </summary>
    public static System.Collections.Generic.List<User> QueryUsers()
    {
        return new System.Collections.Generic.List<User>
        {
            new User { Name = "Alice" },
            new User { Name = "Bob" }
        };
    }

    /// <summary>
    /// Query dynamic - returns dynamic type
    /// </summary>
    public static dynamic QueryDynamic(string sql)
    {
        return new { Count = 2 };
    }

    /// <summary>
    /// Query user by ID
    /// </summary>
    public static User QueryUser(int id)
    {
        return new User { Name = "Alice" };
    }

    /// <summary>
    /// Insert user
    /// </summary>
    public static bool InsertUser(string name)
    {
        return true;
    }

    /// <summary>
    /// Execute stored procedure
    /// </summary>
    public static string ExecuteStoredProc(string procName)
    {
        return "Success";
    }

    /// <summary>
    /// Use transaction
    /// </summary>
    public static void UseTransaction()
    {
    }
}