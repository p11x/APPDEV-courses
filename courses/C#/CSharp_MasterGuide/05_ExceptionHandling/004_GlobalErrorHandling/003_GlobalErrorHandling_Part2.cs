/*
 * TOPIC: Global Error Handling in C#
 * SUBTOPIC: TaskScheduler.UnobservedTaskException
 * FILE: GlobalErrorHandling_Part2.cs
 * PURPOSE: Demonstrate handling unobserved exceptions in async/Task operations using TaskScheduler.UnobservedTaskException
 */

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._05_ExceptionHandling._04_GlobalErrorHandling
{
    /// <summary>
    /// Demonstrates the UnobservedTaskException event for handling exceptions in async operations
    /// that are not explicitly observed
    /// </summary>
    public class GlobalErrorHandlingPart2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== TaskScheduler UnobservedTaskException Demo ===\n");

            // Register the UnobservedTaskException event handler
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

            // Example 1: Task that throws and is not awaited
            Console.WriteLine("Example 1: Fire-and-forget task with unobserved exception");
            FireAndForgetTask();

            // Allow time for the exception to propagate
            Thread.Sleep(100);

            // Example 2: Multiple fire-and-forget tasks
            Console.WriteLine("\nExample 2: Multiple tasks with unobserved exceptions");
            MultipleFireAndForgetTasks();

            Thread.Sleep(100);

            // Example 3: Using ContinueWith without proper exception handling
            Console.WriteLine("\nExample 3: Task with ContinueWith and unobserved exception");
            TaskWithContinueWith();

            Thread.Sleep(100);

            // Example 4: Nested async operations
            Console.WriteLine("\nExample 4: Nested async operations with unobserved exceptions");
            NestedAsyncOperations();
        }

        static void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Console.WriteLine($"[UnobservedTaskException Handler] Caught: {e.Exception.Message}");
            Console.WriteLine($"[UnobservedTaskException Handler] Inner Exception: {e.Exception.InnerException?.Message}");
            
            // Mark the exception as observed to prevent it from escalating
            e.SetObserved();
            
            // Log the exception
            LogUnobservedTaskException(e.Exception);
        }

        static async void FireAndForgetTask()
        {
            // Fire-and-forget - exception will go to UnobservedTaskException
            await Task.Run(() =>
            {
                throw new InvalidOperationException("Exception in fire-and-forget task");
            });
        }

        static void MultipleFireAndForgetTasks()
        {
            // Create multiple tasks that throw exceptions
            for (int i = 1; i <= 3; i++)
            {
                int taskId = i;
                Task.Run(() =>
                {
                    if (taskId == 2)
                    {
                        throw new ArgumentException($"Exception in task {taskId}");
                    }
                });
            }
        }

        static async void TaskWithContinueWith()
        {
            // Using ContinueWith without proper exception handling
            Task.Run(() => 42)
                .ContinueWith antecedent =>
                {
                    throw new DivideByZeroException("Exception in ContinueWith");
                });
        }

        static void NestedAsyncOperations()
        {
            // Nested fire-and-forget tasks
            Task.Run(() =>
            {
                Task.Run(() =>
                {
                    throw new TimeoutException("Exception in nested task");
                });
            });
        }

        static void LogUnobservedTaskException(Exception ex)
        {
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "unobserved_task_errors.log");
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex.GetType().Name}: {ex.Message}\n";
            
            if (ex.InnerException != null)
            {
                logMessage += $"Inner: {ex.InnerException.Message}\n";
            }
            
            logMessage += "\n";
            
            try
            {
                File.AppendAllText(logPath, logMessage);
                Console.WriteLine($"[UnobservedTaskException Handler] Logged to {logPath}");
            }
            catch
            {
                Console.WriteLine("Failed to log exception");
            }
        }
    }

    /// <summary>
    /// Real-world example: Proper async exception handling patterns
    /// </summary>
    public class AsyncExceptionHandlingRealWorld
    {
        // Queue to track background tasks
        private static readonly Queue<Task> _backgroundTasks = new Queue<Task>();

        public static void Main(string[] args)
        {
            Console.WriteLine("=== Real-World Async Exception Handling ===\n");

            // Register global unobserved task exception handler
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

            // Initialize background task manager
            InitializeBackgroundTaskManager();

            // Start background tasks
            StartBackgroundTasks();

            // Simulate main application work
            RunMainApplication();

            // Wait for background tasks
            Thread.Sleep(200);

            // Cleanup
            Console.WriteLine("\nCleaning up background tasks...");
            CleanupBackgroundTasks();
        }

        static void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Console.WriteLine("\n[Global Handler] Unobserved task exception:");
            Console.WriteLine($"  {e.Exception.Message}");
            
            // Log to error tracking service
            LogErrorToTrackingService(e.Exception);
            
            // Mark as observed to prevent app crash
            e.SetObserved();
        }

        static void InitializeBackgroundTaskManager()
        {
            Console.WriteLine("Initializing background task manager...");
        }

        static void StartBackgroundTasks()
        {
            Console.WriteLine("Starting background tasks...");

            // Background data sync task
            var syncTask = Task.Run(() =>
            {
                Thread.Sleep(50);
                throw new IOException("Failed to sync data with server");
            });
            _backgroundTasks.Enqueue(syncTask);

            // Background cleanup task
            var cleanupTask = Task.Run(() =>
            {
                throw new UnauthorizedAccessException("Access denied during cleanup");
            });
            _backgroundTasks.Enqueue(cleanupTask);

            // Background notification task
            var notificationTask = Task.Run(() =>
            {
                throw new InvalidOperationException("Notification service unavailable");
            });
            _backgroundTasks.Enqueue(notificationTask);
        }

        static void RunMainApplication()
        {
            Console.WriteLine("Main application running...");

            // Create a fire-and-forget task that throws
            Task.Run(() =>
            {
                throw new TimeoutException("Network timeout in main task");
            });
        }

        static void CleanupBackgroundTasks()
        {
            Console.WriteLine("Cleaning up any orphaned tasks...");
            _backgroundTasks.Clear();
        }

        static void LogErrorToTrackingService(Exception ex)
        {
            // In real-world, this would send to error tracking service (e.g., Sentry, Application Insights)
            Console.WriteLine($"  [Error Tracking] Error logged: {ex.GetType().Name}");
            Console.WriteLine($"  [Error Tracking] Timestamp: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");
        }
    }

    /// <summary>
    /// Demonstrates proper async/await exception handling patterns
    /// </summary>
    public class AsyncProperExceptionHandling
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("=== Proper Async Exception Handling Patterns ===\n");

            // Pattern 1: Try-catch with async
            Console.WriteLine("Pattern 1: Try-catch with async/await");
            await HandleAsyncException();

            // Pattern 2: Using Task.ContinueWith for exception handling
            Console.WriteLine("\nPattern 2: Using ContinueWith for exceptions");
            await HandleWithContinueWith();

            // Pattern 3: Using WhenAll with exception handling
            Console.WriteLine("\nPattern 3: Using WhenAll to observe all exceptions");
            await HandleMultipleTasksWithWhenAll();
        }

        static async Task HandleAsyncException()
        {
            try
            {
                await Task.Run(() =>
                {
                    throw new InvalidOperationException("Async operation failed");
                });
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Caught: {ex.Message}");
            }
        }

        static Task HandleWithContinueWith()
        {
            return Task.Run(() => { throw new Exception("Test exception"); })
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Console.WriteLine($"Handled in ContinueWith: {task.Exception?.InnerException?.Message}");
                    }
                }, TaskContinuationOptions.OnlyOnFaulted);
        }

        static async Task HandleMultipleTasksWithWhenAll()
        {
            var tasks = new[]
            {
                Task.Run(() => { throw new Exception("Error 1"); }),
                Task.Run(() => { throw new Exception("Error 2"); }),
                Task.Run(() => { throw new Exception("Error 3"); })
            };

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Caught {tasks.Count(t => t.IsFaulted)} exceptions");
                foreach (var task in tasks.Where(t => t.IsFaulted))
                {
                    Console.WriteLine($"  - {task.Exception?.InnerException?.Message}");
                }
            }
        }
    }
}
