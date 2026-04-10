/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : Stack & Queue - Real-World Applications
 * FILE      : StackQueue_RealWorld.cs
 * PURPOSE   : Demonstrates practical applications of Stack
 *            and Queue in real-world scenarios
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._04_Collections._03_Stack_Queue
{
    class StackQueueRealWorldDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Stack & Queue Real-World Examples ===\n");

            // ═══════════════════════════════════════════════════════════
            // EXAMPLE 1: Undo/Redo System using Stack
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== Example 1: Text Editor Undo System ===");

            var documentStateStack = new Stack<DocumentState>();
            var redoStack = new Stack<DocumentState>();

            // Document starts with some content
            var currentDoc = new DocumentState("Hello World", new List<string> { "Hello", "World" });

            // Save state before making changes
            documentStateStack.Push(currentDoc.Copy());
            currentDoc.Content += " - Updated";
            currentDoc.Words.Add("Updated");
            Console.WriteLine($"Document: {currentDoc.Content}");

            documentStateStack.Push(currentDoc.Copy());
            currentDoc.Content += " - Version 3";
            currentDoc.Words.Add("Version3");
            Console.WriteLine($"Document: {currentDoc.Content}");

            documentStateStack.Push(currentDoc.Copy());
            currentDoc.Content += " - Final";
            currentDoc.Words.Add("Final");
            Console.WriteLine($"Document: {currentDoc.Content}");

            Console.WriteLine($"\nStates in undo stack: {documentStateStack.Count}");
            // Output: States in undo stack: 3

            // Perform Undo
            Console.WriteLine("\n--- Performing Undo ---");
            if (documentStateStack.Count > 0)
            {
                // Save current state to redo stack before undoing
                redoStack.Push(currentDoc.Copy());
                
                // Restore previous state
                currentDoc = documentStateStack.Pop();
                Console.WriteLine($"After undo: {currentDoc.Content}");
                // Output: After undo: Hello World - Updated - Version 3
            }

            if (documentStateStack.Count > 0)
            {
                redoStack.Push(currentDoc.Copy());
                currentDoc = documentStateStack.Pop();
                Console.WriteLine($"After undo: {currentDoc.Content}");
                // Output: After undo: Hello World - Updated
            }

            Console.WriteLine($"\nUndo stack: {documentStateStack.Count}, Redo stack: {redoStack.Count}");
            // Output: Undo stack: 1, Redo stack: 2

            // ═══════════════════════════════════════════════════════════
            // EXAMPLE 2: Browser History using Stack
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Example 2: Browser Navigation ===");

            var browserBackStack = new Stack<WebPage>();
            var forwardStack = new Stack<WebPage>();

            // Start browsing
            var currentPage = new WebPage("https://google.com", "Google Home");
            Console.WriteLine($"Loading: {currentPage.Title}");

            // Navigate to new page
            browserBackStack.Push(currentPage);
            currentPage = new WebPage("https://youtube.com", "YouTube");
            forwardStack.Clear(); // Clear forward when new page
            Console.WriteLine($"Loading: {currentPage.Title}");

            browserBackStack.Push(currentPage);
            currentPage = new WebPage("https://github.com", "GitHub");
            forwardStack.Clear();
            Console.WriteLine($"Loading: {currentPage.Title}");

            // Back button
            Console.WriteLine("\n--- Pressing Back ---");
            if (browserBackStack.Count > 0)
            {
                forwardStack.Push(currentPage);
                currentPage = browserBackStack.Pop();
                Console.WriteLine($"Now viewing: {currentPage.Title}");
                // Output: Now viewing: YouTube
            }

            // Forward button
            Console.WriteLine("\n--- Pressing Forward ---");
            if (forwardStack.Count > 0)
            {
                browserBackStack.Push(currentPage);
                currentPage = forwardStack.Pop();
                Console.WriteLine($"Now viewing: {currentPage.Title}");
                // Output: Now viewing: GitHub
            }

            Console.WriteLine($"\nBack stack: {browserBackStack.Count}, Forward stack: {forwardStack.Count}");
            // Output: Back stack: 2, Forward stack: 0

            // ═══════════════════════════════════════════════════════════
            // EXAMPLE 3: Print Spooler using Queue
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Example 3: Print Spooler ===");

            var printQueue = new Queue<PrintJobItem>();

            // Add print jobs to queue
            printQueue.Enqueue(new PrintJobItem("AnnualReport.pdf", 50, "HR"));
            printQueue.Enqueue(new PrintJobItem("Invoice_001.pdf", 2, "Finance"));
            printQueue.Enqueue(new PrintJobItem("MarketingFlyer.pdf", 10, "Marketing"));
            printQueue.Enqueue(new PrintJobItem("Photo.jpg", 1, "Personal"));
            printQueue.Enqueue(new PrintJobItem("Budget2024.xlsx", 25, "Finance"));

            Console.WriteLine($"Print queue has {printQueue.Count} jobs");
            // Output: Print queue has 5 jobs

            // Process print jobs in FIFO order
            int totalPagesPrinted = 0;
            while (printQueue.Count > 0)
            {
                PrintJobItem job = printQueue.Dequeue();
                totalPagesPrinted += job.Pages;
                Console.WriteLine($"Printing: {job.FileName} ({job.Pages} pages) - Dept: {job.Department}");
            }
            // Output:
            //   Printing: AnnualReport.pdf (50 pages) - Dept: HR
            //   Printing: Invoice_001.pdf (2 pages) - Dept: Finance
            //   Printing: MarketingFlyer.pdf (10 pages) - Dept: Marketing
            //   Printing: Photo.jpg (1 pages) - Dept: Personal
            //   Printing: Budget2024.xlsx (25 pages) - Dept: Finance

            Console.WriteLine($"\nTotal pages printed: {totalPagesPrinted}");
            // Output: Total pages printed: 88
            Console.WriteLine($"Jobs remaining: {printQueue.Count}");
            // Output: Jobs remaining: 0

            // ═══════════════════════════════════════════════════════════
            // EXAMPLE 4: Task Scheduler using Queue
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Example 4: Task Scheduler ===");

            var taskQueue = new Queue<TaskWorkItem>();

            // Add tasks to queue
            EnqueueTask(taskQueue, "Send daily report", "Email");
            EnqueueTask(taskQueue, "Process payment", "Payment");
            EnqueueTask(taskQueue, "Backup database", "Maintenance");
            EnqueueTask(taskQueue, "Generate invoice", "Billing");
            EnqueueTask(taskQueue, "Clean temp files", "Maintenance");

            Console.WriteLine($"Total tasks: {taskQueue.Count}");
            // Output: Total tasks: 5

            // Process tasks
            int tasksProcessed = 0;
            while (taskQueue.Count > 0)
            {
                TaskWorkItem task = taskQueue.Dequeue();
                tasksProcessed++;
                Console.WriteLine($"[{tasksProcessed}] Executing: {task.Name} (Type: {task.Type})");
            }
            // Output:
            //   [1] Executing: Send daily report (Type: Email)
            //   [2] Executing: Process payment (Type: Payment)
            //   [3] Executing: Backup database (Type: Maintenance)
            //   [4] Executing: Generate invoice (Type: Billing)
            //   [5] Executing: Clean temp files (Type: Maintenance)

            Console.WriteLine($"\nTasks completed: {tasksProcessed}");
            // Output: Tasks completed: 5

            // Helper method
            void EnqueueTask(Queue<TaskWorkItem> queue, string name, string type)
            {
                queue.Enqueue(new TaskWorkItem(name, type));
            }

            // ═══════════════════════════════════════════════════════════
            // EXAMPLE 5: Call Center using Queue
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Example 5: Call Center Queue ===");

            var callWaitingQueue = new Queue<CallItem>();

            // Simulate incoming calls
            callWaitingQueue.Enqueue(new CallItem("Alice", "Billing question", 120));
            callWaitingQueue.Enqueue(new CallItem("Bob", "Technical support", 300));
            callWaitingQueue.Enqueue(new CallItem("Charlie", "Sales inquiry", 60));
            callWaitingQueue.Enqueue(new CallItem("Diana", "Account help", 90));

            Console.WriteLine($"Calls waiting: {callWaitingQueue.Count}");
            // Output: Calls waiting: 4

            // Simulate agent taking calls
            int callsHandled = 0;
            int totalWaitTime = 0;

            while (callWaitingQueue.Count > 0)
            {
                CallItem call = callWaitingQueue.Dequeue();
                callsHandled++;
                totalWaitTime += call.WaitTime;
                
                Console.WriteLine($"Agent handled call from {call.CustomerName}");
                Console.WriteLine($"  Issue: {call.Reason}, Wait time: {call.WaitTime}s");
            }
            // Output:
            //   Agent handled call from Alice
            //     Issue: Billing question, Wait time: 120s
            //   Agent handled call from Bob
            //     Issue: Technical support, Wait time: 300s
            //   ...

            double avgWaitTime = (double)totalWaitTime / callsHandled;
            Console.WriteLine($"\nCalls handled: {callsHandled}");
            Console.WriteLine($"Average wait time: {avgWaitTime:F1}s");
            // Output: Average wait time: 142.5s

            // ═══════════════════════════════════════════════════════════
            // EXAMPLE 6: Message Queue (Producer-Consumer)
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Example 6: Message Queue Processing ===");

            var messageQueue = new Queue<MessageItem>();

            // Producer: Add messages
            Console.WriteLine("Producer adding messages...");
            messageQueue.Enqueue(new MessageItem("USER_REGISTERED", "{userId: 1}"));
            Console.WriteLine("  Added: USER_REGISTERED message");
            messageQueue.Enqueue(new MessageItem("ORDER_CREATED", "{orderId: 123}"));
            Console.WriteLine("  Added: ORDER_CREATED message");
            messageQueue.Enqueue(new MessageItem("PAYMENT_SUCCESS", "{orderId: 123, amount: 99.99}"));
            Console.WriteLine("  Added: PAYMENT_SUCCESS message");
            messageQueue.Enqueue(new MessageItem("ORDER_SHIPPED", "{orderId: 123}"));
            Console.WriteLine("  Added: ORDER_SHIPPED message");

            Console.WriteLine($"Messages in queue: {messageQueue.Count}");
            // Output: Messages in queue: 4

            // Consumer: Process messages
            Console.WriteLine("\nConsumer processing messages...");
            int messagesProcessed = 0;

            while (messageQueue.Count > 0)
            {
                MessageItem msg = messageQueue.Dequeue();
                messagesProcessed++;
                Console.WriteLine($"  Processed: [{msg.Type}] {msg.Payload}");
                // Output:
                //   Processed: [USER_REGISTERED] {userId: 1}
                //   Processed: [ORDER_CREATED] {orderId: 123}
                //   Processed: [PAYMENT_SUCCESS] {orderId: 123, amount: 99.99}
                //   Processed: [ORDER_SHIPPED] {orderId: 123}
            }

            Console.WriteLine($"\nTotal messages processed: {messagesProcessed}");
            // Output: Total messages processed: 4

            // ═══════════════════════════════════════════════════════════
            // EXAMPLE 7: Combined Stack & Queue - Web Server Request
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Example 7: Web Server Request Handling ===");

            var requestQueue = new Queue<WebRequest>();
            var processingStack = new Stack<WebRequest>();

            // Incoming requests
            requestQueue.Enqueue(new WebRequest("GET /home", "client1"));
            requestQueue.Enqueue(new WebRequest("POST /login", "client2"));
            requestQueue.Enqueue(new WebRequest("GET /products", "client3"));
            requestQueue.Enqueue(new WebRequest("POST /checkout", "client4"));

            Console.WriteLine($"Incoming requests: {requestQueue.Count}");
            // Output: Incoming requests: 4

            // Move to processing
            while (requestQueue.Count > 0)
            {
                processingStack.Push(requestQueue.Dequeue());
            }

            Console.WriteLine($"Requests moved to processing: {processingStack.Count}");
            // Output: Requests moved to processing: 4

            // Process in reverse order (stack behavior)
            Console.WriteLine("\nProcessing requests (LIFO):");
            while (processingStack.Count > 0)
            {
                WebRequest req = processingStack.Pop();
                Console.WriteLine($"  Handled: {req.Endpoint} from {req.ClientId}");
            }
            // Output:
            //   Handled: POST /checkout from client4
            //   Handled: GET /products from client3
            //   Handled: POST /login from client2
            //   Handled: GET /home from client1

            Console.WriteLine("\n=== Real-World Examples Complete ===");
        }
    }

    // Document state for undo system
    public class DocumentState
    {
        public string Content { get; set; }
        public List<string> Words { get; set; }

        public DocumentState(string content, List<string> words)
        {
            Content = content;
            Words = words;
        }

        public DocumentState Copy()
        {
            return new DocumentState(Content, new List<string>(Words));
        }
    }

    // Web page for browser history
    public class WebPage
    {
        public string Url { get; set; }
        public string Title { get; set; }

        public WebPage(string url, string title)
        {
            Url = url;
            Title = title;
        }
    }

    // Print job item
    public class PrintJobItem
    {
        public string FileName { get; set; }
        public int Pages { get; set; }
        public string Department { get; set; }

        public PrintJobItem(string fileName, int pages, string department)
        {
            FileName = fileName;
            Pages = pages;
            Department = department;
        }
    }

    // Task work item
    public class TaskWorkItem
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public TaskWorkItem(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }

    // Call item for call center
    public class CallItem
    {
        public string CustomerName { get; set; }
        public string Reason { get; set; }
        public int WaitTime { get; set; }

        public CallItem(string name, string reason, int waitTime)
        {
            CustomerName = name;
            Reason = reason;
            WaitTime = waitTime;
        }
    }

    // Message item for message queue
    public class MessageItem
    {
        public string Type { get; set; }
        public string Payload { get; set; }

        public MessageItem(string type, string payload)
        {
            Type = type;
            Payload = payload;
        }
    }

    // Web request for server
    public class WebRequest
    {
        public string Endpoint { get; set; }
        public string ClientId { get; set; }

        public WebRequest(string endpoint, string clientId)
        {
            Endpoint = endpoint;
            ClientId = clientId;
        }
    }
}