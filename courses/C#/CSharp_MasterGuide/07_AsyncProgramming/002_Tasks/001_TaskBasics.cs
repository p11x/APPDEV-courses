/*
 * ============================================================
 * TOPIC     : Asynchronous Programming
 * SUBTOPIC  : Task Basics
 * FILE      : 01_TaskBasics.cs
 * PURPOSE   : Introduction to Task.Run, Start, Wait operations and basic async patterns
 * ============================================================
 */
using System; // needed for Console.WriteLine and basic types
using System.Threading; // needed for Thread.Sleep, Thread.CurrentThread
using System.Threading.Tasks; // needed for Task, Task<T>

namespace CSharp_MasterGuide._07_AsyncProgramming._02_Tasks
{
    /// <summary>
    /// Demonstrates fundamental Task operations including creation, execution, and waiting
    /// </summary>
    public class TaskBasics
    {
        /// <summary>
        /// Entry point demonstrating basic Task operations
        /// </summary>
        public static void Main()
        {
            // Console.WriteLine = outputs message to console
            // Output: === Task Basics Demo ===
            Console.WriteLine("=== Task Basics Demo ===\n");

            // Create new instance of TaskBasics for method calls
            var demo = new TaskBasics();

            // ── CONCEPT: Task.Run - Simplest way to start background work ─────
            // Task.Run queues work to the Thread Pool and returns a Task
            // This is the recommended way to run CPU-bound or I/O-bound work off the main thread

            // Example 1: Task.Run with lambda expression
            // Output: 1. Task.Run with lambda:
            Console.WriteLine("1. Task.Run with lambda:");
            
            // Task.Run takes Action delegate - executes lambda on thread pool
            // Thread.Sleep(100) = pause current thread for 100ms
            Task runTask = Task.Run(() =>
            {
                // Thread.CurrentThread.ManagedThreadId = unique ID for managed thread
                // Output: Task.Run executing on thread [threadId]
                Console.WriteLine("   Task.Run executing on thread " + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(100); // 100ms delay to simulate work
            });
            
            // Task.Wait() = blocks calling thread until task completes
            // Output: Task.Run completed
            runTask.Wait();
            Console.WriteLine("   Task.Run completed");

            // ── CONCEPT: Task with Return Value ────────────────────────────────
            // Task<T> allows returning a value from async work
            // .Result property blocks until result is available

            // Example 2: Task<T> with return value
            // Output: 2. Task with return value:
            Console.WriteLine("\n2. Task with return value:");
            
            // Task.Run<Func<int>> returns Task<int> - the generic version
            // Thread.Sleep(50) = 50ms delay
            // return 42 = the result that will be returned
            Task<int> taskWithResult = Task.Run(() =>
            {
                Thread.Sleep(50);
                return 42; // arbitrary return value
            });
            
            // .Result = blocking property that waits for and returns the result
            // int result = receives the value returned from Task
            int result = taskWithResult.Result;
            
            // Output: Result: 42
            Console.WriteLine($"   Result: {result}");

            // ── CONCEPT: Task.Start - Explicit Task Control ───────────────────
            // new Task() creates Task but does NOT start it
            // Must call Start() explicitly to begin execution

            // Example 3: Task.Start for explicit control
            // Output: 3. Task.Start:
            Console.WriteLine("\n3. Task.Start:");
            
            // new Task(Action) = creates task but doesn't start it yet
            // Useful when you need to configure before starting
            var explicitTask = new Task(() =>
            {
                // Output: Explicit task starting
                Console.WriteLine("   Explicit task starting");
                Thread.Sleep(50);
                // Output: Explicit task finishing
                Console.WriteLine("   Explicit task finishing");
            });
            
            explicitTask.Start(); // Must call Start() explicitly
            explicitTask.Wait(); // Wait for completion
            // Output: Explicit task done
            Console.WriteLine("   Explicit task done");

            // ── CONCEPT: Task.Wait with Timeout ────────────────────────────────
            // Wait(timeout) returns true if completed, false if timed out
            // Non-blocking way to wait with time limit

            // Example 4: Task.Wait with timeout
            // Output: 4. Task.Wait with timeout:
            Console.WriteLine("\n4. Task.Wait with timeout:");
            
            var waitTask = Task.Run(() =>
            {
                Thread.Sleep(200); // 200ms work (longer than timeout)
                // Output: WaitTask completed
                Console.WriteLine("   WaitTask completed");
            });
            
            // Wait(100) = wait maximum 100ms
            // bool completed = true if task finished, false if timed out
            bool completed = waitTask.Wait(100);
            
            // Output: Wait returned: [bool], Task status: [status]
            Console.WriteLine($"   Wait returned: {completed}, Task status: {waitTask.Status}");

            // ── CONCEPT: Task.WaitAny - Wait for First Completion ─────────────
            // WaitAny blocks until ANY of the specified tasks complete
            // Returns index of completed task in the params array

            // Example 5: Task.WaitAny
            // Output: 5. Task.WaitAny:
            Console.WriteLine("\n5. Task.WaitAny:");
            
            // Create 3 tasks with different sleep durations
            // t1 sleeps 100ms, t2 sleeps 50ms, t3 sleeps 150ms
            // t2 will complete first (shortest time)
            var t1 = Task.Run(() => { Thread.Sleep(100); return 1; });
            var t2 = Task.Run(() => { Thread.Sleep(50); return 2; });
            var t3 = Task.Run(() => { Thread.Sleep(150); return 3; });
            
            // Task.WaitAny = blocks until first task completes
            // int index = index of first completed task in array
            int index = Task.WaitAny(t1, t2, t3);
            
            // Output: First completed: index [0, 1, or 2]
            Console.WriteLine($"   First completed: index {index}");

            // ── CONCEPT: TaskCreationOptions - Configure Task Behavior ─────────
            // LongRunning = hints to scheduler to run on dedicated thread
            // Useful for long-running blocking operations

            // Example 6: TaskCreationOptions.LongRunning
            // Output: 6. TaskCreationOptions:
            Console.WriteLine("\n6. TaskCreationOptions:");
            
            // TaskCreationOptions.LongRunning = hint to use separate thread
            // Thread.CurrentThread.IsThreadPoolThread = checks if running on thread pool
            var longRunningTask = Task.Run(() =>
            {
                // Output: Long running on thread [IsThreadPoolThread:True/False]
                Console.WriteLine($"   Long running on thread {Thread.CurrentThread.IsThreadPoolThread}");
                Thread.Sleep(50);
            }, TaskCreationOptions.LongRunning);
            
            longRunningTask.Wait();

            // ── CONCEPT: Task Exceptions - Handling Faulted Tasks ────────────
            // If Task throws, it becomes Faulted and wraps exception in AggregateException
            // Must catch to observe the original exception

            // Example 7: Task with exception
            // Output: 7. Task with exception:
            Console.WriteLine("\n7. Task with exception:");
            
            // Throwing inside Task marks it as Faulted
            var exceptionTask = Task.Run(() =>
            {
                throw new InvalidOperationException("Task exception!");
            });
            
            try
            {
                // Waiting on faulted task throws AggregateException
                exceptionTask.Wait();
            }
            catch (AggregateException ae)
            {
                // ae.InnerException = the original exception thrown
                // Output: Caught: Task exception!
                Console.WriteLine($"   Caught: {ae.InnerException.Message}");
            }

            // ── CONCEPT: ContinueWith - Chaining Tasks ────────────────────────
            // ContinueWith schedules another task to run after the first completes
            // Useful for sequential async operations

            // Example 8: ContinueWith
            // Output: 8. ContinueWith:
            Console.WriteLine("\n8. ContinueWith:");
            
            // Task.ContinueWith accepts Action<Task> - runs after antecedent completes
            // _ = parameter (antecedent task) that we ignore
            Task.ContinueWith(_ =>
            {
                // Output: ContinueWith executed
                Console.WriteLine("   ContinueWith executed");
            }).Wait(); // Chain Wait() to ensure completion

            // Output: === End of Demo ===
            Console.WriteLine("\n=== End of Demo ===");
        }
    }

