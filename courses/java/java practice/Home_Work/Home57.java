import java.util.Scanner;
public class Home57 {
    public static void main(String[] args) {
        
        //37. Find determinant of a matrix (3x3)

        Scanner scanner = new Scanner(System.in);
        System.out.println("Enter elements of 3x3 matrix:");
        
        int[][] mat = new int[3][3];
        
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                mat[i][j] = scanner.nextInt();
            }
        }
        
        int determinant = mat[0][0] * (mat[1][1] * mat[2][2] - mat[1][2] * mat[2][1])
                        - mat[0][1] * (mat[1][0] * mat[2][2] - mat[1][2] * mat[2][0])
                        + mat[0][2] * (mat[1][0] * mat[2][1] - mat[1][1] * mat[2][0]);
        
        System.out.println("Determinant = " + determinant);
        
        scanner.close();
    }
}
