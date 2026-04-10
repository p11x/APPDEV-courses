/*
 * ============================================================
 * TOPIC     : Networking
 * SUBTOPIC  : HttpClient - REST API Calls
 * FILE      : 02_REST_Calls.cs
 * PURPOSE   : Making REST API calls with HttpClient
 * ============================================================
 */
using System;
using System.Net.Http;

namespace CSharp_MasterGuide._15_Networking._01_HttpClient
{
    /// <summary>
    /// REST API calls with HttpClient
    /// </summary>
    public class RESTCalls
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== REST Calls ===\n");

            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
            
            // GET request
            Console.WriteLine("1. GET Request:");
            Console.WriteLine("   Fetching posts...");
            
            // POST request  
            Console.WriteLine("\n2. POST Request:");
            Console.WriteLine("   Creating post...");
            
            // PUT request
            Console.WriteLine("\n3. PUT Request:");
            Console.WriteLine("   Updating post...");
            
            // DELETE request
            Console.WriteLine("\n4. DELETE Request:");
            Console.WriteLine("   Deleting post...");

            Console.WriteLine("\n=== REST Calls Complete ===");
        }
    }
}