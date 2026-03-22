/*
 * SUB TOPIC: Advanced Collections - LinkedList, HashMap, TreeSet
 * 
 * DEFINITION:
 * LinkedList is a doubly-linked list implementation for fast insertions/deletions. HashMap provides 
 * key-value pair storage with O(1) lookup. TreeSet maintains sorted elements without duplicates.
 * 
 * FUNCTIONALITIES:
 * 1. LinkedList - Add, remove, get from both ends
 * 2. HashMap - Put, get, containsKey, remove by key
 * 3. TreeSet - Sorted set, navigation methods
 * 4. HashMap iteration
 * 5. LinkedList as Stack/Queue
 */

import java.util.*;

public class Example51 {
    public static void main(String[] args) {
        
        // LinkedList - Dynamic list
        System.out.println("=== LinkedList ===");
        LinkedList<String> linkedList = new LinkedList<>();
        
        linkedList.add("First");
        linkedList.add("Second");
        linkedList.addFirst("New First");
        linkedList.addLast("Last");
        
        System.out.println("List: " + linkedList);
        System.out.println("First: " + linkedList.getFirst());
        System.out.println("Last: " + linkedList.getLast());
        
        // HashMap - Key-value pairs
        System.out.println("\n=== HashMap ===");
        HashMap<Integer, String> map = new HashMap<>();
        
        map.put(1, "One");
        map.put(2, "Two");
        map.put(3, "Three");
        
        System.out.println("Map: " + map);
        System.out.println("Get key 2: " + map.get(2));
        System.out.println("Contains key 3: " + map.containsKey(3));
        
        // TreeSet - Sorted unique elements
        System.out.println("\n=== TreeSet ===");
        TreeSet<Integer> treeSet = new TreeSet<>();
        
        treeSet.add(50);
        treeSet.add(20);
        treeSet.add(40);
        treeSet.add(10);
        
        System.out.println("TreeSet: " + treeSet);
        System.out.println("First: " + treeSet.first());
        System.out.println("Last: " + treeSet.last());
        System.out.println("Lower than 30: " + treeSet.lower(30));
        System.out.println("Higher than 30: " + treeSet.higher(30));
        
        // Real-time Example 1: Student queue
        System.out.println("\n=== Example 1: Student Queue ===");
        
        LinkedList<String> studentQueue = new LinkedList<>();
        studentQueue.add("John");
        studentQueue.add("Jane");
        studentQueue.add("Mike");
        
        System.out.println("Serving: " + studentQueue.poll());
        System.out.println("Serving: " + studentQueue.poll());
        System.out.println("Remaining: " + studentQueue);
        
        // Real-time Example 2: Phonebook
        System.out.println("\n=== Example 2: Phonebook ===");
        
        HashMap<String, String> phonebook = new HashMap<>();
        phonebook.put("John", "9876543210");
        phonebook.put("Jane", "9876543211");
        phonebook.put("Mike", "9876543212");
        
        for (String name : phonebook.keySet()) {
            System.out.println(name + ": " + phonebook.get(name));
        }
        
        // Real-time Example 3: Inventory
        System.out.println("\n=== Example 3: Inventory ===");
        
        HashMap<String, Integer> inventory = new HashMap<>();
        inventory.put("Apples", 50);
        inventory.put("Oranges", 30);
        
        inventory.put("Apples", inventory.get("Apples") + 10);
        System.out.println("Apples: " + inventory.get("Apples"));
        
        // Real-time Example 4: Unique scores
        System.out.println("\n=== Example 4: Unique Scores ===");
        
        TreeSet<Integer> scores = new TreeSet<>();
        scores.add(95);
        scores.add(87);
        scores.add(92);
        scores.add(87);
        
        System.out.println("Sorted unique scores: " + scores);
        
        // Real-time Example 5: Navigation
        System.out.println("\n=== Example 5: Score Range ===");
        
        TreeSet<Integer> testScores = new TreeSet<>();
        testScores.add(60);
        testScores.add(70);
        testScores.add(80);
        testScores.add(90);
        
        System.out.println("Scores: " + testScores);
        System.out.println("SubSet 70-90: " + testScores.subSet(70, 90));
        
        // Real-time Example 6: Cache simulation
        System.out.println("\n=== Example 6: Cache ===");
        
        HashMap<String, String> cache = new HashMap<>();
        
        cache.put("user1", "John Data");
        cache.putIfAbsent("user2", "Jane Data");
        
        System.out.println("Cache size: " + cache.size());
        System.out.println("Get user1: " + cache.get("user1"));
    }
}
