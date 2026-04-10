/*
 * ============================================================
 * TOPIC     : Diagnostics & Logging
 * SUBTOPIC  : Health Checks
 * FILE      : HealthChecks_Demo.cs
 * PURPOSE   : Application health monitoring
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._22_Diagnostics_Logging._01_Diagnostics
{
    /// <summary>
    /// Health checks demonstration
    /// </summary>
    public class HealthChecksDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Health Checks Demo ===\n");

            // Output: --- Basic Health Check ---
            Console.WriteLine("--- Basic Health Check ---");

            var check = new BasicHealthCheck();
            var result = check.Check();
            Console.WriteLine($"   Status: {result.Status}");
            // Output: Status: Healthy

            // Output: --- Database Check ---
            Console.WriteLine("\n--- Database Check ---");

            var dbCheck = new DatabaseHealthCheck();
            result = dbCheck.Check();
            Console.WriteLine($"   DB: {result.Status}");
            // Output: DB: Healthy

            // Output: --- Custom Checks ---
            Console.WriteLine("\n--- Custom Checks ---");

            var custom = new CustomHealthCheck("External API");
            result = custom.Check();
            Console.WriteLine($"   {custom.Name}: {result.Status}");
            // Output: External API: Healthy

            // Output: --- Health Check UI ---
            Console.WriteLine("\n--- Health Check UI ---");

            Console.WriteLine("   /health endpoint");
            Console.WriteLine("   /health/readyz endpoint");
            // Output: /health endpoint
            // Output: /health/readyz endpoint

            Console.WriteLine("\n=== Health Checks Complete ===");
        }
    }

    /// <summary>
    /// Health check result
    /// </summary>
    public class HealthCheckResult
    {
        public string Status { get; set; } = "Healthy"; // property: status
    }

    /// <summary>
    /// Health check interface
    /// </summary>
    public interface IHealthCheck
    {
        HealthCheckResult Check(); // method: check health
    }

    /// <summary>
    /// Basic health check
    /// </summary>
    public class BasicHealthCheck : IHealthCheck
    {
        public HealthCheckResult Check() => new HealthCheckResult();
    }

    /// <summary>
    /// Database health check
    /// </summary>
    public class DatabaseHealthCheck : IHealthCheck
    {
        public HealthCheckResult Check() => new HealthCheckResult();
    }

    /// <summary>
    /// Custom health check
    /// </summary>
    public class CustomHealthCheck : IHealthCheck
    {
        public string Name { get; } // property: name
        public CustomHealthCheck(string name) => Name = name;
        public HealthCheckResult Check() => new HealthCheckResult();
    }
}