import java.util.Scanner;
public class Home53 {
    public static void main(String[] args) {
        
        //33. Find lower triangular matrix

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
        
        System.out.println("Lower triangular matrix:");
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                if (i >= j) {
                    System.out.print(mat[i][j] + " ");
                } else {
                    System.out.print("0 ");
                }
            }
            System.out.println();
        }
        
        scanner.close();
    }
}
