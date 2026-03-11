// HashMapDemo - Demonstrates HashMap in Java Collections Framework
// Important for Angular developers - maps to JSON objects

import java.util.*;

public class HashMapDemo {
    
    public static void main(String[] args) {
        System.out.println("=== HASHMAP DEMO ===");
        
        // Create HashMap
        HashMap<Integer, String> studentMap = new HashMap<>();
        
        // Put elements
        studentMap.put(101, "Alice");
        studentMap.put(102, "Bob");
        studentMap.put(103, "Charlie");
        studentMap.put(104, "Diana");
        
        System.out.println("Students: " + studentMap);
        
        // Get by key
        System.out.println("Student 102: " + studentMap.get(102));
        
        // Check existence
        System.out.println("Contains key 105: " + studentMap.containsKey(105));
        System.out.println("Contains value 'Alice': " + studentMap.containsValue("Alice"));
        
        // Iterate
        System.out.println("All entries:");
        for (Map.Entry<Integer, String> entry : studentMap.entrySet()) {
            System.out.println("  ID: " + entry.getKey() + ", Name: " + entry.getValue());
        }
        
        // Remove
        studentMap.remove(103);
        System.out.println("After removing 103: " + studentMap);
        
        System.out.println("\n=== USE CASES ===");
        System.out.println("1. Store user data by ID");
        System.out.println("2. Cache frequently accessed data");
        System.out.println("3. Maps directly to JSON objects");
    }
}
