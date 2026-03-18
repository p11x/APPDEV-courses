// Example19: Array of Booleans - Beginner Tutorial
// This shows different ways to work with boolean arrays

public class Example19 {
    public static void main(String[] args) {
        
        // ===== METHOD 1: Create boolean array with size =====
        System.out.println("=== Method 1: Create with size ===");
        
        boolean[] flags = new boolean[3];
        
        // Default value is false
        flags[0] = true;
        flags[1] = false;
        flags[2] = true;
        
        System.out.println("Flags:");
        for (int i = 0; i < flags.length; i++) {
            System.out.println("  Index " + i + ": " + flags[i]);
        }
        
        // ===== METHOD 2: Direct initialization =====
        System.out.println("\n=== Method 2: Direct initialization ===");
        
        boolean[] answers = {true, false, true, true, false};
        
        System.out.print("Answers: ");
        for (boolean answer : answers) {
            System.out.print(answer + " ");
        }
        System.out.println();
        
        // ===== PRACTICAL EXAMPLE: Attendance System =====
        System.out.println("\n=== Practical: Attendance System ===");
        
        String[] students = {"Alice", "Bob", "Charlie", "Diana", "Eve"};
        boolean[] attendance = {true, true, false, true, false};
        
        System.out.println("Student Attendance:");
        for (int i = 0; i < students.length; i++) {
            String status = attendance[i] ? "Present" : "Absent";
            System.out.println("  " + students[i] + ": " + status);
        }
        
        // Count present and absent
        int presentCount = 0;
        int absentCount = 0;
        
        for (boolean present : attendance) {
            if (present) {
                presentCount++;
            } else {
                absentCount++;
            }
        }
        
        System.out.println("\nTotal Present: " + presentCount);
        System.out.println("Total Absent: " + absentCount);
        
        // ===== PRACTICAL EXAMPLE: Quiz/Exam Results =====
        System.out.println("\n=== Practical: Quiz Results ===");
        
        String[] questions = {"Q1", "Q2", "Q3", "Q4", "Q5"};
        boolean[] correctAnswers = {true, false, true, true, false};
        boolean[] userAnswers = {true, false, false, true, true};
        
        int correctCount = 0;
        
        System.out.println("Question | Correct | User | Status");
        System.out.println("---------|---------|------|-------");
        
        for (int i = 0; i < questions.length; i++) {
            boolean isCorrect = correctAnswers[i] == userAnswers[i];
            if (isCorrect) {
                correctCount++;
            }
            
            String status = isCorrect ? "✓" : "✗";
            System.out.println(questions[i] + "        | " + 
                             correctAnswers[i] + "      | " + 
                             userAnswers[i] + "   | " + status);
        }
        
        System.out.println("\nScore: " + correctCount + "/" + questions.length);
        
        // Calculate percentage
        double percentage = (correctCount * 100.0) / questions.length;
        System.out.println("Percentage: " + percentage + "%");
        
        // ===== PRACTICAL EXAMPLE: Yes/No Survey =====
        System.out.println("\n=== Practical: Survey Results ===");
        
        String[] surveyQuestions = {"Like Java?", "Have programming experience?", "Will recommend?"};
        boolean[] yesResponses = {true, true, false};
        boolean[] noResponses = {false, false, true};
        
        for (int i = 0; i < surveyQuestions.length; i++) {
            String result = yesResponses[i] ? "Yes" : "No";
            System.out.println(surveyQuestions[i] + " -> " + result);
        }
        
        // ===== USING BOOLEAN ARRAY AS FILTERS =====
        System.out.println("\n=== Using Boolean Array as Filter ===");
        
        int[] numbers = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        boolean[] isEven = new boolean[numbers.length];
        
        // Determine even/odd
        for (int i = 0; i < numbers.length; i++) {
            isEven[i] = (numbers[i] % 2 == 0);
        }
        
        System.out.println("Numbers and Even status:");
        for (int i = 0; i < numbers.length; i++) {
            String type = isEven[i] ? "Even" : "Odd";
            System.out.println("  " + numbers[i] + " is " + type);
        }
        
        // Filter: Print only even numbers
        System.out.print("Even numbers: ");
        for (int i = 0; i < numbers.length; i++) {
            if (isEven[i]) {
                System.out.print(numbers[i] + " ");
            }
        }
        System.out.println();
        
        // ===== PRACTICAL EXAMPLE: Login System =====
        System.out.println("\n=== Practical: Login Status ===");
        
        String[] usernames = {"admin", "user1", "guest", "user2"};
        boolean[] isLoggedIn = {true, false, false, true};
        
        // Check if user is logged in
        String checkUser = "user1";
        boolean found = false;
        
        for (int i = 0; i < usernames.length; i++) {
            if (usernames[i].equals(checkUser)) {
                found = true;
                System.out.println(checkUser + " is logged in: " + isLoggedIn[i]);
                break;
            }
        }
        
        if (!found) {
            System.out.println("User not found");
        }
        
        // Find all logged in users
        System.out.println("Logged in users:");
        for (int i = 0; i < usernames.length; i++) {
            if (isLoggedIn[i]) {
                System.out.println("  - " + usernames[i]);
            }
        }
        
        // ===== BOOLEAN OPERATIONS =====
        System.out.println("\n=== Boolean Operations ===");
        
        boolean[] values = {true, false, true, false};
        
        // AND operation on all values
        boolean andResult = true;
        for (boolean v : values) {
            andResult = andResult && v;
        }
        System.out.println("AND of all: " + andResult);
        
        // OR operation on all values
        boolean orResult = false;
        for (boolean v : values) {
            orResult = orResult || v;
        }
        System.out.println("OR of all: " + orResult);
        
        // NOT operation
        boolean original = true;
        System.out.println("NOT true: " + (!original));
        System.out.println("NOT false: " + (!false));
        
        // ===== TOGGLE BOOLEAN VALUES =====
        System.out.println("\n=== Toggle Boolean Values ===");
        
        boolean[] toggles = {true, false, true, true, false};
        
        System.out.print("Before toggle: ");
        for (boolean t : toggles) {
            System.out.print(t + " ");
        }
        System.out.println();
        
        // Toggle all values
        for (int i = 0; i < toggles.length; i++) {
            toggles[i] = !toggles[i];
        }
        
        System.out.print("After toggle: ");
        for (boolean t : toggles) {
            System.out.print(t + " ");
        }
        System.out.println();
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. DECLARING BOOLEAN ARRAYS:
 *    boolean[] array = new boolean[size];
 *    boolean[] array = {true, false, true};
 * 
 * 2. BOOLEAN VALUES:
 *    - true  = yes, on, 1, correct
 *    - false = no, off, 0, incorrect
 *    - Default value is false
 * 
 * 3. COMMON USES:
 *    - Attendance tracking (present/absent)
 *    - Quiz/exam results (correct/wrong)
 *    - User login status
 *    - Yes/No surveys
 *    - Filtering data (even/odd)
 *    - Toggle switches
 * 
 * 4. BOOLEAN OPERATIONS:
 *    - && (AND) - true if both are true
 *    - || (OR)  - true if at least one is true
 *    - ! (NOT)  - inverts the value
 * 
 * 5. BOOLEAN IN CONDITIONS:
 *    - Use ternary operator: condition ? value1 : value2
 *    - Use if-else statements
 *    - Compare with == (but not needed for boolean variables)
 * 
 * 6. TIPS:
 *    - Don't compare booleans: use "if (flag)" not "if (flag == true)"
 *    - Boolean arrays are great for filters
 *    - Use Arrays.toString() to print boolean arrays easily
 */
