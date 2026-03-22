// Example31: String Methods - Beginner Tutorial
// This covers the most important String methods in Java

public class Example31 {
    public static void main(String[] args) {
        
        // ===== CREATING STRINGS =====
        System.out.println("=== Creating Strings ===\n");
        
        // Method 1: Using string literal
        String str1 = "Hello";
        
        // Method 2: Using new keyword
        String str2 = new String("World");
        
        // Method 3: From character array
        char[] chars = {'J', 'A', 'V', 'A'};
        String str3 = new String(chars);
        
        System.out.println("str1: " + str1);
        System.out.println("str2: " + str2);
        System.out.println("str3: " + str3);
        
        // ===== LENGTH METHOD =====
        System.out.println("\n=== length() ===\n");
        
        String text = "Hello Java!";
        System.out.println("String: \"" + text + "\"");
        System.out.println("Length: " + text.length());
        
        // ===== CHARACTER ACCESS =====
        System.out.println("\n=== charAt() and toCharArray() ===\n");
        
        String word = "Java";
        
        System.out.println("String: \"" + word + "\"");
        
        // Get character at index
        System.out.println("Character at index 0: " + word.charAt(0));
        System.out.println("Character at index 2: " + word.charAt(2));
        
        // Convert to character array
        char[] charArray = word.toCharArray();
        System.out.print("As char array: ");
        for (char c : charArray) {
            System.out.print(c + " ");
        }
        System.out.println();
        
        // ===== CASE CONVERSION =====
        System.out.println("\n=== toUpperCase() and toLowerCase() ===\n");
        
        String mixed = "HeLLo WoRLD";
        System.out.println("Original: \"" + mixed + "\"");
        System.out.println("Uppercase: \"" + mixed.toUpperCase() + "\"");
        System.out.println("Lowercase: \"" + mixed.toLowerCase() + "\"");
        
        // ===== TRIM =====
        System.out.println("\n=== trim() ===\n");
        
        String withSpaces = "   Hello Java   ";
        System.out.println("Before trim: \"" + withSpaces + "\"");
        System.out.println("After trim: \"" + withSpaces.trim() + "\"");
        
        // ===== SUBSTRING =====
        System.out.println("\n=== substring() ===\n");
        
        String sentence = "Hello World";
        System.out.println("Original: \"" + sentence + "\"");
        
        System.out.println("substring(6): \"" + sentence.substring(6) + "\"");
        System.out.println("substring(0, 5): \"" + sentence.substring(0, 5) + "\"");
        System.out.println("substring(6, 11): \"" + sentence.substring(6, 11) + "\"");
        
        // ===== REPLACE =====
        System.out.println("\n=== replace() ===\n");
        
        String original = "Hello World";
        System.out.println("Original: \"" + original + "\"");
        
        System.out.println("replace('o', 'X'): \"" + original.replace('o', 'X') + "\"");
        System.out.println("replace(\"World\", \"Java\"): \"" + original.replace("World", "Java") + "\"");
        System.out.println("replaceAll(\"l\", \"L\"): \"" + original.replaceAll("l", "L") + "\"");
        
        // ===== SPLIT =====
        System.out.println("\n=== split() ===\n");
        
        String csv = "apple,banana,cherry,date";
        System.out.println("Original: \"" + csv + "\"");
        
        String[] fruits = csv.split(",");
        System.out.println("After split by comma:");
        for (String fruit : fruits) {
            System.out.println("  - " + fruit);
        }
        
        String sentence2 = "Java is a programming language";
        String[] words = sentence2.split(" ");
        System.out.println("\nSplit by space:");
        for (String w : words) {
            System.out.println("  - " + w);
        }
        
        // ===== INDEXOF =====
        System.out.println("\n=== indexOf() and lastIndexOf() ===\n");
        
        String text2 = "Hello World, Welcome to Java";
        System.out.println("String: \"" + text2 + "\"");
        
        System.out.println("indexOf('o'): " + text2.indexOf('o'));
        System.out.println("indexOf(\"Java\"): " + text2.indexOf("Java"));
        System.out.println("indexOf('o', 5): " + text2.indexOf('o', 5));
        System.out.println("lastIndexOf('o'): " + text2.lastIndexOf('o'));
        
        // ===== CONTAINS =====
        System.out.println("\n=== contains() ===\n");
        
        String str = "Hello World";
        System.out.println("String: \"" + str + "\"");
        
        System.out.println("contains(\"World\"): " + str.contains("World"));
        System.out.println("contains(\"Java\"): " + str.contains("Java"));
        System.out.println("contains(\"Hello\"): " + str.contains("Hello"));
        
        // ===== STARTSWITH AND ENDSWITH =====
        System.out.println("\n=== startsWith() and endsWith() ===\n");
        
        String filename = "document.pdf";
        System.out.println("String: \"" + filename + "\"");
        
        System.out.println("startsWith(\"doc\"): " + filename.startsWith("doc"));
        System.out.println("startsWith(\"document\"): " + filename.startsWith("document"));
        System.out.println("endsWith(\".pdf\"): " + filename.endsWith(".pdf"));
        System.out.println("endsWith(\".txt\"): " + filename.endsWith(".txt"));
        
        // ===== EQUALS =====
        System.out.println("\n=== equals() and equalsIgnoreCase() ===\n");
        
        String s1 = "Java";
        String s2 = "Java";
        String s3 = "java";
        String s4 = new String("Java");
        
        System.out.println("s1 = \"Java\"");
        System.out.println("s2 = \"Java\"");
        System.out.println("s3 = \"java\"");
        System.out.println("s4 = new String(\"Java\")");
        
        System.out.println("\ns1.equals(s2): " + s1.equals(s2));
        System.out.println("s1.equals(s3): " + s1.equals(s3));
        System.out.println("s1.equalsIgnoreCase(s3): " + s1.equalsIgnoreCase(s3));
        System.out.println("s1 == s2: " + (s1 == s2));  // Same reference
        System.out.println("s1 == s4: " + (s1 == s4));  // Different objects
        
        // ===== COMPARETO =====
        System.out.println("\n=== compareTo() ===\n");
        
        String a = "apple";
        String b = "banana";
        String c = "apple";
        
        System.out.println("a = \"apple\"");
        System.out.println("b = \"banana\"");
        System.out.println("c = \"apple\"");
        
        System.out.println("\na.compareTo(b): " + a.compareTo(b));
        System.out.println("b.compareTo(a): " + b.compareTo(a));
        System.out.println("a.compareTo(c): " + a.compareTo(c));
        
        // ===== ISEMPTY =====
        System.out.println("\n=== isEmpty() ===\n");
        
        String empty = "";
        String notEmpty = "Hello";
        
        System.out.println("\"\".isEmpty(): " + empty.isEmpty());
        System.out.println("\"Hello\".isEmpty(): " + notEmpty.isEmpty());
        
        // ===== CONCAT =====
        System.out.println("\n=== concat() ===\n");
        
        String part1 = "Hello";
        String part2 = "World";
        
        System.out.println("part1: \"" + part1 + "\"");
        System.out.println("part2: \"" + part2 + "\"");
        System.out.println("part1.concat(\" \" + part2): \"" + part1.concat(" " + part2) + "\"");
        
        // Using + operator
        System.out.println("part1 + \" \" + part2: \"" + part1 + " " + part2 + "\"");
        
        // ===== VALUEOF =====
        System.out.println("\n=== String.valueOf() ===\n");
        
        int num = 123;
        double d = 45.67;
        boolean b2 = true;
        char[] arr = {'J', 'A', 'V', 'A'};
        
        System.out.println("String.valueOf(123): \"" + String.valueOf(num) + "\"");
        System.out.println("String.valueOf(45.67): \"" + String.valueOf(d) + "\"");
        System.out.println("String.valueOf(true): \"" + String.valueOf(b2) + "\"");
        System.out.println("String.valueOf(char[]): \"" + String.valueOf(arr) + "\"");
        
        // ===== FORMAT =====
        System.out.println("\n=== String.format() ===\n");
        
        String name = "John";
        int age = 25;
        double gpa = 3.75;
        
        System.out.println(String.format("Name: %s", name));
        System.out.println(String.format("Age: %d", age));
        System.out.println(String.format("GPA: %.2f", gpa));
        System.out.println(String.format("%s is %d years old", name, age));
        
        // ===== PRACTICAL EXAMPLES =====
        System.out.println("\n=== Practical Examples ===\n");
        
        // Validate email
        String email = "user@example.com";
        System.out.println("Email: " + email);
        
        boolean hasAt = email.contains("@");
        boolean hasDot = email.contains(".");
        
        if (hasAt && hasDot) {
            System.out.println("Valid email format");
        } else {
            System.out.println("Invalid email format");
        }
        
        // Reverse string
        String reverseMe = "Java";
        System.out.println("\nOriginal: " + reverseMe);
        
        String reversed = "";
        for (int i = reverseMe.length() - 1; i >= 0; i--) {
            reversed += reverseMe.charAt(i);
        }
        System.out.println("Reversed: " + reversed);
        
        // Count vowels
        String countMe = "Hello World";
        System.out.println("\nCounting vowels in: \"" + countMe + "\"");
        
        int vowelCount = 0;
        countMe = countMe.toLowerCase();
        for (int i = 0; i < countMe.length(); i++) {
            char c2 = countMe.charAt(i);
            if (c2 == 'a' || c2 == 'e' || c2 == 'i' || c2 == 'o' || c2 == 'u') {
                vowelCount++;
            }
        }
        System.out.println("Vowel count: " + vowelCount);
        
        // ===== COMMON METHODS SUMMARY =====
        System.out.println("\n=== Common String Methods Summary ===\n");
        
        System.out.println("length()          - Returns string length");
        System.out.println("charAt(index)     - Returns character at index");
        System.out.println("toUpperCase()     - Converts to uppercase");
        System.out.println("toLowerCase()     - Converts to lowercase");
        System.out.println("trim()            - Removes leading/trailing spaces");
        System.out.println("substring()       - Extracts part of string");
        System.out.println("replace()         - Replaces characters/strings");
        System.out.println("split()           - Splits string into array");
        System.out.println("indexOf()         - Finds position of character/string");
        System.out.println("contains()        - Checks if string contains substring");
        System.out.println("startsWith()      - Checks if starts with prefix");
        System.out.println("endsWith()        - Checks if ends with suffix");
        System.out.println("equals()          - Compares two strings (case-sensitive)");
        System.out.println("equalsIgnoreCase() - Compares strings (case-insensitive)");
        System.out.println("isEmpty()         - Checks if string is empty");
        System.out.println("concat()          - Joins two strings");
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. IMMUTABLE:
 *    - String objects cannot be changed
 *    - Methods return new strings
 *    - Original string remains unchanged
 * 
 * 2. LENGTH vs CAPACITY:
 *    - length(): number of characters
 *    - No capacity method for String
 * 
 * 3. == vs equals():
 *    - == compares references
 *    - equals() compares content
 *    - Always use equals() for string comparison
 * 
 * 4. STRING POOL:
 *    - Java stores string literals in a pool
 *    - Same literals share memory
 *    - new String() creates new object
 * 
 * 5. COMMON MISTAKES:
 *    - Using == instead of equals()
 *    - Forgetting strings are immutable
 *    - Null pointer exceptions
 */
