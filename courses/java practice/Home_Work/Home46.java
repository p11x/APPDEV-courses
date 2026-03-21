import java.util.Scanner;
public class Home46 {
    public static void main(String[] args) {
        
        //26. Multiply two matrices

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter number of rows of first matrix: ");
        int r1 = scanner.nextInt();
        System.out.print("Enter number of columns of first matrix: ");
        int c1 = scanner.nextInt();
        
        int[][] mat1 = new int[r1][c1];
        
        System.out.println("Enter elements of first matrix:");
        for (int i = 0; i < r1; i++) {
            for (int j = 0; j < c1; j++) {
                mat1[i][j] = scanner.nextInt();
            }
        }
        
        System.out.print("Enter number of rows of second matrix: ");
        int r2 = scanner.nextInt();
        System.out.print("Enter number of columns of second matrix: ");
        int c2 = scanner.nextInt();
        
        if (c1 != r2) {
            System.out.println("Matrix multiplication not possible");
            scanner.close();
            return;
        }
        
        int[][] mat2 = new int[r2][c2];
        
        System.out.println("Enter elements of second matrix:");
        for (int i = 0; i < r2; i++) {
            for (int j = 0; j < c2; j++) {
                mat2[i][j] = scanner.nextInt();
            }
        }
        
        int[][] result = new int[r1][c2];
        
        for (int i = 0; i < r1; i++) {
            for (int j = 0; j < c2; j++) {
                result[i][j] = 0;
                for (int k = 0; k < c1; k++) {
                    result[i][j] = result[i][j] + mat1[i][k] * mat2[k][j];
                }
            }
        }
        
        System.out.println("Product of two matrices:");
        for (int i = 0; i < r1; i++) {
            for (int j = 0; j < c2; j++) {
                System.out.print(result[i][j] + " ");
            }
            System.out.println();
        }
        
        scanner.close();
    }
}
