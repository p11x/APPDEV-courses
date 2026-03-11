/*
================================================================================
TOPIC 27: MULTITHREADING
================================================================================

Multithreading enables parallel execution for better performance.

TABLE OF CONTENTS:
1. What is Multithreading?
2. Creating Threads
3. Thread Synchronization
4. Parallel Class
================================================================================
*/

using System;
using System.Threading;

namespace MultithreadingExamples
{
    class Program
    {
        static void DoWork(object message)
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"{message}: {i}");
                Thread.Sleep(100);
            }
        }
        
        static void Main()
        {
            Console.WriteLine("=== Multithreading ===");
            
            // Create threads
            Thread t1 = new Thread(DoWork);
            Thread t2 = new Thread(DoWork);
            
            // Start threads
            t1.Start("Thread 1");
            t2.Start("Thread 2");
            
            // Wait for completion
            t1.Join();
            t2.Join();
            
            Console.WriteLine("All threads completed");
            
            // Thread-safe counter
            int counter = 0;
            object lockObj = new object();
            
            Thread t3 = new Thread(() => 
            {
                for (int i = 0; i < 1000; i++)
                {
                    lock(lockObj)
                    {
                        counter++;
                    }
                }
            });
            
            Thread t4 = new Thread(() => 
            {
                for (int i = 0; i < 1000; i++)
                {
                    lock(lockObj)
                    {
                        counter++;
                    }
                }
            });
            
            t3.Start();
            t4.Start();
            t3.Join();
            t4.Join();
            
            Console.WriteLine($"Counter: {counter}");
        }
    }
}

/*
THREADING CONCEPTS:
-------------------
Thread       - Unit of execution
lock         - Synchronization primitive
Monitor      - More control than lock
Interlocked  - Atomic operations
ThreadPool   - Reuse threads
Task Parallel Library - Higher-level APIs
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 28 covers Dependency Injection.
*/
