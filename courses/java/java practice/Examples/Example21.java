// Example21: Array of Shorts - Beginner Tutorial
// This shows different ways to work with short arrays

public class Example21 {
    public static void main(String[] args) {
        
        // ===== METHOD 1: Create short array with size =====
        System.out.println("=== Method 1: Create with size ===");
        
        short[] numbers = new short[5];
        
        // Default value is 0
        numbers[0] = 100;
        numbers[1] = 250;
        numbers[2] = 500;
        numbers[3] = 750;
        numbers[4] = 1000;
        
        System.out.println("Short values:");
        for (int i = 0; i < numbers.length; i++) {
            System.out.println("  Index " + i + ": " + numbers[i]);
        }
        
        // ===== METHOD 2: Direct initialization =====
        System.out.println("\n=== Method 2: Direct initialization ===");
        
        short[] digits = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        
        System.out.print("Digits: ");
        for (short d : digits) {
            System.out.print(d + " ");
        }
        System.out.println();
        
        // ===== SHORT RANGE =====
        System.out.println("\n=== Short Range ===");
        
        // Short ranges from -32,768 to 32,767
        short minValue = -32768;
        short maxValue = 32767;
        
        System.out.println("Minimum short value: " + minValue);
        System.out.println("Maximum short value: " + maxValue);
        
        // ===== PRACTICAL: Student Scores =====
        System.out.println("\n=== Practical: Student Test Scores ===");
        
        // Perfect use case - test scores (0-100)
        short[] scores = {85, 90, 78, 92, 88, 95, 72, 80};
        String[] students = {"Alice", "Bob", "Charlie", "Diana", "Eve", "Frank", "Grace", "Henry"};
        
        System.out.println("Student Scores:");
        for (int i = 0; i < scores.length; i++) {
            System.out.println("  " + students[i] + ": " + scores[i]);
        }
        
        // Calculate average
        int total = 0;
        for (short score : scores) {
            total += score;
        }
        
        double average = (double) total / scores.length;
        System.out.println("\nClass Average: " + average);
        
        // Find highest and lowest
        short highest = scores[0];
        short lowest = scores[0];
        String topStudent = students[0];
        String lowStudent = students[0];
        
        for (int i = 1; i < scores.length; i++) {
            if (scores[i] > highest) {
                highest = scores[i];
                topStudent = students[i];
            }
            if (scores[i] < lowest) {
                lowest = scores[i];
                lowStudent = students[i];
            }
        }
        
        System.out.println("Highest: " + topStudent + " with " + highest);
        System.out.println("Lowest: " + lowStudent + " with " + lowest);
        
        // ===== PRACTICAL: Inventory Count =====
        System.out.println("\n=== Practical: Store Inventory ===");
        
        // Good for inventory - small positive numbers
        short[] inventory = {150, 200, 75, 300, 50, 125};
        String[] items = {"Apples", "Oranges", "Bananas", "Mangoes", "Grapes", "Peaches"};
        
        System.out.println("Store Inventory:");
        for (int i = 0; i < items.length; i++) {
            System.out.println("  " + items[i] + ": " + inventory[i] + " units");
        }
        
        // Total inventory
        int totalItems = 0;
        for (short count : inventory) {
            totalItems += count;
        }
        
        System.out.println("Total inventory: " + totalItems + " units");
        
        // ===== CONVERT SHORT TO OTHER TYPES =====
        System.out.println("\n=== Convert Short to Other Types ===");
        
        short s = 500;
        
        // Short to int (widening)
        int intValue = s;
        System.out.println("Short to int: " + s + " -> " + intValue);
        
        // Short to long
        long longValue = s;
        System.out.println("Short to long: " + s + " -> " + longValue);
        
        // Short to double
        double doubleValue = s;
        System.out.println("Short to double: " + s + " -> " + doubleValue);
        
        // Short to String
        String stringValue = Short.toString(s);
        System.out.println("Short to String: " + s + " -> " + stringValue);
        
        // ===== CONVERT TO SHORT =====
        System.out.println("\n=== Convert to Short ===");
        
        // String to short
        String strNum = "1000";
        short shortFromString = Short.parseShort(strNum);
        System.out.println("String to short: \"" + strNum + "\" -> " + shortFromString);
        
        // Int to short (narrowing - needs casting)
        int intNum = 500;
        short shortFromInt = (short) intNum;
        System.out.println("Int to short: " + intNum + " -> " + shortFromInt);
        
        // ===== PRACTICAL: Temperature Records =====
        System.out.println("\n=== Practical: Temperature Records ===");
        
        // Temperatures in Celsius (can be negative but not too large)
        short[] temps = {22, 25, 19, 30, 24, 18, 15, 20, 27, 23};
        String[] days = {"Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun", "Mon", "Tue", "Wed"};
        
        System.out.println("Weekly Temperatures:");
        for (int i = 0; i < days.length; i++) {
            System.out.println("  " + days[i] + ": " + temps[i] + "°C");
        }
        
        // Calculate average temperature
        int tempSum = 0;
        for (short temp : temps) {
            tempSum += temp;
        }
        
        double avgTemp = (double) tempSum / temps.length;
        System.out.println("Average temperature: " + avgTemp + "°C");
        
        // ===== SHORT ARRAYS WITH MATH =====
        System.out.println("\n=== Math Operations ===");
        
        short[] values = {10, 20, 30, 40, 50};
        
        // Sum using int (to avoid overflow)
        int sum = 0;
        for (short v : values) {
            sum += v;
        }
        System.out.println("Sum: " + sum);
        
        // Multiply each by 2
        System.out.print("Doubled values: ");
        for (short v : values) {
            System.out.print((v * 2) + " ");
        }
        System.out.println();
        
        // ===== WHY USE SHORT? =====
        System.out.println("\n=== Why Use Short? ===");
        
        // Smaller than int but bigger than byte
        // Good for: test scores, temperatures, inventory counts
        
        short[] percentages = {45, 67, 89, 23, 78, 90, 56, 34};
        
        System.out.println("Percentages (perfect for short):");
        for (short p : percentages) {
            System.out.print(p + "% ");
        }
        System.out.println();
        
        // Memory comparison
        short[] shortArray = new short[1000];
        int[] intArray = new int[1000];
        
        System.out.println("\nMemory comparison for 1000 elements:");
        System.out.println("  short array: " + shortArray.length * 2 + " bytes");
        System.out.println("  int array: " + intArray.length * 4 + " bytes");
        System.out.println("  Savings: " + (intArray.length * 4 - shortArray.length * 2) + " bytes!");
        
        // ===== PRACTICAL: Days in Months =====
        System.out.println("\n=== Practical: Days in Months ===");
        
        // Days in each month (short is perfect!)
        short[] daysInMonth = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
        String[] months = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", 
                          "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
        
        System.out.println("Days in each month:");
        for (int i = 0; i < months.length; i++) {
            System.out.println("  " + months[i] + ": " + daysInMonth[i] + " days");
        }
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. DECLARING SHORT ARRAYS:
 *    short[] array = new short[size];
 *    short[] array = {1, 2, 3, 4, 5};
 * 
 * 2. SHORT RANGE:
 *    - Signed: -32,768 to 32,767
 *    - Takes 2 bytes of memory (16 bits)
 * 
 * 3. WHY USE SHORT?
 *    - Saves memory (2 bytes vs 4 bytes for int)
 *    - Perfect for moderate small numbers:
 *      * Test scores (0-100)
 *      * Temperatures (-100 to 100)
 *      * Inventory counts
 *      * Percentages (0-100)
 *      * Days in months (28-31)
 *      * Ages (0-150, but shorts are better than bytes for this)
 * 
 * 4. CONVERSIONS:
 *    - short to int: automatic (widening)
 *    - short to long: automatic
 *    - short to String: Short.toString(short)
 *    - String to short: Short.parseShort(string)
 *    - int to short: requires casting (short)
 * 
 * 5. IMPORTANT NOTES:
 *    - Default value is 0
 *    - Can store both positive and negative numbers
 *    - Math operations may require casting to int to avoid overflow
 *    - Use short when you don't need full int range but want to save memory
 * 
 * 6. MEMORY SAVINGS:
 *    - short: 2 bytes
 *    - int: 4 bytes (2x more than short)
 *    - 50% memory savings compared to int!
 */
