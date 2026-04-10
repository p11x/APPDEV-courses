/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : Dispose Finalize Pattern
 * FILE      : 04_DisposeFinalize.cs
 * PURPOSE   : Teaches finalizer pattern, safety nets,
 *            and when to implement finalizers
 * ============================================================
 */

using System; // System namespace for Console, basic types
using System.IO; // For stream types

namespace CSharp_MasterGuide._08_MemoryManagement._02_IDisposable
{
    /// <summary>
    /// Demonstrates the finalize pattern (finalizer/destructor).
    /// Finalizers run when objects are garbage collected
    /// if Dispose() was never called.
    /// </summary>
    class DisposeFinalize
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // CONCEPT: Finalizer (Destructor) ──────────────────────
            // ═══════════════════════════════════════════════════════════
            // Finalizer = ~ClassName() - called by GC before memory freed
            // Purpose: Safety net for unmanaged resources
            //
            // Key points:
            // - Non-deterministic (runs at GC's discretion)
            // - No guarantees it will run at all
            // - Exceptions in finalizer are ignored
            // - Not supported in .NET 5+ for reference types
            //   (but still available for COM interop)
            //
            // MODERN APPROACH: Avoid finalizers when possible.
            // Use SafeHandle instead for unmanaged handles.

            Console.WriteLine("=== Dispose Finalize Pattern Demo ===\n");

            // ── EXAMPLE 1: Basic Finalizer ───────────────────────────
            // Class with finalizer that prints when called.

            Console.WriteLine("1. Basic finalizer:");

            // Create object with finalizer
            var objectWithFinalizer = new ObjectWithFinalizer(); // ObjectWithFinalizer = object
            objectWithFinalizer.DoWork(); // Use object
            Console.WriteLine("   Object used"); // Output: Object used

            // Explicitly dispose (suppresses finalizer)
            objectWithFinalizer.Dispose(); // Call dispose
            Console.WriteLine("   Object disposed"); // Output: Object disposed

            // ── EXAMPLE 2: Finalizer with Unmanaged Resource ───────
            // Shows proper pattern for unmanaged handles.

            Console.WriteLine("\n2. Finalizer with unmanaged resource:");

            // Create resource
            var unmanagedWrapper = new UnmanagedResourceWrapper(); // UnmanagedResourceWrapper = wrapper
            unmanagedWrapper.Allocate(); // Allocate unmanaged resource
            Console.WriteLine("   Unmanaged resource allocated"); // Output: Unmanaged resource allocated

            // Don't dispose - let GC collect it later
            // Finalizer will clean up

            // ── EXAMPLE 3: Why Finalizers Are Problematic ──────────────
            // Finalizers delay collection and can cause issues.

            Console.WriteLine("\n3. Finalizer issues:");

            Console.WriteLine("   Creating many objects with finalizers..."); // Output message
            for (int i = 0; i < 3; i++) // int = loop counter
            {
                var finalizable = new SlowFinalizer(); // SlowFinalizer = triggers delay
                finalizable.MarkInUse(); // Mark as in use
            }
            Console.WriteLine("   Objects created"); // Output: Objects created
            // When these are GC'd, finalizers cause delays

            // ── EXAMPLE 4: GC.SuppressFinalize ─────────────────────
            // Dispose should call GC.SuppressFinalize(this).

            Console.WriteLine("\n4. GC.SuppressFinalize demonstration:");

            var suppressable = new SuppressibleResource(); // SuppressibleResource = test object
            suppressable.Use(); // Use object
            Console.WriteLine("   Resource used"); // Output: Resource used

            suppressable.Dispose(); // Call dispose (suppresses finalizer)
            Console.WriteLine("   Resource disposed (finalizer suppressed)"); // Output: Resource disposed (finalizer suppressed)

            // GC suppressed - finalizer won't run

            // ── EXAMPLE 5: Dispose(bool) Pattern ────────────────────
            // Standard pattern: Dispose(bool disposing).

            Console.WriteLine("\n5. Dispose(bool) pattern:");

            var disposePattern = new DisposePatternDemo(); // DisposePatternDemo = test object
            disposePattern.Use(); // Use object
            disposePattern.Dispose(); // Dispose properly

            Console.WriteLine("   Pattern demo complete"); // Output: Pattern demo complete

            // ── EXAMPLE 6: SafeHandle Alternative ─────────────────────
            // Modern code should use SafeHandle instead of finalizers.

