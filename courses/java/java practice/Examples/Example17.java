// Example17: Array of Strings - Beginner Tutorial
// This shows different ways to work with String arrays

// Step 1: Simple String Array - Declaration Methods

public class Example17 {
    public static void main(String[] args) {
        
        // ===== METHOD 1: Declare array first, then assign values =====
        System.out.println("=== Method 1: Assign values later ===");
        
        String[] names = new String[3];
        names[0] = "Alice";
        names[1] = "Bob";
        names[2] = "Charlie";
        
        // Print using for loop
        System.out.println("Names:");
        for (int i = 0; i < names.length; i++) {
            System.out.println("  " + names[i]);
        }
        
        // ===== METHOD 2: Initialize with values directly =====
        System.out.println("\n=== Method 2: Direct initialization ===");
        
        String[] fruits = {"Apple", "Banana", "Orange", "Mango"};
        
        // Print using for-each loop
        System.out.println("Fruits:");
        for (String fruit : fruits) {
            System.out.println("  " + fruit);
        }
        
        // ===== COMMON STRING OPERATIONS IN ARRAYS =====
        System.out.println("\n=== Common String Operations ===");
        
        String[] words = {"hello", "WORLD", "Java", "Programming"};
        
        // 1. toUpperCase() - convert to uppercase
        System.out.println("Original: " + words[0] + " -> Uppercase: " + words[0].toUpperCase());
        
        // 2. toLowerCase() - convert to lowercase
        System.out.println("Original: " + words[1] + " -> Lowercase: " + words[1].toLowerCase());
        
        // 3. length() - get string length
        System.out.println("Length of '" + words[2] + "': " + words[2].length());
        
        // 4. charAt() - get character at position
        System.out.println("First character of '" + words[3] + "': " + words[3].charAt(0));
        
        // 5. contains() - check if contains substring
        System.out.println("'" + words[3] + "' contains 'gram': " + words[3].contains("gram"));
        
        // ===== PRACTICAL EXAMPLES =====
        System.out.println("\n=== Practical Example 1: Student Names ===");
        
        String[] students = {"John", "Emma", "Liam", "Olivia", "Noah"};
        
        // Find a specific name
        String searchName = "Emma";
        boolean found = false;
        
        for (String student : students) {
            if (student.equals(searchName)) {
                found = true;
                break;
            }
        }
        
        System.out.println("Is '" + searchName + "' in the list? " + found);
        
        // Count names starting with letter
        char letter = 'O';
        int count = 0;
        for (String student : students) {
            if (student.charAt(0) == letter) {
                count++;
            }
        }
        System.out.println("Names starting with '" + letter + "': " + count);
        
        // ===== SORTING STRINGS =====
        System.out.println("\n=== Sorting Strings ===");
        
        String[] cities = {"New York", "London", "Paris", "Tokyo", "Sydney"};
        
        System.out.println("Before sorting:");
        for (String city : cities) {
            System.out.print(city + " ");
        }
        
        // Sort the array (alphabetical order)
        java.util.Arrays.sort(cities);
        
        System.out.println("\nAfter sorting:");
        for (String city : cities) {
            System.out.print(city + " ");
        }
        
        // ===== SEARCHING IN SORTED ARRAY =====
        System.out.println("\n\n=== Binary Search (in sorted array) ===");
        
        int position = java.util.Arrays.binarySearch(cities, "Paris");
        System.out.println("'Paris' found at index: " + position);
        
        // ===== ARRAY OF STRINGS - USER INPUT =====
        System.out.println("\n=== String Array from Console ===");
        
        // Example: Creating array from parts
        String sentence = "Java is a programming language";
        String[] parts = sentence.split(" ");  // Split by space
        
        System.out.println("Words in sentence:");
        for (String part : parts) {
            System.out.println("  - " + part);
        }
        
        // Join array back to string
        String joined = String.join(", ", parts);
        System.out.println("Joined back: " + joined);
        
        // ===== LOOPING TECHNIQUES =====
        System.out.println("\n=== Different Looping Methods ===");
        
        String[] colors = {"Red", "Green", "Blue"};
        
        // Method 1: Regular for loop
        System.out.print("For loop: ");
        for (int i = 0; i < colors.length; i++) {
            System.out.print(colors[i] + " ");
        }
        
        // Method 2: For-each loop
        System.out.print("\nFor-each: ");
        for (String color : colors) {
            System.out.print(color + " ");
        }
        
        // Method 3: Using Arrays.toString()
        System.out.println("\nUsing Arrays.toString(): " + java.util.Arrays.toString(colors));
        
        // ===== STRING COMPARISON IN ARRAYS =====
        System.out.println("\n=== Comparing Strings ===");
        
        String[] loginNames = {"admin", "user123", "guest"};
        
        // Using equals() - compares content
        System.out.println("'admin'.equals(loginNames[0]): " + "admin".equals(loginNames[0]));
        
        // Using == - compares references (be careful!)
        String a = "hello";
        String b = "hello";
        String c = new String("hello");
        
        System.out.println("a == b: " + (a == b) + " (same string pool)");
        System.out.println("a == c: " + (a == c) + " (different objects)");
        System.out.println("a.equals(c): " + a.equals(c) + " (compares content)");
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. DECLARING STRING ARRAYS:
 *    String[] array = new String[size];
 *    String[] array = {"value1", "value2", "value3"};
 * 
 * 2. COMMON STRING METHODS:
 *    - length()       -> returns number of characters
 *    - toUpperCase()  -> converts to uppercase
 *    - toLowerCase()  -> converts to lowercase
 *    - charAt(index)  -> returns character at position
 *    - equals(other)  -> compares two strings (content)
 *    - contains(sub) -> checks if contains substring
 *    - split(delimiter) -> splits string into array
 * 
 * 3. IMPORTANT TIPS:
 *    - Always use equals() for string comparison, not ==
 *    - String comparison with == compares references, not content
 *    - Use Arrays.sort() to sort string arrays
 *    - Use Arrays.toString() for easy printing
 *    - Use binarySearch() only on sorted arrays
 * 
 * 4. WHY STRING ARRAYS?
 *    - Store lists of text data (names, cities, etc.)
 *    - Easy to sort and search
 *    - Many built-in methods available
 */
