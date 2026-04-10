/*
 * ============================================================
 * TOPIC     : Networking
 * SUBTOPIC  : GraphQL
 * FILE      : 04_GraphQLDemo.cs
 * PURPOSE   : Demonstrates GraphQL concepts in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._15_Networking._04_GraphQL
{
    /// <summary>
    /// Demonstrates GraphQL
    /// </summary>
    public class GraphQLDemo
    {
        /// <summary>
        /// Entry point for GraphQL examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === GraphQL Demo ===
            Console.WriteLine("=== GraphQL Demo ===\n");

            // ── CONCEPT: GraphQL vs REST ───────────────────────────────────────
            // Ask for exactly what you need

            // Example 1: Query
            // Output: 1. GraphQL Query:
            Console.WriteLine("1. GraphQL Query:");
            
            var client = new GraphQLClient();
            
            var query = @"
                query {
                    user(id: 1) {
                        name
                        email
                    }
                }";
            
            var result = client.Execute(query);
            // Output: Query: user { name, email }
            Console.WriteLine($"   Query result: {result}");

            // Example 2: Mutation
            // Output: 2. GraphQL Mutation:
            Console.WriteLine("\n2. GraphQL Mutation:");
            
            var mutation = @"
                mutation {
                    createUser(name: ""John"", email: ""john@email.com"") {
                        id
                        name
                    }
                }";
            
            var mutResult = client.Execute(mutation);
            // Output: Mutation result: id=1, name=John
            Console.WriteLine($"   Mutation result: {mutResult}");

            Console.WriteLine("\n=== GraphQL Complete ===");
        }
    }

    /// <summary>
    /// Mock GraphQL client
    /// </summary>
    public class GraphQLClient
    {
        public string Execute(string query)
        {
            if (query.Contains("mutation"))
                return "{\"id\":1,\"name\":\"John\"}";
            return "{\"user\":{\"name\":\"John\",\"email\":\"john@email.com\"}}";
        }
    }
}