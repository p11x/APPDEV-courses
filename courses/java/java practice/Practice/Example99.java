/*
 * SUB TOPIC: Queue Interface in Java
 * 
 * DEFINITION:
 * Queue is an interface in the Java Collections Framework that extends Collection and represents 
 * a FIFO (First-In-First-Out) data structure. Elements are added at the rear and removed from 
 * the front. It models real-world queues like line at a ticket counter.
 * 
 * FUNCTIONALITIES:
 * 1. add()/offer() - Add element to queue (offer preferred, add throws exception if full)
 * 2. remove()/poll() - Remove and return element (poll returns null, remove throws exception)
 * 3. element()/peek() - View front element without removing
 * 4. isEmpty() - Check if queue is empty
 * 5. size() - Get number of elements
 * 6. clear() - Remove all elements
 */

import java.util.*;

public class Example99 {
    public static void main(String[] args) {
        
        // Creating a Queue using LinkedList
        Queue<String> queue = new LinkedList<>();
        
        // add() - Adding elements to queue
        queue.add("Customer1");
        queue.add("Customer2");
        queue.add("Customer3");
        
        System.out.println("=== Basic Queue Operations ===");
        System.out.println("Queue after adds: " + queue);
        
        // peek() - View front element without removing
        System.out.println("peek(): " + queue.peek());
        System.out.println("Queue unchanged: " + queue);
        
        // element() - Similar to peek but throws exception if empty
        System.out.println("element(): " + queue.element());
        
        // poll() - Remove and return front element
        String served = queue.poll();
        System.out.println("\nServed: " + served);
        System.out.println("Queue after poll: " + queue);
        
        // remove() - Similar to poll but throws exception if empty
        served = queue.remove();
        System.out.println("Served: " + served);
        System.out.println("Queue after remove: " + queue);
        
        // offer() - Adding elements (preferred over add)
        System.out.println("\n=== Using offer() ===");
        Queue<Integer> numberQueue = new LinkedList<>();
        
        numberQueue.offer(10);
        numberQueue.offer(20);
        numberQueue.offer(30);
        
        System.out.println("Queue: " + numberQueue);
        System.out.println("poll(): " + numberQueue.poll());
        System.out.println("After poll: " + numberQueue);
        
        // Using ArrayDeque for better performance
        System.out.println("\n=== ArrayDeque Implementation ===");
        Queue<String> arrayDeque = new ArrayDeque<>(3);
        
        arrayDeque.add("A");
        arrayDeque.add("B");
        arrayDeque.add("C");
        
        System.out.println("Full queue: " + arrayDeque);
        System.out.println("offer('D'): " + arrayDeque.offer("D")); // May succeed/fail based on implementation
        
        // Real-time Example 1: Print job queue
        System.out.println("\n=== Example 1: Print Job Queue ===");
        Queue<String> printQueue = new LinkedList<>();
        
        // Add print jobs
        printQueue.offer("Document1.pdf");
        printQueue.offer("Image1.png");
        printQueue.offer("Report.docx");
        
        System.out.println("Print jobs in queue: " + printQueue.size());
        
        // Process print jobs
        while (!printQueue.isEmpty()) {
            String job = printQueue.poll();
            System.out.println("Printing: " + job);
        }
        System.out.println("All jobs completed");
        
        // Real-time Example 2: Call center system
        System.out.println("\n=== Example 2: Call Center Queue ===");
        Queue<String> calls = new LinkedList<>();
        
        calls.offer("Call from Customer A");
        calls.offer("Call from Customer B");
        calls.offer("Call from Customer C");
        calls.offer("Call from Customer D");
        
        System.out.println("Waiting calls: " + calls.size());
        
        // Answer calls in order
        for (int i = 1; i <= 3; i++) {
            if (!calls.isEmpty()) {
                System.out.println("Agent " + i + " answering: " + calls.poll());
            }
        }
        System.out.println("Remaining calls: " + calls.size());
        
        // Real-time Example 3: Event processing queue
        System.out.println("\n=== Example 3: Event Queue ===");
        Queue<String> eventQueue = new LinkedList<>();
        
        eventQueue.offer("USER_LOGIN");
        eventQueue.offer("USER_LOGOUT");
        eventQueue.offer("PURCHASE_ITEM");
        eventQueue.offer("ADD_TO_CART");
        
        // Process events
        while (!eventQueue.isEmpty()) {
            String event = eventQueue.poll();
            System.out.println("Processing event: " + event);
        }
        
        // Real-time Example 4: Message queue (email notifications)
        System.out.println("\n=== Example 4: Email Notification Queue ===");
        Queue<String> emailQueue = new LinkedList<>();
        
        emailQueue.offer("Welcome email to user1");
        emailQueue.offer("Password reset to user2");
        emailQueue.offer("Order confirmation to user3");
        
        System.out.println("Emails waiting: " + emailQueue.size());
        
        // Simulate sending emails
        while (!emailQueue.isEmpty()) {
            String email = emailQueue.poll();
            System.out.println("Sent: " + email);
        }
        System.out.println("All emails sent!");
        
        // Real-time Example 5: Ticket booking system
        System.out.println("\n=== Example 5: Ticket Booking ===");
        Queue<String> ticketQueue = new LinkedList<>();
        
        ticketQueue.offer("User1 - 2 tickets");
        ticketQueue.offer("User2 - 1 ticket");
        ticketQueue.offer("User3 - 4 tickets");
        ticketQueue.offer("User4 - 1 ticket");
        
        int availableSeats = 5;
        System.out.println("Available seats: " + availableSeats);
        
        while (!ticketQueue.isEmpty() && availableSeats > 0) {
            String request = ticketQueue.poll();
            System.out.println("Processing: " + request);
            availableSeats--;
        }
        
        System.out.println("Seats remaining: " + availableSeats);
        System.out.println("Unfulfilled requests: " + ticketQueue.size());
        
        // Real-time Example 6: Background task scheduler
        System.out.println("\n=== Example 6: Task Scheduler ===");
        Queue<String> taskQueue = new LinkedList<>();
        
        taskQueue.offer("Task1 - Backup database");
        taskQueue.offer("Task2 - Generate report");
        taskQueue.offer("Task3 - Send notifications");
        taskQueue.offer("Task4 - Clean cache");
        
        System.out.println("Total tasks: " + taskQueue.size());
        
        // Execute tasks with time limit simulation
        int maxTasks = 3;
        for (int i = 0; i < maxTasks; i++) {
            if (!taskQueue.isEmpty()) {
                String task = taskQueue.poll();
                System.out.println("Executing: " + task);
            }
        }
        
        if (!taskQueue.isEmpty()) {
            System.out.println("Tasks pending: " + taskQueue.size());
        }
        
        // Additional operations
        System.out.println("\n=== Additional Operations ===");
        Queue<String> testQueue = new LinkedList<>();
        testQueue.add("One");
        testQueue.add("Two");
        
        System.out.println("Contains 'One': " + testQueue.contains("One"));
        System.out.println("Size: " + testQueue.size());
        System.out.println("Is empty: " + testQueue.isEmpty());
        
        // Using iterator
        System.out.println("Using iterator:");
        Iterator<String> iter = testQueue.iterator();
        while (iter.hasNext()) {
            System.out.println("  " + iter.next());
        }
        
        // Clear queue
        testQueue.clear();
        System.out.println("After clear: " + testQueue);
        
        // Using LinkedList as Queue with null elements (allowed)
        System.out.println("\n=== Queue with null elements ===");
        Queue<String> nullQueue = new LinkedList<>();
        nullQueue.offer(null);
        nullQueue.offer("First");
        System.out.println("Queue with null: " + nullQueue);
        
        // Using PriorityQueue (sorted queue)
        System.out.println("\n=== PriorityQueue (Sorted) ===");
        Queue<Integer> priorityQ = new PriorityQueue<>();
        priorityQ.offer(5);
        priorityQ.offer(1);
        priorityQ.offer(3);
        priorityQ.offer(2);
        
        System.out.println("Elements in order (poll):");
        while (!priorityQ.isEmpty()) {
            System.out.println("  " + priorityQ.poll());
        }
    }
}
