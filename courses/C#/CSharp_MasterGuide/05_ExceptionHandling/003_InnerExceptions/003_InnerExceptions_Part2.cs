/*
 * TOPIC: Exception Handling in C#
 * SUBTOPIC: Inner Exceptions - Wrapping Exceptions
 * FILE: InnerExceptions_Part2.cs
 * PURPOSE: Demonstrate exception wrapping patterns and re-throwing with inner exceptions
 */

using System;
using System.IO;
using System.Net;
using System.Collections.Generic;

namespace CSharp_MasterGuide._05_ExceptionHandling._03_InnerExceptions
{
    class InnerExceptions_Part2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Inner Exceptions Part 2 - Wrapping Patterns ===\n");

            DemonstrateExceptionWrapping();

            Console.WriteLine("\n=== Custom Exception Wrappers ===\n");

            DemonstrateCustomWrapping();

            Console.WriteLine("\n=== Exception Transformation ===\n");

            DemonstrateExceptionTransformation();
        }

        static void DemonstrateExceptionWrapping()
        {
            Console.WriteLine("1. Database Exception Wrapping:");
            Console.WriteLine("-----------------------");

            try
            {
                Repository repository = new Repository();
                repository.FindUser(999);
            }
            catch (RepositoryException ex)
            {
                Console.WriteLine($"Custom Exception: {ex.Message}");
                Console.WriteLine($"Original Exception: {ex.InnerException?.GetType().Name}");
                Console.WriteLine($"Error Code: {ex.ErrorCode}");
            }

            // Output: Custom Exception: Failed to retrieve user record
            // Output: Original Exception: InvalidOperationException
            // Output: Error Code: 1001
        }

        static void DemonstrateCustomWrapping()
        {
            Console.WriteLine("2. Service Layer Wrapping:");
            Console.WriteLine("----------------------");

            try
            {
                UserService service = new UserService();
                service.GetUserProfile(0);
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Service Error: {ex.Message}");
                Console.WriteLine($"Server Error Code: {ex.ErrorCode}");
                Console.WriteLine($"Is Recoverable: {ex.IsRecoverable}");
            }

            // Output: Service Error: User service operation failed
            // Output: Server Error Code: USR_001
            // Output: Is Recoverable: False
        }

        static void DemonstrateExceptionTransformation()
        {
            Console.WriteLine("3. Exception Transformation Pattern:");
            Console.WriteLine("-------------------------------------");

            try
            {
                DataProcessor processor = new DataProcessor();
                processor.ProcessData("nonexistent.csv");
            }
            catch (ProcessingException ex)
            {
                Console.WriteLine($"Processed Error: {ex.Message}");
                Console.WriteLine($"Processing Stage: {ex.Stage}");
                Console.WriteLine($"Stack Trace:\n{ex.StackTrace}");
            }

            // Output: Processed Error: Error during data processing
            // Output: Processing Stage: FileRead
        }
    }

    class RepositoryException : Exception
    {
        public int ErrorCode { get; }

        public RepositoryException(string message, Exception inner, int errorCode)
            : base(message, inner)
        {
            ErrorCode = errorCode;
        }
    }

    class Repository
    {
        public object FindUser(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new InvalidOperationException("User ID must be positive");
                }

                return new object();
            }
            catch (InvalidOperationException ex)
            {
                throw new RepositoryException("Failed to retrieve user record", ex, 1001);
            }
        }
    }

    class ServiceException : Exception
    {
        public string ErrorCode { get; }
        public bool IsRecoverable { get; }

        public ServiceException(string message, Exception inner, string errorCode, bool isRecoverable)
            : base(message, inner)
        {
            ErrorCode = errorCode;
            IsRecoverable = isRecoverable;
        }
    }

    class UserService
    {
        public object GetUserProfile(int userId)
        {
            try
            {
                if (userId == 0)
                {
                    throw new ArgumentException("Invalid user ID", "userId");
                }

                return new object();
            }
            catch (ArgumentException ex)
            {
                throw new ServiceException("User service operation failed", ex, "USR_001", false);
            }
        }
    }

    class ProcessingException : Exception
    {
        public string Stage { get; }

        public ProcessingException(string message, Exception inner, string stage)
            :	base(message, inner)
        {
            Stage = stage;
        }
    }

    class DataProcessor
    {
        public void ProcessData(string filePath)
        {
            try
            {
                string content = File.ReadAllText(filePath);
            }
            catch (FileNotFoundException ex)
            {
                throw new ProcessingException("Error during data processing", ex, "FileRead");
            }
        }
    }
}