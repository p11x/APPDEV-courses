/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Delegates and Events - Delegate Basics
 * FILE      : DelegateBasics.cs
 * PURPOSE   : Teaches delegate fundamentals in C#, declaring delegates,
 *            invoking delegates, multicast delegates, and
 *            delegate types
 * ============================================================
 */

using System; // Core System namespace

namespace CSharp_MasterGuide._03_Advanced_OOP._02_Delegates_Events
{
    class DelegateBasics
    {
        // Delegate declaration - defines the signature
        public delegate void MessageDelegate(string message);
        public delegate int CalculateDelegate(int a, int b);
        public delegate bool FilterDelegate(int number);

        static void Main(string[] args)
        {
            Console.WriteLine("=== Delegate Basics in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: What is a Delegate?
            // ═══════════════════════════════════════════════════════════

            // Delegates are type-safe function pointers
            // They allow passing methods as parameters
            
            // ── EXAMPLE 1: Simple Delegate ───────────────────────────────
            MessageDelegate del = ShowMessage;
            del("Hello from delegate!"); // Invoke the delegate

            // ── EXAMPLE 2: Delegate with Return Value ────────────────────
            CalculateDelegate calc = Add;
            int result = calc(10, 20);
            Console.WriteLine($"\nCalculation result: {result}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Delegate Invocation
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Invoke Directly ───────────────────────────────
            var delegate1 = new MessageDelegate(ShowMessage);
            delegate1.Invoke("Direct invoke");

            // ── EXAMPLE 2: Use as Method Parameter ──────────────────────
            ProcessMessage("Direct call", ShowMessage);

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Multicast Delegates
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Combine Delegates ───────────────────────────
            MessageDelegate multi = ShowMessage;
            multi += PrintMessage;
            multi += LogMessage;
            
            Console.WriteLine("\n--- Multicast Delegate ---");
            multi("Calling all methods!"); // Calls all in sequence

            // ── EXAMPLE 2: Remove from Delegate ─────────────────────────
            multi -= PrintMessage;
            Console.WriteLine("\n--- After removing PrintMessage ---");
            multi("Only Show and Log remain");

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Built-in Delegate Types
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Action Delegate (no return) ──────────────────
            Action<string> action = s => Console.WriteLine($"  Action: {s}");
            action("Hello");
            
            Action<string, int> action2 = (s, i) => Console.WriteLine($"  {s} {i}");
            action2("Count:", 42);

            // ── EXAMPLE 2: Func Delegate (with return) ──────────────────
            Func<int, int, int> func = (a, b) => a + b;
            Console.WriteLine($"\nFunc result: {func(5, 3)}");
            
            Func<string, string> func2 = s => s.ToUpper();
            Console.WriteLine($"Func string: {func2("hello")}");

            // ── EXAMPLE 3: Predicate Delegate (returns bool) ───────────
            Predicate<int> isEven = n => n % 2 == 0;
            Console.WriteLine($"\nIs 4 even: {isEven(4)}");
            Console.WriteLine($"Is 5 even: {isEven(5)}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Anonymous Methods
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Anonymous Method ─────────────────────────────
            FilterDelegate filter = delegate (int number)
            {
                return number > 5;
            };
            
            Console.WriteLine($"\nAnonymous filter > 5: {filter(10)}");

            // ── EXAMPLE 2: Lambda as Delegate ──────────────────────────
            FilterDelegate filterLambda = n => n > 5;
            Console.WriteLine($"Lambda filter > 5: {filterLambda(3)}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World: Callback Pattern
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Callback with Delegate ──────────────────────
            var processor = new DataProcessor();
            processor.Process(
                new[] { 1, 2, 3, 4, 5 },
                n => n * 2 // callback to transform each item
            );

            Console.WriteLine("\n=== Delegate Basics Complete ===");
        }

        // Methods that match delegate signatures
        static void ShowMessage(string msg)
        {
            Console.WriteLine($"  ShowMessage: {msg}");
        }

        static void PrintMessage(string msg)
        {
            Console.WriteLine($"  PrintMessage: {msg}");
        }

        static void LogMessage(string msg)
        {
            Console.WriteLine($"  LogMessage: {msg}");
        }

        static int Add(int a, int b)
        {
            return a + b;
        }

        // Method that accepts delegate as parameter
        static void ProcessMessage(string message, MessageDelegate callback)
        {
            Console.WriteLine($"  Processing: {message}");
            callback?.Invoke(message);
        }

        // Data processor using callback
        class DataProcessor
        {
            public void Process(int[] data, Func<int, int> transform)
            {
                Console.WriteLine("\n--- Processing with callback ---");
                foreach (var item in data)
                {
                    var result = transform(item);
                    Console.WriteLine($"  {item} -> {result}");
                }
            }
        }
    }
}