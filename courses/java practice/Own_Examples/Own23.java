import java.util.Scanner;

// Palindrome Checker (Number + String)
public class Own23 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Palindrome Checker (Number + String) ===");
        System.out.println();
        
        // ==================== NUMBER PALINDROME ====================
        System.out.println("===== Number Palindrome =====");
        System.out.print("Enter a number: ");
        int number = scanner.nextInt();
        
        int originalNumber = number;
        int reversedNumber = 0;
        
        // Reverse the number
        while (number > 0) {
            int digit = number % 10;
            reversedNumber = reversedNumber * 10 + digit;
            number = number / 10;
        }
        
        // Check if palindrome
        if (originalNumber == reversedNumber) {
            System.out.println(originalNumber + " is a Palindrome number!");
        } else {
            System.out.println(originalNumber + " is NOT a Palindrome number!");
        }
        
        System.out.println();
        
        // ==================== STRING PALINDROME ====================
        System.out.println("===== String Palindrome =====");
        System.out.print("Enter a word: ");
        String word = scanner.next();
        
        // Remove spaces and convert to lowercase
        String cleanWord = word.replaceAll("\\s+", "").toLowerCase();
        
        // Reverse the string
        String reversedWord = "";
        for (int i = cleanWord.length() - 1; i >= 0; i--) {
            reversedWord = reversedWord + cleanWord.charAt(i);
        }
        
        // Check if palindrome
        if (cleanWord.equals(reversedWord)) {
            System.out.println(word + " is a Palindrome word!");
        } else {
            System.out.println(word + " is NOT a Palindrome word!");
        }
        
        scanner.close();
    }
}