            Console.WriteLine("\n6. SafeHandle pattern (recommended):");

            var safeHandle = new SafeHandleWrapper(); // SafeHandleWrapper = modern pattern
            safeHandle.Acquire(); // Acquire handle
            Console.WriteLine("   Handle acquired"); // Output: Handle acquired
            safeHandle.Dispose(); // Dispose and cleanup

            Console.WriteLine("   Handle properly disposed"); // Output: Handle properly disposed

            // ── REAL-WORLD EXAMPLE: File Handle with Finalizer ────
            Console.WriteLine("\n7. Real-world: File wrapper with finalizer:");

            var fileWrapper = new FileHandleWrapper("log.txt"); // FileHandleWrapper = file wrap
            fileWrapper.Open(); // Open file
            fileWrapper.Write("Test log entry"); // Write log
            Console.WriteLine("   Log entry written"); // Output: Log entry written

            // In real app, might not call Dispose()
            // but finalizer ensures cleanup
            fileWrapper.Dispose(); // Proper dispose

            Console.WriteLine("\n=== Dispose Finalize Pattern Complete ===");
        }
    }

    /// <summary>
    /// Simple class with finalizer for demonstration.
    /// </summary>
    class ObjectWithFinalizer
    {
        public ObjectWithFinalizer() // Constructor
        {
            Console.WriteLine("   ObjectWithFinalizer: Created"); // Output: ObjectWithFinalizer: Created
        }

        public void DoWork() // Use object
        {
            Console.WriteLine("   ObjectWithFinalizer: Doing work"); // Output: ObjectWithFinalizer: Doing work
        }

        public void Dispose() // Explicit dispose
        {
            Console.WriteLine("   ObjectWithFinalizer: Dispose called"); // Output: ObjectWithFinalizer: Dispose called
            GC.SuppressFinalize(this); // Suppress finalizer
        }

        /// <summary>
        /// Finalizer - called by GC if Dispose not called.
        /// </summary>
        ~ObjectWithFinalizer() // Finalizer syntax
        {
            Console.WriteLine("   ObjectWithFinalizer: Finalizer called!"); // Output: ObjectWithFinalizer: Finalizer called!
        }
    }

    /// <summary>
    /// Wraps unmanaged resource (like file handle).
    /// Finalizer provides safety net for cleanup.
    /// </summary>
    class UnmanagedResourceWrapper
    {
        private IntPtr _unmanagedHandle = IntPtr.Zero; // IntPtr = unmanaged handle

        public void Allocate() // Allocate unmanaged resource
        {
            // Simulate allocating unmanaged handle (like file open)
            _unmanagedHandle = new IntPtr(1); // Allocate fake handle
            Console.WriteLine("   UnmanagedResourceWrapper: Handle allocated"); // Output: UnmanagedResourceWrapper: Handle allocated
        }

        public void Use() // Use the resource
        {
            if (_unmanagedHandle == IntPtr.Zero) // Check if allocated
                throw new InvalidOperationException("Not allocated"); // Throw if not

            Console.WriteLine("   UnmanagedResourceWrapper: Using handle"); // Output: UnmanagedResourceWrapper: Using handle
        }

        /// <summary>
        /// Dispose - calls GC.SuppressFinalize.
        /// </summary>
        public void Dispose() // IDisposable implementation
        {
            Dispose(disposing: true); // Call overload
            GC.SuppressFinalize(this); // Suppress finalizer
        }

        /// <summary>
        /// Protected virtual Dispose(bool) - standard pattern.
        /// </summary>
        /// <param name="disposing">true if from Dispose(), false from finalizer</param>
        protected virtual void Dispose(bool disposing) // Overload
        {
            if (_unmanagedHandle != IntPtr.Zero) // Check if handle exists
            {
                // Release unmanaged resource
                _unmanagedHandle = IntPtr.Zero; // Clear handle
                Console.WriteLine("   UnmanagedResourceWrapper: Handle released"); // Output: UnmanagedResourceWrapper: Handle released
            }
        }

        /// <summary>
        /// Finalizer - safety net if Dispose not called.
        /// </summary>
        ~UnmanagedResourceWrapper() // Finalizer
        {
            // Modern code: Use SafeHandle instead
            // This is just for demonstration
            Console.WriteLine("   UnmanagedResourceWrapper: Finalizer cleanup!"); // Output: UnmanagedResourceWrapper: Finalizer cleanup!
            Dispose(disposing: false); // Cleanup from finalizer
        }
    }

    /// <summary>
    /// Class that shows delayed collection due to finalizer.
    /// Finalizers put objects in finalization queue.
    /// </summary>
    class SlowFinalizer
    {
        private bool _inUse = false; // bool = usage flag

        public void MarkInUse() // Mark object as in use
        {
            _inUse = true; // Set flag
        }

        /// <summary>
        /// Finalizer causes object to go to finalization queue.
        /// GC must run twice to collect this object.
        /// </summary>
        ~SlowFinalizer() // Finalizer
        {
            // Simulating slow cleanup work
            Console.WriteLine("   SlowFinalizer: Running (DELAYED!)"); // Output: SlowFinalizer: Running (DELAYED!)
        }
    }

    /// <summary>
    /// Class that properly calls GC.SuppressFinalize.
    /// This avoids finalization delays.
    /// </summary>
    class SuppressibleResource
    {
        private bool _disposed = false; // bool = disposal flag

        public void Use() // Use resource
        {
            if (_disposed) // Check if disposed
                throw new ObjectDisposedException(nameof(SuppressibleResource)); // Throw if disposed

            Console.WriteLine("   SuppressibleResource: Using"); // Output: SuppressibleResource: Using
        }

        /// <summary>
        /// Dispose calls GC.SuppressFinalize to avoid finalizer running.
        /// </summary>
        public void Dispose() // IDisposable implementation
        {
            if (!_disposed) // Check if already disposed
            {
                _disposed = true; // Mark disposed
                Console.WriteLine("   SuppressibleResource: Disposed, finalizer suppressed"); // Output: SuppressibleResource: Disposed, finalizer suppressed
                GC.SuppressFinalize(this); // IMPORTANT: Suppress finalization
            }
        }

        /// <summary>
        /// Finalizer - only runs if Dispose not called.
        /// </summary>
        ~SuppressibleResource() // Finalizer
        {
            Console.WriteLine("   SuppressibleResource: Finalizer ran (should not happen)"); // Output message
        }
    }

    /// <summary>
    /// Standard Dispose(bool) pattern with finalizer.
    /// </summary>
    class DisposePatternDemo : IDisposable
    {
        private bool _disposed = false; // bool = disposal flag

        public void Use() // Use resource
        {
            if (_disposed) // Check if disposed
                throw new ObjectDisposedException(nameof(DisposePatternDemo)); // Throw if disposed

            Console.WriteLine("   DisposePatternDemo: Using"); // Output: DisposePatternDemo: Using
        }

        /// <summary>
        /// Public Dispose method.
        /// </summary>
        public void Dispose() // IDisposable.Dispose
        {
            Dispose(disposing: true); // Call overload
            GC.SuppressFinalize(this); // Suppress finalizer
        }

        /// <summary>
        /// Protected virtual Dispose(bool).
        /// </summary>
        /// <param name="disposing">true if from Dispose(), false from finalizer</param>
        protected virtual void Dispose(bool disposing) // Pattern method
        {
            if (!_disposed) // Check if disposed
            {
                if (disposing) // Check if from Dispose()
                {
                    Console.WriteLine("   DisposePatternDemo: Managed cleanup"); // Output: DisposePatternDemo: Managed cleanup
                }

                // Unmanaged cleanup would go here
                Console.WriteLine("   DisposePatternDemo: Unmanaged cleanup"); // Output: DisposePatternDemo: Unmanaged cleanup

                _disposed = true; // Mark disposed
            }
        }

        /// <summary>
        /// Finalizer - safety net.
        /// </summary>
        ~DisposePatternDemo() // Finalizer
        {
            Console.WriteLine("   DisposePatternDemo: Finalizer safety net"); // Output: DisposePatternDemo: Finalizer safety net
            Dispose(disposing: false); // Called from finalizer
        }
    }

    /// <summary>
    /// SafeHandle is the modern recommended approach.
    /// SafeHandle handles finalization automatically.
    /// </summary>
    class SafeHandleWrapper : IDisposable
    {
        // SafeHandle is recommended over manual finalizers
        // In real code, you'd inherit from SafeHandle
        private bool _isAcquired = false; // bool = handle state
        private bool _disposed = false; // bool = disposal flag

        public void Acquire() // Acquire handle
        {
            _isAcquired = true; // Set acquired
            Console.WriteLine("   SafeHandleWrapper: Handle acquired"); // Output: SafeHandleWrapper: Handle acquired
        }

        public void Use() // Use handle
        {
            if (!_isAcquired || _disposed) // Check if acquired
                throw new InvalidOperationException("Handle not acquired"); // Throw if not

            Console.WriteLine("   SafeHandleWrapper: Using handle"); // Output: SafeHandleWrapper: Using handle
        }

        /// <summary>
        /// Dispose with SafeHandle handles cleanup.
        /// </summary>
        public void Dispose() // IDisposable implementation
        {
            if (!_disposed) // Check if disposed
            {
                if (_isAcquired) // Check if acquired
                {
                    _isAcquired = false; // Release handle
                    Console.WriteLine("   SafeHandleWrapper: Handle released"); // Output: SafeHandleWrapper: Handle released
                }

                _disposed = true; // Mark disposed
                GC.SuppressFinalize(this); // Suppress finalization
            }
        }

        // SafeHandle already handles finalization
        // No ~SafeHandleWrapper needed
        ~SafeHandleWrapper() // Finalizer (not needed with SafeHandle)
        {
            // SafeHandle manages this automatically
            // This finalizer is just for demonstration
            Console.WriteLine("   SafeHandleWrapper: cleanup (if needed)"); // Output message
        }
    }

    /// <summary>
    /// Real-world wrapper for file handle.
    /// Simulates wrapping OS file handle.
    /// </summary>
    class FileHandleWrapper : IDisposable
    {
        private readonly string _filePath; // string = file path
        private IntPtr _fileHandle = IntPtr.Zero; // IntPtr = OS handle
        private bool _disposed = false; // bool = disposal flag
        private FileStream _stream = null; // FileStream = managed wrapper

        public FileHandleWrapper(string filePath) // Constructor
        {
            _filePath = filePath; // Set file path
        }

        public void Open() // Open file
        {
            _stream = new FileStream(_filePath, FileMode.Create); // Create file
            Console.WriteLine($"   FileHandleWrapper: Opened {_filePath}"); // Output: FileHandleWrapper: Opened [path]
        }

        public void Write(string content) // Write to file
        {
            if (_disposed || _stream == null) // Check if open
                throw new ObjectDisposedException(nameof(FileHandleWrapper)); // Throw if disposed

            byte[] data = System.Text.Encoding.UTF8.GetBytes(content); // byte[] = content as bytes
            _stream.Write(data, 0, data.Length); // Write data
        }

        /// <summary>
        /// Dispose releases both managed and unmanaged.
        /// </summary>
        public void Dispose() // IDisposable implementation
        {
            Dispose(disposing: true); // Call overload
            GC.SuppressFinalize(this); // Prevent finalization
        }

        /// <summary>
        /// Protected virtual Dispose(bool).
        /// </summary>
        /// <param name="disposing">true if from Dispose(), false from finalizer</param>
        protected virtual void Dispose(bool disposing) // Overload
        {
            if (!_disposed) // Check if disposed
            {
                if (disposing) // From Dispose()
                {
                    // Dispose managed resources
                    if (_stream != null) // Check if stream exists
                    {
                        _stream.Dispose(); // Dispose stream
                        _stream = null; // Clear reference
                        Console.WriteLine("   FileHandleWrapper: Stream disposed"); // Output: FileHandleWrapper: Stream disposed
                    }
                }

                // Dispose unmanaged resources (OS handle)
                if (_fileHandle != IntPtr.Zero) // Check if handle exists
                {
                    // In real code: CloseHandle(_fileHandle)
                    _fileHandle = IntPtr.Zero; // Clear handle
                    Console.WriteLine("   FileHandleWrapper: OS handle closed"); // Output: FileHandleWrapper: OS handle closed
                }

                _disposed = true; // Mark disposed
            }
        }

        /// <summary>
        /// Finalizer - safety net.
        /// </summary>
        ~FileHandleWrapper() // Finalizer
        {
            // Safety net if Dispose never called
            Console.WriteLine("   FileHandleWrapper: Finalizer safety net!"); // Output: FileHandleWrapper: Finalizer safety net!
            Dispose(disposing: false); // Cleanup from finalizer
        }
    }
}