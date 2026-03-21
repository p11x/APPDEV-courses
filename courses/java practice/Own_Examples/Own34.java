import java.util.Scanner;

// Alphabet Frequency Counter
public class Own34 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Alphabet Frequency Counter ===");
        System.out.println();
        
        // Input a sentence
        System.out.print("Enter a sentence: ");
        String sentence = scanner.nextLine();
        
        // Count frequency of each alphabet (case insensitive)
        int[] frequency = new int[26];
        
        for (int i = 0; i < sentence.length(); i++) {
            char ch = sentence.charAt(i);
            
            // Check if it's a letter
            if (ch >= 'a' && ch <= 'z') {
                frequency[ch - 'a']++;
            } else if (ch >= 'A' && ch <= 'Z') {
                frequency[ch - 'A']++;
            }
        }
        
        // Display alphabet frequencies
        System.out.println();
        System.out.println("=== Alphabet Frequency ===");
        
        boolean hasAlphabet = false;
        for (int i = 0; i < 26; i++) {
            if (frequency[i] > 0) {
                char letter = (char) ('a' + i);
                System.out.println(letter + " (or " + (char)('A' + i) + "): " + frequency[i]);
                hasAlphabet = true;
            }
        }
        
        if (!hasAlphabet) {
            System.out.println("No alphabets found in the sentence!");
        }
        
        // Find most frequent alphabet
        int maxFreq = 0;
        char maxLetter = ' ';
        
        for (int i = 0; i < 26; i++) {
            if (frequency[i] > maxFreq) {
                maxFreq = frequency[i];
                maxLetter = (char) ('a' + i);
            }
        }
        
        if (maxFreq > 0) {
            System.out.println();
            System.out.println("Most frequent alphabet: " + maxLetter + " (appeared " + maxFreq + " times)");
        }
        
        scanner.close();
    }
}
