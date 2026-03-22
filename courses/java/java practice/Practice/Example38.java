/*
 * SUB TOPIC: File I/O Operations in Java
 * 
 * DEFINITION:
 * File I/O (Input/Output) in Java involves reading data from files and writing data to files. 
 * Java provides various classes in the java.io and java.nio packages for file operations, 
 * including FileReader, FileWriter, BufferedReader, BufferedWriter, and file streams.
 * 
 * FUNCTIONALITIES:
 * 1. File class - Create, delete, check file existence
 * 2. FileReader/FileWriter - Character-based file I/O
 * 3. BufferedReader/BufferedWriter - Buffered character streams
 * 4. FileInputStream/FileOutputStream - Byte-based I/O
 * 5. Creating and reading files
 * 6. Appending to files
 */

import java.io.*; // Import all I/O classes
import java.nio.file.*; // Import NIO classes

public class Example38 {
    public static void main(String[] args) {
        
        String fileName = "sample.txt";
        
        // Topic Explanation: Writing to File using FileWriter
        
        // Write to file using FileWriter
        System.out.println("=== Writing to File ===");
        try (FileWriter writer = new FileWriter(fileName)) {
            writer.write("Hello, this is line 1\n");
            writer.write("This is line 2\n");
            writer.write("This is line 3\n");
            System.out.println("Data written to file successfully!");
        } catch (IOException e) {
            System.out.println("Error writing: " + e.getMessage());
        }
        
        // Read from file using FileReader
        System.out.println("\n=== Reading from File (FileReader) ===");
        try (FileReader reader = new FileReader(fileName)) {
            int ch;
            while ((ch = reader.read()) != -1) {
                System.out.print((char) ch);
            }
        } catch (IOException e) {
            System.out.println("Error reading: " + e.getMessage());
        }
        
        // Using BufferedReader for efficient reading
        System.out.println("\n=== Reading with BufferedReader ===");
        try (BufferedReader br = new BufferedReader(new FileReader(fileName))) {
            String line;
            while ((line = br.readLine()) != null) {
                System.out.println("Line: " + line);
            }
        } catch (IOException e) {
            System.out.println("Error: " + e.getMessage());
        }
        
        // Using BufferedWriter for efficient writing
        System.out.println("\n=== Writing with BufferedWriter ===");
        String bufferFile = "buffered.txt";
        try (BufferedWriter bw = new BufferedWriter(new FileWriter(bufferFile))) {
            bw.write("Buffered writing example\n");
            bw.write("Line 2 with buffer\n");
            bw.write("Line 3 with buffer\n");
            System.out.println("Buffered writing completed!");
        } catch (IOException e) {
            System.out.println("Error: " + e.getMessage());
        }
        
        // File class operations
        System.out.println("\n=== File Class Operations ===");
        File file = new File(fileName);
        System.out.println("File exists: " + file.exists());
        System.out.println("File name: " + file.getName());
        System.out.println("File path: " + file.getPath());
        System.out.println("File size: " + file.length() + " bytes");
        System.out.println("Is readable: " + file.canRead());
        System.out.println("Is writable: " + file.canWrite());
        
        // Real-time Example 1: User log file
        System.out.println("\n=== Example 1: User Activity Log ===");
        String logFile = "activity.log";
        
        try (BufferedWriter logWriter = new BufferedWriter(new FileWriter(logFile))) {
            logWriter.write("User login at: 2024-01-15 10:00:00\n");
            logWriter.write("User viewed profile at: 2024-01-15 10:05:00\n");
            logWriter.write("User updated settings at: 2024-01-15 10:10:00\n");
            logWriter.write("User logout at: 2024-01-15 10:30:00\n");
            System.out.println("Activity log saved!");
        } catch (IOException e) {
            System.out.println("Error: " + e.getMessage());
        }
        
        // Read and display log
        try (BufferedReader logReader = new BufferedReader(new FileReader(logFile))) {
            String logLine;
            System.out.println("Log contents:");
            while ((logLine = logReader.readLine()) != null) {
                System.out.println("  " + logLine);
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
        
        // Real-time Example 2: Contact Manager - Save contacts
        System.out.println("\n=== Example 2: Contact Manager ===");
        String contactsFile = "contacts.txt";
        
        try (BufferedWriter contactWriter = new BufferedWriter(new FileWriter(contactsFile))) {
            contactWriter.write("John,9876543210,john@email.com\n");
            contactWriter.write("Jane,9876543211,jane@email.com\n");
            contactWriter.write("Mike,9876543212,mike@email.com\n");
            System.out.println("Contacts saved to file!");
        } catch (IOException e) {
            e.printStackTrace();
        }
        
        // Read contacts
        try (BufferedReader contactReader = new BufferedReader(new FileReader(contactsFile))) {
            String contact;
            System.out.println("Saved contacts:");
            while ((contact = contactReader.readLine()) != null) {
                String[] parts = contact.split(",");
                System.out.println("  Name: " + parts[0] + ", Phone: " + parts[1]);
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
        
        // Real-time Example 3: Append to file (shopping cart)
        System.out.println("\n=== Example 3: Shopping Cart Log ===");
        String cartFile = "cart_history.txt";
        
        try (BufferedWriter cartWriter = new BufferedWriter(new FileWriter(cartFile, true))) {
            cartWriter.write("Added: Laptop - $999\n");
            cartWriter.write("Added: Mouse - $29\n");
            cartWriter.write("Added: Keyboard - $79\n");
            System.out.println("Cart items appended!");
        } catch (IOException e) {
            e.printStackTrace();
        }
        
        // Real-time Example 4: File copy operation
        System.out.println("\n=== Example 4: Copy File ===");
        String sourceFile = fileName;
        String destFile = "copy_of_sample.txt";
        
        try (BufferedReader source = new BufferedReader(new FileReader(sourceFile));
             BufferedWriter dest = new BufferedWriter(new FileWriter(destFile))) {
            
            String line;
            while ((line = source.readLine()) != null) {
                dest.write(line);
                dest.newLine();
            }
            System.out.println("File copied successfully!");
        } catch (IOException e) {
            System.out.println("Error copying: " + e.getMessage());
        }
        
        // Real-time Example 5: Count lines in file
        System.out.println("\n=== Example 5: Count File Lines ===");
        try (BufferedReader reader = new BufferedReader(new FileReader(fileName))) {
            int lineCount = 0;
            while (reader.readLine() != null) {
                lineCount++;
            }
            System.out.println("Number of lines: " + lineCount);
        } catch (IOException e) {
            e.printStackTrace();
        }
        
        // Real-time Example 6: Search in file
        System.out.println("\n=== Example 6: Search Text in File ===");
        String searchTerm = "line";
        
        try (BufferedReader reader = new BufferedReader(new FileReader(fileName))) {
            String line;
            int lineNum = 0;
            System.out.println("Searching for '" + searchTerm + "':");
            
            while ((line = reader.readLine()) != null) {
                lineNum++;
                if (line.toLowerCase().contains(searchTerm.toLowerCase())) {
                    System.out.println("  Line " + lineNum + ": " + line);
                }
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
        
        // Bonus: Using Path and Files (Java NIO)
        System.out.println("\n=== Bonus: Java NIO ===");
        Path path = Paths.get("nio_test.txt");
        
        try {
            // Write using Files class
            Files.writeString(path, "Written using Java NIO\nNew line here");
            
            // Read using Files class
            String content = Files.readString(path);
            System.out.println("NIO Read: " + content);
            
            // Delete file
            Files.deleteIfExists(path);
            System.out.println("NIO file deleted");
        } catch (IOException e) {
            e.printStackTrace();
        }
        
        System.out.println("\n=== File I/O Examples Complete ===");
    }
}
