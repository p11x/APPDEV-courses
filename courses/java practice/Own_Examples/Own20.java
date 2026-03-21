import java.util.Scanner;

// Simple Attendance Percentage Calculator
public class Own20 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Attendance Percentage Calculator ===");
        System.out.println();
        
        // Input student details
        System.out.print("Enter student name: ");
        String name = scanner.nextLine();
        
        // Input total classes held
        System.out.print("Enter total number of classes held: ");
        int totalClasses = scanner.nextInt();
        
        // Input classes attended
        System.out.print("Enter number of classes attended: ");
        int classesAttended = scanner.nextInt();
        
        // Validate inputs
        while (totalClasses <= 0 || classesAttended < 0 || classesAttended > totalClasses) {
            System.out.println("Invalid input! Please check your values.");
            System.out.print("Enter total number of classes held: ");
            totalClasses = scanner.nextInt();
            System.out.print("Enter number of classes attended: ");
            classesAttended = scanner.nextInt();
        }
        
        // Calculate attendance percentage
        double attendancePercentage = (classesAttended * 100.0) / totalClasses;
        
        // Display results
        System.out.println();
        System.out.println("=== Attendance Report ===");
        System.out.println("Student Name: " + name);
        System.out.println("Total Classes Held: " + totalClasses);
        System.out.println("Classes Attended: " + classesAttended);
        System.out.printf("Attendance Percentage: %.2f%%%n", attendancePercentage);
        
        // Warning if attendance is less than 75%
        if (attendancePercentage < 75) {
            System.out.println();
            System.out.println("WARNING: Your attendance is below 75%!");
            System.out.println("You may not be eligible to appear for exams.");
            System.out.println("Please attend more classes.");
        } else {
            System.out.println();
            System.out.println("Great! Your attendance is satisfactory.");
            System.out.println("You are eligible to appear for exams.");
        }
        
        scanner.close();
    }
}
