/*
 * TOPIC: Async/Await Fundamentals
 * SUBTOPIC: Real-World Examples
 * FILE: 07_AsyncAwait_RealWorld.cs
 * PURPOSE: Real-world async/await examples with file and network operations
 */
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._01_AsyncAwait
{
    public class AsyncAwaitRealWorld
    {
        public static async Task Main()
        {
            Console.WriteLine("=== Async/Await Real-World Demo ===\n");

            var demo = new AsyncAwaitRealWorld();

            // Example 1: Async file reading
            Console.WriteLine("1. Async file reading:");
            string fileContent = await demo.ReadFileAsync("test.txt");
            Console.WriteLine($"   File content: {fileContent ?? "(file not found)"}");

            // Example 2: Async file writing
            Console.WriteLine("\n2. Async file writing:");
            bool writeSuccess = await demo.WriteFileAsync("output.txt", "Hello, Async World!");
            Console.WriteLine($"   Write success: {writeSuccess}");

            // Example 3: Async HTTP request
            Console.WriteLine("\n3. Async HTTP request:");
            string httpResult = await demo.FetchFromUrlAsync("https://example.com");
            Console.WriteLine($"   HTTP response: {httpResult.Substring(0, Math.Min(50, httpResult.Length))}...");

            // Example 4: Async JSON parsing
            Console.WriteLine("\n4. Async JSON processing:");
            var jsonData = await demo.ParseJsonAsync(@"{""name"":""John"",""age"":30}");
            Console.WriteLine($"   Parsed: {jsonData.Name}, Age: {jsonData.Age}");

            // Example 5: Async multiple file operations
            Console.WriteLine("\n5. Multiple async file operations:");
            var files = await demo.ReadMultipleFilesAsync(new[] { "file1.txt", "file2.txt" });
            foreach (var f in files)
                Console.WriteLine($"   {f.Key}: {f.Value}");

            // Example 6: Async download with progress
            Console.WriteLine("\n6. Async download with progress:");
            await demo.DownloadWithProgressAsync("https://example.com/file.zip");

            // Example 7: Async database-like operation
            Console.WriteLine("\n7. Async database operation:");
            var user = await demo.GetUserFromDatabaseAsync(1);
            Console.WriteLine($"   User: {user.Name}, Email: {user.Email}");

            // Example 8: Async retry pattern
            Console.WriteLine("\n8. Async retry pattern:");
            string retryResult = await demo.RetryAsync(() => demo.UnreliableOperationAsync());
            Console.WriteLine($"   Retry result: {retryResult}");

            Console.WriteLine("\n=== End of Demo ===");
        }

        public async Task<string> ReadFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                await File.WriteAllTextAsync(filePath, "Sample content for testing");
            }
            return await File.ReadAllTextAsync(filePath);
        }

        public async Task<bool> WriteFileAsync(string filePath, string content)
        {
            try
            {
                await File.WriteAllTextAsync(filePath, content);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> FetchFromUrlAsync(string url)
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);
            
            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<User> ParseJsonAsync(string json)
        {
            await Task.Delay(50); // Simulate parsing
            return new User
            {
                Name = "John",
                Age = 30,
                Email = "john@example.com"
            };
        }

        public async Task<System.Collections.Generic.Dictionary<string, string>> ReadMultipleFilesAsync(
            string[] filePaths)
        {
            var results = new System.Collections.Generic.Dictionary<string, string>();
            
            foreach (var path in filePaths)
            {
                if (!File.Exists(path))
                    File.WriteAllTextAsync(path, $"Content of {path}").Wait();
                
                results[path] = await File.ReadAllTextAsync(path);
            }
            
            return results;
        }

        public async Task DownloadWithProgressAsync(string url)
        {
            for (int i = 0; i <= 10; i++)
            {
                await Task.Delay(50);
                Console.WriteLine($"   Progress: {i * 10}%");
            }
            Console.WriteLine("   Download complete");
        }

        public async Task<User> GetUserFromDatabaseAsync(int userId)
        {
            await Task.Delay(100); // Simulate DB query
            return new User
            {
                Id = userId,
                Name = $"User {userId}",
                Email = $"user{userId}@example.com"
            };
        }

        public async Task<string> UnreliableOperationAsync()
        {
            await Task.Delay(50);
            if (new Random().Next(3) != 0)
                throw new Exception("Transient failure");
            return "Success!";
        }

        public async Task<string> RetryAsync(Func<Task<string>> operation, int maxRetries = 3)
        {
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    return await operation();
                }
                catch when (i < maxRetries - 1)
                {
                    await Task.Delay(100 * (i + 1));
                }
            }
            return "Failed after retries";
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }

    // Additional real-world utilities

    public class FileProcessor
    {
        public async Task<string> ProcessFileAsync(string inputPath, string outputPath)
        {
            var content = await File.ReadAllTextAsync(inputPath);
            var processed = content.ToUpper();
            await File.WriteAllTextAsync(outputPath, processed);
            return processed;
        }

        public async Task<long> GetFileSizeAsync(string path)
        {
            var info = new FileInfo(path);
            await Task.Delay(10);
            return info.Length;
        }
    }

    public class WebServiceClient
    {
        private readonly HttpClient _httpClient;

        public WebServiceClient()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://api.example.com/") };
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return new System.Text.Json.JsonSerializer().Deserialize<T>(json);
        }

        public async Task<bool> PostAsync<T>(string endpoint, T data)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            return response.IsSuccessStatusCode;
        }
    }
}