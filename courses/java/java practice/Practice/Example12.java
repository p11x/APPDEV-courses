/*
 * SUB TOPIC: Arrays (One Dimensional and Two Dimensional)
 * 
 * DEFINITION:
 * An array is a collection of elements of the same type stored in contiguous memory locations.
 * One-dimensional arrays store elements in a linear fashion, while two-dimensional arrays
 * store elements in a matrix/table format with rows and columns.
 * 
 * FUNCTIONALITIES:
 * 1. Declaration and initialization of arrays
 * 2. Accessing array elements using index
 * 3. Iterating through arrays using loops
 * 4. Two-dimensional array operations
 * 5. Array length calculation
 */

public class Example12 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: One Dimensional Array
        System.out.println("=== One Dimensional Array ===");
        int arr[] = new int[] {10, 20, 30, 40, 50}; // Declare and initialize array
        
        System.out.println("Array elements: ");
        for (int i = 0; i < arr.length; i++) { // Loop through array
            System.out.print(arr[i] + "\t"); // Print each element
        }
        
        System.out.println();
        
        // Real-time Example 1: Two Dimensional Array
        System.out.println("\n=== Two Dimensional Array ===");
        int arr2[][] = new int[][] {{10, 20, 30}, {40, 50, 60}}; // 2D array
        
        System.out.println("Matrix elements: ");
        for (int i = 0; i < arr2.length; i++) { // Loop through rows
            for (int j = 0; j < arr2[i].length; j++) { // Loop through columns
                System.out.print(arr2[i][j] + "\t"); // Print element
            }
            System.out.println(); // New line after each row
        }
        
        // Real-time Example 2: Sum of array elements
        System.out.println("\n=== Sum of Array Elements ===");
        int[] numbers = {5, 10, 15, 20, 25}; // Sample array
        int arraySum = 0; // Initialize sum
        
        for (int i = 0; i < numbers.length; i++) { // Loop through array
            arraySum += numbers[i]; // Add each element
        }
        
        System.out.print("Array: ");
        for (int num : numbers) { // For-each loop
            System.out.print(num + " ");
        }
        System.out.println("\nSum: " + arraySum);
        
        // Real-time Example 3: Find maximum in array
        System.out.println("\n=== Find Maximum Element ===");
        int[] values = {25, 50, 15, 80, 35}; // Sample array
        int max = values[0]; // Assume first element is maximum
        
        for (int i = 1; i < values.length; i++) { // Start from second element
            if (values[i] > max) { // Compare with max
                max = values[i]; // Update max
            }
        }
        
        System.out.print("Array: ");
        for (int val : values) {
            System.out.print(val + " ");
        }
        System.out.println("\nMaximum: " + max);
        
        // Real-time Example 4: Find minimum in array
        System.out.println("\n=== Find Minimum Element ===");
        int min = values[0]; // Assume first element is minimum
        
        for (int i = 1; i < values.length; i++) { // Start from second
            if (values[i] < min) { // Compare with min
                min = values[i]; // Update min
            }
        }
        
        System.out.println("Minimum: " + min);
        
        // Real-time Example 5: Matrix addition
        System.out.println("\n=== Matrix Addition ===");
        int[][] matrix1 = {{1, 2, 3}, {4, 5, 6}}; // First matrix
        int[][] matrix2 = {{7, 8, 9}, {10, 11, 12}}; // Second matrix
        int[][] result = new int[2][3]; // Result matrix
        
        System.out.println("Matrix 1:");
        for (int i = 0; i < 2; i++) { // Loop through rows
            for (int j = 0; j < 3; j++) { // Loop through columns
                System.out.print(matrix1[i][j] + " "); // Print element
            }
            System.out.println();
        }
        
        System.out.println("Matrix 2:");
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 3; j++) {
                System.out.print(matrix2[i][j] + " ");
            }
            System.out.println();
        }
        
        // Add matrices
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 3; j++) {
                result[i][j] = matrix1[i][j] + matrix2[i][j]; // Add elements
            }
        }
        
        System.out.println("Result (Matrix 1 + Matrix 2):");
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 3; j++) {
                System.out.print(result[i][j] + " ");
            }
            System.out.println();
        }
        
        // Real-time Example 6: Transpose of matrix
        System.out.println("\n=== Transpose of Matrix ===");
        int[][] original = {{1, 2, 3}, {4, 5, 6}}; // Original matrix
        int[][] transpose = new int[3][2]; // Transpose matrix
        
        System.out.println("Original Matrix:");
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 3; j++) {
                System.out.print(original[i][j] + " ");
            }
            System.out.println();
        }
        
        // Transpose
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 3; j++) {
                transpose[j][i] = original[i][j]; // Swap rows and columns
            }
        }
        
        System.out.println("Transpose Matrix:");
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 2; j++) {
                System.out.print(transpose[i][j] + " ");
            }
            System.out.println();
        }
    }
}
