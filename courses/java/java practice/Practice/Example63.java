/*
 * SUB TOPIC: Working with HashMap and HashSet
 * 
 * DEFINITION:
 * HashMap is a collection that stores key-value pairs with O(1) lookup. HashSet is a collection that stores 
 * unique elements with no duplicates.
 * 
 * FUNCTIONALITIES:
 * 1. HashMap - Key-value storage
 * 2. HashSet - Unique elements
 * 3. Iteration
 * 4. getOrDefault
 * 5. putIfAbsent
 * 6. Remove and clear
 */

import java.util.*;

public class Example63 {
    public static void main(String[] args) {
        
        // HashMap
        System.out.println("=== HashMap ===");
        
        HashMap<String, Integer> map = new HashMap<>();
        map.put("Apple", 10);
        map.put("Banana", 20);
        map.put("Orange", 15);
        
        System.out.println("Map: " + map);
        System.out.println("Get Apple: " + map.get("Apple"));
        
        // HashSet
        System.out.println("\n=== HashSet ===");
        
        HashSet<Integer> set = new HashSet<>();
        set.add(1);
        set.add(2);
        set.add(3);
        set.add(2); // Duplicate - ignored
        
        System.out.println("Set: " + set);
        System.out.println("Size: " + set.size());
        
        // Iteration
        System.out.println("\n=== Iteration ===");
        
        for (String key : map.keySet()) {
            System.out.println(key + ": " + map.get(key));
        }
        
        // getOrDefault
        System.out.println("\n=== getOrDefault ===");
        
        System.out.println("Apple: " + map.getOrDefault("Apple", 0));
        System.out.println("Grape: " + map.getOrDefault("Grape", 0));
        
        // putIfAbsent
        System.out.println("\n=== putIfAbsent ===");
        
        map.putIfAbsent("Grape", 25);
        System.out.println("After putIfAbsent: " + map);
        
        // Real-time Example 1: Word frequency
        System.out.println("\n=== Example 1: Word Count ===");
        
        String text = "hello world hello java world";
        String[] words = text.split(" ");
        HashMap<String, Integer> wordCount = new HashMap<>();
        
        for (String word : words) {
            wordCount.put(word, wordCount.getOrDefault(word, 0) + 1);
        }
        
        System.out.println("Word count: " + wordCount);
        
        // Real-time Example 2: Unique visitors
        System.out.println("\n=== Example 2: Unique Visitors ===");
        
        HashSet<String> visitors = new HashSet<>();
        visitors.add("User1");
        visitors.add("User2");
        visitors.add("User1"); // Duplicate
        visitors.add("User3");
        
        System.out.println("Unique visitors: " + visitors.size());
        
        // Real-time Example 3: Price lookup
        System.out.println("\n=== Example 3: Products ===");
        
        HashMap<String, Double> prices = new HashMap<>();
        prices.put("Laptop", 999.99);
        prices.put("Phone", 699.99);
        
        System.out.println("Laptop price: $" + prices.get("Laptop"));
        
        // Real-time Example 4: Group by first letter
        System.out.println("\n=== Example 4: Group ===");
        
        String[] names = {"Alice", "Bob", "Anna", "Charlie"};
        HashMap<Character, List<String>> grouped = new HashMap<>();
        
        for (String name : names) {
            char first = name.charAt(0);
            grouped.putIfAbsent(first, new ArrayList<>());
            grouped.get(first).add(name);
        }
        
        System.out.println("Grouped: " + grouped);
        
        // Real-time Example 5: Set operations
        System.out.println("\n=== Example 5: Set Operations ===");
        
        HashSet<Integer> set1 = new HashSet<>(Arrays.asList(1, 2, 3, 4));
        HashSet<Integer> set2 = new HashSet<>(Arrays.asList(3, 4, 5, 6));
        
        HashSet<Integer> union = new HashSet<>(set1);
        union.addAll(set2);
        
        HashSet<Integer> intersection = new HashSet<>(set1);
        intersection.retainAll(set2);
        
        System.out.println("Union: " + union);
        System.out.println("Intersection: " + intersection);
        
        // Real-time Example 6: Remove entries
        System.out.println("\n=== Example 6: Remove ===");
        
        HashMap<String, Integer> scores = new HashMap<>();
        scores.put("John", 90);
        scores.put("Jane", 85);
        scores.put("Bob", 80);
        
        scores.remove("Bob");
        System.out.println("After remove: " + scores);
    }
}
