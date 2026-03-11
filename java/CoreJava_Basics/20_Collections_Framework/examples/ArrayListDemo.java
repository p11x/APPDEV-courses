// ArrayListDemo - Demonstrates ArrayList in Java Collections Framework
// Important for Angular developers to understand dynamic lists

import java.util.*;

public class ArrayListDemo {
    
    public static void main(String[] args) {
        System.out.println("=== ARRAYLIST DEMO ===");
        
        // Create ArrayList
        ArrayList<String> fruits = new ArrayList<>();
        
        // Add elements
        fruits.add("Apple");
        fruits.add("Banana");
        fruits.add("Orange");
        fruits.add(1, "Mango");  // Insert at index 1
        
        System.out.println("Fruits: " + fruits);
        System.out.println("Size: " + fruits.size());
        System.out.println("First item: " + fruits.get(0));
        System.out.println("Contains 'Banana': " + fruits.contains("Banana"));
        
        // Modify
        fruits.set(0, "Green Apple");
        System.out.println("After set: " + fruits);
        
        // Remove
        fruits.remove("Banana");
        fruits.remove(0);
        System.out.println("After remove: " + fruits);
        
        // Iterate
        System.out.println("Using for-each:");
        for (String fruit : fruits) {
            System.out.println("  - " + fruit);
        }
        
        System.out.println("\n=== KEY OPERATIONS ===");
        System.out.println("add() - Add element");
        System.out.println("get() - Get element by index");
        System.out.println("set() - Update element");
        System.out.println("remove() - Remove element");
        System.out.println("size() - Get list size");
    }
}
