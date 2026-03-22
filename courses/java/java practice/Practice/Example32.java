/*
 * SUB TOPIC: StringBuilder and StringBuffer
 * 
 * DEFINITION:
 * StringBuilder and StringBuffer are mutable character sequences in Java. Unlike String which 
 * is immutable (cannot be changed), these classes allow modification of the string content 
 * without creating new objects. StringBuilder is faster (not thread-safe), while StringBuffer 
 * is synchronized (thread-safe).
 * 
 * FUNCTIONALITIES:
 * 1. append() - Adds content to the end
 * 2. insert() - Inserts content at specified position
 * 3. replace() - Replaces portion of string
 * 4. delete() - Removes portion of string
 * 5. reverse() - Reverses the character sequence
 * 6. capacity() - Returns internal buffer capacity
 * 7. setCharAt() - Modifies character at specific index
 */

public class Example32 {
    public static void main(String[] args) {
        
        // Topic Explanation: Why use StringBuilder?
        
        // Problem with String: creates new objects on each modification
        String text = "Hello";
        text += " World"; // Creates new object
        text += "!"; // Creates another new object
        System.out.println("String result: " + text);
        
        // Solution: StringBuilder - mutable, no new objects created
        StringBuilder sb = new StringBuilder("Hello");
        sb.append(" World"); // Modifies in place
        sb.append("!"); // Still same object
        System.out.println("StringBuilder result: " + sb.toString());
        
        // append() - Add to the end of StringBuilder
        StringBuilder sb1 = new StringBuilder("Start");
        sb1.append(" Middle"); // Adds to end
        sb1.append(" End"); // Adds to end
        System.out.println("\nAfter append: " + sb1.toString());
        
        // insert() - Insert at specific position (index, string)
        StringBuilder sb2 = new StringBuilder("Hello World");
        sb2.insert(5, ", "); // Insert at index 5
        System.out.println("After insert: " + sb2.toString());
        
        // replace() - Replace portion (startIndex, endIndex, newString)
        StringBuilder sb3 = new StringBuilder("Hello World");
        sb3.replace(6, 11, "Java"); // Replace from index 6 to 11
        System.out.println("After replace: " + sb3.toString());
        
        // delete() - Remove portion (startIndex, endIndex)
        StringBuilder sb4 = new StringBuilder("Hello World!");
        sb4.delete(5, 7); // Delete from index 5 to 7
        System.out.println("After delete: " + sb4.toString());
        
        // reverse() - Reverse the entire sequence
        StringBuilder sb5 = new StringBuilder("Java");
        sb5.reverse();
        System.out.println("After reverse: " + sb5.toString());
        
        // capacity() vs length()
        StringBuilder sb6 = new StringBuilder();
        System.out.println("\nDefault capacity: " + sb6.capacity()); // 16
        sb6.append("Hello");
        System.out.println("After adding 'Hello' - Length: " + sb6.length() + ", Capacity: " + sb6.capacity());
        
        // setCharAt() - Modify single character
        StringBuilder sb7 = new StringBuilder("Hello");
        sb7.setCharAt(0, 'J'); // Change 'H' to 'J'
        System.out.println("After setCharAt: " + sb7.toString());
        
        // Real-time Example 1: Building dynamic SQL query
        System.out.println("\n=== Example 1: Building SQL Query ===");
        StringBuilder query = new StringBuilder("SELECT * FROM users");
        String[] conditions = {"active=true", "age>18", "country='USA'"};
        
        if (conditions.length > 0) {
            query.append(" WHERE ");
            for (int i = 0; i < conditions.length; i++) {
                query.append(conditions[i]);
                if (i < conditions.length - 1) {
                    query.append(" AND ");
                }
            }
        }
        System.out.println("Generated Query: " + query.toString());
        
        // Real-time Example 2: Chat message builder
        System.out.println("\n=== Example 2: Chat Message ===");
        StringBuilder message = new StringBuilder();
        message.append("<div class='message'>");
        message.append("<span class='sender'>John:</span>");
        message.append("<span class='text'>Hello everyone!</span>");
        message.append("</div>");
        System.out.println("HTML Message: " + message.toString());
        
        // Real-time Example 3: URL builder for API calls
        System.out.println("\n=== Example 3: Building API URL ===");
        StringBuilder url = new StringBuilder("https://api.example.com");
        url.append("/users"); // Base endpoint
        url.append("?page=1"); // Query param
        url.append("&limit=10"); // Another param
        System.out.println("API URL: " + url.toString());
        
        // Real-time Example 4: Log message builder
        System.out.println("\n=== Example 4: Building Log Entry ===");
        StringBuilder log = new StringBuilder();
        log.append("[ERROR] ");
        log.append("2024-01-15 10:30:45 ");
        log.append("User login failed for username: admin");
        System.out.println("Log: " + log.toString());
        
        // Real-time Example 5: Palindrome check using reverse
        System.out.println("\n=== Example 5: Palindrome Check ===");
        StringBuilder palindromeTest = new StringBuilder("madam");
        StringBuilder reversed = new StringBuilder(palindromeTest);
        reversed.reverse();
        if (palindromeTest.toString().equals(reversed.toString())) {
            System.out.println("'" + palindromeTest + "' is a palindrome");
        } else {
            System.out.println("'" + palindromeTest + "' is not a palindrome");
        }
        
        // Real-time Example 6: JSON string builder
        System.out.println("\n=== Example 6: Building JSON ===");
        StringBuilder json = new StringBuilder();
        json.append("{");
        json.append("\"name\":\"John\",");
        json.append("\"age\":30,");
        json.append("\"city\":\"New York\"");
        json.append("}");
        System.out.println("JSON: " + json.toString());
        
        // StringBuffer: Same API but thread-safe
        System.out.println("\n=== StringBuffer Example ===");
        StringBuffer buffer = new StringBuffer("Hello");
        buffer.append(" World");
        System.out.println("StringBuffer result: " + buffer.toString());
    }
}
