/*
 * ============================================================
 * TOPIC     : Networking
 * SUBTOPIC  : WebSockets
 * FILE      : 02_WebSocketsDemo.cs
 * PURPOSE   : Demonstrates WebSocket communication in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._15_Networking._02_WebSockets
{
    /// <summary>
    /// Demonstrates WebSocket usage
    /// </summary>
    public class WebSocketsDemo
    {
        /// <summary>
        /// Entry point for WebSocket examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === WebSockets Demo ===
            Console.WriteLine("=== WebSockets Demo ===\n");

            // ── CONCEPT: What are WebSockets? ───────────────────────────────────
            // Full-duplex communication channel

            // Example 1: WebSocket Connection
            // Output: 1. WebSocket Connection:
            Console.WriteLine("1. WebSocket Connection:");
            
            var server = new WebSocketServer();
            server.Start();
            // Output: WebSocket server started on port 8080

            // Example 2: Client Connection
            // Output: 2. Client Connection:
            Console.WriteLine("\n2. Client Connection:");
            
            var client = new WebSocketClient();
            client.Connect("ws://localhost:8080");
            // Output: Connected to ws://localhost:8080

            // Example 3: Message Exchange
            // Output: 3. Message Exchange:
            Console.WriteLine("\n3. Message Exchange:");
            
            client.Send("Hello Server");
            server.Broadcast("Welcome!");
            // Output: Client sent: Hello Server
            // Output: Server broadcast: Welcome!

            Console.WriteLine("\n=== WebSockets Complete ===");
        }
    }

    /// <summary>
    /// WebSocket server
    /// </summary>
    public class WebSocketServer
    {
        public void Start() => Console.WriteLine("   WebSocket server started on port 8080");
        public void Broadcast(string message) => Console.WriteLine($"   Server broadcast: {message}");
    }

    /// <summary>
    /// WebSocket client
    /// </summary>
    public class WebSocketClient
    {
        public void Connect(string url) => Console.WriteLine($"   Connected to {url}");
        public void Send(string message) => Console.WriteLine($"   Client sent: {message}");
    }
}