    /// <summary>
    /// Additional Task operation examples for reference
    /// </summary>
    public class TaskOperations
    {
        /// <summary>
        /// Demonstrates various ways to create and start tasks
        /// </summary>
        public void DemonstrateTaskCreation()
        {
            // new Task(Action) = creates task without starting
            // Task.Run(Action) = creates and starts task in one call
            Task t1 = new Task(() => Console.WriteLine("Task 1"));
            Task t2 = new Task(() => Console.WriteLine("Task 2"), TaskCreationOptions.None);
            
            t1.Start(); // Start the manually created task
            t2.Start();
            
            // Task.WaitAll = blocks until ALL tasks complete
            Task.WaitAll(t1, t2);
        }

        /// <summary>
        /// Demonstrates Task.Run returning a value
        /// </summary>
        /// <returns>Task containing computed result</returns>
        public Task<int> CalculateAsync()
        {
            // Task.Run<Func<int>> = runs function on thread pool, returns Task<int>
            return Task.Run(() =>
            {
                Thread.Sleep(100);
                return 100; // computed result
            });
        }

        /// <summary>
        /// Demonstrates different Wait method overloads
        /// </summary>
        public void DemonstrateWaitMethods()
        {
            // Create a simple task
            var task = Task.Run(() => Thread.Sleep(50));
            
            // task.Wait() = blocks indefinitely until complete
            task.Wait();
            
            // task.Wait(TimeSpan) = blocks with timeout
            // TimeSpan.FromSeconds(5) = 5 second timeout
            task.Wait(TimeSpan.FromSeconds(5));
            
            // task.Wait(CancellationToken) = can be cancelled
            // CancellationToken.None = non-cancellable token
            task.Wait(CancellationToken.None);
        }
    }

    // ── REAL-WORLD EXAMPLE: Background Data Processing ─────────────────────
    /// <summary>
    /// Real-world pattern: Background data processing service
    /// Demonstrates how to use Task.Run for background work in production applications
    /// </summary>
    public class DataProcessor
    {
        /// <summary>
        /// Processes data array in background without blocking main thread
        /// </summary>
        /// <param name="data">Array of data items to process</param>
        /// <returns>Task that completes when processing finishes</returns>
        public Task ProcessDataAsync(string[] data)
        {
            // Task.Run = queue work to thread pool for background processing
            // return Task = allows caller to await completion
            return Task.Run(() =>
            {
                // foreach = iterate through all items
                foreach (var item in data)
                {
                    // Console.WriteLine = output progress
                    Console.WriteLine($"   Processing: {item}");
                    Thread.Sleep(20); // Simulate processing time
                }
            });
        }

        /// <summary>
        /// Simulates fetching data from server asynchronously
        /// </summary>
        /// <returns>Task containing fetched data string</returns>
        public Task<string> FetchDataAsync()
        {
            // Return Task<string> for async data retrieval pattern
            return Task.Run(() =>
            {
                Thread.Sleep(100); // Simulate network delay
                return "Fetched data from server";
            });
        }
    }
}
