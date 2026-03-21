import java.util.Scanner;

// Employee Salary Slip Generator
public class Own18 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Employee Salary Slip Generator ===");
        System.out.println();
        
        // Input employee details
        System.out.print("Enter employee name: ");
        String name = scanner.nextLine();
        
        System.out.print("Enter employee ID: ");
        String empId = scanner.nextLine();
        
        System.out.print("Enter basic salary: Rs. ");
        double basicSalary = scanner.nextDouble();
        
        // Calculate allowances and deductions
        // HRA = 30% of basic, DA = 95% of basic
        double hra = basicSalary * 0.30;
        double da = basicSalary * 0.95;
        
        // PF deduction = 12% of basic
        double pf = basicSalary * 0.12;
        
        // Tax deduction = 10% of basic
        double tax = basicSalary * 0.10;
        
        // Calculate gross salary and net salary
        double grossSalary = basicSalary + hra + da;
        double totalDeductions = pf + tax;
        double netSalary = grossSalary - totalDeductions;
        
        // Display salary slip
        System.out.println();
        System.out.println("==============================================");
        System.out.println("           SALARY SLIP                        ");
        System.out.println("==============================================");
        System.out.println("Employee Name: " + name);
        System.out.println("Employee ID: " + empId);
        System.out.println("----------------------------------------------");
        System.out.println("EARNINGS");
        System.out.println("----------------------------------------------");
        System.out.printf("Basic Salary:      Rs. %.2f%n", basicSalary);
        System.out.printf("HRA (30%%):         Rs. %.2f%n", hra);
        System.out.printf("DA (95%%):         Rs. %.2f%n", da);
        System.out.println("----------------------------------------------");
        System.out.printf("Gross Salary:      Rs. %.2f%n", grossSalary);
        System.out.println("==============================================");
        System.out.println("DEDUCTIONS");
        System.out.println("----------------------------------------------");
        System.out.printf("PF (12%%):          Rs. %.2f%n", pf);
        System.out.printf("Tax (10%%):         Rs. %.2f%n", tax);
        System.out.println("----------------------------------------------");
        System.out.printf("Total Deductions:  Rs. %.2f%n", totalDeductions);
        System.out.println("==============================================");
        System.out.printf("NET SALARY:        Rs. %.2f%n", netSalary);
        System.out.println("==============================================");
        
        scanner.close();
    }
}
