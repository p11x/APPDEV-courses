/*
 * TOPIC: Async/Await Fundamentals
 * SUBTOPIC: Async Return Types - Task
 * FILE: 03_AsyncReturn_Task.cs
 * PURPOSE: Understanding Task return types in async methods
 */
using System;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._01_AsyncAwait
{
    public class AsyncReturnTask
    {
        public static async Task Main()
        {
            Console.WriteLine("=== Async Return Types - Task Demo ===\n");

            var demo = new AsyncReturnTask();

            // Example 1: Task without return value
            Console.WriteLine("1. Task (no return value):");
            await demo.NoReturnValueAsync();
            Console.WriteLine("   Completed");

            // Example 2: Task<T> with generic type
            Console.WriteLine("\n2. Task<T> with generic type:");
            int intResult = await demo.ReturnsIntAsync();
            Console.WriteLine($"   Result: {intResult}");

            string stringResult = await demo.ReturnsStringAsync();
            Console.WriteLine($"   Result: {stringResult}");

            // Example 3: Task<T> with complex types
            Console.WriteLine("\n3. Task<T> with complex types:");
            var person = await demo.ReturnsObjectAsync();
            Console.WriteLine($"   Person: {person.Name}, Age: {person.Age}");

            // Example 4: Task<T> with collections
            Console.WriteLine("\n4. Task<T> with collections:");
            var items = await demo.ReturnsArrayAsync();
            foreach (var item in items)
                Console.WriteLine($"   Item: {item}");

            // Example 5: Task as return type for fire-and-forget
            Console.WriteLine("\n5. Task for async void pattern (fire-and-forget):");
            await demo.FireAndForgetPatternAsync();
            Console.WriteLine("   Main continued");

            // Example 6: Multiple await in sequence
            Console.WriteLine("\n6. Multiple sequential awaits:");
            var combined = await demo.MultipleAwaitsAsync();
            Console.WriteLine($"   Combined result: {combined}");

            // Example 7: Conditional await
            Console.WriteLine("\n7. Conditional await:");
            int conditionalResult = await demo.ConditionalAwaitAsync(useCache: true);
            Console.WriteLine($"   Conditional result: {conditionalResult}");

            Console.WriteLine("\n=== End of Demo ===");
        }

        public async Task NoReturnValueAsync()
        {
            Console.WriteLine("   NoReturnValueAsync started");
            await Task.Delay(100);
            Console.WriteLine("   NoReturnValueAsync finished");
        }

        public async Task<int> ReturnsIntAsync()
        {
            await Task.Delay(50);
            return 123;
        }

        public async Task<string> ReturnsStringAsync()
        {
            await Task.Delay(50);
            return "Hello, Async World!";
        }

        public async Task<Person> ReturnsObjectAsync()
        {
            await Task.Delay(50);
            return new Person { Name = "Alice", Age = 30 };
        }

        public async Task<string[]> ReturnsArrayAsync()
        {
            await Task.Delay(50);
            return new[] { "Apple", "Banana", "Cherry" };
        }

        public async Task FireAndForgetPatternAsync()
        {
            await Task.Delay(100);
            Console.WriteLine("   Fire-and-forget completed");
        }

        public async Task<string> MultipleAwaitsAsync()
        {
            string part1 = await ReturnsStringAsync();
            string part2 = await ReturnsIntAsync().ContinueWith(i => i.Result.ToString());
            return $"{part1} - {part2}";
        }

        public async Task<int> ConditionalAwaitAsync(bool useCache)
        {
            if (useCache)
            {
                await Task.Delay(10);
                return 999; // Cached value
            }
            return await ReturnsIntAsync();
        }

        // Real-world example: API client
        public async Task<User> GetUserAsync(int userId)
        {
            await Task.Delay(100); // Simulate API call
            return new User { Id = userId, Username = $"user{userId}", Email = $"user{userId}@example.com" };
        }

        public async Task<bool> SaveUserAsync(User user)
        {
            await Task.Delay(100); // Simulate save
            Console.WriteLine($"   Saved user: {user.Username}");
            return true;
        }

        public async Task<Order> GetOrderAsync(string orderId)
        {
            await Task.Delay(100);
            return new Order { OrderId = orderId, Total = 99.99m, Status = "Processing" };
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class Order
    {
        public string OrderId { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
    }
}