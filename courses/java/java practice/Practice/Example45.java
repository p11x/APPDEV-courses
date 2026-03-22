/*
 * SUB TOPIC: Variable Arguments (varargs), Enum, and Static Import in Java
 * 
 * DEFINITION:
 * Variable Arguments (varargs) allows a method to accept zero or more arguments. Enumerations (enum) 
 * are a special type that represents a fixed set of constants. Static import allows accessing static 
 * members without the class name.
 * 
 * FUNCTIONALITIES:
 * 1. Varargs - Variable number of arguments
 * 2. Enum - Define fixed set of constants
 * 3. Enum with methods
 * 4. Static import - Access static members directly
 */

public class Example45 {
    
    // Varargs example - method with variable arguments
    static int sum(int... numbers) {
        int total = 0;
        for (int num : numbers) {
            total += num;
        }
        return total;
    }
    
    static void printAll(String... items) {
        for (String item : items) {
            System.out.print(item + " ");
        }
        System.out.println();
    }
    
    // Enum example
    enum Day {
        MONDAY("Monday", 1),
        TUESDAY("Tuesday", 2),
        WEDNESDAY("Wednesday", 3),
        THURSDAY("Thursday", 4),
        FRIDAY("Friday", 5),
        SATURDAY("Saturday", 6),
        SUNDAY("Sunday", 7);
        
        private final String name;
        private final int value;
        
        Day(String name, int value) {
            this.name = name;
            this.value = value;
        }
        
        public String getName() { return name; }
        public int getValue() { return value; }
    }
    
    // Enum for Status
    enum Status {
        PENDING,
        IN_PROGRESS,
        COMPLETED,
        FAILED;
        
        public String getDescription() {
            switch(this) {
                case PENDING: return "Waiting to start";
                case IN_PROGRESS: return "Currently working";
                case COMPLETED: return "Finished successfully";
                case FAILED: return "Did not succeed";
                default: return "Unknown";
            }
        }
    }
    
