import java.util.Scanner;

// Multiplication Table Grid
public class Own38 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Multiplication Table Grid ===");
        System.out.println();
        
        // Input n
        System.out.print("Enter a number (n) for n x n table: ");
        int n = scanner.nextInt();
        
        // Validate input
        while (n <= 0) {
            System.out.print("Invalid! Enter a positive number: ");
            n = scanner.nextInt();
        }
        
        // Print n x n multiplication table grid with headers
        System.out.println();
        
        // Print header row
        System.out.printf("%5s", "");
        for (int j = 1; j <= n; j++) {
            System.out.printf("%5d", j);
        }
        System.out.println();
        
        // Print separator
        System.out.printf("%5s", "");
        for (int j = 1; j <= n; j++) {
            System.out.printf("%5s", "-----");
        }
        System.out.println();
        
        // Print table rows
        for (int i = 1; i <= n; i++) {
            // Print row header
            System.out.printf("%4d |", i);
            
            // Print multiplication values
            for (int j = 1; j <= n; j++) {
                System.out.printf("%5d", i * j);
            }
            System.out.println();
        }
        
        scanner.close();
    }
}
