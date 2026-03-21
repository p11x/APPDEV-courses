import java.util.Scanner;
public class Home74 {
    public static void main(String[] args) {
        
        //15. Check whether a number is Armstrong number or not

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter a number: ");
        int num = scanner.nextInt();
        
        int temp = num;
        int count = 0;
        
        // Count number of digits
        while (temp > 0) {
            count++;
            temp = temp / 10;
        }
        
        temp = num;
        int sum = 0;
        
        // Calculate sum of digits raised to power of number of digits
        while (temp > 0) {
            int digit = temp % 10;
            int power = 1;
            for (int i = 1; i <= count; i++) {
                power = power * digit;
            }
            sum = sum + power;
            temp = temp / 10;
        }
        
        if (sum == num) {
            System.out.println(num + " is an Armstrong number");
        } else {
            System.out.println(num + " is not an Armstrong number");
        }
        
        scanner.close();
    }
}
