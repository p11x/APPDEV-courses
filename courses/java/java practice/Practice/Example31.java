/*
 * SUB TOPIC: String Methods in Java
 * 
 * DEFINITION:
 * String is a sequence of characters in Java. It is an object that represents a collection 
 * of characters. Java provides numerous built-in methods to manipulate strings, including 
 * finding length, extracting substrings, searching, replacing, and converting cases.
 * 
 * FUNCTIONALITIES:
 * 1. length() - Returns the number of characters in a string
 * 2. charAt() - Returns character at specified index
 * 3. substring() - Extracts portion of string
 * 4. indexOf() - Finds position of character or substring
 * 5. toUpperCase()/toLowerCase() - Converts string case
 * 6. trim() - Removes leading and trailing spaces
 * 7. replace() - Replaces characters or substrings
 * 8. split() - Splits string into array
 * 9. equals() - Compares two strings for equality
 * 10. contains() - Checks if string contains substring
 */

// Topic Explanation with Code: Common String Methods
public class Example31 {
    public static void main(String[] args) {
        
        // Creating Strings - multiple ways to create string objects
        String str1 = "Hello"; // Using string literal - stored in string pool
        String str2 = new String("World"); // Using new keyword - creates new object
        char[] chars = {'J', 'A', 'V', 'A'}; // Character array
        String str3 = new String(chars); // Creating from character array
        
        System.out.println("String from literal: " + str1);
        System.out.println("String from new: " + str2);
        System.out.println("String from char array: " + str3);
        
        // length() - Returns total number of characters
        String text = "Hello Java!";
        System.out.println("\nString: " + text);
        System.out.println("Length: " + text.length()); // Returns 11
        
        // charAt() - Get character at specific index (0-based)
        String word = "Java";
        System.out.println("\nCharacter at index 0: " + word.charAt(0)); // J
        System.out.println("Character at index 2: " + word.charAt(2)); // v
        
        // toUpperCase() and toLowerCase() - Case conversion
        String mixed = "HeLLo WoRLD";
        System.out.println("\nUppercase: " + mixed.toUpperCase()); // HELLO WORLD
        System.out.println("Lowercase: " + mixed.toLowerCase()); // hello world
        
        // substring() - Extract part of string
        String sentence = "Hello World";
        System.out.println("\nFrom index 6: " + sentence.substring(6)); // World
        System.out.println("Index 0 to 5: " + sentence.substring(0, 5)); // Hello
        
        // indexOf() - Find position of character or string
        String text2 = "Hello World";
        System.out.println("\nIndex of 'o': " + text2.indexOf('o')); // 4
        System.out.println("Index of 'World': " + text2.indexOf("World")); // 6
        
        // replace() - Replace characters or strings
        String original = "Hello World";
        System.out.println("\nReplace 'o' with 'X': " + original.replace('o', 'X')); // HellX WXrld
        System.out.println("Replace 'World' with 'Java': " + original.replace("World", "Java")); // Hello Java
        
        // Real-time Example 1: User input validation - checking username length
        System.out.println("\n=== Example 1: Username Validation ===");
        String username = "john_doe";
        if (username.length() >= 5 && username.length() <= 15) {
            System.out.println("Username '" + username + "' is valid");
        } else {
            System.out.println("Username must be 5-15 characters");
        }
        
        // Real-time Example 2: Extracting file extension from filename
        System.out.println("\n=== Example 2: File Extension ===");
        String filename = "document.pdf";
        int dotIndex = filename.lastIndexOf('.'); // Find last dot position
        String extension = filename.substring(dotIndex + 1); // Extract after dot
        System.out.println("File: " + filename);
        System.out.println("Extension: " + extension); // pdf
        
        // Real-time Example 3: Email validation using contains
        System.out.println("\n=== Example 3: Email Validation ===");
        String email = "user@example.com";
        boolean hasAt = email.contains("@"); // Check for @ symbol
        boolean hasDot = email.contains("."); // Check for dot
        if (hasAt && hasDot) {
            System.out.println("Email '" + email + "' appears valid");
        } else {
            System.out.println("Invalid email format");
        }
        
        // Real-time Example 4: Password masking simulation
        System.out.println("\n=== Example 4: Password Processing ===");
        String password = "MyPass123";
        String masked = "";
        for (int i = 0; i < password.length(); i++) {
            masked += "*"; // Replace each character with asterisk
        }
        System.out.println("Original: " + password);
        System.out.println("Masked: " + masked); // ********
        
        // Real-time Example 5: Name formatting (capitalize first letter)
        System.out.println("\n=== Example 5: Name Formatting ===");
        String fullName = "john doe";
        String[] parts = fullName.split(" "); // Split by space
        String formatted = "";
        for (String part : parts) {
            if (part.length() > 0) {
                // Capitalize first letter, lowercase rest
                formatted += part.substring(0, 1).toUpperCase() + 
                            part.substring(1).toLowerCase() + " ";
            }
        }
        formatted = formatted.trim(); // Remove trailing space
        System.out.println("Original: " + fullName);
        System.out.println("Formatted: " + formatted); // John Doe
        
        // Real-time Example 6: Checking string equality (login simulation)
        System.out.println("\n=== Example 6: Login Validation ===");
        String enteredPassword = "admin123";
        String storedPassword = "admin123";
        if (enteredPassword.equals(storedPassword)) {
            System.out.println("Password correct - Login successful");
        } else {
            System.out.println("Incorrect password");
        }
    }
}
