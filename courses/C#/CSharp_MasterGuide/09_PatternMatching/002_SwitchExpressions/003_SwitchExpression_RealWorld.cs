/*
 * ============================================================
 * TOPIC     : Pattern Matching
 * SUBTOPIC  : Switch Expressions - Real-World
 * FILE      : 03_SwitchExpression_RealWorld.cs
 * PURPOSE   : Demonstrates practical real-world applications of switch expressions
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._09_PatternMatching._02_SwitchExpressions
{
    /// <summary>
    /// Real-world applications of switch expressions
    /// </summary>
    public class SwitchExpression_RealWorld
    {
        /// <summary>
        /// Entry point for real-world switch expression examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Real-World Switch Expression Demo ===
            Console.WriteLine("=== Real-World Switch Expression Demo ===\n");

            // ── REAL-WORLD: HTTP Status Code Handler ──────────────────────────
            // Output: 1. HTTP Status Code Handler:
            Console.WriteLine("1. HTTP Status Code Handler:");
            
            // Handle different HTTP status codes
            // GetHttpResponse returns response message
            int[] statusCodes = { 200, 201, 301, 400, 401, 403, 404, 500, 503 };
            
            // foreach = iterate through codes
            foreach (int code in statusCodes)
            {
                // Output: Status [n]: [response]
                Console.WriteLine($"   Status {code}: {GetHttpResponse(code)}");
            }

            // ── REAL-WORLD: Order Status Processor ───────────────────────────
            // Output: 2. Order Status Processor:
            Console.WriteLine("\n2. Order Status Processor:");
            
            // Process order statuses
            // GetOrderAction returns action description
            string[] statuses = { "pending", "confirmed", "shipped", "delivered", "cancelled", "returned" };
            
            foreach (string status in statuses)
            {
                // Output: Order [status]: [action]
                Console.WriteLine($"   Order {status}: {GetOrderAction(status)}");
            }

            // ── REAL-WORLD: Permission Checker ──────────────────────────────
            // Output: 3. Permission Checker:
            Console.WriteLine("\n3. Permission Checker:");
            
            // Check permissions using switch expression
            // CheckPermission returns permission result
            Console.WriteLine($"   Admin, read: {CheckPermission("admin", "read")}");
            Console.WriteLine($"   Admin, write: {CheckPermission("admin", "write")}");
            Console.WriteLine($"   User, read: {CheckPermission("user", "read")}");
            Console.WriteLine($"   User, write: {CheckPermission("user", "write")}");
            Console.WriteLine($"   Guest, read: {CheckPermission("guest", "read")}");
            Console.WriteLine($"   Guest, write: {CheckPermission("guest", "write")}");

            // ── REAL-WORLD: Date/Time Processor ───────────────────────────────
            // Output: 4. Date/Time Processor:
            Console.WriteLine("\n4. Date/Time Processor:");
            
            // Process dates to get day type
            // GetDayType returns day classification
            Console.WriteLine($"   Monday: {GetDayType(DayOfWeek.Monday)}");
            Console.WriteLine($"   Saturday: {GetDayType(DayOfWeek.Saturday)}");
            Console.WriteLine($"   Sunday: {GetDayType(DayOfWeek.Sunday)}");

            // ── REAL-WORLD: Shipping Rate Calculator ─────────────────────────
            // Output: 5. Shipping Rate Calculator:
            Console.WriteLine("\n5. Shipping Rate Calculator:");
            
            // Calculate shipping based on multiple factors
            // CalculateShippingRate returns rate
            (string zone, double weight, bool express)[] shipments = {
                ("local", 2.0, false),
                ("regional", 5.0, false),
                ("national", 10.0, false),
                ("international", 5.0, true),
                ("local", 0.5, true)
            };
            
            foreach (var shipment in shipments)
            {
                // Output: Shipment [zone], [weight]kg, express=[bool]: $[rate]
                Console.WriteLine($"   {shipment.zone}, {shipment.weight}kg, express={shipment.express}: ${CalculateShippingRate(shipment.zone, shipment.weight, shipment.express):F2}");
            }

            // ── REAL-WORLD: Tax Calculator ──────────────────────────────────
            // Output: 6. Tax Calculator:
            Console.WriteLine("\n6. Tax Calculator:");
            
            // Calculate tax based on income and filing status
            // CalculateTax returns tax amount
            (decimal income, string status)[] taxCases = {
                (25000m, "single"),
                (50000m, "single"),
                (100000m, "single"),
                (50000m, "married"),
                (100000m, "married"),
                (25000m, "head")
            };
            
            foreach (var case_ in taxCases)
            {
                // Output: Income $[income], [status]: Tax = $[tax]
                Console.WriteLine($"   ${case_.income:N0}, {case_.status}: Tax = ${CalculateTax(case_.income, case_.status):N2}");
            }

            Console.WriteLine("\n=== Real-World Switch Expression Complete ===");
        }

        /// <summary>
        /// Returns HTTP response message for status code
        /// </summary>
        public static string GetHttpResponse(int statusCode)
        {
            // Switch expression mapping status codes to messages
            return statusCode switch
            {
                // 1xx Informational
                100 => "Continue",
                101 => "Switching Protocols",
                
                // 2xx Success
                200 => "OK",
                201 => "Created",
                202 => "Accepted",
                204 => "No Content",
                
                // 3xx Redirection
                301 => "Moved Permanently",
                302 => "Found",
                304 => "Not Modified",
                
                // 4xx Client Errors
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                405 => "Method Not Allowed",
                409 => "Conflict",
                422 => "Unprocessable Entity",
                
                // 5xx Server Errors
                500 => "Internal Server Error",
                501 => "Not Implemented",
                502 => "Bad Gateway",
                503 => "Service Unavailable",
                504 => "Gateway Timeout",
                
                // Default for unknown codes
                _ => $"HTTP {statusCode}"
            };
        }

        /// <summary>
        /// Returns action to take based on order status
        /// </summary>
        public static string GetOrderAction(string status)
        {
            // Map order status to action
            return status.ToLower() switch
            {
                // pending = awaiting confirmation
                "pending" => "Awaiting confirmation",
                
                // confirmed = preparing for shipment
                "confirmed" => "Preparing for shipment",
                
                // shipped = in transit to customer
                "shipped" => "In transit to customer",
                
                // delivered = completed
                "delivered" => "Completed - no action needed",
                
                // cancelled = refund if applicable
                "cancelled" => "Process refund if applicable",
                
                // returned = initiate return process
                "returned" => "Initiate return process",
                
                // Unknown = manual review
                _ => "Manual review required"
            };
        }

        /// <summary>
        /// Checks permission for role-action combination
        /// </summary>
        public static string CheckPermission(string role, string action)
        {
            // Tuple pattern: (role, action)
            // Returns whether permission is granted
            return (role.ToLower(), action.ToLower()) switch
            {
                // Admin can do anything
                ("admin", _) => "Granted",
                
                // Manager can read and write, but not delete
                ("manager", "delete") => "Denied",
                ("manager", _) => "Granted",
                
                // User can read, write own resources
                ("user", "read") => "Granted",
                ("user", "write") => "Granted (own only)",
                ("user", "delete") => "Denied",
                
                // Guest can only read public resources
                ("guest", "read") => "Granted (public only)",
                ("guest", _) => "Denied",
                
                // Unknown role = denied
                _ => "Denied"
            };
        }

        /// <summary>
        /// Returns day type based on day of week
        /// </summary>
        public static string GetDayType(DayOfWeek day)
        {
            // DayOfWeek is enum from System
            return day switch
            {
                // Saturday or Sunday = weekend
                DayOfWeek.Saturday or DayOfWeek.Sunday => "Weekend",
                
                // Monday through Friday = weekday
                DayOfWeek.Monday or DayOfWeek.Tuesday or DayOfWeek.Wednesday 
                    or DayOfWeek.Thursday or DayOfWeek.Friday => "Weekday",
                
                // Default
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Calculates shipping rate based on zone, weight, and express option
        /// </summary>
        public static double CalculateShippingRate(string zone, double weightKg, bool express)
        {
            // Combine multiple factors in tuple pattern
            return (zone.ToLower(), weightKg, express) switch
            {
                // Local, light, not express = cheapest
                ("local", <= 1.0, false) => 5.00,
                
                // Local, light, express = moderate
                ("local", <= 1.0, true) => 12.00,
                
                // Local, any weight, not express = standard
                ("local", _, false) => 8.00,
                
                // Regional, any weight, not express = medium
                ("regional", _, false) => 15.00,
                
                // National, any weight, not express = higher
                ("national", _, false) => 25.00,
                
                // Any zone with express = premium rate
                (_, _, true) => 35.00,
                
                // International = highest rate
                ("international", _, _) => 50.00,
                
                // Default = standard rate
                _ => 10.00
            };
        }

        /// <summary>
        /// Calculates tax based on income and filing status
        /// </summary>
        public static decimal CalculateTax(decimal income, string status)
        {
            // Apply different tax brackets based on filing status
            return (income, status.ToLower()) switch
            {
                // Low income = low tax rate
                (<= 30000, "single") => income * 0.10m,
                (<= 30000, "married") => income * 0.10m,
                (<= 30000, "head") => income * 0.12m,
                
                // Medium income = medium tax rate
                (> 30000 and <= 60000, "single") => income * 0.22m,
                (> 30000 and <= 60000, "married") => income * 0.20m,
                (> 30000 and <= 60000, "head") => income * 0.22m,
                
                // High income = higher tax rate
                (> 60000 and <= 100000, "single") => income * 0.24m,
                (> 60000 and <= 100000, "married") => income * 0.22m,
                (> 60000 and <= 100000, "head") => income * 0.24m,
                
                // Very high income = highest rate
                (> 100000, "single") => income * 0.32m,
                (> 100000, "married") => income * 0.30m,
                (> 100000, "head") => income * 0.32m,
                
                // Default
                _ => income * 0.20m
            };
        }
    }
}
