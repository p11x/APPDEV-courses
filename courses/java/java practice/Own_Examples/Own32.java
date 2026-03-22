import java.util.Scanner;

// Word Frequency Counter
public class Own32 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Word Frequency Counter ===");
        System.out.println();
        
        // Input a sentence
        System.out.print("Enter a sentence: ");
        String sentence = scanner.nextLine();
        
        // Find number of words manually by scanning character by character
        int wordCount = 0;
        boolean inWord = false;
        
        for (int i = 0; i < sentence.length(); i++) {
            char ch = sentence.charAt(i);
            if (ch != ' ') {
                if (!inWord) {
                    wordCount++;
                    inWord = true;
                }
            } else {
                inWord = false;
            }
        }
        
        // Create array to store words
        String[] words = new String[wordCount];
        int[] frequency = new int[wordCount];
        int uniqueWords = 0;
        
        // Extract words manually
        String currentWord = "";
        int index = 0;
        
        for (int i = 0; i < sentence.length(); i++) {
            char ch = sentence.charAt(i);
            
            if (ch != ' ') {
                currentWord = currentWord + ch;
            } else {
                if (!currentWord.isEmpty()) {
                    // Check if word already exists
                    boolean found = false;
                    for (int j = 0; j < uniqueWords; j++) {
                        if (words[j].equalsIgnoreCase(currentWord)) {
                            frequency[j]++;
                            found = true;
                            break;
                        }
                    }
                    if (!found) {
                        words[uniqueWords] = currentWord;
                        frequency[uniqueWords] = 1;
                        uniqueWords++;
                    }
                    currentWord = "";
                }
            }
        }
        
        // Add last word
        if (!currentWord.isEmpty()) {
            boolean found = false;
            for (int j = 0; j < uniqueWords; j++) {
                if (words[j].equalsIgnoreCase(currentWord)) {
                    frequency[j]++;
                    found = true;
                    break;
                }
            }
            if (!found) {
                words[uniqueWords] = currentWord;
                frequency[uniqueWords] = 1;
                uniqueWords++;
            }
        }
        
        // Display word frequencies
        System.out.println();
        System.out.println("=== Word Frequency ===");
        for (int i = 0; i < uniqueWords; i++) {
            System.out.println(words[i] + ": " + frequency[i]);
        }
        
        System.out.println();
        System.out.println("Total unique words: " + uniqueWords);
        
        scanner.close();
    }
}
