import java.util.Scanner;
public class Home81 {
    public static void main(String[] args) {
        
        //22. Find LCM of two numbers

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter first number: ");
        int a = scanner.nextInt();
        System.out.print("Enter second number: ");
        int b = scanner.nextInt();
        
        int hcf = a;
        int temp = b;
        
        // Calculate HCF using Euclidean algorithm
        while (hcf != temp) {
            if (hcf > temp) {
                hcf = hcf - temp;
            } else {
                temp = temp - hcf;
            }
        }
        
        // LCM = (a * b) / HCF
        int lcm = (a * b) / hcf;
        
        System.out.println("LCM = " + lcm);
        
        scanner.close();
    }
}
