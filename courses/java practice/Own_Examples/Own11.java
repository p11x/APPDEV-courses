import java.util.Scanner;

// Student Report Card Generator
public class Own11 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Student Report Card Generator ===");
        System.out.println();
        
        // Input student details
        System.out.print("Enter student name: ");
        String name = scanner.nextLine();
        
        // Input marks for 5 subjects
        System.out.println("Enter marks for 5 subjects (out of 100):");
        
        System.out.print("Physics: ");
        double physics = scanner.nextDouble();
        
        System.out.print("Chemistry: ");
        double chemistry = scanner.nextDouble();
        
        System.out.print("Biology: ");
        double biology = scanner.nextDouble();
        
        System.out.print("Mathematics: ");
        double mathematics = scanner.nextDouble();
        
        System.out.print("Computer: ");
        double computer = scanner.nextDouble();
        
        // Calculate total
        double total = physics + chemistry + biology + mathematics + computer;
        
        // Calculate percentage
        double percentage = (total / 5);
        
        // Determine grade
        char grade;
        if (percentage >= 90) {
            grade = 'A';
        } else if (percentage >= 80) {
            grade = 'B';
        } else if (percentage >= 70) {
            grade = 'C';
        } else if (percentage >= 60) {
            grade = 'D';
        } else if (percentage >= 40) {
            grade = 'E';
        } else {
            grade = 'F';
        }
        
        // Display report card
        System.out.println();
        System.out.println("=================================");
        System.out.println("       STUDENT REPORT CARD       ");
        System.out.println("=================================");
        System.out.println("Student Name: " + name);
        System.out.println("---------------------------------");
        System.out.println("Subject           Marks");
        System.out.println("---------------------------------");
        System.out.println("Physics           " + physics);
        System.out.println("Chemistry         " + chemistry);
        System.out.println("Biology           " + biology);
        System.out.println("Mathematics       " + mathematics);
        System.out.println("Computer          " + computer);
        System.out.println("---------------------------------");
        System.out.println("Total Marks:      " + total + "/500");
        System.out.println("Percentage:       " + percentage + "%");
        System.out.println("Grade:            " + grade);
        System.out.println("=================================");
        
        scanner.close();
    }
}
