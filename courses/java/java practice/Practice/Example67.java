/*
 * SUB TOPIC: Java Arrays - Deep Dive
 * 
 * DEFINITION:
 * Arrays are fixed-size data structures that store elements of the same type. They can be single-dimensional 
 * or multi-dimensional.
 * 
 * FUNCTIONALITIES:
 * 1. Array declaration and initialization
 * 2. Array traversal
 * 3. Array searching
 * 4. Array sorting
 * 5. 2D arrays
 * 6. Jagged arrays
 */

import java.util.*;

public class Example67 {
    public static void main(String[] args) {
        
        // Declaration and initialization
        System.out.println("=== Array Basics ===");
        
        int[] arr = {1, 2, 3, 4, 5};
        System.out.println("Array: " + Arrays.toString(arr));
        
        // Traversal
        System.out.println("\n=== Traversal ===");
        for (int i = 0; i < arr.length; i++) {
            System.out.print(arr[i] + " ");
        }
        System.out.println();
        
        // Enhanced for
        System.out.println("\n=== Enhanced For ===");
        for (int num : arr) {
            System.out.print(num + " ");
        }
        System.out.println();
        
        // Sorting
        System.out.println("\n=== Sorting ===");
        int[] unsorted = {5, 2, 8, 1, 9};
        Arrays.sort(unsorted);
        System.out.println("Sorted: " + Arrays.toString(unsorted));
        
        // Binary Search
        System.out.println("\n=== Binary Search ===");
        int index = Arrays.binarySearch(unsorted, 5);
        System.out.println("Index of 5: " + index);
        
        // 2D Array
        System.out.println("\n=== 2D Array ===");
        int[][] matrix = {{1,2,3}, {4,5,6}};
        System.out.println("Matrix: " + Arrays.deepToString(matrix));
        
        // Real-time Example 1: Average
        System.out.println("\n=== Example 1: Average ===");
        
        int[] marks = {85, 90, 78, 92, 88};
        int sum = 0;
        for (int m : marks) sum += m;
        double avg = (double) sum / marks.length;
        System.out.println("Average: " + avg);
        
        // Real-time Example 2: Max
        System.out.println("\n=== Example 2: Max ===");
        
        int max = marks[0];
        for (int m : marks) {
            if (m > max) max = m;
        }
        System.out.println("Max: " + max);
        
        // Real-time Example 3: Copy
        System.out.println("\n=== Example 3: Copy ===");
        
        int[] original = {1, 2, 3};
        int[] copy = Arrays.copyOf(original, 5);
        System.out.println("Copy: " + Arrays.toString(copy));
        
        // Real-time Example 4: Fill
        System.out.println("\n=== Example 4: Fill ===");
        
        int[] filled = new int[5];
        Arrays.fill(filled, 10);
        System.out.println("Filled: " + Arrays.toString(filled));
        
        // Real-time Example 5: Jagged array
        System.out.println("\n=== Example 5: Jagged ===");
        
        int[][] jagged = new int[3][];
        jagged[0] = new int[2];
        jagged[1] = new int[3];
        jagged[2] = new int[1];
        
        System.out.println(Arrays.deepToString(jagged));
        
        // Real-time Example 6: Reverse
        System.out.println("\n=== Example 6: Reverse ===");
        
        Collections.reverse(Arrays.asList(arr));
        System.out.println("Reversed: " + Arrays.toString(arr));
    }
}
