import java.util.Scanner;
public class Home4 {
    public static void main(String[] args) {


        //5.check if number is even or odd

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter the number: ");
        int number = scanner.nextInt();
        if (number % 2 == 0) {
            System.out.println("The number is even");
        } else {
            System.out.println("The number is odd");
        }

        
    }
}
