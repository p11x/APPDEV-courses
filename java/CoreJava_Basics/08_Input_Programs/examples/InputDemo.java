// InputDemo - Java Input Methods
// Different ways to get user input in Java

import java.io.*;

public class InputDemo {
    
    public static void main(String[] args) throws IOException {
        System.out.println("=== JAVA INPUT METHODS ===\n");
        
        // Method 1: Command Line Arguments
        System.out.println("--- 1. Command Line Args ---");
        System.out.println("Pass args when running: java Program arg1 arg2");
        System.out.println("Access via: args[0], args[1]");
        
        // Method 2: BufferedReader (old way)
        System.out.println("\n--- 2. BufferedReader ---");
        System.out.println("BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));");
        System.out.println("String input = reader.readLine();");
        
        // Method 3: Scanner (modern way)
        System.out.println("\n--- 3. Scanner Class ---");
        System.out.println("Scanner scanner = new Scanner(System.in);");
        System.out.println("String name = scanner.nextLine();");
        System.out.println("int number = scanner.nextInt();");
        System.out.println("double value = scanner.nextDouble();");
        
        // Method 4: Console class
        System.out.println("\n--- 4. Console Class ---");
        System.out.println("Console console = System.console();");
        System.out.println("String input = console.readLine(\"Enter name: \");");
        
        // Method 5: GUI Dialogs (for desktop)
        System.out.println("\n--- 5. JOptionPane (GUI) ---");
        System.out.println("String input = JOptionPane.showInputDialog(\"Enter name\");");
        
        System.out.println("\n--- Scanner Example ---");
        java.util.Scanner scan = new java.util.Scanner("John 25 50000.50");
        
        String name = scan.next();
        int age = scan.nextInt();
        double salary = scan.nextDouble();
        
        System.out.println("Name: " + name);
        System.out.println("Age: " + age);
        System.out.println("Salary: $" + salary);
        
        scan.close();
        
        // Angular Note
        System.out.println("\n--- Angular Note ---");
        System.out.println("In web apps, use HTML forms and Angular inputs");
        System.out.println("Angular: <input [(ngModel)]=\"variable\">");
        System.out.println("Java backend receives via @RequestBody");
    }
}
