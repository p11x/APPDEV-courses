import java.util.Scanner;
public class Home58 {
    public static void main(String[] args) {
        
        //38. Check Identity matrix

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
        
        boolean isIdentity = true;
        
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                if (i == j) {
                    if (mat[i][j] != 1) {
                        isIdentity = false;
                        break;
                    }
                } else {
                    if (mat[i][j] != 0) {
                        isIdentity = false;
                        break;
                    }
                }
            }
        }
        
        if (isIdentity) {
            System.out.println("Identity matrix");
        } else {
            System.out.println("Not an Identity matrix");
        }
        
        scanner.close();
    }
}
