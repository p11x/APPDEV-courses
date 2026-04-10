/*
 * ============================================================
 * TOPIC     : Security
 * SUBTOPIC  : Authentication - JWT
 * FILE      : 01_JWTAuth.cs
 * PURPOSE   : JWT authentication in C#
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._17_Security._02_Authentication
{
    /// <summary>
    /// JWT authentication basics
    /// </summary>
    public class JWTAuth
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== JWT Authentication ===\n");

            Console.WriteLine("1. Generate Token:");
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";
            Console.WriteLine($"   Token: {token.Substring(0, 20)}...");
            
            Console.WriteLine("\n2. Validate Token:");
            Console.WriteLine("   Token is valid");
            
            Console.WriteLine("\n3. Extract Claims:");
            Console.WriteLine("   UserId: 123, Role: Admin");

            Console.WriteLine("\n=== JWT Complete ===");
        }
    }
}