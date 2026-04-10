/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Hello World - Real World Applications
 * FILE      : HelloWorld_RealWorld.cs
 * PURPOSE   : This file demonstrates practical, real-world uses of console output in production applications,
 *             including logging, user prompts, formatted output, and debugging techniques.
 * ============================================================
 */

// --- SECTION: Real-World Console Applications ---
// In production environments, console applications are used for:
// - CLI (Command Line Interface) tools
// - Build scripts and automation
// - Background services and daemons
// - DevOps tooling and deployment scripts

using System;
// Using System namespace for Console class and other fundamental types

namespace CSharp_MasterGuide._01_Fundamentals._01_HelloWorld
{
    // This class demonstrates real-world console application patterns
    class HelloWorld_RealWorld
    {
        static void Main(string[] args)
        {
            // ── REAL-WORLD EXAMPLE 1: CLI Application Header ──────────────────────
            // Professional CLI tools display a banner with version info
            // This is common in tools like dotnet CLI, npm, git, etc.
            
            string appName = "FileProcessor"; // Application name for display
            string version = "1.0.0"; // Semantic versioning - major.minor.patch
            string author = "Development Team"; // Author or organization name
            
            // Print a formatted header box commonly seen in CLI tools
            Console.WriteLine("╔══════════════════════════════════════╗"); // Box top border
            Console.WriteLine("║     FileProcessor v1.0.0             ║"); // Version in box
            Console.WriteLine("║     (c) 2024 Development Team        ║"); // Copyright notice
            Console.WriteLine("╚══════════════════════════════════════╝"); // Box bottom border
            // Output:
            // ╔══════════════════════════════════════╗
            // ║     FileProcessor v1.0.0             ║
            // ║     (c) 2024 Development Team        ║
            // ╚══════════════════════════════════════╝

            // ── REAL-WORLD EXAMPLE 2: Progress Reporting ──────────────────────
            // Console applications often show progress bars for long-running operations
            // Common in file processing, data imports, installers, etc.
            
            Console.WriteLine("Starting file processing..."); // Inform user operation is beginning
            
            int totalFiles = 10; // Total number of files to process (simulated)
            for (int i = 1; i <= totalFiles; i++)
            {
                // Calculate percentage complete - progress / total * 100
                int percentComplete = i * 100 / totalFiles; 
                
                // Create progress bar: [#####     ] 50%
                int barLength = 10; // Length of the progress bar in characters
                int filled = i * barLength / totalFiles; // Number of filled characters
                int empty = barLength - filled; // Number of empty characters
                
                // Build progress bar string manually for control
                string progressBar = "["; // Start of progress bar
                for (int j = 0; j < filled; j++) progressBar += "#"; // Filled portion
                for (int j = 0; j < empty; j++) progressBar += " "; // Empty portion
                progressBar += $"] {percentComplete}%"; // Percentage at end
                
                // \r (carriage return) moves cursor to beginning of line without newline
                // This allows us to overwrite the same line
                Console.Write($"\rProcessing: {progressBar}"); // Overwrite previous progress
                System.Threading.Thread.Sleep(200); // Simulate work - 200ms delay
            }
            Console.WriteLine(); // Print newline after progress completes
            Console.WriteLine("Processing complete!"); // Final status message
            // Output: Progress bar fills from 0% to 100%, then "Processing complete!"

            // ── REAL-WORLD EXAMPLE 3: User Prompts and Input ──────────────────────
            // Interactive CLI tools need to prompt users for input
            
            Console.Write("Enter your username: "); // Prompt without newline
            string username = Console.ReadLine(); // ReadLine blocks until user presses Enter
            
            Console.Write("Enter your email: "); // Another prompt
            string email = Console.ReadLine(); // Capture user's email input
            
            // Display captured information back to user
            Console.WriteLine($"\nRegistration Details:"); // $ allows string interpolation
            Console.WriteLine($"  Username: {username}"); // Show username
            Console.WriteLine($"  Email: {email}"); // Show email
            // Output:
            // Enter your username: [user types "john"]
            // Enter your email: [user types "john@example.com"]
            //
            // Registration Details:
            //   Username: john
            //   Email: john@example.com

            // ── REAL-WORLD EXAMPLE 4: Log Levels ──────────────────────
            // Professional applications use different log levels for filtering
            // Common levels: DEBUG, INFO, WARNING, ERROR, CRITICAL
            
            // In a real application, you'd use a logging framework like Serilog or NLog
            // Here we demonstrate the concept with simple console output
            
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // ISO 8601 format
            string logLevel = "INFO"; // Current severity level
            string message = "Application started successfully"; // Log message
            
            // Format: [TIMESTAMP] [LEVEL] Message
            Console.WriteLine($"[{timestamp}] [{logLevel}] {message}");
            // Output: [2024-01-15 10:30:45] [INFO] Application started successfully
            
            // Simulate warning log
            timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Updated timestamp
            logLevel = "WARNING"; // Higher severity
            message = "Configuration file not found, using defaults"; // Warning message
            Console.WriteLine($"[{timestamp}] [{logLevel}] {message}");
            // Output: [2024-01-15 10:30:46] [WARNING] Configuration file not found, using defaults
            
            // Simulate error log
            timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            logLevel = "ERROR";
            message = "Failed to connect to database";
            Console.WriteLine($"[{timestamp}] [{logLevel}] {message}");
            // Output: [2024-01-15 10:30:47] [ERROR] Failed to connect to database

            // ── REAL-WORLD EXAMPLE 5: Table-Style Output ──────────────────────
            // Displaying data in aligned columns is common for reports
            
            // Headers - using PadRight to ensure consistent column width
            Console.WriteLine("ID    Name              Price     Status"); // Column headers
            Console.WriteLine("──────────────────────────────────────────"); // Separator line
            
            // Data rows - each field is padded to align columns
            Console.WriteLine($"{1.ToString().PadRight(5)} Product A".PadRight(23) + "$10.00   Active");
            Console.WriteLine($"{2.ToString().PadRight(5)} Very Long Product Name".PadRight(23) + "$25.50   Inactive");
            Console.WriteLine($"{3.ToString().PadRight(5)} Item C".PadRight(23) + "$5.99    Active");
            // Output:
            // ID    Name              Price     Status
            // ───────────────────────────────────────────
            // 1     Product A         $10.00   Active
            // 2     Very Long Product Name    $25.50   Inactive
            // 3     Item C            $5.99    Active

            // ── REAL-WORLD EXAMPLE 6: Error Output to stderr ──────────────────────
            // Console.Error is separate from Console.Out - useful for errors
            // Allows redirecting errors separately from normal output
            
            // Normal output goes to stdout (Console.Out)
            Console.WriteLine("This is normal output"); // Goes to standard output stream
            
            // Errors should go to stderr (Console.Error) for proper handling
            // In bash: ./program > output.txt 2> errors.txt
            Console.Error.WriteLine("This is an error message"); // Goes to error stream
            // Output (stdout): This is normal output
            // Output (stderr): This is an error message
        }
    }
}
