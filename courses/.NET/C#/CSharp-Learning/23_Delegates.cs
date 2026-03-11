/*
================================================================================
TOPIC 23: DELEGATES
================================================================================

Delegates are type-safe function pointers that allow passing methods as parameters.

TABLE OF CONTENTS:
1. What are Delegates?
2. Declaring Delegates
3. Using Delegates
4. Multicast Delegates
================================================================================
*/

using System;

namespace DelegateExamples
{
    // Delegate declaration
    delegate int Operation(int a, int b);
    
    class Program
    {
        // Methods that match delegate signature
        static int Add(int a, int b) => a + b;
        static int Multiply(int a, int b) => a * b;
        
        static void Main()
        {
            // Create delegate instance
            Operation op = Add;
            
            // Invoke delegate
            int result = op(5, 3);
            Console.WriteLine($"Add result: {result}");
            
            // Change delegate reference
            op = Multiply;
            result = op(5, 3);
            Console.WriteLine($"Multiply result: {result}");
            
            // Multicast delegate
            Console.WriteLine("\n=== Multicast Delegate ===");
            Operation multi = Add;
            multi += Multiply;
            
            // Calling multicast - only last result returned
            result = multi(5, 3);
            Console.WriteLine($"Multicast result: {result}");
            
            // Invoke all with GetInvocationList
            Delegate[] delegates = multi.GetInvocationList();
            foreach (Operation d in delegates)
            {
                Console.WriteLine($"{d.Method.Name}: {d(5, 3)}");
            }
        }
    }
}

/*
DELEGATE FEATURES:
------------------
- Type-safe function references
- Can be multicast (multiple methods)
- Enable callback patterns
- Foundation for events and lambdas
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 24 covers Events.
*/
