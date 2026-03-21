import java.util.Scanner;

// Student Grade Book
public class Own42 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Student Grade Book ===");
        System.out.println();
        
        // Number of students
        int numStudents = 5;
        
        // Arrays for student data
        String[] names = new String[numStudents];
        int[][] marks = new int[numStudents][3];
        int[] total = new int[numStudents];
        double[] percentage = new double[numStudents];
        char[] grade = new char[numStudents];
        
        // Input student details
        for (int i = 0; i < numStudents; i++) {
            System.out.println("Student " + (i + 1) + ":");
            System.out.print("  Name: ");
            names[i] = scanner.next();
            
            System.out.print("  Marks in Subject 1: ");
            marks[i][0] = scanner.nextInt();
            
            System.out.print("  Marks in Subject 2: ");
            marks[i][1] = scanner.nextInt();
            
            System.out.print("  Marks in Subject 3: ");
            marks[i][2] = scanner.nextInt();
            
            // Calculate total and percentage
            total[i] = marks[i][0] + marks[i][1] + marks[i][2];
            percentage[i] = total[i] / 3.0;
            
            // Determine grade
            if (percentage[i] >= 90) {
                grade[i] = 'A';
            } else if (percentage[i] >= 80) {
                grade[i] = 'B';
            } else if (percentage[i] >= 70) {
                grade[i] = 'C';
            } else if (percentage[i] >= 60) {
                grade[i] = 'D';
            } else if (percentage[i] >= 40) {
                grade[i] = 'E';
            } else {
                grade[i] = 'F';
            }
            
            System.out.println();
        }
        
        // Sort students by percentage (descending) using bubble sort
        for (int i = 0; i < numStudents - 1; i++) {
            for (int j = 0; j < numStudents - i - 1; j++) {
                if (percentage[j] < percentage[j + 1]) {
                    // Swap percentages
                    double tempPercent = percentage[j];
                    percentage[j] = percentage[j + 1];
                    percentage[j + 1] = tempPercent;
                    
                    // Swap totals
                    int tempTotal = total[j];
                    total[j] = total[j + 1];
                    total[j + 1] = tempTotal;
                    
                    // Swap grades
                    char tempGrade = grade[j];
                    grade[j] = grade[j + 1];
                    grade[j + 1] = tempGrade;
                    
                    // Swap marks
                    for (int k = 0; k < 3; k++) {
                        int tempMark = marks[j][k];
                        marks[j][k] = marks[j + 1][k];
                        marks[j + 1][k] = tempMark;
                    }
                    
                    // Swap names
                    String tempName = names[j];
                    names[j] = names[j + 1];
                    names[j + 1] = tempName;
                }
            }
        }
        
        // Display ranked list
        System.out.println("=== Ranked Student List ===");
        System.out.println("Rank | Name     | Total | Percentage | Grade");
        System.out.println("-----|----------|-------|------------|------");
        
        for (int i = 0; i < numStudents; i++) {
            System.out.printf("%4d | %-8s | %5d | %10.2f | %5c%n", 
                           (i + 1), names[i], total[i], percentage[i], grade[i]);
        }
        
        // Calculate class average
        double classTotal = 0;
        int highestIndex = 0;
        int lowestIndex = 0;
        
        for (int i = 0; i < numStudents; i++) {
            classTotal += percentage[i];
            if (percentage[i] > percentage[highestIndex]) {
                highestIndex = i;
            }
            if (percentage[i] < percentage[lowestIndex]) {
                lowestIndex = i;
            }
        }
        
        double classAverage = classTotal / numStudents;
        
        System.out.println();
        System.out.println("=== Class Statistics ===");
        System.out.println("Class Average: " + classAverage + "%");
        System.out.println("Highest Scorer: " + names[highestIndex] + " (" + percentage[highestIndex] + "%)");
        System.out.println("Lowest Scorer: " + names[lowestIndex] + " (" + percentage[lowestIndex] + "%)");
        
        scanner.close();
    }
}
