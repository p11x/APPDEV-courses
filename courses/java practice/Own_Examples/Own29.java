import java.util.Scanner;

// Anagram Checker
public class Own29 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Anagram Checker ===");
        System.out.println();
        
        // Input two words
        System.out.print("Enter first word: ");
        String word1 = scanner.next();
        
        System.out.print("Enter second word: ");
        String word2 = scanner.next();
        
        // Convert to lowercase for comparison
        word1 = word1.toLowerCase();
        word2 = word2.toLowerCase();
        
        // Check if lengths are equal
        if (word1.length() != word2.length()) {
            System.out.println();
            System.out.println(word1 + " and " + word2 + " are NOT anagrams.");
            System.out.println("Reason: Different lengths.");
        } else {
            // Use character frequency counting with array of size 26
            int[] freq1 = new int[26];
            int[] freq2 = new int[26];
            
            // Count frequency of each character in word1
            for (int i = 0; i < word1.length(); i++) {
                char ch = word1.charAt(i);
                freq1[ch - 'a']++;
            }
            
            // Count frequency of each character in word2
            for (int i = 0; i < word2.length(); i++) {
                char ch = word2.charAt(i);
                freq2[ch - 'a']++;
            }
            
            // Compare frequencies
            boolean isAnagram = true;
            for (int i = 0; i < 26; i++) {
                if (freq1[i] != freq2[i]) {
                    isAnagram = false;
                    break;
                }
            }
            
            // Display result
            System.out.println();
            if (isAnagram) {
                System.out.println(word1 + " and " + word2 + " are ANAGRAMS!");
                System.out.println("They contain the same letters in different order.");
            } else {
                System.out.println(word1 + " and " + word2 + " are NOT anagrams.");
            }
        }
        
        scanner.close();
    }
}
