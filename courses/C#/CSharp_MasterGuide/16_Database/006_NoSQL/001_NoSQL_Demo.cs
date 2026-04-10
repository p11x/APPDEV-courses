/*
 * ============================================================
 * TOPIC     : Database
 * SUBTOPIC  : NoSQL Databases
 * FILE      : NoSQL_Demo.cs
 * PURPOSE   : Working with NoSQL databases
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._16_Database._05_NoSQL
{
    /// <summary>
    /// NoSQL demonstration
    /// </summary>
    public class NoSQLDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== NoSQL Demo ===\n");

            // Output: --- Key-Value Store ---
            Console.WriteLine("--- Key-Value Store ---");

            var cache = new RedisClient();
            cache.Set("name", "Alice");
            var name = cache.Get<string>("name");
            Console.WriteLine($"   Value: {name}");
            // Output: Value: Alice

            // Output: --- Document Store ---
            Console.WriteLine("\n--- Document Store ---");

            var doc = new MongoClient();
            var doc1 = new { Name = "Alice", Age = 30 };
            doc.Insert("users", doc1);
            Console.WriteLine("   Document inserted");
            // Output: Document inserted

            var found = doc.Find("users", "Alice");
            Console.WriteLine($"   Found: {found}");
            // Output: Found: True

            // Output: --- Wide Column ---
            Console.WriteLine("\n--- Wide Column ---");

            var cassandra = new CassandraClient();
            cassandra.Insert("users", "key1", new { Data = "value" });
            Console.WriteLine("   Wide column inserted");
            // Output: Wide column inserted

            // Output: --- Graph Database ---
            Console.WriteLine("\n--- Graph Database ---");

            var graph = new Neo4jClient();
            graph.CreateEdge("Alice", "KNOWS", "Bob");
            var related = graph.GetRelated("Alice");
            Console.WriteLine($"   Related: {related}");
            // Output: Related: Bob

            Console.WriteLine("\n=== NoSQL Complete ===");
        }
    }

    /// <summary>
    /// Redis-like client
    /// </summary>
    public class RedisClient
    {
        private readonly System.Collections.Generic.Dictionary<string, object> _store = new();

        public void Set(string key, object value) => _store[key] = value;
        public T Get<T>(string key) => (T)_store[key];
    }

    /// <summary>
    /// MongoDB-like client
    /// </summary>
    public class MongoClient
    {
        public void Insert(string collection, object document)
        {
            Console.WriteLine("   Inserting document");
        }

        public object Find(string collection, string query)
        {
            return true;
        }
    }

    /// <summary>
    /// Cassandra-like client
    /// </summary>
    public class CassandraClient
    {
        public void Insert(string table, string key, object value) { }
    }

    /// <summary>
    /// Neo4j-like client
    /// </summary>
    public class Neo4jClient
    {
        public void CreateEdge(string from, string edgeType, string to) { }
        public string GetRelated(string node) => "Bob";
    }
}