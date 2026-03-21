import java.util.Scanner;

// Calendar Day Finder
public class Own36 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Calendar Day Finder ===");
        System.out.println();
        
        // Input day, month, and year
        System.out.print("Enter day: ");
        int day = scanner.nextInt();
        
        System.out.print("Enter month (1-12): ");
        int month = scanner.nextInt();
        
        System.out.print("Enter year: ");
        int year = scanner.nextInt();
        
        // Validate inputs
        while (day < 1 || day > 31 || month < 1 || month > 12 || year < 1) {
            System.out.println("Invalid input! Please enter valid date.");
            System.out.print("Enter day: ");
            day = scanner.nextInt();
            System.out.print("Enter month (1-12): ");
            month = scanner.nextInt();
            System.out.print("Enter year: ");
            year = scanner.nextInt();
        }
        
        // Zeller's Congruence to find day of the week
        // Adjust month for calculation (March = 3, ..., December = 12, Jan = 13, Feb = 14 of previous year)
        int m = month;
        int y = year;
        
        if (month == 1) {
            m = 13;
            y = year - 1;
        } else if (month == 2) {
            m = 14;
            y = year - 1;
        }
        
        // Zeller's formula
        int k = y % 100;       // Year of the century
        int j = y / 100;       // Century
        
        // Calculate day of the week
        // h = (q + ((13*(m+1))/5) + k + (k/4) + (j/4) + 5*j) % 7
        // where q = day of month
        int q = day;
        
        int h = (q + (13 * (m + 1)) / 5 + k + k / 4 + j / 4 + 5 * j) % 7;
        
        // Convert to day name
        // h = 0 = Saturday, 1 = Sunday, 2 = Monday, 3 = Tuesday, 4 = Wednesday, 5 = Thursday, 6 = Friday
        String dayName = "";
        
        switch (h) {
            case 0:
                dayName = "Saturday";
                break;
            case 1:
                dayName = "Sunday";
                break;
            case 2:
                dayName = "Monday";
                break;
            case 3:
                dayName = "Tuesday";
                break;
            case 4:
                dayName = "Wednesday";
                break;
            case 5:
                dayName = "Thursday";
                break;
            case 6:
                dayName = "Friday";
                break;
        }
        
        // Display result
        System.out.println();
        System.out.println("Date: " + day + "/" + month + "/" + year);
        System.out.println("Day: " + dayName);
        
        scanner.close();
    }
}
