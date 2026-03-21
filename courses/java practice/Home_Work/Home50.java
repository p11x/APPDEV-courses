import java.util.Scanner;
public class Home50 {
    public static void main(String[] args) {
        
        //30. Find sum of each row and column of a matrix

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter number of rows: ");
        int rows = scanner.nextInt();
        System.out.print("Enter number of columns: ");
        int cols = scanner.nextInt();
        
        int[][] mat = new int[rows][cols];
        
        System.out.println("Enter elements of matrix:");
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                mat[i][j] = scanner.nextInt();
            }
        }
        
        System.out.println("Sum of each row:");
        for (int i = 0; i < rows; i++) {
            int rowSum = 0;
            for (int j = 0; j < cols; j++) {
                rowSum = rowSum + mat[i][j];
            }
            System.out.println("Row " + (i + 1) + " = " + rowSum);
        }
        
        System.out.println("Sum of each column:");
        for (int j = 0; j < cols; j++) {
            int colSum = 0;
            for (int i = 0; i < rows; i++) {
                colSum = colSum + mat[i][j];
            }
            System.out.println("Column " + (j + 1) + " = " + colSum);
        }
        
        scanner.close();
    }
}
