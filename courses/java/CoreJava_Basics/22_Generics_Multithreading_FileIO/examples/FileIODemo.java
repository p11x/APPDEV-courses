// FileIODemo - Demonstrates Java File I/O Operations
// Important for reading/writing files in backend

import java.io.*;
import java.nio.file.*;
import java.util.Scanner;

public class FileIODemo {
    
    public static void main(String[] args) {
        System.out.println("=== FILE I/O DEMO ===\n");
        
        String filename = "test.txt";
        
        // Write to file (try-with-resources)
        System.out.println("--- Writing to File ---");
        try (FileWriter writer = new FileWriter(filename)) {
            writer.write("Hello from Java File I/O!\n");
            writer.write("Line 2: This is a test.\n");
            writer.write("Line 3: End of file.");
            System.out.println("Written successfully!");
        } catch (IOException e) {
            System.out.println("Error writing: " + e.getMessage());
        }
        
        // Read from file
        System.out.println("\n--- Reading from File ---");
        try (FileReader reader = new FileReader(filename)) {
            int ch;
            while ((ch = reader.read()) != -1) {
                System.out.print((char) ch);
            }
        } catch (IOException e) {
            System.out.println("Error reading: " + e.getMessage());
        }
        
        // Using BufferedReader (more efficient)
        System.out.println("\n--- BufferedReader ---");
        try (BufferedReader br = new BufferedReader(new FileReader(filename))) {
            String line;
            while ((line = br.readLine()) != null) {
                System.out.println("> " + line);
            }
        } catch (IOException e) {
            System.out.println("Error: " + e.getMessage());
        }
        
        // Using Scanner
        System.out.println("\n--- Using Scanner ---");
        try (Scanner scanner = new Scanner(new File(filename))) {
            scanner.useDelimiter("\\n");
            while (scanner.hasNext()) {
                System.out.println("~ " + scanner.next());
            }
        } catch (FileNotFoundException e) {
            System.out.println("File not found: " + e.getMessage());
        }
        
        // Using Files class (Java NIO)
        System.out.println("\n--- Java NIO Files ---");
        try {
            String content = Files.readString(Path.of(filename));
            System.out.println("Content:\n" + content);
            
            // Create new file
            String newFile = "output.txt";
            Files.writeString(Path.of(newFile), "Written via NIO\n");
            System.out.println("NIO write complete!");
            
            // List directory
            System.out.println("\nDirectory contents:");
            Files.list(Paths.get(".")).forEach(p -> System.out.println(p));
        } catch (IOException e) {
            System.out.println("NIO Error: " + e.getMessage());
        }
        
        // File operations
        System.out.println("\n--- File Operations ---");
        File file = new File(filename);
        System.out.println("Exists: " + file.exists());
        System.out.println("Can read: " + file.canRead());
        System.out.println("Can write: " + file.canWrite());
        System.out.println("Size: " + file.length() + " bytes");
    }
}
