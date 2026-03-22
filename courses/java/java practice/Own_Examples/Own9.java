import java.util.Scanner;

// Magic Square Checker (3x3)
public class Own9 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Magic Square Checker (3x3) ===");
        System.out.println();
        
        // Create 3x3 matrix
        int[][] matrix = new int[3][3];
        
        // Input elements
        System.out.println("Enter 9 numbers for 3x3 matrix:");
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                System.out.print("Element [" + i + "][" + j + "]: ");
                matrix[i][j] = scanner.nextInt();
            }
        }
        
        // Display matrix
        System.out.println();
        System.out.println("Entered Matrix:");
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                System.out.print(matrix[i][j] + "\t");
            }
            System.out.println();
        }
        
        // Check if it's a Magic Square
        // All rows, columns, and both diagonals should sum to the same value
        int magicSum = matrix[0][0] + matrix[0][1] + matrix[0][2];
        boolean isMagic = true;
        
        // Check rows
        for (int i = 0; i < 3; i++) {
            int rowSum = matrix[i][0] + matrix[i][1] + matrix[i][2];
            if (rowSum != magicSum) {
                isMagic = false;
                break;
            }
        }
        
        // Check columns
        if (isMagic) {
            for (int j = 0; j < 3; j++) {
                int colSum = matrix[0][j] + matrix[1][j] + matrix[2][j];
                if (colSum != magicSum) {
                    isMagic = false;
                    break;
                }
            }
        }
        
        // Check main diagonal
        if (isMagic) {
            int diagSum = matrix[0][0] + matrix[1][1] + matrix[2][2];
            if (diagSum != magicSum) {
                isMagic = false;
            }
        }
        
        // Check secondary diagonal
        if (isMagic) {
            int diagSum = matrix[0][2] + matrix[1][1] + matrix[2][0];
            if (diagSum != magicSum) {
                isMagic = false;
            }
        }
        
        // Display result
        System.out.println();
        if (isMagic) {
            System.out.println("It is a MAGIC SQUARE!");
            System.out.println("Magic Sum: " + magicSum);
        } else {
            System.out.println("It is NOT a Magic Square.");
        }
        
        scanner.close();
    }
}
