/*
 * SUB TOPIC: PriorityQueue in Java
 * 
 * DEFINITION:
 * PriorityQueue is a special type of queue that orders elements according to their natural 
 * ordering (natural order) or by a custom Comparator provided at construction time. 
 * The head of the queue is the least element according to the specified ordering. 
 * It does not permit null elements and is not thread-safe.
 * 
 * FUNCTIONALITIES:
 * 1. add()/offer() - Add element to queue
 * 2. poll() - Remove and return highest priority element
 * 3. peek() - View highest priority element without removing
 * 4. remove() - Remove specific element
 * 5. contains() - Check if element exists
 * 6. size() - Get number of elements
 * 7. toArray() - Convert to array
 * 8. clear() - Remove all elements
 */

import java.util.*;

public class Example93 {
    public static void main(String[] args) {
        
        // Creating a PriorityQueue (natural ordering - min heap)
        PriorityQueue<Integer> numbers = new PriorityQueue<>();
        
        // Adding elements
        numbers.add(30);
        numbers.add(10);
        numbers.add(50);
        numbers.add(20);
        numbers.add(40);
        
        System.out.println("=== Basic PriorityQueue Operations ===");
        System.out.println("Elements added: 30, 10, 50, 20, 40");
        
        // peek() - Get highest priority element without removing
        System.out.println("peek(): " + numbers.peek()); // Returns smallest (10)
        
        // poll() - Remove and return highest priority element
        System.out.println("poll(): " + numbers.poll()); // Removes 10
        System.out.println("After poll(): " + numbers);
        
        System.out.println("poll(): " + numbers.poll()); // Removes 20
        System.out.println("After poll(): " + numbers);
        
        // Using offer() to add elements
        numbers.offer(5); // Adding new element
        System.out.println("After offer(5): " + numbers);
        System.out.println("poll(): " + numbers.poll()); // Should return 5
        
        // PriorityQueue with custom ordering (reverse - max heap)
        System.out.println("\n=== Max Heap (Reverse Order) ===");
        PriorityQueue<Integer> maxHeap = new PriorityQueue<>(Comparator.reverseOrder());
        maxHeap.add(30);
        maxHeap.add(10);
        maxHeap.add(50);
        maxHeap.add(20);
        
        System.out.println("peek() in max heap: " + maxHeap.peek()); // Returns largest (50)
        System.out.println("poll() from max heap: " + maxHeap.poll()); // Removes 50
        
        // PriorityQueue with Strings (alphabetical order)
        System.out.println("\n=== String PriorityQueue ===");
        PriorityQueue<String> words = new PriorityQueue<>();
        words.add("Zebra");
        words.add("Apple");
        words.add("Mango");
        words.add("Banana");
        
        System.out.println("peek(): " + words.peek()); // Returns "Apple" (alphabetically first)
        System.out.println("All elements in order:");
        while (!words.isEmpty()) {
            System.out.println(words.poll());
        }
        
        // PriorityQueue with initial capacity
        System.out.println("\n=== With Initial Capacity ===");
        PriorityQueue<Double> prices = new PriorityQueue<>(100); // Initial capacity 100
        prices.add(99.99);
        prices.add(49.99);
        prices.add(149.99);
        prices.add(9.99);
        
        System.out.println("peek(): " + prices.peek()); // Returns 9.99 (lowest)
        
        // Real-time Example 1: Emergency room patient triage
        System.out.println("\n=== Example 1: Emergency Room Triage ===");
        PriorityQueue<String> erQueue = new PriorityQueue<>();
        erQueue.add("Patient D - Broken arm");
        erQueue.add("Patient A - Heart attack (critical)");
        erQueue.add("Patient C - Fever");
        erQueue.add("Patient B - Severe bleeding");
        
        System.out.println("Patients in order of priority:");
        while (!erQueue.isEmpty()) {
            System.out.println("Treating: " + erQueue.poll());
        }
        
        // Real-time Example 2: Task scheduler (priority-based)
        System.out.println("\n=== Example 2: Task Scheduler ===");
        class Task implements Comparable<Task> {
            int priority;
            String name;
            Task(int priority, String name) {
                this.priority = priority;
                this.name = name;
            }
            public int compareTo(Task t) {
                return this.priority - t.priority; // Lower number = higher priority
            }
            public String toString() { return priority + ": " + name; }
        }
        
        PriorityQueue<Task> tasks = new PriorityQueue<>();
        tasks.add(new Task(5, "Send email"));
        tasks.add(new Task(1, "Fix critical bug"));
        tasks.add(new Task(3, "Code review"));
        tasks.add(new Task(2, "Deploy to staging"));
        
        System.out.println("Tasks by priority (1=highest):");
        while (!tasks.isEmpty()) {
            System.out.println("Executing: " + tasks.poll());
        }
        
        // Real-time Example 3: Restaurant waitlist (VIP priority)
        System.out.println("\n=== Example 3: Restaurant Reservations ===");
        class Reservation implements Comparable<Reservation> {
            int priority; // Lower = higher priority
            String name;
            Reservation(int priority, String name) {
                this.priority = priority;
                this.name = name;
            }
            public int compareTo(Reservation r) {
                return this.priority - r.priority;
            }
            public String toString() { return name + " (Priority: " + priority + ")"; }
        }
        
        PriorityQueue<Reservation> reservations = new PriorityQueue<>();
        reservations.add(new Reservation(3, "Regular Customer"));
        reservations.add(new Reservation(1, "VIP Guest"));
        reservations.add(new Reservation(2, "Business Partner"));
        reservations.add(new Reservation(4, "Walk-in"));
        
        System.out.println("Seating order:");
        while (!reservations.isEmpty()) {
            System.out.println("Seating: " + reservations.poll());
        }
        
        // Real-time Example 4: Stock price alerts (lowest first)
        System.out.println("\n=== Example 4: Stock Price Alerts ===");
        PriorityQueue<Double> priceAlerts = new PriorityQueue<>();
        priceAlerts.add(150.50);
        priceAlerts.add(145.00);
        priceAlerts.add(155.75);
        priceAlerts.add(140.25);
        
        System.out.println("Alert triggered for lowest price:");
        double lowestPrice = priceAlerts.poll();
        System.out.println("Buy signal at: $" + lowestPrice);
        
        // Real-time Example 5: Print job queue (document priority)
        System.out.println("\n=== Example 5: Print Job Queue ===");
        class PrintJob implements Comparable<PrintJob> {
            int pages;
            String document;
            int priority;
            
            PrintJob(int pages, String document, int priority) {
                this.pages = pages;
                this.document = document;
                this.priority = priority;
            }
            
            public int compareTo(PrintJob p) {
                if (this.priority != p.priority) {
                    return this.priority - p.priority;
                }
                return this.pages - p.pages; // Tie-breaker: fewer pages first
            }
            
            public String toString() {
                return document + " - " + pages + " pages - Priority: " + priority;
            }
        }
        
        PriorityQueue<PrintJob> printQueue = new PriorityQueue<>();
        printQueue.add(new PrintJob(10, "Report.pdf", 2));
        printQueue.add(new PrintJob(100, "Book.pdf", 3));
        printQueue.add(new PrintJob(1, "Ticket.pdf", 1));
        printQueue.add(new PrintJob(5, "Memo.pdf", 2));
        
        System.out.println("Print queue order:");
        while (!printQueue.isEmpty()) {
            System.out.println("Printing: " + printQueue.poll());
        }
        
        // Real-time Example 6: Event-driven simulation (event time priority)
        System.out.println("\n=== Example 6: Event Simulation ===");
        class Event implements Comparable<Event> {
            int time;
            String event;
            
            Event(int time, String event) {
                this.time = time;
                this.event = event;
            }
            
            public int compareTo(Event e) {
                return this.time - e.time; // Earlier time = higher priority
            }
            
            public String toString() {
                return "Time " + time + ": " + event;
            }
        }
        
        PriorityQueue<Event> eventQueue = new PriorityQueue<>();
        eventQueue.add(new Event(15, "Game ends"));
        eventQueue.add(new Event(5, "Player joins"));
        eventQueue.add(new Event(10, "Game starts"));
        eventQueue.add(new Event(1, "Server starts"));
        
        System.out.println("Event execution order:");
        while (!eventQueue.isEmpty()) {
            System.out.println(eventQueue.poll());
        }
        
        // Additional operations
        System.out.println("\n=== Additional Operations ===");
        PriorityQueue<String> colors = new PriorityQueue<>();
        colors.add("Red");
        colors.add("Blue");
        
        System.out.println("Contains 'Red': " + colors.contains("Red"));
        System.out.println("Size: " + colors.size());
        
        String[] colorArray = colors.toArray(new String[0]);
        System.out.println("Converted to array: " + Arrays.toString(colorArray));
        
        colors.clear();
        System.out.println("After clear, isEmpty: " + colors.isEmpty());
    }
}
