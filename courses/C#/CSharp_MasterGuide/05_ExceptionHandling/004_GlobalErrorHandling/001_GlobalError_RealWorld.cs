/*
 * ============================================================
 * TOPIC     : Exception Handling
 * SUBTOPIC  : Global Error Handling
 * FILE      : GlobalError_RealWorld.cs
 * PURPOSE   : Demonstrate centralized error handling, logging infrastructure, and graceful degradation patterns
 * ============================================================
 */
using System; // needed for Console, Guid, Exception types
using System.Collections.Concurrent; // needed for ConcurrentQueue (thread-safe)
using System.Collections.Generic; // needed for generic collections
using System.IO; // needed for File, Directory operations
using System.Linq; // needed for LINQ extensions
using System.Threading; // needed for Thread exception handling
using System.Threading.Tasks; // needed for TaskScheduler events

namespace CSharp_MasterGuide._05_ExceptionHandling._04_GlobalErrorHandling
{
    /// <summary>
    /// Centralized error handling service for production applications
    /// Registers global exception handlers and provides centralized logging
    /// </summary>
    public class ErrorHandlingService
    {
        // ConcurrentQueue<ErrorLog> = thread-safe queue for storing error logs
        // static readonly = initialized once, cannot be reassigned
        private static readonly ConcurrentQueue<ErrorLog> _errorQueue = new ConcurrentQueue<ErrorLog>();
        
        // bool _isInitialized = tracks whether service has been initialized
        private static bool _isInitialized = false;
        
        // object _lock = synchronization object for thread-safe initialization
        private static readonly object _lock = new object();

        /// <summary>
        /// Initializes the error handling service and registers global handlers
        /// Uses double-checked locking pattern for thread-safe singleton initialization
        /// </summary>
        public static void Initialize()
        {
            // Check if already initialized (first check, no locking)
            if (_isInitialized) return;

            // lock statement = ensures thread-safe initialization
            lock (_lock)
            {
                // Double-check inside lock to prevent race condition
                if (_isInitialized) return;

                // Register handler for unhandled exceptions in AppDomain
                // AppDomain.CurrentDomain.UnhandledException = fires when unhandled exception occurs
                AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
                
                // Register handler for unobserved task exceptions
                // TaskScheduler.UnobservedTaskException = fires when faulted task exception goes unobserved
                TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

                // Mark as initialized
                _isInitialized = true;
                
                // Console.WriteLine = output initialization message
                // Output: [ErrorHandlingService] Initialized
                Console.WriteLine("[ErrorHandlingService] Initialized");
            }
        }

        /// <summary>
        /// Handles unhandled exceptions at the AppDomain level
        /// Called when an exception escapes all try-catch blocks
        /// </summary>
        /// <param name="sender">Source of the event (AppDomain)</param>
        /// <param name="e">Event arguments containing exception and termination flag</param>
        static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Exception ex = extract exception from event arguments object
            // as Exception = safe cast returns null if not Exception type
            Exception ex = e.ExceptionObject as Exception;
            
            // Create error log with appropriate severity
            // e.IsTerminating = true if runtime is terminating due to this exception
            // ErrorSeverity.Critical = highest severity for terminating exceptions
            ErrorLog log = CreateErrorLog("UnhandledException", ex, e.IsTerminating ? ErrorSeverity.Critical : ErrorSeverity.High);
            
            // _errorQueue.Enqueue = add error log to thread-safe queue
            _errorQueue.Enqueue(log);
            
            // Output: [UnhandledException] [message]
            Console.WriteLine($"[UnhandledException] {ex?.Message}");
            // Output: [UnhandledException] IsTerminating: [true/false]
            Console.WriteLine($"[UnhandledException] IsTerminating: {e.IsTerminating}");

            // If terminating, perform emergency shutdown
            if (e.IsTerminating)
            {
                // Call method to perform emergency shutdown procedures
                PerformEmergencyShutdown();
            }
        }

        /// <summary>
        /// Handles unobserved task exceptions from the Task Parallel Library
        /// These exceptions would otherwise cause process termination
        /// </summary>
        /// <param name="sender">Source of the event (TaskScheduler)</param>
        /// <param name="e">Event arguments containing the unobserved exception</param>
        static void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            // Create error log with Medium severity (recoverable)
            // e.Exception = the AggregateException that was unobserved
            ErrorLog log = CreateErrorLog("UnobservedTaskException", e.Exception, ErrorSeverity.Medium);
            
            // Enqueue to error log queue
            _errorQueue.Enqueue(log);
            
