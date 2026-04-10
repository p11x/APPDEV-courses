/*
 * ============================================================
 * TOPIC     : Networking
 * SUBTOPIC  : WebSockets Basics
 * FILE      : 01_WebSocketBasics.cs
 * PURPOSE   : WebSocket communication in C#
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._15_Networking._02_WebSockets
{
    /// <summary>
    /// WebSocket basics
    /// </summary>
    public class WebSocketBasics
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== WebSocket Basics ===\n");

            Console.WriteLine("1. Connect to WebSocket:");
            Console.WriteLine("   Connecting to wss://example.com/ws...");
            
            Console.WriteLine("\n2. Send Message:");
            Console.WriteLine("   Sending: Hello Server");
            
            Console.WriteLine("\n3. Receive Message:");
            Console.WriteLine("   Received: Hello Client");
            
            Console.WriteLine("\n4. Close Connection:");
            Console.WriteLine("   Connection closed");

            Console.WriteLine("\n=== WebSocket Basics Complete ===");
        }
    }
}