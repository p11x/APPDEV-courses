/*
 * ============================================================
 * TOPIC     : Security
 * SUBTOPIC  : Authorization - Role-Based
 * FILE      : 01_RoleBased.cs
 * PURPOSE   : Role-based access control
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._17_Security._03_Authorization
{
    /// <summary>
    /// Role-based authorization
    /// </summary>
    public class RoleBasedAuth
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Role-Based Auth ===\n");

            Console.WriteLine("1. Roles:");
            Console.WriteLine("   Admin, User, Guest");
            
            Console.WriteLine("\n2. Authorization:");
            Console.WriteLine("   Check user role before access");
            
            Console.WriteLine("\n3. Claims:");
            Console.WriteLine("   Additional user information");

            Console.WriteLine("\n=== Role-Based Complete ===");
        }
    }
}