import java.util.Scanner;
public class Home20 {
    public static void main(String[] args) {
        
        

        //21. Calculate electricity bill

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter electricity units: ");
        int units = scanner.nextInt();
        
        double bill = 0;
        
        if (units <= 50) {
            bill = units * 0.50;
        } else if (units <= 150) {
            bill = (50 * 0.50) + ((units - 50) * 0.75);
        } else if (units <= 250) {
            bill = (50 * 0.50) + (100 * 0.75) + ((units - 150) * 1.20);
        } else {
            bill = (50 * 0.50) + (100 * 0.75) + (100 * 1.20) + ((units - 250) * 1.50);
        }
        
        double surcharge = bill * 0.20;
        double totalBill = bill + surcharge;
        
        System.out.println("Electricity Bill = " + bill);
        System.out.println("Surcharge (20%) = " + surcharge);
        System.out.println("Total Bill = " + totalBill);
        
        scanner.close();
    }
}
