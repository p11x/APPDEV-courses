/*
================================================================================
TOPIC 26: ASYNCHRONOUS PROGRAMMING
================================================================================

Async programming improves application responsiveness by not blocking threads.

TABLE OF CONTENTS:
1. Why Async?
2. async and await Keywords
3. Task Class
4. Async Best Practices
================================================================================
*/

using System;
using System.Threading.Tasks;

namespace AsyncExamples
{
    class Program
    {
        // Async method
        static async Task<string> FetchDataAsync()
        {
            Console.WriteLine("Starting async operation...");
            
            // Simulate long-running task
            await Task.Delay(2000);  // Wait 2 seconds
            
            return "Data fetched!";
        }
        
        // Non-async caller
        static async Task Main()
        {
            Console.WriteLine("=== Async Programming ===");
            
            // Call async method
            string result = await FetchDataAsync();
            Console.WriteLine(result);
            
            // Multiple async operations
            Task t1 = Task.Delay(1000).ContinueWith(_ => Console.WriteLine("Task 1 done"));
            Task t2 = Task.Delay(500).ContinueWith(_ => Console.WriteLine("Task 2 done"));
            
            await Task.WhenAll(t1, t2);
            
            Console.WriteLine("All tasks completed");
        }
    }
}

/*
ASYNC KEYWORDS:
--------------
async    - Marks method as asynchronous
await    - Pauses until task completes
Task     - Represents async operation
Task<T>  - Async operation returning value

WHY ASYNC?
----------
- Keeps UI responsive
- Better scalability
- Non-blocking I/O
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 27 covers Multithreading.
*/
