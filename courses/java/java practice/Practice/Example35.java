/*
 * SUB TOPIC: Collections Framework in Java
 * 
 * DEFINITION:
 * The Collections Framework in Java is a set of classes and interfaces that provide ready-made 
 * data structures for storing and manipulating groups of objects. It includes List, Set, Map, 
 * Queue interfaces and their implementations like ArrayList, HashSet, HashMap, etc.
 * 
 * FUNCTIONALITIES:
 * 1. ArrayList - Dynamic array that grows and shrinks
 * 2. HashSet - Collection with no duplicate elements
 * 3. HashMap - Key-value pair storage
 * 4. LinkedList - Linked list implementation
 * 5. Iterator - Traverse collections
 * 6. Collections utility methods - sort, reverse, etc.
 */

import java.util.*; // Import all collection classes

public class Example35 {
    public static void main(String[] args) {
        
        // Topic Explanation: ArrayList - Dynamic Array
        
        // ArrayList: Resizable array implementation
        System.out.println("=== ArrayList ===");
        ArrayList<String> fruits = new ArrayList<>(); // Create ArrayList
        
        // add() - Add elements
        fruits.add("Apple");
        fruits.add("Banana");
        fruits.add("Cherry");
        
        // size() - Get number of elements
        System.out.println("Size: " + fruits.size());
        
        // get() - Get element at index
        System.out.println("First fruit: " + fruits.get(0));
        
        // set() - Update element
        fruits.set(0, "Mango");
        System.out.println("After set: " + fruits.get(0));
        
        // remove() - Remove element
        fruits.remove("Banana");
        System.out.println("After remove: " + fruits);
        
        // Topic Explanation: HashSet - No Duplicates
        
        System.out.println("\n=== HashSet ===");
        HashSet<Integer> numbers = new HashSet<>();
        
        numbers.add(10);
        numbers.add(20);
        numbers.add(10); // Duplicate - ignored
        numbers.add(30);
        
        System.out.println("HashSet elements: " + numbers);
        System.out.println("Size: " + numbers.size());
        
        // contains() - Check if element exists
        System.out.println("Contains 20: " + numbers.contains(20));
        
        // Topic Explanation: HashMap - Key-Value Pairs
        
        System.out.println("\n=== HashMap ===");
        HashMap<String, Integer> studentScores = new HashMap<>();
        
        // put() - Add key-value pairs
        studentScores.put("Alice", 95);
        studentScores.put("Bob", 87);
        studentScores.put("Charlie", 92);
        
        // get() - Get value by key
        System.out.println("Alice's score: " + studentScores.get("Alice"));
        
        // keySet() - Get all keys
        System.out.println("All students: " + studentScores.keySet());
        
        // values() - Get all values
        System.out.println("All scores: " + studentScores.values());
        
        // putIfAbsent() - Add only if key doesn't exist
        studentScores.putIfAbsent("David", 88);
        
        // remove() - Remove by key
        studentScores.remove("Bob");
        
        System.out.println("After changes: " + studentScores);
        
        // Real-time Example 1: Shopping cart using ArrayList
        System.out.println("\n=== Example 1: Shopping Cart ===");
        ArrayList<String> cart = new ArrayList<>();
        
        cart.add("Laptop");
        cart.add("Mouse");
        cart.add("Keyboard");
        cart.add("Mouse"); // Duplicate item
        
        System.out.println("Items in cart: " + cart);
        System.out.println("Total items: " + cart.size());
        
        // Real-time Example 2: Unique tags using HashSet
        System.out.println("\n=== Example 2: Unique Tags ===");
        HashSet<String> tags = new HashSet<>();
        
        String[] articleTags = {"java", "programming", "tutorial", "java", "beginners", "programming"};
        
        for (String tag : articleTags) {
            tags.add(tag.toLowerCase()); // Add unique tags
        }
        
        System.out.println("Article tags: " + tags);
        
        // Real-time Example 3: Phonebook using HashMap
        System.out.println("\n=== Example 3: Phonebook ===");
        HashMap<String, String> phonebook = new HashMap<>();
        
        phonebook.put("John", "9876543210");
        phonebook.put("Jane", "9876543211");
        phonebook.put("Mike", "9876543212");
        
        System.out.println("John's number: " + phonebook.get("John"));
        
        // Update number
        phonebook.put("John", "9999999999");
        System.out.println("John's updated number: " + phonebook.get("John"));
        
        // Real-time Example 4: Employee directory
        System.out.println("\n=== Example 4: Employee Directory ===");
        HashMap<Integer, String> employees = new HashMap<>();
        
        employees.put(101, "Alice");
        employees.put(102, "Bob");
        employees.put(103, "Charlie");
        
        // Display all employees
        System.out.println("Employee IDs: " + employees.keySet());
        
        // Search by ID
        int searchId = 102;
        if (employees.containsKey(searchId)) {
            System.out.println("Employee " + searchId + ": " + employees.get(searchId));
        }
        
        // Real-time Example 5: Inventory management
        System.out.println("\n=== Example 5: Inventory ===");
        HashMap<String, Integer> inventory = new HashMap<>();
        
        inventory.put("Apples", 50);
        inventory.put("Oranges", 30);
        inventory.put("Bananas", 20);
        
        // Reduce stock
        String item = "Apples";
        int currentStock = inventory.get(item);
        inventory.put(item, currentStock - 5);
        
        System.out.println("Apples remaining: " + inventory.get("Apples"));
        
        // Real-time Example 6: Queue simulation (waiting list)
        System.out.println("\n=== Example 6: Waiting List ===");
        LinkedList<String> waitingList = new LinkedList<>();
        
        waitingList.add("Customer1");
        waitingList.add("Customer2");
        waitingList.add("Customer3");
        
        System.out.println("Waiting: " + waitingList);
        
        // First customer served
        String served = waitingList.poll(); // Removes first element
        System.out.println("Served: " + served);
        System.out.println("Remaining: " + waitingList);
        
        // Add new customer to end
        waitingList.add("Customer4");
        System.out.println("After adding: " + waitingList);
        
        // Collections utility methods
        System.out.println("\n=== Collections Utility ===");
        ArrayList<Integer> nums = new ArrayList<>();
        nums.add(5);
        nums.add(2);
        nums.add(8);
        nums.add(1);
        
        System.out.println("Original: " + nums);
        
        Collections.sort(nums); // Sort ascending
        System.out.println("Sorted: " + nums);
        
        Collections.reverse(nums); // Reverse order
        System.out.println("Reversed: " + nums);
        
        Collections.shuffle(nums); // Shuffle randomly
        System.out.println("Shuffled: " + nums);
    }
}
