/*
 * TOPIC: Task Parallel Library
 * SUBTOPIC: Tasks Real-World
 * FILE: 10_Tasks_RealWorld.cs
 * PURPOSE: Real-world Task patterns and best practices
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._02_Tasks
{
    public class TasksRealWorld
    {
        public static async Task Main()
        {
            Console.WriteLine("=== Tasks Real-World Demo ===\n");

            var demo = new TasksRealWorld();

            // Example 1: File processing with Tasks
            Console.WriteLine("1. File processing:");
            await demo.ProcessFilesAsync();

            // Example 2: Web request with retry
            Console.WriteLine("\n2. Web request with retry:");
            var response = await demo.FetchWithRetryAsync("https://example.com");
            Console.WriteLine($"   Response length: {response.Length}");

            // Example 3: Parallel data fetching
            Console.WriteLine("\n3. Parallel data fetching:");
            var data = await demo.FetchMultipleDataAsync();
            Console.WriteLine($"   Fetched: {data.Count} items");

            // Example 4: Background task with progress
            Console.WriteLine("\n4. Background task with progress:");
            await demo.BackgroundProcessingWithProgressAsync();

            // Example 5: Task-based pipeline
            Console.WriteLine("\n5. Task-based pipeline:");
            await demo.PipelineProcessingAsync();

            // Example 6: Async queue processing
            Console.WriteLine("\n6. Async queue processing:");
            await demo.AsyncQueueProcessingAsync();

            // Example 7: Concurrent API calls
            Console.WriteLine("\n7. Concurrent API calls:");
            var results = await demo.ConcurrentApiCallsAsync();
            Console.WriteLine($"   Results: {string.Join(", ", results)}");

            Console.WriteLine("\n=== End of Demo ===");
        }

        public async Task ProcessFilesAsync()
        {
            var files = new[] { "file1.txt", "file2.txt", "file3.txt" };
            
            // Ensure files exist
            foreach (var f in files)
            {
                if (!File.Exists(f))
                    await File.WriteAllTextAsync(f, $"Content of {f}");
            }

            var tasks = files.Select(async f =>
            {
                var content = await File.ReadAllTextAsync(f);
                return (f, content.Length);
            });

            var results = await Task.WhenAll(tasks);
            foreach (var (file, length) in results)
                Console.WriteLine($"   {file}: {length} chars");
        }

        public async Task<string> FetchWithRetryAsync(string url, int maxRetries = 3)
        {
            var delay = 100;
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
                    return await client.GetStringAsync(url);
                }
                catch when (i < maxRetries - 1)
                {
                    await Task.Delay(delay);
                    delay *= 2;
                }
            }
            return "Fallback response";
        }

        public async Task<Dictionary<string, string>> FetchMultipleDataAsync()
        {
            var urls = new[] { "url1", "url2", "url3" };
            var tasks = urls.Select(async url =>
            {
                await Task.Delay(50);
                return (url, $"Data for {url}");
            });

            var results = await Task.WhenAll(tasks);
            return results.ToDictionary(r => r.url, r => r.Item2);
        }

        public async Task BackgroundProcessingWithProgressAsync()
        {
            var progress = new Progress<int>(p =>
            {
                Console.WriteLine($"   Progress: {p}%");
            });

            await Task.Run(() =>
            {
                for (int i = 0; i <= 10; i++)
                {
                    Thread.Sleep(50);
                    progress.Report(i * 10);
                }
            });
        }

        public async Task PipelineProcessingAsync()
        {
            var items = Enumerable.Range(1, 10).ToList();

            // Stage 1: Transform
            var stage1 = items.Select(i => Task.Run(() => i * 2));
            var results1 = await Task.WhenAll(stage1);

            // Stage 2: Filter
            var stage2 = results1.Where(x => x > 5).ToList();

            // Stage 3: Aggregate
            var sum = stage2.Sum();
            Console.WriteLine($"   Pipeline result: {sum}");
        }

        public async Task AsyncQueueProcessingAsync()
        {
            var queue = new System.Collections.Concurrent.ConcurrentQueue<int>();
            var results = new System.Collections.Concurrent.ConcurrentBag<int>();

            // Producer
            var producer = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    queue.Enqueue(i);
                    Thread.Sleep(20);
                }
            });

            // Consumer
            var consumer = Task.Run(async () =>
            {
                while (!producer.IsCompleted || queue.Count > 0)
                {
                    if (queue.TryDequeue(out var item))
                    {
                        await Task.Delay(30);
                        results.Add(item * 2);
                    }
                }
            });

            await Task.WhenAll(producer, consumer);
            Console.WriteLine($"   Processed: {results.Count} items");
        }

        public async Task<List<string>> ConcurrentApiCallsAsync()
        {
            var endpoints = new[] { "users", "posts", "comments" };
            
            var tasks = endpoints.Select(async endpoint =>
            {
                await Task.Delay(50);
                return $"{endpoint}: data";
            });

            return (await Task.WhenAll(tasks)).ToList();
        }
    }

    // Real-world service patterns
    public class DataFetchService
    {
        private readonly HttpClient _httpClient;

        public DataFetchService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://api.example.com/") };
        }

        public async Task<UserData> GetUserDataAsync(int userId)
        {
            var profile = await FetchAsync<UserProfile>($"/users/{userId}");
            var orders = await FetchAsync<List<Order>>($"/users/{userId}/orders");

            return new UserData
            {
                Profile = profile,
                Orders = orders
            };
        }

        private async Task<T> FetchAsync<T>(string endpoint)
        {
            await Task.Delay(50);
            return default;
        }

        public async Task<BatchResult> FetchBatchAsync(IEnumerable<int> userIds)
        {
            var tasks = userIds.Select(id => GetUserDataAsync(id));
            var results = await Task.WhenAll(tasks);

            return new BatchResult
            {
                SuccessCount = results.Count(r => r != null),
                FailedCount = results.Count(r => r == null)
            };
        }
    }

    public class UserProfile { public string Name { get; set; } }
    public class Order { public int Id { get; set; } }
    public class UserData { public UserProfile Profile { get; set; } public List<Order> Orders { get; set; } }
    public class BatchResult { public int SuccessCount { get; set; } public int FailedCount { get; set; } }

    public class BackgroundSyncService
    {
        private CancellationTokenSource _cts;

        public Task StartSyncAsync()
        {
            _cts = new CancellationTokenSource();
            return Task.Run(async () =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        await SyncDataAsync(_cts.Token);
                        await Task.Delay(TimeSpan.FromMinutes(5), _cts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            });
        }

        public void StopSync() => _cts?.Cancel();

        private async Task SyncDataAsync(CancellationToken token)
        {
            Console.WriteLine("   Syncing data...");
            await Task.Delay(100, token);
        }
    }

    public class FileProcessingService
    {
        public async Task<ProcessResult> ProcessDirectoryAsync(
            string directory, IProgress<string> progress)
        {
            var files = Directory.GetFiles(directory, "*.txt");
            var results = new List<string>();

            foreach (var file in files)
            {
                progress?.Report($"Processing {file}");
                var content = await File.ReadAllTextAsync(file);
                await Task.Delay(50);
                results.Add(content);
            }

            return new ProcessResult
            {
                FilesProcessed = files.Length,
                TotalSize = results.Sum(r => r.Length)
            };
        }

        public async Task<string> ProcessLargeFileAsync(
            string filePath, CancellationToken token)
        {
            var sb = new StringBuilder();
            using var reader = new StreamReader(filePath);

            string line;
            while ((line = await reader.ReadLineAsync(token)) != null)
            {
                token.ThrowIfCancellationRequested();
                sb.AppendLine(line.ToUpper());
            }

            return sb.ToString();
        }
    }

    public class ProcessResult
    {
        public int FilesProcessed { get; set; }
        public long TotalSize { get; set; }
    }
}