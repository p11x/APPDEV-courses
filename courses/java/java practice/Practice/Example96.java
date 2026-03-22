/*
 * SUB TOPIC: Scanner Advanced Usage in Java
 * 
 * DEFINITION:
 * Scanner is a class in java.util package that breaks down input into tokens using delimiter 
 * patterns. It can parse primitive types and strings using regular expressions. Scanner 
 * can read from various input sources including standard input, files, and strings.
 * 
 * FUNCTIONALITIES:
 * 1. nextInt(), nextFloat(), nextDouble(), nextLong(), nextByte() - Read primitive types
 * 2. next() - Read next token (whitespace-separated)
 * 3. nextLine() - Read entire line
 * 4. hasNextInt(), hasNextFloat(), etc. - Check if next token is specific type
 * 5. useDelimiter() - Set custom delimiter
 * 6. findInLine() - Find pattern without moving to next line
 * 7. skip() - Skip specified pattern
 * 8. close() - Close the scanner
 */

import java.util.*;

public class Example96 {
    public static void main(String[] args) {
        
        // Creating Scanner with predefined input (simulated)
        String input = "John 25 95.5\nJane 30 88.0\nBob 22 92.5\n";
        Scanner scanner = new Scanner(input);
        
        System.out.println("=== Basic Scanner Operations ===");
        
        // next() - Read string token
        String name = scanner.next();
        int age = scanner.nextInt();
        double score = scanner.nextDouble();
        
        System.out.println("Name: " + name);
        System.out.println("Age: " + age);
        System.out.println("Score: " + score);
        
        scanner.close();
        
        // Using hasNext() for safe reading
        System.out.println("\n=== Safe Reading with hasNext() ===");
        String input2 = "Alice Bob Charlie";
        Scanner scanner2 = new Scanner(input2);
        
        while (scanner2.hasNext()) {
            System.out.println("Token: " + scanner2.next());
        }
        scanner2.close();
        
        // Reading different data types
        System.out.println("\n=== Reading Different Types ===");
        String input3 = "100 3.14 true hello";
        Scanner scanner3 = new Scanner(input3);
        
        if (scanner3.hasNextInt()) {
            System.out.println("Integer: " + scanner3.nextInt());
        }
        if (scanner3.hasNextDouble()) {
            System.out.println("Double: " + scanner3.nextDouble());
        }
        if (scanner3.hasNextBoolean()) {
            System.out.println("Boolean: " + scanner3.nextBoolean());
        }
        if (scanner3.hasNext()) {
            System.out.println("String: " + scanner3.next());
        }
        scanner3.close();
        
        // Using delimiters
        System.out.println("\n=== Custom Delimiter ===");
        String csvData = "apple,banana,cherry,date";
        Scanner delimScanner = new Scanner(csvData);
        delimScanner.useDelimiter(","); // Set comma as delimiter
        
        while (delimScanner.hasNext()) {
            System.out.println("Fruit: " + delimScanner.next());
        }
        delimScanner.close();
        
        // nextLine() - Read entire line
        System.out.println("\n=== nextLine() Usage ===");
        String multiLine = "Line 1\nLine 2\nLine 3";
        Scanner lineScanner = new Scanner(multiLine);
        
        while (lineScanner.hasNextLine()) {
            System.out.println("Read: " + lineScanner.nextLine());
        }
        lineScanner.close();
        
        // Real-time Example 1: Student data entry
        System.out.println("\n=== Example 1: Student Registration ===");
        String studentData = "John Doe 20 85.5\nJane Smith 22 90.0\n";
        Scanner studentScanner = new Scanner(studentData);
        
        List<String[]> students = new ArrayList<>();
        while (studentScanner.hasNextLine()) {
            String line = studentScanner.nextLine();
            Scanner lineScan = new Scanner(line);
            String firstName = lineScan.next();
            String lastName = lineScan.next();
            int age2 = lineScan.nextInt();
            double gpa = lineScan.nextDouble();
            
            students.add(new String[]{firstName, lastName, String.valueOf(age2), String.valueOf(gpa)});
            lineScan.close();
        }
        
        System.out.println("Registered Students:");
        for (String[] s : students) {
            System.out.println("  " + s[0] + " " + s[1] + ", Age: " + s[2] + ", GPA: " + s[3]);
        }
        studentScanner.close();
        
        // Real-time Example 2: Command parser
        System.out.println("\n=== Example 2: Command Parser ===");
        String commands = "ADD user1\nDELETE user2\nUPDATE user3\nVIEW all";
        Scanner cmdScanner = new Scanner(commands);
        
        while (cmdScanner.hasNextLine()) {
            String line = cmdScanner.nextLine();
            Scanner cmdParse = new Scanner(line);
            String command = cmdParse.next();
            
            switch (command) {
                case "ADD":
                    System.out.println("Adding user: " + cmdParse.next());
                    break;
                case "DELETE":
                    System.out.println("Deleting user: " + cmdParse.next());
                    break;
                case "UPDATE":
                    System.out.println("Updating user: " + cmdParse.next());
                    break;
                case "VIEW":
                    System.out.println("Viewing: " + cmdParse.next());
                    break;
            }
            cmdParse.close();
        }
        cmdScanner.close();
        
        // Real-time Example 3: Price calculator
        System.out.println("\n=== Example 3: Price Calculator ===");
        String priceData = "ProductA 100 2\nProductB 50 3\nProductC 75 1";
        Scanner priceScanner = new Scanner(priceData);
        
        double total = 0;
        while (priceScanner.hasNext()) {
            String product = priceScanner.next();
            double price = priceScanner.nextDouble();
            int quantity = priceScanner.nextInt();
            
            double subtotal = price * quantity;
            total += subtotal;
            System.out.println(product + ": $" + price + " x " + quantity + " = $" + subtotal);
        }
        System.out.println("Total: $" + total);
        priceScanner.close();
        
        // Real-time Example 4: Email validator input
        System.out.println("\n=== Example 4: Email List Processing ===");
        String emailList = "user1@email.com user2@email.com user3@email.com";
        Scanner emailScanner = new Scanner(emailList);
        
        int validCount = 0;
        while (emailScanner.hasNext()) {
            String email = emailScanner.next();
            if (email.contains("@") && email.contains(".")) {
                validCount++;
                System.out.println("Valid email: " + email);
            }
        }
        System.out.println("Total valid: " + validCount);
        emailScanner.close();
        
        // Real-time Example 5: Game score input
        System.out.println("\n=== Example 5: Game Scores ===");
        String scores = "Player1 1500\nPlayer2 2300\nPlayer3 1800\nPlayer4 2800";
        Scanner gameScanner = new Scanner(scores);
        
        String winner = "";
        int highScore = 0;
        
        while (gameScanner.hasNextLine()) {
            Scanner lineParser = new Scanner(gameScanner.nextLine());
            String player = lineParser.next();
            int playerScore = lineParser.nextInt();
            
            System.out.println(player + ": " + playerScore);
            
            if (playerScore > highScore) {
                highScore = playerScore;
                winner = player;
            }
            lineParser.close();
        }
        System.out.println("Winner: " + winner + " with " + highScore + " points!");
        gameScanner.close();
        
        // Real-time Example 6: Inventory system
        System.out.println("\n=== Example 6: Inventory Update ===");
        String inventory = "LAPTOP 10 999.99\nMOUSE 50 29.99\nKEYBOARD 30 79.99";
        Scanner invScanner = new Scanner(inventory);
        
        double totalValue = 0;
        while (invScanner.hasNext()) {
            String item = invScanner.next();
            int qty = invScanner.nextInt();
            double price = invScanner.nextDouble();
            
            double itemValue = qty * price;
            totalValue += itemValue;
            System.out.println(item + ": " + qty + " units @ $" + price + " = $" + itemValue);
        }
        System.out.println("Total Inventory Value: $" + totalValue);
        invScanner.close();
        
        // skip() method
        System.out.println("\n=== Using skip() ===");
        String skipData = "ID:123 Name:John Age:25";
        Scanner skipScanner = new Scanner(skipData);
        skipScanner.useDelimiter(":");
        
        skipScanner.skip("ID"); // Skip "ID" token
        String id = skipScanner.next(); // Read "123"
        skipScanner.skip("Name"); 
        String name2 = skipScanner.next(); // Read "John"
        
        System.out.println("ID: " + id + ", Name: " + name2);
        skipScanner.close();
        
        // Resetting scanner
        System.out.println("\n=== Multiple Scans on Same Data ===");
        String multiScan = "10 20 30";
        Scanner multi = new Scanner(multiScan);
        
        // First pass
        int sum = 0;
        while (multi.hasNextInt()) {
            sum += multi.nextInt();
        }
        System.out.println("Sum: " + sum);
        multi.close();
    }
}
