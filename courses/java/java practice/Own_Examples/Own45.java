import java.util.Scanner;

// Loan EMI Calculator
public class Own45 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Loan EMI Calculator ===");
        System.out.println();
        
        // Input principal amount
        System.out.print("Enter principal amount (Rs.): ");
        double principal = scanner.nextDouble();
        
        // Input annual interest rate
        System.out.print("Enter annual interest rate (%%): ");
        double annualRate = scanner.nextDouble();
        
        // Input tenure in months
        System.out.print("Enter tenure (months): ");
        int months = scanner.nextInt();
        
        // Validate inputs
        while (principal <= 0 || annualRate <= 0 || months <= 0) {
            System.out.println("Invalid input! All values must be positive.");
            System.out.print("Enter principal amount (Rs.): ");
            principal = scanner.nextDouble();
            System.out.print("Enter annual interest rate (%%): ");
            annualRate = scanner.nextDouble();
            System.out.print("Enter tenure (months): ");
            months = scanner.nextInt();
        }
        
        // Calculate EMI
        // EMI = P * r * (1+r)^n / ((1+r)^n - 1)
        // where r = monthly interest rate = annual rate / 12 / 100
        double monthlyRate = annualRate / 12 / 100;
        double emi;
        
        if (monthlyRate == 0) {
            // No interest case
            emi = principal / months;
        } else {
            double power = Math.pow(1 + monthlyRate, months);
            emi = principal * monthlyRate * power / (power - 1);
        }
        
        // Calculate total payment and interest
        double totalPayment = emi * months;
        double totalInterest = totalPayment - principal;
        
        // Display results
        System.out.println();
        System.out.println("=== Loan Details ===");
        System.out.println("Principal Amount: Rs. " + principal);
        System.out.println("Annual Interest Rate: " + annualRate + "%");
        System.out.println("Tenure: " + months + " months");
        System.out.println();
        System.out.println("=== Results ===");
        System.out.println("Monthly EMI: Rs. " + emi);
        System.out.println("Total Payment: Rs. " + totalPayment);
        System.out.println("Total Interest Paid: Rs. " + totalInterest);
        
        scanner.close();
    }
}
