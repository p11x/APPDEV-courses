/*
 * SUB TOPIC: Advanced Array Operations
 * 
 * DEFINITION:
 * Arrays in Java are fixed-size data structures that hold elements of the same type. Advanced operations
 * include copying, filling, sorting, searching, and working with multidimensional arrays.
 * 
 * FUNCTIONALITIES:
 * 1. Arrays.fill - Fill array with value
 * 2. Arrays.copyOf - Copy array with new size
 * 3. Arrays.sort - Sort array
 * 4. Arrays.binarySearch - Search in sorted array
 * 5. Arrays.equals - Compare arrays
 * 6. 2D arrays
 */

import java.util.*;

public class Example59 {
    public static void main(String[] args) {
        
        // Arrays.fill
        System.out.println("=== Arrays.fill ===");
        int[] arr = new int[5];
        Arrays.fill(arr, 10);
        System.out.println("Filled: " + Arrays.toString(arr));
        
        // Arrays.copyOf
        System.out.println("\n=== Arrays.copyOf ===");
        int[] original = {1, 2, 3};
        int[] copy = Arrays.copyOf(original, 5);
        System.out.println("Original: " + Arrays.toString(original));
        System.out.println("Copy: " + Arrays.toString(copy));
        
        // Arrays.sort
        System.out.println("\n=== Arrays.sort ===");
        int[] unsorted = {5, 2, 8, 1, 9};
        Arrays.sort(unsorted);
        System.out.println("Sorted: " + Arrays.toString(unsorted));
        
        // Arrays.binarySearch
        System.out.println("\n=== Arrays.binarySearch ===");
        int[] sorted = {1, 2, 3, 4, 5};
        int index = Arrays.binarySearch(sorted, 3);
        System.out.println("Index of 3: " + index);
        
        // Arrays.equals
        System.out.println("\n=== Arrays.equals ===");
        int[] a1 = {1, 2, 3};
        int[] a2 = {1, 2, 3};
        System.out.println("Equal: " + Arrays.equals(a1, a2));
        
        // 2D Arrays
        System.out.println("\n=== 2D Arrays ===");
        int[][] matrix = {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };
        
        for (int[] row : matrix) {
            System.out.println(Arrays.toString(row));
        }
        
        // Real-time Example 1: Student scores
        System.out.println("\n=== Example 1: Scores ===");
        
        double[] scores = {85.5, 92.0, 78.5, 90.0, 88.5};
        Arrays.sort(scores);
        System.out.println("Sorted: " + Arrays.toString(scores));
        
        // Real-time Example 2: Matrix addition
        System.out.println("\n=== Example 2: Matrix ===");
        
        int[][] m1 = {{1, 2}, {3, 4}};
        int[][] m2 = {{5, 6}, {7, 8}};
        int[][] sum = new int[2][2];
        
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 2; j++) {
                sum[i][j] = m1[i][j] + m2[i][j];
            }
        }
        
        System.out.println("Result:");
        for (int[] row : sum) {
            System.out.println(Arrays.toString(row));
        }
        
        // Real-time Example 3: Prefix sum
        System.out.println("\n=== Example 3: Prefix Sum ===");
        
        int[] nums = {1, 2, 3, 4, 5};
        int[] prefix = new int[nums.length];
        prefix[0] = nums[0];
        
        for (int i = 1; i < nums.length; i++) {
            prefix[i] = prefix[i - 1] + nums[i];
        }
        
        System.out.println("Original: " + Arrays.toString(nums));
        System.out.println("Prefix: " + Arrays.toString(prefix));
        
        // Real-time Example 4: Frequency
        System.out.println("\n=== Example 4: Frequency ===");
        
        int[] values = {1, 2, 2, 3, 3, 3, 4, 4, 4, 4};
        Map<Integer, Integer> freq = new HashMap<>();
        
        for (int v : values) {
            freq.put(v, freq.getOrDefault(v, 0) + 1);
        }
        
        System.out.println("Frequency: " + freq);
        
        // Real-time Example 5: Rotate array
        System.out.println("\n=== Example 5: Rotate ===");
        
        int[] toRotate = {1, 2, 3, 4, 5};
        int k = 2;
        int n = toRotate.length;
        int[] rotated = new int[n];
        
        for (int i = 0; i < n; i++) {
            rotated[(i + k) % n] = toRotate[i];
        }
        
        System.out.println("Original: " + Arrays.toString(toRotate));
        System.out.println("Rotated: " + Arrays.toString(rotated));
        
        // Real-time Example 6: Transpose
        System.out.println("\n=== Example 6: Transpose ===");
        
        int[][] mat = {{1, 2}, {3, 4}};
        int[][] trans = new int[2][2];
        
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 2; j++) {
                trans[i][j] = mat[j][i];
            }
        }
        
        System.out.println("Original:");
        for (int[] row : mat) System.out.println(Arrays.toString(row));
        System.out.println("Transpose:");
        for (int[] row : trans) System.out.println(Arrays.toString(row));
    }
}
