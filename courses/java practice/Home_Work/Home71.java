import java.util.Scanner;
public class Home71 {
    public static void main(String[] args) {
        
        //12. Reverse a number using both while loop and for loop

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter a number: ");
        int num = scanner.nextInt();
        
        // Using while loop
        System.out.println("Using while loop:");
        int temp = num;
        int reverse = 0;
        
        while (temp > 0) {
            int digit = temp % 10;
            reverse = reverse * 10 + digit;
            temp = temp / 10;
        }
        
        System.out.println("Reverse = " + reverse);
        
        // Using for loop
        System.out.println("Using for loop:");
        int reverse2 = 0;
        
        for (int t = num; t > 0; t = t / 10) {
            int digit = t % 10;
            reverse2 = reverse2 * 10 + digit;
        }
        
        System.out.println("Reverse = " + reverse2);
        
        scanner.close();
    }
}
