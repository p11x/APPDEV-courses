// LinkedListDemo - Demonstrates LinkedList in Java Collections Framework
// Important for queue and deque operations

import java.util.*;

public class LinkedListDemo {
    
    public static void main(String[] args) {
        System.out.println("=== LINKEDLIST DEMO ===");
        
        LinkedList<String> queue = new LinkedList<>();
        
        queue.add("First");
        queue.add("Second");
        queue.add("Third");
        
        System.out.println("Queue: " + queue);
        
        // Queue operations
        System.out.println("Peek: " + queue.peek());
        System.out.println("Poll: " + queue.poll());
        System.out.println("After poll: " + queue);
        
        // Add to front/back
        queue.addFirst("New First");
        queue.addLast("New Last");
        System.out.println("After additions: " + queue);
        
        System.out.println("\n=== KEY OPERATIONS ===");
        System.out.println("addFirst(), addLast() - Add elements");
        System.out.println("removeFirst(), removeLast() - Remove elements");
        System.out.println("peek() - Get without removing");
        System.out.println("poll() - Get and remove");
    }
}
