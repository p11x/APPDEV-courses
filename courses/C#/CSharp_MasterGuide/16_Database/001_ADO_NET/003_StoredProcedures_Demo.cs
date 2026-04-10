/*
 * ============================================================
 * TOPIC     : Database
 * SUBTOPIC  : Stored Procedures
 * FILE      : StoredProcedures_Demo.cs
 * PURPOSE   : Working with stored procedures
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._16_Database._01_ADO_NET
{
    /// <summary>
    /// Stored procedures demonstration
    /// </summary>
    public class StoredProceduresDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Stored Procedures Demo ===\n");

            // Output: --- Basic Execute ---
            Console.WriteLine("--- Basic Execute ---");

            ExecuteStoredProc("sp_GetUsers");
            // Output: Executing: sp_GetUsers

            // Output: --- With Parameters ---
            Console.WriteLine("\n--- With Parameters ---");

            ExecuteWithParams("sp_GetUser", 1);
            // Output: Executing: sp_GetUser with @Id=1

            // Output: --- Output Parameters ---
            Console.WriteLine("\n--- Output Parameters ---");

            var total = ExecuteOutputParam("sp_GetCount");
            Console.WriteLine($"   Total: {total}");
            // Output: Total: 100

            // Output: --- Return Values ---
            Console.WriteLine("\n--- Return Values ---");

            var result = ExecuteReturnValue("sp_InsertUser", "Alice");
            Console.WriteLine($"   Result: {result}");
            // Output: Result: Success

            // Output: --- Multiple Result Sets ---
            Console.WriteLine("\n--- Multiple Result Sets ---");

            ExecuteMultipleResults("sp_GetUsersWithOrders");
            // Output: Resultset 1: Users
            // Output: Resultset 2: Orders

            Console.WriteLine("\n=== Stored Procedures Complete ===");
        }
    }

    /// <summary>
    /// Execute stored procedure
    /// </summary>
    public static void ExecuteStoredProc(string procName)
    {
        Console.WriteLine($"   Executing: {procName}");
    }

    /// <summary>
    /// Execute with parameters
    /// </summary>
    public static void ExecuteWithParams(string procName, int id)
    {
        Console.WriteLine($"   Executing: {procName} with @Id={id}");
    }

    /// <summary>
    /// Execute with output parameter
    /// </summary>
    public static int ExecuteOutputParam(string procName)
    {
        return 100;
    }

    /// <summary>
    /// Execute with return value
    /// </summary>
    public static string ExecuteReturnValue(string procName, string name)
    {
        return "Success";
    }

    /// <summary>
    /// Execute with multiple result sets
    /// </summary>
    public static void ExecuteMultipleResults(string procName)
    {
        Console.WriteLine("   Resultset 1: Users");
        Console.WriteLine("   Resultset 2: Orders");
    }
}