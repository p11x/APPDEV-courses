/*
 * SUB TOPIC: Character Array Operations
 * 
 * DEFINITION:
 * A character array (char[]) stores characters. Java provides Character class methods for character
 * manipulation. Characters can be converted to/from strings and have various operations available.
 * 
 * FUNCTIONALITIES:
 * 1. Character array declaration and initialization
 * 2. String to char array conversion
 * 3. Character class methods
 * 4. Character manipulation
 * 5. ASCII values
 */

public class Example18 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: Character Array
        System.out.println("=== Character Array ===");
        
        char[] letters = new char[5]; // Create char array of size 5
        letters[0] = 'A';
        letters[1] = 'B';
        letters[2] = 'C';
        letters[3] = 'D';
        letters[4] = 'E';
        
        for (int i = 0; i < letters.length; i++) {
            System.out.print(letters[i] + " ");
        }
        
        // Real-time Example 1: Direct initialization
        System.out.println("\n\n=== Vowels ===");
        
        char[] vowels = {'A', 'E', 'I', 'O', 'U'};
        
        for (char v : vowels) {
            System.out.print(v + " ");
        }
        
        // Real-time Example 2: String to Char Array
        System.out.println("\n\n=== String to Char Array ===");
        
        String word = "Hello";
        char[] chars = word.toCharArray(); // Convert string to char array
        
        System.out.println("String: " + word);
        System.out.print("Characters: ");
        for (char c : chars) {
            System.out.print(c + " ");
        }
        
        // Real-time Example 3: Character Operations
        System.out.println("\n\n=== Character Operations ===");
        
        char letter = 'A';
        
        System.out.println("'" + letter + "' is letter: " + Character.isLetter(letter));
        System.out.println("'5' is digit: " + Character.isDigit('5'));
        System.out.println("'a' is lowercase: " + Character.isLowerCase('a'));
        System.out.println("'A' is uppercase: " + Character.isUpperCase('A'));
        System.out.println("'A' to lowercase: " + Character.toLowerCase('A'));
        System.out.println("'a' to uppercase: " + Character.toUpperCase('a'));
        
        // Real-time Example 4: Count vowels in string
        System.out.println("\n=== Count Vowels ===");
        
        String sentence = "Programming is fun";
        char[] sentenceChars = sentence.toCharArray();
        
        int vowelCount = 0;
        
        for (char c : sentenceChars) {
            char upper = Character.toUpperCase(c);
            if (upper == 'A' || upper == 'E' || upper == 'I' || upper == 'O' || upper == 'U') {
                vowelCount++;
            }
        }
        
        System.out.println("Vowels in '" + sentence + "': " + vowelCount);
        
        // Real-time Example 5: Reverse string
        System.out.println("\n=== Reverse String ===");
        
        String original = "Java";
        char[] originalChars = original.toCharArray();
        char[] reversed = new char[originalChars.length];
        
        for (int i = 0; i < originalChars.length; i++) {
            reversed[i] = originalChars[originalChars.length - 1 - i];
        }
        
        String reversedString = new String(reversed);
        System.out.println("Original: " + original);
        System.out.println("Reversed: " + reversedString);
        
        // Real-time Example 6: Alphabet array
        System.out.println("\n=== Alphabet ===");
        
        char[] alphabet = new char[26];
        
        for (int i = 0; i < 26; i++) {
            alphabet[i] = (char) ('a' + i);
        }
        
        System.out.print("Alphabet: ");
        for (char c : alphabet) {
            System.out.print(c + " ");
        }
    }
}
