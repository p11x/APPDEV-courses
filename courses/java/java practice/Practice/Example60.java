/*
 * SUB TOPIC: Command Line Arguments in Java
 * 
 * DEFINITION:
 * Command line arguments are passed to a Java program from the command line during execution.
 * They are stored in the String[] args parameter of the main method and allow users to 
 * provide input without using interactive prompts.
 * 
 * FUNCTIONALITIES:
 * 1. Passing arguments from command line
 * 2. Parsing numeric arguments
 * 3. Multiple arguments
 * 4. Option handling
 * 5. Argument validation
 */

public class Example60 {
    public static void main(String[] args) {
        
        // Topic Explanation: Command Line Arguments
        
        // Print all arguments
        System.out.println("=== Command Line Arguments ===");
        System.out.println("Number of arguments: " + args.length);
        
        for (int i = 0; i < args.length; i++) {
            System.out.println("args[" + i + "]: " + args[i]);
        }
        
        // Parse integer argument
        System.out.println("\n=== Parsing Arguments ===");
        
        if (args.length > 0) {
            try {
                int num = Integer.parseInt(args[0]);
                System.out.println("Parsed number: " + num);
            } catch (NumberFormatException e) {
                System.out.println("Not a valid number");
            }
        }
        
        // Check argument count
        System.out.println("\n=== Argument Count ===");
        
        if (args.length == 0) {
            System.out.println("No arguments provided");
        } else if (args.length == 1) {
            System.out.println("One argument: " + args[0]);
        } else {
            System.out.println("Multiple arguments: " + args.length);
        }
        
        // Real-time Example 1: Add two numbers
        System.out.println("\n=== Example 1: Add Numbers ===");
        
        if (args.length >= 2) {
            try {
                int a = Integer.parseInt(args[0]);
                int b = Integer.parseInt(args[1]);
                System.out.println(a + " + " + b + " = " + (a + b));
            } catch (NumberFormatException e) {
                System.out.println("Please provide valid numbers");
            }
        } else {
            System.out.println("Usage: java Example60 <num1> <num2>");
        }
        
        // Real-time Example 2: Greeting
        System.out.println("\n=== Example 2: Greeting ===");
        
        if (args.length > 0) {
            System.out.println("Hello, " + args[0] + "!");
        } else {
            System.out.println("Hello, Guest!");
        }
        
        // Real-time Example 3: File operation mode
        System.out.println("\n=== Example 3: Mode ===");
        
        String mode = args.length > 0 ? args[0] : "read";
        
        switch (mode) {
            case "read":
                System.out.println("Opening file in read mode");
                break;
            case "write":
                System.out.println("Opening file in write mode");
                break;
            case "append":
                System.out.println("Opening file in append mode");
                break;
            default:
                System.out.println("Unknown mode");
        }
        
        // Real-time Example 4: Verbose flag
        System.out.println("\n=== Example 4: Flags ===");
        
        boolean verbose = false;
        
        for (String arg : args) {
            if (arg.equals("-v") || arg.equals("--verbose")) {
                verbose = true;
            }
        }
        
        System.out.println("Verbose mode: " + verbose);
        
        // Real-time Example 5: Configuration
        System.out.println("\n=== Example 5: Config ===");
        
        String configFile = "config.txt";
        int port = 8080;
        
        for (int i = 0; i < args.length - 1; i++) {
            if (args[i].equals("-f")) {
                configFile = args[i + 1];
            }
            if (args[i].equals("-p")) {
                port = Integer.parseInt(args[i + 1]);
            }
        }
        
        System.out.println("Config file: " + configFile);
        System.out.println("Port: " + port);
        
        // Real-time Example 6: Sum all arguments
        System.out.println("\n=== Example 6: Sum All ===");
        
        int sum = 0;
        for (String arg : args) {
            try {
                sum += Integer.parseInt(arg);
            } catch (NumberFormatException e) {
                // Skip non-numeric
            }
        }
        
        System.out.println("Sum: " + sum);
    }
}
