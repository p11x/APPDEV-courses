/*
 * TOPIC: Task Parallel Library
 * SUBTOPIC: ValueTask
 * FILE: 09_ValueTask.cs
 * PURPOSE: Understanding ValueTask<T> for performance optimization in hot paths
 */
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._02_Tasks
{
    public class ValueTaskDemo
    {
        public static void Main()
        {
            Console.WriteLine("=== ValueTask Demo ===\n");

            var demo = new ValueTaskDemo();

            // Example 1: Basic ValueTask usage
            Console.WriteLine("1. Basic ValueTask:");
            demo.BasicValueTaskDemo();

            // Example 2: ValueTask vs Task performance
            Console.WriteLine("\n2. ValueTask vs Task:");
            demo.ValueTaskVsTaskDemo();

            // Example 3: Synchronous completion pattern
            Console.WriteLine("\n3. Synchronous completion pattern:");
            demo.SynchronousCompletionDemo();

            // Example 4: Caching with ValueTask
            Console.WriteLine("\n4. Caching with ValueTask:");
            demo.CachedValueTaskDemo();

            // Example 5: ValueTask in async method
            Console.WriteLine("\n5. ValueTask in async method:");
            demo.AsyncMethodValueTaskDemo();

            // Example 6: Proper ValueTask usage
            Console.WriteLine("\n6. Proper ValueTask usage:");
            demo.ProperValueTaskUsage();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public void BasicValueTaskDemo()
        {
            // ValueTask can be awaited like Task
            ValueTask<int> vt = GetValueAsync();
            int result = vt.Result;
            Console.WriteLine($"   Result: {result}");
        }

        private ValueTask<int> GetValueAsync()
        {
            // Can return synchronously
            return new ValueTask<int>(42);
        }

        public void ValueTaskVsTaskDemo()
        {
            // Task allocation (heap)
            var task = Task.FromResult(42);
            
            // ValueTask (struct, potentially stack-allocated)
            var valueTask = new ValueTask<int>(42);

            // Both can be awaited
            Console.WriteLine($"   Task result: {task.Result}");
            Console.WriteLine($"   ValueTask result: {valueTask.Result}");
        }

        public void SynchronousCompletionDemo()
        {
            var result = TryGetValueSync();
            Console.WriteLine($"   Sync result: {result}");
        }

        private ValueTask<int> TryGetValueSync()
        {
            // Fast path: return synchronously
            bool found = true;
            if (found)
                return new ValueTask<int>(100);

            // Slow path: return async
            return new ValueTask<int>(LoadValueAsync());
        }

        private async Task<int> LoadValueAsync()
        {
            await Task.Delay(50);
            return 200;
        }

        public void CachedValueTaskDemo()
        {
            var cached = GetCachedValue();
            Console.WriteLine($"   Cached: {cached.IsCompleted}, Result: {cached.Result}");
        }

        private ValueTask<int> _cachedValue;

        private ValueTask<int> GetCachedValue()
        {
            if (_cachedValue.IsCompleted)
                return _cachedValue;

            return _cachedValue = new ValueTask<int>(LoadValueAsync());
        }

        public async void AsyncMethodValueTaskDemo()
        {
            var result = await ProcessValueTaskAsync();
            Console.WriteLine($"   Async result: {result}");
        }

        private async ValueTask<string> ProcessValueTaskAsync()
        {
            // Can await both Task and ValueTask
            await Task.Delay(50);
            return "Processed";
        }

        public void ProperValueTaskUsage()
        {
            // DON'T: Use ValueTask in hot paths where Task is more appropriate
            // DO: Use ValueTask when you often complete synchronously

            // Example of proper usage: caching pattern
            var cached = _cache.GetOrCreate("key", () => new ValueTask<int>(42));
        }

        private readonly Cache<string, int> _cache = new();
    }

    public class Cache<TKey, TValue>
    {
        public ValueTask<TValue> GetOrCreate(TKey key, Func<TValue> factory)
        {
            return new ValueTask<TValue>(factory());
        }
    }

    // Real-world ValueTask usage
    public class AsyncCache<T>
    {
        private TValue _cached;

        public ValueTask<TValue> GetAsync() where TValue : class
        {
            if (_cached != null)
                return new ValueTask<TValue>(_cached);

            return new ValueTask<TValue>(LoadAsync());
        }

        private async Task<TValue> LoadAsync()
        {
            await Task.Delay(50);
            return _cached = new TValue();
        }
    }

    public class SynchronousResult<T>
    {
        private T _result;

        public ValueTask<T> GetResultAsync(bool useCache)
        {
            if (useCache && _result != null)
                return new ValueTask<T>(_result);

            return new ValueTask<T>(ComputeAsync());
        }

        private async Task<T> ComputeAsync()
        {
            await Task.Delay(100);
            return _result;
        }
    }

    public class BufferPool
    {
        private byte[] _buffer;

        public ValueTask<byte[]> RentAsync()
        {
            if (_buffer != null)
            {
                var b = _buffer;
                _buffer = null;
                return new ValueTask<byte[]>(b);
            }
            return new ValueTask<byte[]>(AllocateAsync());
        }

        private async Task<byte[]> AllocateAsync()
        {
            await Task.CompletedTask;
            return new byte[1024];
        }

        public void Return(byte[] buffer)
        {
            _buffer = buffer;
        }
    }

    public class HotPathProcessor
    {
        public ValueTask<int> ProcessQuicklyAsync(bool cached)
        {
            if (cached)
                return new ValueTask<int>(Calculate());

            return new ValueTask<int>(ProcessSlowAsync());
        }

        private int Calculate() => 42;

        private async Task<int> ProcessSlowAsync()
        {
            await Task.Delay(10);
            return 100;
        }
    }
}