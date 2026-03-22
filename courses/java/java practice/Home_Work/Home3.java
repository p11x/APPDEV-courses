import java.util.Scanner;
public class Home3 {
    public static void main(String[] args) {

        //4.check if number is divisible by 5 and 11
        Scanner scanner = new Scanner(System.in);
        System.out.println("Enter the number: ");
        int number = scanner.nextInt();

        if (number % 5 == 0 && number % 11 == 0) {
            System.out.println("The number is divisible by 5 and 11.");
        } else {
            System.out.println("The number is not divisible by 5 and 11.");
        }

        
    }
}
