import java.util.Scanner;
public class Home59 {
    public static void main(String[] args) {
        
        //39. Check Symmetric matrix

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
        
        int[][] transpose = new int[size][size];
        
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                transpose[i][j] = mat[j][i];
            }
        }
        
        boolean isSymmetric = true;
        
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                if (mat[i][j] != transpose[i][j]) {
                    isSymmetric = false;
                    break;
                }
            }
        }
        
        if (isSymmetric) {
            System.out.println("Symmetric matrix");
        } else {
            System.out.println("Not a Symmetric matrix");
        }
        
        scanner.close();
    }
}
