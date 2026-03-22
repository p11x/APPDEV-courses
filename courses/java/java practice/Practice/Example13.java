/*
 * SUB TOPIC: Array Operations (Sorting and Searching)
 * 
 * DEFINITION:
 * Array operations include sorting (arranging elements in order) and searching (finding elements).
 * Java provides built-in methods for sorting and searching, along with manual implementations.
 * 
 * FUNCTIONALITIES:
 * 1. Bubble sort algorithm
 * 2. Binary search algorithm
 * 3. Linear search
 * 4. Using Arrays class methods
 * 5. Selection sort
 */

import java.util.Arrays;
import java.util.Scanner;

public class Example13 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        // Topic Explanation with Code: Array Sorting
        System.out.println("=== Array Sorting ===");
        int[] arr = {64, 34, 25, 12, 22, 11, 90}; // Sample unsorted array
        
        System.out.println("Before sorting:");
        for (int i = 0; i < arr.length; i++) { // Loop through array
            System.out.print(arr[i] + " "); // Print element
        }
        
        // Sort using Arrays.sort()
        Arrays.sort(arr); // Built-in sort method
        
        System.out.println("\nAfter sorting:");
        for (int i = 0; i < arr.length; i++) {
            System.out.print(arr[i] + " "); // Print sorted elements
        }
        
        // Real-time Example 1: Binary Search
        System.out.println("\n\n=== Binary Search ===");
        int[] sortedArr = {2, 5, 8, 12, 16, 23, 38, 56, 72, 91}; // Sorted array
        
        System.out.print("Array: ");
        for (int num : sortedArr) {
            System.out.print(num + " ");
        }
        
        System.out.print("\nEnter number to search: ");
        int key = scanner.nextInt(); // Number to search
        
        int result = Arrays.binarySearch(sortedArr, key); // Binary search
        
        if (result >= 0) { // If found
            System.out.println("Element " + key + " found at index " + result);
        } else {
            System.out.println("Element " + key + " not found");
        }
        
        // Real-time Example 2: Linear Search
        System.out.println("\n=== Linear Search ===");
        int[] linearArr = {10, 20, 30, 40, 50}; // Sample array
        
        System.out.print("Array: ");
        for (int num : linearArr) {
            System.out.print(num + " ");
        }
        
        System.out.print("\nEnter number to search: ");
        int linearKey = scanner.nextInt(); // Number to search
        int linearIndex = -1; // Initialize index
        
        for (int i = 0; i < linearArr.length; i++) { // Loop through array
            if (linearArr[i] == linearKey) { // If found
                linearIndex = i; // Store index
                break;
            }
        }
        
        if (linearIndex >= 0) {
            System.out.println("Element found at index " + linearIndex);
        } else {
            System.out.println("Element not found");
        }
        
        // Real-time Example 3: Bubble Sort
        System.out.println("\n=== Bubble Sort ===");
        int[] bubbleArr = {64, 34, 25, 12, 22}; // Unsorted array
        
        System.out.print("Before: ");
        for (int num : bubbleArr) {
            System.out.print(num + " ");
        }
        
        // Bubble sort algorithm
        int n = bubbleArr.length; // Array length
        for (int i = 0; i < n - 1; i++) { // Outer loop
            for (int j = 0; j < n - i - 1; j++) { // Inner loop
                if (bubbleArr[j] > bubbleArr[j + 1]) { // Compare adjacent
                    // Swap
                    int temp = bubbleArr[j]; // Store in temp
                    bubbleArr[j] = bubbleArr[j + 1]; // Swap
                    bubbleArr[j + 1] = temp; // Assign temp
                }
            }
        }
        
        System.out.print("\nAfter: ");
        for (int num : bubbleArr) {
            System.out.print(num + " "); // Print sorted array
        }
        
        // Real-time Example 4: Selection Sort
        System.out.println("\n\n=== Selection Sort ===");
        int[] selectionArr = {64, 25, 12, 22, 11}; // Unsorted
        
        System.out.print("Before: ");
        for (int num : selectionArr) {
            System.out.print(num + " ");
        }
        
        // Selection sort
        for (int i = 0; i < selectionArr.length - 1; i++) { // Outer loop
            int minIndex = i; // Assume minimum at i
            
            for (int j = i + 1; j < selectionArr.length; j++) { // Find minimum
                if (selectionArr[j] < selectionArr[minIndex]) { // If smaller
                    minIndex = j; // Update index
                }
            }
            
            // Swap
            int temp = selectionArr[minIndex];
            selectionArr[minIndex] = selectionArr[i];
            selectionArr[i] = temp;
        }
        
        System.out.print("\nAfter: ");
        for (int num : selectionArr) {
            System.out.print(num + " ");
        }
        
        // Real-time Example 5: Reverse Array
        System.out.println("\n\n=== Reverse Array ===");
        int[] reverseArr = {1, 2, 3, 4, 5}; // Sample array
        
        System.out.print("Original: ");
        for (int num : reverseArr) {
            System.out.print(num + " ");
        }
        
        int left = 0; // Left pointer
        int right = reverseArr.length - 1; // Right pointer
        
        while (left < right) { // While left < right
            // Swap
            int temp = reverseArr[left]; // Store left
            reverseArr[left] = reverseArr[right]; // Swap
            reverseArr[right] = temp; // Assign
            left++; // Increment left
            right--; // Decrement right
        }
        
        System.out.print("\nReversed: ");
        for (int num : reverseArr) {
            System.out.print(num + " ");
        }
        
        // Real-time Example 6: Find duplicate elements
        System.out.println("\n\n=== Find Duplicates ===");
        int[] duplicateArr = {1, 2, 3, 2, 4, 3, 5, 6, 5}; // Array with duplicates
        
        System.out.print("Array: ");
        for (int num : duplicateArr) {
            System.out.print(num + " ");
        }
        
        System.out.println("\nDuplicate elements:");
        for (int i = 0; i < duplicateArr.length; i++) { // Outer loop
            for (int j = i + 1; j < duplicateArr.length; j++) { // Inner loop
                if (duplicateArr[i] == duplicateArr[j]) { // If duplicate
                    System.out.print(duplicateArr[i] + " "); // Print
                    break;
                }
            }
        }
        
        scanner.close();
    }
}
