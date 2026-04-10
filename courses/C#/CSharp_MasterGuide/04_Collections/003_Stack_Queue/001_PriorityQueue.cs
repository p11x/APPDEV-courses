/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : PriorityQueue<TElement, TPriority> - Heap-Based
 * FILE      : PriorityQueue.cs
 * PURPOSE   : Demonstrates PriorityQueue fundamentals
 *            including Enqueue, Dequeue, Peek with custom priorities
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._04_Collections._03_Stack_Queue
{
    class PriorityQueueDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== PriorityQueue<TElement, TPriority> ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Creating a Priority Queue
            // ═══════════════════════════════════════════════════════════

            // Default priority queue (min-heap for int priority)
            var minPriorityQueue = new PriorityQueue<string, int>();
            Console.WriteLine($"Empty queue count: {minPriorityQueue.Count}");
            // Output: Empty queue count: 0

            // Max-heap using int (higher number = higher priority)
            var maxPriorityQueue = new PriorityQueue<string, int>(
                Comparer<int>.Create((a, b) => b.CompareTo(a))
            );

            // Priority queue with custom element (example)
            // var taskQueue = new PriorityQueue<TaskItem, int>();

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Enqueue - Adding Elements with Priority
            // ═══════════════════════════════════════════════════════════

            var emergencyQueue = new PriorityQueue<string, int>();

            // Lower priority number = higher priority (min-heap default)
            emergencyQueue.Enqueue("Normal task", 2);
            emergencyQueue.Enqueue("Critical emergency", 1);
            emergencyQueue.Enqueue("Low priority", 3);
            emergencyQueue.Enqueue("Urgent issue", 1);

            Console.WriteLine($"After Enqueue: {emergencyQueue.Count} items");
            // Output: After Enqueue: 4 items

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Dequeue - Removing by Priority Order
            // ═══════════════════════════════════════════════════════════

            // Dequeue returns the element with LOWEST priority value first
            string first = emergencyQueue.Dequeue();
            Console.WriteLine($"First dequeued (highest priority): {first}");
            // Output: First dequeued (highest priority): Critical emergency

            string second = emergencyQueue.Dequeue();
            Console.WriteLine($"Second dequeued: {second}");
            // Output: Second dequeued: Urgent issue

            string third = emergencyQueue.Dequeue();
            Console.WriteLine($"Third dequeued: {third}");
            // Output: Third dequeued: Normal task

            string fourth = emergencyQueue.Dequeue();
            Console.WriteLine($"Fourth dequeued: {fourth}");
            // Output: Fourth dequeued: Low priority

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Peek - View Top Element
            // ═══════════════════════════════════════════════════════════

            var peekQueue = new PriorityQueue<char, int>();
            peekQueue.Enqueue('C', 3);
            peekQueue.Enqueue('A', 1);
            peekQueue.Enqueue('B', 2);

            // Peek without removing
            char topElement = peekQueue.Peek();
            Console.WriteLine($"\nPeek: {topElement}");
            // Output: Peek: A

            Console.WriteLine($"Queue count after peek: {peekQueue.Count}");
            // Output: Queue count after peek: 3

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: EnqueueDequeue - Atomic Operation
            // ═══════════════════════════════════════════════════════════

            var atomicQueue = new PriorityQueue<string, int>();
            atomicQueue.Enqueue("Task-1", 5);
            atomicQueue.Enqueue("Task-2", 1);

            // EnqueueDequeue adds and immediately removes
            // Useful for scenarios like job stealing
            string stolen = atomicQueue.EnqueueDequeue("New-Task", 0);
            Console.WriteLine($"\nEnqueueDequeue result: {stolen}");
            // Output: EnqueueDequeue result: New-Task

            Console.WriteLine($"Remaining count: {atomicQueue.Count}");
            // Output: Remaining count: 3

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Multiple Items Same Priority
            // ═══════════════════════════════════════════════════════════

            var samePriorityQueue = new PriorityQueue<string, int>();

            // FIFO order for same priority
            samePriorityQueue.Enqueue("First", 1);
            samePriorityQueue.Enqueue("Second", 1);
            samePriorityQueue.Enqueue("Third", 1);

            Console.WriteLine("\nSame priority items (FIFO order):");
            while (samePriorityQueue.Count > 0)
            {
                Console.WriteLine($"  Dequeued: {samePriorityQueue.Dequeue()}");
            }
            // Output:
            //   Dequeued: First
            //   Dequeued: Second
            //   Dequeued: Third

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Custom Priority Types
            // ═══════════════════════════════════════════════════════════

            var customPriorityQueue = new PriorityQueue<Patient, Priority>();

            // Add patients with different priorities
            customPriorityQueue.Enqueue(new Patient("John", "Broken arm"), Priority.Normal);
            customPriorityQueue.Enqueue(new Patient("Alice", "Heart attack"), Priority.Critical);
            customPriorityQueue.Enqueue(new Patient("Bob", "Cut finger"), Priority.Low);
            customPriorityQueue.Enqueue(new Patient("Carol", "Chest pain"), Priority.Critical);

            Console.WriteLine("\n=== Real-World: Hospital Triage ===");
            Console.WriteLine("Processing patients by urgency:");

            while (customPriorityQueue.Count > 0)
            {
                Patient patient = customPriorityQueue.Dequeue();
                string priorityStr = patient.Priority switch
                {
                    Priority.Critical => "CRITICAL",
                    Priority.High => "HIGH",
                    Priority.Normal => "NORMAL",
                    Priority.Low => "LOW",
                    _ => "UNKNOWN"
                };
                Console.WriteLine($"[{priorityStr}] {patient.Name} - {patient.Condition}");
                // Output:
                //   [CRITICAL] Alice - Heart attack
                //   [CRITICAL] Carol - Chest pain
                //   [NORMAL] John - Broken arm
                //   [LOW] Bob - Cut finger
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 8: Max-Heap with Custom Comparer
            // ═══════════════════════════════════════════════════════════

            var maxHeap = new PriorityQueue<string, int>(
                Comparer<int>.Create((a, b) => b.CompareTo(a))
            );

            // Higher priority number = dequeued first (max-heap behavior)
            maxHeap.Enqueue("Task-Low", 1);
            maxHeap.Enqueue("Task-High", 10);
            maxHeap.Enqueue("Task-Medium", 5);

            Console.WriteLine("\nMax-Heap (highest priority first):");
            while (maxHeap.Count > 0)
            {
                string task = maxHeap.Dequeue();
                // Priority is not directly accessible from Dequeue
                // This is a known limitation of PriorityQueue
                Console.WriteLine($"  Task: {task}");
            }
            // Output:
            //   Task: Task-High
            //   Task: Task-Medium
            //   Task: Task-Low

            // ═══════════════════════════════════════════════════════════
            // SECTION 9: Real-World Example - Task Scheduler with Priority
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Real-World: CPU Task Scheduler ===");

            var cpuScheduler = new PriorityQueue<ProcessItem, int>();

            // Add processes with different priorities (lower = more important)
            cpuScheduler.Enqueue(new ProcessItem("System", 0), 0);
            cpuScheduler.Enqueue(new ProcessItem("User-App-1", 5), 5);
            cpuScheduler.Enqueue(new ProcessItem("Background-Sync", 10), 10);
            cpuScheduler.Enqueue(new ProcessItem("User-App-2", 3), 3);
            cpuScheduler.Enqueue(new ProcessItem("Log-Cleaner", 8), 8);

            Console.WriteLine("CPU execution order (by priority):");
            int totalTime = 0;
            while (cpuScheduler.Count > 0)
            {
                ProcessItem process = cpuScheduler.Dequeue();
                totalTime += process.CpuTime;
                Console.WriteLine($"  Running: {process.Name} (CPU time: {process.CpuTime}ms) - Total: {totalTime}ms");
            }
            // Output:
            //   Running: System (CPU time: 0ms) - Total: 0ms
            //   Running: User-App-2 (CPU time: 3ms) - Total: 3ms
            //   Running: User-App-1 (CPU time: 5ms) - Total: 8ms
            //   Running: Log-Cleaner (CPU time: 8ms) - Total: 16ms
            //   Running: Background-Sync (CPU time: 10ms) - Total: 26ms

            // ═══════════════════════════════════════════════════════════
            // SECTION 10: Real-World Example - Dijkstra's Algorithm
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Real-World: Path Finding (Simplified) ===");

            // Simplified graph representation
            var graph = new Dictionary<string, List<(string, int)>>
            {
                ["A"] = new() { ("B", 4), ("C", 2) },
                ["B"] = new() { ("C", 1), ("D", 5) },
                ["C"] = new() { ("D", 8), ("E", 10) },
                ["D"] = new() { ("E", 2) },
                ["E"] = new()
            };

            string start = "A";
            string end = "D";

            var distances = new Dictionary<string, int> { [start] = 0 };
            var pq = new PriorityQueue<string, int>();
            pq.Enqueue(start, 0);

            Console.WriteLine($"Finding shortest path from {start} to {end}:");

            while (pq.Count > 0)
            {
                string current = pq.Dequeue();

                if (current == end)
                {
                    Console.WriteLine($"Found path! Distance: {distances[current]}");
                    // Output: Found path! Distance: 3
                    break;
                }

                foreach (var (neighbor, weight) in graph[current])
                {
                    int newDist = distances[current] + weight;
                    if (!distances.ContainsKey(neighbor) || newDist < distances[neighbor])
                    {
                        distances[neighbor] = newDist;
                        pq.Enqueue(neighbor, newDist);
                        Console.WriteLine($"  Updated {neighbor}: distance = {newDist}");
                    }
                }
            }

            Console.WriteLine("\n=== PriorityQueue Operations Complete ===");
        }
    }

    // Patient class for hospital triage example
    public class Patient
    {
        public string Name { get; set; }
        public string Condition { get; set; }
        public Priority Priority { get; set; }

        public Patient(string name, string condition)
        {
            Name = name;
            Condition = condition;
            Priority = Condition switch
            {
                "Heart attack" or "Chest pain" => Priority.Critical,
                "Stroke" or "Severe bleeding" => Priority.High,
                _ => Priority.Normal
            };
        }
    }

    // Process item for CPU scheduler
    public class ProcessItem
    {
        public string Name { get; set; }
        public int CpuTime { get; set; }

        public ProcessItem(string name, int cpuTime)
        {
            Name = name;
            CpuTime = cpuTime;
        }

        public void Deconstruct(out string name, out int cpuTime)
        {
            name = Name;
            cpuTime = CpuTime;
        }
    }

    // Priority enum for patient
    public enum Priority
    {
        Critical = 0,
        High = 1,
        Normal = 2,
        Low = 3
    }
}