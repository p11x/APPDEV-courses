import java.util.Scanner;

// Electricity Bill Calculator (Advanced)
public class Own15 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Electricity Bill Calculator (Advanced) ===");
        System.out.println();
        
        // Input customer details
        System.out.print("Enter customer name: ");
        String customerName = scanner.nextLine();
        
        System.out.print("Enter unit number: ");
        String unitNumber = scanner.nextLine();
        
        System.out.print("Enter units consumed: ");
        int units = scanner.nextInt();
        
        // Fixed meter charge
        double meterCharge = 100.0;
        
        // Calculate electricity charge based on tiered pricing
        double electricityCharge = 0;
        
        if (units <= 50) {
            // First 50 units: Rs. 0.50/unit
            electricityCharge = units * 0.50;
        } else if (units <= 150) {
            // Next 100 units (51-150): Rs. 0.75/unit
            electricityCharge = (50 * 0.50) + ((units - 50) * 0.75);
        } else if (units <= 250) {
            // Next 100 units (151-250): Rs. 1.20/unit
            electricityCharge = (50 * 0.50) + (100 * 0.75) + ((units - 150) * 1.20);
        } else {
            // Above 250 units: Rs. 1.50/unit
            electricityCharge = (50 * 0.50) + (100 * 0.75) + (100 * 1.20) + ((units - 250) * 1.50);
        }
        
        // Calculate subtotal (meter charge + electricity charge)
        double subtotal = meterCharge + electricityCharge;
        
        // Add 20% surcharge
        double surcharge = subtotal * 0.20;
        
        // Calculate total bill
        double totalBill = subtotal + surcharge;
        
        // Display formatted bill
        System.out.println();
        System.out.println("==============================================");
        System.out.println("           ELECTRICITY BILL                   ");
        System.out.println("==============================================");
        System.out.println("Customer Name: " + customerName);
        System.out.println("Unit Number: " + unitNumber);
        System.out.println("Units Consumed: " + units);
        System.out.println("----------------------------------------------");
        System.out.printf("Meter Charge:        Rs. %.2f%n", meterCharge);
        System.out.printf("Electricity Charge:  Rs. %.2f%n", electricityCharge);
        System.out.println("----------------------------------------------");
        System.out.printf("Subtotal:            Rs. %.2f%n", subtotal);
        System.out.printf("Surcharge (20%%):     Rs. %.2f%n", surcharge);
        System.out.println("==============================================");
        System.out.printf("TOTAL BILL:          Rs. %.2f%n", totalBill);
        System.out.println("==============================================");
        
        scanner.close();
    }
}
