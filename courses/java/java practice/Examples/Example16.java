// Example16: Array of Enums - Beginner Tutorial
// This shows how to use arrays with enum types

// Step 1: Define an enum (a set of named constants)
// Enums are perfect for representing fixed set of values
enum Day {
    MONDAY,    // Each item is automatically a Day object
    TUESDAY,
    WEDNESDAY,
    THURSDAY,
    FRIDAY,
    SATURDAY,
    SUNDAY
}

// Step 2: Another enum for colors
enum Color {
    RED,
    BLUE,
    GREEN,
    YELLOW,
    ORANGE
}

// Step 3: Enum with values (more advanced)
enum Grade {
    A("Excellent", 90),
    B("Good", 80),
    C("Average", 70),
    D("Pass", 60),
    F("Fail", 50);
    
    // Each enum constant can have associated data
    public String description;
    public int minScore;
    
    // Private constructor
    Grade(String desc, int score) {
        description = desc;
        minScore = score;
    }
}

// Main class
public class Example16 {
    public static void main(String[] args) {
        
        // ===== BASIC ARRAY OF ENUMS =====
        System.out.println("=== Basic Array of Enums ===");
        
        // Create an array of Day enum
        Day[] weekdays = new Day[5];
        weekdays[0] = Day.MONDAY;
        weekdays[1] = Day.TUESDAY;
        weekdays[2] = Day.WEDNESDAY;
        weekdays[3] = Day.THURSDAY;
        weekdays[4] = Day.FRIDAY;
        
        // Print all weekdays
        System.out.println("Weekdays:");
        for (int i = 0; i < weekdays.length; i++) {
            System.out.println("  " + weekdays[i]);
        }
        
        // ===== SIMPLER WAY: Initialize array directly =====
        System.out.println("\n=== Direct Initialization ===");
        
        Day[] weekend = {Day.SATURDAY, Day.SUNDAY};
        
        System.out.println("Weekend days:");
        for (Day d : weekend) {
            System.out.println("  " + d);
        }
        
        // ===== ARRAY OF ALL ENUM VALUES =====
        System.out.println("\n=== All Days of Week ===");
        
        // Get all values of an enum using values() method
        Day[] allDays = Day.values();
        
        for (Day d : allDays) {
            System.out.println("  " + d);
        }
        
        // ===== ENUM WITH SWITCH STATEMENT =====
        System.out.println("\n=== Using Enum with Switch ===");
        
        Day today = Day.WEDNESDAY;
        
        switch(today) {
            case MONDAY:
            case TUESDAY:
            case WEDNESDAY:
            case THURSDAY:
            case FRIDAY:
                System.out.println(today + " is a weekday");
                break;
            case SATURDAY:
            case SUNDAY:
                System.out.println(today + " is weekend!");
                break;
        }
        
        // ===== ADVANCED: Enum with values in array =====
        System.out.println("\n=== Enum with Associated Values ===");
        
        Grade[] allGrades = Grade.values();
        
        for (Grade g : allGrades) {
            System.out.println("Grade " + g + ": " + g.description + " (min score: " + g.minScore + ")");
        }
        
        // ===== PRACTICAL EXAMPLE: Array of Colors =====
        System.out.println("\n=== Practical Example: Traffic Lights ===");
        
        Color[] trafficColors = {Color.RED, Color.YELLOW, Color.GREEN};
        
        for (Color c : trafficColors) {
            System.out.print(c + " -> ");
            switch(c) {
                case RED:
                    System.out.println("STOP");
                    break;
                case YELLOW:
                    System.out.println("GET READY");
                    break;
                case GREEN:
                    System.out.println("GO");
                    break;
            }
        }
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. WHAT IS AN ENUM?
 *    - A special data type that defines a fixed set of constants
 *    - Example: Days of week, Months, Colors, Status
 * 
 * 2. HOW TO CREATE AN ENUM?
 *    enum EnumName {
 *        VALUE1, VALUE2, VALUE3
 *    }
 * 
 * 3. HOW TO CREATE ARRAY OF ENUMS?
 *    EnumName[] array = new EnumName[size];
 *    array[0] = EnumName.VALUE1;
 * 
 * 4. USEFUL ENUM METHODS:
 *    - values() - returns all values of enum as an array
 *    - ordinal() - returns position of enum (0-based)
 *    - name() - returns the name as String
 *    - valueOf(String) - converts String to enum
 * 
 * 5. WHY USE ENUMS?
 *    - Type safety: compiler catches typos
 *    - Readable code: more meaningful than numbers
 *    - Fixed values: prevents invalid values
 */
