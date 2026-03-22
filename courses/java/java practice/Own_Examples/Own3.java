import java.util.Scanner;

// Digital Clock Simulator
public class Own3 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Digital Clock Simulator ===");
        
        // Input hours
        System.out.print("Enter hours (0-23): ");
        int hours = scanner.nextInt();
        
        // Validate hours
        while (hours < 0 || hours > 23) {
            System.out.println("Invalid hours! Hours must be between 0 and 23.");
            System.out.print("Enter hours (0-23): ");
            hours = scanner.nextInt();
        }
        
        // Input minutes
        System.out.print("Enter minutes (0-59): ");
        int minutes = scanner.nextInt();
        
        // Validate minutes
        while (minutes < 0 || minutes > 59) {
            System.out.println("Invalid minutes! Minutes must be between 0 and 59.");
            System.out.print("Enter minutes (0-59): ");
            minutes = scanner.nextInt();
        }
        
        // Input seconds
        System.out.print("Enter seconds (0-59): ");
        int seconds = scanner.nextInt();
        
        // Validate seconds
        while (seconds < 0 || seconds > 59) {
            System.out.println("Invalid seconds! Seconds must be between 0 and 59.");
            System.out.print("Enter seconds (0-59): ");
            seconds = scanner.nextInt();
        }
        
        // Display in HH:MM:SS format with leading zeros
        System.out.println();
        System.out.println("=== Digital Time ===");
        System.out.printf("Time: %02d:%02d:%02d%n", hours, minutes, seconds);
        
        // Determine AM/PM
        String amPm = (hours < 12) ? "AM" : "PM";
        int displayHours = (hours == 0) ? 12 : (hours > 12 ? hours - 12 : hours);
        System.out.printf("Time (12-hour): %02d:%02d:%02d %s%n", displayHours, minutes, seconds, amPm);
        
        scanner.close();
    }
}
