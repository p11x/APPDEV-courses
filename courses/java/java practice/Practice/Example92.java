/*
 * SUB TOPIC: LinkedList Operations in Java
 * 
 * DEFINITION:
 * LinkedList is a doubly-linked list implementation of the List and Deque interfaces. 
 * It allows null elements and provides O(1) insertion/deletion at both ends. Unlike ArrayList, 
 * LinkedList provides efficient O(1) time complexity for adding and removing elements from 
 * both ends, but O(n) for random access by index.
 * 
 * FUNCTIONALITIES:
 * 1. addFirst()/addLast() - Add element at beginning/end
 * 2. removeFirst()/removeLast() - Remove from beginning/end
 * 3. getFirst()/getLast() - Get first/last element without removing
 * 4. push()/pop() - Stack operations
 * 5. pollFirst()/pollLast() - Remove and return first/last (returns null if empty)
 * 6. peekFirst()/peekLast() - Get without removing (returns null if empty)
 * 7. contains() - Check if element exists
 * 8. size() - Get number of elements
 */

import java.util.*;

public class Example92 {
    public static void main(String[] args) {
        
        // Creating a LinkedList of strings
        LinkedList<String> colors = new LinkedList<>();
        
        // Adding elements using add()
        colors.add("Red");
        colors.add("Blue");
        colors.add("Green");
        
        System.out.println("=== Basic LinkedList Operations ===");
        System.out.println("Initial list: " + colors);
        
        // addFirst() - Add element at the beginning
        colors.addFirst("Yellow");
        System.out.println("After addFirst('Yellow'): " + colors);
        
        // addLast() - Add element at the end
        colors.addLast("Purple");
        System.out.println("After addLast('Purple'): " + colors);
        
        // getFirst() and getLast() - Get without removing
        System.out.println("\nFirst element: " + colors.getFirst());
        System.out.println("Last element: " + colors.getLast());
        
        // removeFirst() - Remove and return first element
        String removed = colors.removeFirst();
        System.out.println("\nRemoved first: " + removed);
        System.out.println("After removeFirst(): " + colors);
        
        // removeLast() - Remove and return last element
        removed = colors.removeLast();
        System.out.println("Removed last: " + removed);
        System.out.println("After removeLast(): " + colors);
        
        // Stack operations: push() and pop()
        System.out.println("\n=== Stack Operations (push/pop) ===");
        LinkedList<Integer> stack = new LinkedList<>();
        stack.push(10); // Add to top of stack
        stack.push(20);
        stack.push(30);
        System.out.println("Stack after pushes: " + stack);
        System.out.println("Pop: " + stack.pop()); // Remove from top
        System.out.println("Stack after pop: " + stack);
        
        // Queue operations: offer(), poll(), peek()
        System.out.println("\n=== Queue Operations ===");
        LinkedList<String> queue = new LinkedList<>();
        queue.offer("Customer1"); // Add to queue
        queue.offer("Customer2");
        queue.offer("Customer3");
        System.out.println("Queue: " + queue);
        System.out.println("Poll (serve): " + queue.poll()); // Remove from front
        System.out.println("After poll: " + queue);
        System.out.println("Peek (front): " + queue.peek());
        
        // pollFirst() and pollLast() - Safe removal
        System.out.println("\n=== Safe Removal Operations ===");
        LinkedList<String> names = new LinkedList<>(Arrays.asList("A", "B", "C", "D", "E"));
        System.out.println("Original: " + names);
        System.out.println("pollFirst(): " + names.pollFirst()); // Returns null if empty
        System.out.println("pollLast(): " + names.pollLast());
        System.out.println("After polls: " + names);
        
        // peekFirst() and peekLast() - Safe access
        LinkedList<Double> prices = new LinkedList<>(Arrays.asList(10.5, 20.0, 30.75));
        System.out.println("\npeekFirst(): " + prices.peekFirst());
        System.out.println("peekLast(): " + prices.peekLast());
        
        // Real-time Example 1: Browser history (back/forward navigation)
        System.out.println("\n=== Example 1: Browser History ===");
        LinkedList<String> browserHistory = new LinkedList<>();
        browserHistory.add("google.com");
        browserHistory.add("facebook.com");
        browserHistory.add("twitter.com");
        
        System.out.println("Current page: " + browserHistory.peekLast());
        System.out.println("Going back...");
        String previousPage = browserHistory.pollLast();
        System.out.println("Navigated to: " + browserHistory.peekLast());
        
        // Adding new page (clears forward history in real browsers)
        browserHistory.add("linkedin.com");
        System.out.println("New page opened: " + browserHistory.peekLast());
        
        // Real-time Example 2: Music playlist management
        System.out.println("\n=== Example 2: Music Playlist ===");
        LinkedList<String> playlist = new LinkedList<>();
        playlist.add("Song1.mp3");
        playlist.add("Song2.mp3");
        playlist.add("Song3.mp3");
        playlist.add("Song4.mp3");
        
        System.out.println("Playlist: " + playlist);
        System.out.println("Now playing: " + playlist.getFirst());
        playlist.removeFirst(); // Song finished, remove it
        System.out.println("Next song: " + playlist.getFirst());
        
        // Add to end (when adding new songs)
        playlist.addLast("Song5.mp3");
        System.out.println("After adding new song: " + playlist);
        
        // Real-time Example 3: Task execution queue
        System.out.println("\n=== Example 3: Task Execution Queue ===");
        LinkedList<Runnable> taskQueue = new LinkedList<>();
        
        taskQueue.add(() -> System.out.println("Task A executed"));
        taskQueue.add(() -> System.out.println("Task B executed"));
        taskQueue.add(() -> System.out.println("Task C executed"));
        
        System.out.println("Tasks in queue: " + taskQueue.size());
        while (!taskQueue.isEmpty()) {
            Runnable task = taskQueue.poll(); // Get next task
            task.run(); // Execute it
        }
        
        // Real-time Example 4: Undo/Redo functionality
        System.out.println("\n=== Example 4: Undo Operation ===");
        LinkedList<String> documentStates = new LinkedList<>();
        documentStates.add(""); // Initial empty state
        documentStates.add("Hello");
        documentStates.add("Hello World");
        documentStates.add("Hello World Java");
        
        System.out.println("Current document: " + documentStates.peekLast());
        System.out.println("Performing undo...");
        documentStates.pollLast(); // Remove last state
        System.out.println("After undo: " + documentStates.peekLast());
        
        // Real-time Example 5: Railway station platform
        System.out.println("\n=== Example 5: Railway Platform ===");
        LinkedList<String> train = new LinkedList<>();
        train.add("Engine");
        train.add("Coach A");
        train.add("Coach B");
        train.add("Coach C");
        
        System.out.println("Train composition: " + train);
        System.out.println("Adding dining car at front...");
        train.addFirst("Dining Car");
        System.out.println("Train now: " + train);
        
        System.out.println("Removing last coach for maintenance...");
        train.removeLast();
        System.out.println("Train after maintenance: " + train);
        
        // Real-time Example 6: Auction bid management
        System.out.println("\n=== Example 6: Auction Bids ===");
        LinkedList<Double> bids = new LinkedList<>();
        bids.add(100.0);
        bids.add(150.0);
        bids.add(200.0);
        
        System.out.println("Current highest bid: " + bids.peekLast());
        System.out.println("New bid placed: 250.0");
        bids.add(250.0);
        System.out.println("Highest bid now: " + bids.peekLast());
        
        System.out.println("\nAll bids (lowest to highest): " + bids);
        
        // Additional operations
        System.out.println("\n=== Additional Operations ===");
        LinkedList<String> fruits = new LinkedList<>();
        fruits.add("Apple");
        fruits.add("Banana");
        fruits.add("Cherry");
        
        System.out.println("Contains 'Banana': " + fruits.contains("Banana"));
        System.out.println("Size: " + fruits.size());
        System.out.println("Is empty: " + fruits.isEmpty());
        
        // Using ListIterator for bidirectional traversal
        System.out.println("\n=== Using ListIterator ===");
        ListIterator<String> iter = fruits.listIterator();
        while (iter.hasNext()) {
            System.out.print(iter.next() + " ");
        }
        System.out.println();
        while (iter.hasPrevious()) {
            System.out.print(iter.previous() + " ");
        }
    }
}
