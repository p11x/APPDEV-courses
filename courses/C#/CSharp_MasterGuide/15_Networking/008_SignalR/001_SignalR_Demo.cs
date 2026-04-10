/*
 * ============================================================
 * TOPIC     : Networking
 * SUBTOPIC  : SignalR - Real-time Communication
 * FILE      : SignalR_Demo.cs
 * PURPOSE   : SignalR real-time messaging
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._15_Networking._05_SignalR
{
    /// <summary>
    /// SignalR demonstration
    /// </summary>
    public class SignalRDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== SignalR Demo ===\n");

            // Output: --- What is SignalR? ---
            Console.WriteLine("--- What is SignalR? ---");

            // Real-time web communication
            // WebSocket fallback to SSE/polling
            
            Console.WriteLine("   - Real-time updates");
            Console.WriteLine("   - WebSocket fallback");
            Console.WriteLine("   - Automatic reconnection");
            // Output: - Real-time updates
            // Output: - WebSocket fallback
            // Output: - Automatic reconnection

            // Output: --- Hub Communication ---
            Console.WriteLine("\n--- Hub Communication ---");

            var hub = new ChatHub();
            hub.Send("Hello from client");
            // Output: Received: Hello from client

            // Output: --- Broadcast ---
            Console.WriteLine("\n--- Broadcast ---");

            hub.Broadcast("Message");
            // Output: Broadcasting: Message
            // Output: Sent to all clients

            // Output: --- Groups ---
            Console.WriteLine("\n--- Groups ---");

            hub.JoinGroup("Admins");
            hub.SendToGroup("Admins", "Admin message");
            // Output: Joined group: Admins
            // Output: Sent to group: Admins

            Console.WriteLine("\n=== SignalR Complete ===");
        }
    }

    /// <summary>
    /// Chat hub - SignalR hub
    /// </summary>
    public class ChatHub
    {
        public void Send(string message)
        {
            Console.WriteLine($"   Received: {message}");
        }

        public void Broadcast(string message)
        {
            Console.WriteLine($"   Broadcasting: {message}");
            Console.WriteLine("   Sent to all clients");
        }

        public void JoinGroup(string groupName)
        {
            Console.WriteLine($"   Joined group: {groupName}");
        }

        public void SendToGroup(string groupName, string message)
        {
            Console.WriteLine($"   Sent to group: {groupName}");
        }
    }
}