/*
 * SUB TOPIC: Boolean Array Operations
 * 
 * DEFINITION:
 * A boolean array stores true/false values. Boolean arrays are commonly used for tracking status,
 * attendance, filtering data, and making decisions based on conditions.
 * 
 * FUNCTIONALITIES:
 * 1. Boolean array declaration and initialization
 * 2. Using boolean arrays for status tracking
 * 3. Boolean operations (AND, OR, NOT)
 * 4. Filtering data using boolean arrays
 * 5. Toggle operations
 */

public class Example19 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: Boolean Array
        System.out.println("=== Boolean Array ===");
        
        boolean[] flags = new boolean[3]; // Default value is false
        flags[0] = true;
        flags[1] = false;
        flags[2] = true;
        
        for (int i = 0; i < flags.length; i++) {
            System.out.println("flags[" + i + "] = " + flags[i]);
        }
        
        // Real-time Example 1: Attendance System
        System.out.println("\n=== Attendance System ===");
        
        String[] students = {"Alice", "Bob", "Charlie", "Diana"};
        boolean[] attendance = {true, true, false, true};
        
        int presentCount = 0;
        
        for (int i = 0; i < students.length; i++) {
            String status = attendance[i] ? "Present" : "Absent";
            System.out.println(students[i] + ": " + status);
            
            if (attendance[i]) {
                presentCount++;
            }
        }
        
        System.out.println("Total Present: " + presentCount);
        
        // Real-time Example 2: Quiz Results
        System.out.println("\n=== Quiz Results ===");
        
        boolean[] correctAnswers = {true, false, true, true, false};
        boolean[] userAnswers = {true, false, false, true, true};
        
        int correctCount = 0;
        
        for (int i = 0; i < correctAnswers.length; i++) {
            if (correctAnswers[i] == userAnswers[i]) {
                correctCount++;
            }
        }
        
        double percentage = (correctCount * 100.0) / correctAnswers.length;
        System.out.println("Correct: " + correctCount + "/" + correctAnswers.length);
        System.out.println("Percentage: " + percentage + "%");
        
        // Real-time Example 3: Filter Even Numbers
        System.out.println("\n=== Filter Even Numbers ===");
        
        int[] numbers = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        boolean[] isEven = new boolean[numbers.length];
        
        for (int i = 0; i < numbers.length; i++) {
            isEven[i] = (numbers[i] % 2 == 0);
        }
        
        System.out.print("Even numbers: ");
        for (int i = 0; i < numbers.length; i++) {
            if (isEven[i]) {
                System.out.print(numbers[i] + " ");
            }
        }
        
        // Real-time Example 4: Login Status
        System.out.println("\n\n=== Login Status ===");
        
        String[] users = {"admin", "user1", "guest", "user2"};
        boolean[] isLoggedIn = {true, false, true, false};
        
        System.out.println("Logged in users:");
        for (int i = 0; i < users.length; i++) {
            if (isLoggedIn[i]) {
                System.out.println("  - " + users[i]);
            }
        }
        
        // Real-time Example 5: Boolean Operations
        System.out.println("\n=== Boolean Operations ===");
        
        boolean[] values = {true, false, true};
        
        boolean andResult = true;
        for (boolean v : values) {
            andResult = andResult && v;
        }
        System.out.println("AND of all: " + andResult);
        
        boolean orResult = false;
        for (boolean v : values) {
            orResult = orResult || v;
        }
        System.out.println("OR of all: " + orResult);
        
        boolean original = true;
        System.out.println("NOT true: " + (!original));
        
        // Real-time Example 6: Toggle Values
        System.out.println("\n=== Toggle Values ===");
        
        boolean[] toggles = {true, false, true};
        
        System.out.print("Before: ");
        for (boolean t : toggles) {
            System.out.print(t + " ");
        }
        
        for (int i = 0; i < toggles.length; i++) {
            toggles[i] = !toggles[i];
        }
        
        System.out.print("\nAfter: ");
        for (boolean t : toggles) {
            System.out.print(t + " ");
        }
    }
}
