/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Proxy Part 2
 * FILE      : 10_Proxy_Part2.cs
 * PURPOSE   : Demonstrates advanced Proxy patterns in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural._03_Proxy
{
    /// <summary>
    /// Advanced Proxy patterns
    /// </summary>
    public class ProxyAdvanced
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Proxy Advanced ===\n");

            Console.WriteLine("1. Remote Proxy Simulation:");
            var remoteProxy = new RemoteServiceProxy();
            var data = remoteProxy.GetData("user1");
            // Output: Request sent to remote server
            // Output: Data received: user data

            Console.WriteLine("\n2. Protection Proxy:");
            var protectedProxy = new ProtectedResourceProxy("admin");
            protectedProxy.Access();
            // Output: Access granted for admin

            Console.WriteLine("\n=== Proxy Advanced Complete ===");
        }
    }

    public interface IRemoteService
    {
        string GetData(string request);
    }

    public class RemoteServiceProxy : IRemoteService
    {
        public string GetData(string request)
        {
            Console.WriteLine($"   Request sent to remote server: {request}");
            return "user data";
        }
    }

    public interface IProtectedResource
    {
        void Access();
    }

    public class ProtectedResourceProxy : IProtectedResource
    {
        private string _userRole;
        
        public ProtectedResourceProxy(string userRole) => _userRole = userRole;
        
        public void Access()
        {
            if (_userRole == "admin")
                Console.WriteLine("   Access granted for admin");
            else
                Console.WriteLine("   Access denied");
        }
    }
}