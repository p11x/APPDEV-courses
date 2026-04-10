/*
 * ============================================================
 * TOPIC     : Diagnostics & Logging
 * SUBTOPIC  : OpenTelemetry - Distributed Tracing
 * FILE      : OpenTelemetry_Demo.cs
 * PURPOSE   : Using OpenTelemetry for observability
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._22_Diagnostics_Logging._02_Diagnostics
{
    /// <summary>
    /// OpenTelemetry demonstration
    /// </summary>
    public class OpenTelemetryDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== OpenTelemetry Demo ===\n");

            // Output: --- Traces ---
            Console.WriteLine("--- Traces ---");

            using var tracer = new TracerProvider().GetTracer("my-service");
            using var span = tracer.StartSpan("operation");
            span.End();
            Console.WriteLine("   Span created");
            // Output: Span created

            // Output: --- Metrics ---
            Console.WriteLine("\n--- Metrics ---");

            var meter = new Meter("my-service");
            var counter = meter.CreateCounter("requests");
            counter.Add(1);
            Console.WriteLine("   Counter recorded");
            // Output: Counter recorded

            // Output: --- Exporters ---
            Console.WriteLine("\n--- Exporters ---");

            Console.WriteLine("   Exporting to OTLP");
            Console.WriteLine("   Exporting to Jaeger");
            Console.WriteLine("   Exporting to Zipkin");
            // Output: OTLP exporter
            // Output: Jaeger exporter

            // Output: --- Context Propagation ---
            Console.WriteLine("\n--- Context Propagation ---");

            var context = new TraceContext();
            InjectContext(context);
            Console.WriteLine("   Context injected");
            // Output: Context injected

            Console.WriteLine("\n=== OpenTelemetry Complete ===");
        }
    }

    /// <summary>
    /// Tracer provider
    /// </summary>
    public class TracerProvider
    {
        public Tracer GetTracer(string name) => new Tracer();
    }

    /// <summary>
    /// Tracer
    /// </summary>
    public class Tracer
    {
        public Span StartSpan(string name) => new Span();
    }

    /// <summary>
    /// Span
    /// </summary>
    public class Span
    {
        public void End() { }
    }

    /// <summary>
    /// Meter for metrics
    /// </summary>
    public class Meter
    {
        public Meter(string name) { }
        public Counter CreateCounter(string name) => new Counter();
    }

    /// <summary>
    /// Counter metric
    /// </summary>
    public class Counter
    {
        public void Add(int value) { }
    }

    /// <summary>
    /// Trace context
    /// </summary>
    public class TraceContext { }

    /// <summary>
    /// Inject context
    /// </summary>
    public static void InjectContext(TraceContext context) { }
}