            // Output: [UnobservedTaskException] [message]
            Console.WriteLine($"[UnobservedTaskException] {e.Exception.Message}");
            
            // e.SetObserved() = marks exception as observed, prevents process termination
            // CRITICAL: Must call this to prevent application crash
            e.SetObserved();
        }

        /// <summary>
        /// Handles exceptions from Windows Forms thread exceptions
        /// Note: This is for Windows-specific scenarios
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">Event arguments containing the thread exception</param>
        static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            // Create error log with High severity
            ErrorLog log = CreateErrorLog("ThreadException", e.Exception, ErrorSeverity.High);
            _errorQueue.Enqueue(log);
            
            // Output: [ThreadException] [message]
            Console.WriteLine($"[ThreadException] {e.Exception.Message}");
        }

        /// <summary>
        /// Creates an ErrorLog entry from exception data
        /// </summary>
        /// <param name="source">Source component where error occurred</param>
        /// <param name="ex">Exception that occurred (can be null)</param>
        /// <param name="severity">Severity level of the error</param>
        /// <returns>Populated ErrorLog object</returns>
        static ErrorLog CreateErrorLog(string source, Exception ex, ErrorSeverity severity)
        {
            // return new ErrorLog with populated properties
            return new ErrorLog
            {
                // Guid.NewGuid() = generates unique identifier for this log entry
                Id = Guid.NewGuid(),
                // DateTime.UtcNow = current UTC timestamp
                Timestamp = DateTime.UtcNow,
                // string source = component where error originated
                Source = source,
                // ex?.Message = null-conditional access, returns null if ex is null
                // ?? "Unknown error" = null-coalescing, uses default if null
                Message = ex?.Message ?? "Unknown error",
                // ex?.StackTrace = full stack trace for debugging
                StackTrace = ex?.StackTrace ?? string.Empty,
                // ErrorSeverity severity = severity level
                Severity = severity,
                // Environment.MachineName = name of machine for distributed debugging
                MachineName = Environment.MachineName
            };
        }

        /// <summary>
        /// Performs emergency shutdown procedures when critical error occurs
        /// Flushes logs and attempts graceful termination
        /// </summary>
        static void PerformEmergencyShutdown()
        {
            // Output: [ErrorHandlingService] Performing emergency shutdown...
            Console.WriteLine("[ErrorHandlingService] Performing emergency shutdown...");
            
            // Call method to flush any pending error logs
            FlushErrorLogs();
            
            // Output: [ErrorHandlingService] Shutdown complete
            Console.WriteLine("[ErrorHandlingService] Shutdown complete");
        }

        /// <summary>
        /// Flushes all queued error logs to persistent storage (console for demo)
        /// </summary>
        public static void FlushErrorLogs()
        {
            // _errorQueue.Count = number of items in queue
            // Output: [ErrorHandlingService] Flushing [n] error logs:
            Console.WriteLine($"\n[ErrorHandlingService] Flushing {_errorQueue.Count} error logs:");
            
            // while loop with TryDequeue = thread-safe way to drain queue
            // _errorQueue.TryDequeue(out ErrorLog log) = tries to remove and return front item
            while (_errorQueue.TryDequeue(out ErrorLog log))
            {
                // DateTime.ToString("HH:mm:ss") = format timestamp to time only
                // Output: [timestamp] [severity] [source]: [message]
                Console.WriteLine($"  [{log.Timestamp:HH:mm:ss}] [{log.Severity}] {log.Source}: {log.Message}");
            }
        }

        /// <summary>
        /// Logs application-level errors that aren't from exception handling
        /// </summary>
        /// <param name="message">Human-readable error message</param>
        /// <param name="ex">Optional exception that caused the error</param>
        public static void LogError(string message, Exception ex = null)
        {
            // Create error log with Low severity (informational)
            // Pass null for exception since we're using message
            ErrorLog log = CreateErrorLog("Application", ex, ErrorSeverity.Low);
            
            // Override message with provided message parameter
            log.Message = message;
            
            // Enqueue for later processing
            _errorQueue.Enqueue(log);
            
            // Output: [ApplicationError] [message]
            Console.WriteLine($"[ApplicationError] {message}");
        }
    }

    /// <summary>
    /// Data model for error log entries
    /// Used for centralized error tracking and analysis
    /// </summary>
    public class ErrorLog
    {
        // public properties with auto-getter/setter for serialization
        public Guid Id { get; set; } // Unique identifier for this log entry
        public DateTime Timestamp { get; set; } // When the error occurred (UTC)
        public string Source { get; set; } // Component or method where error occurred
        public string Message { get; set; } // Human-readable error description
        public string StackTrace { get; set; } // Full stack trace for debugging
        public ErrorSeverity Severity { get; set; } // Severity level (Low/Medium/High/Critical)
        public string MachineName { get; set; } // Machine where error occurred
    }

    /// <summary>
    /// Severity levels for categorizing errors
    /// Used for filtering and alerting decisions
    /// </summary>
    public enum ErrorSeverity
    {
        Low,      // Informational, no immediate action needed
        Medium,   // Recoverable error, may need attention
        High,     // Significant error requiring investigation
        Critical  // Application-terminating error
    }

    /// <summary>
    /// Simulated Application domain wrapper for demonstration
    /// In real apps, you'd use actual AppDomain or custom infrastructure
    /// </summary>
    public static class ApplicationDomain
    {
        // Static property for singleton access
        public static ApplicationDomainWrapper CurrentApplication { get; } = new ApplicationDomainWrapper();
    }

    /// <summary>
    /// Wrapper class to simulate Application domain functionality
    /// </summary>
    public class ApplicationDomainWrapper
    {
        // Event for thread exception handling (Windows Forms scenario)
        public event EventHandler<ThreadExceptionEventArgs> ThreadException;

        /// <summary>
        /// Raises the ThreadException event with given exception
        /// </summary>
        /// <param name="ex">Exception to wrap in event args</param>
        public void RaiseThreadException(Exception ex)
        {
            // ThreadExceptionEventArgs constructor takes exception
            // EventHandler.Invoke raises the event with this and args
            ThreadException?.Invoke(this, new ThreadExceptionEventArgs(ex));
        }
    }

    // ── REAL-WORLD EXAMPLE: Production Application with Graceful Degradation ─
    /// <summary>
    /// Real-world: Production application demonstrating error handling and graceful degradation
    /// Shows how to handle multiple service failures while maintaining partial functionality
    /// </summary>
    public class ProductionApplication
    {
        // bool _isHealthy = tracks overall application health status
        private static bool _isHealthy = true;
        
        // int _consecutiveErrors = counts consecutive errors for degradation decision
        private static int _consecutiveErrors = 0;
        
        // const int = compile-time constant, cannot be modified
        // MaxConsecutiveErrors = threshold for triggering degradation
        private const int MaxConsecutiveErrors = 3;

        /// <summary>
        /// Main entry point for production application demonstration
        /// </summary>
        /// <param name="args">Command line arguments (not used)</param>
        public static void Main(string[] args)
        {
            // Output: === Real-World Production Application ===
            Console.WriteLine("=== Real-World Production Application ===\n");

            // Call Initialize to register global exception handlers
            ErrorHandlingService.Initialize();

            // Simulate application lifecycle
            RunApplication();
        }

        /// <summary>
        /// Runs the main application logic with simulated operations
        /// </summary>
        static void RunApplication()
        {
            // Output: Starting production application...
            Console.WriteLine("Starting production application...\n");

            // Simulate various service operations that might fail
            SimulateDatabaseOperation();
            SimulateFileOperation();
            SimulateNetworkOperation();
            SimulateBusinessLogic();

            // Trigger degradation mode after errors
            TriggerDegradation();

            // Output: Application health: [true/false]
            Console.WriteLine($"\nApplication health: {_isHealthy}");
            
            // Final flush of error logs
            ErrorHandlingService.FlushErrorLogs();
        }

        /// <summary>
        /// Simulates a database operation that might fail
        /// </summary>
        static void SimulateDatabaseOperation()
        {
            try
            {
                // Output: [Database] Executing query...
                Console.WriteLine("[Database] Executing query...");
                
                // Simulate database error - throw exception
                throw new InvalidOperationException("Database connection timeout");
            }
            catch (Exception ex)
            {
                // Catch and handle using common handler
                HandleServiceError("Database", ex);
            }
        }

        /// <summary>
        /// Simulates a file system operation that might fail
        /// </summary>
        static void SimulateFileOperation()
        {
            try
            {
                // Output: [FileSystem] Reading configuration...
                Console.WriteLine("[FileSystem] Reading configuration...");
                
                // Simulate file not found error
                throw new FileNotFoundException("config.json not found");
            }
            catch (Exception ex)
            {
                HandleServiceError("FileSystem", ex);
            }
        }

        /// <summary>
        /// Simulates a network operation that might fail
        /// </summary>
        static void SimulateNetworkOperation()
        {
            try
            {
                // Output: [Network] Calling external API...
                Console.WriteLine("[Network] Calling external API...");
                
                // Simulate network timeout
                throw new TimeoutException("API request timeout");
            }
            catch (Exception ex)
            {
                HandleServiceError("Network", ex);
            }
        }

        /// <summary>
        /// Simulates business logic that might fail
        /// </summary>
        static void SimulateBusinessLogic()
        {
            try
            {
                // Output: [BusinessLogic] Processing transaction...
                Console.WriteLine("[BusinessLogic] Processing transaction...");
                
                // Simulate invalid data error
                throw new ArgumentException("Invalid transaction data");
            }
            catch (Exception ex)
            {
                HandleServiceError("BusinessLogic", ex);
            }
        }

        /// <summary>
        /// Handles errors from various services with degradation logic
        /// </summary>
        /// <param name="service">Name of service that errored</param>
        /// <param name="ex">Exception that occurred</param>
        static void HandleServiceError(string service, Exception ex)
        {
            // Increment consecutive error counter
            _consecutiveErrors++;
            
            // Output: Error in [service]: [message]
            Console.WriteLine($"  Error in {service}: {ex.Message}");
            
            // Log error to centralized error handling service
            ErrorHandlingService.LogError($"Error in {service}: {ex.Message}", ex);

            // Check if consecutive errors exceed threshold
            if (_consecutiveErrors >= MaxConsecutiveErrors)
            {
                // Output: [HealthCheck] [n] consecutive errors detected
                Console.WriteLine($"\n[HealthCheck] {_consecutiveErrors} consecutive errors detected");
                
                // Trigger graceful degradation
                TriggerDegradation();
            }
        }

        /// <summary>
        /// Triggers graceful degradation mode when too many errors occur
        /// Reduces functionality but keeps application running
        /// </summary>
        static void TriggerDegradation()
        {
            // Output: === Entering Graceful Degradation Mode ===
            Console.WriteLine("\n=== Entering Graceful Degradation Mode ===");
            
            // Mark application as unhealthy
            _isHealthy = false;
            
            // Output: - Disabling non-essential features
            Console.WriteLine("- Disabling non-essential features");
            // Output: - Switching to cached data mode
            Console.WriteLine("- Switching to cached data mode");
            // Output: - Enabling offline mode
            Console.WriteLine("- Enabling offline mode");
            // Output: - Reducing logging frequency
            Console.WriteLine("- Reducing logging frequency");
            
            // Reset error counter after degradation to allow recovery detection
            _consecutiveErrors = 0;
        }
    }

    /// <summary>
    /// Real-world: Logging infrastructure with file rotation capability
    /// Demonstrates production-grade logging patterns
    /// </summary>
    public class LoggingInfrastructure
    {
        // Path.Combine = combines paths in platform-independent way
        // AppDomain.CurrentDomain.BaseDirectory = application executable directory
        // "logs" = subdirectory name for log files
        private static readonly string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        
        // object _logLock = lock object for thread-safe file access
        private static readonly object _logLock = new object();

        /// <summary>
        /// Initializes logging infrastructure by creating log directory if needed
        /// </summary>
        public static void Initialize()
        {
            // Directory.Exists = checks if directory exists
            if (!Directory.Exists(LogDirectory))
            {
                // Directory.CreateDirectory = creates directory and all parent directories
                Directory.CreateDirectory(LogDirectory);
            }
        }

        /// <summary>
        /// Logs a message with timestamp and level to file
        /// Thread-safe using lock statement
        /// </summary>
        /// <param name="level">Log level (INFO, WARN, ERROR, etc.)</param>
        /// <param name="message">Message to log</param>
        public static void Log(string level, string message)
        {
            // DateTime.Now.ToString = format current date/time
            // "yyyyMMdd" = year month day format for daily log files
            string fileName = $"app_{DateTime.Now:yyyyMMdd}.log";
            
            // Path.Combine = combine directory and filename
            string filePath = Path.Combine(LogDirectory, fileName);
            
            // string interpolation for log entry format
            // "[timestamp] [level] [message]"
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] {message}";

            // lock statement = ensures only one thread writes at a time
            lock (_logLock)
            {
                try
                {
                    // File.AppendAllText = appends text to file, creates if doesn't exist
                    File.AppendAllText(filePath, logEntry + Environment.NewLine);
                }
                catch
                {
                    // Fallback to console if file logging fails
                    // This ensures we never lose log messages
                    Console.WriteLine(logEntry);
                }
            }
        }

        /// <summary>
        /// Logs an exception with optional context information
        /// </summary>
        /// <param name="ex">Exception to log</param>
        /// <param name="context">Optional context (method name, etc.)</param>
        public static void LogException(Exception ex, string context = "")
        {
            // string.IsNullOrEmpty = checks if string is null or empty
            // Ternary operator formats message based on context presence
            string message = string.IsNullOrEmpty(context)
                ? $"{ex.GetType().Name}: {ex.Message}"
                : $"{context} - {ex.GetType().Name}: {ex.Message}";
            
            // Log at ERROR level
            Log("ERROR", message);
            
            // Check for inner exception and log it too
            if (ex.InnerException != null)
            {
                // Output: Inner: [inner message]
                Log("ERROR", $"  Inner: {ex.InnerException.Message}");
            }
        }
    }

    /// <summary>
    /// Real-world: Complete error handling pipeline demonstrating production patterns
    /// Integrates logging infrastructure with global exception handlers
    /// </summary>
    public class CompleteErrorHandlingPipeline
    {
        /// <summary>
        /// Main entry point demonstrating complete error handling pipeline
        /// </summary>
        /// <param name="args">Command line arguments (not used)</param>
        public static void Main(string[] args)
        {
            // Output: === Complete Error Handling Pipeline ===
            Console.WriteLine("=== Complete Error Handling Pipeline ===\n");

            // Initialize logging infrastructure
            LoggingInfrastructure.Initialize();

            // Register global handlers that log exceptions
            AppDomain.CurrentDomain.UnhandledException += LogAndHandleException;
            TaskScheduler.UnobservedTaskException += LogAndHandleTaskException;

            // Run business operations
            RunBusinessOperations();
        }

        /// <summary>
        /// Logs and handles unhandled exceptions from AppDomain
        /// </summary>
        static void LogAndHandleException(object sender, UnhandledExceptionEventArgs e)
        {
            // Exception ex = safe cast exception object to Exception
            Exception ex = e.ExceptionObject as Exception;
            
            // Log exception using logging infrastructure
            LoggingInfrastructure.LogException(ex, "UnhandledException");
            
            // Output: [FATAL] Unhandled exception occurred
            Console.WriteLine("\n[FATAL] Unhandled exception occurred");
            // Output: IsTerminating: [true/false]
            Console.WriteLine($"  IsTerminating: {e.IsTerminating}");
            
            // Perform cleanup operations
            PerformCleanup();
            
            // If terminating, exit with error code
            if (e.IsTerminating)
            {
                // Output: Application terminating...
                Console.WriteLine("  Application terminating...");
                
                // Environment.Exit = terminates process with exit code
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Logs and handles unobserved task exceptions
        /// </summary>
        static void LogAndHandleTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            // Log exception
            LoggingInfrastructure.LogException(e.Exception, "UnobservedTaskException");
            
            // Output: [WARN] Unobserved task exception
            Console.WriteLine("\n[WARN] Unobserved task exception");
            // Output: [message]
            Console.WriteLine($"  {e.Exception.Message}");
            
            // CRITICAL: Mark as observed to prevent process termination
            e.SetObserved();
        }

        /// <summary>
        /// Simulates running business operations with error handling
        /// </summary>
        static void RunBusinessOperations()
        {
            // Output: Running business operations...
            Console.WriteLine("Running business operations...");

            // Process multiple orders
            // Each may succeed or fail independently
            ProcessOrder(1);
            ProcessOrder(2);
            ProcessOrder(3);
        }

        /// <summary>
        /// Processes a single order with exception handling
        /// Continues processing other orders even if one fails
        /// </summary>
        /// <param name="orderId">ID of order to process</param>
        static void ProcessOrder(int orderId)
        {
            try
            {
                // Output: Processing order #[id]...
                Console.WriteLine($"Processing order #{orderId}...");
                
                // Simulate business logic - fail order 2 to demonstrate error handling
                if (orderId == 2)
                {
                    // throw = raises exception to be caught by catch block
                    throw new InvalidOperationException($"Order #{orderId} processing failed");
                }
                
                // Output: Order #[id] processed successfully
                Console.WriteLine($"  Order #{orderId} processed successfully");
            }
            catch (Exception ex)
            {
                // Log exception with context
                LoggingInfrastructure.LogException(ex, $"ProcessOrder #{orderId}");
                
                // Output: Error processing order #[id]: [message]
                Console.WriteLine($"  Error processing order #{orderId}: {ex.Message}");
                
                // NOTE: Continue processing other orders - don't rethrow
                // This is key for graceful degradation
            }
        }

        /// <summary>
        /// Performs cleanup operations before shutdown
        /// Saves state, releases resources, etc.
        /// </summary>
        static void PerformCleanup()
        {
            // Output: [Cleanup] Saving state and releasing resources...
            Console.WriteLine("\n[Cleanup] Saving state and releasing resources...");
        }
    }
}
