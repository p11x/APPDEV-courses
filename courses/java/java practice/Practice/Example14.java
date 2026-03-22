/*
 * SUB TOPIC: Advanced Array Types (Jagged, Objects, Interfaces)
 * 
 * DEFINITION:
 * Java supports various types of arrays including jagged arrays (arrays with different row lengths),
 * arrays of objects, arrays of interfaces, and primitive type arrays. Each has specific use cases.
 * 
 * FUNCTIONALITIES:
 * 1. Jagged arrays - arrays with variable row lengths
 * 2. Array of objects - storing object references
 * 3. Array of interfaces - storing interface implementations
 * 4. Different primitive type arrays
 * 5. Array initialization and manipulation
 */

public class Example14 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: Jagged Array
        System.out.println("=== Jagged Array ===");
        int[][] jagged = new int[][] {{10, 20, 30, 40, 50}, {10, 20, 30}, {5, 10}}; // Different row lengths
        
        for (int i = 0; i < jagged.length; i++) { // Loop through rows
            for (int j = 0; j < jagged[i].length; j++) { // Loop through columns
                System.out.print(jagged[i][j] + "\t"); // Print element
            }
            System.out.println(); // New line
        }
        
        // Real-time Example 1: Array of Objects (Strings)
        System.out.println("\n=== Array of Objects (Strings) ===");
        String[] names = new String[] {"Alice", "Bob", "Charlie", "David", "Emma"}; // String array
        
        for (String name : names) { // For-each loop
            System.out.println(name); // Print each name
        }
        
        // Real-time Example 2: Array of Integers
        System.out.println("\n=== Array of Integers ===");
        int[] numbers = new int[5]; // Declare array of size 5
        numbers[0] = 10; // Assign values
        numbers[1] = 20;
        numbers[2] = 30;
        numbers[3] = 40;
        numbers[4] = 50;
        
        for (int i = 0; i < numbers.length; i++) { // Loop
            System.out.println("numbers[" + i + "] = " + numbers[i]);
        }
        
        // Real-time Example 3: Array of Doubles
        System.out.println("\n=== Array of Doubles ===");
        double[] prices = {100.50, 200.75, 150.00, 300.25, 50.00}; // Initialize
        
        System.out.print("Prices: ");
        for (double price : prices) { // For-each
            System.out.print(price + " ");
        }
        
        // Real-time Example 4: Array of Characters
        System.out.println("\n\n=== Array of Characters ===");
        char[] vowels = {'A', 'E', 'I', 'O', 'U'}; // Character array
        
        System.out.print("Vowels: ");
        for (char v : vowels) {
            System.out.print(v + " ");
        }
        
        // Real-time Example 5: Array of Booleans
        System.out.println("\n\n=== Array of Booleans ===");
        boolean[] flags = {true, false, true, true, false}; // Boolean array
        
        System.out.print("Flags: ");
        for (boolean flag : flags) {
            System.out.print(flag + " ");
        }
        
        // Real-time Example 6: Array of Longs
        System.out.println("\n\n=== Array of Longs ===");
        long[] phoneNumbers = {9876543210L, 9876543211L, 9876543212L}; // Long array
        
        System.out.print("Phone Numbers: ");
        for (long phone : phoneNumbers) {
            System.out.print(phone + " ");
        }
        
        // Real-time Example 7: Array of Floats
        System.out.println("\n\n=== Array of Floats ===");
        float[] temperatures = {25.5f, 26.0f, 24.5f, 27.5f}; // Float array
        
        System.out.print("Temperatures: ");
        for (float temp : temperatures) {
            System.out.print(temp + " ");
        }
        
        // Real-time Example 8: Array of Bytes
        System.out.println("\n\n=== Array of Bytes ===");
        byte[] data = {10, 20, 30, 40, 50}; // Byte array (small numbers)
        
        System.out.print("Data: ");
        for (byte b : data) {
            System.out.print(b + " ");
        }
        
        // Real-time Example 9: Array of Shorts
        System.out.println("\n\n=== Array of Shorts ===");
        short[] counts = {100, 200, 300, 400, 500}; // Short array
        
        System.out.print("Counts: ");
        for (short c : counts) {
            System.out.print(c + " ");
        }
        
        // Real-time Example 10: Multidimensional array
        System.out.println("\n\n=== 3D Array ===");
        int[][][] threeD = {{{1, 2}, {3, 4}}, {{5, 6}, {7, 8}}}; // 3D array
        
        for (int i = 0; i < threeD.length; i++) { // First dimension
            for (int j = 0; j < threeD[i].length; j++) { // Second dimension
                for (int k = 0; k < threeD[i][j].length; k++) { // Third dimension
                    System.out.print(threeD[i][j][k] + " "); // Print element
                }
                System.out.println();
            }
            System.out.println();
        }
    }
}
