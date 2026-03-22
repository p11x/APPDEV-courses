import java.util.Scanner;

// Train Ticket Booking Simulator
public class Own49 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        // Declare 3 trains
        String[] trainNames = {"Express 101", "Superfast 202", "Rajdhani 303"};
        String[] sources = {"Delhi", "Mumbai", "Kolkata"};
        String[] destinations = {"Mumbai", "Bangalore", "Delhi"};
        int[] availableSeats = {50, 30, 40};
        double[] prices = {1500, 2000, 2500};
        
        int choice;
        
        System.out.println("=== Train Ticket Booking Simulator ===");
        System.out.println();
        
        // Menu loop
        do {
            System.out.println("===== Train Menu =====");
            System.out.println("1. View all trains");
            System.out.println("2. Book a ticket");
            System.out.println("3. Cancel a ticket");
            System.out.println("4. Check availability");
            System.out.println("5. Exit");
            System.out.print("Enter your choice: ");
            
            choice = scanner.nextInt();
            System.out.println();
            
            switch (choice) {
                case 1:
                    // View all trains
                    System.out.println("=== Available Trains ===");
                    for (int i = 0; i < 3; i++) {
                        System.out.println("Train " + (i + 1) + ": " + trainNames[i]);
                        System.out.println("  From: " + sources[i] + " To: " + destinations[i]);
                        System.out.println("  Price: Rs. " + prices[i]);
                        System.out.println("  Available Seats: " + availableSeats[i]);
                        System.out.println();
                    }
                    break;
                    
                case 2:
                    // Book a ticket
                    System.out.println("Select train:");
                    for (int i = 0; i < 3; i++) {
                        System.out.println((i + 1) + ". " + trainNames[i] + " (Seats: " + availableSeats[i] + ")");
                    }
                    System.out.print("Enter train choice: ");
                    int bookTrain = scanner.nextInt();
                    
                    if (bookTrain >= 1 && bookTrain <= 3) {
                        int trainIndex = bookTrain - 1;
                        System.out.print("Enter number of seats: ");
                        int seats = scanner.nextInt();
                        
                        if (seats > 0 && seats <= availableSeats[trainIndex]) {
                            availableSeats[trainIndex] -= seats;
                            double totalCost = seats * prices[trainIndex];
                            System.out.println("Booking successful!");
                            System.out.println("Total cost: Rs. " + totalCost);
                            System.out.println("Remaining seats: " + availableSeats[trainIndex]);
                        } else {
                            System.out.println("Invalid seats or insufficient availability!");
                        }
                    } else {
                        System.out.println("Invalid train!");
                    }
                    break;
                    
                case 3:
                    // Cancel a ticket
                    System.out.println("Select train to cancel:");
                    for (int i = 0; i < 3; i++) {
                        System.out.println((i + 1) + ". " + trainNames[i]);
                    }
                    System.out.print("Enter train choice: ");
                    int cancelTrain = scanner.nextInt();
                    
                    if (cancelTrain >= 1 && cancelTrain <= 3) {
                        int trainIndex = cancelTrain - 1;
                        System.out.print("Enter number of seats to cancel: ");
                        int seats = scanner.nextInt();
                        
                        if (seats > 0) {
                            availableSeats[trainIndex] += seats;
                            double refund = seats * prices[trainIndex] * 0.80;
                            System.out.println("Cancellation successful!");
                            System.out.println("Refund (80%): Rs. " + refund);
                            System.out.println("Available seats now: " + availableSeats[trainIndex]);
                        } else {
                            System.out.println("Invalid number of seats!");
                        }
                    } else {
                        System.out.println("Invalid train!");
                    }
                    break;
                    
                case 4:
                    // Check availability
                    System.out.println("=== Train Availability ===");
                    for (int i = 0; i < 3; i++) {
                        System.out.println(trainNames[i] + ": " + availableSeats[i] + " seats available");
                    }
                    break;
                    
                case 5:
                    System.out.println("Thank you for using Train Booking!");
                    break;
                    
                default:
                    System.out.println("Invalid choice!");
            }
            
            System.out.println();
            
        } while (choice != 5);
        
        scanner.close();
    }
}
