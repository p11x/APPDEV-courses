import java.util.Scanner;
public class Home64 {
    public static void main(String[] args) {
        
        //5. Print sum of digits entered by user

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter a number: ");
        int num = scanner.nextInt();
        
        int sum = 0;
        int temp = num;
        
        while (temp > 0) {
            int digit = temp % 10;
            sum = sum + digit;
            temp = temp / 10;
        }
        
        System.out.println("Sum of digits = " + sum);
        
        scanner.close();
    }
}
