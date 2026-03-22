import java.util.Scanner;
public class Home63 {
    public static void main(String[] args) {
        
        //4. Print all natural numbers in reverse order

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter a number: ");
        int n = scanner.nextInt();
        
        for (int i = n; i >= 1; i--) {
            System.out.println(i);
        }
        
        scanner.close();
    }
}
