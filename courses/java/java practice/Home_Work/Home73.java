import java.util.Scanner;
public class Home73 {
    public static void main(String[] args) {
        
        //14. Find factorial of any number

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter a number: ");
        int num = scanner.nextInt();
        
        long factorial = 1;
        
        for (int i = 1; i <= num; i++) {
            factorial = factorial * i;
        }
        
        System.out.println("Factorial = " + factorial);
        
        scanner.close();
    }
}
