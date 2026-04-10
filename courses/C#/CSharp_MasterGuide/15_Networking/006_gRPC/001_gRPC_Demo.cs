/*
 * ============================================================
 * TOPIC     : Networking
 * SUBTOPIC  : gRPC - Part 1
 * FILE      : gRPC_Demo.cs
 * PURPOSE   : Introduction to gRPC communication
 * ============================================================
 */
using System; // Core System namespace
using System.Threading.Tasks; // Task namespace

namespace CSharp_MasterGuide._15_Networking._04_gRPC
{
    /// <summary>
    /// gRPC demonstration
    /// </summary>
    public class GRPCDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== gRPC Demo ===\n");

            // Output: --- What is gRPC? ---
            Console.WriteLine("--- What is gRPC? ---");

            // gRPC = Google Remote Procedure Call
            // High-performance, binary protocol
            // Uses HTTP/2 and Protocol Buffers

            Console.WriteLine("   - Fast binary serialization");
            Console.WriteLine("   - HTTP/2 support");
            Console.WriteLine("   - Protocol Buffers");
            // Output: - Fast binary serialization
            // Output: - HTTP/2 support
            // Output: - Protocol Buffers

            // Output: --- Service Definition ---
            Console.WriteLine("\n--- Service Definition ---");

            // Define service contract
            var userService = new UserServiceGrpc();
            var response = userService.GetUser(new GetUserRequest { Id = 1 });
            Console.WriteLine($"   User: {response.Name}");
            // Output: User: Alice

            // Output: --- Streaming ---
            Console.WriteLine("\n--- Streaming ---");

            var stream = userService.GetUsers_stream(new GetUserRequest());
            Console.WriteLine("   Streaming users");
            // Output: Streaming users

            // Output: --- Bi-directional ---
            Console.WriteLine("\n--- Bi-directional ---");

            var chat = new ChatServiceGrpc();
            chat.SendMessage("Hello");
            // Output: Sent: Hello

            Console.WriteLine("\n=== gRPC Complete ===");
        }
    }

    /// <summary>
    /// Get user request message
    /// </summary>
    public class GetUserRequest
    {
        public int Id { get; set; } // field: id
    }

    /// <summary>
    /// User response message
    /// </summary>
    public class UserResponse
    {
        public string Name { get; set; } = "Alice"; // field: name
        public string Email { get; set; } = "alice@example.com"; // field: email
    }

    /// <summary>
    /// User service gRPC
    /// </summary>
    public class UserServiceGrpc
    {
        public UserResponse GetUser(GetUserRequest request)
        {
            return new UserResponse();
        }

        public async IAsyncEnumerator<UserResponse> GetUsers_stream(GetUserRequest request)
        {
            yield return new UserResponse();
        }
    }

    /// <summary>
    /// Chat service gRPC
    /// </summary>
    public class ChatServiceGrpc
    {
        public async Task SendMessage(string message)
        {
            Console.WriteLine($"   Sent: {message}");
        }
    }
}