// Example32: StringBuilder and StringBuffer - Beginner Tutorial
// This explains mutable strings in Java

/*
 * WHY STRING IS IMMUTABLE?
 * -----------------------
 * String str = "Hello";
 * str = str + " World";  // Creates new object, old one is garbage
 * 
 * If you modify strings many times, it creates many objects
 * This is inefficient for memory
 * 
 * SOLUTION: StringBuilder and StringBuffer
 * 
 * StringBuilder: Fast, not thread-safe
 * StringBuffer: Slower, thread-safe (synchronized)
 */

public class Example32 {
    public static void main(String[] args) {
        
        // ===== STRING PROBLEM =====
        System.out.println("=== Problem with String ===\n");
        
        String text = "Hello";
        
        // This creates a NEW string object each time!
        // Old "Hello" becomes garbage
        text += " World";  // Creates new object
        text += "!";        // Creates another new object
        
        System.out.println("Result: " + text);
        System.out.println("Problem: Created multiple temporary objects!");
        
        // ===== STRINGBUILDER =====
        System.out.println("\n=== StringBuilder ===\n");
        
        // Create StringBuilder
        StringBuilder sb = new StringBuilder("Hello");
        
        // append() - Add to the end
        sb.append(" World");
        sb.append("!");
        
        System.out.println("After append: " + sb.toString());
        
        // insert() - Insert at position
        StringBuilder sb2 = new StringBuilder("Hello World");
        sb2.insert(5, ", ");  // Insert at index 5
        
        System.out.println("After insert: " + sb2.toString());
        
        // replace() - Replace portion
        StringBuilder sb3 = new StringBuilder("Hello World");
        sb3.replace(6, 11, "Java");  // Replace from index 6 to 11
        
        System.out.println("After replace: " + sb3.toString());
        
        // delete() - Delete portion
        StringBuilder sb4 = new StringBuilder("Hello World!");
        sb4.delete(5, 7);  // Delete from index 5 to 7
        
        System.out.println("After delete: " + sb4.toString());
        
        // reverse()
        StringBuilder sb5 = new StringBuilder("Java");
        sb5.reverse();
        
        System.out.println("After reverse: " + sb5.toString());
        
        // capacity()
        StringBuilder sb6 = new StringBuilder();
        System.out.println("\nDefault capacity: " + sb6.capacity());
        
        sb6 = new StringBuilder(50);
        System.out.println("Capacity with size 50: " + sb6.capacity());
        
        // length()
        StringBuilder sb7 = new StringBuilder("Hello");
        System.out.println("Length: " + sb7.length());
        
        // charAt()
        StringBuilder sb8 = new StringBuilder("Hello");
        System.out.println("Char at index 1: " + sb8.charAt(1));
        
        // setCharAt()
        StringBuilder sb9 = new StringBuilder("Hello");
        sb9.setCharAt(0, 'J');
        System.out.println("After setCharAt: " + sb9.toString());
        
        // ===== STRINGBUFFER =====
        System.out.println("\n=== StringBuffer ===\n");
        
        // StringBuffer is same as StringBuilder but thread-safe
        StringBuffer sbf = new StringBuffer("Hello");
        
        sbf.append(" World");
        System.out.println("After append: " + sbf.toString());
        
        sbf.insert(5, ",");
        System.out.println("After insert: " + sbf.toString());
        
        sbf.replace(6, 11, "Java");
        System.out.println("After replace: " + sbf.toString());
        
        sbf.reverse();
        System.out.println("After reverse: " + sbf.toString());
        
        // ===== PERFORMANCE COMPARISON =====
        System.out.println("\n=== Performance Comparison ===\n");
        
        // Using String (slow - creates many objects)
        long startTime = System.currentTimeMillis();
        
        String slow = "";
        for (int i = 0; i < 1000; i++) {
            slow += "a";
        }
        
        long stringTime = System.currentTimeMillis() - startTime;
        System.out.println("String time: " + stringTime + " ms");
        
        // Using StringBuilder (fast)
        startTime = System.currentTimeMillis();
        
        StringBuilder fast = new StringBuilder();
        for (int i = 0; i < 1000; i++) {
            fast.append("a");
        }
        
        long builderTime = System.currentTimeMillis() - startTime;
        System.out.println("StringBuilder time: " + builderTime + " ms");
        
        // Using StringBuffer
        startTime = System.currentTimeMillis();
        
        StringBuffer buffer = new StringBuffer();
        for (int i = 0; i < 1000; i++) {
            buffer.append("a");
        }
        
        long bufferTime = System.currentTimeMillis() - startTime;
        System.out.println("StringBuffer time: " + bufferTime + " ms");
        
        // ===== PRACTICAL EXAMPLES =====
        System.out.println("\n=== Practical Examples ===\n");
        
        // Build a sentence
        StringBuilder sentence = new StringBuilder();
        
        String[] words = {"I", "love", "Java", "programming"};
        
        for (int i = 0; i < words.length; i++) {
            sentence.append(words[i]);
            if (i < words.length - 1) {
                sentence.append(" ");
            }
        }
        
        System.out.println("Built sentence: " + sentence.toString());
        
        // Palindrome check
        StringBuilder palindrome = new StringBuilder("madam");
        StringBuilder original = new StringBuilder("madam");
        
        palindrome.reverse();
        
        if (original.toString().equals(palindrome.toString())) {
            System.out.println("madam is a palindrome");
        } else {
            System.out.println("madam is not a palindrome");
        }
        
        // Reverse each word in a sentence
        StringBuilder text2 = new StringBuilder("Hello World");
        
        // Reverse entire string
        text2.reverse();
        System.out.println("Reversed: " + text2.toString());
        
        // ===== WHEN TO USE WHAT =====
        System.out.println("\n=== When to Use What? ===\n");
        
        System.out.println("Use String when:");
        System.out.println("  - Text won't change");
        System.out.println("  - Need thread safety (immutable)");
        System.out.println("  - Using in HashMap/HashSet keys");
        
        System.out.println("\nUse StringBuilder when:");
        System.out.println("  - Building/modifying strings frequently");
        System.out.println("  - Single-threaded environment");
        System.out.println("  - Performance is important");
        
        System.out.println("\nUse StringBuffer when:");
        System.out.println("  - Multiple threads modify the same string");
        System.out.println("  - Thread safety is required");
        System.out.println("  - Shared resource environment");
        
        // ===== COMMON METHODS =====
        System.out.println("\n=== Common Methods Summary ===\n");
        
        System.out.println("append()     - Add to end");
        System.out.println("insert()    - Insert at position");
        System.out.println("replace()   - Replace portion");
        System.out.println("delete()    - Delete portion");
        System.out.println("reverse()   - Reverse the string");
        System.out.println("toString()  - Convert to String");
        System.out.println("length()    - Get length");
        System.out.println("capacity()  - Get capacity");
        System.out.println("charAt()    - Get character at index");
        System.out.println("setCharAt() - Set character at index");
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. WHY MUTABLE STRINGS?
 *    - String is immutable (cannot change)
 *    - Each modification creates new object
 *    - Wasteful for frequent modifications
 * 
 * 2. STRINGBUILDER:
 *    - Mutable sequence of characters
 *    - Fast (no synchronization)
 *    - Not thread-safe
 *    - Use in single-threaded code
 * 
 * 3. STRINGBUFFER:
 *    - Same as StringBuilder
 *    - Thread-safe (synchronized)
 *    - Slower than StringBuilder
 *    - Use in multi-threaded code
 * 
 * 4. CAPACITY:
 *    - Default capacity: 16 + initial string length
 *    - Grows automatically (doubles + 2)
 *    - Can specify initial capacity
 * 
 * 5. PERFORMANCE:
 *    - StringBuilder > StringBuffer > String
 *    - Use StringBuilder for best performance
 * 
 * 6. CONVERTING:
 *    - toString() converts to String
 *    - StringBuilder can take String in constructor
 */
