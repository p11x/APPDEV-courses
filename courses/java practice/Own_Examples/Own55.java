import java.util.Scanner;

// Matrix Chain Operations
public class Own55 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Matrix Chain Operations ===");
        System.out.println();
        
        // Input Matrix A
        System.out.println("Enter elements for Matrix A (3x3):");
        int[][] A = new int[3][3];
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                System.out.print("A[" + i + "][" + j + "]: ");
                A[i][j] = scanner.nextInt();
            }
        }
        
        System.out.println();
        
        // Input Matrix B
        System.out.println("Enter elements for Matrix B (3x3):");
        int[][] B = new int[3][3];
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                System.out.print("B[" + i + "][" + j + "]: ");
                B[i][j] = scanner.nextInt();
            }
        }
        
        System.out.println();
        
        // 1. Addition: A + B
        System.out.println("1. A + B:");
        int[][] add = new int[3][3];
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                add[i][j] = A[i][j] + B[i][j];
                System.out.print(add[i][j] + "\t");
            }
            System.out.println();
        }
        
        System.out.println();
        
        // 2. Subtraction: A - B
        System.out.println("2. A - B:");
        int[][] sub = new int[3][3];
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                sub[i][j] = A[i][j] - B[i][j];
                System.out.print(sub[i][j] + "\t");
            }
            System.out.println();
        }
        
        System.out.println();
        
        // 3. Multiplication: A * B
        System.out.println("3. A * B:");
        int[][] mul = new int[3][3];
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                mul[i][j] = 0;
                for (int k = 0; k < 3; k++) {
                    mul[i][j] += A[i][k] * B[k][j];
                }
                System.out.print(mul[i][j] + "\t");
            }
            System.out.println();
        }
        
        System.out.println();
        
        // 4. Transpose of A
        System.out.println("4. Transpose of A:");
        int[][] transA = new int[3][3];
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                transA[i][j] = A[j][i];
                System.out.print(transA[i][j] + "\t");
            }
            System.out.println();
        }
        
        System.out.println();
        
        // 5. Transpose of B
        System.out.println("5. Transpose of B:");
        int[][] transB = new int[3][3];
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                transB[i][j] = B[j][i];
                System.out.print(transB[i][j] + "\t");
            }
            System.out.println();
        }
        
        scanner.close();
    }
}
