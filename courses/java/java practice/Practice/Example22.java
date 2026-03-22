// Example22: Array of Integers - Beginner Tutorial
// This shows different ways to work with int arrays

public class Example22 {
    public static void main(String[] args) {
        
        // ===== METHOD 1: Create int array with size =====
        System.out.println("=== Method 1: Create with size ===");
        
        int[] numbers = new int[5];
        
        // Default value is 0
        numbers[0] = 100;
        numbers[1] = 200;
        numbers[2] = 300;
        numbers[3] = 400;
        numbers[4] = 500;
        
        System.out.println("Integer values:");
        for (int i = 0; i < numbers.length; i++) {
            System.out.println("  Index " + i + ": " + numbers[i]);
        }
        
        // ===== METHOD 2: Direct initialization =====
        System.out.println("\n=== Method 2: Direct initialization ===");
        
        int[] digits = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        
        System.out.print("Digits: ");
        for (int d : digits) {
            System.out.print(d + " ");
        }
        System.out.println();
        
        // ===== METHOD 3: Array with new keyword =====
        System.out.println("\n=== Method 3: Array with new keyword ===");
        
        int[] values = new int[]{10, 20, 30, 40, 50};
        
        System.out.print("Values: ");
        for (int v : values) {
            System.out.print(v + " ");
        }
        System.out.println();
        
        // ===== INT RANGE =====
        System.out.println("\n=== Integer Range ===");
        
        // Int ranges from -2,147,483,648 to 2,147,483,647
        int minValue = Integer.MIN_VALUE;
        int maxValue = Integer.MAX_VALUE;
        
        System.out.println("Minimum int value: " + minValue);
        System.out.println("Maximum int value: " + maxValue);
        
        // ===== PRACTICAL: Sum and Average =====
        System.out.println("\n=== Practical: Sum and Average ===");
        
        int[] prices = {100, 250, 75, 300, 50};
        
        int sum = 0;
        for (int price : prices) {
            sum += price;
        }
        
        double average = (double) sum / prices.length;
        
        System.out.println("Prices: $100, $250, $75, $300, $50");
        System.out.println("Total: $" + sum);
        System.out.println("Average: $" + average);
        
        // ===== PRACTICAL: Find Min and Max =====
        System.out.println("\n=== Practical: Find Min and Max ===");
        
        int[] temperatures = {22, 25, 19, 30, 24, 18, 15, 20};
        
        int min = temperatures[0];
        int max = temperatures[0];
        
        for (int i = 1; i < temperatures.length; i++) {
            if (temperatures[i] < min) {
                min = temperatures[i];
            }
            if (temperatures[i] > max) {
                max = temperatures[i];
            }
        }
        
        System.out.println("Temperatures: 22, 25, 19, 30, 24, 18, 15, 20");
        System.out.println("Minimum: " + min + "°C");
        System.out.println("Maximum: " + max + "°C");
        
        // ===== PRACTICAL: Count Positive and Negative =====
        System.out.println("\n=== Practical: Count Positive and Negative ===");
        
        int[] mixed = {5, -3, 10, -7, 0, 8, -2, 12, -1};
        
        int positiveCount = 0;
        int negativeCount = 0;
        int zeroCount = 0;
        
        for (int num : mixed) {
            if (num > 0) {
                positiveCount++;
            } else if (num < 0) {
                negativeCount++;
            } else {
                zeroCount++;
            }
        }
        
        System.out.println("Numbers: 5, -3, 10, -7, 0, 8, -2, 12, -1");
        System.out.println("Positive: " + positiveCount);
        System.out.println("Negative: " + negativeCount);
        System.out.println("Zero: " + zeroCount);
        
        // ===== PRACTICAL: Array Search =====
        System.out.println("\n=== Practical: Search in Array ===");
        
        int[] ages = {25, 30, 22, 35, 28, 45, 19, 40};
        
        int searchAge = 35;
        boolean found = false;
        
        for (int i = 0; i < ages.length; i++) {
            if (ages[i] == searchAge) {
                found = true;
                System.out.println("Found " + searchAge + " at index " + i);
                break;
            }
        }
        
        if (!found) {
            System.out.println(searchAge + " not found in array");
        }
        
        // ===== SORTING INTEGERS =====
        System.out.println("\n=== Sorting Integers ===");
        
        int[] unsorted = {64, 25, 12, 22, 11, 90, 45};
        
        System.out.print("Before sorting: ");
        for (int num : unsorted) {
            System.out.print(num + " ");
        }
        System.out.println();
        
        // Sort using Arrays.sort()
        java.util.Arrays.sort(unsorted);
        
        System.out.print("After sorting: ");
        for (int num : unsorted) {
            System.out.print(num + " ");
        }
        System.out.println();
        
        // ===== BINARY SEARCH (on sorted array) =====
        System.out.println("\n=== Binary Search ===");
        
        int[] sorted = {10, 20, 30, 40, 50, 60, 70};
        
        int searchValue = 40;
        int result = java.util.Arrays.binarySearch(sorted, searchValue);
        
        System.out.println("Array: 10, 20, 30, 40, 50, 60, 70");
        System.out.println("Searching for: " + searchValue);
        System.out.println("Found at index: " + result);
        
        // ===== PRACTICAL: Fibonacci Sequence =====
        System.out.println("\n=== Practical: Fibonacci Sequence ===");
        
        int[] fibonacci = new int[10];
        
        // First two numbers
        fibonacci[0] = 0;
        fibonacci[1] = 1;
        
        // Generate rest
        for (int i = 2; i < fibonacci.length; i++) {
            fibonacci[i] = fibonacci[i - 1] + fibonacci[i - 2];
        }
        
        System.out.print("First 10 Fibonacci numbers: ");
        for (int f : fibonacci) {
            System.out.print(f + " ");
        }
        System.out.println();
        
        // ===== PRACTICAL: Even Numbers =====
        System.out.println("\n=== Practical: Even Numbers ===");
        
        int[] evens = new int[10];
        
        for (int i = 0; i < evens.length; i++) {
            evens[i] = (i + 1) * 2;
        }
        
        System.out.print("First 10 even numbers: ");
        for (int e : evens) {
            System.out.print(e + " ");
        }
        System.out.println();
        
        // ===== PRACTICAL: Multiplication Table =====
        System.out.println("\n=== Practical: Multiplication Table ===");
        
        int[][] multiplicationTable = new int[5][5];
        
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 5; j++) {
                multiplicationTable[i][j] = (i + 1) * (j + 1);
            }
        }
        
        System.out.println("5x5 Multiplication Table:");
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 5; j++) {
                System.out.print(multiplicationTable[i][j] + "\t");
            }
            System.out.println();
        }
        
        // ===== CONVERT INT TO OTHER TYPES =====
        System.out.println("\n=== Convert Int to Other Types ===");
        
        int num = 500;
        
        // Int to String
        String stringValue = Integer.toString(num);
        System.out.println("Int to String: " + num + " -> \"" + stringValue + "\"");
        
        // Int to double
        double doubleValue = num;
        System.out.println("Int to double: " + num + " -> " + doubleValue);
        
        // Int to long
        long longValue = num;
        System.out.println("Int to long: " + num + " -> " + longValue);
        
        // ===== CONVERT TO INT =====
        System.out.println("\n=== Convert to Int ===");
        
        // String to int
        String strNum = "123";
        int intFromString = Integer.parseInt(strNum);
        System.out.println("String to int: \"" + strNum + "\" -> " + intFromString);
        
        // Double to int (truncates decimal)
        double d = 99.7;
        int intFromDouble = (int) d;
        System.out.println("Double to int: " + d + " -> " + intFromDouble);
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. DECLARING INT ARRAYS:
 *    int[] array = new int[size];
 *    int[] array = {1, 2, 3, 4, 5};
 *    int[] array = new int[]{1, 2, 3, 4, 5};
 * 
 * 2. INT RANGE:
 *    - Signed: -2,147,483,648 to 2,147,483,647
 *    - Takes 4 bytes of memory (32 bits)
 *    - Most commonly used integer type
 * 
 * 3. WHY USE INT?
 *    - Most common for general purpose numbers
 *    - Perfect for: prices, ages, quantities, counts
 *    - No overflow issues for most everyday calculations
 *    - Supported by all Java operations
 * 
 * 4. COMMON OPERATIONS:
 *    - Sum: accumulate with +=
 *    - Average: sum / count (cast to double for decimals)
 *    - Min/Max: compare each element
 *    - Search: loop through and compare
 *    - Sort: use Arrays.sort()
 *    - Binary search: only on sorted arrays
 * 
 * 5. IMPORTANT METHODS:
 *    - Arrays.sort(array)         - sorts array
 *    - Arrays.binarySearch()     - searches in sorted array
 *    - Arrays.toString()         - converts to string for printing
 *    - Integer.parseInt()        - converts String to int
 *    - Integer.toString()        - converts int to String
 * 
 * 6. TIPS:
 *    - Use Arrays.toString() for easy printing
 *    - Remember to use (int) when converting from double
 *    - Binary search only works on sorted arrays!
 *    - int is default choice for whole numbers
 */
