/*
 * ============================================================
 * TOPIC     : Exception Handling
 * SUBTOPIC  : Try-Catch Real-World Examples
 * FILE      : TryCatch_RealWorld.cs
 * PURPOSE   : Apply exception handling to real-world scenarios
 *            including file operations, validation, and logging
 * ============================================================
 */

using System;
using System.IO;

namespace CSharp_MasterGuide._05_ExceptionHandling._01_TryCatchFinally
{
    class TryCatch_RealWorld
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Try-Catch Real-World Examples ===\n");

            // ═══════════════════════════════════════════════════════════
            // REAL-WORLD 1: File Operations with Exception Handling
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("--- Real-World 1: File Operations ---\n");

            var fileProcessor = new FileProcessor();
            
            // 1a. Read existing file
            Console.WriteLine("  Reading existing file...");
            string content = fileProcessor.ReadFile("existing.txt");
            Console.WriteLine($"  Content: {content}");
            // Output: Content: This file exists
            
            // 1b. Read non-existent file
            Console.WriteLine("\n  Reading non-existent file...");
            content = fileProcessor.ReadFile("missing.txt");
            Console.WriteLine($"  Content: {content ?? "null (file not found handled)"}");
            // Output: FileNotFoundException: File missing.txt not found
            // Output: Content: (null)
            
            // 1c. Write to file
            Console.WriteLine("\n  Writing to file...");
            fileProcessor.WriteFile("output.txt", "Hello, World!");
            Console.WriteLine("  File written successfully");
            // Output: File written successfully

            // ═══════════════════════════════════════════════════════════
            // REAL-WORLD 2: User Input Validation
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n--- Real-World 2: User Input Validation ---\n");

            var validator = new UserInputValidator();
            
            // 2a. Valid email
            string email = "user@example.com";
            bool isValid = validator.ValidateEmail(email);
            Console.WriteLine($"  Email '{email}' is {(isValid ? "valid" : "invalid")}");
            // Output: Email 'user@example.com' is valid
            
            // 2b. Invalid email
            email = "not-an-email";
            isValid = validator.ValidateEmail(email);
            Console.WriteLine($"  Email '{email}' is {(isValid ? "valid" : "invalid")}");
            // Output: Email 'not-an-email' is invalid
            // Output: FormatException: Invalid email format
            
            // 2c. Valid age
            int age = 25;
            isValid = validator.ValidateAge(age);
            Console.WriteLine($"  Age {age} is {(isValid ? "valid" : "invalid")}");
            // Output: Age 25 is valid
            
            // 2d. Invalid age
            age = -5;
            isValid = validator.ValidateAge(age);
            Console.WriteLine($"  Age {age} is {(isValid ? "valid" : "invalid")}");
            // Output: Age -5 is invalid
            // Output: ArgumentOutOfRangeException: Age must be between 0 and 150

            // ═══════════════════════════════════════════════════════════
            // REAL-WORLD 3: Logging with Exception Handling
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n--- Real-World 3: Logging System ---\n");

            var logger = new Logger();
            
            // 3a. Log with timestamp
            Console.WriteLine("  Logging info message...");
            logger.LogInfo("Application started");
            // Output: INFO: 2024-01-01 12:00:00 | Application started
            
            // 3b. Log error with stack trace
            Console.WriteLine("\n  Logging error message...");
            logger.LogError("Database connection failed", new Exception("Connection timeout"));
            // Output: ERROR: 2024-01-01 12:00:00 | Database connection failed
            // Output: Stack trace: at System.Exception...

            // ═══════════════════════════════════════════════════════════
            // REAL-WORLD 4: Calculator with Error Handling
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n--- Real-World 4: Calculator ---\n");

            var calculator = new Calculator();
            
            // 4a. Division by non-zero
            double result = calculator.Divide(10, 2);
            Console.WriteLine($"  10 / 2 = {result}");
            // Output: 10 / 2 = 5
            
            // 4b. Division by zero
            result = calculator.Divide(10, 0);
            Console.WriteLine($"  10 / 0 = {result}");
            // Output: DivideByZeroException: Cannot divide by zero
            // Output: 10 / 0 = NaN
            
            // 4c. Square root
            result = calculator.SquareRoot(16);
            Console.WriteLine($"  Sqrt(16) = {result}");
            // Output: Sqrt(16) = 4
            
