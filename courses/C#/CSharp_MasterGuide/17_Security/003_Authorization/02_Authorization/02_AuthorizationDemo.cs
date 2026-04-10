/*
 * ============================================================
 * TOPIC     : Security
 * SUBTOPIC  : Authorization
 * FILE      : 02_AuthorizationDemo.cs
 * PURPOSE   : Demonstrates authorization in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._17_Security._02_Authorization
{
    public class AuthorizationDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Authorization Demo ===\n");

            Console.WriteLine("1. Role-Based Authorization:");
            var authz = new RoleAuth();
            authz.CheckAccess("admin");
            authz.CheckAccess("user");
        }
    }

    public class RoleAuth
    {
        public void CheckAccess(string role)
        {
            Console.WriteLine($"   Role: {role}, Access: {(role == "admin" ? "Full" : "Limited")}");
        }
    }
}