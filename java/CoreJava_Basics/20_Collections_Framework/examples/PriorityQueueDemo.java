// PriorityQueueDemo - Demonstrates PriorityQueue in Java Collections Framework
// Important for priority-based processing

import java.util.*;

public class PriorityQueueDemo {
    
    public static void main(String[] args) {
        System.out.println("=== PRIORITYQUEUE DEMO ===");
        
        PriorityQueue<Integer> pq = new PriorityQueue<>();
        
        pq.add(30);
        pq.add(10);
        pq.add(50);
        pq.add(20);
        
        System.out.println("Elements (in priority order):");
        while (!pq.isEmpty()) {
            System.out.print(pq.poll() + " ");
        }
        System.out.println();
        
        System.out.println("\n=== USE CASES ===");
        System.out.println("1. Task scheduling");
        System.out.println("2. Priority-based processing");
        System.out.println("3. Finding kth largest/smallest element");
    }
}
