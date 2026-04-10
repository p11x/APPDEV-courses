/*
 * TOPIC: Task Parallel Library
 * SUBTOPIC: Task Cancellation
 * FILE: 06_TaskCancellation.cs
 * PURPOSE: Understanding CancellationToken and cooperative cancellation
 */
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._02_Tasks
{
    public class TaskCancellation
    {
        public static void Main()
        {
            Console.WriteLine("=== Task Cancellation Demo ===\n");

            var demo = new TaskCancellation();

            // Example 1: Basic CancellationToken
            Console.WriteLine("1. Basic CancellationToken:");
            demo.BasicCancellationDemo();

            // Example 2: CancellationTokenSource with timeout
            Console.WriteLine("\n2. CancellationTokenSource with timeout:");
            demo.CancellationWithTimeoutDemo();

            // Example 3: ThrowIfCancellationRequested
            Console.WriteLine("\n3. ThrowIfCancellationRequested:");
            demo.ThrowIfCancelledDemo();

            // Example 4: Linked CancellationTokenSource
            Console.WriteLine("\n4. Linked CancellationTokenSource:");
            demo.LinkedTokensDemo();

            // Example 5: Cancel in async method
            Console.WriteLine("\n5. Cancel in async method:");
            demo.CancelInAsyncDemo();

            // Example 6: Register callback for cleanup
            Console.WriteLine("\n6. Register callback:");
            demo.RegisterCallbackDemo();

            // Example 7: Poll for cancellation
            Console.WriteLine("\n7. Poll for cancellation:");
            demo.PollCancellationDemo();

            // Example 8: Parallel cancellation
            Console.WriteLine("\n8. Parallel cancellation:");
            demo.ParallelCancellationDemo();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public void BasicCancellationDemo()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var task = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("   Cancellation detected!");
                        return;
                    }
                    Thread.Sleep(50);
                }
            }, token);

            cts.Cancel();
            task.Wait();
            Console.WriteLine($"   Task status: {task.Status}");
        }

        public void CancellationWithTimeoutDemo()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            var token = cts.Token;

            var task = Task.Run(() =>
            {
                try
                {
                    for (int i = 0; i < 20; i++)
                    {
                        token.ThrowIfCancellationRequested();
                        Thread.Sleep(50);
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("   Timeout cancelled!");
                }
            }, token);

            task.Wait();
            Console.WriteLine($"   Completed: {task.Status}");
        }

        public void ThrowIfCancelledDemo()
        {
            var cts = new CancellationTokenSource();
            
            var task = Task.Run(() =>
            {
                try
                {
                    Console.WriteLine("   Starting work...");
                    Thread.Sleep(50);
                    cts.Cancel();
                    Console.WriteLine("   Cancellation requested");
                    
                    // This will throw
                    cts.Token.ThrowIfCancellationRequested();
                    
                    Console.WriteLine("   This won't print");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("   Caught cancellation!");
                    throw;
                }
            }, cts.Token);

            task.Wait();
        }

        public void LinkedTokensDemo()
        {
            var cts1 = new CancellationTokenSource();
            var cts2 = new CancellationTokenSource();
            
            var linked = CancellationTokenSource.CreateLinkedTokenSource(
                cts1.Token, cts2.Token);

            var task = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    linked.Token.ThrowIfCancellationRequested();
                    Thread.Sleep(30);
                }
            }, linked.Token);

            cts1.Cancel(); // Cancel via first token
            task.Wait();
            Console.WriteLine($"   Linked token cancelled: {task.IsCanceled}");
        }

        public async void CancelInAsyncDemo()
        {
            var cts = new CancellationTokenSource();
            
            try
            {
                await LongRunningOperationAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("   Async operation cancelled");
            }
        }

        private async Task LongRunningOperationAsync(CancellationToken token)
        {
            Console.WriteLine("   Async operation starting");
            for (int i = 0; i < 10; i++)
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(30, token);
                Console.WriteLine($"   Step {i + 1}");
            }
        }

        public void RegisterCallbackDemo()
        {
            var cts = new CancellationTokenSource();
            var disposed = false;

            using (cts.Token.Register(() => { disposed = true; }))
            {
                cts.Cancel();
            }
            
            Console.WriteLine($"   Disposed: {disposed}");
        }

        public void PollCancellationDemo()
        {
            var cts = new CancellationTokenSource();
            
            var task = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    // Polling pattern
                    if (cts.Token.IsCancellationRequested)
                        break;
                    
                    Thread.Sleep(30);
                }
                Console.WriteLine("   Polling loop exited");
            }, cts.Token);

            cts.Cancel();
            task.Wait();
        }

        public void ParallelCancellationDemo()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var tasks = new Task[3];
            for (int i = 0; i < 3; i++)
            {
                int id = i;
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 10; j++)
                    {
                        token.ThrowIfCancellationRequested();
                        Thread.Sleep(30);
                    }
                    Console.WriteLine($"   Task {id} completed");
                }, token);
            }

            cts.Cancel();
            Task.WaitAll(tasks);
            
            Console.WriteLine($"   All cancelled: {tasks.All(t => t.IsCanceled)}");
        }
    }

    // Real-world cancellation patterns
    public class CancellableDataLoader
    {
        public async Task<string> LoadDataAsync(CancellationToken token)
        {
            var sb = new System.Text.StringBuilder();
            
            for (int i = 0; i < 10; i++)
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(50, token);
                sb.Append($"Data{i},");
            }
            
            return sb.ToString();
        }

        public async Task<T> LoadWithRetryAsync<T>(
            Func<CancellationToken, Task<T>> loader, int maxRetries, CancellationToken token)
        {
            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    return await loader(token);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch
                {
                    if (attempt == maxRetries - 1) throw;
                    await Task.Delay(100 * (attempt + 1), token);
                }
            }
            throw new Exception("Max retries exceeded");
        }
    }

    public class BackgroundWorker
    {
        private CancellationTokenSource _cts;
        
        public void Start()
        {
            _cts = new CancellationTokenSource();
            Task.Run(() => ProcessAsync(_cts.Token));
        }

        public void Stop() => _cts?.Cancel();

        private async Task ProcessAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(100, token);
                    Console.WriteLine("   Working...");
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("   Worker stopped");
            }
        }
    }
}