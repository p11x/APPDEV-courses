/*
 * TOPIC: Async/Await Fundamentals
 * SUBTOPIC: ConfigureAwait
 * FILE: 06_ConfigureAwait.cs
 * PURPOSE: Understanding ConfigureAwait(false) and when to use it
 */
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._01_AsyncAwait
{
    public class ConfigureAwaitDemo
    {
        public static async Task Main()
        {
            Console.WriteLine("=== ConfigureAwait Demo ===\n");

            var demo = new ConfigureAwaitDemo();

            // Example 1: Default behavior - captures synchronization context
            Console.WriteLine("1. Default ConfigureAwait (true):");
            Console.WriteLine($"   Main thread: {Thread.CurrentThread.ManagedThreadId}");
            await demo.DefaultBehaviorAsync();
            Console.WriteLine($"   Resumed on thread: {Thread.CurrentThread.ManagedThreadId}");

            // Example 2: ConfigureAwait(false) - avoids context capture
            Console.WriteLine("\n2. ConfigureAwait(false):");
            Console.WriteLine($"   Main thread: {Thread.CurrentThread.ManagedThreadId}");
            await demo.ConfigureAwaitFalseAsync();
            Console.WriteLine($"   Resumed on thread: {Thread.CurrentThread.ManagedThreadId}");

            // Example 3: Library code should use ConfigureAwait(false)
            Console.WriteLine("\n3. Library code pattern:");
            var result = await demo.LibraryMethodAsync();
            Console.WriteLine($"   Result: {result}");

            // Example 4: UI application - prefer ConfigureAwait(true)
            Console.WriteLine("\n4. UI application pattern:");
            await demo.UiMethodAsync();

            // Example 5: Nested ConfigureAwait
            Console.WriteLine("\n5. Nested ConfigureAwait:");
            await demo.NestedConfigureAwaitAsync();

            // Example 6: Mixing in same method
            Console.WriteLine("\n6. Mixing ConfigureAwait in method:");
            await demo.MixedConfigureAwaitAsync();

            // Example 7: ContinueOnCapturedContext alternative
            Console.WriteLine("\n7. TaskContinuationOptions:");
            await demo.ContinuationOptionsDemoAsync();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public async Task DefaultBehaviorAsync()
        {
            Console.WriteLine($"   DefaultBehavior thread: {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(100);
            Console.WriteLine($"   DefaultBehavior resumed: {Thread.CurrentThread.ManagedThreadId}");
        }

        public async Task ConfigureAwaitFalseAsync()
        {
            Console.WriteLine($"   ConfigureAwaitFalse thread: {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(100).ConfigureAwait(false);
            Console.WriteLine($"   ConfigureAwaitFalse resumed: {Thread.CurrentThread.ManagedThreadId}");
        }

        // Library method - should use ConfigureAwait(false)
        public async Task<int> LibraryMethodAsync()
        {
            int result = await ComputeAsync().ConfigureAwait(false);
            return result;
        }

        private async Task<int> ComputeAsync()
        {
            await Task.Delay(50).ConfigureAwait(false);
            return 42;
        }

        // UI method - may need synchronization context
        public async Task UiMethodAsync()
        {
            await Task.Delay(50);
            // In UI app, would update UI controls here
            Console.WriteLine("   UI context available for UI updates");
        }

        public async Task NestedConfigureAwaitAsync()
        {
            await OuterMethodAsync();
            Console.WriteLine("   Back in outer method");
        }

        private async Task OuterMethodAsync()
        {
            await InnerMethodAsync().ConfigureAwait(false);
        }

        private async Task InnerMethodAsync()
        {
            await Task.Delay(50).ConfigureAwait(false);
            Console.WriteLine("   Inner method executed");
        }

        public async Task MixedConfigureAwaitAsync()
        {
            await Step1Async().ConfigureAwait(false);
            await Step2Async(); // Uses default
            await Step3Async().ConfigureAwait(false);
        }

        private async Task Step1Async() { await Task.Delay(30); Console.WriteLine("   Step1"); }
        private async Task Step2Async() { await Task.Delay(30); Console.WriteLine("   Step2"); }
        private async Task Step3Async() { await Task.Delay(30); Console.WriteLine("   Step3"); }

        public async Task ContinuationOptionsDemoAsync()
        {
            var task = Task.FromResult(42);
            int result = await task.ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine($"   Result via continuation: {result}");
        }
    }

    // Real-world library example
    public class HttpClientWrapper
    {
        public async Task<string> GetAsync(string url)
        {
            // Using ConfigureAwait(false) in library code
            await Task.Delay(100).ConfigureAwait(false); // Simulate network
            return $"Response from {url}";
        }

        public async Task<byte[]> GetBytesAsync(string url)
        {
            await Task.Delay(100).ConfigureAwait(false);
            return new byte[] { 1, 2, 3, 4, 5 };
        }

        public async Task<bool> PostAsync(string url, string data)
        {
            await Task.Delay(100).ConfigureAwait(false);
            return true;
        }
    }

    // Example: Service layer (should use ConfigureAwait(false))
    public class DataService
    {
        public async Task<Data> GetDataAsync(int id)
        {
            var rawData = await FetchRawDataAsync(id).ConfigureAwait(false);
            return ProcessData(rawData);
        }

        private async Task<string> FetchRawDataAsync(int id)
        {
            await Task.Delay(100).ConfigureAwait(false);
            return $"RawData_{id}";
        }

        private Data ProcessData(string rawData)
        {
            return new Data { Value = rawData };
        }
    }

    public class Data
    {
        public string Value { get; set; }
    }

    // Best practices summary
    public class BestPractices
    {
        // Library code: Use ConfigureAwait(false)
        public async Task LibraryMethodAsync() 
            => await Task.Delay(100).ConfigureAwait(false);

        // Application code: Default is fine, or use explicitly based on needs
        public async Task AppMethodAsync()
        {
            // Some operations may need ConfigureAwait(false) for performance
            await ComputeIntensiveWorkAsync().ConfigureAwait(false);
        }

        private async Task ComputeIntensiveWorkAsync()
        {
            await Task.Delay(50);
        }
    }
}