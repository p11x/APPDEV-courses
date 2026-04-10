/*
 * TOPIC: Task Parallel Library
 * SUBTOPIC: Task Basics Part 2
 * FILE: 02_TaskBasics_Part2.cs
 * PURPOSE: Advanced Task patterns, TaskFactory, and task coordination
 */
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._02_Tasks
{
    public class TaskBasicsPart2
    {
        public static void Main()
        {
            Console.WriteLine("=== Task Basics Part 2 Demo ===\n");

            var demo = new TaskBasicsPart2();

            // Example 1: TaskFactory
            Console.WriteLine("1. TaskFactory:");
            var factory = new TaskFactory();
            var factoryTask = factory.StartNew(() =>
            {
                Console.WriteLine("   Factory task executing");
                return 42;
            });
            Console.WriteLine($"   Factory result: {factoryTask.Result}");

            // Example 2: Task.FromResult
            Console.WriteLine("\n2. Task.FromResult (completed task):");
            Task<string> completedTask = Task.FromResult("Immediate result");
            Console.WriteLine($"   Completed task result: {completedTask.Result}");

            // Example 3: Task.FromCanceled
            Console.WriteLine("\n3. Task.FromCanceled:");
            var cts = new CancellationTokenSource();
            cts.Cancel();
            Task cancelledTask = Task.FromCanceled(cts.Token);
            Console.WriteLine($"   Cancelled task status: {cancelledTask.Status}");

            // Example 4: Task.FromException
            Console.WriteLine("\n4. Task.FromException:");
            Task<string> exceptionalTask = Task.FromException<string>(
                new Exception("Exception task"));
            try
            {
                _ = exceptionalTask.Result;
            }
            catch (AggregateException ae)
            {
                Console.WriteLine($"   Caught: {ae.InnerException.Message}");
            }

            // Example 5: TaskScheduler
            Console.WriteLine("\n5. Custom TaskScheduler:");
            var scheduler = new LimitedConcurrencyTaskScheduler(2);
            var limitedFactory = new TaskFactory(scheduler);
            
            var tasks = new Task[4];
            for (int i = 0; i < 4; i++)
            {
                int taskId = i;
                tasks[i] = limitedFactory.StartNew(() =>
                {
                    Console.WriteLine($"   Task {taskId} on thread {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(50);
                });
            }
            Task.WaitAll(tasks);

            // Example 6: Unwrap for nested tasks
            Console.WriteLine("\n6. Task.Unwrap:");
            Task<Task<string>> nestedTask = Task.Run(() => Task.Run(() => "Nested result"));
            Task<string> unwrappedTask = nestedTask.Unwrap();
            Console.WriteLine($"   Unwrapped result: {unwrappedTask.Result}");

            // Example 7: Task.Delay
            Console.WriteLine("\n7. Task.Delay:");
            Task delayTask = Task.Delay(50);
            delayTask.Wait();
            Console.WriteLine("   Task.Delay completed");

            // Example 8: Task with progress
            Console.WriteLine("\n8. Task with progress reporting:");
            demo.ProcessWithProgressAsync().Wait();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public async Task ProcessWithProgressAsync()
        {
            var progress = new Progress<int>(p =>
            {
                Console.WriteLine($"   Progress: {p}%");
            });

            await Task.Run(() =>
            {
                for (int i = 0; i <= 10; i++)
                {
                    Thread.Sleep(30);
                    progress.Report(i * 10);
                }
            });
        }
    }

    // Custom TaskScheduler for limited concurrency
    public class LimitedConcurrencyTaskScheduler : TaskScheduler
    {
        private readonly int _maxConcurrency;
        private readonly LinkedList<Task> _tasks = new();
        private int _running;

        public LimitedConcurrencyTaskScheduler(int maxConcurrency)
        {
            _maxConcurrency = maxConcurrency;
        }

        protected override void QueueTask(Task task)
        {
            lock (_tasks)
            {
                _tasks.AddLast(task);
                if (_running < _maxConcurrency)
                {
                    _running++;
                    RunTask(task);
                }
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            lock (_tasks)
                return _tasks.ToArray();
        }

        private void RunTask(Task task)
        {
            Task.Run(() =>
            {
                TryExecuteTask(task);
                lock (_tasks)
                {
                    _tasks.Remove(task);
                    if (_tasks.Count > 0)
                        RunTask(_tasks.First.Value);
                    else
                        _running--;
                }
            });
        }
    }

    // Real-world examples
    public class TaskFactoryExamples
    {
        public Task<string> GetDataAsync()
        {
            var factory = new TaskFactory<string>();
            return factory.StartNew(() =>
            {
                Thread.Sleep(100);
                return "Data loaded";
            });
        }

        public Task RunWithCancellationTokenAsync(CancellationToken token)
        {
            return Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(50);
                }
            }, token);
        }

        public Task<T> CreateLazyTask<T>(Func<T> factory)
        {
            return Task.Run(factory);
        }
    }
}