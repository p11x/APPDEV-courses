/*
 * ============================================================
 * TOPIC     : Security
 * SUBTOPIC  : Real-World Security
 * FILE      : 04_Security_RealWorld.cs
 * PURPOSE   : Real-world security examples
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._17_Security._04_RealWorld
{
    public class SecurityRealWorldDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Security Real-World ===\n");

            Console.WriteLine("1. SQL Injection Prevention:");
            var sqlSafe = new SQLSafeQuery();
            sqlSafe.Execute("SELECT * FROM users WHERE id = @id", 1);

            Console.WriteLine("\n2. XSS Prevention:");
            var xssSafe = new XSSProtector();
            var safe = xssSafe.Sanitize("<script>alert('xss')</script>");
            Console.WriteLine($"   Sanitized: {safe}");

            Console.WriteLine("\n=== Security Real-World Complete ===");
        }
    }

    public class SQLSafeQuery
    {
        public void Execute(string query, int id) => Console.WriteLine($"   Executed: {query}");
    }

    public class XSSProtector
    {
        public string Sanitize(string input) => input.Replace("<", "&lt;");
    }
}