/*
 * ============================================================
 * TOPIC     : Networking
 * SUBTOPIC  : HttpClient - Basics Part 2
 * FILE      : 02_HttpClientBasics_Part2.cs
 * PURPOSE   : Advanced HttpClient features
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._15_Networking._01_HttpClient
{
    /// <summary>
    /// Advanced HttpClient
    /// </summary>
    public class HttpClientBasicsPart2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== HttpClient Part 2 ===\n");

            Console.WriteLine("1. HttpClientFactory:");
            Console.WriteLine("   Managed HTTP client lifecycle");
            
            Console.WriteLine("\n2. HttpMessageHandler:");
            Console.WriteLine("   Custom message handlers");
            
            Console.WriteLine("\n3. Polly Integration:");
            Console.WriteLine("   Retry, circuit breaker patterns");

            Console.WriteLine("\n=== HttpClient Part 2 Complete ===");
        }
    }
}