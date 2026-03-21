import java.util.Scanner;
public class Home78 {
    public static void main(String[] args) {
        
        //19. Check whether a number is palindrome or not

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter a number: ");
        int num = scanner.nextInt();
        
        int temp = num;
        int reverse = 0;
        
        while (temp > 0) {
            int digit = temp % 10;
            reverse = reverse * 10 + digit;
            temp = temp / 10;
        }
        
        if (reverse == num) {
            System.out.println(num + " is a palindrome number");
        } else {
            System.out.println(num + " is not a palindrome number");
        }
        
        scanner.close();
    }
}
