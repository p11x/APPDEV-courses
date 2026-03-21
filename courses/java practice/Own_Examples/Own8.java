import java.util.Scanner;

// Fibonacci Series Generator
public class Own8 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Fibonacci Series Generator ===");
        System.out.println();
        
        // Get number of terms
        System.out.print("Enter number of terms: ");
        int n = scanner.nextInt();
        
        // Validate input
        while (n <= 0) {
            System.out.print("Invalid! Enter a positive number: ");
            n = scanner.nextInt();
        }
        
        // Generate Fibonacci series
        System.out.println();
        System.out.println("Fibonacci Series:");
        
        int first = 0, second = 1;
        
        for (int i = 1; i <= n; i++) {
            System.out.print(first);
            
            if (i < n) {
                System.out.print(", ");
            }
            
            // Calculate next term
            int next = first + second;
            first = second;
            second = next;
        }
        
        // Print whether each number is even or odd
        System.out.println();
        System.out.println();
        System.out.println("Even/Odd Analysis:");
        
        first = 0;
        second = 1;
        
        for (int i = 1; i <= n; i++) {
            System.out.print(first + " is ");
            
            if (first % 2 == 0) {
                System.out.println("Even");
            } else {
                System.out.println("Odd");
            }
            
            // Calculate next term
            int next = first + second;
            first = second;
            second = next;
        }
        
        scanner.close();
    }
}
