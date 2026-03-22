import java.util.Scanner;

// Number Guessing Game
public class Own1 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        // Fixed secret number
        int secretNumber = 42;
        int guess;
        int attempts = 0;
        
        System.out.println("=== Number Guessing Game ===");
        System.out.println("I have chosen a number between 1 and 100.");
        System.out.println("Try to guess it!");
        System.out.println();
        
        // Loop until user guesses correctly
        do {
            System.out.print("Enter your guess: ");
            guess = scanner.nextInt();
            attempts++;
            
            if (guess > secretNumber) {
                System.out.println("Too High! Try again.");
            } else if (guess < secretNumber) {
                System.out.println("Too Low! Try again.");
            } else {
                System.out.println();
                System.out.println("Correct! You guessed it!");
                System.out.println("Total attempts: " + attempts);
            }
            System.out.println();
            
        } while (guess != secretNumber);
        
        scanner.close();
    }
}
