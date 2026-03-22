/*
 * SUB TOPIC: StringTokenizer in Java
 * 
 * DEFINITION:
 * StringTokenizer is a class in java.util that breaks a string into tokens based on delimiters.
 * It is a legacy class but still widely used for simple tokenization. It implements the 
 * Enumeration interface and provides methods to iterate through tokens one at a time.
 * 
 * FUNCTIONALITIES:
 * 1. nextToken() - Returns next token
 * 2. hasMoreTokens() - Checks if more tokens exist
 * 3. countTokens() - Returns total number of tokens
 * 4. nextToken(delimiter) - Set new delimiter
 * 5. hasMoreElements() - Part of Enumeration interface
 * 6. nextElement() - Part of Enumeration interface
 */

import java.util.*;

public class Example97 {
    public static void main(String[] args) {
        
        // Basic StringTokenizer with default delimiter (whitespace)
        System.out.println("=== Basic StringTokenizer ===");
        String text = "Hello World Java";
        StringTokenizer tokenizer = new StringTokenizer(text);
        
        System.out.println("Tokenizing: \"" + text + "\"");
        System.out.println("Number of tokens: " + tokenizer.countTokens());
        
        while (tokenizer.hasMoreTokens()) {
            System.out.println("Token: " + tokenizer.nextToken());
        }
        
        // StringTokenizer with custom delimiter
        System.out.println("\n=== Custom Delimiter ===");
        String csv = "apple,banana,cherry,date";
        StringTokenizer csvTokenizer = new StringTokenizer(csv, ",");
        
        while (csvTokenizer.hasMoreTokens()) {
            System.out.println("Fruit: " + csvTokenizer.nextToken());
        }
        
        // Multiple delimiters
        System.out.println("\n=== Multiple Delimiters ===");
        String data = "apple@banana#cherry$date";
        StringTokenizer multiDelim = new StringTokenizer(data, "@#$");
        
        while (multiDelim.hasMoreTokens()) {
            System.out.println("Token: " + multiDelim.nextToken());
        }
        
        // Include delimiters as tokens
        System.out.println("\n=== Include Delimiters ===");
        String withDelim = "apple,banana,cherry";
        StringTokenizer includeDelim = new StringTokenizer(withDelim, ",", true); // true = return delimiters
        
        while (includeDelim.hasMoreTokens()) {
            System.out.println("Token: " + includeDelim.nextToken());
        }
        
        // Real-time Example 1: CSV parsing
        System.out.println("\n=== Example 1: CSV Parsing ===");
        String csvData = "John,Doe,25,New York";
        StringTokenizer csvParser = new StringTokenizer(csvData, ",");
        
        String firstName = csvParser.nextToken();
        String lastName = csvParser.nextToken();
        String age = csvParser.nextToken();
        String city = csvParser.nextToken();
        
        System.out.println("First Name: " + firstName);
        System.out.println("Last Name: " + lastName);
        System.out.println("Age: " + age);
        System.out.println("City: " + city);
        
        // Real-time Example 2: Log file parsing
        System.out.println("\n=== Example 2: Log Parsing ===");
        String logEntry = "2024-01-15 10:30:45 ERROR Database connection failed";
        StringTokenizer logParser = new StringTokenizer(logEntry, " ");
        
        String date = logParser.nextToken();
        String time = logParser.nextToken();
        String level = logParser.nextToken();
        String message = "";
        while (logParser.hasMoreTokens()) {
            message += logParser.nextToken() + " ";
        }
        
        System.out.println("Date: " + date);
        System.out.println("Time: " + time);
        System.out.println("Level: " + level);
        System.out.println("Message: " + message.trim());
        
        // Real-time Example 3: Command parsing
        System.out.println("\n=== Example 3: Command Parsing ===");
        String command = "SEND user@email.com Hello there!";
        StringTokenizer cmdParser = new StringTokenizer(command, " ", true);
        
        List<String> tokens = new ArrayList<>();
        while (cmdParser.hasMoreTokens()) {
            tokens.add(cmdParser.nextToken());
        }
        System.out.println("Command tokens: " + tokens);
        
        // Parse as command
        String cmdText = "SEND|user@example.com|Hello World";
        StringTokenizer sendCmd = new StringTokenizer(cmdText, "|");
        
        String action = sendCmd.nextToken();
        String recipient = sendCmd.nextToken();
        String content = sendCmd.nextToken();
        
        System.out.println("Action: " + action);
        System.out.println("Recipient: " + recipient);
        System.out.println("Content: " + content);
        
        // Real-time Example 4: URL parsing
        System.out.println("\n=== Example 4: URL Parsing ===");
        String url = "https://api.example.com:8080/users/123/profile";
        StringTokenizer urlParser = new StringTokenizer(url, "/:");
        
        System.out.println("URL Parts:");
        while (urlParser.hasMoreTokens()) {
            System.out.println("  " + urlParser.nextToken());
        }
        
        // Real-time Example 5: Data record parsing
        System.out.println("\n=== Example 5: Employee Data ===");
        String employeeData = "EMP001$John Doe$Developer$50000";
        StringTokenizer empParser = new StringTokenizer(employeeData, "$");
        
        String empId = empParser.nextToken();
        String empName = empParser.nextToken();
        String empRole = empParser.nextToken();
        String empSalary = empParser.nextToken();
        
        System.out.println("ID: " + empId);
        System.out.println("Name: " + empName);
        System.out.println("Role: " + empRole);
        System.out.println("Salary: " + empSalary);
        
        // Real-time Example 6: IP address parsing
        System.out.println("\n=== Example 6: IP Address ===");
        String ipAddress = "192.168.1.100";
        StringTokenizer ipParser = new StringTokenizer(ipAddress, ".");
        
        int[] octets = new int[4];
        int i = 0;
        while (ipParser.hasMoreTokens()) {
            octets[i++] = Integer.parseInt(ipParser.nextToken());
        }
        
        System.out.println("IP Address Components:");
        System.out.println("  Octet 1: " + octets[0]);
        System.out.println("  Octet 2: " + octets[1]);
        System.out.println("  Octet 3: " + octets[2]);
        System.out.println("  Octet 4: " + octets[3]);
        
        // Additional operations
        System.out.println("\n=== Additional Operations ===");
        String test = "A B C D";
        StringTokenizer testToken = new StringTokenizer(test);
        
        System.out.println("Count tokens: " + testToken.countTokens()); // 4
        
        // Use hasMoreElements() (Enumeration interface)
        String enumTest = "one two three";
        StringTokenizer enumToken = new StringTokenizer(enumTest);
        
        System.out.println("Using hasMoreElements:");
        while (enumToken.hasMoreElements()) {
            System.out.println("  " + enumToken.nextElement());
        }
        
        // Changing delimiters mid-iteration
        System.out.println("\n=== Changing Delimiters ===");
        String multiDelim2 = "apple+banana,cherry";
        StringTokenizer changeDelim = new StringTokenizer(multiDelim2, "+");
        
        System.out.println("Initial delimiter (+):");
        while (changeDelim.hasMoreTokens()) {
            System.out.println("  " + changeDelim.nextToken());
        }
        
        // Use nextToken with new delimiter
        String newDelimTest = "cat-dog,bird";
        StringTokenizer newDelimToken = new StringTokenizer(newDelimTest, "-");
        
        System.out.println("\nFirst token: " + newDelimToken.nextToken());
        System.out.println("Switching to comma:");
        newDelimToken = new StringTokenizer(newDelimToken.nextToken(","), ",");
        
        while (newDelimToken.hasMoreTokens()) {
            System.out.println("  " + newDelimToken.nextToken());
        }
        
        // Using countTokens() in a loop
        System.out.println("\n=== Using countTokens() ===");
        String countTest = "1,2,3,4,5";
        StringTokenizer countToken = new StringTokenizer(countTest, ",");
        
        int tokenCount = countToken.countTokens();
        System.out.println("Total tokens: " + tokenCount);
        
        for (int j = 0; j < tokenCount; j++) {
            System.out.println("Token " + (j + 1) + ": " + countToken.nextToken());
        }
    }
}
