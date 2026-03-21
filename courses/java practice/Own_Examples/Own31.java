import java.util.Scanner;

// Dice Rolling Simulator
public class Own31 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Dice Rolling Simulator ===");
        System.out.println();
        
        // Fixed dice values using array and index cycling
        int[] dice1Values = {3, 1, 6, 2, 5, 4, 1, 3, 6, 2};
        int[] dice2Values = {4, 2, 1, 5, 3, 6, 2, 4, 1, 5};
        
        // Ask user how many times to roll
        System.out.print("How many times do you want to roll the dice? ");
        int rolls = scanner.nextInt();
        
        // Validate input
        while (rolls <= 0) {
            System.out.print("Invalid! Enter a positive number: ");
            rolls = scanner.nextInt();
        }
        
        // Track frequency of each sum (2 to 12)
        int[] frequency = new int[13]; // Index 2 to 12 used
        
        // Roll the dice
        System.out.println();
        System.out.println("=== Rolling Results ===");
        
        for (int i = 0; i < rolls; i++) {
            // Cycle through the arrays using modulo
            int die1 = dice1Values[i % dice1Values.length];
            int die2 = dice2Values[i % dice2Values.length];
            int sum = die1 + die2;
            
            // Track frequency
            frequency[sum]++;
            
            System.out.println("Roll " + (i + 1) + ": Die1 = " + die1 + ", Die2 = " + die2 + ", Sum = " + sum);
        }
        
        // Display frequency
        System.out.println();
        System.out.println("=== Frequency of Each Sum ===");
        for (int sum = 2; sum <= 12; sum++) {
            System.out.println("Sum " + sum + ": " + frequency[sum] + " times");
        }
        
        // Find most common sum
        int maxFreq = 0;
        int mostCommonSum = 0;
        System.out.println();
        System.out.println("=== Statistics ===");
        for (int sum = 2; sum <= 12; sum++) {
            if (frequency[sum] > maxFreq) {
                maxFreq = frequency[sum];
                mostCommonSum = sum;
            }
        }
        System.out.println("Most common sum: " + mostCommonSum + " (appeared " + maxFreq + " times)");
        
        scanner.close();
    }
}
