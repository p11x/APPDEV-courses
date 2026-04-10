/*
 * TOPIC: Parallel Programming
 * SUBTOPIC: Parallel.Invoke
 * FILE: 04_Parallel_Invoke.cs
 * PURPOSE: Understanding Parallel.Invoke for executing multiple actions in parallel
 */
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._04_ParallelProgramming
{
    public class ParallelInvokeDemo
    {
        public static void Main()
        {
            Console.WriteLine("=== Parallel.Invoke Demo ===\n");

            var demo = new ParallelInvokeDemo();

            // Example 1: Basic Parallel.Invoke
            Console.WriteLine("1. Basic Parallel.Invoke:");
            demo.BasicParallelInvoke();

            // Example 2: Multiple actions
            Console.WriteLine("\n2. Multiple actions:");
            demo.MultipleActionsDemo();

            // Example 3: Parallel.Invoke vs sequential
            Console.WriteLine("\n3. Performance comparison:");
            demo.PerformanceComparison();

            // Example 4: Exception handling
            Console.WriteLine("\n4. Exception handling:");
            demo.ExceptionHandlingDemo();

            // Example 5: With ParallelOptions
            Console.WriteLine("\n5. With ParallelOptions:");
            demo.ParallelOptionsDemo();

            // Example 6: Action array
            Console.WriteLine("\n6. Action array:");
            demo.ActionArrayDemo();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public void BasicParallelInvoke()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();

            Parallel.Invoke(
                () => { Thread.Sleep(50); Console.WriteLine("   Action 1"); },
                () => { Thread.Sleep(50); Console.WriteLine("   Action 2"); },
                () => { Thread.Sleep(50); Console.WriteLine("   Action 3"); }
            );

            sw.Stop();
            Console.WriteLine($"   Time: {sw.ElapsedMilliseconds}ms (sequential would be ~150ms)");
        }

        public void MultipleActionsDemo()
        {
            var actions = new Action[5];
            for (int i = 0; i < 5; i++)
            {
                int id = i;
                actions[i] = () => Console.WriteLine($"   Action {id} executed");
            }

            Parallel.Invoke(actions);
        }

        public void PerformanceComparison()
        {
            // Sequential
            var sw = System.Diagnostics.Stopwatch.StartNew();
            Action1(); Action2(); Action3(); Action4(); Action5();
            sw.Stop();
            Console.WriteLine($"   Sequential: {sw.ElapsedMilliseconds}ms");

            // Parallel
            sw.Restart();
            Parallel.Invoke(Action1, Action2, Action3, Action4, Action5);
            sw.Stop();
            Console.WriteLine($"   Parallel: {sw.ElapsedMilliseconds}ms");
        }

        private void Action1() { Thread.Sleep(30); }
        private void Action2() { Thread.Sleep(30); }
        private void Action3() { Thread.Sleep(30); }
        private void Action4() { Thread.Sleep(30); }
        private void Action5() { Thread.Sleep(30); }

        public void ExceptionHandlingDemo()
        {
            try
            {
                Parallel.Invoke(
                    () => { Thread.Sleep(20); },
                    () => { throw new Exception("Error in action 2"); },
                    () => { Thread.Sleep(20); }
                );
            }
            catch (AggregateException ae)
            {
                Console.WriteLine($"   Caught: {ae.InnerExceptions[0].Message}");
            }
        }

        public void ParallelOptionsDemo()
        {
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 2,
                CancellationToken = CancellationToken.None
            };

            Parallel.Invoke(options,
                () => { Thread.Sleep(30); Console.WriteLine("   Task A"); },
                () => { Thread.Sleep(30); Console.WriteLine("   Task B"); },
                () => { Thread.Sleep(30); Console.WriteLine("   Task C"); }
            );
        }

        public void ActionArrayDemo()
        {
            var actions = Enumerable.Range(1, 8)
                .Select(i => (Action)(() => Console.WriteLine($"   Task {i}")))
                .ToArray();

            Parallel.Invoke(actions);
        }
    }

    // Real-world examples
    public class ParallelDataLoader
    {
        public void LoadAllData()
        {
            Parallel.Invoke(
                () => LoadUsers(),
                () => LoadOrders(),
                () => LoadProducts()
            );
        }

        private void LoadUsers() { Thread.Sleep(50); Console.WriteLine("   Users loaded"); }
        private void LoadOrders() { Thread.Sleep(50); Console.WriteLine("   Orders loaded"); }
        private void LoadProducts() { Thread.Sleep(50); Console.WriteLine("   Products loaded"); }
    }

    public class ParallelReportGenerator
    {
        public void GenerateReport()
        {
            Parallel.Invoke(
                () => GenerateSalesReport(),
                () => GenerateInventoryReport(),
                () => GenerateCustomerReport()
            );
        }

        private void GenerateSalesReport() { Thread.Sleep(50); }
        private void GenerateInventoryReport() { Thread.Sleep(50); }
        private void GenerateCustomerReport() { Thread.Sleep(50); }
    }

    public class InitializationPipeline
    {
        public void Initialize()
        {
            Parallel.Invoke(
                InitializeCache,
                InitializeLogging,
                InitializeDatabase,
                InitializeConfig
            );
        }

        private void InitializeCache() { Thread.Sleep(30); Console.WriteLine("   Cache initialized"); }
        private void InitializeLogging() { Thread.Sleep(30); Console.WriteLine("   Logging initialized"); }
        private void InitializeDatabase() { Thread.Sleep(30); Console.WriteLine("   Database initialized"); }
        private void InitializeConfig() { Thread.Sleep(30); Console.WriteLine("   Config initialized"); }
    }
}