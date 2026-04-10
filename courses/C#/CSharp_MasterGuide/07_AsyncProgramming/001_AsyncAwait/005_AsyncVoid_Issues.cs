/*
 * TOPIC: Async/Await Fundamentals
 * SUBTOPIC: Async Void Issues
 * FILE: 05_AsyncVoid_Issues.cs
 * PURPOSE: Understanding problems with async void and proper alternatives
 */
using System;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._01_AsyncAwait
{
    public class AsyncVoidIssues
    {
        public static async Task Main()
        {
            Console.WriteLine("=== Async Void Issues Demo ===\n");

            var demo = new AsyncVoidIssues();

            // Example 1: Problem with async void - exceptions cannot be caught
            Console.WriteLine("1. Async void - exceptions cannot be caught:");
            demo.AsyncVoidMethod(); // Should NOT do this in production
            await Task.Delay(200);
            Console.WriteLine("   Main continued (exception was lost)");

            // Example 2: Correct approach - use async Task
            Console.WriteLine("\n2. Correct approach - async Task:");
            try
            {
                await demo.AsyncTaskMethodWithException();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   Caught properly: {ex.Message}");
            }

            // Example 3: Async void in event handlers (necessary evil)
            Console.WriteLine("\n3. Async void in event handlers:");
            demo.ButtonClickHandler(null, EventArgs.Empty);
            await Task.Delay(100);

            // Example 4: Fire-and-forget pattern alternatives
            Console.WriteLine("\n4. Fire-and-forget alternatives:");
            await demo.FireAndForgetCorrectlyAsync();
            Console.WriteLine("   Properly handled");

            // Example 5: Async void and exception handling
            Console.WriteLine("\n5. Unhandled exception in async void:");
            demo.AsyncVoidWithUnhandledException();
            await Task.Delay(100);
            Console.WriteLine("   Program continues (exception may crash later)");

            // Example 6: Async void and async state machine issues
            Console.WriteLine("\n6. Async void timing issues:");
            demo.AsyncVoidTimingIssue();
            Console.WriteLine("   Main immediate continuation (async may not have started)");

            // Example 7: Convert async void to async Task
            Console.WriteLine("\n7. Proper pattern - return Task:");
            await demo.ProperAsyncPatternAsync();
            Console.WriteLine("   Completed with proper handling");

            Console.WriteLine("\n=== End of Demo ===");
        }

        // WRONG: async void - problems with exceptions, testing, and error handling
        public async void AsyncVoidMethod()
        {
            await Task.Delay(100);
            Console.WriteLine("   AsyncVoidMethod completed");
        }

        // CORRECT: async Task - allows proper await and exception handling
        public async Task AsyncTaskMethodWithException()
        {
            await Task.Delay(50);
            throw new InvalidOperationException("AsyncTaskMethod exception");
        }

        // This is acceptable - event handlers require void return
        public async void ButtonClickHandler(object sender, EventArgs e)
        {
            Console.WriteLine("   Button click started");
            await Task.Delay(100);
            Console.WriteLine("   Button click completed");
        }

        public async Task FireAndForgetCorrectlyAsync()
        {
            // Properly handle fire-and-forget with logging and error handling
            try
            {
                await Task.Delay(100);
                Console.WriteLine("   Fire-and-forget completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   Error (properly handled): {ex.Message}");
            }
        }

        public async void AsyncVoidWithUnhandledException()
        {
            await Task.Delay(50);
            throw new Exception("Unhandled async void exception");
        }

        public async void AsyncVoidTimingIssue()
        {
            await Task.Delay(100);
            Console.WriteLine("   AsyncVoidTimingIssue - late execution");
        }

        public async Task ProperAsyncPatternAsync()
        {
            await Task.Delay(50);
            Console.WriteLine("   Proper pattern - Task returned");
        }

        // Additional examples showing correct patterns

        public Task ProcessDataAsync(string data)
        {
            return Task.Run(async () =>
            {
                await Task.Delay(100);
                Console.WriteLine($"   Processed: {data}");
            });
        }

        public async Task<string> GetDataWithTimeoutAsync(TimeSpan timeout)
        {
            using var cts = new System.Threading.CancellationTokenSource(timeout);
            try
            {
                await Task.Delay(200, cts.Token);
                return "Data";
            }
            catch (OperationCanceledException)
            {
                return "Timeout";
            }
        }
    }

    // Real-world example: Proper async wrapper for legacy code
    public class LegacyEventProcessor
    {
        // Event handler - must be async void but handle exceptions properly
        public async void OnDataReceived(string data)
        {
            try
            {
                await ProcessDataAsync(data);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private async Task ProcessDataAsync(string data)
        {
            await Task.Delay(50);
            Console.WriteLine($"   Processed: {data}");
        }

        private void LogError(Exception ex)
        {
            Console.WriteLine($"   ERROR: {ex.Message}");
        }
    }

    // Best practice: Use async Task for everything possible
    public class AsyncService
    {
        public async Task<bool> DoWorkAsync()
        {
            await Task.Delay(100);
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            await Task.Delay(50);
            return 42;
        }

        // Exception: event handlers
        public event EventHandler<string> DataProcessed;
        
        public void RaiseDataProcessed(string data)
        {
            DataProcessed?.Invoke(this, data);
        }
    }
}