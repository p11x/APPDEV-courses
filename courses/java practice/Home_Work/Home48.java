import java.util.Scanner;
public class Home48 {
    public static void main(String[] args) {
        
        //28. Find sum of main diagonal elements of a matrix

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
            sum = sum + mat[i][i];
        }
        
        System.out.println("Sum of main diagonal elements = " + sum);
        
        scanner.close();
    }
}
