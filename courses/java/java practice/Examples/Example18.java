// Example18: Array of Characters - Beginner Tutorial
// This shows different ways to work with char arrays

public class Example18 {
    public static void main(String[] args) {
        
        // ===== METHOD 1: Create char array with size =====
        System.out.println("=== Method 1: Create with size ===");
        
        char[] letters = new char[5];
        letters[0] = 'A';
        letters[1] = 'B';
        letters[2] = 'C';
        letters[3] = 'D';
        letters[4] = 'E';
        
        // Print each character
        System.out.print("Letters: ");
        for (int i = 0; i < letters.length; i++) {
            System.out.print(letters[i] + " ");
        }
        System.out.println();
        
        // ===== METHOD 2: Direct initialization =====
        System.out.println("\n=== Method 2: Direct initialization ===");
        
        char[] vowels = {'A', 'E', 'I', 'O', 'U'};
        
        System.out.print("Vowels: ");
        for (char v : vowels) {
            System.out.print(v + " ");
        }
        System.out.println();
        
        // ===== CONVERT STRING TO CHAR ARRAY =====
        System.out.println("\n=== String to Char Array ===");
        
        String word = "Hello";
        
        // Convert string to char array using toCharArray()
        char[] chars = word.toCharArray();
        
        System.out.println("String: " + word);
        System.out.print("As characters: ");
        for (char c : chars) {
            System.out.print(c + " ");
        }
        System.out.println();
        
        // ===== CONVERT CHAR ARRAY TO STRING =====
        System.out.println("\n=== Char Array to String ===");
        
        char[] myChars = {'J', 'A', 'V', 'A'};
        
        // Convert char array to string using String constructor
        String myString = new String(myChars);
        
        System.out.println("Char array: J A V A");
        System.out.println("As string: " + myString);
        
        // ===== CHARACTER OPERATIONS =====
        System.out.println("\n=== Character Operations ===");
        
        char letter = 'A';
        
        // Check if character is letter
        System.out.println("'" + letter + "' is letter: " + Character.isLetter(letter));
        
        // Check if character is digit
        System.out.println("'5' is digit: " + Character.isDigit('5'));
        
        // Check if character is lowercase
        System.out.println("'a' is lowercase: " + Character.isLowerCase('a'));
        
        // Check if character is uppercase
        System.out.println("'A' is uppercase: " + Character.isUpperCase('A'));
        
        // Convert to lowercase
        System.out.println("'A' to lowercase: " + Character.toLowerCase('A'));
        
        // Convert to uppercase
        System.out.println("'a' to uppercase: " + Character.toUpperCase('a'));
        
        // Check if character is whitespace
        System.out.println("' ' is whitespace: " + Character.isWhitespace(' '));
        
        // ===== PRACTICAL EXAMPLE: Count vowels in string =====
        System.out.println("\n=== Practical Example: Count Vowels ===");
        
        String sentence = "Programming is fun";
        char[] sentenceChars = sentence.toCharArray();
        
        int vowelCount = 0;
        int consonantCount = 0;
        
        for (char c : sentenceChars) {
            // Convert to uppercase for easier checking
            char upper = Character.toUpperCase(c);
            
            if (upper == 'A' || upper == 'E' || upper == 'I' || upper == 'O' || upper == 'U') {
                vowelCount++;
            } else if (Character.isLetter(c)) {
                consonantCount++;
            }
        }
        
        System.out.println("Sentence: " + sentence);
        System.out.println("Vowels: " + vowelCount);
        System.out.println("Consonants: " + consonantCount);
        
        // ===== PRACTICAL EXAMPLE: Reverse a string =====
        System.out.println("\n=== Practical Example: Reverse String ===");
        
        String original = "Java";
        char[] originalChars = original.toCharArray();
        
        // Create new array for reversed
        char[] reversed = new char[originalChars.length];
        
        // Copy characters in reverse order
        for (int i = 0; i < originalChars.length; i++) {
            reversed[i] = originalChars[originalChars.length - 1 - i];
        }
        
        String reversedString = new String(reversed);
        
        System.out.println("Original: " + original);
        System.out.println("Reversed: " + reversedString);
        
        // ===== PRACTICAL EXAMPLE: Palindrome Check =====
        System.out.println("\n=== Practical Example: Palindrome Check ===");
        
        String testWord = "madam";
        char[] testChars = testWord.toCharArray();
        
        boolean isPalindrome = true;
        
        for (int i = 0; i < testChars.length / 2; i++) {
            if (testChars[i] != testChars[testChars.length - 1 - i]) {
                isPalindrome = false;
                break;
            }
        }
        
        System.out.println("Word: " + testWord);
        System.out.println("Is palindrome: " + isPalindrome);
        
        // ===== ALPHABET ARRAY =====
        System.out.println("\n=== Alphabet Array ===");
        
        char[] alphabet = new char[26];
        
        for (int i = 0; i < 26; i++) {
            alphabet[i] = (char) ('a' + i);
        }
        
        System.out.print("Alphabet (lowercase): ");
        for (char c : alphabet) {
            System.out.print(c + " ");
        }
        System.out.println();
        
        // Uppercase alphabet
        System.out.print("Alphabet (uppercase): ");
        for (char c : alphabet) {
            System.out.print(Character.toUpperCase(c) + " ");
        }
        System.out.println();
        
        // ===== FIND CHARACTER IN ARRAY =====
        System.out.println("\n=== Find Character in Array ===");
        
        char[] searchArray = {'A', 'B', 'C', 'D', 'E'};
        char searchChar = 'C';
        
        int position = -1;
        for (int i = 0; i < searchArray.length; i++) {
            if (searchArray[i] == searchChar) {
                position = i;
                break;
            }
        }
        
        System.out.println("Array: A B C D E");
        System.out.println("Looking for: " + searchChar);
        System.out.println("Found at index: " + position);
        
        // ===== ASCII VALUES =====
        System.out.println("\n=== ASCII Values ===");
        
        char[] asciiChars = {'A', 'Z', 'a', 'z', '0', '9'};
        
        for (char c : asciiChars) {
            System.out.println("'" + c + "' = " + (int)c);
        }
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. DECLARING CHAR ARRAYS:
 *    char[] array = new char[size];
 *    char[] array = {'a', 'b', 'c'};
 * 
 * 2. CONVERTING BETWEEN STRING AND CHAR:
 *    - String to char:   String.toCharArray()
 *    - char to String:   new String(charArray)
 * 
 * 3. CHARACTER CLASS METHODS:
 *    - Character.isLetter(c)     -> is it a letter?
 *    - Character.isDigit(c)     -> is it a digit?
 *    - Character.isLowerCase(c) -> is it lowercase?
 *    - Character.isUpperCase(c) -> is it uppercase?
 *    - Character.toLowerCase(c) -> convert to lowercase
 *    - Character.toUpperCase(c) -> convert to uppercase
 *    - Character.isWhitespace(c) -> is it whitespace?
 * 
 * 4. IMPORTANT FACTS:
 *    - char uses single quotes: 'A' not "A"
 *    - char is 16-bit unsigned integer (0 to 65535)
 *    - Characters can be converted to numbers (ASCII/Unicode)
 *    - 'a' + 1 gives 'b' (can do math with chars)
 * 
 * 5. COMMON USES:
 *    - String manipulation
 *    - Finding characters in text
 *    - Checking if word is palindrome
 *    - Converting case
 *    - Building custom strings
 */
