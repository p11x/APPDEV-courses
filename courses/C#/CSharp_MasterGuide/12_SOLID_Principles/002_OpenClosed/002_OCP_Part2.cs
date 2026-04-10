/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Open-Closed Principle - Part 2
 * FILE      : 02_OCP_Part2.cs
 * PURPOSE   : Advanced OCP with real-world patterns and examples
 * ============================================================
 */
using System; // Core System namespace for Console
using System.Collections.Generic; // Generic collections

namespace CSharp_MasterGuide._12_SOLID_Principles._02_OpenClosed._02_OCP_Part2
{
    /// <summary>
    /// Demonstrates OCP advanced examples
    /// </summary>
    public class OCPPart2Demo
    {
        /// <summary>
        /// Entry point for OCP Part 2 examples
        /// </summary>
        public static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Notification System
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== OCP Part 2 ===\n");

            // Output: --- Notification System ---
            Console.WriteLine("--- Notification System ---");

            // Add new notification types without modifying sender
            var notifications = new List<INotifiable>
            {
                new EmailNotification(),
                new SMSNotification(),
                new PushNotification()
            };

            var sender = new NotificationSender();
            sender.SendAll(notifications, "Hello!");
            // Output: Email sent: Hello!
            // Output: SMS sent: Hello!
            // Output: Push sent: Hello!

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Payment Processing
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Payment Processing ---
            Console.WriteLine("\n--- Payment Processing ---");

            // Add new payment methods without changing processor
            var payments = new List<IPaymentMethod>
            {
                new CreditCardPayment(),
                new PayPalPayment(),
                new BankTransferPayment()
            };

            var processor = new PaymentProcessor();
            foreach (var payment in payments)
            {
                processor.Process(payment, 100m);
            }
            // Output: Credit card: $100.00
            // Output: PayPal: $100.00
            // Output: Bank transfer: $100.00

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Validation Rules
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Validation Rules ---
            Console.WriteLine("\n--- Validation Rules ---");

            // Add new rules without modifying validator
            var rules = new List<IValidationRule>
            {
                new RequiredFieldRule(),
                new EmailFormatRule(),
                new MinLengthRule { MinLength = 3 }
            };

            var validator = new FormValidator();
            var isValid = validator.Validate(rules, "test@email.com");
            Console.WriteLine($"   Valid: {isValid}");
            // Output: Valid: True

            // Add new rule - no code change needed
            rules.Add(new MaxLengthRule { MaxLength = 50 });
            isValid = validator.Validate(rules, "test@email.com");
            Console.WriteLine($"   Valid with new rule: {isValid}");
            // Output: Valid with new rule: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Export Formats
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Export Formats ---
            Console.WriteLine("\n--- Export Formats ---");

            // Add new export formats without changing exporter
            var exporters = new List<IExporter>
            {
                new JsonExporter(),
                new XmlExporter(),
                new CsvExporter()
            };

            var data = new Dictionary<string, string> { { "name", "John" } };
            var exporter = new DataExporter();
            
            foreach (var exp in exporters)
            {
                exp.Export(data);
            }
            // Output: JSON: {"name":"John"}
            // Output: XML: <name>John</name>
            // Output: CSV: name,John

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Filter Pipeline
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Filter Pipeline ---
            Console.WriteLine("\n--- Filter Pipeline ---");

            // Add filters without modifying pipeline
            var filters = new List<IFilter<string>>
            {
                new TrimFilter(),
                new UpperCaseFilter(),
                new LengthFilter { MaxLength = 10 }
            };

            var pipeline = new FilterPipeline();
            var result = pipeline.Apply(filters, "  hello  ");
            Console.WriteLine($"   Filtered: '{result}'");
            // Output: Filtered: 'HELLO'

