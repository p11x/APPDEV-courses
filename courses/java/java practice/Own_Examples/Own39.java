import java.util.Scanner;

// Simple Voting System
public class Own39 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Simple Voting System ===");
        System.out.println();
        
        // Declare 3 candidates
        String[] candidates = {"Alice", "Bob", "Charlie"};
        int[] votes = {0, 0, 0};
        
        // Input number of voters
        System.out.print("Enter number of voters: ");
        int numVoters = scanner.nextInt();
        
        // Validate input
        while (numVoters <= 0) {
            System.out.print("Invalid! Enter positive number: ");
            numVoters = scanner.nextInt();
        }
        
        // Each voter votes
        System.out.println();
        System.out.println("Candidates:");
        for (int i = 0; i < candidates.length; i++) {
            System.out.println((i + 1) + ". " + candidates[i]);
        }
        
        System.out.println();
        
        for (int i = 1; i <= numVoters; i++) {
            System.out.print("Voter " + i + " - Enter your choice (1, 2, or 3): ");
            int choice = scanner.nextInt();
            
            if (choice >= 1 && choice <= 3) {
                votes[choice - 1]++;
                System.out.println("Vote recorded for " + candidates[choice - 1]);
            } else {
                System.out.println("Invalid vote! Vote discarded.");
            }
        }
        
        // Display results
        System.out.println();
        System.out.println("=== Voting Results ===");
        for (int i = 0; i < candidates.length; i++) {
            System.out.println(candidates[i] + ": " + votes[i] + " votes");
        }
        
        // Find winner
        int maxVotes = 0;
        int winnerIndex = 0;
        for (int i = 0; i < candidates.length; i++) {
            if (votes[i] > maxVotes) {
                maxVotes = votes[i];
                winnerIndex = i;
            }
        }
        
        // Check for tie
        int tieCount = 0;
        for (int i = 0; i < candidates.length; i++) {
            if (votes[i] == maxVotes) {
                tieCount++;
            }
        }
        
        System.out.println();
        if (tieCount > 1) {
            System.out.println("Result: It's a TIE between:");
            for (int i = 0; i < candidates.length; i++) {
                if (votes[i] == maxVotes) {
                    System.out.println("  - " + candidates[i]);
                }
            }
        } else {
            System.out.println("Winner: " + candidates[winnerIndex] + " with " + maxVotes + " votes!");
        }
        
        scanner.close();
    }
}