            // 4d. Square root of negative
            result = calculator.SquareRoot(-4);
            Console.WriteLine($"  Sqrt(-4) = {result}");
            // Output: ArgumentException: Cannot calculate square root of negative number
            // Output: Sqrt(-4) = NaN

            // ═══════════════════════════════════════════════════════════
            // REAL-WORLD 5: API Response Handling
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n--- Real-World 5: API Response Handling ---\n");

            var apiClient = new ApiClient();
            
            // 5a. Successful response
            var response = apiClient.GetUser(1);
            Console.WriteLine($"  GET /users/1: {response}");
            // Output: GET /users/1: User: John (age 30)
            
            // 5b. Not found response
            response = apiClient.GetUser(999);
            Console.WriteLine($"  GET /users/999: {response}");
            // Output: GET /users/999: User not found
            
            // 5c. Unauthorized response
            response = apiClient.GetAdminData();
            Console.WriteLine($"  GET /admin: {response}");
            // Output: GET /admin: Access denied - insufficient permissions

            Console.WriteLine("\n=== Try-Catch Real-World Examples Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World 1: File Processor
    // ═══════════════════════════════════════════════════════════

    class FileProcessor
    {
        private string[] _files = { "existing.txt" };
        private string _data = "This file exists";

        public string ReadFile(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    throw new FileNotFoundException($"File {fileName} not found");
                }
                
                return _data;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"  FileNotFoundException: {ex.Message}");
                return null;
            }
        }

        public void WriteFile(string fileName, string content)
        {
            try
            {
                // Simulate writing to file
                Console.WriteLine($"  Writing to {fileName}: {content}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"  IOException: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"  UnauthorizedAccessException: {ex.Message}");
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World 2: User Input Validator
    // ═══════════════════════════════════════════════════════════

    class UserInputValidator
    {
        public bool ValidateEmail(string email)
        {
            try
            {
                ValidateEmailInternal(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public bool ValidateAge(int age)
        {
            try
            {
                ValidateAgeInternal(age);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        private void ValidateEmailInternal(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new FormatException("Email cannot be empty");
            }
            
            if (!email.Contains("@") || !email.Contains("."))
            {
                throw new FormatException("Invalid email format");
            }
        }

        private void ValidateAgeInternal(int age)
        {
            if (age < 0 || age > 150)
            {
                throw new ArgumentOutOfRangeException("age", "Age must be between 0 and 150");
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World 3: Logger
    // ═══════════════════════════════════════════════════════════

    class Logger
    {
        public void LogInfo(string message)
        {
            try
            {
                WriteLog("INFO", message);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"  Failed to write log: {ex.Message}");
            }
        }

        public void LogError(string message, Exception ex)
        {
            try
            {
                WriteLog("ERROR", message);
                
                if (ex != null)
                {
                    Console.WriteLine($"  Stack trace: {ex.StackTrace}");
                }
            }
            catch (IOException)
            {
                Console.WriteLine($"  Failed to write error log");
            }
        }

        private void WriteLog(string level, string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"  {level}: {timestamp} | {message}");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World 4: Calculator
    // ═══════════════════════════════════════════════════════════

    class Calculator
    {
        public double Divide(double a, double b)
        {
            try
            {
                if (b == 0)
                {
                    throw new DivideByZeroException("Cannot divide by zero");
                }
                
                return a / b;
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"  DivideByZeroException: {ex.Message}");
                return double.NaN;
            }
        }

        public double SquareRoot(double value)
        {
            try
            {
                if (value < 0)
                {
                    throw new ArgumentException("Cannot calculate square root of negative number");
                }
                
                return Math.Sqrt(value);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  ArgumentException: {ex.Message}");
                return double.NaN;
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World 5: API Client
    // ═══════════════════════════════════════════════════════════

    class ApiClient
    {
        public string GetUser(int userId)
        {
            try
            {
                if (userId == 1)
                {
                    return "User: John (age 30)";
                }
                
                throw new KeyNotFoundException($"User {userId} not found");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"  NotFoundException: {ex.Message}");
                return "User not found";
            }
        }

        public string GetAdminData()
        {
            try
            {
                throw new UnauthorizedAccessException("Access denied - insufficient permissions");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"  UnauthorizedAccessException: {ex.Message}");
                return "Access denied - insufficient permissions";
            }
        }
    }
}