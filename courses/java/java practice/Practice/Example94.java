/*
 * SUB TOPIC: TreeMap in Java
 * 
 * DEFINITION:
 * TreeMap is a Red-Black tree based implementation of the Map interface. It maintains keys 
 * in sorted (ascending) order by default, using the natural ordering of keys, or a custom 
 * Comparator provided at construction time. TreeMap provides O(log n) time complexity for 
 * get, put, and containsKey operations.
 * 
 * FUNCTIONALITIES:
 * 1. put() - Add key-value pair
 * 2. get() - Get value by key
 * 3. containsKey() - Check if key exists
 * 4. containsValue() - Check if value exists
 * 5. firstKey()/lastKey() - Get smallest/largest key
 * 6. lowerKey()/higherKey() - Get key less than/greater than given
 * 7. subMap() - Get portion of map
 * 8. headMap()/tailMap() - Get portion less than/greater than key
 * 9. remove() - Remove key-value pair
 * 10. size() - Get number of entries
 */

import java.util.*;

public class Example94 {
    public static void main(String[] args) {
        
        // Creating a TreeMap with natural ordering
        TreeMap<Integer, String> studentMap = new TreeMap<>();
        
        // put() - Adding key-value pairs
        studentMap.put(3, "Charlie");
        studentMap.put(1, "Alice");
        studentMap.put(2, "Bob");
        studentMap.put(5, "Eve");
        studentMap.put(4, "Diana");
        
        System.out.println("=== Basic TreeMap Operations ===");
        System.out.println("TreeMap (sorted by keys): " + studentMap);
        
        // get() - Get value by key
        System.out.println("\nget(2): " + studentMap.get(2));
        System.out.println("get(10): " + studentMap.get(10)); // Returns null
        
        // containsKey() - Check key existence
        System.out.println("\ncontainsKey(3): " + studentMap.containsKey(3));
        System.out.println("containsKey(10): " + studentMap.containsKey(10));
        
        // containsValue() - Check value existence
        System.out.println("\ncontainsValue('Alice'): " + studentMap.containsValue("Alice"));
        System.out.println("containsValue('John'): " + studentMap.containsValue("John"));
        
        // firstKey() and lastKey()
        System.out.println("\nfirstKey(): " + studentMap.firstKey());
        System.out.println("lastKey(): " + studentMap.lastKey());
        
        // lowerKey(), higherKey(), floorKey(), ceilingKey()
        System.out.println("\nlowerKey(3): " + studentMap.lowerKey(3)); // Key < 3
        System.out.println("higherKey(3): " + studentMap.higherKey(3)); // Key > 3
        System.out.println("floorKey(3): " + studentMap.floorKey(3)); // Key <= 3
        System.out.println("ceilingKey(3): " + studentMap.ceilingKey(3)); // Key >= 3
        
        // TreeMap with custom comparator (reverse order)
        System.out.println("\n=== Reverse Order TreeMap ===");
        TreeMap<String, Integer> reverseMap = new TreeMap<>(Comparator.reverseOrder());
        reverseMap.put("Apple", 10);
        reverseMap.put("Banana", 20);
        reverseMap.put("Cherry", 30);
        
        System.out.println("Reverse sorted map: " + reverseMap);
        
        // subMap() - Get portion of map
        System.out.println("\n=== SubMap Operations ===");
        TreeMap<Integer, String> scores = new TreeMap<>();
        scores.put(1, "Alice");
        scores.put(2, "Bob");
        scores.put(3, "Charlie");
        scores.put(4, "Diana");
        scores.put(5, "Eve");
        
        System.out.println("Original: " + scores);
        System.out.println("subMap(2, 4): " + scores.subMap(2, 4)); // Keys >= 2 and < 4
        System.out.println("subMap(2, true, 4, true): " + scores.subMap(2, true, 4, true)); // Inclusive
        
        // headMap() and tailMap()
        System.out.println("\nheadMap(3): " + scores.headMap(3)); // Keys < 3
        System.out.println("headMap(3, true): " + scores.headMap(3, true)); // Keys <= 3
        System.out.println("tailMap(3): " + scores.tailMap(3)); // Keys >= 3
        
        // Real-time Example 1: Student grade management
        System.out.println("\n=== Example 1: Student Grades ===");
        TreeMap<String, Double> grades = new TreeMap<>();
        grades.put("Alice", 95.5);
        grades.put("Bob", 87.0);
        grades.put("Charlie", 92.5);
        grades.put("Diana", 78.0);
        grades.put("Eve", 88.5);
        
        System.out.println("Students sorted alphabetically: " + grades);
        System.out.println("Top student: " + grades.lastKey());
        System.out.println("First student: " + grades.firstKey());
        
        // Get students with grades at end (last entries)
        System.out.println("Top students (last entries):");
        for (String name : grades.descendingMap().keySet()) {
            System.out.println(name + ": " + grades.get(name));
            if (grades.get(name) >= 90) break;
        }
        
        // Real-time Example 2: Product inventory by SKU
        System.out.println("\n=== Example 2: Product Inventory ===");
        TreeMap<String, Integer> inventory = new TreeMap<>();
        inventory.put("SKU001", 100);
        inventory.put("SKU010", 50);
        inventory.put("SKU005", 75);
        inventory.put("SKU002", 200);
        
        System.out.println("Products sorted by SKU: " + inventory);
        
        // Find next SKU after SKU003
        System.out.println("Higher SKU than SKU003: " + inventory.higherKey("SKU003"));
        System.out.println("Lower SKU than SKU010: " + inventory.lowerKey("SKU010"));
        
        // Real-time Example 3: Meeting room booking (time-based)
        System.out.println("\n=== Example 3: Meeting Schedule ===");
        TreeMap<Integer, String> meetings = new TreeMap<>();
        meetings.put(900, "Team Standup");
        meetings.put(1400, "Client Call");
        meetings.put(1100, "Code Review");
        meetings.put(1500, "Sprint Planning");
        
        System.out.println("Meetings sorted by time: " + meetings);
        System.out.println("First meeting: " + meetings.get(meetings.firstKey()));
        System.out.println("Last meeting: " + meetings.get(meetings.lastKey()));
        
        // Find meetings after 1200
        System.out.println("Afternoon meetings:");
        meetings.tailMap(1200).forEach((time, event) -> 
            System.out.println(time + ": " + event));
        
        // Real-time Example 4: Bank account transactions by ID
        System.out.println("\n=== Example 4: Transaction History ===");
        TreeMap<Long, String> transactions = new TreeMap<>();
        transactions.put(1001L, "Deposit $500");
        transactions.put(1003L, "Withdrawal $200");
        transactions.put(1002L, "Transfer $100");
        transactions.put(1005L, "Payment $50");
        transactions.put(1004L, "Deposit $300");
        
        System.out.println("Transactions in order: " + transactions);
        
        // Get transactions between ID 1002 and 1004
        System.out.println("Transactions 1002-1004: " + 
            transactions.subMap(1002L, true, 1004L, true));
        
        // Real-time Example 5: Country population (sorted)
        System.out.println("\n=== Example 5: Country Population ===");
        TreeMap<String, Long> population = new TreeMap<>();
        population.put("USA", 331000000L);
        population.put("China", 1440000000L);
        population.put("India", 1380000000L);
        population.put("Brazil", 212000000L);
        population.put("Canada", 38000000L);
        
        System.out.println("Countries sorted alphabetically: " + population);
        System.out.println("First country: " + population.firstKey());
        System.out.println("Last country: " + population.lastKey());
        
        // Real-time Example 6: Cache with expiration (simulated)
        System.out.println("\n=== Example 6: Cache System ===");
        TreeMap<Integer, String> cache = new TreeMap<>();
        cache.put(1000, "User1_data");
        cache.put(2000, "User2_data");
        cache.put(1500, "User3_data");
        cache.put(500, "User4_data");
        
        System.out.println("Cache entries: " + cache);
        
        // Simulate checking expired entries (older than 1500)
        System.out.println("Expired entries (key < 1500):");
        cache.headMap(1500).forEach((key, value) -> 
            System.out.println("Expired: " + key + " -> " + value));
        
        // Additional operations
        System.out.println("\n=== Additional Operations ===");
        TreeMap<Character, Integer> charCount = new TreeMap<>();
        charCount.put('A', 5);
        charCount.put('B', 10);
        
        System.out.println("Size: " + charCount.size());
        System.out.println("Is empty: " + charCount.isEmpty());
        
        charCount.remove('A');
        System.out.println("After remove('A'): " + charCount);
        
        // Get all keys
        System.out.println("Keys: " + charCount.keySet());
        
        // Get all values
        System.out.println("Values: " + charCount.values());
        
        // Get all entries
        System.out.println("Entries: " + charCount.entrySet());
        
        // Poll first and last entries
        System.out.println("\nPoll first entry: " + charCount.pollFirstEntry());
        System.out.println("Poll last entry: " + charCount.pollLastEntry());
    }
}
