/*
 * ============================================================
 * TOPIC     : Database
 * SUBTOPIC  : Entity Framework Core - Setup
 * FILE      : 01_EFCore_Setup.cs
 * PURPOSE   : Entity Framework Core configuration
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._16_Database._02_EntityFrameworkCore
{
    /// <summary>
    /// EF Core setup basics
    /// </summary>
    public class EFCoreSetup
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== EF Core Setup ===\n");

            Console.WriteLine("1. Install Package:");
            Console.WriteLine("   Microsoft.EntityFrameworkCore");
            
            Console.WriteLine("\n2. Create DbContext:");
            Console.WriteLine("   Defining data context...");
            
            Console.WriteLine("\n3. Configure Connection:");
            Console.WriteLine("   Setting up connection string...");
            
            Console.WriteLine("\n4. Add to DI Container:");
            Console.WriteLine("   Registering services...");

            Console.WriteLine("\n=== EF Core Setup Complete ===");
        }
    }
}