// Example20: Array of Bytes - Beginner Tutorial
// This shows different ways to work with byte arrays

public class Example20 {
    public static void main(String[] args) {
        
        // ===== METHOD 1: Create byte array with size =====
        System.out.println("=== Method 1: Create with size ===");
        
        byte[] numbers = new byte[5];
        
        // Default value is 0
        numbers[0] = 10;
        numbers[1] = 25;
        numbers[2] = 50;
        numbers[3] = 75;
        numbers[4] = 100;
        
        System.out.println("Byte values:");
        for (int i = 0; i < numbers.length; i++) {
            System.out.println("  Index " + i + ": " + numbers[i]);
        }
        
        // ===== METHOD 2: Direct initialization =====
        System.out.println("\n=== Method 2: Direct initialization ===");
        
        byte[] digits = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        
        System.out.print("Digits: ");
        for (byte d : digits) {
            System.out.print(d + " ");
        }
        System.out.println();
        
        // ===== BYTE RANGE =====
        System.out.println("\n=== Byte Range ===");
        
        // Byte ranges from -128 to 127
        byte minValue = -128;
        byte maxValue = 127;
        
        System.out.println("Minimum byte value: " + minValue);
        System.out.println("Maximum byte value: " + maxValue);
        
        // ===== PRACTICAL: Sum and Average =====
        System.out.println("\n=== Practical: Sum and Average ===");
        
        byte[] scores = {85, 90, 78, 92, 88};
        
        int sum = 0;
        for (byte score : scores) {
            sum += score;
        }
        
        double average = (double) sum / scores.length;
        
        System.out.println("Scores: 85, 90, 78, 92, 88");
        System.out.println("Sum: " + sum);
        System.out.println("Average: " + average);
        
        // ===== PRACTICAL: Find Min and Max =====
        System.out.println("\n=== Practical: Find Min and Max ===");
        
        byte[] temperatures = {22, 25, 19, 30, 24, 18};
        
        byte min = temperatures[0];
        byte max = temperatures[0];
        
        for (int i = 1; i < temperatures.length; i++) {
            if (temperatures[i] < min) {
                min = temperatures[i];
            }
            if (temperatures[i] > max) {
                max = temperatures[i];
            }
        }
        
        System.out.println("Temperatures: 22, 25, 19, 30, 24, 18");
        System.out.println("Minimum: " + min + "°C");
        System.out.println("Maximum: " + max + "°C");
        
        // ===== PRACTICAL: Age Store =====
        System.out.println("\n=== Practical: Store Ages ===");
        
        // Perfect use case - ages are small numbers
        byte[] ages = {25, 30, 22, 35, 28, 45, 19};
        
        System.out.println("Ages of family members:");
        String[] names = {"Dad", "Mom", "Brother", "Sister", "Me", "Grandpa", "Cousin"};
        
        for (int i = 0; i < ages.length; i++) {
            System.out.println("  " + names[i] + ": " + ages[i] + " years");
        }
        
        // ===== CONVERT BYTE TO OTHER TYPES =====
        System.out.println("\n=== Convert Byte to Other Types ===");
        
        byte b = 100;
        
        // Byte to int
        int intValue = b;
        System.out.println("Byte to int: " + b + " -> " + intValue);
        
        // Byte to double
        double doubleValue = b;
        System.out.println("Byte to double: " + b + " -> " + doubleValue);
        
        // Byte to String
        String stringValue = Byte.toString(b);
        System.out.println("Byte to String: " + b + " -> " + stringValue);
        
        // Byte to short
        short shortValue = b;
        System.out.println("Byte to short: " + b + " -> " + shortValue);
        
        // ===== CONVERT OTHER TYPES TO BYTE =====
        System.out.println("\n=== Convert to Byte ===");
        
        // String to byte
        String strNum = "50";
        byte byteFromString = Byte.parseByte(strNum);
        System.out.println("String to byte: \"" + strNum + "\" -> " + byteFromString);
        
        // ===== BYTE ARRAYS AND LOOPS =====
        System.out.println("\n=== Using Different Loops ===");
        
        byte[] data = {5, 10, 15, 20, 25};
        
        // For loop
        System.out.print("For loop: ");
        for (int i = 0; i < data.length; i++) {
            System.out.print(data[i] + " ");
        }
        System.out.println();
        
        // For-each loop
        System.out.print("For-each: ");
        for (byte d : data) {
            System.out.print(d + " ");
        }
        System.out.println();
        
        // ===== MATH OPERATIONS ON BYTES =====
        System.out.println("\n=== Math Operations ===");
        
        byte[] mathValues = {10, 20, 30, 40};
        
        int total = 0;
        for (byte v : mathValues) {
            total += v * 2;  // Multiply each by 2
        }
        
        System.out.println("Values doubled and summed: " + total);
        
        // ===== PRACTICAL: Grade Scale =====
        System.out.println("\n=== Practical: Grade Points ===");
        
        byte[] gradePoints = {4, 3, 3, 2, 4};
        String[] subjects = {"Math", "Science", "English", "History", "Art"};
        
        int totalPoints = 0;
        for (byte gp : gradePoints) {
            totalPoints += gp;
        }
        
        double gpa = (double) totalPoints / gradePoints.length;
        
        System.out.println("Subjects and grades:");
        for (int i = 0; i < subjects.length; i++) {
            System.out.println("  " + subjects[i] + ": " + gradePoints[i]);
        }
        
        System.out.println("GPA: " + gpa);
        
        // ===== WHY USE BYTE? =====
        System.out.println("\n=== Why Use Byte? ===");
        
        // Save memory - byte is 1 byte, int is 4 bytes
        // Perfect for small values like ages, grades, months
        
        byte[] months = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
        
        System.out.println("Months of the year (stored as bytes):");
        for (byte m : months) {
            System.out.print(m + " ");
        }
        System.out.println();
        
        // Compare memory usage
        byte[] byteArray = new byte[1000];
        int[] intArray = new int[1000];
        
        System.out.println("\nMemory comparison for 1000 elements:");
        System.out.println("  byte array: " + byteArray.length + " bytes");
        System.out.println("  int array: " + intArray.length * 4 + " bytes");
        System.out.println("  Savings: " + (intArray.length * 4 - byteArray.length) + " bytes!");
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. DECLARING BYTE ARRAYS:
 *    byte[] array = new byte[size];
 *    byte[] array = {1, 2, 3, 4, 5};
 * 
 * 2. BYTE RANGE:
 *    - Signed: -128 to 127
 *    - Unsigned: 0 to 255
 *    - Takes 1 byte of memory (8 bits)
 * 
 * 3. WHY USE BYTE?
 *    - Saves memory (1 byte vs 4 bytes for int)
 *    - Perfect for small numbers:
 *      * Ages (0-150)
 *      * Grade points (0-4 or 0-10)
 *      * Months (1-12)
 *      * Days (1-31)
 *      * Small counters
 * 
 * 4. CONVERSIONS:
 *    - byte to int: automatic (widening)
 *    - byte to String: Byte.toString(byte)
 *    - String to byte: Byte.parseByte(string)
 * 
 * 5. IMPORTANT NOTES:
 *    - Default value is 0
 *    - Can't store large numbers (use int or long)
 *    - Math operations may require casting to int
 *    - Great for file I/O and network operations
 * 
 * 6. WHEN NOT TO USE:
 *    - Large numbers (>127 or <-128)
 *    - Precise decimal calculations
 *    - Calculations that might overflow
 */
