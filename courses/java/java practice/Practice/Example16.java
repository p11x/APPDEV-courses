/*
 * SUB TOPIC: Enumerations (Enum)
 * 
 * DEFINITION:
 * An enum is a special data type that defines a fixed set of constants. It provides type safety
 * and makes code more readable. Enums can have fields, methods, and constructors.
 * 
 * FUNCTIONALITIES:
 * 1. Creating enum types
 * 2. Using enum with arrays
 * 3. Enum methods (values(), ordinal())
 * 4. Enum with switch statements
 * 5. Enum with custom fields and methods
 */

// Define enum for Days
enum Day {
    MONDAY, TUESDAY, WEDNESDAY, THURSDAY, FRIDAY, SATURDAY, SUNDAY
}

// Define enum for Colors
enum Color {
    RED, BLUE, GREEN, YELLOW, ORANGE
}

// Define enum with values
enum Grade {
    A("Excellent", 90),
    B("Good", 80),
    C("Average", 70),
    D("Pass", 60),
    F("Fail", 50);
    
    public String description;
    public int minScore;
    
    Grade(String desc, int score) {
        description = desc;
        minScore = score;
    }
}

public class Example16 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: Enum Basics
        System.out.println("=== Enum Basics ===");
        
        Day[] weekdays = new Day[5]; // Array of enum type
        weekdays[0] = Day.MONDAY;
        weekdays[1] = Day.TUESDAY;
        weekdays[2] = Day.WEDNESDAY;
        weekdays[3] = Day.THURSDAY;
        weekdays[4] = Day.FRIDAY;
        
        System.out.println("Weekdays:");
        for (int i = 0; i < weekdays.length; i++) {
            System.out.println("  " + weekdays[i]);
        }
        
        // Real-time Example 1: Using enum values()
        System.out.println("\n=== All Days of Week ===");
        
        Day[] allDays = Day.values(); // Get all enum values
        for (Day d : allDays) {
            System.out.println(d);
        }
        
        // Real-time Example 2: Enum with Switch
        System.out.println("\n=== Enum with Switch ===");
        
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
                System.out.println(today + " is weekend");
                break;
        }
        
        // Real-time Example 3: Enum with custom values
        System.out.println("\n=== Enum with Values ===");
        
        Grade[] allGrades = Grade.values();
        for (Grade g : allGrades) {
            System.out.println("Grade " + g + ": " + g.description + " (min: " + g.minScore + ")");
        }
        
        // Real-time Example 4: Traffic light simulation
        System.out.println("\n=== Traffic Light ===");
        
        Color[] trafficColors = {Color.RED, Color.YELLOW, Color.GREEN};
        
        for (Color c : trafficColors) {
            switch(c) {
                case RED:
                    System.out.println(c + " -> STOP");
                    break;
                case YELLOW:
                    System.out.println(c + " -> GET READY");
                    break;
                case GREEN:
                    System.out.println(c + " -> GO");
                    break;
            }
        }
        
        // Real-time Example 5: Enum ordinal
        System.out.println("\n=== Enum Ordinal ===");
        
        Day day = Day.FRIDAY;
        System.out.println(day + " is at position: " + day.ordinal());
        
        // Real-time Example 6: Enum array initialization
        System.out.println("\n=== Enum Array ===");
        
        Day[] weekend = {Day.SATURDAY, Day.SUNDAY};
        
        for (Day d : weekend) {
            System.out.println(d);
        }
    }
}
