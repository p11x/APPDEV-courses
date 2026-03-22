import java.util.Scanner;

// Distance & Speed Calculator
public class Own44 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Distance & Speed Calculator ===");
        System.out.println();
        
        // Show menu of travel modes
        System.out.println("Travel Modes:");
        System.out.println("1. Walking (5 km/h)");
        System.out.println("2. Cycling (15 km/h)");
        System.out.println("3. Car (60 km/h)");
        System.out.println("4. Train (120 km/h)");
        
        // Input distance
        System.out.println();
        System.out.print("Enter distance in km: ");
        double distance = scanner.nextDouble();
        
        // Validate input
        while (distance <= 0) {
            System.out.print("Invalid! Enter a positive distance: ");
            distance = scanner.nextDouble();
        }
        
        // Calculate time for each mode
        double[] speeds = {5, 15, 60, 120};
        String[] modes = {"Walking", "Cycling", "Car", "Train"};
        
        System.out.println();
        System.out.println("=== Time to Travel " + distance + " km ===");
        
        for (int i = 0; i < 4; i++) {
            double hours = distance / speeds[i];
            int totalMinutes = (int)(hours * 60);
            int displayHours = totalMinutes / 60;
            int displayMinutes = totalMinutes % 60;
            
            System.out.println(modes[i] + " (" + speeds[i] + " km/h): " + 
                             displayHours + " hours " + displayMinutes + " minutes");
        }
        
        scanner.close();
    }
}
