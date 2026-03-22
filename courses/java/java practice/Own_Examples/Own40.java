import java.util.Scanner;

// Leap Year Finder in a Range
public class Own40 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Leap Year Finder in a Range ===");
        System.out.println();
        
        // Input start and end year
        System.out.print("Enter start year: ");
        int startYear = scanner.nextInt();
        
        System.out.print("Enter end year: ");
        int endYear = scanner.nextInt();
        
        // Validate input
        while (startYear > endYear || startYear < 1) {
            System.out.println("Invalid range! Start year must be less than end year.");
            System.out.print("Enter start year: ");
            startYear = scanner.nextInt();
            System.out.print("Enter end year: ");
            endYear = scanner.nextInt();
        }
        
        // Find leap years
        System.out.println();
        System.out.println("Leap years between " + startYear + " and " + endYear + ":");
        
        int leapYearCount = 0;
        
        for (int year = startYear; year <= endYear; year++) {
            // A year is a leap year if:
            // - divisible by 4 AND not divisible by 100
            // - OR divisible by 400
            if ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0)) {
                System.out.print(year + " ");
                leapYearCount++;
            }
        }
        
        System.out.println();
        System.out.println("Total leap years found: " + leapYearCount);
        
        // Find century years that are NOT leap years
        System.out.println();
        System.out.println("Century years that are NOT leap years:");
        
        int centuryCount = 0;
        
        for (int year = startYear; year <= endYear; year++) {
            // Century year: divisible by 100 but not by 400
            if (year % 100 == 0 && year % 400 != 0) {
                System.out.print(year + " ");
                centuryCount++;
            }
        }
        
        if (centuryCount == 0) {
            System.out.println("None");
        } else {
            System.out.println();
            System.out.println("Total century years (not leap): " + centuryCount);
        }
        
        scanner.close();
    }
}
