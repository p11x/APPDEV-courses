
import java.util.Scanner;

public class Example5 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        System.out.println("Enter the number: ");
        int n = scanner.nextInt();
        
        int i = 1;
        int sum = 0;

        while (i<=n) {
            sum += i;
            i++;
        }
        System.out.println("The sum of numbers from 1 to " + n + " is: " + sum);
        
        scanner.close();
   
    }
}
