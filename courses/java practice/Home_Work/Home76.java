import java.util.Scanner;
public class Home76 {
    public static void main(String[] args) {
        
        //17. Calculate Compound Interest

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter Principal amount: ");
        double P = scanner.nextDouble();
        System.out.print("Enter Rate of interest: ");
        double R = scanner.nextDouble();
        System.out.print("Enter Time in years: ");
        double T = scanner.nextDouble();
        
        double CI = P * Math.pow((1 + R / 100), T) - P;
        
        System.out.println("Compound Interest = " + CI);
        
        scanner.close();
    }
}
