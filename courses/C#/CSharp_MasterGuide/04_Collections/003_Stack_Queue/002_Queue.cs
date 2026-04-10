/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : Queue<T> - FIFO Operations
 * FILE      : Queue.cs
 * PURPOSE   : Demonstrates Queue<T> fundamental operations
 *            including Enqueue, Dequeue, Peek, and Contains
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._04_Collections._03_Stack_Queue
{
    class QueueDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Queue<T> FIFO Operations ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Creating a Queue
            // ═══════════════════════════════════════════════════════════

            // Empty queue of integers
            var emptyQueue = new Queue<int>();
            Console.WriteLine($"Empty queue count: {emptyQueue.Count}");
            // Output: Empty queue count: 0

            // Queue from array initialization
            var queueFromArray = new Queue<string>(new[] { "First", "Second", "Third" });
            Console.WriteLine($"Queue from array count: {queueFromArray.Count}");
            // Output: Queue from array count: 3

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Enqueue - Adding Elements
            // ═══════════════════════════════════════════════════════════

            var printQueue = new Queue<string>();

            // Enqueue adds elements to the END of the queue
            printQueue.Enqueue("Document1.pdf");
            printQueue.Enqueue("Document2.docx");
            printQueue.Enqueue("Presentation.pptx");
            printQueue.Enqueue("Image.png");

            Console.WriteLine($"After Enqueue: {printQueue.Count} documents");
            // Output: After Enqueue: 4 documents

            // Peek to see front element without removing
            Console.WriteLine($"Next to print (Peek): {printQueue.Peek()}");
            // Output: Next to print (Peek): Document1.pdf

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Dequeue - Removing Elements
            // ═══════════════════════════════════════════════════════════

            // Dequeue removes and returns the FRONT element (FIFO order)
            string firstDoc = printQueue.Dequeue();
            Console.WriteLine($"Dequeued: {firstDoc}");
            // Output: Dequeued: Document1.pdf

            Console.WriteLine($"Queue count after dequeue: {printQueue.Count}");
            // Output: Queue count after dequeue: 3

            // Dequeue another element
            string secondDoc = printQueue.Dequeue();
            Console.WriteLine($"Dequeued: {secondDoc}");
            // Output: Dequeued: Document2.docx

            // Peek to see what's next
            Console.WriteLine($"Now at front (Peek): {printQueue.Peek()}");
            // Output: Now at front (Peek): Presentation.pptx

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Contains - Checking Elements
            // ═══════════════════════════════════════════════════════════

            bool hasImage = printQueue.Contains("Image.png");
            bool hasDoc = printQueue.Contains("Report.xls");

            Console.WriteLine($"\nContains 'Image.png': {hasImage}");
            // Output: Contains 'Image.png': True
            Console.WriteLine($"Contains 'Report.xls': {hasDoc}");
            // Output: Contains 'Report.xls': False

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Iterating Without Removing
            // ═══════════════════════════════════════════════════════════

            var customerQueue = new Queue<string>();
            customerQueue.Enqueue("Customer-A");
            customerQueue.Enqueue("Customer-B");
            customerQueue.Enqueue("Customer-C");
            customerQueue.Enqueue("Customer-D");
            customerQueue.Enqueue("Customer-E");

            Console.WriteLine("\nAll customers in queue (front to back):");
            foreach (string customer in customerQueue)
            {
                Console.WriteLine($"  {customer}");
            }
            // Output:
            //   Customer-A
            //   Customer-B
            //   Customer-C
            //   Customer-D
            //   Customer-E

            // Copy to array (preserves FIFO order)
            string[] queueArray = customerQueue.ToArray();
            Console.WriteLine("\nAs array: " + string.Join(", ", queueArray));
            // Output: As array: Customer-A, Customer-B, Customer-C, Customer-D, Customer-E

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World Example - Print Spooler
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Real-World: Print Spooler ===");

            var printSpooler = new Queue<PrintJob>();

            // Add print jobs to spooler
            printSpooler.Enqueue(new PrintJob("Report.pdf", 5));
            printSpooler.Enqueue(new PrintJob("Invoice.docx", 2));
            printSpooler.Enqueue(new PrintJob("Banner.png", 10));
            printSpooler.Enqueue(new PrintJob("Memo.txt", 1));

            Console.WriteLine($"Print queue has {printSpooler.Count} jobs");

            // Process print jobs in order
            while (printSpooler.Count > 0)
            {
                PrintJob job = printSpooler.Dequeue();
                Console.WriteLine($"Printing: {job.FileName} ({job.Pages} pages)");
                // Output:
                //   Printing: Report.pdf (5 pages)
                //   Printing: Invoice.docx (2 pages)
                //   Printing: Banner.png (10 pages)
                //   Printing: Memo.txt (1 pages)
            }

            Console.WriteLine("All print jobs completed");
            // Output: All print jobs completed

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Real-World Example - Customer Service Queue
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Real-World: Customer Service Call Center ===");

            var callQueue = new Queue<CustomerCall>();

            // Calls come in and get queued
            callQueue.Enqueue(new CustomerCall("Alice", "Billing question"));
            callQueue.Enqueue(new CustomerCall("Bob", "Technical support"));
            callQueue.Enqueue(new CustomerCall("Charlie", "Sales inquiry"));
            callQueue.Enqueue(new CustomerCall("Diana", "Account issue"));

            Console.WriteLine($"Waiting calls: {callQueue.Count}");
            // Output: Waiting calls: 4

            // Simulate answering calls
            Console.WriteLine("\nProcessing calls:");
            for (int i = 0; i < 3; i++)
            {
                CustomerCall call = callQueue.Dequeue();
                Console.WriteLine($"Answered: {call.CustomerName} - {call.Reason}");
            }
            // Output:
            //   Answered: Alice - Billing question
            //   Answered: Bob - Technical support
            //   Answered: Charlie - Sales inquiry

            Console.WriteLine($"\nCalls remaining: {callQueue.Count}");
            // Output: Calls remaining: 1

            Console.WriteLine("\n=== Queue<T> FIFO Operations Complete ===");
        }
    }

    // Print job class for real-world example
    public class PrintJob
    {
        public string FileName { get; set; }
        public int Pages { get; set; }

        public PrintJob(string fileName, int pages)
        {
            FileName = fileName;
            Pages = pages;
        }
    }

    // Customer call class for real-world example
    public class CustomerCall
    {
        public string CustomerName { get; set; }
        public string Reason { get; set; }

        public CustomerCall(string name, string reason)
        {
            CustomerName = name;
            Reason = reason;
        }
    }
}