/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Bridge Pattern Extended
 * FILE      : 02_Bridge_Part2.cs
 * PURPOSE   : Extended Bridge pattern examples
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural._01_Bridge
{
    /// <summary>
    /// Extended Bridge pattern examples
    /// </summary>
    public class BridgePatternExtended
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Bridge Pattern Extended ===\n");

            // Example: Payment Methods
            Console.WriteLine("1. Payment Bridge:");
            var creditProcessor = new PaymentProcessor(new CreditCard());
            creditProcessor.Process(100);
            
            var paypalProcessor = new PaymentProcessor(new PayPal());
            paypalProcessor.Process(50);

            Console.WriteLine("\n=== Bridge Extended Complete ===");
        }
    }

    public interface IPaymentGateway
    {
        void Pay(decimal amount);
    }

    public class CreditCard : IPaymentGateway
    {
        public void Pay(decimal amount) => Console.WriteLine($"   Paid ${amount} via Credit Card");
    }

    public class PayPal : IPaymentGateway
    {
        public void Pay(decimal amount) => Console.WriteLine($"   Paid ${amount} via PayPal");
    }

    public class PaymentProcessor
    {
        private readonly IPaymentGateway _gateway;
        
        public PaymentProcessor(IPaymentGateway gateway)
        {
            _gateway = gateway;
        }
        
        public void Process(decimal amount)
        {
            _gateway.Pay(amount);
        }
    }
}