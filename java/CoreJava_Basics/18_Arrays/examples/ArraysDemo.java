// ArraysDemo - Demonstrates Java Arrays
// Arrays are fundamental for storing collections of data

public class ArraysDemo {
    
    public static void main(String[] args) {
        System.out.println("=== ARRAYS DEMO ===\n");
        
        // Declare and initialize arrays
        int[] numbers = {1, 2, 3, 4, 5};
        String[] names = new String[3];
        names[0] = "Alice";
        names[1] = "Bob";
        names[2] = "Charlie";
        
        // Basic operations
        System.out.println("--- Basic Operations ---");
        System.out.println("First number: " + numbers[0]);
        System.out.println("Array length: " + numbers.length);
        
        // Loop through array
        System.out.println("\n--- For Loop ---");
        for (int i = 0; i < numbers.length; i++) {
            System.out.println("numbers[" + i + "] = " + numbers[i]);
        }
        
        // Enhanced for loop
        System.out.println("\n--- Enhanced For Loop ---");
        for (String name : names) {
            System.out.println("Name: " + name);
        }
        
        // Multidimensional array
        System.out.println("\n--- Multidimensional Array ---");
        int[][] matrix = {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };
        
        for (int row = 0; row < matrix.length; row++) {
            for (int col = 0; col < matrix[row].length; col++) {
                System.out.print(matrix[row][col] + " ");
            }
            System.out.println();
        }
        
        // Array utility methods
        System.out.println("\n--- Arrays Utility Methods ---");
        int[] toSort = {5, 2, 8, 1, 9};
        System.out.println("Before sort: ");
        printArray(toSort);
        
        java.util.Arrays.sort(toSort);
        System.out.println("After sort: ");
        printArray(toSort);
        
        // Binary search (array must be sorted)
        int index = java.util.Arrays.binarySearch(toSort, 5);
        System.out.println("Index of 5: " + index);
        
        // Fill array
        int[] filled = new int[5];
        java.util.Arrays.fill(filled, 10);
        System.out.println("Filled array: ");
        printArray(filled);
        
        // Copy array
        int[] copied = java.util.Arrays.copyOf(numbers, numbers.length * 2);
        System.out.println("Copied array length: " + copied.length);
    }
    
    static void printArray(int[] arr) {
        for (int num : arr) {
            System.out.print(num + " ");
        }
        System.out.println();
    }
}
