/*
 * SUB TOPIC: Do-While Loop
 * 
 * DEFINITION:
 * The do-while loop in Java is a control flow statement that executes a block of code at least once,
 * then checks the condition. Unlike while loop, do-while guarantees at least one execution of the loop body.
 * 
 * FUNCTIONALITIES:
 * 1. Executes code at least once before checking condition
 * 2. Post-checks condition after iteration
 * 3. Useful for menu-driven programs
 * 4. Always executes the body at minimum once
 */

import java.util.Scanner;

public class Example6 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        // Topic Explanation with Code: Do-While - Menu Driven Program
        System.out.println("=== Do-While Loop: Menu Driven Program ===");
        int choice;
        
        do {
            System.out.println("\n===== MENU =====");
            System.out.println("1. Add Two Numbers");
            System.out.println("2. Subtract Two Numbers");
            System.out.println("3. Multiply Two Numbers");
            System.out.println("4. Exit");
            System.out.print("Enter your choice: ");
            choice = scanner.nextInt(); // Read user choice
            
            switch(choice) {
                case 1:
                    System.out.print("Enter first number: ");
                    int a = scanner.nextInt();
                    System.out.print("Enter second number: ");
                    int b = scanner.nextInt();
                    System.out.println("Sum: " + (a + b));
                    break;
                case 2:
                    System.out.print("Enter first number: ");
                    int c = scanner.nextInt();
                    System.out.print("Enter second number: ");
                    int d = scanner.nextInt();
                    System.out.println("Difference: " + (c - d));
                    break;
                case 3:
                    System.out.print("Enter first number: ");
                    int e = scanner.nextInt();
                    System.out.print("Enter second number: ");
                    int f = scanner.nextInt();
                    System.out.println("Product: " + (e * f));
                    break;
                case 4:
                    System.out.println("Exiting...");
                    break;
                default:
                    System.out.println("Invalid choice!");
            }
        } while (choice != 4); // Continue while choice is not 4
        
        // Real-time Example 1: Input validation with do-while
        System.out.println("\n=== Input Validation ===");
        int age;
        
        do {
            System.out.print("Enter age (must be 18-100): ");
            age = scanner.nextInt(); // Read age input
            
            if (age < 18 || age > 100) {
                System.out.println("Invalid age! Please try again.");
            }
        } while (age < 18 || age > 100); // Repeat if invalid
        
        System.out.println("Valid age entered: " + age);
        
        // Real-time Example 2: ATM PIN Verification
        System.out.println("\n=== ATM PIN Verification ===");
        int enteredPin;
        int correctPin = 1234;
        
        do {
            System.out.print("Enter your 4-digit PIN: ");
            enteredPin = scanner.nextInt(); // Read PIN
            
            if (enteredPin != correctPin) {
                System.out.println("Incorrect PIN! Please try again.");
            }
        } while (enteredPin != correctPin); // Repeat until correct PIN
        
        System.out.println("PIN Verified Successfully!");
        
        // Real-time Example 3: Display numbers from 1 to n
        System.out.println("\n=== Display Numbers ===");
        System.out.print("Enter a number: ");
        int num = scanner.nextInt(); // Read number
        int displayCount = 1; // Initialize counter
        
        do {
            System.out.print(displayCount + " "); // Print current number
            displayCount++; // Increment counter
        } while (displayCount <= num); // Repeat until count reaches num
        
        // Real-time Example 4: Calculate average of numbers
        System.out.println("\n=== Calculate Average ===");
        System.out.println("Enter numbers to calculate average (enter -1 to stop):");
        double sum = 0; // Initialize sum
        int numCount = 0; // Initialize count
        double number; // Variable for each input
        
        do {
            System.out.print("Enter number: ");
            number = scanner.nextDouble(); // Read number
            if (number != -1) { // Check if not stop value
                sum += number; // Add to sum
                numCount++; // Increment count
            }
        } while (number != -1); // Repeat until -1 is entered
        
        if (numCount > 0) {
            double average = sum / numCount; // Calculate average
            System.out.println("Average: " + average);
        } else {
            System.out.println("No numbers entered.");
        }
        
        // Real-time Example 5: Game menu
        System.out.println("\n=== Game Menu ===");
        int gameChoice;
        
        do {
            System.out.println("\n===== GAME MENU =====");
            System.out.println("1. Start Game");
            System.out.println("2. Load Game");
            System.out.println("3. Settings");
            System.out.println("4. Quit");
            System.out.print("Enter choice: ");
            gameChoice = scanner.nextInt(); // Read game choice
            
            switch(gameChoice) {
                case 1:
                    System.out.println("Starting new game...");
                    break;
                case 2:
                    System.out.println("Loading saved game...");
                    break;
                case 3:
                    System.out.println("Opening settings...");
                    break;
                case 4:
                    System.out.println("Thanks for playing!");
                    break;
                default:
                    System.out.println("Invalid choice!");
            }
        } while (gameChoice != 4);
        
        scanner.close();
    }
}
