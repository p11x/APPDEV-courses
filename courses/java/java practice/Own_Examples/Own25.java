import java.util.Scanner;

// Matrix Operations Menu
public class Own25 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        // Create 3x3 matrix
        int[][] matrix = new int[3][3];
        
        // Input matrix elements
        System.out.println("=== Matrix Operations Menu ===");
        System.out.println();
        System.out.println("Enter elements for 3x3 matrix:");
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                System.out.print("Element [" + i + "][" + j + "]: ");
                matrix[i][j] = scanner.nextInt();
            }
        }
        
        int choice;
        
        // Menu loop
        do {
            System.out.println();
            System.out.println("===== Matrix Operations Menu =====");
            System.out.println("1. Display Matrix");
            System.out.println("2. Transpose");
            System.out.println("3. Row sums and Column sums");
            System.out.println("4. Check if Identity Matrix");
            System.out.println("5. Exit");
            System.out.print("Enter your choice: ");
            
            choice = scanner.nextInt();
            System.out.println();
            
            switch (choice) {
                case 1:
                    // Display Matrix
                    System.out.println("Matrix:");
                    for (int i = 0; i < 3; i++) {
                        for (int j = 0; j < 3; j++) {
                            System.out.print(matrix[i][j] + "\t");
                        }
                        System.out.println();
                    }
                    break;
                    
                case 2:
                    // Transpose
                    System.out.println("Transpose of Matrix:");
                    for (int i = 0; i < 3; i++) {
                        for (int j = 0; j < 3; j++) {
                            System.out.print(matrix[j][i] + "\t");
                        }
                        System.out.println();
                    }
                    break;
                    
                case 3:
                    // Row sums and Column sums
                    System.out.println("Row sums:");
                    for (int i = 0; i < 3; i++) {
                        int rowSum = matrix[i][0] + matrix[i][1] + matrix[i][2];
                        System.out.println("Row " + (i + 1) + ": " + rowSum);
                    }
                    
                    System.out.println("Column sums:");
                    for (int j = 0; j < 3; j++) {
                        int colSum = matrix[0][j] + matrix[1][j] + matrix[2][j];
                        System.out.println("Column " + (j + 1) + ": " + colSum);
                    }
                    break;
                    
                case 4:
                    // Check if Identity Matrix
                    boolean isIdentity = true;
                    for (int i = 0; i < 3; i++) {
                        for (int j = 0; j < 3; j++) {
                            if (i == j) {
                                if (matrix[i][j] != 1) {
                                    isIdentity = false;
                                    break;
                                }
                            } else {
                                if (matrix[i][j] != 0) {
                                    isIdentity = false;
                                    break;
                                }
                            }
                        }
                    }
                    
                    if (isIdentity) {
                        System.out.println("It is an Identity Matrix!");
                    } else {
                        System.out.println("It is NOT an Identity Matrix.");
                    }
                    break;
                    
                case 5:
                    System.out.println("Exiting... Thank you!");
                    break;
                    
                default:
                    System.out.println("Invalid choice! Please enter 1-5.");
            }
            
        } while (choice != 5);
        
        scanner.close();
    }
}
