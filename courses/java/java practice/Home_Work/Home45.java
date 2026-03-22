import java.util.Scanner;
public class Home45 {
    public static void main(String[] args) {
        
        //25. Perform Scalar matrix multiplication

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter number of rows: ");
        int rows = scanner.nextInt();
        System.out.print("Enter number of columns: ");
        int cols = scanner.nextInt();
        
        int[][] mat = new int[rows][cols];
        int[][] result = new int[rows][cols];
        
        System.out.println("Enter elements of matrix:");
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                mat[i][j] = scanner.nextInt();
            }
        }
        
        System.out.print("Enter scalar value: ");
        int scalar = scanner.nextInt();
        
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                result[i][j] = mat[i][j] * scalar;
            }
        }
        
        System.out.println("Result after scalar multiplication:");
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                System.out.print(result[i][j] + " ");
            }
            System.out.println();
        }
        
        scanner.close();
    }
}
