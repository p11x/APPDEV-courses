/*
 * ============================================================
 * TOPIC     : Networking
 * SUBTOPIC  : HTTP Client
 * FILE      : 01_HttpClientDemo.cs
 * PURPOSE   : Demonstrates HttpClient for HTTP requests in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._15_Networking._01_HttpClient
{
    /// <summary>
    /// Demonstrates HttpClient usage
    /// </summary>
    public class HttpClientDemo
    {
        /// <summary>
        /// Entry point for HttpClient examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === HttpClient Demo ===
            Console.WriteLine("=== HttpClient Demo ===\n");

            // ── CONCEPT: GET Request ──────────────────────────────────────────
            // Fetching data from API

            // Example 1: GET Request
            // Output: 1. GET Request:
            Console.WriteLine("1. GET Request:");
            
            var client = new MockHttpClient();
            var response = client.Get("https://api.example.com/users");
            // Output: GET https://api.example.com/users
            // Output: Status: 200 OK
            Console.WriteLine($"   Status: {response.StatusCode}");

            // ── CONCEPT: POST Request ─────────────────────────────────────────
            // Sending data to server

            // Example 2: POST Request
            // Output: 2. POST Request:
            Console.WriteLine("\n2. POST Request:");
            
            var postResponse = client.Post("https://api.example.com/users", "{\"name\":\"John\"}");
            // Output: POST https://api.example.com/users
            // Output: Created: 201
            Console.WriteLine($"   Created: {postResponse.StatusCode}");

            // ── CONCEPT: Async Requests ───────────────────────────────────────
            // Non-blocking HTTP calls

            // Example 3: Async Requests
            // Output: 3. Async Requests:
            Console.WriteLine("\n3. Async Requests:");
            
            var asyncClient = new AsyncHttpClient();
            asyncClient.FetchDataAsync();
            // Output: Fetching data asynchronously...

            Console.WriteLine("\n=== HttpClient Complete ===");
        }
    }

    /// <summary>
    /// Mock HTTP client
    /// </summary>
    public class MockHttpClient
    {
        public HttpResponse Get(string url)
        {
            Console.WriteLine($"   GET {url}");
            return new HttpResponse { StatusCode = "200 OK", Body = "[]" };
        }
        
        public HttpResponse Post(string url, string body)
        {
            Console.WriteLine($"   POST {url}");
            return new HttpResponse { StatusCode = "201 Created", Body = "{}" };
        }
    }

    /// <summary>
    /// HTTP response
    /// </summary>
    public class HttpResponse
    {
        public string StatusCode { get; set; } // property: HTTP status
        public string Body { get; set; } // property: response body
    }

    /// <summary>
    /// Async HTTP client
    /// </summary>
    public class AsyncHttpClient
    {
        public async void FetchDataAsync()
        {
            Console.WriteLine("   Fetching data asynchronously...");
        }
    }
}