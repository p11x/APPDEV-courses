/*
================================================================================
TOPIC 37: SIGNALR
================================================================================

SignalR enables real-time bidirectional communication.

TABLE OF CONTENTS:
1. What is SignalR?
2. Hubs
3. Connections
4. Real-time Applications
================================================================================
*/

namespace SignalRConcepts
{
    // ====================================================================
    // SIGNALR HUB EXAMPLE
    // ====================================================================
    
    // Example: ChatHub.cs
    /*
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", "System", "User joined");
        }
        
        public async Task SendToGroup(string groupName, string user, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
        }
    }
    */
    
    // ====================================================================
    // CLIENT EXAMPLE (JavaScript)
    // ====================================================================
    
    // Example: JavaScript client
    /*
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .build();
    
    connection.on("ReceiveMessage", (user, message) => {
        console.log(`${user}: ${message}`);
    });
    
    await connection.start();
    
    await connection.invoke("SendMessage", "John", "Hello!");
    */
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SignalR ===");
            
            Console.WriteLine("\nReal-time Features:");
            Console.WriteLine("- Chat applications");
            Console.WriteLine("- Live dashboards");
            Console.WriteLine("- Notifications");
            Console.WriteLine("- Collaborative editing");
            Console.WriteLine("- Gaming");
            
            Console.WriteLine("\nTransports:");
            Console.WriteLine("- WebSockets (preferred)");
            Console.WriteLine("- Server-Sent Events");
            Console.WriteLine("- Long Polling (fallback)");
            
            Console.WriteLine("\nNuGet:");
            Console.WriteLine("Microsoft.AspNetCore.SignalR");
        }
    }
}

/*
SIGNALR CONCEPTS:
-----------------
Hub: Communication endpoint
Connection: Client connection to hub
Group: Collection of connections
Client: Can invoke hub methods
Server: Can push to clients

Scale:
------
- Azure SignalR Service
- Redis backplane
- SQL Server backplane
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 38 covers Configuration and Logging.
*/
