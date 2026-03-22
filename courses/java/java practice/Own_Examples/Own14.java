import java.util.Scanner;

// Simple Voting Eligibility Checker
public class Own14 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Simple Voting Eligibility Checker ===");
        System.out.println();
        
        // Input person details
        System.out.print("Enter name: ");
        String name = scanner.nextLine();
        
        System.out.print("Enter age: ");
        int age = scanner.nextInt();
        
        // Consume newline
        scanner.nextLine();
        
        System.out.print("Enter nationality: ");
        String nationality = scanner.nextLine();
        
        // Check eligibility
        // Eligibility: age >= 18 and nationality is Indian
        boolean isAgeEligible = (age >= 18);
        boolean isNationalityEligible = nationality.equalsIgnoreCase("Indian");
        
        // Display result
        System.out.println();
        System.out.println("=== Eligibility Status ===");
        System.out.println("Name: " + name);
        System.out.println("Age: " + age);
        System.out.println("Nationality: " + nationality);
        
        if (isAgeEligible && isNationalityEligible) {
            System.out.println();
            System.out.println("Congratulations " + name + "!");
            System.out.println("You are ELIGIBLE to vote in India.");
        } else {
            System.out.println();
            System.out.println("Sorry " + name + "!");
            System.out.println("You are NOT ELIGIBLE to vote.");
            
            if (!isAgeEligible) {
                System.out.println("Reason: You must be at least 18 years old.");
            }
            if (!isNationalityEligible) {
                System.out.println("Reason: Only Indian citizens can vote.");
            }
        }
        
        scanner.close();
    }
}
