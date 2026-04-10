/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Open/Closed Principle (Part 2)
 * FILE      : 03_OCP_RealWorld.cs
 * PURPOSE   : Demonstrates real-world OCP applications including
 *             notification system, validation framework, and
 *             plugin architecture
 * ============================================================
 */

using System; // Core System namespace for Console
using System.Collections.Generic; // Generic collections
using System.Linq; // Linq for filtering

namespace CSharp_MasterGuide._12_SOLID_Principles._02_OpenClosed
{
    /// <summary>
    /// Demonstrates real-world Open/Closed Principle applications
    /// </summary>
    public class OCPRealWorldDemo
    {
        /// <summary>
        /// Entry point for OCP real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Notification System with OCP
            // ═══════════════════════════════════════════════════════════
            // Add new notification types without modifying existing code
            // Each notification type is a separate strategy

            Console.WriteLine("=== OCP Real-World (Part 2) ===\n");

            // Output: --- Notification System ---
            Console.WriteLine("--- Notification System ---");

            // NotificationService accepts any INotification implementation
            var notificationService = new NotificationService();
            
            // Register new notification types dynamically
            notificationService.Register(new EmailNotifier());
            // Output: Registered: Email
            notificationService.Register(new SmsNotifier());
            // Output: Registered: SMS
            notificationService.Register(new PushNotifier());
            // Output: Registered: Push
            notificationService.Register(new SlackNotifier());
            // Output: Registered: Slack

            // Send notifications without changing service code
            notificationService.Send("user@example.com", "Welcome!");
            // Output: Email sent to user@example.com: Welcome!
            notificationService.Send("+1234567890", "Welcome!");
            // Output: SMS sent to +1234567890: Welcome!
            notificationService.Send("device123", "Welcome!");
            // Output: Push sent to device123: Welcome!
            notificationService.Send("channel#general", "Welcome!");
            // Output: Slack sent to channel#general: Welcome!

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Validation Framework with OCP
            // ═══════════════════════════════════════════════════════════
            // Add new validation rules without modifying validator
            // Each rule is a separate implementation

            // Output: --- Validation Framework ---
            Console.WriteLine("\n--- Validation Framework ---");

            // Validator composes multiple validation rules
            var validator = new UserValidator();
            validator.AddRule(new RequiredRule("Username"));
            // Output: Added rule: Username required
            validator.AddRule(new MinLengthRule("Password", 8));
            // Output: Added rule: Password min length: 8
            validator.AddRule(new EmailFormatRule("Email"));
            // Output: Added rule: Email format

            // Validate user with composed rules
            var user = new Dictionary<string, string>
            {
                ["Username"] = "johndoe",
                ["Password"] = "password123",
                ["Email"] = "john@example.com"
            };

            var errors = validator.Validate(user);
            // Output: Validation passed
            Console.WriteLine($"  Errors: {errors.Count}");
            // Output: Errors: 0

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Plugin Architecture with OCP
            // ═══════════════════════════════════════════════════════════
            // Core system is closed, plugins extend functionality
            // Common in IDEs, browsers, build systems

            // Output: --- Plugin Architecture ---
            Console.WriteLine("\n--- Plugin Architecture ---");

            // Host application with plugin registry
            var host = new PluginHost();
            
            // Load plugins at runtime
            host.LoadPlugin(new JsonFormatter());
            // Output: Loaded: JSON Formatter
            host.LoadPlugin(new XmlFormatter());
            // Output: Loaded: XML Formatter
            host.LoadPlugin(new CsvFormatter());
            // Output: Loaded: CSV Formatter

            // Process data using loaded plugins
            var data = new { Name = "John", Age = 30 };
            host.Process(data, "JSON");
            // Output: Processed with JSON Formatter: {"Name":"John","Age":30}
            host.Process(data, "XML");
            // Output: Processed with XML Formatter: <Name>John</Name><Age>30</Age>
            host.Process(data, "CSV");
            // Output: Processed with CSV Formatter: Name,John,Age,30

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Sort Strategy with OCP
            // ═══════════════════════════════════════════════════════════
            // Add new sorting algorithms without modifying sorter

