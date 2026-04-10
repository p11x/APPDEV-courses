/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : Queue<T> - Advanced Operations
 * FILE      : Queue_Part2.cs
 * PURPOSE   : Demonstrates Queue<T> advanced operations
 *            including TryDequeue, Contains, and performance
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._04_Collections._03_Stack_Queue
{
    class QueueAdvancedDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Queue<T> Advanced Operations ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: TryDequeue and TryPeek - Safe Operations
            // ═══════════════════════════════════════════════════════════

            var safeQueue = new Queue<string>();

            // TryPeek - safely retrieves front element without removing
            bool peekSuccess = safeQueue.TryPeek(out string peekValue);
            Console.WriteLine($"TryPeek on empty: success={peekSuccess}");
            // Output: TryPeek on empty: success=False

            // Add elements
            safeQueue.Enqueue("Task-A");
            safeQueue.Enqueue("Task-B");
            safeQueue.Enqueue("Task-C");

            // TryPeek on non-empty queue
            peekSuccess = safeQueue.TryPeek(out peekValue);
            Console.WriteLine($"TryPeek: success={peekSuccess}, value={peekValue}");
            // Output: TryPeek: success=True, value=Task-A

            // TryDequeue - safely removes and returns front element
            bool dequeueSuccess = safeQueue.TryDequeue(out string dequeueValue);
            Console.WriteLine($"TryDequeue: success={dequeueSuccess}, value={dequeueValue}");
            // Output: TryDequeue: success=True, value=Task-A

            Console.WriteLine($"Queue count after TryDequeue: {safeQueue.Count}");
            // Output: Queue count after TryDequeue: 2

            // TryDequeue on remaining elements
            while (safeQueue.TryDequeue(out string remaining))
            {
                Console.WriteLine($"Dequeued: {remaining}");
            }
            // Output:
            //   Dequeued: Task-B
            //   Dequeued: Task-C

            // TryDequeue on empty queue
            dequeueSuccess = safeQueue.TryDequeue(out dequeueValue);
            Console.WriteLine($"TryDequeue on empty: success={dequeueSuccess}");
            // Output: TryDequeue on empty: success=False

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Clear - Removing All Elements
            // ═══════════════════════════════════════════════════════════

            var clearQueue = new Queue<char>();
            clearQueue.Enqueue('A');
            clearQueue.Enqueue('B');
            clearQueue.Enqueue('C');

            Console.WriteLine($"\nBefore Clear: {clearQueue.Count} elements");
            // Output: Before Clear: 3 elements

            clearQueue.Clear();
            Console.WriteLine($"After Clear: {clearQueue.Count} elements");
            // Output: After Clear: 0 elements

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Enumerator Usage
            // ═══════════════════════════════════════════════════════════

            var enumQueue = new Queue<int>();
            enumQueue.Enqueue(10);
            enumQueue.Enqueue(20);
            enumQueue.Enqueue(30);

            // Using GetEnumerator directly
            IEnumerator<int> enumerator = enumQueue.GetEnumerator();
            Console.WriteLine("\nUsing enumerator:");
            while (enumerator.MoveNext())
            {
                Console.WriteLine($"  Current: {enumerator.Current}");
            }
            // Output:
            //   Current: 10
            //   Current: 20
            //   Current: 30

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: ToArray - Converting to Array
            // ═══════════════════════════════════════════════════════════

            var arrayQueue = new Queue<string>();
            arrayQueue.Enqueue("One");
            arrayQueue.Enqueue("Two");
            arrayQueue.Enqueue("Three");
            arrayQueue.Enqueue("Four");

            // ToArray creates array in FIFO order (front first)
            string[] asArray = arrayQueue.ToArray();
            Console.WriteLine("\nToArray result: " + string.Join(", ", asArray));
            // Output: ToArray result: One, Two, Three, Four

            // Array can be used to create new queue
            var newQueue = new Queue<string>(asArray);
            Console.WriteLine($"New queue from array, count: {newQueue.Count}");
            // Output: New queue from array, count: 4

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Peek with Exception Handling
            // ═══════════════════════════════════════════════════════════

            var exceptionQueue = new Queue<double>();

            // Using TryPeek to avoid exceptions
            if (exceptionQueue.TryPeek(out double value))
            {
                Console.WriteLine($"Peek value: {value}");
            }
            else
            {
                Console.WriteLine("Queue is empty - no value to peek");
                // Output: Queue is empty - no value to peek
            }

            // Adding elements
            exceptionQueue.Enqueue(3.14);
            exceptionQueue.Enqueue(2.71);

            if (exceptionQueue.TryPeek(out value))
            {
                Console.WriteLine($"Peek value: {value}");
                // Output: Peek value: 3.14
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Performance Considerations
            // ═══════════════════════════════════════════════════════════

            // Queue uses a circular buffer internally
            var perfQueue = new Queue<int>(10000);

            // Efficient bulk operations
            Console.WriteLine("\n=== Performance Demo ===");

            // Adding many elements (O(n) total, O(1) amortized per element)
            for (int i = 0; i < 1000; i++)
            {
                perfQueue.Enqueue(i);
            }
            Console.WriteLine($"Added 1000 elements, count: {perfQueue.Count}");
            // Output: Added 1000 elements, count: 1000

            // Removing all elements
            int dequeuedCount = 0;
            while (perfQueue.TryDequeue(out _))
            {
                dequeuedCount++;
            }
            Console.WriteLine($"Dequeued {dequeuedCount} elements");
            // Output: Dequeued 1000 elements

            // Queue doesn't have TrimExcess like other collections
            // but automatically manages memory internally

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Real-World Example - Message Queue
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Real-World: Message Queue Processing ===");

            var messageQueue = new Queue<Message>();

            // Producer: Add messages to queue
            messageQueue.Enqueue(new Message("USER_CREATED", "User John registered"));
            messageQueue.Enqueue(new Message("ORDER_PLACED", "Order #12345 placed"));
            messageQueue.Enqueue(new Message("PAYMENT_RECEIVED", "Payment for Order #12345"));
            messageQueue.Enqueue(new Message("ORDER_SHIPPED", "Order #12345 shipped"));

            Console.WriteLine($"Messages in queue: {messageQueue.Count}");
            // Output: Messages in queue: 4

            // Consumer: Process messages in order
            Console.WriteLine("\nProcessing messages:");
            while (messageQueue.TryDequeue(out Message msg))
            {
                Console.WriteLine($"[{msg.Type}] {msg.Payload}");
                // Output:
                //   [USER_CREATED] User John registered
                //   [ORDER_PLACED] Order #12345 placed
                //   [PAYMENT_RECEIVED] Payment for Order #12345
                //   [ORDER_SHIPPED] Order #12345 shipped
            }

            Console.WriteLine($"\nMessages remaining: {messageQueue.Count}");
            // Output: Messages remaining: 0

            // ═══════════════════════════════════════════════════════════
            // SECTION 8: Real-World Example - Task Scheduler
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Real-World: Task Scheduler ===");

            var taskScheduler = new Queue<TaskItem>();

            // Add tasks to scheduler
            taskScheduler.Enqueue(new TaskItem("Backup database", Priority.Low));
            taskScheduler.Enqueue(new TaskItem("Send email notification", Priority.Normal));
            taskScheduler.Enqueue(new TaskItem("Process payment", Priority.High));
            taskScheduler.Enqueue(new TaskItem("Generate report", Priority.Normal));
            taskScheduler.Enqueue(new TaskItem("Cleanup temp files", Priority.Low));

            Console.WriteLine($"Total tasks: {taskScheduler.Count}");
            // Output: Total tasks: 5

            // Process tasks in order
            Console.WriteLine("\nExecuting tasks:");
            while (taskScheduler.TryDequeue(out TaskItem task))
            {
                string priorityStr = task.Priority switch
                {
                    Priority.Low => "LOW",
                    Priority.Normal => "NORMAL",
                    Priority.High => "HIGH",
                    _ => "UNKNOWN"
                };
                Console.WriteLine($"[{priorityStr}] {task.Description}");
                // Output:
                //   [NORMAL] Send email notification
                //   [HIGH] Process payment
                //   [NORMAL] Generate report
                //   [LOW] Backup database
                //   [LOW] Cleanup temp files
            }

            Console.WriteLine("\nAll tasks completed");
            // Output: All tasks completed

            Console.WriteLine("\n=== Queue<T> Advanced Operations Complete ===");
        }
    }

    // Message class for real-world example
    public class Message
    {
        public string Type { get; set; }
        public string Payload { get; set; }

        public Message(string type, string payload)
        {
            Type = type;
            Payload = payload;
        }
    }

    // Task item class for real-world example
    public class TaskItem
    {
        public string Description { get; set; }
        public Priority Priority { get; set; }

        public TaskItem(string description, Priority priority)
        {
            Description = description;
            Priority = priority;
        }
    }

    public enum Priority
    {
        Low,
        Normal,
        High
    }
}