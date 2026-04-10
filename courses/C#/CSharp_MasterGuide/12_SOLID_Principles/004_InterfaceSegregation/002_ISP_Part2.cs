/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Interface Segregation Principle - Part 2
 * FILE      : 02_ISP_Part2.cs
 * PURPOSE   : Advanced ISP with real-world patterns
 * ============================================================
 */
using System; // Core System namespace for Console

namespace CSharp_MasterGuide._12_SOLID_Principles._04_InterfaceSegregation._02_ISP_Part2
{
    /// <summary>
    /// Demonstrates ISP advanced examples
    /// </summary>
    public class ISPPart2Demo
    {
        /// <summary>
        /// Entry point for ISP Part 2 examples
        /// </summary>
        public static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Role-based Interfaces
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== ISP Part 2 ===\n");

            // Output: --- Role-based Interfaces ---
            Console.WriteLine("--- Role-based Interfaces ---");

            // Different roles need different interfaces
            var viewer = new DocumentViewer();
            viewer.View();
            // Output: Viewing document

            var editor = new DocumentEditor();
            editor.Edit();
            // Output: Editing document

            var exporter = new DocumentExporter();
            exporter.Export();
            // Output: Exporting document

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Entity Interfaces
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Entity Interfaces ---
            Console.WriteLine("\n--- Entity Interfaces ---");

            // Entities expose different capabilities
            IReadable readable = new UserEntity();
            Console.WriteLine($"   Can read: {readable.CanRead()}");
            // Output: Can read: True

            IWritable writable = new UserEntity();
            Console.WriteLine($"   Can write: {writable.CanWrite()}");
            // Output: Can write: True

            IDeletable deletable = new UserEntity();
            Console.WriteLine($"   Can delete: {deletable.CanDelete()}");
            // Output: Can delete: False

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Service Interfaces
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Service Interfaces ---
            Console.WriteLine("\n--- Service Interfaces ---");

            // Services are split by capability
            IMessageSender sender = new EmailService();
            sender.Send("message");
            // Output: Email send: message

            IMessageReceiver receiver = new EmailService();
            receiver.Receive();
            // Output: Email receive

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Marker Interfaces
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Marker Interfaces ---
            Console.WriteLine("\n--- Marker Interfaces ---");

            // Mark behaviors with empty interfaces
            var cache = new InMemoryCache();
            if (cache is ICache)
            {
                Console.WriteLine("   Is cacheable");
                // Output: Is cacheable
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Composable Interfaces
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Composable Interfaces ---
            Console.WriteLine("\n--- Composable Interfaces ---");

            // Compose behaviors from small pieces
            var system = new ComposedSystem();
            
            // Use multiple small interfaces
            if (system is ILoggable loggable)
            {
                loggable.Log("event");
            }
            // Output: Logging: event

            if (system is IAuditable auditable)
            {
                Console.WriteLine("   Can audit");
            }
            // Output: Can audit

            Console.WriteLine("\n=== ISP Part 2 Complete ===");
        }
    }

    /// <summary>
    /// Viewer role interface
    /// </summary>
    public interface IViewer
    {
        void View(); // method: view document
    }

    /// <summary>
    /// Editor role interface
    /// </summary>
    public interface IEditor
    {
        void Edit(); // method: edit document
    }

    /// <summary>
    /// Exporter role interface
    /// </summary>
    public interface IExporter
    {
        void Export(); // method: export document
    }

    /// <summary>
    /// Document viewer
    /// </summary>
    public class DocumentViewer : IViewer
    {
        public void View() => Console.WriteLine("   Viewing document");
    }

    /// <summary>
    /// Document editor
    /// </summary>
    public class DocumentEditor : IEditor
    {
        public void Edit() => Console.WriteLine("   Editing document");
    }

    /// <summary>
    /// Document exporter
    /// </summary>
    public class DocumentExporter : IExporter
    {
        public void Export() => Console.WriteLine("   Exporting document");
    }

    /// <summary>
    /// Read capability interface
    /// </summary>
    public interface IReadable
    {
        bool CanRead(); // method: check read capability
    }

    /// <summary>
    /// Write capability interface
    /// </summary>
    public interface IWritable
    {
        bool CanWrite(); // method: check write capability
    }

    /// <summary>
    /// Delete capability interface
    /// </summary>
    public interface IDeletable
    {
        bool CanDelete(); // method: check delete capability
    }

    /// <summary>
    /// User entity with selective capabilities
    /// </summary>
    public class UserEntity : IReadable, IWritable, IDeletable
    {
        public bool CanRead() => true;
        public bool CanWrite() => true;
        public bool CanDelete() => false; // no delete permission
    }

    /// <summary>
    /// Message sender interface
    /// </summary>
    public interface IMessageSender
    {
        void Send(string message); // method: send message
    }

    /// <summary>
    /// Message receiver interface
    /// </summary>
    public interface IMessageReceiver
    {
        void Receive(); // method: receive message
    }

    /// <summary>
    /// Email service - implements both interfaces
    /// </summary>
    public class EmailService : IMessageSender, IMessageReceiver
    {
        public void Send(string message) => Console.WriteLine($"   Email send: {message}");
        public void Receive() => Console.WriteLine("   Email receive");
    }

    /// <summary>
    /// Cache marker interface
    /// </summary>
    public interface ICache { }

    /// <summary>
    /// In-memory cache - marked with interface
    /// </summary>
    public class InMemoryCache : ICache { }

    /// <summary>
    /// Logging interface
    /// </summary>
    public interface ILoggable
    {
        void Log(string message); // method: log message
    }

    /// <summary>
    /// Auditing interface
    /// </summary>
    public interface IAuditable
    {
        void Audit(); // method: perform audit
    }

    /// <summary>
    /// Composed system - implements multiple small interfaces
    /// </summary>
    public class ComposedSystem : ILoggable, IAuditable
    {
        public void Log(string message) => Console.WriteLine($"   Logging: {message}");
        public void Audit() => Console.WriteLine("   Performing audit");
    }
}