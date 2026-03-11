# Java Strings and String Handling

## Table of Contents
1. [Introduction to Strings](#introduction-to-strings)
2. [String Creation](#string-creation)
3. [String Immutability](#string-immutability)
4. [String Methods](#string-methods)
5. [StringBuilder and StringBuffer](#stringbuilder-and-stringbuffer)
6. [String Formatting](#string-formatting)
7. [Regular Expressions](#regular-expressions)
8. [Code Examples](#code-examples)
9. [Exercises](#exercises)
10. [Solutions](#solutions)

---

## 1. Introduction to Strings

### What is a String?

In Java, a **String** is a sequence of characters. It's one of the most commonly used data types.

```
┌─────────────────────────────────────────────────────────────┐
│                      STRING CONCEPT                          │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   String: "Hello World"                                     │
│                                                              │
│   Index:    [0] [1] [2] [3] [4] [5] [6] [7] [8] [9] [10]  │
│   Char:     H  e  l  l  o     W  o  r  l  d                │
│                                                              │
│   Length: 11 characters                                      │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 2. String Creation

### Methods of Creating Strings

```java
// Method 1: String literal (most common)
String s1 = "Hello";

// Method 2: Using new keyword
String s2 = new String("Hello");

// Method 3: From character array
char[] chars = {'H', 'e', 'l', 'l', 'o'};
String s3 = new String(chars);

// Method 4: From byte array
byte[] bytes = {72, 101, 108, 108, 111};
String s4 = new String(bytes);
```

---

## 3. String Immutability

### What is Immutability?

Strings in Java are **immutable** - once created, they cannot be changed.

```
┌─────────────────────────────────────────────────────────────┐
│                   STRING IMMUTABILITY                        │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   String s = "Hello";                                       │
│   s = s + " World";  // Creates NEW string, doesn't modify  │
│                                                              │
│   BEFORE:  [H][e][l][l][o]                                  │
│                                                              │
│   AFTER:   [H][e][l][l][o]  [W][o][r][l][d]  (NEW object) │
│                                                              │
│   Old string gets garbage collected                         │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### String Pool

```
┌─────────────────────────────────────────────────────────────┐
│                      STRING POOL                            │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   String s1 = "Java";        // Created in pool            │
│   String s2 = "Java";        // Reuses existing "Java"     │
│   String s3 = new String("Java");  // Creates new object    │
│                                                              │
│   s1 == s2    → true   (same reference)                   │
│   s1 == s3    → false  (different objects)                │
│   s1.equals(s3) → true   (same content)                    │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 4. String Methods

### Common String Methods

| Method | Description |
|--------|-------------|
| `length()` | Returns string length |
| `charAt(index)` | Returns character at index |
| `substring(start, end)` | Returns portion of string |
| `toUpperCase()` | Converts to uppercase |
| `toLowerCase()` | Converts to lowercase |
| `trim()` | Removes whitespace |
| `replace(old, new)` | Replaces characters |
| `split(delimiter)` | Splits into array |
| `indexOf(char)` | Finds position |
| `contains(substring)` | Checks if contains |
| `isEmpty()` | Checks if empty |
| `concat(str)` | Concatenates strings |
| `equals(str)` | Compares content |
| `equalsIgnoreCase(str)` | Compares ignoring case |

---

## 5. StringBuilder and StringBuffer

### When to Use?

Use `StringBuilder` or `StringBuffer` when you need to modify strings frequently.

| Class | Thread Safe | Performance |
|-------|-------------|-------------|
| String | N/A (immutable) | Slow for many modifications |
| StringBuilder | No | Fast |
| StringBuffer | Yes | Moderate |

```java
// String - slow for many modifications
String result = "";
for (int i = 0; i < 1000; i++) {
    result += i;  // Creates new object each time!
}

// StringBuilder - fast
StringBuilder sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.append(i);  // Modifies in place
}
String result = sb.toString();

// StringBuffer - thread-safe version
StringBuffer sbf = new StringBuffer();
sbf.append("Hello").append(" World");
```

---

## 6. String Formatting

### printf-style Formatting

```java
// Using printf
System.out.printf("Name: %s, Age: %d, Price: %.2f%n", "John", 25, 99.99);

// Using String.format
String formatted = String.format("Name: %s, Age: %d", "John", 25);
```

### Format Specifiers

| Specifier | Description | Example |
|-----------|-------------|---------|
| `%s` | String | "Hello" |
| `%d` | Integer | 42 |
| `%f` | Float/Double | 3.14 |
| `%.2f` | Float with 2 decimals | 3.14 |
| `%n` | Newline | (line break) |
| `%b` | Boolean | true |
| `%c` | Character | 'A' |

---

## 7. Regular Expressions

### Pattern Matching

```java
import java.util.regex.*;

String email = "user@example.com";
String pattern = "^[A-Za-z0-9+_.-]+@(.+)$";

if (email.matches(pattern)) {
    System.out.println("Valid email");
}
```

### Common Regex Patterns

| Pattern | Matches |
|---------|---------|
| `\d` | Any digit |
| `\w` | Word character |
| `\s` | Whitespace |
| `.` | Any character |
| `*` | Zero or more |
| `+` | One or more |
| `?` | Zero or one |
| `^` | Start of string |
| `$` | End of string |

---

## 8. Code Examples

### Example 1: String Operations Demo

```java
/**
 * StringOperations - Demonstrates common string operations
 * Essential for Angular backend data processing
 */
public class StringOperations {
    
    public static void main(String[] args) {
        System.out.println("=== STRING OPERATIONS DEMO ===\n");
        
        String text = "  Hello, World! Welcome to Java Programming  ";
        
        // Basic operations
        System.out.println("Original: \"" + text + "\"");
        System.out.println("Length: " + text.length());
        System.out.println("Char at 0: " + text.charAt(0));
        System.out.println("Uppercase: " + text.toUpperCase());
        System.out.println("Lowercase: " + text.toLowerCase());
        System.out.println("Trimmed: \"" + text.trim() + "\"");
        
        // Substring
        System.out.println("\n--- Substring Examples ---");
        System.out.println("Substring(0,5): " + text.substring(0, 5));
        System.out.println("Substring(7): " + text.substring(7));
        
        // Search
        System.out.println("\n--- Search Examples ---");
        System.out.println("Index of 'World': " + text.indexOf("World"));
        System.out.println("Contains 'Java': " + text.contains("Java"));
        System.out.println("Starts with '  Hello': " + text.startsWith("  Hello"));
        System.out.println("Ends with 'ming  ': " + text.endsWith("ming  "));
        
        // Replace
        System.out.println("\n--- Replace Examples ---");
        System.out.println("Replace 'Java' with 'Python': " + 
                          text.replace("Java", "Python"));
        System.out.println("Replace all digits with '#': " + 
                          text.replaceAll("\\d", "#"));
        
        // Split
        System.out.println("\n--- Split Examples ---");
        String csv = "apple,banana,cherry,date";
        String[] fruits = csv.split(",");
        System.out.print("Split by comma: ");
        for (String fruit : fruits) {
            System.out.print(fruit + " | ");
        }
        System.out.println();
        
        // Concatenation
        System.out.println("\n--- Concatenation ---");
        String s1 = "Hello";
        String s2 = "World";
        System.out.println("concat: " + s1.concat(" ").concat(s2));
        System.out.println("+ operator: " + s1 + " " + s2);
        
        // Comparison
        System.out.println("\n--- Comparison ---");
        String a = "hello";
        String b = "hello";
        String c = new String("hello");
        System.out.println("a == b: " + (a == b));  // true (same reference)
        System.out.println("a == c: " + (a == c));  // false (different objects)
        System.out.println("a.equals(c): " + a.equals(c));  // true (same content)
        System.out.println("equalsIgnoreCase: " + 
                          "HELLO".equalsIgnoreCase("hello"));
    }
}
```

---

### Example 2: User Input Validation

```java
import java.util.Scanner;

/**
 * UserInputValidator - Validates user input using strings
 * Common in Angular + Java backends for form validation
 */
public class UserInputValidator {
    
    /**
     * Validate email address
     */
    public static boolean isValidEmail(String email) {
        // Simple email validation
        String regex = "^[A-Za-z0-9+_.-]+@[A-Za-z0-9.-]+$";
        return email != null && email.matches(regex);
    }
    
    /**
     * Validate phone number (10 digits)
     */
    public static boolean isValidPhone(String phone) {
        String regex = "^\\d{10}$";
        return phone != null && phone.matches(regex);
    }
    
    /**
     * Validate username (alphanumeric, 5-15 chars)
     */
    public static boolean isValidUsername(String username) {
        String regex = "^[A-Za-z0-9_]{5,15}$";
        return username != null && username.matches(regex);
    }
    
    /**
     * Validate password (at least 8 chars, 1 digit, 1 letter)
     */
    public static boolean isValidPassword(String password) {
        if (password == null || password.length() < 8) {
            return false;
        }
        
        boolean hasDigit = false;
        boolean hasLetter = false;
        
        for (char c : password.toCharArray()) {
            if (Character.isDigit(c)) hasDigit = true;
            if (Character.isLetter(c)) hasLetter = true;
        }
        
        return hasDigit && hasLetter;
    }
    
    /**
     * Check if string contains only alphabets
     */
    public static boolean isAlphabetic(String str) {
        return str != null && str.matches("^[A-Za-z]+$");
    }
    
    /**
     * Check if string contains only digits
     */
    public static boolean isNumeric(String str) {
        return str != null && str.matches("^\\d+$");
    }
    
    /**
     * Mask sensitive data (like passwords)
     */
    public static String maskData(String data, int visibleChars) {
        if (data == null) return null;
        if (data.length() <= visibleChars) return data;
        
        String visible = data.substring(0, visibleChars);
        String masked = "*".repeat(data.length() - visibleChars);
        return visible + masked;
    }
    
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== USER INPUT VALIDATOR ===\n");
        
        // Email validation
        System.out.print("Enter email: ");
        String email = scanner.nextLine();
        System.out.println("Valid email? " + isValidEmail(email));
        
        // Phone validation
        System.out.print("Enter phone (10 digits): ");
        String phone = scanner.nextLine();
        System.out.println("Valid phone? " + isValidPhone(phone));
        
        // Username validation
        System.out.print("Enter username: ");
        String username = scanner.nextLine();
        System.out.println("Valid username? " + isValidUsername(username));
        
        // Password validation
        System.out.print("Enter password: ");
        String password = scanner.nextLine();
        System.out.println("Valid password? " + isValidPassword(password));
        
        // Mask password
        System.out.println("Masked password: " + maskData(password, 2));
        
        scanner.close();
    }
}
```

---

### Example 3: StringBuilder Demo

```java
/**
 * StringBuilderDemo - Shows efficient string manipulation
 */
public class StringBuilderDemo {
    
    public static void main(String[] args) {
        System.out.println("=== STRINGBUILDER DEMO ===\n");
        
        // Create StringBuilder
        StringBuilder sb = new StringBuilder();
        
        // Append
        sb.append("Hello");
        sb.append(" ");
        sb.append("World");
        System.out.println("After append: " + sb);
        
        // Insert
        sb.insert(5, ",");
        System.out.println("After insert: " + sb);
        
        // Replace
        sb.replace(0, 5, "Hi");
        System.out.println("After replace: " + sb);
        
        // Delete
        sb.delete(0, 2);
        System.out.println("After delete: " + sb);
        
        // Reverse
        sb.reverse();
        System.out.println("After reverse: " + sb);
        
        // Chain operations
        StringBuilder builder = new StringBuilder();
        String result = builder
            .append("Java")
            .append(" ")
            .append("is")
            .append(" ")
            .append("awesome")
            .toString();
        
        System.out.println("\nChained result: " + result);
        
        // Performance comparison
        System.out.println("\n=== PERFORMANCE TEST ===");
        int iterations = 10000;
        
        // String concatenation (slow)
        long start = System.currentTimeMillis();
        String s = "";
        for (int i = 0; i < iterations; i++) {
            s += "a";
        }
        long stringTime = System.currentTimeMillis() - start;
        System.out.println("String time: " + stringTime + "ms");
        
        // StringBuilder (fast)
        start = System.currentTimeMillis();
        StringBuilder sb2 = new StringBuilder();
        for (int i = 0; i < iterations; i++) {
            sb2.append("a");
        }
        long builderTime = System.currentTimeMillis() - start;
        System.out.println("StringBuilder time: " + builderTime + "ms");
    }
}
```

---

### Example 4: CSV Parser

```java
/**
 * CSVParser - Parse CSV data (common for Angular backend integration)
 */
public class CSVParser {
    
    /**
     * Parse CSV line into array
     */
    public static String[] parseCSVLine(String line) {
        return line.split(",");
    }
    
    /**
     * Parse CSV with quotes handling
     */
    public static String[] parseCSVWithQuotes(String line) {
        // Simple implementation - split by comma not inside quotes
        java.util.List<String> result = new java.util.ArrayList<>();
        StringBuilder current = new StringBuilder();
        boolean inQuotes = false;
        
        for (char c : line.toCharArray()) {
            if (c == '"') {
                inQuotes = !inQuotes;
            } else if (c == ',' && !inQuotes) {
                result.add(current.toString().trim());
                current = new StringBuilder();
            } else {
                current.append(c);
            }
        }
        result.add(current.toString().trim());
        
        return result.toArray(new String[0]);
    }
    
    /**
     * Create CSV line from array
     */
    public static String createCSVLine(String[] data) {
        return String.join(",", data);
    }
    
    /**
     * Parse entire CSV file
     */
    public static String[][] parseCSV(String csvData) {
        String[] lines = csvData.split("\n");
        String[][] result = new String[lines.length][];
        
        for (int i = 0; i < lines.length; i++) {
            result[i] = parseCSVLine(lines[i]);
        }
        
        return result;
    }
    
    public static void main(String[] args) {
        System.out.println("=== CSV PARSER DEMO ===\n");
        
        // Sample CSV data
        String csvLine = "John,Doe,25,john@example.com";
        System.out.println("Input: " + csvLine);
        
        String[] parsed = parseCSVLine(csvLine);
        System.out.println("Parsed:");
        for (int i = 0; i < parsed.length; i++) {
            System.out.println("  [" + i + "]: " + parsed[i]);
        }
        
        // Create CSV line
        String[] data = {"Alice", "Smith", "30", "alice@example.com"};
        System.out.println("\nCreated CSV: " + createCSVLine(data));
        
        // Multi-line CSV
        String csvData = "Name,Age,Email\nJohn,25,john@test.com\nJane,30,jane@test.com";
        System.out.println("\n--- Multi-line CSV ---");
        String[][] multi = parseCSV(csvData);
        for (String[] row : multi) {
            for (String cell : row) {
                System.out.print(cell + " | ");
            }
            System.out.println();
        }
    }
}
```

---

## 9. Exercises

### Exercise 1: Palindrome Checker

**Requirements:**
1. Create a method to check if a string is a palindrome
2. Handle case-insensitivity
3. Ignore spaces and punctuation
4. Test with: "racecar", "A man a plan a canal Panama", "hello"

---

### Exercise 2: Word Counter

**Requirements:**
1. Count total words in a sentence
2. Count occurrences of each word
3. Find the most frequent word
4. Use String methods: split, toLowerCase, trim

---

### Exercise 3: Password Strength Checker

**Requirements:**
1. Check minimum 8 characters
2. At least one uppercase letter
3. At least one lowercase letter
4. At least one digit
5. At least one special character

---

## 10. Solutions

### Solution 1: Palindrome Checker

```java
public class PalindromeChecker {
    
    public static boolean isPalindrome(String str) {
        // Remove spaces and convert to lowercase
        String cleaned = str.toLowerCase().replaceAll("[^a-z0-9]", "");
        
        // Reverse and compare
        String reversed = new StringBuilder(cleaned).reverse().toString();
        
        return cleaned.equals(reversed);
    }
    
    public static void main(String[] args) {
        String[] testStrings = {
            "racecar",
            "A man a plan a canal Panama",
            "hello",
            "Was it a car or a cat I saw"
        };
        
        for (String s : testStrings) {
            System.out.println("\"" + s + "\" is palindrome: " + isPalindrome(s));
        }
    }
}
```

---

### Solution 2: Word Counter

```java
import java.util.*;

public class WordCounter {
    
    public static void countWords(String sentence) {
        // Clean and split
        String[] words = sentence.toLowerCase().trim().split("\\s+");
        
        // Count occurrences
        Map<String, Integer> wordCount = new HashMap<>();
        for (String word : words) {
            wordCount.put(word, wordCount.getOrDefault(word, 0) + 1);
        }
        
        // Find most frequent
        String mostFrequent = "";
        int maxCount = 0;
        for (Map.Entry<String, Integer> entry : wordCount.entrySet()) {
            if (entry.getValue() > maxCount) {
                maxCount = entry.getValue();
                mostFrequent = entry.getKey();
            }
        }
        
        // Output
        System.out.println("Total words: " + words.length);
        System.out.println("Unique words: " + wordCount.size());
        System.out.println("Most frequent: \"" + mostFrequent + "\" (" + maxCount + " times)");
        System.out.println("Word frequencies: " + wordCount);
    }
    
    public static void main(String[] args) {
        String sentence = "the quick brown fox jumps over the lazy dog the dog was not amused";
        countWords(sentence);
    }
}
```

---

### Solution 3: Password Strength Checker

```java
public class PasswordStrengthChecker {
    
    public static String checkStrength(String password) {
        if (password == null || password.length() < 8) {
            return "Weak - Too short";
        }
        
        boolean hasUpper = false, hasLower = false, hasDigit = false, hasSpecial = false;
        
        for (char c : password.toCharArray()) {
            if (Character.isUpperCase(c)) hasUpper = true;
            else if (Character.isLowerCase(c)) hasLower = true;
            else if (Character.isDigit(c)) hasDigit = true;
            else hasSpecial = true;
        }
        
        int strength = 0;
        if (hasUpper) strength++;
        if (hasLower) strength++;
        if (hasDigit) strength++;
        if (hasSpecial) strength++;
        
        return switch (strength) {
            case 4 -> "Strong";
            case 3 -> "Medium";
            default -> "Weak";
        };
    }
    
    public static void main(String[] args) {
        String[] passwords = {"pass", "password", "Password1", "Pass@word1", "Str0ng!Pass"};
        
        for (String p : passwords) {
            System.out.println(p + ": " + checkStrength(p));
        }
    }
}
```

---

## Summary

### Key Takeaways

1. **Strings are immutable** - Use StringBuilder for modifications
2. **String pool** - Literals are reused for efficiency
3. **Common methods** - length, charAt, substring, indexOf, replace, split
4. **StringBuilder** - Efficient for string concatenation
5. **Regex** - Powerful pattern matching with Pattern/Matcher

### Angular Backend Connection

- JSON uses string data extensively
- Input validation in Java mirrors Angular validation
- String formatting for API responses

---

*Happy Coding! 🚀*
