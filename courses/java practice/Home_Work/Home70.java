import java.util.Scanner;
public class Home70 {
    public static void main(String[] args) {
        
        //11. Calculate product of digits of a number

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter a number: ");
        int num = scanner.nextInt();
        
        int product = 1;
        int temp = num;
        
        while (temp > 0) {
            int digit = temp % 10;
            product = product * digit;
            temp = temp / 10;
        }
        
        System.out.println("Product of digits = " + product);
        
        scanner.close();
    }
}
