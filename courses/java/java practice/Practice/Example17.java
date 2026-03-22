/*
 * SUB TOPIC: String Array Operations
 * 
 * DEFINITION:
 * A String array is an array that holds String objects. Strings in Java are immutable and provide
 * various methods for manipulation. String arrays are commonly used for storing text data.
 * 
 * FUNCTIONALITIES:
 * 1. String array declaration and initialization
 * 2. String methods (toUpperCase, toLowerCase, length, etc.)
 * 3. Sorting string arrays
 * 4. Searching in string arrays
 * 5. String manipulation (split, join)
 */

import java.util.Arrays;

public class Example17 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: String Array Declaration
        System.out.println("=== String Array ===");
        
        String[] names = new String[3]; // Declare array of size 3
        names[0] = "Alice"; // Assign values
        names[1] = "Bob";
        names[2] = "Charlie";
        
        for (int i = 0; i < names.length; i++) {
            System.out.println("names[" + i + "] = " + names[i]);
        }
        
        // Real-time Example 1: Direct initialization
        System.out.println("\n=== Direct Initialization ===");
        
        String[] fruits = {"Apple", "Banana", "Orange", "Mango"};
        
        for (String fruit : fruits) {
            System.out.println(fruit);
        }
        
        // Real-time Example 2: String methods
        System.out.println("\n=== String Methods ===");
        
        String[] words = {"hello", "WORLD", "Java"};
        
        System.out.println("Original: " + words[0] + " -> Uppercase: " + words[0].toUpperCase());
        System.out.println("Original: " + words[1] + " -> Lowercase: " + words[1].toLowerCase());
        System.out.println("Length of '" + words[2] + "': " + words[2].length());
        System.out.println("First char of '" + words[2] + "': " + words[2].charAt(0));
        
        // Real-time Example 3: Sort strings
        System.out.println("\n=== Sort Strings ===");
        
        String[] cities = {"New York", "London", "Paris", "Tokyo", "Sydney"};
        
        System.out.print("Before: ");
        for (String city : cities) {
            System.out.print(city + " ");
        }
        
        Arrays.sort(cities); // Sort alphabetically
        
        System.out.print("\nAfter: ");
        for (String city : cities) {
            System.out.print(city + " ");
        }
        
        // Real-time Example 4: Search in array
        System.out.println("\n\n=== Search in Array ===");
        
        int pos = Arrays.binarySearch(cities, "Paris");
        System.out.println("'Paris' found at index: " + pos);
        
        // Real-time Example 5: Split and Join
        System.out.println("\n=== Split and Join ===");
        
        String sentence = "Java is a programming language";
        String[] parts = sentence.split(" "); // Split by space
        
        System.out.println("Words:");
        for (String part : parts) {
            System.out.println("  - " + part);
        }
        
        String joined = String.join(", ", parts);
        System.out.println("Joined: " + joined);
        
        // Real-time Example 6: String comparison
        System.out.println("\n=== String Comparison ===");
        
        String[] loginNames = {"admin", "user123", "guest"};
        
        System.out.println("'admin'.equals(loginNames[0]): " + "admin".equals(loginNames[0]));
        
        String a = "hello";
        String b = "hello";
        String c = new String("hello");
        
        System.out.println("a == b: " + (a == b));
        System.out.println("a == c: " + (a == c));
        System.out.println("a.equals(c): " + a.equals(c));
    }
}