            Console.WriteLine("\n=== OCP Part 2 Complete ===");
        }
    }

    /// <summary>
    /// Notification abstraction
    /// </summary>
    public interface INotifiable
    {
        void Send(string message); // method: sends notification
    }

    /// <summary>
    /// Email notification
    /// </summary>
    public class EmailNotification : INotifiable
    {
        public void Send(string message) => Console.WriteLine($"   Email sent: {message}");
    }

    /// <summary>
    /// SMS notification
    /// </summary>
    public class SMSNotification : INotifiable
    {
        public void Send(string message) => Console.WriteLine($"   SMS sent: {message}");
    }

    /// <summary>
    /// Push notification
    /// </summary>
    public class PushNotification : INotifiable
    {
        public void Send(string message) => Console.WriteLine($"   Push sent: {message}");
    }

    /// <summary>
    /// Notification sender - closed for modification
    /// </summary>
    public class NotificationSender
    {
        public void SendAll(List<INotifiable> notifications, string message)
        {
            foreach (var n in notifications)
            {
                n.Send(message);
            }
        }
    }

    /// <summary>
    /// Payment method abstraction
    /// </summary>
    public interface IPaymentMethod
    {
        void Process(decimal amount); // method: processes payment
    }

    /// <summary>
    /// Credit card payment
    /// </summary>
    public class CreditCardPayment : IPaymentMethod
    {
        public void Process(decimal amount) => Console.WriteLine($"   Credit card: {amount:C}");
    }

    /// <summary>
    /// PayPal payment
    /// </summary>
    public class PayPalPayment : IPaymentMethod
    {
        public void Process(decimal amount) => Console.WriteLine($"   PayPal: {amount:C}");
    }

    /// <summary>
    /// Bank transfer payment
    /// </summary>
    public class BankTransferPayment : IPaymentMethod
    {
        public void Process(decimal amount) => Console.WriteLine($"   Bank transfer: {amount:C}");
    }

    /// <summary>
    /// Payment processor - closed for modification
    /// </summary>
    public class PaymentProcessor
    {
        public void Process(IPaymentMethod method, decimal amount)
        {
            method.Process(amount);
        }
    }

    /// <summary>
    /// Validation rule abstraction
    /// </summary>
    public interface IValidationRule
    {
        bool Validate(string value); // method: validates value
    }

    /// <summary>
    /// Required field rule
    /// </summary>
    public class RequiredFieldRule : IValidationRule
    {
        public bool Validate(string value) => !string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Email format rule
    /// </summary>
    public class EmailFormatRule : IValidationRule
    {
        public bool Validate(string value) => value.Contains("@");
    }

    /// <summary>
    /// Min length rule
    /// </summary>
    public class MinLengthRule : IValidationRule
    {
        public int MinLength { get; set; } // property: minimum length
        public bool Validate(string value) => value.Length >= MinLength;
    }

    /// <summary>
    /// Max length rule - added without modifying validator
    /// </summary>
    public class MaxLengthRule : IValidationRule
    {
        public int MaxLength { get; set; } // property: maximum length
        public bool Validate(string value) => value.Length <= MaxLength;
    }

    /// <summary>
    /// Form validator - closed for modification
    /// </summary>
    public class FormValidator
    {
        public bool Validate(List<IValidationRule> rules, string value)
        {
            foreach (var rule in rules)
            {
                if (!rule.Validate(value)) return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Data exporter abstraction
    /// </summary>
    public interface IExporter
    {
        void Export(Dictionary<string, string> data); // method: exports data
    }

    /// <summary>
    /// JSON exporter
    /// </summary>
    public class JsonExporter : IExporter
    {
        public void Export(Dictionary<string, string> data)
        {
            Console.WriteLine($"   JSON: {data}");
        }
    }

    /// <summary>
    /// XML exporter
    /// </summary>
    public class XmlExporter : IExporter
    {
        public void Export(Dictionary<string, string> data)
        {
            foreach (var kvp in data)
            {
                Console.WriteLine($"   <{kvp.Key}>{kvp.Value}</{kvp.Key}>");
            }
        }
    }

    /// <summary>
    /// CSV exporter
    /// </summary>
    public class CsvExporter : IExporter
    {
        public void Export(Dictionary<string, string> data)
        {
            Console.WriteLine($"   {string.Join(",", data.Keys)}: {string.Join(",", data.Values)}");
        }
    }

    /// <summary>
    /// Data exporter - closed for modification
    /// </summary>
    public class DataExporter
    {
        public void ExportAll(List<IExporter> exporters, Dictionary<string, string> data)
        {
            foreach (var e in exporters)
            {
                e.Export(data);
            }
        }
    }

    /// <summary>
    /// Filter abstraction
    /// </summary>
    public interface IFilter<T>
    {
        T Apply(T value); // method: applies filter
    }

    /// <summary>
    /// Trim filter
    /// </summary>
    public class TrimFilter : IFilter<string>
    {
        public string Apply(string value) => value.Trim();
    }

    /// <summary>
    /// Upper case filter
    /// </summary>
    public class UpperCaseFilter : IFilter<string>
    {
        public string Apply(string value) => value.ToUpper();
    }

    /// <summary>
    /// Length filter - added without modifying pipeline
    /// </summary>
    public class LengthFilter : IFilter<string>
    {
        public int MaxLength { get; set; } // property: max length

        public string Apply(string value)
        {
            return value.Length > MaxLength
                ? value.Substring(0, MaxLength)
                : value;
        }
    }

    /// <summary>
    /// Filter pipeline - closed for modification
    /// </summary>
    public class FilterPipeline
    {
        public string Apply(List<IFilter<string>> filters, string value)
        {
            foreach (var filter in filters)
            {
                value = filter.Apply(value);
            }
            return value;
        }
    }
}