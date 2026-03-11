// ControlFlowDemo - Demonstrates Java Control Flow Statements
// If-else, switch, loops, and branching statements

public class ControlFlowDemo {
    
    public static void main(String[] args) {
        System.out.println("=== CONTROL FLOW DEMO ===\n");
        
        // IF-ELSE
        System.out.println("--- If-Else ---");
        int age = 20;
        if (age >= 18) {
            System.out.println("Adult");
        } else if (age >= 13) {
            System.out.println("Teenager");
        } else {
            System.out.println("Child");
        }
        
        // Ternary operator
        System.out.println("\n--- Ternary Operator ---");
        String status = age >= 18 ? "Adult" : "Minor";
        System.out.println("Status: " + status);
        
        // SWITCH
        System.out.println("\n--- Switch Statement ---");
        int day = 3;
        String dayName;
        switch (day) {
            case 1: dayName = "Monday"; break;
            case 2: dayName = "Tuesday"; break;
            case 3: dayName = "Wednesday"; break;
            case 4: dayName = "Thursday"; break;
            case 5: dayName = "Friday"; break;
            case 6: dayName = "Saturday"; break;
            case 7: dayName = "Sunday"; break;
            default: dayName = "Invalid";
        }
        System.out.println("Day: " + dayName);
        
        // Switch expression (Java 14+)
        System.out.println("\n--- Switch Expression ---");
        String result = switch (age) {
            case 18 -> "Exactly 18";
            case 19, 20 -> "Young adult";
            default -> "Other age";
        };
        System.out.println(result);
        
        // FOR LOOP
        System.out.println("\n--- For Loop ---");
        for (int i = 1; i <= 5; i++) {
            System.out.print(i + " ");
        }
        System.out.println();
        
        // FOR-EACH
        System.out.println("\n--- For-Each Loop ---");
        String[] fruits = {"Apple", "Banana", "Cherry"};
        for (String fruit : fruits) {
            System.out.println(fruit);
        }
        
        // WHILE
        System.out.println("\n--- While Loop ---");
        int count = 0;
        while (count < 3) {
            System.out.println("Count: " + count);
            count++;
        }
        
        // DO-WHILE
        System.out.println("\n--- Do-While Loop ---");
        int num = 0;
        do {
            System.out.println("Number: " + num);
            num++;
        } while (num < 3);
        
        // BREAK and CONTINUE
        System.out.println("\n--- Break ---");
        for (int i = 1; i <= 10; i++) {
            if (i == 5) break;
            System.out.print(i + " ");
        }
        System.out.println();
        
        System.out.println("\n--- Continue ---");
        for (int i = 1; i <= 5; i++) {
            if (i == 3) continue;
            System.out.print(i + " ");
        }
        System.out.println();
        
        // LABELED BREAK
        System.out.println("\n--- Labeled Break ---");
        outer: for (int i = 1; i <= 3; i++) {
            for (int j = 1; j <= 3; j++) {
                if (i == 2 && j == 2) break outer;
                System.out.println("i=" + i + ", j=" + j);
            }
        }
    }
}
