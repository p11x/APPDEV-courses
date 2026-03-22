import java.util.Scanner;
public class Home51 {
    public static void main(String[] args) {
        
        //31. Interchange diagonals of a matrix

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of square matrix: ");
        int size = scanner.nextInt();
        
        int[][] mat = new int[size][size];
        
        System.out.println("Enter elements of matrix:");
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                mat[i][j] = scanner.nextInt();
            }
        }
        
        for (int i = 0; i < size; i++) {
            int temp = mat[i][i];
            mat[i][i] = mat[i][size - 1 - i];
            mat[i][size - 1 - i] = temp;
        }
        
        System.out.println("Matrix after interchange of diagonals:");
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                System.out.print(mat[i][j] + " ");
            }
            System.out.println();
        }
        
        scanner.close();
    }
}
