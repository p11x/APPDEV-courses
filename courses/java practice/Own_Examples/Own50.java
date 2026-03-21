import java.util.Scanner;

// Cricket Score Tracker
public class Own50 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Cricket Score Tracker ===");
        System.out.println();
        
        // Input number of overs (max 5)
        System.out.print("Enter number of overs (max 5): ");
        int overs = scanner.nextInt();
        
        while (overs <= 0 || overs > 5) {
            System.out.print("Invalid! Enter 1-5: ");
            overs = scanner.nextInt();
        }
        
        // Arrays to store runs and wickets per over
        int[] runsPerOver = new int[overs];
        int[] wicketsPerOver = new int[overs];
        
        // Input for each over
        for (int i = 0; i < overs; i++) {
            System.out.println("Over " + (i + 1) + ":");
            
            System.out.print("  Runs scored (0-36): ");
            runsPerOver[i] = scanner.nextInt();
            
            while (runsPerOver[i] < 0 || runsPerOver[i] > 36) {
                System.out.print("  Invalid! Enter 0-36: ");
                runsPerOver[i] = scanner.nextInt();
            }
            
            System.out.print("  Wickets fallen (0-2): ");
            wicketsPerOver[i] = scanner.nextInt();
            
            while (wicketsPerOver[i] < 0 || wicketsPerOver[i] > 2) {
                System.out.print("  Invalid! Enter 0-2: ");
                wicketsPerOver[i] = scanner.nextInt();
            }
        }
        
        // Calculate totals
        int totalRuns = 0;
        int totalWickets = 0;
        
        for (int i = 0; i < overs; i++) {
            totalRuns += runsPerOver[i];
            totalWickets += wicketsPerOver[i];
        }
        
        // Calculate run rate
        double runRate = totalRuns / (double) overs;
        
        // Find highest and lowest scoring over
        int highestOver = 0;
        int lowestOver = 0;
        
        for (int i = 1; i < overs; i++) {
            if (runsPerOver[i] > runsPerOver[highestOver]) {
                highestOver = i;
            }
            if (runsPerOver[i] < runsPerOver[lowestOver]) {
                lowestOver = i;
            }
        }
        
        // Find best and worst over
        int bestOver = runsPerOver[0];
        int worstOver = runsPerOver[0];
        
        for (int i = 1; i < overs; i++) {
            if (runsPerOver[i] > bestOver) {
                bestOver = runsPerOver[i];
            }
            if (runsPerOver[i] < worstOver) {
                worstOver = runsPerOver[i];
            }
        }
        
        // Display results
        System.out.println();
        System.out.println("=== Match Summary ===");
        System.out.println("Total Runs: " + totalRuns);
        System.out.println("Total Wickets: " + totalWickets);
        System.out.println("Run Rate: " + runRate);
        System.out.println("Highest Scoring Over: Over " + (highestOver + 1) + " (" + runsPerOver[highestOver] + " runs)");
        System.out.println("Best Over: " + bestOver + " runs");
        System.out.println("Worst Over: " + worstOver + " runs");
        
        scanner.close();
    }
}