    public static void main(String[] args) {
        
        // Topic Explanation: Varargs
        
        System.out.println("=== Varargs ===");
        
        // Call with different number of arguments
        System.out.println("Sum of 1,2: " + sum(1, 2));
        System.out.println("Sum of 1,2,3: " + sum(1, 2, 3));
        System.out.println("Sum of 1-5: " + sum(1, 2, 3, 4, 5));
        System.out.println("Sum of no args: " + sum());
        
        printAll("Apple", "Banana", "Cherry");
        
        // Topic Explanation: Enum
        
        System.out.println("\n=== Enum Basic ===");
        
        // Get enum value
        Day today = Day.MONDAY;
        System.out.println("Today is: " + today);
        
        // Enum name and ordinal
        System.out.println("Name: " + today.name());
        System.out.println("Ordinal: " + today.ordinal());
        
        // Get all enum values
        System.out.println("\n=== All Days ===");
        for (Day d : Day.values()) {
            System.out.println(d + " - " + d.getValue());
        }
        
        // Topic Explanation: Enum with switch
        System.out.println("\n=== Enum in Switch ===");
        
        Day day = Day.FRIDAY;
        switch(day) {
            case MONDAY:
            case TUESDAY:
            case WEDNESDAY:
            case THURSDAY:
            case FRIDAY:
                System.out.println("Weekday");
                break;
            case SATURDAY:
            case SUNDAY:
                System.out.println("Weekend");
                break;
        }
        
        // Topic Explanation: Enum methods
        System.out.println("\n=== Enum Methods ===");
        Status status = Status.IN_PROGRESS;
        System.out.println("Status: " + status);
        System.out.println("Description: " + status.getDescription());
        
        // Real-time Example 1: Traffic Light
        System.out.println("\n=== Example 1: Traffic Light ===");
        
        enum TrafficLight {
            RED("Stop", 30),
            YELLOW("Wait", 5),
            GREEN("Go", 30);
            
            private String action;
            private int duration;
            
            TrafficLight(String action, int duration) {
                this.action = action;
                this.duration = duration;
            }
            
            public String getAction() { return action; }
            public int getDuration() { return duration; }
        }
        
        TrafficLight light = TrafficLight.GREEN;
        System.out.println("Light: " + light);
        System.out.println("Action: " + light.getAction());
        System.out.println("Duration: " + light.getDuration() + " seconds");
        
        // Real-time Example 2: Order Status Tracking
        System.out.println("\n=== Example 2: Order Status ===");
        
        Status orderStatus = Status.PENDING;
        
        while (orderStatus != Status.COMPLETED) {
            System.out.println("Current status: " + orderStatus.getDescription());
            
            if (orderStatus == Status.PENDING) {
                orderStatus = Status.IN_PROGRESS;
            } else if (orderStatus == Status.IN_PROGRESS) {
                orderStatus = Status.COMPLETED;
            }
        }
        System.out.println("Final: " + orderStatus.getDescription());
        
        // Real-time Example 3: Calculator with varargs
        System.out.println("\n=== Example 3: Calculator ===");
        
        class Calculator {
            public int add(int... nums) {
                int sum = 0;
                for (int n : nums) sum += n;
                return sum;
            }
            
            public int multiply(int... nums) {
                int result = 1;
                for (int n : nums) result *= n;
                return result;
            }
            
            public double average(int... nums) {
                if (nums.length == 0) return 0;
                return (double) add(nums) / nums.length;
            }
        }
        
        Calculator calc = new Calculator();
        System.out.println("Add: " + calc.add(1, 2, 3, 4, 5));
        System.out.println("Multiply: " + calc.multiply(2, 3, 4));
        System.out.println("Average: " + calc.average(10, 20, 30));
        
        // Real-time Example 4: Log Levels
        System.out.println("\n=== Example 4: Log Levels ===");
        
        enum LogLevel {
            DEBUG(1, "Debug"),
            INFO(2, "Info"),
            WARN(3, "Warning"),
            ERROR(4, "Error");
            
            private int priority;
            private String display;
            
            LogLevel(int priority, String display) {
                this.priority = priority;
                this.display = display;
            }
            
            public boolean isHigherThan(LogLevel other) {
                return this.priority > other.priority;
            }
        }
        
        LogLevel currentLevel = LogLevel.INFO;
        System.out.println("Current: " + currentLevel);
        
        if (currentLevel.isHigherThan(LogLevel.DEBUG)) {
            System.out.println("Debug logs are shown");
        }
        
        // Real-time Example 5: Grade System
        System.out.println("\n=== Example 5: Grade System ===");
        
        enum Grade {
            A(90, "Excellent"),
            B(80, "Good"),
            C(70, "Average"),
            D(60, "Pass"),
            F(0, "Fail");
            
            private int minScore;
            private String description;
            
            Grade(int minScore, String description) {
                this.minScore = minScore;
                this.description = description;
            }
            
            public static Grade getGrade(int score) {
                for (Grade g : values()) {
                    if (score >= g.minScore) {
                        return g;
                    }
                }
                return F;
            }
            
            public String getDescription() { return description; }
        }
        
        int score = 85;
        Grade grade = Grade.getGrade(score);
        System.out.println("Score: " + score);
        System.out.println("Grade: " + grade);
        System.out.println("Description: " + grade.getDescription());
        
        // Real-time Example 6: Method overloading with varargs
        System.out.println("\n=== Example 6: Message Logger ===");
        
        class Logger {
            public void log(String... messages) {
                for (String msg : messages) {
                    System.out.println("[LOG] " + msg);
                }
            }
            
            public void log(int priority, String... messages) {
                for (String msg : messages) {
                    System.out.println("[LOG-" + priority + "] " + msg);
                }
            }
        }
        
        Logger logger = new Logger();
        logger.log("System started");
        logger.log("User logged in", "Loading data");
        logger.log(3, "Warning message", "Error message");
    }
}
