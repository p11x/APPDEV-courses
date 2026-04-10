/*
 * ============================================================
 * TOPIC     : Networking
 * SUBTOPIC  : REST API
 * FILE      : 03_RESTAPIDemo.cs
 * PURPOSE   : Demonstrates REST API concepts in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._15_Networking._03_RESTAPI
{
    /// <summary>
    /// Demonstrates REST API
    /// </summary>
    public class RESTAPIDemo
    {
        /// <summary>
        /// Entry point for REST API examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === REST API Demo ===
            Console.WriteLine("=== REST API Demo ===\n");

            // ── CONCEPT: REST Principles ───────────────────────────────────────
            // Resource-based, stateless, uniform interface

            // Example 1: REST Endpoints
            // Output: 1. REST Endpoints:
            Console.WriteLine("1. REST Endpoints:");
            
            var api = new MockRestAPI();
            
            // GET - retrieve
            var users = api.GetUsers();
            // Output: GET /users - 200 OK
            
            // GET single
            var user = api.GetUser(1);
            // Output: GET /users/1 - 200 OK
            
            // POST - create
            var created = api.CreateUser("John");
            // Output: POST /users - 201 Created
            
            // PUT - update
            var updated = api.UpdateUser(1, "Jane");
            // Output: PUT /users/1 - 200 OK
            
            // DELETE - remove
            var deleted = api.DeleteUser(1);
            // Output: DELETE /users/1 - 204 No Content

            Console.WriteLine("\n=== REST API Complete ===");
        }
    }

    /// <summary>
    /// Mock REST API
    /// </summary>
    public class MockRestAPI
    {
        public string GetUsers() { Console.WriteLine("   GET /users - 200 OK"); return "[]"; }
        public string GetUser(int id) { Console.WriteLine($"   GET /users/{id} - 200 OK"); return "{}"; }
        public string CreateUser(string name) { Console.WriteLine("   POST /users - 201 Created"); return "{}"; }
        public string UpdateUser(int id, string name) { Console.WriteLine($"   PUT /users/{id} - 200 OK"); return "{}"; }
        public string DeleteUser(int id) { Console.WriteLine($"   DELETE /users/{id} - 204 No Content"); return ""; }
    }
}