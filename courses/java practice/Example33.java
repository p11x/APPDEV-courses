// Example33: Regular Expressions (Regex) - Beginner Tutorial
// This explains regex patterns in Java

import java.util.regex.*;

public class Example33 {
    public static void main(String[] args) {
        
        // ===== WHAT IS REGEX? =====
        System.out.println("=== What is Regular Expression? ===\n");
        
        System.out.println("Regex = Pattern matching for strings");
        System.out.println("Used to:");
        System.out.println("  - Validate input");
        System.out.println("  - Find patterns");
        System.out.println("  - Replace text");
        
        // ===== BASIC PATTERNS =====
        System.out.println("\n=== Basic Patterns ===\n");
        
        // Pattern: matches() method
        String str1 = "hello";
        
        System.out.println("String: \"" + str1 + "\"");
        System.out.println("Pattern \"hello\": " + Pattern.matches("hello", str1));
        System.out.println("Pattern \"world\": " + Pattern.matches("world", str1));
        
        // ===== CHARACTER CLASSES =====
        System.out.println("\n=== Character Classes ===\n");
        
        // [abc] - Any of a, b, or c
        System.out.println("[abc] - Any of a, b, or c:");
        System.out.println("\"a\": " + Pattern.matches("[abc]", "a"));
        System.out.println("\"d\": " + Pattern.matches("[abc]", "d"));
        
        // [^abc] - Not a, b, or c
        System.out.println("\n[^abc] - Not a, b, or c:");
        System.out.println("\"d\": " + Pattern.matches("[^abc]", "d"));
        System.out.println("\"a\": " + Pattern.matches("[^abc]", "a"));
        
        // [a-z] - Range a to z
        System.out.println("\n[a-z] - Any lowercase:");
        System.out.println("\"a\": " + Pattern.matches("[a-z]", "a"));
        System.out.println("\"Z\": " + Pattern.matches("[a-z]", "Z"));
        
        // [A-Z] - Any uppercase
        System.out.println("\n[A-Z] - Any uppercase:");
        System.out.println("\"A\": " + Pattern.matches("[A-Z]", "A"));
        System.out.println("\"a\": " + Pattern.matches("[A-Z]", "a"));
        
        // [0-9] - Any digit
        System.out.println("\n[0-9] - Any digit:");
        System.out.println("\"5\": " + Pattern.matches("[0-9]", "5"));
        System.out.println("\"a\": " + Pattern.matches("[0-9]", "a"));
        
        // [a-zA-Z] - Any letter
        System.out.println("\n[a-zA-Z] - Any letter:");
        System.out.println("\"a\": " + Pattern.matches("[a-zA-Z]", "a"));
        System.out.println("\"Z\": " + Pattern.matches("[a-zA-Z]", "Z"));
        
        // [a-zA-Z0-9] - Any alphanumeric
        System.out.println("\n[a-zA-Z0-9] - Any alphanumeric:");
        System.out.println("\"a5\": " + Pattern.matches("[a-zA-Z0-9]", "a5"));
        
        // ===== PREDEFINED CHARACTER CLASSES =====
        System.out.println("\n=== Predefined Character Classes ===\n");
        
        // . - Any character
        System.out.println(". (any character):");
        System.out.println("\"a\": " + Pattern.matches(".", "a"));
        System.out.println("\"@\": " + Pattern.matches(".", "@"));
        
        // \\d - Any digit [0-9]
        System.out.println("\\d (digit):");
        System.out.println("\"5\": " + Pattern.matches("\\d", "5"));
        System.out.println("\"a\": " + Pattern.matches("\\d", "a"));
        
        // \\D - Non-digit
        System.out.println("\\D (non-digit):");
        System.out.println("\"a\": " + Pattern.matches("\\D", "a"));
        System.out.println("\"5\": " + Pattern.matches("\\D", "5"));
        
        // \\w - Word character [a-zA-Z0-9_]
        System.out.println("\\w (word character):");
        System.out.println("\"a\": " + Pattern.matches("\\w", "a"));
        System.out.println("\"5\": " + Pattern.matches("\\w", "5"));
        System.out.println("\"_\": " + Pattern.matches("\\w", "_"));
        
        // \\W - Non-word character
        System.out.println("\\W (non-word character):");
        System.out.println("\"@\": " + Pattern.matches("\\W", "@"));
        
        // \\s - Whitespace
        System.out.println("\\s (whitespace):");
        System.out.println("\" \": " + Pattern.matches("\\s", " "));
        
        // ===== QUANTIFIERS =====
        System.out.println("\n=== Quantifiers ===\n");
        
        // * - Zero or more
        System.out.println("* (zero or more):");
        System.out.println("\"\": " + Pattern.matches("a*", ""));
        System.out.println("\"aaa\": " + Pattern.matches("a*", "aaa"));
        
        // + - One or more
        System.out.println("+ (one or more):");
        System.out.println("\"a\": " + Pattern.matches("a+", "a"));
        System.out.println("\"aaa\": " + Pattern.matches("a+", "aaa"));
        System.out.println("\"\": " + Pattern.matches("a+", ""));
        
        // ? - Zero or one
        System.out.println("? (zero or one):");
        System.out.println("\"a\": " + Pattern.matches("a?", "a"));
        System.out.println("\"\": " + Pattern.matches("a?", ""));
        
        // {n} - Exactly n times
        System.out.println("{n} (exactly n times):");
        System.out.println("\"aaa\": " + Pattern.matches("a{3}", "aaa"));
        System.out.println("\"aa\": " + Pattern.matches("a{3}", "aa"));
        
        // {n,} - n or more times
        System.out.println("{n,} (n or more):");
        System.out.println("\"aaa\": " + Pattern.matches("a{2,}", "aaa"));
        
        // {n,m} - Between n and m times
        System.out.println("{n,m} (between n and m):");
        System.out.println("\"aaa\": " + Pattern.matches("a{2,4}", "aaa"));
        
        // ===== GROUPING =====
        System.out.println("\n=== Grouping ===\n");
        
        // () - Group
        System.out.println("(abc) - Group:");
        System.out.println("\"abc\": " + Pattern.matches("(abc)", "abc"));
        
        // | - Alternation (or)
        System.out.println("| (or):");
        System.out.println("\"cat\": " + Pattern.matches("cat|dog", "cat"));
        System.out.println("\"dog\": " + Pattern.matches("cat|dog", "dog"));
        
        // ===== PRACTICAL VALIDATIONS =====
        System.out.println("\n=== Practical Validations ===\n");
        
        // Email validation
        String email = "user@example.com";
        String emailPattern = "^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$";
        
        System.out.println("Email: " + email);
        System.out.println("Valid: " + Pattern.matches(emailPattern, email));
        
        // Phone number validation (10 digits)
        String phone = "1234567890";
        String phonePattern = "^[0-9]{10}$";
        
        System.out.println("\nPhone: " + phone);
        System.out.println("Valid: " + Pattern.matches(phonePattern, phone));
        
        // Username validation (alphanumeric, 5-15 chars)
        String username = "john123";
        String usernamePattern = "^[a-zA-Z0-9]{5,15}$";
        
        System.out.println("\nUsername: " + username);
        System.out.println("Valid: " + Pattern.matches(usernamePattern, username));
        
        // Password validation (at least 8 chars, 1 uppercase, 1 digit)
        String password = "Password1";
        String passwordPattern = "^(?=.*[A-Z])(?=.*\\d)[A-Za-z\\d]{8,}$";
        
        System.out.println("\nPassword: " + password);
        System.out.println("Valid: " + Pattern.matches(passwordPattern, password));
        
        // ===== PATTERN CLASS USAGE =====
        System.out.println("\n=== Pattern and Matcher ===\n");
        
        String text = "The quick brown fox jumps over the lazy dog";
        Pattern pattern = Pattern.compile("fox");
        Matcher matcher = pattern.matcher(text);
        
        System.out.println("Text: " + text);
        System.out.println("Pattern: fox");
        System.out.println("Found: " + matcher.find());
        
        // Find all occurrences
        String text2 = "one two three two one";
        Pattern pattern2 = Pattern.compile("two");
        Matcher matcher2 = pattern2.matcher(text2);
        
        System.out.println("\nText: " + text2);
        System.out.println("Finding all 'two':");
        
        while (matcher2.find()) {
            System.out.println("  Found at index: " + matcher2.start());
        }
        
        // ===== REPLACE WITH REGEX =====
        System.out.println("\n=== Replace Using Regex ===\n");
        
        String original = "Hello World";
        
        // Replace all
        System.out.println("Original: " + original);
        System.out.println("Replace 'World' with 'Java': " + original.replaceAll("World", "Java"));
        
        // Replace digits
        String withNumbers = "abc123def456";
        System.out.println("\nOriginal: " + withNumbers);
        System.out.println("Remove digits: " + withNumbers.replaceAll("[0-9]", ""));
        
        // Replace multiple spaces
        String withSpaces = "Hello    World    !";
        System.out.println("\nOriginal: \"" + withSpaces + "\"");
        System.out.println("Replace multiple spaces: \"" + withSpaces.replaceAll("\\s+", " ") + "\"");
        
        // ===== SPLIT WITH REGEX =====
        System.out.println("\n=== Split Using Regex ===\n");
        
        String csv = "apple,banana,cherry,date";
        String[] fruits = csv.split(",");
        
        System.out.println("Original: " + csv);
        System.out.println("Split by comma:");
        for (String f : fruits) {
            System.out.println("  - " + f);
        }
        
        // Split by whitespace
        String text3 = "Hello   World   Java";
        String[] words = text3.split("\\s+");
        
        System.out.println("\nOriginal: " + text3);
        System.out.println("Split by whitespace:");
        for (String w : words) {
            System.out.println("  - " + w);
        }
        
        // ===== COMMON REGEX PATTERNS =====
        System.out.println("\n=== Common Regex Patterns ===\n");
        
        System.out.println("^[a-z]$           - Single lowercase letter");
        System.out.println("^[A-Z]$           - Single uppercase letter");
        System.out.println("^[0-9]$           - Single digit");
        System.out.println("^[a-zA-Z]{3}$     - Exactly 3 letters");
        System.out.println("^\\d{10}$          - 10 digit phone number");
        System.out.println("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$ - Email");
        System.out.println("^https?://        - HTTP or HTTPS URL");
        System.out.println("^\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}$ - IP Address");
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. REGEX BASICS:
 *    - Pattern matching for strings
 *    - Used for validation, search, replace
 * 
 * 2. CHARACTER CLASSES:
 *    [abc]   - a, b, or c
 *    [^abc]  - NOT a, b, or c
 *    [a-z]   - a to z
 *    [0-9]   - 0 to 9
 * 
 * 3. PREDEFINED CLASSES:
 *    .   - Any character
 *    \\d  - Digit [0-9]
 *    \\D  - Non-digit
 *    \\w  - Word [a-zA-Z0-9_]
 *    \\W  - Non-word
 *    \\s  - Whitespace
 * 
 * 4. QUANTIFIERS:
 *    *   - Zero or more
 *    +   - One or more
 *    ?   - Zero or one
 *    {n} - Exactly n times
 *    {n,} - n or more times
 *    {n,m} - Between n and m times
 * 
 * 5. ANCHORS:
 *    ^   - Start of string
 *    $   - End of string
 * 
 * 6. COMMON METHODS:
 *    Pattern.matches()     - Check if matches
 *    Pattern.compile()    - Compile pattern
 *    Matcher.find()       - Find next match
 *    String.replaceAll()  - Replace all
 *    String.split()       - Split by pattern
 */
