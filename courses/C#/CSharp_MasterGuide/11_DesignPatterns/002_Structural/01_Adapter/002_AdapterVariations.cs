/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Adapter Variations
 * FILE      : 02_AdapterVariations.cs
 * PURPOSE   : Demonstrates different Adapter pattern approaches
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural._01_Adapter
{
    /// <summary>
    /// Demonstrates Adapter variations
    /// </summary>
    public class AdapterVariations
    {
        /// <summary>
        /// Entry point for Adapter variations
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Adapter Variations ===
            Console.WriteLine("=== Adapter Variations ===\n");

            // ── CONCEPT: Two-Way Adapter ───────────────────────────────────────
            // Adapts both directions between interfaces

            // Example 1: Two-Way Adapter
            // Output: 1. Two-Way Adapter:
            Console.WriteLine("1. Two-Way Adapter:");
            
            // Can be used as either interface
            var twoWay = new TwoWayAdapter();
            
            // Use as interface A
            var resultA = twoWay.MethodA();
            // Output: MethodA result
            Console.WriteLine($"   {resultA}");
            
            // Use as interface B
            var resultB = twoWay.MethodB();
            // Output: MethodB result
            Console.WriteLine($"   {resultB}");

            // ── CONCEPT: Adapter with Factory ─────────────────────────────────
            // Factory creates appropriate adapter

            // Example 2: Adapter Factory
            // Output: 2. Adapter Factory:
            Console.WriteLine("\n2. Adapter Factory:");
            
            // Get appropriate adapter for data source
            var csvAdapter = DataAdapterFactory.GetAdapter("CSV");
            var jsonAdapter = DataAdapterFactory.GetAdapter("JSON");
            var xmlAdapter = DataAdapterFactory.GetAdapter("XML");
            
            // Read data using same interface
            var csvData = csvAdapter.Read("file.csv");
            var jsonData = jsonAdapter.Read("file.json");
            var xmlData = xmlAdapter.Read("file.xml");
            
            // Output: CSV: 100 records
            // Output: JSON: 100 records
            // Output: XML: 100 records

            // ── CONCEPT: Generic Adapter ─────────────────────────────────────
            // Reusable adapter for similar types

            // Example 3: Generic Adapter
            // Output: 3. Generic Adapter:
            Console.WriteLine("\n3. Generic Adapter:");
            
            // Use generic adapter for different types
            var intAdapter = new GenericAdapter<int>(new IntAdaptee());
            var stringAdapter = new GenericAdapter<string>(new StringAdaptee());
            
            var intResult = intAdapter.Convert(42);
            var stringResult = stringAdapter.Convert("test");
            
            // Output: Int: 42
            // Output: String: test

            // ── REAL-WORLD EXAMPLE: Service Adapter ──────────────────────────
            // Output: --- Real-World: Service Adapter ---
            Console.WriteLine("\n--- Real-World: Service Adapter ---");
            
            // External payment services have different APIs
            var stripeAdapter = new PaymentAdapterFactory.GetPaymentAdapter("Stripe");
            var paypalAdapter = new PaymentAdapterFactory.GetPaymentAdapter("PayPal");
            
            // Same interface for all
            stripeAdapter.Charge(100.00m);
            paypalAdapter.Charge(100.00m);
            
            // Output: Stripe: Charged $100.00
            // Output: PayPal: Charged $100.00

            Console.WriteLine("\n=== Adapter Variations Complete ===");
        }
    }

    /// <summary>
    /// Interface A
    /// </summary>
    public interface IInterfaceA
    {
        string MethodA(); // method: interface A method
    }

    /// <summary>
    /// Interface B
    /// </summary>
    public interface IInterfaceB
    {
        string MethodB(); // method: interface B method
    }

    /// <summary>
    /// Two-way adapter implementing both interfaces
    /// </summary>
    public class TwoWayAdapter : IInterfaceA, IInterfaceB
    {
        public string MethodA()
        {
            return "MethodA result";
        }
        
        public string MethodB()
        {
            return "MethodB result";
        }
    }

    /// <summary>
    /// Data reader interface
    /// </summary>
    public interface IDataReader
    {
        string Read(string filePath); // method: reads data
    }

    /// <summary>
    /// CSV reader
    /// </summary>
    public class CSVReader : IDataReader
    {
        public string Read(string filePath)
        {
            return "100 records"; // simulated
        }
    }

    /// <summary>
    /// JSON reader
    /// </summary>
    public class JSONReader : IDataReader
    {
        public string Read(string filePath)
        {
            return "100 records"; // simulated
        }
    }

    /// <summary>
    /// XML reader
    /// </summary>
    public class XMLReader : IDataReader
    {
        public string Read(string filePath)
        {
            return "100 records"; // simulated
        }
    }

    /// <summary>
    /// Data adapter factory
    /// </summary>
    public static class DataAdapterFactory
    {
        /// <summary>
        /// Gets appropriate data adapter
        /// </summary>
        public static IDataReader GetAdapter(string type)
        {
            return type switch
            {
                "CSV" => new CSVReader(),
                "JSON" => new JSONReader(),
                "XML" => new XMLReader(),
                _ => throw new ArgumentException($"Unknown type: {type}")
            };
        }
    }

    /// <summary>
    /// Generic adapter interface
    /// </summary>
    public interface IGenericAdapter<T>
    {
        T Convert(T input); // method: converts input
    }

    /// <summary>
    /// Int adaptee
    /// </summary>
    public class IntAdaptee
    {
        public int Process(int value) => value;
    }

    /// <summary>
    /// String adaptee
    /// </summary>
    public class StringAdaptee
    {
        public string Process(string value) => value;
    }

    /// <summary>
    /// Generic adapter
    /// </summary>
    public class GenericAdapter<T> : IGenericAdapter<T>
    {
        private dynamic _adaptee; // dynamic for any type
        
        public GenericAdapter(dynamic adaptee)
        {
            _adaptee = adaptee;
        }
        
        public T Convert(T input)
        {
            return _adaptee.Process(input);
        }
    }

    /// <summary>
    /// Payment interface
    /// </summary>
    public interface IPaymentProcessor
    {
        void Charge(decimal amount); // method: charges amount
    }

    /// <summary>
    /// Stripe payment
    /// </summary>
    public class StripePayment
    {
        public void MakePayment(string amount)
        {
            Console.WriteLine($"   Stripe: Charged ${amount}");
        }
    }

    /// <summary>
    /// PayPal payment
    /// </summary>
    public class PayPalPayment
    {
        public void SendPayment(string amount)
        {
            Console.WriteLine($"   PayPal: Charged ${amount}");
        }
    }

    /// <summary>
    /// Stripe adapter
    /// </summary>
    public class StripeAdapter : IPaymentProcessor
    {
        private StripePayment _stripe = new StripePayment();
        
        public void Charge(decimal amount)
        {
            _stripe.MakePayment(amount.ToString());
        }
    }

    /// <summary>
    /// PayPal adapter
    /// </summary>
    public class PayPalAdapter : IPaymentProcessor
    {
        private PayPalPayment _paypal = new PayPalPayment();
        
        public void Charge(decimal amount)
        {
            _paypal.SendPayment(amount.ToString());
        }
    }

    /// <summary>
    /// Payment adapter factory
    /// </summary>
    public static class PaymentAdapterFactory
    {
        /// <summary>
        /// Gets payment adapter
        /// </summary>
        public static IPaymentProcessor GetPaymentAdapter(string provider)
        {
            return provider switch
            {
                "Stripe" => new StripeAdapter(),
                "PayPal" => new PayPalAdapter(),
                _ => throw new ArgumentException($"Unknown provider: {provider}")
            };
        }
    }
}