import java.util.Scanner;
public class Home55 {
    public static void main(String[] args) {
        
        //35. Find sum of lower triangular matrix

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
        
        int sum = 0;
        
        for (int i = 1; i < size; i++) {
            for (int j = 0; j < i; j++) {
                sum = sum + mat[i][j];
            }
        }
        
        System.out.println("Sum of lower triangular matrix = " + sum);
        
        scanner.close();
    }
}
