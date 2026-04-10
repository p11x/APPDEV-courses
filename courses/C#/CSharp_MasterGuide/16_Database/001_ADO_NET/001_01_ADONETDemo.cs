/*
 * ============================================================
 * TOPIC     : Database
 * SUBTOPIC  : ADO.NET Basics
 * FILE      : 01_ADONETDemo.cs
 * PURPOSE   : Demonstrates ADO.NET database access in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._16_Database._01_ADO.NET
{
    /// <summary>
    /// Demonstrates ADO.NET
    /// </summary>
    public class ADONETDemo
    {
        /// <summary>
        /// Entry point for ADO.NET examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === ADO.NET Demo ===
            Console.WriteLine("=== ADO.NET Demo ===\n");

            // ── CONCEPT: Connection ───────────────────────────────────────────
            // Database connection management

            // Example 1: Connection
            // Output: 1. Connection:
            Console.WriteLine("1. Connection:");
            
            var connection = new DbConnection("Server=localhost;Database=TestDB");
            connection.Open();
            // Output: Connected to: Server=localhost;Database=TestDB
            Console.WriteLine($"   Connected to: {connection.ConnectionString}");
            connection.Close();
            // Output: Connection closed

            // ── CONCEPT: Command ───────────────────────────────────────────────
            // Execute SQL commands

            // Example 2: Command
            // Output: 2. Command:
            Console.WriteLine("\n2. Command:");
            
            var cmd = new DbCommand(connection, "SELECT * FROM Users");
            var result = cmd.Execute();
            // Output: Executed: SELECT * FROM Users
            // Output: Rows returned: 3

            // ── CONCEPT: DataReader ─────────────────────────────────────────────
            // Fast forward-only reading

            // Example 3: DataReader
            // Output: 3. DataReader:
            Console.WriteLine("\n3. DataReader:");
            
            var reader = new DataReader();
            while (reader.Read())
            {
                // Output: Row: id=1, name=John
                Console.WriteLine($"   Row: id={reader["id"]}, name={reader["name"]}");
            }

            Console.WriteLine("\n=== ADO.NET Complete ===");
        }
    }

    /// <summary>
    /// Mock database connection
    /// </summary>
    public class DbConnection
    {
        public string ConnectionString { get; }
        
        public DbConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }
        
        public void Open() { }
        public void Close() { }
    }

    /// <summary>
    /// Mock database command
    /// </summary>
    public class DbCommand
    {
        private DbConnection _connection;
        private string _sql;
        
        public DbCommand(DbConnection connection, string sql)
        {
            _connection = connection;
            _sql = sql;
        }
        
        public string Execute()
        {
            Console.WriteLine($"   Executed: {_sql}");
            return "[]";
        }
    }

    /// <summary>
    /// Mock data reader
    /// </summary>
    public class DataReader
    {
        private int _index;
        
        public object this[string column] => _index switch
        {
            0 => new { id = 1, name = "John" },
            _ => null
        };
        
        public bool Read()
        {
            if (_index < 1) { _index++; return true; }
            return false;
        }
    }
}