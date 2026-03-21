import java.util.Scanner;

// Pattern using Numbers
public class Own24 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Pattern using Numbers ===");
        System.out.println();
        
        // ==================== Pattern 1: Floyd's Triangle ====================
        System.out.println("Pattern 1: Floyd's Triangle");
        int num = 1;
        for (int i = 1; i <= 5; i++) {
            for (int j = 1; j <= i; j++) {
                System.out.print(num + " ");
                num++;
            }
            System.out.println();
        }
        
        System.out.println();
        
        // ==================== Pattern 2: Number Pyramid ====================
        System.out.println("Pattern 2: Number Pyramid");
        for (int i = 1; i <= 5; i++) {
            // Print spaces
            for (int j = 1; j <= 5 - i; j++) {
                System.out.print(" ");
            }
            // Print numbers
            for (int k = 1; k <= i; k++) {
                System.out.print(i + " ");
            }
            System.out.println();
        }
        
        System.out.println();
        
        // ==================== Pattern 3: Pascal's Triangle ====================
        System.out.println("Pattern 3: Pascal's Triangle (first 6 rows)");
        
        for (int i = 0; i < 6; i++) {
            // Print spaces for formatting
            for (int j = 0; j < 6 - i; j++) {
                System.out.print(" ");
            }
            
            // Calculate and print Pascal's triangle values
            for (int j = 0; j <= i; j++) {
                System.out.print(factorial(i) / (factorial(j) * factorial(i - j)) + " ");
            }
            System.out.println();
        }
        
        scanner.close();
    }
    
    // Method to calculate factorial
    public static int factorial(int n) {
        int fact = 1;
        for (int i = 1; i <= n; i++) {
            fact = fact * i;
        }
        return fact;
    }
}
