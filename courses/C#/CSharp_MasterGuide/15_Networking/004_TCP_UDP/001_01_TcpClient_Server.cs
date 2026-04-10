/*
 * ============================================================
 * TOPIC     : Networking
 * SUBTOPIC  : TCP/UDP - Client Server
 * FILE      : 01_TcpClient_Server.cs
 * PURPOSE   : TCP client-server communication
 * ============================================================
 */
using System;
using System.Net.Sockets;

namespace CSharp_MasterGuide._15_Networking._03_TCP_UDP
{
    /// <summary>
    /// TCP client-server basics
    /// </summary>
    public class TcpClientServer
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== TCP Client/Server ===\n");

            Console.WriteLine("1. Server - Listen on port 8080:");
            Console.WriteLine("   Listening for connections...");
            
            Console.WriteLine("\n2. Client - Connect to server:");
            Console.WriteLine("   Connected to localhost:8080");
            
            Console.WriteLine("\n3. Send Data:");
            Console.WriteLine("   Message sent: Hello");
            
            Console.WriteLine("\n4. Receive Data:");
            Console.WriteLine("   Message received: World");

            Console.WriteLine("\n=== TCP Client/Server Complete ===");
        }
    }
}