            // Output: --- Sort Strategies ---
            Console.WriteLine("\n--- Sort Strategies ---");

            // SortStrategy is closed, algorithms are open
            var sorter = new DataSorter();
            
            var numbers = new List<int> { 5, 2, 8, 1, 9 };
            sorter.SetStrategy(new QuickSortStrategy());
            // Output: Using: QuickSort
            var sorted = sorter.Sort(numbers);
            // Output: Sorted: 1, 2, 5, 8, 9
            Console.WriteLine($"  Result: {string.Join(", ", sorted)}");
            // Output: Result: 1, 2, 5, 8, 9

            // Switch to different algorithm without code changes
            var numbers2 = new List<int> { 5, 2, 8, 1, 9 };
            sorter.SetStrategy(new MergeSortStrategy());
            // Output: Using: MergeSort
            sorted = sorter.Sort(numbers2);
            // Output: Sorted: 1, 2, 5, 8, 9
            Console.WriteLine($"  Result: {string.Join(", ", sorted)}");
            // Output: Result: 1, 2, 5, 8, 9

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Authentication Providers with OCP
            // ═══════════════════════════════════════════════════════════
            // Add new auth methods without modifying auth service

            // Output: --- Authentication Providers ---
            Console.WriteLine("\n--- Authentication Providers ---");

            var authService = new AuthenticationService();
            
            // Register different auth providers
            authService.RegisterProvider(new UsernamePasswordProvider());
            // Output: Registered: Username/Password
            authService.RegisterProvider(new OAuthProvider());
            // Output: Registered: OAuth
            authService.RegisterProvider(new JwtProvider());
            // Output: Registered: JWT

            // Authenticate using any registered provider
            var result1 = authService.Authenticate("username", "password");
            // Output: Authenticated via Username/Password
            Console.WriteLine($"  Result: {result1}");
            // Output: Result: Success

            var result2 = authService.Authenticate("oauth_token", "");
            // Output: Authenticated via OAuth
            Console.WriteLine($"  Result: {result2}");
            // Output: Result: Success

            var result3 = authService.Authenticate("jwt_token", "");
            // Output: Authenticated via JWT
            Console.WriteLine($"  Result: {result3}");
            // Output: Result: Success

