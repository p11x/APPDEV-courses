import java.util.Scanner;

// String Operations Without Library
public class Own54 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== String Operations Without Library ===");
        System.out.println();
        
        // Input a string
        System.out.print("Enter a string: ");
        String input = scanner.next();
        
        // 1. Find length manually
        int length = 0;
        for (int i = 0; i < input.length(); i++) {
            length++;
        }
        System.out.println();
        System.out.println("1. Length: " + length);
        
        // 2. Reverse the string
        String reversed = "";
        for (int i = input.length() - 1; i >= 0; i--) {
            reversed = reversed + input.charAt(i);
        }
        System.out.println("2. Reversed: " + reversed);
        
        // 3. Check if palindrome
        boolean isPalindrome = true;
        for (int i = 0; i < input.length() / 2; i++) {
            if (input.charAt(i) != input.charAt(input.length() - 1 - i)) {
                isPalindrome = false;
                break;
            }
        }
        System.out.println("3. Is Palindrome: " + isPalindrome);
        
        // 4. Count vowels and consonants
        int vowels = 0;
        int consonants = 0;
        String lowerInput = input.toLowerCase();
        
        for (int i = 0; i < input.length(); i++) {
            char ch = lowerInput.charAt(i);
            if (ch >= 'a' && ch <= 'z') {
                if (ch == 'a' || ch == 'e' || ch == 'i' || ch == 'o' || ch == 'u') {
                    vowels++;
                } else {
                    consonants++;
                }
            }
        }
        System.out.println("4. Vowels: " + vowels + ", Consonants: " + consonants);
        
        // 5. Convert to uppercase manually
        String uppercase = "";
        for (int i = 0; i < input.length(); i++) {
            char ch = input.charAt(i);
            if (ch >= 'a' && ch <= 'z') {
                char upper = (char) (ch - 32);
                uppercase = uppercase + upper;
            } else {
                uppercase = uppercase + ch;
            }
        }
        System.out.println("5. Uppercase: " + uppercase);
        
        scanner.close();
    }
}
