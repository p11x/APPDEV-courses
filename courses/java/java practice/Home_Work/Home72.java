import java.util.Scanner;
public class Home72 {
    public static void main(String[] args) {
        
        //13. Calculate power using both while loop and for loop

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter base: ");
        int base = scanner.nextInt();
        System.out.print("Enter exponent: ");
        int exp = scanner.nextInt();
        
        // Using while loop
        System.out.println("Using while loop:");
        int result1 = 1;
        int i = 1;
        
        while (i <= exp) {
            result1 = result1 * base;
            i++;
        }
        
        System.out.println(base + "^" + exp + " = " + result1);
        
        // Using for loop
        System.out.println("Using for loop:");
        int result2 = 1;
        
        for (int j = 1; j <= exp; j++) {
            result2 = result2 * base;
        }
        
        System.out.println(base + "^" + exp + " = " + result2);
        
        scanner.close();
    }
}
