import java.util.Scanner;
public class Home75 {
    public static void main(String[] args) {
        
        //16. Find all Armstrong numbers between 1 to n

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter a number: ");
        int n = scanner.nextInt();
        
        System.out.println("Armstrong numbers between 1 and " + n + ":");
        
        for (int num = 1; num <= n; num++) {
            int temp = num;
            int count = 0;
            
            // Count number of digits
            while (temp > 0) {
                count++;
                temp = temp / 10;
            }
            
            // Recalculate for actual checking
            temp = num;
            int sum = 0;
            
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
                System.out.println(num);
            }
        }
        
        scanner.close();
    }
}
