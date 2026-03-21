import java.util.Scanner;

// Rock Paper Scissors Game
public class Own6 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Rock Paper Scissors Game ===");
        System.out.println("Rules: Rock beats Scissors, Scissors beats Paper, Paper beats Rock");
        System.out.println();
        
        // Get user choice
        System.out.println("Enter your choice:");
        System.out.println("1. Rock");
        System.out.println("2. Paper");
        System.out.println("3. Scissors");
        System.out.print("Enter your choice (1-3): ");
        
        int userChoice = scanner.nextInt();
        String userMove = "";
        
        // Validate and assign user choice
        while (userChoice < 1 || userChoice > 3) {
            System.out.print("Invalid choice! Enter 1, 2, or 3: ");
            userChoice = scanner.nextInt();
        }
        
        if (userChoice == 1) {
            userMove = "Rock";
        } else if (userChoice == 2) {
            userMove = "Paper";
        } else {
            userMove = "Scissors";
        }
        
        // Computer choice using a fixed pattern (based on user input for demonstration)
        // In real game, this would be random
        int computerChoice = (userChoice % 3) + 1;
        String computerMove = "";
        
        if (computerChoice == 1) {
            computerMove = "Rock";
        } else if (computerChoice == 2) {
            computerMove = "Paper";
        } else {
            computerMove = "Scissors";
        }
        
        // Display choices
        System.out.println();
        System.out.println("Your choice: " + userMove);
        System.out.println("Computer choice: " + computerMove);
        System.out.println();
        
        // Determine winner using if-else logic
        if (userChoice == computerChoice) {
            System.out.println("Result: It's a Tie!");
        } else if ((userChoice == 1 && computerChoice == 3) ||
                   (userChoice == 2 && computerChoice == 1) ||
                   (userChoice == 3 && computerChoice == 2)) {
            System.out.println("Result: You Win! " + userMove + " beats " + computerMove);
        } else {
            System.out.println("Result: Computer Wins! " + computerMove + " beats " + userMove);
        }
        
        scanner.close();
    }
}
