import java.util.Scanner;
public class Home18 {
    public static void main(String[] args) {
        
        

        //19. Calculate percentage and grade

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter marks in Physics: ");
        double physics = scanner.nextDouble();
        System.out.print("Enter marks in Chemistry: ");
        double chemistry = scanner.nextDouble();
        System.out.print("Enter marks in Biology: ");
        double biology = scanner.nextDouble();
        System.out.print("Enter marks in Mathematics: ");
        double mathematics = scanner.nextDouble();
        System.out.print("Enter marks in Computer: ");
        double computer = scanner.nextDouble();
        
        double total = physics + chemistry + biology + mathematics + computer;
        double percentage = total / 5;
        
        System.out.println("Percentage = " + percentage + "%");
        
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
        
        System.out.println("Grade = " + grade);
        
        scanner.close();
    }
}
