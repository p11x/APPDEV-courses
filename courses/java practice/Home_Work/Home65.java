import java.util.Scanner;
public class Home65 {
    public static void main(String[] args) {
        
        //6. Find sum of even numbers between 1 to n

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter a number: ");
        int n = scanner.nextInt();
        
        int sum = 0;
        
        for (int i = 2; i <= n; i += 2) {
            sum = sum + i;
        }
        
        System.out.println("Sum of even numbers = " + sum);
        
        scanner.close();
    }
}
