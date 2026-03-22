/*
 * SUB TOPIC: Byte Array Operations
 * 
 * DEFINITION:
 * A byte array stores small integer values from -128 to 127. It uses 1 byte of memory per element,
 * making it memory efficient for storing small numbers like ages, grades, and small counters.
 * 
 * FUNCTIONALITIES:
 * 1. Byte array declaration and initialization
 * 2. Byte range and limitations
 * 3. Type conversions
 * 4. Memory efficiency
 * 5. Mathematical operations
 */

public class Example20 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: Byte Array
        System.out.println("=== Byte Array ===");
        
        byte[] numbers = new byte[5]; // Default value is 0
        numbers[0] = 10;
        numbers[1] = 20;
        numbers[2] = 30;
        numbers[3] = 40;
        numbers[4] = 50;
        
        for (int i = 0; i < numbers.length; i++) {
            System.out.println("numbers[" + i + "] = " + numbers[i]);
        }
        
        // Real-time Example 1: Byte Range
        System.out.println("\n=== Byte Range ===");
        
        byte minValue = -128;
        byte maxValue = 127;
        
        System.out.println("Minimum: " + minValue);
        System.out.println("Maximum: " + maxValue);
        
        // Real-time Example 2: Sum and Average
        System.out.println("\n=== Sum and Average ===");
        
        byte[] scores = {85, 90, 78, 92, 88};
        
        int sum = 0;
        for (byte score : scores) {
            sum += score;
        }
        
        double average = (double) sum / scores.length;
        System.out.println("Sum: " + sum);
        System.out.println("Average: " + average);
        
        // Real-time Example 3: Find Min and Max
        System.out.println("\n=== Find Min and Max ===");
        
        byte[] temps = {22, 25, 19, 30, 24};
        
        byte min = temps[0];
        byte max = temps[0];
        
        for (int i = 1; i < temps.length; i++) {
            if (temps[i] < min) {
                min = temps[i];
            }
            if (temps[i] > max) {
                max = temps[i];
            }
        }
        
        System.out.println("Minimum: " + min);
        System.out.println("Maximum: " + max);
        
        // Real-time Example 4: Type Conversions
        System.out.println("\n=== Type Conversions ===");
        
        byte b = 100;
        
        int intValue = b; // byte to int
        double doubleValue = b; // byte to double
        String stringValue = Byte.toString(b); // byte to String
        
        System.out.println("Byte to int: " + intValue);
        System.out.println("Byte to double: " + doubleValue);
        System.out.println("Byte to String: " + stringValue);
        
        // Real-time Example 5: String to Byte
        System.out.println("\n=== String to Byte ===");
        
        String strNum = "50";
        byte byteFromString = Byte.parseByte(strNum);
        System.out.println("String to byte: " + byteFromString);
        
        // Real-time Example 6: Grade Points
        System.out.println("\n=== Grade Points ===");
        
        byte[] gradePoints = {4, 3, 3, 2, 4};
        String[] subjects = {"Math", "Science", "English", "History", "Art"};
        
        int totalPoints = 0;
        for (byte gp : gradePoints) {
            totalPoints += gp;
        }
        
        double gpa = (double) totalPoints / gradePoints.length;
        
        for (int i = 0; i < subjects.length; i++) {
            System.out.println(subjects[i] + ": " + gradePoints[i]);
        }
        
        System.out.println("GPA: " + gpa);
    }
}
