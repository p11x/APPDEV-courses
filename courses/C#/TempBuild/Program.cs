/*
 * ============================================================
 * TOPIC     : Asynchronous Programming
 * SUBTOPIC  : Real-World Async Examples
 * FILE      : AsyncRealWorld.cs
 * PURPOSE   : Learn real-world async patterns including file I/O,
 *            HTTP calls, and database operations
 * ============================================================
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._08_Asynchronous_Programming
{
    class AsyncRealWorld
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Real-World Async Examples in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Async File I/O
            // ═══════════════════════════════════════════════════════════

            // ── CONCEPT: Why Async File I/O? ───────────────────────────
            // File I/O is I/O-bound - async prevents thread blocking
            // Critical for responsive applications with large files

            // ── EXAMPLE 1: Async File Writing ───────────────────────────
            Console.WriteLine("--- Async File Writing ---");
            string filePath = "sample_async.txt";
            
            await WriteToFileAsync(filePath, "Hello, Async World!\nThis is line 2.\nLine 3 here.");
            Console.WriteLine("  File written asynchronously");
            // Output: File written asynchronously

            // ── EXAMPLE 2: Async File Reading ───────────────────────────
            Console.WriteLine("\n--- Async File Reading ---");
            string content = await ReadFromFileAsync(filePath);
            Console.WriteLine($"  File content: {content.Substring(0, Math.Min(30, content.Length))}...");
            // Output: File content: Hello, Async World!...

            // ── EXAMPLE 3: Reading Large Files Line by Line ─────────────
            Console.WriteLine("\n--- Async Line-by-Line Reading ---");
            await ReadFileLineByLineAsync("sample_async.txt");
            // Output: Line: Hello, Async World!
            // Output: Line: This is line 2.
            // Output: Line: Line 3 here.

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Async HTTP Calls
            // ═══════════════════════════════════════════════════════════

            // ── CONCEPT: Async HTTP Requests ────────────────────────────
            // HTTP calls involve network latency - async keeps UI responsive
            // Use HttpClient for modern async HTTP operations

            // ── EXAMPLE 1: GET Request Pattern ──────────────────────────
            Console.WriteLine("\n--- Async HTTP GET Request ---");
            string httpResult = await FetchDataFromUrlAsync("https://api.example.com/data");
            Console.WriteLine($"  Response: {httpResult}");
            // Output: Response: {"data": "sample", "status": "ok"}

            // ── EXAMPLE 2: POST Request Pattern ─────────────────────────
            Console.WriteLine("\n--- Async HTTP POST Request ---");
            string postResult = await PostDataToUrlAsync("https://api.example.com/submit", 
                "{\"name\": \"John\", \"age\": 30}");
            Console.WriteLine($"  POST Response: {postResult}");
            // Output: POST Response: {"success": true, "id": 12345}

            // ── EXAMPLE 3: Multiple HTTP Requests in Parallel ───────────
            Console.WriteLine("\n--- Parallel HTTP Requests ---");
            var urls = new[]
            {
                "https://api.example.com/users/1",
                "https://api.example.com/users/2",
                "https://api.example.com/users/3"
            };
            
            var tasks = urls.Select(url => FetchDataFromUrlAsync(url)).ToList();
            var responses = await Task.WhenAll(tasks);
            
            foreach (var resp in responses)
                Console.WriteLine($"  {resp.Substring(0, Math.Min(40, resp.Length))}...");
            // Output: Multiple responses from parallel requests

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Async Database Operations (Simulated)
            // ═══════════════════════════════════════════════════════════

            // ── CONCEPT: Async Database Access ─────────────────────────
            // Database queries involve network/磁盘 I/O - use async to avoid blocking
            // Patterns shown here apply to real DB with Entity Framework, Dapper, etc.

            // ── EXAMPLE 1: Simulated Async Query ─────────────────────────
            Console.WriteLine("\n--- Async Database Query ---");
            var users = await QueryUsersAsync();
            foreach (var user in users)
                Console.WriteLine($"  User: {user.Name}, Email: {user.Email}");
            // Output: User: John Doe, Email: john@example.com
            // Output: User: Jane Smith, Email: jane@example.com

            // ── EXAMPLE 2: Async Insert Operation ───────────────────────
            Console.WriteLine("\n--- Async Database Insert ---");
            var newUser = new User { Name = "Bob Wilson", Email = "bob@example.com" };
            int insertedId = await InsertUserAsync(newUser);
            Console.WriteLine($"  Inserted user with ID: {insertedId}");
            // Output: Inserted user with ID: 3

            // ── EXAMPLE 3: Async Update and Delete ───────────────────────
            Console.WriteLine("\n--- Async Database Update/Delete ---");
            bool updated = await UpdateUserAsync(insertedId, "Robert Wilson");
            Console.WriteLine($"  Update result: {(updated ? "Success" : "Failed")}");
            
            bool deleted = await DeleteUserAsync(insertedId);
            Console.WriteLine($"  Delete result: {(deleted ? "Success" : "Failed")}");
            // Output: Update result: Success
            // Output: Delete result: Success

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Real-World Application Patterns
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Async Service Layer Pattern ───────────────────
            Console.WriteLine("\n--- Async Service Layer Pattern ---");
            var service = new UserService();
            var userList = await service.GetAllUsersAsync();
            Console.WriteLine($"  Service returned {userList.Count} users");
            // Output: Service returned 2 users

            // ── EXAMPLE 2: Async Background Job Pattern ──────────────────
            Console.WriteLine("\n--- Async Background Job Pattern ---");
            var job = new BackgroundJobProcessor();
            await job.ProcessJobAsync("data_import");
            // Output: Job data_import started
            // Output: Processing batch 1
            // Output: Processing batch 2
            // Output: Processing batch 3
            // Output: Job data_import completed

            // ── EXAMPLE 3: Async Caching Pattern ─────────────────────────
            Console.WriteLine("\n--- Async Caching Pattern ---");
            var cache = new AsyncCache<string, string>();
            
            // First call - fetches from "source"
            string cachedValue1 = await cache.GetOrAddAsync("key1", async () => 
            {
                await Task.Delay(100); // Simulate slow source
                return "Value from source";
            });
            Console.WriteLine($"  First fetch: {cachedValue1}");
            
            // Second call - returns cached value (faster)
            string cachedValue2 = await cache.GetOrAddAsync("key1", async () => 
            {
                await Task.Delay(100);
                return "Value from source";
            });
            Console.WriteLine($"  Second fetch (cached): {cachedValue2}");
            // Output: First fetch: Value from source
            // Output: Second fetch (cached): Value from source

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Chained Async Operations
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Sequential Async Pipeline ────────────────────
            Console.WriteLine("\n--- Sequential Async Pipeline ---");
            var pipeline = new DataPipeline();
            var finalResult = await pipeline.ProcessAsync("raw_input.txt");
            Console.WriteLine($"  Pipeline result: {finalResult}");
            // Output: Pipeline result: Processed: RAW_INPUT.TXT

            Console.WriteLine("\n=== Real-World Async Examples Complete ===");
        }

        // ═══════════════════════════════════════════════════════════════
        // FILE I/O METHODS
        // ═══════════════════════════════════════════════════════════════
        
        // Async file writing
        static async Task WriteToFileAsync(string path, string content)
        {
            // WriteAllTextAsync is the async version - doesn't block thread
            await File.WriteAllTextAsync(path, content);
        }

        // Async file reading
        static async Task<string> ReadFromFileAsync(string path)
        {
            // ReadAllTextAsync reads file without blocking calling thread
            return await File.ReadAllTextAsync(path);
        }

        // Async line-by-line reading (for large files)
        static async Task ReadFileLineByLineAsync(string path)
        {
            using var reader = new StreamReader(path);
            
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                Console.WriteLine($"  Line: {line}");
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // HTTP METHODS
        // ═══════════════════════════════════════════════════════════════
        
        // Simulated async GET request
        static async Task<string> FetchDataFromUrlAsync(string url)
        {
            // In real code: using var client = new HttpClient(); return await client.GetStringAsync(url);
            // Simulating network delay
            await Task.Delay(100);
            
            // Simulated response
            if (url.Contains("users"))
                return $"{{\"id\": {url.LastOrDefault()}, \"name\": \"User {url.LastOrDefault()}\"}}";
            return "{\"data\": \"sample\", \"status\": \"ok\"}";
        }

        // Simulated async POST request
        static async Task<string> PostDataToUrlAsync(string url, string jsonData)
        {
            // In real code: using var client = new HttpClient(); 
            //                 var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            //                 return await client.PostAsync(url, content).Result.Content.ReadAsStringAsync();
            await Task.Delay(100);
            return "{\"success\": true, \"id\": 12345}";
        }

        // ═══════════════════════════════════════════════════════════════
        // DATABASE METHODS (Simulated)
        // ═══════════════════════════════════════════════════════════════
        
        // Simulated async database query
        static async Task<List<User>> QueryUsersAsync()
        {
            // Simulate database query delay
            await Task.Delay(100);
            
            // Return simulated data
            return new List<User>
            {
                new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
                new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com" }
            };
        }

        // Simulated async insert
        static async Task<int> InsertUserAsync(User user)
        {
            await Task.Delay(50);
            // Simulate auto-increment ID
            return new Random().Next(100, 1000);
        }

        // Simulated async update
        static async Task<bool> UpdateUserAsync(int userId, string newName)
        {
            await Task.Delay(50);
            return true; // Simulate success
        }

        // Simulated async delete
        static async Task<bool> DeleteUserAsync(int userId)
        {
            await Task.Delay(50);
            return true; // Simulate success
        }

        // ═══════════════════════════════════════════════════════════════
        // HELPER CLASSES
        // ═══════════════════════════════════════════════════════════════
        
        class User
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public string Email { get; set; } = "";
        }

        // Service layer pattern
        class UserService
        {
            public async Task<List<User>> GetAllUsersAsync()
            {
                await Task.Delay(50);
                return new List<User>
                {
                    new User { Name = "User1", Email = "user1@example.com" },
                    new User { Name = "User2", Email = "user2@example.com" }
                };
            }
        }

        // Background job processor
        class BackgroundJobProcessor
        {
            public async Task ProcessJobAsync(string jobId)
            {
                Console.WriteLine($"  Job {jobId} started");
                
                // Process in batches
                for (int i = 1; i <= 3; i++)
                {
                    await Task.Delay(100);
                    Console.WriteLine($"  Processing batch {i}");
                }
                
                Console.WriteLine($"  Job {jobId} completed");
            }
        }

        // Simple async cache
        class AsyncCache<TKey, TValue> where TKey : notnull
        {
            private readonly Dictionary<TKey, TValue> _cache = new();
            private readonly SemaphoreSlim _lock = new(1, 1);
            
            public async Task<TValue> GetOrAddAsync(TKey key, Func<Task<TValue>> factory)
            {
                await _lock.WaitAsync();
                try
                {
                    if (_cache.TryGetValue(key, out var value))
                        return value;
                    
                    var newValue = await factory();
                    _cache[key] = newValue;
                    return newValue;
                }
                finally
                {
                    _lock.Release();
                }
            }
        }

        // Data pipeline for chained operations
        class DataPipeline
        {
            public async Task<string> ProcessAsync(string input)
            {
                // Step 1: Read
                var data = await ReadAsync(input);
                
                // Step 2: Transform
                var transformed = await TransformAsync(data);
                
                // Step 3: Save
                await SaveAsync(transformed);
                
                return transformed;
            }
            
            async Task<string> ReadAsync(string input)
            {
                await Task.Delay(50);
                return input.ToUpper();
            }
            
            async Task<string> TransformAsync(string data)
            {
                await Task.Delay(50);
                return $"Processed: {data}";
            }
            
            async Task SaveAsync(string data)
            {
                await Task.Delay(50);
                Console.WriteLine($"  Saved: {data}");
            }
        }
    }
}