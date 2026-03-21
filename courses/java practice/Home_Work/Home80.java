import java.util.Scanner;
public class Home80 {
    public static void main(String[] args) {
        
        //21. Find HCF of two numbers

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter first number: ");
        int a = scanner.nextInt();
        System.out.print("Enter second number: ");
        int b = scanner.nextInt();
        
        // Euclidean algorithm
        while (a != b) {
            if (a > b) {
                a = a - b;
            } else {
                b = b - a;
            }
        }
        
        System.out.println("HCF = " + a);
        
        scanner.close();
    }
}
