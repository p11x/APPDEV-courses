import java.util.Scanner;

// Fuel Cost Calculator
public class Own48 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Fuel Cost Calculator ===");
        System.out.println();
        
        // Input distance
        System.out.print("Enter distance to travel (km): ");
        double distance = scanner.nextDouble();
        
        // Input vehicle mileage
        System.out.print("Enter vehicle mileage (km per litre): ");
        double mileage = scanner.nextDouble();
        
        // Input fuel price
        System.out.print("Enter fuel price per litre (Rs.): ");
        double fuelPrice = scanner.nextDouble();
        
        // Validate inputs
        while (distance <= 0 || mileage <= 0 || fuelPrice <= 0) {
            System.out.println("Invalid input! All values must be positive.");
            System.out.print("Enter distance to travel (km): ");
            distance = scanner.nextDouble();
            System.out.print("Enter vehicle mileage (km per litre): ");
            mileage = scanner.nextDouble();
            System.out.print("Enter fuel price per litre (Rs.): ");
            fuelPrice = scanner.nextDouble();
        }
        
        // Calculate fuel needed and cost
        double fuelNeeded = distance / mileage;
        double totalCost = fuelNeeded * fuelPrice;
        
        System.out.println();
        System.out.println("=== Results for Your Vehicle ===");
        System.out.println("Distance: " + distance + " km");
        System.out.println("Mileage: " + mileage + " km/L");
        System.out.println("Fuel Price: Rs. " + fuelPrice + "/L");
        System.out.println("Fuel Needed: " + fuelNeeded + " litres");
        System.out.println("Total Fuel Cost: Rs. " + totalCost);
        
        // Show comparison for 3 vehicle types
        System.out.println();
        System.out.println("=== Comparison for Different Vehicles ===");
        
        String[] vehicles = {"Bike", "Car", "SUV"};
        double[] mileages = {45, 15, 10};
        
        System.out.println("Vehicle    Mileage    Fuel Needed    Total Cost");
        
        for (int i = 0; i < 3; i++) {
            double fuel = distance / mileages[i];
            double cost = fuel * fuelPrice;
            System.out.printf("%-10s %5.0f km/L   %10.2f L    Rs. %.2f%n", 
                           vehicles[i], mileages[i], fuel, cost);
        }
        
        scanner.close();
    }
}
