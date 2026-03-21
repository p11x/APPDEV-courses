import java.util.Scanner;
public class Home47 {
    public static void main(String[] args) {
        
        //27. Check whether two matrices are equal or not

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter number of rows: ");
        int rows = scanner.nextInt();
        System.out.print("Enter number of columns: ");
        int cols = scanner.nextInt();
        
        int[][] mat1 = new int[rows][cols];
        int[][] mat2 = new int[rows][cols];
        
        System.out.println("Enter elements of first matrix:");
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                mat1[i][j] = scanner.nextInt();
            }
        }
        
        System.out.println("Enter elements of second matrix:");
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                mat2[i][j] = scanner.nextInt();
            }
        }
        
        boolean equal = true;
        
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                if (mat1[i][j] != mat2[i][j]) {
                    equal = false;
                    break;
                }
            }
        }
        
        if (equal) {
            System.out.println("Matrices are equal");
        } else {
            System.out.println("Matrices are not equal");
        }
        
        scanner.close();
    }
}