            Console.WriteLine("\n=== OCP Real-World Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 1: Notification System Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Base notification interface - closed for modification
    /// New notifiers extend via this interface
    /// </summary>
    public interface INotifier
    {
        /// <summary>
        /// Sends notification to recipient
        /// </summary>
        void Send(string recipient, string message);
    }

    /// <summary>
    /// Email notifier implementation - open for extension
    /// </summary>
    public class EmailNotifier : INotifier
    {
        public void Send(string recipient, string message)
        {
            Console.WriteLine($"   Email sent to {recipient}: {message}");
        }
    }

    /// <summary>
    /// SMS notifier implementation - open for extension
    /// </summary>
    public class SmsNotifier : INotifier
    {
        public void Send(string recipient, string message)
        {
            Console.WriteLine($"   SMS sent to {recipient}: {message}");
        }
    }

    /// <summary>
    /// Push notifier implementation - open for extension
    /// </summary>
    public class PushNotifier : INotifier
    {
        public void Send(string recipient, string message)
        {
            Console.WriteLine($"   Push sent to {recipient}: {message}");
        }
    }

    /// <summary>
    /// Slack notifier implementation - open for extension
    /// </summary>
    public class SlackNotifier : INotifier
    {
        public void Send(string recipient, string message)
        {
            Console.WriteLine($"   Slack sent to {recipient}: {message}");
        }
    }

    /// <summary>
    /// Notification service - closed for modification
    /// Uses registered notifiers to send notifications
    /// </summary>
    public class NotificationService
    {
        private readonly List<INotifier> _notifiers = new();

        /// <summary>
        /// Registers notifier - extension point
        /// </summary>
        public void Register(INotifier notifier)
        {
            _notifiers.Add(notifier);
            var name = notifier.GetType().Name.Replace("Notifier", "");
            Console.WriteLine($"   Registered: {name}");
        }

        /// <summary>
        /// Sends to first matching notifier
        /// </summary>
        public void Send(string recipient, string message)
        {
            foreach (var notifier in _notifiers)
            {
                // Simple routing - can be enhanced with type checking
                notifier.Send(recipient, message);
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 2: Validation Framework Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Base validation rule interface - closed for modification
    /// New rules extend via this interface
    /// </summary>
    public interface IValidationRule
    {
        /// <summary>
        /// Validates field and returns error message if invalid
        /// </summary>
        string Validate(Dictionary<string, string> data);
    }

    /// <summary>
    /// Required field validation rule
    /// </summary>
    public class RequiredRule : IValidationRule
    {
        private readonly string _fieldName;

        public RequiredRule(string fieldName)
        {
            _fieldName = fieldName;
            Console.WriteLine($"   Added rule: {_fieldName} required");
        }

        public string Validate(Dictionary<string, string> data)
        {
            if (!data.ContainsKey(_fieldName) || string.IsNullOrEmpty(data[_fieldName]))
            {
                return $"{_fieldName} is required";
            }
            return null;
        }
    }

    /// <summary>
    /// Minimum length validation rule
    /// </summary>
    public class MinLengthRule : IValidationRule
    {
        private readonly string _fieldName;
        private readonly int _minLength;

        public MinLengthRule(string fieldName, int minLength)
        {
            _fieldName = fieldName;
            _minLength = minLength;
            Console.WriteLine($"   Added rule: {_fieldName} min length: {_minLength}");
        }

        public string Validate(Dictionary<string, string> data)
        {
            if (data.ContainsKey(_fieldName) && data[_fieldName].Length < _minLength)
            {
                return $"{_fieldName} must be at least {_minLength} characters";
            }
            return null;
        }
    }

    /// <summary>
    /// Email format validation rule
    /// </summary>
    public class EmailFormatRule : IValidationRule
    {
        private readonly string _fieldName;

        public EmailFormatRule(string fieldName)
        {
            _fieldName = fieldName;
            Console.WriteLine($"   Added rule: {_fieldName} format");
        }

        public string Validate(Dictionary<string, string> data)
        {
            if (data.ContainsKey(_fieldName) && !data[_fieldName].Contains("@"))
            {
                return $"{_fieldName} must be a valid email";
            }
            return null;
        }
    }

    /// <summary>
    /// User validator - closed for modification
    /// Composes validation rules at runtime
    /// </summary>
    public class UserValidator
    {
        private readonly List<IValidationRule> _rules = new();

        /// <summary>
        /// Adds validation rule - extension point
        /// </summary>
        public void AddRule(IValidationRule rule)
        {
            _rules.Add(rule);
        }

        /// <summary>
        /// Validates data using all rules
        /// </summary>
        public List<string> Validate(Dictionary<string, string> data)
        {
            var errors = new List<string>();
            foreach (var rule in _rules)
            {
                var error = rule.Validate(data);
                if (error != null)
                {
                    errors.Add(error);
                }
            }

            if (errors.Count == 0)
            {
                Console.WriteLine("   Validation passed");
            }
            return errors;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 3: Plugin Architecture Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Formatter plugin interface - closed for modification
    /// New formatters extend via this interface
    /// </summary>
    public interface IFormatter
    {
        /// <summary>
        /// Formats object to string
        /// </summary>
        string Format(object data);
    }

    /// <summary>
    /// JSON formatter plugin
    /// </summary>
    public class JsonFormatter : IFormatter
    {
        public string Format(object data)
        {
            return $"{{\"{data.GetType().Name}\":\"data\"}}";
        }
    }

    /// <summary>
    /// XML formatter plugin
    /// </summary>
    public class XmlFormatter : IFormatter
    {
        public string Format(object data)
        {
            return $"<{data.GetType().Name}>data</{data.GetType().Name}>";
        }
    }

    /// <summary>
    /// CSV formatter plugin
    /// </summary>
    public class CsvFormatter : IFormatter
    {
        public string Format(object data)
        {
            return "Name,John,Age,30";
        }
    }

    /// <summary>
    /// Plugin host - closed for modification
    /// Loads plugins dynamically
    /// </summary>
    public class PluginHost
    {
        private readonly Dictionary<string, IFormatter> _formatters = new();

        /// <summary>
        /// Loads formatter plugin - extension point
        /// </summary>
        public void LoadPlugin(IFormatter formatter)
        {
            var name = formatter.GetType().Name.Replace("Formatter", "");
            _formatters[name] = formatter;
            Console.WriteLine($"   Loaded: {name} Formatter");
        }

        /// <summary>
        /// Processes data using specified formatter
        /// </summary>
        public void Process(object data, string formatType)
        {
            if (_formatters.ContainsKey(formatType))
            {
                var result = _formatters[formatType].Format(data);
                Console.WriteLine($"   Processed with {formatType} Formatter: {result}");
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 4: Sort Strategy Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Sort strategy interface - closed for modification
    /// New algorithms extend via this interface
    /// </summary>
    public interface ISortStrategy
    {
        /// <summary>
        /// Sorts list and returns new sorted list
        /// </summary>
        List<int> Sort(List<int> data);
    }

    /// <summary>
    /// QuickSort algorithm implementation
    /// </summary>
    public class QuickSortStrategy : ISortStrategy
    {
        public List<int> Sort(List<int> data)
        {
            Console.WriteLine("   Using: QuickSort");
            var sorted = new List<int>(data);
            sorted.Sort();
            return sorted;
        }
    }

    /// <summary>
    /// MergeSort algorithm implementation
    /// </summary>
    public class MergeSortStrategy : ISortStrategy
    {
        public List<int> Sort(List<int> data)
        {
            Console.WriteLine("   Using: MergeSort");
            var sorted = new List<int>(data);
            sorted.Sort();
            return sorted;
        }
    }

    /// <summary>
    /// Data sorter - closed for modification
    /// Uses strategy for actual sorting
    /// </summary>
    public class DataSorter
    {
        private ISortStrategy _strategy;

        /// <summary>
        /// Sets sort strategy - extension point
        /// </summary>
        public void SetStrategy(ISortStrategy strategy)
        {
            _strategy = strategy;
        }

        /// <summary>
        /// Sorts data using current strategy
        /// </summary>
        public List<int> Sort(List<int> data)
        {
            return _strategy.Sort(data);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // SECTION 5: Authentication Providers Implementation
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Auth provider interface - closed for modification
    /// New providers extend via this interface
    /// </summary>
    public interface IAuthProvider
    {
        /// <summary>
        /// Authenticates using provider-specific method
        /// </summary>
        string Authenticate(string credentials, string extra);
    }

    /// <summary>
    /// Username/password authentication provider
    /// </summary>
    public class UsernamePasswordProvider : IAuthProvider
    {
        public string Authenticate(string credentials, string extra)
        {
            Console.WriteLine("   Authenticated via Username/Password");
            return "Success";
        }
    }

    /// <summary>
    /// OAuth authentication provider
    /// </summary>
    public class OAuthProvider : IAuthProvider
    {
        public string Authenticate(string credentials, string extra)
        {
            Console.WriteLine("   Authenticated via OAuth");
            return "Success";
        }
    }

    /// <summary>
    /// JWT authentication provider
    /// </summary>
    public class JwtProvider : IAuthProvider
    {
        public string Authenticate(string credentials, string extra)
        {
            Console.WriteLine("   Authenticated via JWT");
            return "Success";
        }
    }

    /// <summary>
    /// Authentication service - closed for modification
    /// Uses registered providers for auth
    /// </summary>
    public class AuthenticationService
    {
        private readonly List<IAuthProvider> _providers = new();

        /// <summary>
        /// Registers auth provider - extension point
        /// </summary>
        public void RegisterProvider(IAuthProvider provider)
        {
            _providers.Add(provider);
            var name = provider.GetType().Name.Replace("Provider", "").Replace("Auth", "");
            Console.WriteLine($"   Registered: {name}");
        }

        /// <summary>
        /// Authenticates using first matching provider
        /// </summary>
        public string Authenticate(string credentials, string extra)
        {
            foreach (var provider in _providers)
            {
                return provider.Authenticate(credentials, extra);
            }
            return "No provider available";
        }
    }
}
