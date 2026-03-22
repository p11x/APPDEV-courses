import java.util.Scanner;
public class Home66 {
    public static void main(String[] args) {
        
        //7. Find sum of odd numbers between 1 to n

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter a number: ");
        int n = scanner.nextInt();
        
        int sum = 0;
        
        for (int i = 1; i <= n; i += 2) {
            sum = sum + i;
        }
        
        System.out.println("Sum of odd numbers = " + sum);
        
        scanner.close();
    }
}
