/*
 * TOPIC: Exception Handling in C#
 * SUBTOPIC: Real-World Inner Exceptions - Exception Chaining and Logging
 * FILE: InnerExceptions_RealWorld.cs
 * PURPOSE: Demonstrate practical inner exception patterns in real-world scenarios with logging
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._05_ExceptionHandling._03_InnerExceptions
{
    class InnerExceptions_RealWorld
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Real-World Inner Exceptions Demo ===\n");

            RealWorldExceptionChaining();

            Console.WriteLine("\n=== Production Exception Logging ===\n");

            ProductionLogging();

            Console.WriteLine("\n=== API Exception Handling ===\n");

            APIExceptionHandling();
        }

        static void RealWorldExceptionChaining()
        {
            Console.WriteLine("1. Multi-Layer Exception Chaining:");
            Console.WriteLine("----------------------------------");

            try
            {
                var controller = new OrderController();
                controller.ProcessOrder(-1);
            }
            catch (OrderProcessingException ex)
            {
                Console.WriteLine($"API Error: {ex.ErrorCode}");
                Console.WriteLine($"User Message: {ex.UserMessage}");
                Console.WriteLine($"Timestamp: {ex.Timestamp}");
                Console.WriteLine($"Correlation ID: {ex.CorrelationId}");

                Console.WriteLine("\n=== Inner Exception Chain ===");

                var current = ex;
                int depth = 0;

                while (current != null)
                {
                    Console.WriteLine($"Depth {depth}: {current.GetType().Name}");
                    Console.WriteLine($"  Message: {current.Message}");
                    current = current.InnerException as OrderProcessingException;
                    depth++;
                }
            }

            // Output: API Error: ORD_001
            // Output: User Message: Failed to process order. Please try again later.
            // Output: Timestamp: 4/4/2026 11:18:40 AM
            // Output: Correlation ID: [guid]
            // Output: 
            // Output: === Inner Exception Chain ===
            // Output: Depth 0: OrderProcessingException
            // Output:   Message: Order processing failed
            // Output: Depth 1: OrderProcessingException
            // Output:   Message: Validation failed
        }

        static void ProductionLogging()
        {
            Console.WriteLine("2. Production Exception Logging:");
            Console.WriteLine("---------------------------------");

            var logger = new LoggingService();

            try
            {
                var service = new PaymentService(logger);
                service.ProcessPayment(0, 100);
            }
            catch (PaymentException ex)
            {
                Console.WriteLine("Exception logged to file/console");
                Console.WriteLine($"Log ID: {ex.LogId}");
            }

            // Output: Exception logged to file/console
            // Output: Log ID: [some-guid]
        }

        static void APIExceptionHandling()
        {
            Console.WriteLine("3. REST API Exception Handling:");
            Console.WriteLine("------------------------------");

            try
            {
                var apiClient = new ApiClient();
                apiClient.GetResource("/users/999");
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Status Code: {ex.StatusCode}");
                Console.WriteLine($"Error Type: {ex.ErrorType}");
                Console.WriteLine($"Detail: {ex.Detail}");
                Console.WriteLine($"Instance: {ex.Instance}");
            }

            // Output: Status Code: 404
            // Output: Error Type: https://api.example.com/errors/not-found
            // Output: Detail: The requested user was not found
            // Output: Instance: /users/999
        }
    }

    class OrderProcessingException : Exception
    {
        public string ErrorCode { get; }
        public string UserMessage { get; }
        public DateTime Timestamp { get; }
        public string CorrelationId { get; }

        public OrderProcessingException(string message, Exception inner, string errorCode, string userMessage)
            : base(message, inner)
        {
            ErrorCode = errorCode;
            UserMessage = userMessage;
            Timestamp = DateTime.UtcNow;
            CorrelationId = Guid.NewGuid().ToString();
        }
    }

    class OrderController
    {
        public void ProcessOrder(int orderId)
        {
            try
            {
                var validator = new OrderValidator();
                validator.Validate(orderId);
            }
            catch (OrderProcessingException ex)
            {
                throw new OrderProcessingException(
                    "Order processing failed", ex, "ORD_001", "Failed to process order. Please try again later.");
            }
        }
    }

    class OrderValidator
    {
        public void Validate(int orderId)
        {
            try
            {
                if (orderId <= 0)
                {
                    throw new ArgumentException("Order ID must be positive", "orderId");
                }
            }
            catch (ArgumentException ex)
            {
                throw new OrderProcessingException("Validation failed", ex, "ORD_002", "Invalid order data");
            }
        }
    }

    class LoggingService
    {
        public void LogException(Exception ex, string context)
        {
            var logEntry = new StringBuilder();
            logEntry.AppendLine($"Timestamp: {DateTime.UtcNow}");
            logEntry.AppendLine($"Context: {context}");
            logEntry.AppendLine($"Exception: {ex.GetType().Name}");
            logEntry.AppendLine($"Message: {ex.Message}");

            if (ex.InnerException != null)
            {
                logEntry.AppendLine($"Inner Exception: {ex.InnerException.GetType().Name}");
                logEntry.AppendLine($"Inner Message: {ex.InnerException.Message}");
            }

            Console.WriteLine(logEntry.ToString());
        }
    }

    class PaymentException : Exception
    {
        public string LogId { get; }

        public PaymentException(string message, Exception inner, string logId)
            : base(message, inner)
        {
            LogId = logId;
        }
    }

    class PaymentService
    {
        private readonly LoggingService _logger;

        public PaymentService(LoggingService logger)
        {
            _logger = logger;
        }

        public void ProcessPayment(int userId, decimal amount)
        {
            try
            {
                if (userId == 0)
                {
                    throw new ArgumentException("User ID is required", "userId");
                }

                if (amount <= 0)
                {
                    throw new ArgumentException("Amount must be positive", "amount");
                }
            }
            catch (ArgumentException ex)
            {
                var logId = Guid.NewGuid().ToString();
                _logger.LogException(ex, "PaymentService.ProcessPayment");
                throw new PaymentException("Payment processing failed", ex, logId);
            }
        }
    }

    class ApiException : Exception
    {
        public int StatusCode { get; }
        public string ErrorType { get; }
        public string Detail { get; }
        public string Instance { get; }

        public ApiException(string message, Exception inner, int statusCode, string errorType, string detail, string instance)
            : base(message, inner)
        {
            StatusCode = statusCode;
            ErrorType = errorType;
            Detail = detail;
            Instance = instance;
        }
    }

    class ApiClient
    {
        public void GetResource(string path)
        {
            try
            {
                if (path == "/users/999")
                {
                    throw new KeyNotFoundException("User not found");
                }
            }
            catch (KeyNotFoundException ex)
            {
                throw new ApiException(
                    "Not Found", ex, 404,
                    "https://api.example.com/errors/not-found",
                    "The requested user was not found",
                    path
                );
            }
        }
    }
}