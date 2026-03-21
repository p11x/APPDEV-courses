import java.util.Scanner;

// Number Spiral Matrix Generator
public class Own30 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Number Spiral Matrix Generator ===");
        System.out.println();
        
        // Input size (odd number)
        System.out.print("Enter size of matrix (odd number like 3, 5): ");
        int n = scanner.nextInt();
        
        // Validate input (must be odd and positive)
        while (n % 2 == 0 || n <= 0) {
            System.out.print("Invalid! Enter an odd positive number: ");
            n = scanner.nextInt();
        }
        
        // Create n x n matrix
        int[][] spiral = new int[n][n];
        
        // Fill matrix in clockwise spiral order
        int num = 1;
        int startRow = 0, endRow = n - 1;
        int startCol = 0, endCol = n - 1;
        
        while (num <= n * n) {
            // Fill top row (left to right)
            for (int i = startCol; i <= endCol; i++) {
                spiral[startRow][i] = num++;
            }
            startRow++;
            
            // Fill right column (top to bottom)
            for (int i = startRow; i <= endRow; i++) {
                spiral[i][endCol] = num++;
            }
            endCol--;
            
            // Fill bottom row (right to left)
            for (int i = endCol; i >= startCol; i--) {
                spiral[endRow][i] = num++;
            }
            endRow--;
            
            // Fill left column (bottom to top)
            for (int i = endRow; i >= startRow; i--) {
                spiral[i][startCol] = num++;
            }
            startCol++;
        }
        
        // Display spiral matrix
        System.out.println();
        System.out.println("=== Spiral Matrix (" + n + "x" + n + ") ===");
        System.out.println();
        
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                System.out.printf("%4d", spiral[i][j]);
            }
            System.out.println();
        }
        
        scanner.close();
    }
}
