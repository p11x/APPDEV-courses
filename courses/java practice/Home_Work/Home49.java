import java.util.Scanner;
public class Home49 {
    public static void main(String[] args) {
        
        //29. Find sum of minor diagonal elements of a matrix

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
        
        for (int i = 0; i < size; i++) {
            sum = sum + mat[i][size - 1 - i];
        }
        
        System.out.println("Sum of minor diagonal elements = " + sum);
        
        scanner.close();
    }
}
