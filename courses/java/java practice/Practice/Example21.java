/*
 * SUB TOPIC: Short Array Operations
 * 
 * DEFINITION:
 * A short array stores small integer values from -32,768 to 32,767. It uses 2 bytes of memory per element,
 * making it more memory efficient than int for moderate small numbers like test scores and temperatures.
 * 
 * FUNCTIONALITIES:
 * 1. Short array declaration and initialization
 * 2. Short range and limitations
 * 3. Type conversions
 * 4. Mathematical operations
 * 5. Practical applications
 */

public class Example21 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: Short Array
        System.out.println("=== Short Array ===");
        
        short[] numbers = new short[5]; // Default value is 0
        numbers[0] = 100;
        numbers[1] = 200;
        numbers[2] = 300;
        numbers[3] = 400;
        numbers[4] = 500;
        
        for (int i = 0; i < numbers.length; i++) {
            System.out.println("numbers[" + i + "] = " + numbers[i]);
        }
        
        // Real-time Example 1: Short Range
        System.out.println("\n=== Short Range ===");
        
        short minValue = -32768;
        short maxValue = 32767;
        
        System.out.println("Minimum: " + minValue);
        System.out.println("Maximum: " + maxValue);
        
        // Real-time Example 2: Student Scores
        System.out.println("\n=== Student Scores ===");
        
        short[] scores = {85, 90, 78, 92, 88};
        String[] students = {"Alice", "Bob", "Charlie", "Diana", "Eve"};
        
        int total = 0;
        for (short score : scores) {
            total += score;
        }
        
        double average = (double) total / scores.length;
        
        for (int i = 0; i < students.length; i++) {
            System.out.println(students[i] + ": " + scores[i]);
        }
        
        System.out.println("Average: " + average);
        
        // Real-time Example 3: Temperature Records
        System.out.println("\n=== Temperature Records ===");
        
        short[] temps = {22, 25, 19, 30, 24, 18, 15};
        
        short min = temps[0];
        short max = temps[0];
        
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
        
        short s = 500;
        
        int intValue = s; // short to int
        long longValue = s; // short to long
        double doubleValue = s; // short to double
        String stringValue = Short.toString(s); // short to String
        
        System.out.println("Short to int: " + intValue);
        System.out.println("Short to long: " + longValue);
        System.out.println("Short to double: " + doubleValue);
        System.out.println("Short to String: " + stringValue);
        
        // Real-time Example 5: Days in Months
        System.out.println("\n=== Days in Months ===");
        
        short[] daysInMonth = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
        String[] months = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
        
        for (int i = 0; i < months.length; i++) {
            System.out.println(months[i] + ": " + daysInMonth[i] + " days");
        }
        
        // Real-time Example 6: Inventory Count
        System.out.println("\n=== Inventory Count ===");
        
        short[] inventory = {150, 200, 75, 300, 50};
        String[] items = {"Apples", "Oranges", "Bananas", "Mangoes", "Grapes"};
        
        int totalItems = 0;
        for (short count : inventory) {
            totalItems += count;
        }
        
        for (int i = 0; i < items.length; i++) {
            System.out.println(items[i] + ": " + inventory[i]);
        }
        
        System.out.println("Total: " + totalItems);
    }
}
