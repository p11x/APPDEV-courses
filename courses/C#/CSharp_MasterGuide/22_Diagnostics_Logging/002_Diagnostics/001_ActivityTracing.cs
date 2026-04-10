/*
 * ============================================================
 * TOPIC     : Diagnostics & Logging
 * SUBTOPIC  : Diagnostics - Activity Tracing
 * FILE      : 01_ActivityTracing.cs
 * PURPOSE   : System.Diagnostics.Activity tracing
 * ============================================================
 */
using System;
using System.Diagnostics;

namespace CSharp_MasterGuide._22_Diagnostics_Logging._02_Diagnostics
{
    /// <summary>
    /// Activity tracing basics
    /// </summary>
    public class ActivityTracing
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Activity Tracing ===\n");

            // Create activity
            using var activity = new Activity("Request");
            activity.Start();
            
            Console.WriteLine("   Activity started");
            
            // Add tag
            activity.AddTag("userId", "123");
            
            // Add event
            activity.AddEvent(new ActivityEvent("Processing"));
            
            // Stop activity
            activity.Stop();
            Console.WriteLine("   Activity stopped");

            Console.WriteLine("\n=== Activity Tracing Complete ===");
        }
    }
}