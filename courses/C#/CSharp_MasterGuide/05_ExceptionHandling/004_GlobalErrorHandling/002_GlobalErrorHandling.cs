/*
 * TOPIC: Global Error Handling in C#
 * SUBTOPIC: AppDomain.CurrentDomain.UnhandledException
 * FILE: GlobalErrorHandling.cs
 * PURPOSE: Demonstrate how to handle uncaught exceptions at the AppDomain level using UnhandledException event
 */

using System;
using System.IO;

namespace CSharp_MasterGuide._05_ExceptionHandling._04_GlobalErrorHandling
{
    /// <summary>
    /// Demonstrates the UnhandledException event handler for catching exceptions that escape Try-Catch blocks
    /// </summary>
    public class GlobalErrorHandling
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Global Error Handling Demo ===\n");

            // Register the UnhandledException event handler
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            // Example 1: Basic unhandled exception
            Console.WriteLine("Example 1: Throwing an unhandled exception");
            try
            {
                ThrowBasicException();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Caught locally: {ex.Message}");
            }

            // This exception will escape to the UnhandledException handler
            Console.WriteLine("\nExample 2: Exception that escapes to UnhandledException handler");
            ThrowUnhandledException();

            // Example 3: Using UnhandledExceptionEventArgs
            Console.WriteLine("\nExample 3: Checking IsTerminating property");
            ThrowExceptionWithIsTerminatingCheck();
        }

        static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = e.ExceptionObject as Exception;
            Console.WriteLine($"[UnhandledException Handler] Caught: {exception?.Message}");
            Console.WriteLine($"[UnhandledException Handler] IsTerminating: {e.IsTerminating}");
            
            // Log the exception details
            LogException(exception);
        }

        static void ThrowBasicException()
        {
            throw new InvalidOperationException("This is a basic exception");
        }

        static void ThrowUnhandledException()
        {
            // This exception is not caught and will trigger UnhandledException
            throw new DivideByZeroException("Unhandled division by zero");
        }

        static void ThrowExceptionWithIsTerminatingCheck()
        {
            // Simulate a non-terminating exception
            if (AppDomain.CurrentDomain.IsFinalizingForUncatchableException)
            {
                Console.WriteLine("AppDomain is finalizing for uncatchable exception");
            }
        }

        static void LogException(Exception ex)
        {
            if (ex == null) return;
            
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log");
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}\n";
            
            try
            {
                File.AppendAllText(logPath, logMessage);
                Console.WriteLine($"[UnhandledException Handler] Exception logged to {logPath}");
            }
            catch
            {
                Console.WriteLine("Failed to log exception");
            }
        }
    }

    /// <summary>
    /// Real-world example: Global exception handler with cleanup logic
    /// </summary>
    public class GlobalErrorHandlingRealWorld
    {
        private static bool _isCleanupPerformed = false;

        public static void Main(string[] args)
        {
            Console.WriteLine("=== Real-World Global Error Handling ===\n");

            // Register global exception handler with cleanup logic
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledExceptionWithCleanup;

            // Simulate application startup
            InitializeApplication();

            // Trigger an unhandled exception
            Console.WriteLine("\nSimulating unhandled exception in business logic...");
            ProcessBusinessData(null);
        }

        static void OnUnhandledExceptionWithCleanup(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            
            Console.WriteLine("\n=== CRITICAL: Unhandled Exception ===");
            Console.WriteLine($"Type: {ex?.GetType().Name}");
            Console.WriteLine($"Message: {ex?.Message}");
            Console.WriteLine($"IsTerminating: {e.IsTerminating}");

            // Perform cleanup operations
            if (!_isCleanupPerformed)
            {
                PerformCleanup();
                _isCleanupPerformed = true;
            }

            // Attempt graceful shutdown
            if (e.IsTerminating)
            {
                Console.WriteLine("\nApplication is terminating. Saving state...");
                SaveApplicationState();
                Console.WriteLine("Cleanup complete. Exiting...");
                Environment.Exit(1);
            }
        }

        static void InitializeApplication()
        {
            Console.WriteLine("Initializing application resources...");
            // Initialize databases, connections, etc.
        }

        static void ProcessBusinessData(object data)
        {
            // Simulate business logic that throws
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Business data cannot be null");
            }
        }

        static void PerformCleanup()
        {
            Console.WriteLine("Performing cleanup: closing connections, releasing resources...");
            // Close file handles, database connections, etc.
        }

        static void SaveApplicationState()
        {
            Console.WriteLine("Saving application state before termination...");
            // Save pending data, user preferences, etc.
        }
    }
}
