/*
 * SUB TOPIC: Nested For Loops (Pattern Printing)
 * 
 * DEFINITION:
 * Nested for loops consist of one or more for loops inside another for loop. They are commonly
 * used for pattern printing, matrix operations, and multi-dimensional data processing.
 * 
 * FUNCTIONALITIES:
 * 1. Pattern printing (triangles, pyramids, etc.)
 * 2. Matrix operations
 * 3. Multi-dimensional array traversal
 * 4. Complex iterations
 */

public class Example8 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: Nested Loop - Star Triangle
        System.out.println("=== Nested For Loop: Star Triangle ===");
        int n = 5; // Number of rows
        
        for (int i = 1; i <= n; i++) { // Outer loop for rows
            for (int j = 1; j <= i; j++) { // Inner loop for columns
                System.out.print("*"); // Print star
            }
            System.out.println(); // New line after each row
        }
        
        // Real-time Example 1: Number triangle
        System.out.println("\n=== Number Triangle ===");
        
        for (int i = 1; i <= 5; i++) { // Outer loop for rows
            for (int j = 1; j <= i; j++) { // Inner loop for numbers
                System.out.print(j + " "); // Print j value
            }
            System.out.println(); // New line
        }
        
        // Real-time Example 2: Inverted star triangle
        System.out.println("\n=== Inverted Star Triangle ===");
        
        for (int i = 5; i >= 1; i--) { // Outer loop - decreasing rows
            for (int j = 1; j <= i; j++) { // Inner loop - decreasing stars
                System.out.print("*");
            }
            System.out.println();
        }
        
        // Real-time Example 3: Pyramid pattern
        System.out.println("\n=== Pyramid Pattern ===");
        
        for (int i = 1; i <= 5; i++) { // Outer loop for rows
            // Print spaces
            for (int j = 5; j > i; j--) { // Spaces before stars
                System.out.print(" ");
            }
            // Print stars
            for (int k = 1; k <= (2 * i - 1); k++) { // Stars in row
                System.out.print("*");
            }
            System.out.println();
        }
        
        // Real-time Example 4: Square pattern
        System.out.println("\n=== Square Pattern ===");
        
        for (int i = 1; i <= 5; i++) { // Outer loop for rows
            for (int j = 1; j <= 5; j++) { // Inner loop for columns
                System.out.print("* ");
            }
            System.out.println();
        }
        
        // Real-time Example 5: Alphabet triangle
        System.out.println("\n=== Alphabet Triangle ===");
        
        for (int i = 1; i <= 5; i++) { // Outer loop
            char ch = 'A'; // Starting character
            for (int j = 1; j <= i; j++) { // Inner loop
                System.out.print(ch + " "); // Print character
                ch++; // Increment character
            }
            System.out.println();
        }
        
        // Real-time Example 6: Floyd's Triangle
        System.out.println("\n=== Floyd's Triangle ===");
        
        int num = 1; // Starting number
        for (int i = 1; i <= 5; i++) { // Outer loop for rows
            for (int j = 1; j <= i; j++) { // Inner loop for numbers
                System.out.print(num + " "); // Print number
                num++; // Increment number
            }
            System.out.println();
        }
    }
}
