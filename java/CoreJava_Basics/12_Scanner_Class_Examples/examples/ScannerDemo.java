// ScannerDemo - Demonstrates Java Scanner class for user input
// Scanner is used to read input from console, files, or strings

import java.util.Scanner;

public class ScannerDemo {
    
    public static void main(String[] args) {
        System.out.println("=== SCANNER CLASS DEMO ===\n");
        
        // Create Scanner object for console input
        Scanner scanner = new Scanner(System.in);
        
        // For demo purposes, we'll use predefined input
        // In real programs, uncomment the scanner.nextLine() calls
        
        System.out.println("--- Reading Different Types ---");
        
        // String input
        System.out.print("Enter your name: ");
        String name = "John Doe";  // Simulated input
        // String name = scanner.nextLine();  // Real input
        System.out.println("Name: " + name);
        
        // Integer input
        System.out.print("Enter your age: ");
        int age = 25;  // Simulated
        // int age = scanner.nextInt();
        System.out.println("Age: " + age);
        
        // Double input
        System.out.print("Enter salary: ");
        double salary = 50000.50;  // Simulated
        // double salary = scanner.nextDouble();
        System.out.println("Salary: $" + salary);
        
        // Boolean input
        System.out.print("Are you employed? (true/false): ");
        boolean employed = true;  // Simulated
        // boolean employed = scanner.nextBoolean();
        System.out.println("Employed: " + employed);
        
        // Character input
        System.out.print("Enter first letter: ");
        char grade = 'A';  // Simulated
        // String gradeStr = scanner.next();
        // char grade = gradeStr.charAt(0);
        System.out.println("Grade: " + grade);
        
        // Using Scanner methods
        System.out.println("\n--- Scanner Methods ---");
        String input = "Hello World 123 45.67";
        Scanner stringScanner = new Scanner(input);
        
        System.out.println("next(): " + stringScanner.next());
        System.out.println("next(): " + stringScanner.next());
        System.out.println("nextInt(): " + stringScanner.nextInt());
        System.out.println("nextDouble(): " + stringScanner.nextDouble());
        stringScanner.close();
        
        // Best practices
        System.out.println("\n--- Best Practices ---");
        System.out.println("1. Always close Scanner when done");
        System.out.println("2. Use hasNext() to check for input");
        System.out.println("3. Handle InputMismatchException");
        System.out.println("4. Use BufferedReader for large inputs");
        
        scanner.close();
        
        System.out.println("\n=== Angular Integration Note ===");
        System.out.println("In web apps, use forms instead of Scanner");
        System.out.println("Angular FormsModule provides ngModel for input");
    }
}
