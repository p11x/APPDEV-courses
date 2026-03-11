// HashSetDemo - Demonstrates HashSet in Java Collections Framework
// Important for storing unique elements

import java.util.*;

public class HashSetDemo {
    
    public static void main(String[] args) {
        System.out.println("=== HASHSET DEMO ===");
        
        HashSet<Integer> numbers = new HashSet<>();
        
        numbers.add(10);
        numbers.add(20);
        numbers.add(30);
        numbers.add(10);  // Duplicate - ignored
        
        System.out.println("Numbers: " + numbers);
        System.out.println("Size: " + numbers.size());
        System.out.println("Contains 20: " + numbers.contains(20));
        
        numbers.remove(20);
        System.out.println("After remove: " + numbers);
        
        System.out.println("\n=== KEY FEATURES ===");
        System.out.println("1. No duplicate elements");
        System.out.println("2. Fast lookup O(1)");
        System.out.println("3. Unordered collection");
    }
}
