/*
 * TOPIC: Async/Await Fundamentals
 * SUBTOPIC: Async/Await Basics
 * FILE: 01_AsyncAwaitBasics.cs
 * PURPOSE: Introduction to async/await keywords and basic async patterns
 */
using System;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._01_AsyncAwait
{
    public class AsyncAwaitBasics
    {
        public static async Task Main()
        {
            Console.WriteLine("=== Async/Await Basics Demo ===\n");

            var demo = new AsyncAwaitBasics();

            // Example 1: Basic async method call
            Console.WriteLine("1. Calling async method synchronously:");
            demo.SimpleAsyncMethod();

            // Example 2: Awaiting async method
            Console.WriteLine("\n2. Awaiting async method:");
            await demo.SimpleAwaitableMethod();

            // Example 3: Async method returning value
            Console.WriteLine("\n3. Async method returning value:");
            int result = await demo.AsyncMethodWithReturn();
            Console.WriteLine($"   Result: {result}");

            // Example 4: Multiple async calls
            Console.WriteLine("\n4. Multiple sequential async calls:");
            await demo.FirstAsyncMethod();
            await demo.SecondAsyncMethod();

            // Example 5: Using async lambda
            Console.WriteLine("\n5. Using async lambda:");
            Func<Task> asyncLambda = async () =>
            {
                await Task.Delay(100);
                Console.WriteLine("   Lambda executed");
            };
            await asyncLambda();

            // Example 6: Fire and forget (not recommended for void)
            Console.WriteLine("\n6. Fire and forget pattern:");
            _ = demo.AsyncMethodWithReturn();

            // Example 7: Async method with try-catch
            Console.WriteLine("\n7. Async method with error handling:");
            try
            {
                await demo.AsyncMethodWithException();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   Caught: {ex.Message}");
            }

            Console.WriteLine("\n=== End of Demo ===");
        }

        public async void SimpleAsyncMethod()
        {
            Console.WriteLine("   SimpleAsyncMethod started");
            await Task.Delay(100);
            Console.WriteLine("   SimpleAsyncMethod completed");
        }

        public async Task SimpleAwaitableMethod()
        {
            Console.WriteLine("   SimpleAwaitableMethod started");
            await Task.Delay(100);
            Console.WriteLine("   SimpleAwaitableMethod completed");
        }

        public async Task<int> AsyncMethodWithReturn()
        {
            Console.WriteLine("   AsyncMethodWithReturn started");
            await Task.Delay(100);
            int value = 42;
            Console.WriteLine($"   AsyncMethodWithReturn returning {value}");
            return value;
        }

        public async Task FirstAsyncMethod()
        {
            Console.WriteLine("   FirstAsyncMethod running...");
            await Task.Delay(50);
            Console.WriteLine("   FirstAsyncMethod done");
        }

        public async Task SecondAsyncMethod()
        {
            Console.WriteLine("   SecondAsyncMethod running...");
            await Task.Delay(50);
            Console.WriteLine("   SecondAsyncMethod done");
        }

        public async Task AsyncMethodWithException()
        {
            await Task.Delay(50);
            throw new InvalidOperationException("Async exception occurred");
        }

        // Additional demonstration methods

        public async Task<int> CalculateAsync(int a, int b)
        {
            await Task.Delay(100);
            return a + b;
        }

        public async Task<string> GetDataAsync()
        {
            await Task.Delay(100);
            return "Async Data";
        }

        public async Task<bool> ValidateAsync(string input)
        {
            await Task.Delay(50);
            return !string.IsNullOrEmpty(input);
        }
    }

    // Additional example class for more complex scenarios
    public class AsyncOperationDemo
    {
        public async Task<string> ProcessDataAsync(string data)
        {
            Console.WriteLine($"   Processing: {data}");
            await Task.Delay(100);
            return $"Processed: {data.ToUpper()}";
        }

        public async Task<double> ComputeAsync(double[] values)
        {
            await Task.Delay(50);
            double sum = 0;
            foreach (var v in values)
                sum += v;
            return sum / values.Length;
        }

        public async Task ChainOperationsAsync()
        {
            var result1 = await ProcessDataAsync("step1");
            var result2 = await ProcessDataAsync("step2");
            var result3 = await ProcessDataAsync("step3");
            Console.WriteLine($"   Results: {result1}, {result2}, {result3}");
        }
    }
}