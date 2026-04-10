/*
 * TOPIC: Async/Await Fundamentals
 * SUBTOPIC: Async Return Types - Task Part 2
 * FILE: 04_AsyncReturn_Task_Part2.cs
 * PURPOSE: Advanced Task return patterns including generic types, patterns, and best practices
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._01_AsyncAwait
{
    public class AsyncReturnTaskPart2
    {
        public static async Task Main()
        {
            Console.WriteLine("=== Async Return Types - Task Part 2 Demo ===\n");

            var demo = new AsyncReturnTaskPart2();

            // Example 1: Task<T> with nullable types
            Console.WriteLine("1. Task<T> with nullable types:");
            var nullableResult = await demo.GetOptionalValueAsync();
            Console.WriteLine($"   Result: {nullableResult ?? "null"}");

            // Example 2: Task<T> with Dictionary
            Console.WriteLine("\n2. Task<T> with Dictionary:");
            var dict = await demo.ReturnsDictionaryAsync();
            foreach (var kvp in dict)
                Console.WriteLine($"   {kvp.Key}: {kvp.Value}");

            // Example 3: Task<bool> for success/failure
            Console.WriteLine("\n3. Task<bool> for success/failure:");
            bool success = await demo.TryOperationAsync();
            Console.WriteLine($"   Operation success: {success}");

            // Example 4: Task<T> with Result pattern
            Console.WriteLine("\n4. Task<T> with Result pattern:");
            var result = await demo.GetResultAsync();
            Console.WriteLine($"   IsSuccess: {result.IsSuccess}, Value: {result.Value}");

            // Example 5: Task<T> with custom wrapper
            Console.WriteLine("\n5. Task<T> with API response wrapper:");
            var apiResponse = await demo.FetchApiResponseAsync();
            Console.WriteLine($"   Status: {apiResponse.StatusCode}, Data: {apiResponse.Data}");

            // Example 6: Chained async operations
            Console.WriteLine("\n6. Chained async operations:");
            var chainResult = await demo.ChainOperationsAsync();
            Console.WriteLine($"   Chain result: {chainResult}");

            // Example 7: Task with tuple returns
            Console.WriteLine("\n7. Task with tuple returns:");
            var (count, total, average) = await demo.GetStatisticsAsync();
            Console.WriteLine($"   Count: {count}, Total: {total}, Avg: {average}");

            // Example 8: Async method returning Task<Task> (nested tasks)
            Console.WriteLine("\n8. Nested Task pattern:");
            var innerTask = await demo.GetNestedTaskAsync();
            await innerTask;
            Console.WriteLine("   Inner task completed");

            Console.WriteLine("\n=== End of Demo ===");
        }

        public async Task<int?> GetOptionalValueAsync()
        {
            await Task.Delay(50);
            Random rand = new Random();
            return rand.Next(2) == 0 ? (int?)null : 42;
        }

        public async Task<Dictionary<string, int>> ReturnsDictionaryAsync()
        {
            await Task.Delay(50);
            return new Dictionary<string, int>
            {
                { "One", 1 },
                { "Two", 2 },
                { "Three", 3 }
            };
        }

        public async Task<bool> TryOperationAsync()
        {
            await Task.Delay(50);
            return true; // Simulate success
        }

        public async Task<Result<int>> GetResultAsync()
        {
            await Task.Delay(50);
            return Result<int>.Success(100);
        }

        public async Task<ApiResponse<string>> FetchApiResponseAsync()
        {
            await Task.Delay(100);
            return new ApiResponse<string>
            {
                StatusCode = 200,
                Data = "Success",
                IsSuccess = true
            };
        }

        public async Task<string> ChainOperationsAsync()
        {
            string step1 = await Step1Async();
            string step2 = await Step2Async(step1);
            string step3 = await Step3Async(step2);
            return step3;
        }

        private async Task<string> Step1Async() { await Task.Delay(30); return "Step1"; }
        private async Task<string> Step2Async(string input) { await Task.Delay(30); return $"{input}->Step2"; }
        private async Task<string> Step3Async(string input) { await Task.Delay(30); return $"{input}->Step3"; }

        public async Task<(int Count, int Total, double Average)> GetStatisticsAsync()
        {
            await Task.Delay(50);
            var numbers = new[] { 10, 20, 30, 40, 50 };
            return (numbers.Length, numbers.Sum(), numbers.Average());
        }

        public async Task<Task> GetNestedTaskAsync()
        {
            await Task.Delay(50);
            return Task.CompletedTask;
        }

        // Real-world examples

        public async Task<Product> GetProductAsync(int id)
        {
            await Task.Delay(100);
            return new Product { Id = id, Name = $"Product {id}", Price = id * 10.99m };
        }

        public async Task<List<OrderItem>> GetOrderItemsAsync(int orderId)
        {
            await Task.Delay(100);
            return new List<OrderItem>
            {
                new OrderItem { ProductId = 1, Quantity = 2, Price = 19.99m },
                new OrderItem { ProductId = 2, Quantity = 1, Price = 49.99m }
            };
        }

        public async Task<bool> ProcessPaymentAsync(PaymentInfo payment)
        {
            await Task.Delay(200);
            Console.WriteLine($"   Processed payment: {payment.Amount:C}");
            return true;
        }
    }

    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T Value { get; private set; }
        public string Error { get; private set; }

        public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };
        public static Result<T> Failure(string error) => new Result<T> { IsSuccess = false, Error = error };
    }

    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class PaymentInfo
    {
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
    }
}