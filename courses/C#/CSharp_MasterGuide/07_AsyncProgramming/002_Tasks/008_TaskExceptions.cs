/*
 * TOPIC: Task Parallel Library
 * SUBTOPIC: Task Exceptions
 * FILE: 08_TaskExceptions.cs
 * PURPOSE: Understanding exception handling in Tasks, AggregateException, and faulted tasks
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._02_Tasks
{
    public class TaskExceptions
    {
        public static void Main()
        {
            Console.WriteLine("=== Task Exceptions Demo ===\n");

            var demo = new TaskExceptions();

            // Example 1: Basic task exception
            Console.WriteLine("1. Basic task exception:");
            demo.BasicExceptionDemo();

            // Example 2: AggregateException
            Console.WriteLine("\n2. AggregateException:");
            demo.AggregateExceptionDemo();

            // Example 3: Handling multiple exceptions
            Console.WriteLine("\n3. Handling multiple exceptions:");
            demo.MultipleExceptionsDemo();

            // Example 4: Task Exception handling patterns
            Console.WriteLine("\n4. Exception handling patterns:");
            demo.ExceptionHandlingPatternsDemo();

            // Example 5: Unobserved task exception
            Console.WriteLine("\n5. Unobserved task exception:");
            demo.UnobservedExceptionDemo();

            // Example 6: Exception in ContinueWith
            Console.WriteLine("\n6. Exception in ContinueWith:");
            demo.ContinueWithExceptionDemo();

            // Example 7: Flattening AggregateException
            Console.WriteLine("\n7. Flattening AggregateException:");
            demo.FlattenExceptionDemo();

            // Example 8: Custom exception handling
            Console.WriteLine("\n8. Custom exception handling:");
            demo.CustomExceptionHandlingDemo();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public void BasicExceptionDemo()
        {
            var task = Task.Run(() =>
            {
                throw new InvalidOperationException("Basic exception");
            });

            try
            {
                task.Wait();
            }
            catch (AggregateException ae)
            {
                Console.WriteLine($"   Caught: {ae.InnerException.Message}");
                Console.WriteLine($"   Task status: {task.Status}");
            }
        }

        public void AggregateExceptionDemo()
        {
            var tasks = new Task[3];
            tasks[0] = Task.Run(() => throw new Exception("Error 1"));
            tasks[1] = Task.Run(() => throw new Exception("Error 2"));
            tasks[2] = Task.Run(() => { }); // Success

            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException ae)
            {
                Console.WriteLine($"   Exception count: {ae.InnerExceptions.Count}");
                foreach (var ex in ae.InnerExceptions)
                    Console.WriteLine($"   - {ex.Message}");
            }
        }

        public void MultipleExceptionsDemo()
        {
            var tasks = new Task[5];
            for (int i = 0; i < 5; i++)
            {
                int id = i;
                tasks[i] = Task.Run(() =>
                {
                    if (id % 2 == 0)
                        throw new Exception($"Task {id} failed");
                });
            }

            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException ae)
            {
                var flattened = ae.Flatten().InnerExceptions;
                Console.WriteLine($"   Total exceptions: {flattened.Count}");
            }
        }

        public void ExceptionHandlingPatternsDemo()
        {
            var task = Task.Run(() =>
            {
                throw new Exception("Test exception");
            });

            // Pattern 1: Check IsFaulted
            task.Wait();
            if (task.IsFaulted)
                Console.WriteLine($"   IsFaulted: {task.Exception.Message}");

            // Pattern 2: Use ContinueWith
            Task.Run(() => throw new Exception("ContinueWith test"))
                .ContinueWith(t => Console.WriteLine($"   ContinueWith error: {t.Exception.Message}"),
                    TaskContinuationOptions.OnlyOnFaulted)
                .Wait();

            // Pattern 3: Try to access Result (rethrows)
            var t = Task.Run(() => throw new Exception("Result access"));
            Thread.Sleep(100);
            try
            {
                _ = t.Result;
            }
            catch (AggregateException ae)
            {
                Console.WriteLine($"   Result access caught: {ae.InnerException.Message}");
            }
        }

        public void UnobservedExceptionDemo()
        {
            var task = Task.Run(() => throw new Exception("Unobserved"));

            // Unobserved task exception handler
            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Console.WriteLine($"   Unobserved: {e.Exception.Message}");
                e.SetObserved();
            };

            Thread.Sleep(100);
            task.Wait();
        }

        public void ContinueWithExceptionDemo()
        {
            var task = Task.Run(() =>
            {
                throw new Exception("Original exception");
            })
            .ContinueWith(t =>
            {
                Console.WriteLine($"   ContinueWith - IsFaulted: {t.IsFaulted}");
            });

            Thread.Sleep(100);
            Console.WriteLine($"   Main task status: {task.Status}");
        }

        public void FlattenExceptionDemo()
        {
            var innerTask = Task.Run(() =>
            {
                throw new Exception("Inner exception");
            });

            var outerTask = Task.Run(() =>
            {
                try
                {
                    innerTask.Wait();
                }
                catch
                {
                    throw new Exception("Outer exception", innerTask.Exception);
                }
            });

            try
            {
                outerTask.Wait();
            }
            catch (AggregateException ae)
            {
                var flattened = ae.Flatten();
                Console.WriteLine($"   Flattened count: {flattened.InnerExceptions.Count}");
            }
        }

        public void CustomExceptionHandlingDemo()
        {
            var task = Task.Run(() =>
            {
                throw new ArgumentException("Invalid argument");
            });

            try
            {
                task.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                {
                    switch (ex)
                    {
                        case ArgumentException ae_:
                            Console.WriteLine($"   Argument error: {ae_.ParamName}");
                            break;
                        case InvalidOperationException ioe:
                            Console.WriteLine($"   Invalid operation: {ioe.Message}");
                            break;
                        default:
                            Console.WriteLine($"   Other: {ex.Message}");
                            break;
                    }
                }
            }
        }
    }

    // Real-world exception handling
    public class ResilientTaskRunner
    {
        public async Task<T> RunWithRetryAsync<T>(
            Func<Task<T>> operation,
            int maxRetries = 3,
            Action<Exception> onRetry = null)
        {
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    return await operation();
                }
                catch (Exception ex)
                {
                    if (attempt == maxRetries) throw;
                    onRetry?.Invoke(ex);
                    await Task.Delay(100 * attempt);
                }
            }
            throw new Exception("Should not reach here");
        }

        public async Task RunWithFallbackAsync(
            Func<Task> primary,
            Func<Task> fallback)
        {
            try
            {
                await primary();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   Primary failed: {ex.Message}");
                await fallback();
            }
        }

        public async Task<T> RunWithCircuitBreakerAsync<T>(
            Func<Task<T>> operation,
            int failureThreshold = 3,
            TimeSpan resetTimeout = default)
        {
            // Simplified circuit breaker pattern
            int failures = 0;
            bool isOpen = false;

            try
            {
                return await operation();
            }
            catch
            {
                failures++;
                if (failures >= failureThreshold)
                    isOpen = true;
                throw;
            }
        }
    }

    public class ExceptionLoggingService
    {
        public void LogTaskException(Task task)
        {
            if (!task.IsFaulted) return;

            foreach (var ex in task.Exception.InnerExceptions)
            {
                Console.WriteLine($"   LOG: {ex.GetType().Name}: {ex.Message}");
            }
        }

        public async Task HandleTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                await LogExceptionAsync(ex);
            }
        }

        private Task LogExceptionAsync(Exception ex)
        {
            Console.WriteLine($"   Logging: {ex.Message}");
            return Task.CompletedTask;
        }
    }
}