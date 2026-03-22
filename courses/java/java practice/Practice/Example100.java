/*
 * SUB TOPIC: Deque Interface in Java
 * 
 * DEFINITION:
 * Deque (Double-Ended Queue) is an interface that extends Queue and allows insertion and removal 
 * at both ends. It can be used as a queue (FIFO) or stack (LIFO). ArrayDeque is the most common 
 * implementation which provides O(1) time complexity for add/remove operations at both ends.
 * 
 * FUNCTIONALITIES:
 * 1. addFirst()/addLast() - Add element at front/rear
 * 2. removeFirst()/removeLast() - Remove from front/rear
 * 3. getFirst()/getLast() - View first/last without removing
 * 4. offerFirst()/offerLast() - Add (returns false if full)
 * 5. pollFirst()/pollLast() - Remove (returns null if empty)
 * 6. peekFirst()/peekLast() - View (returns null if empty)
 * 7. push()/pop() - Stack operations
 * 8. descendingIterator() - Iterate in reverse
 */

import java.util.*;

public class Example100 {
    public static void main(String[] args) {
        
        // Creating a Deque using ArrayDeque
        Deque<String> deque = new ArrayDeque<>();
        
        // addFirst() - Add at beginning
        deque.addFirst("First");
        deque.addFirst("Before First");
        
        // addLast() - Add at end
        deque.addLast("Last");
        deque.addLast("After Last");
        
        System.out.println("=== Basic Deque Operations ===");
        System.out.println("Deque: " + deque);
        
        // getFirst() and getLast() - View without removing
        System.out.println("\ngetFirst(): " + deque.getFirst());
        System.out.println("getLast(): " + deque.getLast());
        
        // removeFirst() and removeLast()
        System.out.println("\nremoveFirst(): " + deque.removeFirst());
        System.out.println("After removeFirst: " + deque);
        
        System.out.println("removeLast(): " + deque.removeLast());
        System.out.println("After removeLast: " + deque);
        
        // offerFirst() and offerLast() (preferred methods)
        System.out.println("\n=== Using offerFirst/offerLast ===");
        Deque<Integer> numbers = new ArrayDeque<>();
        
        numbers.offerFirst(10);  // Add at front
        numbers.offerLast(20);   // Add at back
        numbers.offerFirst(5);   // Add at front
        numbers.offerLast(30);   // Add at back
        
        System.out.println("Deque: " + numbers);
        System.out.println("pollFirst(): " + numbers.pollFirst());
        System.out.println("pollLast(): " + numbers.pollLast());
        System.out.println("After polls: " + numbers);
        
        // peekFirst() and peekLast()
        System.out.println("\npeekFirst(): " + numbers.peekFirst());
        System.out.println("peekLast(): " + numbers.peekLast());
        
        // Using as Stack (LIFO)
        System.out.println("\n=== Deque as Stack (LIFO) ===");
        Deque<String> stack = new ArrayDeque<>();
        
        stack.push("Item1");
        stack.push("Item2");
        stack.push("Item3");
        
        System.out.println("Stack: " + stack);
        System.out.println("pop(): " + stack.pop());
        System.out.println("pop(): " + stack.pop());
        System.out.println("After pops: " + stack);
        
        // Using as Queue (FIFO)
        System.out.println("\n=== Deque as Queue (FIFO) ===");
        Deque<String> queue = new ArrayDeque<>();
        
        queue.add("Customer1");
        queue.add("Customer2");
        queue.add("Customer3");
        
        System.out.println("Queue: " + queue);
        System.out.println("poll(): " + queue.poll());
        System.out.println("poll(): " + queue.poll());
        System.out.println("After polls: " + queue);
        
        // Real-time Example 1: Browser history (forward/back navigation)
        System.out.println("\n=== Example 1: Browser History ===");
        Deque<String> history = new ArrayDeque<>();
        
        history.addLast("google.com");
        history.addLast("facebook.com");
        history.addLast("twitter.com");
        
        System.out.println("Current page: " + history.getLast());
        System.out.println("Going back...");
        String previous = history.removeLast();
        System.out.println("Now at: " + history.getLast());
        
        // Navigate to new page
        history.addLast("linkedin.com");
        System.out.println("Now at: " + history.getLast());
        
        // Real-time Example 2: Undo/Redo operations
        System.out.println("\n=== Example 2: Undo/Redo System ===");
        Deque<String> undoStack = new ArrayDeque<>();
        Deque<String> redoStack = new ArrayDeque<>();
        
        // Add actions
        undoStack.addLast("Type 'A'");
        undoStack.addLast("Type 'B'");
        undoStack.addLast("Type 'C'");
        
        System.out.println("Actions: " + undoStack);
        
        // Undo last action
        String undone = undoStack.removeLast();
        redoStack.addLast(undone);
        System.out.println("Undo: " + undone);
        
        // Undo again
        undone = undoStack.removeLast();
        redoStack.addLast(undone);
        System.out.println("Undo: " + undone);
        
        System.out.println("Undo stack: " + undoStack);
        System.out.println("Redo stack: " + redoStack);
        
        // Redo
        String redone = redoStack.removeLast();
        undoStack.addLast(redone);
        System.out.println("Redo: " + redone);
        
        // Real-time Example 3: Sliding window maximum
        System.out.println("\n=== Example 3: Recent Items Cache ===");
        Deque<String> recentItems = new ArrayDeque<>(5); // Max 5 items
        
        recentItems.addLast("Item1");
        recentItems.addLast("Item2");
        recentItems.addLast("Item3");
        recentItems.addLast("Item4");
        recentItems.addLast("Item5");
        
        System.out.println("Recent items: " + recentItems);
        
        // Add new item (removes oldest if full)
        if (recentItems.size() >= 5) {
            recentItems.removeFirst(); // Remove oldest
        }
        recentItems.addLast("Item6");
        System.out.println("After adding Item6: " + recentItems);
        
        // Real-time Example 4: Task distribution
        System.out.println("\n=== Example 4: Round-Robin Task Distribution ===");
        Deque<String> taskQueue = new ArrayDeque<>();
        
        // Add tasks from multiple producers
        taskQueue.addLast("TaskA");
        taskQueue.addLast("TaskB");
        taskQueue.addLast("TaskC");
        taskQueue.addLast("TaskD");
        
        // Multiple workers process tasks
        for (int worker = 1; worker <= 3; worker++) {
            if (!taskQueue.isEmpty()) {
                String task = taskQueue.removeFirst();
                System.out.println("Worker " + worker + " processing: " + task);
            }
        }
        System.out.println("Remaining tasks: " + taskQueue);
        
        // Real-time Example 5: Palindrome checker
        System.out.println("\n=== Example 5: Palindrome Checker ===");
        String text = "racecar";
        
        Deque<Character> charDeque = new ArrayDeque<>();
        for (char c : text.toCharArray()) {
            charDeque.addLast(c);
        }
        
        boolean isPalindrome = true;
        while (charDeque.size() > 1) {
            if (!charDeque.removeFirst().equals(charDeque.removeLast())) {
                isPalindrome = false;
                break;
            }
        }
        
        System.out.println("Text: " + text);
        System.out.println("Is Palindrome: " + isPalindrome);
        
        // Another example
        text = "hello";
        charDeque.clear();
        for (char c : text.toCharArray()) {
            charDeque.addLast(c);
        }
        
        isPalindrome = true;
        while (charDeque.size() > 1) {
            if (!charDeque.removeFirst().equals(charDeque.removeLast())) {
                isPalindrome = false;
                break;
            }
        }
        System.out.println("Text: " + text);
        System.out.println("Is Palindrome: " + isPalindrome);
        
        // Real-time Example 6: Priority message handling
        System.out.println("\n=== Example 6: Message Priority Handling ===");
        Deque<String> priorityMessages = new ArrayDeque<>();
        Deque<String> normalMessages = new ArrayDeque<>();
        
        // Add messages
        priorityMessages.addLast("URGENT: Server down!");
        priorityMessages.addLast("URGENT: Database error");
        normalMessages.addLast("Daily report ready");
        normalMessages.addLast("Meeting reminder");
        
        System.out.println("Processing messages:");
        
        // Process priority first
        while (!priorityMessages.isEmpty()) {
            System.out.println("[PRIORITY] " + priorityMessages.removeFirst());
        }
        
        // Then normal
        while (!normalMessages.isEmpty()) {
            System.out.println("[NORMAL] " + normalMessages.removeFirst());
        }
        
        // Additional operations
        System.out.println("\n=== Additional Operations ===");
        Deque<String> testDeque = new ArrayDeque<>();
        testDeque.add("A");
        testDeque.add("B");
        testDeque.add("C");
        
        System.out.println("Contains 'B': " + testDeque.contains("B"));
        System.out.println("Size: " + testDeque.size());
        
        // Descending iterator
        System.out.println("Descending order:");
        Iterator<String> descIter = testDeque.descendingIterator();
        while (descIter.hasNext()) {
            System.out.println("  " + descIter.next());
        }
        
        // Remove first/last occurrence
        testDeque.removeFirst();
        testDeque.removeLast();
        System.out.println("After removals: " + testDeque);
        
        // Using ArrayDeque as efficient queue
        System.out.println("\n=== ArrayDeque Performance ===");
        Deque<Integer> efficientDeque = new ArrayDeque<>(1000);
        
        long startTime = System.currentTimeMillis();
        for (int i = 0; i < 100000; i++) {
            efficientDeque.addLast(i);
        }
        long endTime = System.currentTimeMillis();
        
        System.out.println("Added 100,000 elements in " + (endTime - startTime) + "ms");
        System.out.println("Size: " + efficientDeque.size());
        
        // Clear
        efficientDeque.clear();
        System.out.println("After clear, isEmpty: " + efficientDeque.isEmpty());
    }
}
