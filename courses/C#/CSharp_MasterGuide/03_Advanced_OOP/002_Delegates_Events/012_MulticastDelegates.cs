/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Delegates and Events - Multicast Delegates
 * FILE      : MulticastDelegates.cs
 * PURPOSE   : Teaches multicast delegates in C#, combining and
 *            removing delegates, invocation order, and
 *            return value handling
 * ============================================================
 */

using System; // Core System namespace

namespace CSharp_MasterGuide._03_Advanced_OOP._02_Delegates_Events
{
    class MulticastDelegates
    {
        // Delegate with return value
        public delegate int OperationDelegate(int value);

        static void Main(string[] args)
        {
            Console.WriteLine("=== Multicast Delegates in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Combining Delegates
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Using + Operator ─────────────────────────────
            OperationDelegate del = DoubleValue;
            del += TripleValue;
            del += SquareValue;
            
            Console.WriteLine("--- Combined Delegates ---");
            int result = del(5); // Calls all in sequence, returns last result
            Console.WriteLine($"Final result (last): {result}"); // 5*5=25

            // ── EXAMPLE 2: Using Delegate.Combine ───────────────────────
            Delegate combined = Delegate.Combine(
                new OperationDelegate(DoubleValue),
                new OperationDelegate(TripleValue)
            );
            
            Console.WriteLine($"\nCombined type: {combined.GetType().Name}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Removing Delegates
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Using -= Operator ────────────────────────────
            MessageDelegateMulti multi = ShowMessageM;
            multi += PrintMessageM;
            multi += LogMessageM;
            
            Console.WriteLine("\n--- All Delegates ---");
            multi("Initial message");
            
            Console.WriteLine("\n--- After removing PrintMessage ---");
            multi -= PrintMessageM;
            multi("After removal");

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Invocation List
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Inspect Invocation List ─────────────────────
            MessageDelegateMulti del1 = ShowMessageM;
            del1 += PrintMessageM;
            del1 += LogMessageM;
            
            Console.WriteLine("\n--- Invocation List ---");
            Console.WriteLine($"Delegate count: {del1.GetInvocationList().Length}");
            
            foreach (var d in del1.GetInvocationList())
            {
                Console.WriteLine($"  Method: {d.Method.Name}");
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Return Value Behavior
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Only Last Return Value Used ─────────────────
            FuncDelegate addValues = AddFive;
            addValues += AddTen;
            
            // Only the last delegate's return value is used
            int returnResult = addValues(5);
            Console.WriteLine($"\nReturn value: {returnResult}"); // 5+10=15

            // ── EXAMPLE 2: Get All Return Values ───────────────────────
            Console.WriteLine("\n--- Getting All Results ---");
            var results = GetAllResults(5, addValues);
            foreach (var r in results)
            {
                Console.WriteLine($"  Result: {r}");
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Exception Handling
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Exception in One Delegate ───────────────────
            SafeDelegate safeDel = SafeMethod;
            safeDel += ExceptionMethod; // This will throw
            safeDel += AnotherSafeMethod;
            
            Console.WriteLine("\n--- With Exception Handling ---");
            try
            {
                safeDel("Test");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Exception caught: {ex.Message}");
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Order of Execution
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Sequential Execution ───────────────────────
            OrderDelegate order = Step1;
            order += Step2;
            order += Step3;
            
            Console.WriteLine("\n--- Execution Order ---");
            order();

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Real-World: Event Logging System
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Logger with Multiple Handlers ───────────────
            var logger = new LoggerMulti();
            logger.LogMessage += ConsoleLoggerM;
            logger.LogMessage += FileLoggerM;
            logger.LogMessage += DatabaseLoggerM;
            
            Console.WriteLine("\n--- Multi-Channel Logging ---");
            logger.Log("Application started");
            logger.Log("User logged in");

            Console.WriteLine("\n=== Multicast Delegates Complete ===");
        }

        // Operation delegates for examples
        static int DoubleValue(int x) => x * 2;
        static int TripleValue(int x) => x * 3;
        static int SquareValue(int x) => x * x;

        // Message delegates
        public delegate void MessageDelegateMulti(string message);
        
        static void ShowMessageM(string msg) => Console.WriteLine($"  SHOW: {msg}");
        static void PrintMessageM(string msg) => Console.WriteLine($"  PRINT: {msg}");
        static void LogMessageM(string msg) => Console.WriteLine($"  LOG: {msg}");

        // Return value handling
        public delegate int FuncDelegate(int value);
        
        static int AddFive(int x) => x + 5;
        static int AddTen(int x) => x + 10;

        static List<int> GetAllResults(int input, FuncDelegate del)
        {
            var results = new List<int>();
            foreach (var method in del.GetInvocationList())
            {
                var func = (FuncDelegate)method;
                results.Add(func(input));
            }
            return results;
        }

        // Exception handling
        public delegate void SafeDelegate(string message);
        
        static void SafeMethod(string msg) => Console.WriteLine($"  Safe: {msg}");
        static void ExceptionMethod(string msg) => throw new Exception("Intentional error");
        static void AnotherSafeMethod(string msg) => Console.WriteLine($"  Another safe: {msg}");

        // Order of execution
        public delegate void OrderDelegate();
        
        static void Step1() => Console.WriteLine("  Step 1: Initialize");
        static void Step2() => Console.WriteLine("  Step 2: Process");
        static void Step3() => Console.WriteLine("  Step 3: Finalize");

        // Real-World: Logger
        class LoggerMulti
        {
            public event SafeDelegate LogMessage;
            
            public void Log(string message)
            {
                LogMessage?.Invoke(message);
            }
        }

        static void ConsoleLoggerM(string msg) => Console.WriteLine($"  [CONSOLE] {msg}");
        static void FileLoggerM(string msg) => Console.WriteLine($"  [FILE] {msg}");
        static void DatabaseLoggerM(string msg) => Console.WriteLine($"  [DB] {msg}");
    }
}