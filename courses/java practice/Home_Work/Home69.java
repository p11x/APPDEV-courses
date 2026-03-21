import java.util.Scanner;
public class Home69 {
    public static void main(String[] args) {
        
        //10. Find first and last digit of any number

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter a number: ");
        int num = scanner.nextInt();
        
        int lastDigit = num % 10;
        int firstDigit = num;
        
        while (firstDigit >= 10) {
            firstDigit = firstDigit / 10;
        }
        
        System.out.println("First digit = " + firstDigit);
        System.out.println("Last digit = " + lastDigit);
        
        scanner.close();
    }
}
