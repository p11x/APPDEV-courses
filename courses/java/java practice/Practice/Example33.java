/*
 * SUB TOPIC: Regular Expressions (Regex) in Java
 * 
 * DEFINITION:
 * Regular Expression (Regex) is a pattern used to match character combinations in strings. 
 * It provides a powerful way to search, validate, and manipulate text. Java supports regex 
 * through the java.util.regex package with Pattern and Matcher classes.
 * 
 * FUNCTIONALITIES:
 * 1. Pattern matching - Find if string matches a pattern
 * 2. Validation - Validate input formats (email, phone, etc.)
 * 3. Search and replace - Find and replace text using patterns
 * 4. Splitting - Split strings based on patterns
 * 5. Character classes - Define sets of characters to match
 * 6. Quantifiers - Specify how many times to match
 */

import java.util.regex.*; // Import regex classes

public class Example33 {
    public static void main(String[] args) {
        
        // Topic Explanation: Basic Pattern Matching
        
        // Pattern.matches() - Check if entire string matches pattern
        String str = "hello";
        System.out.println("Pattern 'hello' matches 'hello': " + Pattern.matches("hello", str)); // true
        System.out.println("Pattern 'world' matches 'hello': " + Pattern.matches("world", str)); // false
        
        // Character Classes - Define what characters to match
        System.out.println("\n=== Character Classes ===");
        
        // [abc] - Match any one of a, b, or c
        System.out.println("[abc] matches 'a': " + Pattern.matches("[abc]", "a")); // true
        System.out.println("[abc] matches 'd': " + Pattern.matches("[abc]", "d")); // false
        
        // [^abc] - Match any character EXCEPT a, b, or c
        System.out.println("[^abc] matches 'd': " + Pattern.matches("[^abc]", "d")); // true
        
        // [a-z] - Match any lowercase letter
        System.out.println("[a-z] matches 'm': " + Pattern.matches("[a-z]", "m")); // true
        System.out.println("[a-z] matches 'Z': " + Pattern.matches("[a-z]", "Z")); // false
        
        // [0-9] - Match any digit
        System.out.println("[0-9] matches '5': " + Pattern.matches("[0-9]", "5")); // true
        
        // Predefined Character Classes
        System.out.println("\n=== Predefined Classes ===");
        
        // . - Match any single character
        System.out.println(". matches 'a': " + Pattern.matches(".", "a")); // true
        
        // \d - Match any digit [0-9]
        System.out.println("\\d matches '5': " + Pattern.matches("\\d", "5")); // true
        System.out.println("\\d matches 'a': " + Pattern.matches("\\d", "a")); // false
        
        // \w - Match word character [a-zA-Z0-9_]
        System.out.println("\\w matches 'a': " + Pattern.matches("\\w", "a")); // true
        System.out.println("\\w matches '5': " + Pattern.matches("\\w", "5")); // true
        System.out.println("\\w matches '_': " + Pattern.matches("\\w", "_")); // true
        
        // \s - Match whitespace
        System.out.println("\\s matches ' ': " + Pattern.matches("\\s", " ")); // true
        
        // Quantifiers - Specify how many times to match
        System.out.println("\n=== Quantifiers ===");
        
        // * - Zero or more times
        System.out.println("a* matches '': " + Pattern.matches("a*", "")); // true
        System.out.println("a* matches 'aaa': " + Pattern.matches("a*", "aaa")); // true
        
        // + - One or more times
        System.out.println("a+ matches 'a': " + Pattern.matches("a+", "a")); // true
        System.out.println("a+ matches '': " + Pattern.matches("a+", "")); // false
        
        // ? - Zero or one time
        System.out.println("a? matches 'a': " + Pattern.matches("a?", "a")); // true
        System.out.println("a? matches '': " + Pattern.matches("a?", "")); // true
        
        // {n} - Exactly n times
        System.out.println("a{3} matches 'aaa': " + Pattern.matches("a{3}", "aaa")); // true
        
        // Real-time Example 1: Email validation
        System.out.println("\n=== Example 1: Email Validation ===");
        String email = "user@example.com";
        String emailPattern = "^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$";
        System.out.println("Email: " + email);
        System.out.println("Valid: " + Pattern.matches(emailPattern, email));
        
        // Real-time Example 2: Phone number validation (10 digits)
        System.out.println("\n=== Example 2: Phone Validation ===");
        String phone = "9876543210";
        String phonePattern = "^[0-9]{10}$";
        System.out.println("Phone: " + phone);
        System.out.println("Valid: " + Pattern.matches(phonePattern, phone));
        
        // Real-time Example 3: Username validation (alphanumeric, 5-15 chars)
        System.out.println("\n=== Example 3: Username Validation ===");
        String username = "john123";
        String usernamePattern = "^[a-zA-Z0-9]{5,15}$";
        System.out.println("Username: " + username);
        System.out.println("Valid: " + Pattern.matches(usernamePattern, username));
        
        // Real-time Example 4: Password validation (min 8 chars, 1 uppercase, 1 digit)
        System.out.println("\n=== Example 4: Password Validation ===");
        String password = "Password1";
        String passwordPattern = "^(?=.*[A-Z])(?=.*\\d)[A-Za-z\\d]{8,}$";
        System.out.println("Password: " + password);
        System.out.println("Valid: " + Pattern.matches(passwordPattern, password));
        
        // Real-time Example 5: Finding and replacing using Pattern and Matcher
        System.out.println("\n=== Example 5: Find and Replace ===");
        String text = "Hello World, Welcome to Java";
        Pattern pattern = Pattern.compile("Java");
        Matcher matcher = pattern.matcher(text);
        
        System.out.println("Original: " + text);
        String replaced = matcher.replaceAll("Python");
        System.out.println("Replaced: " + replaced);
        
        // Real-time Example 6: Extracting all numbers from text
        System.out.println("\n=== Example 6: Extract Numbers ===");
        String textWithNumbers = "abc123def456ghi789";
        Pattern numPattern = Pattern.compile("\\d+");
        Matcher numMatcher = numPattern.matcher(textWithNumbers);
        
        System.out.println("Text: " + textWithNumbers);
        System.out.print("Numbers found: ");
        while (numMatcher.find()) {
            System.out.print(numMatcher.group() + " "); // 123 456 789
        }
        
        // Additional: Split with regex
        System.out.println("\n\n=== Split with Regex ===");
        String csv = "apple,banana,cherry,date";
        String[] fruits = csv.split(",");
        System.out.println("Split by comma:");
        for (String f : fruits) {
            System.out.println("  - " + f);
        }
    }
}
