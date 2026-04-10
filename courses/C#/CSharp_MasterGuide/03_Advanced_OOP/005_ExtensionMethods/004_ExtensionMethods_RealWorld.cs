/*
 * TOPIC: Extension Methods
 * SUBTOPIC: Real-World Examples
 * FILE: ExtensionMethods_RealWorld.cs
 * PURPOSE: Demonstrates practical real-world applications of extension methods including validation,
 *          data transformation, fluent API patterns, and common utility functions.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CSharp_MasterGuide._03_Advanced_OOP._05_ExtensionMethods
{
    public static class ValidationExtensions
    {
        // Validate credit card number using Luhn algorithm
        public static bool IsValidCreditCard(this string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return false;

            // Remove spaces and dashes
            string digits = cardNumber.Replace(" ", "").Replace("-", "");

            if (!digits.All(char.IsDigit) || digits.Length < 13 || digits.Length > 19)
                return false;

            // Luhn algorithm
            int sum = 0;
            bool alternate = false;
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(digits[i].ToString());
                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9;
                }
                sum += digit;
                alternate = !alternate;
            }
            return sum % 10 == 0;
        }

        // Validate URL format
        public static bool IsValidUrl(this string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        // Validate phone number (basic US format)
        public static bool IsValidPhoneNumber(this string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            // Remove common formatting characters
            string cleaned = phoneNumber.Replace("(", "").Replace(")", "")
                                       .Replace("-", "").Replace(" ", "").Replace(".", "");

            return cleaned.Length == 10 && cleaned.All(char.IsDigit);
        }

        // Validate strong password
        public static bool IsStrongPassword(this string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(c => !char.IsLetterOrDigit(c));

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }
    }

    public static class TransformationExtensions
    {
        // Convert camelCase to PascalCase
        public static string ToPascalCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            var words = Regex.Split(input, @"(?=[A-Z])")
                           .Where(s => !string.IsNullOrWhiteSpace(s))
                           .Select(s => char.ToUpper(s[0]) + s.Substring(1).ToLower());

            return string.Join("", words);
        }

        // Convert PascalCase to camelCase
        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            if (input.Length == 1)
                return input.ToLower();

            return char.ToLower(input[0]) + input.Substring(1);
        }

        // Convert to slug (URL-friendly format)
        public static string ToSlug(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Lowercase and remove special characters
            string slug = input.ToLower()
                              .Replace(" ", "-")
                              .Replace(".", "-")
                              .Replace(",", "-")
                              .Replace("(", "")
                              .Replace(")", "");

            // Remove non-alphanumeric characters (except hyphen)
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");

            // Remove multiple consecutive hyphens
            slug = Regex.Replace(slug, @"-+", "-");

            // Trim hyphens from edges
            return slug.Trim('-');
        }

        // Convert to title case with custom words preserved
        public static string ToTitleCaseEx(this string input, params string[] preserveUpper)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            var preserveSet = preserveUpper.Select(s => s.ToLower()).ToHashSet();
            var words = input.Split(' ');
            var result = new List<string>();

            foreach (var word in words)
            {
                if (preserveSet.Contains(word.ToLower()))
                    result.Add(word);
                else if (word.Length > 0)
                    result.Add(char.ToUpper(word[0]) + word.Substring(1).ToLower());
            }

            return string.Join(" ", result);
        }

        // Mask sensitive data (show only first and last characters)
        public static string MaskSensitive(this string input, int visibleChars = 2)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length <= visibleChars * 2)
                return input;

            string masked = new string('*', input.Length - visibleChars * 2);
            return input.Substring(0, visibleChars) + masked + input.Substring(input.Length - visibleChars);
        }
    }

    public static class FluentApiExtensions
    {
        // Fluent validation - allows chaining validation rules
        public static FluentValidator<T> Validate<T>(this T value)
        {
            return new FluentValidator<T>(value);
        }

        // Fluent string builder
        public static FluentStringBuilder Build(this string start)
        {
            return new FluentStringBuilder(start);
        }
    }

    public class FluentValidator<T>
    {
        private readonly T _value;
        private readonly List<string> _errors = new List<string>();
        private bool _isValid;

        public FluentValidator(T value)
        {
            _value = value;
            _isValid = true;
        }

        public FluentValidator<T> NotNull(string errorMessage = null)
        {
            if (_value == null)
            {
                _isValid = false;
                _errors.Add(errorMessage ?? "Value cannot be null");
            }
            return this;
        }

        public FluentValidator<T> NotEmpty(string errorMessage = null) where T : class
        {
            if (_value == null)
            {
                _isValid = false;
                _errors.Add(errorMessage ?? "Value cannot be null");
            }
            else if (_value is string str && string.IsNullOrWhiteSpace(str))
            {
                _isValid = false;
                _errors.Add(errorMessage ?? "String cannot be empty");
            }
            else if (_value is ICollection<object> col && col.Count == 0)
            {
                _isValid = false;
                _errors.Add(errorMessage ?? "Collection cannot be empty");
            }
            return this;
        }

        public FluentValidator<T> MinLength(int length, string errorMessage = null)
        {
            if (_value is string str && str.Length < length)
            {
                _isValid = false;
                _errors.Add(errorMessage ?? $"Minimum length is {length}");
            }
            return this;
        }

        public FluentValidator<T> MaxLength(int length, string errorMessage = null)
        {
            if (_value is string str && str.Length > length)
            {
                _isValid = false;
                _errors.Add(errorMessage ?? $"Maximum length is {length}");
            }
            return this;
        }

        public FluentValidator<T> Custom(Func<T, bool> predicate, string errorMessage)
        {
            if (!predicate(_value))
            {
                _isValid = false;
                _errors.Add(errorMessage);
            }
            return this;
        }

        public (bool IsValid, IEnumerable<string> Errors) Result()
        {
            return (_isValid, _errors);
        }

        public void ThrowIfInvalid()
        {
            if (!_isValid)
                throw new InvalidOperationException(string.Join("; ", _errors));
        }
    }

    public class FluentStringBuilder
    {
        private readonly StringBuilder _sb;

        public FluentStringBuilder(string start)
        {
            _sb = new StringBuilder(start);
        }

        public FluentStringBuilder Append(string text)
        {
            _sb.Append(text);
            return this;
        }

        public FluentStringBuilder AppendLine(string text = "")
        {
            _sb.AppendLine(text);
            return this;
        }

        public FluentStringBuilder AppendLineOnce(string text)
        {
            if (!_sb.ToString().EndsWith(text + Environment.NewLine))
                _sb.AppendLine(text);
            return this;
        }

        public FluentStringBuilder AppendFormatted(string format, params object[] args)
        {
            _sb.AppendFormat(format, args);
            return this;
        }

        public FluentStringBuilder AppendIf(bool condition, string text)
        {
            if (condition)
                _sb.Append(text);
            return this;
        }

        public FluentStringBuilder Indent(int spaces = 4)
        {
            _sb.Append(new string(' ', spaces));
            return this;
        }

        public override string ToString() => _sb.ToString();
    }

    public static class CollectionExtensions
    {
        // Add item only if not null
        public static void AddIfNotNull<T>(this List<T> list, T item) where T : class
        {
            if (item != null)
                list.Add(item);
        }

        // Add item only if predicate returns true
        public static void AddIf<T>(this List<T> list, T item, Func<T, bool> predicate)
        {
            if (predicate(item))
                list.Add(item);
        }

        // Add range with filter
        public static void AddRangeWhere<T>(this List<T> list, IEnumerable<T> items, Func<T, bool> predicate)
        {
            foreach (var item in items.Where(predicate))
            {
                list.Add(item);
            }
        }

        // Get or default if index out of range
        public static T GetOrDefault<T>(this List<T> list, int index, T defaultValue = default(T))
        {
            return index >= 0 && index < list.Count ? list[index] : defaultValue;
        }

        // Update all elements in place
        public static void UpdateAll<T>(this List<T> list, Func<T, T> transformer)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = transformer(list[i]);
            }
        }
    }

    public static class DateTimeExtensions
    {
        // Check if date is today
        public static bool IsToday(this DateTime date)
        {
            return date.Date == DateTime.Today;
        }

        // Check if date is in current month
        public static bool IsCurrentMonth(this DateTime date)
        {
            return date.Year == DateTime.Now.Year && date.Month == DateTime.Now.Month;
        }

        // Get relative time description (e.g., "2 hours ago")
        public static string ToRelativeTime(this DateTime date)
        {
            var span = DateTime.Now - date;

            if (span.TotalSeconds < 60)
                return "just now";
            if (span.TotalMinutes < 60)
                return $"{(int)span.TotalMinutes} minute{(span.TotalMinutes >= 2 ? "s" : "")} ago";
            if (span.TotalHours < 24)
                return $"{(int)span.TotalHours} hour{(span.TotalHours >= 2 ? "s" : "")} ago";
            if (span.TotalDays < 30)
                return $"{(int)span.TotalDays} day{(span.TotalDays >= 2 ? "s" : "")} ago";
            if (span.TotalDays < 365)
                return $"{(int)(span.TotalDays / 30)} month{((span.TotalDays / 30) >= 2 ? "s" : "")} ago";

            return $"{(int)(span.TotalDays / 365)} year{((span.TotalDays / 365) >= 2 ? "s" : "")} ago";
        }

        // Get start of week (Monday)
        public static DateTime StartOfWeek(this DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-diff).Date;
        }

        // Get start of month
        public static DateTime StartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
    }

    class ExtensionMethods_RealWorld
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== VALIDATION EXTENSIONS ===");

            string creditCard = "4532015112830366";
            Console.WriteLine($"Credit card '{creditCard}' valid: {creditCard.IsValidCreditCard()}");
            // Output: Credit card '4532015112830366' valid: True

            string invalidCard = "1234567890123456";
            Console.WriteLine($"Credit card '{invalidCard}' valid: {invalidCard.IsValidCreditCard()}");
            // Output: Credit card '1234567890123456' valid: False

            string url = "https://www.example.com/page";
            Console.WriteLine($"URL '{url}' valid: {url.IsValidUrl()}");
            // Output: URL 'https://www.example.com/page' valid: True

            string phone = "(555) 123-4567";
            Console.WriteLine($"Phone '{phone}' valid: {phone.IsValidPhoneNumber()}");
            // Output: Phone '(555) 123-4567' valid: True

            string weakPass = "password";
            string strongPass = "P@ssw0rd!";
            Console.WriteLine($"Password '{weakPass}' strong: {weakPass.IsStrongPassword()}");
            // Output: Password 'password' strong: False
            Console.WriteLine($"Password '{strongPass}' strong: {strongPass.IsStrongPassword()}");
            // Output: Password 'P@ssw0rd!' strong: True

            Console.WriteLine("\n=== TRANSFORMATION EXTENSIONS ===");

            string camelCase = "thisIsCamelCase";
            Console.WriteLine($"'{camelCase}' to PascalCase: {camelCase.ToPascalCase()}");
            // Output: 'thisIsCamelCase' to PascalCase: Thisiscamelcase

            string pascalCase = "ThisIsPascalCase";
            Console.WriteLine($"'{pascalCase}' to camelCase: {pascalCase.ToCamelCase()}");
            // Output: 'ThisIsPascalCase' to camelCase: thisIsPascalCase

            string title = "Hello World! How Are You?";
            Console.WriteLine($"'{title}' to slug: {title.ToSlug()}");
            // Output: 'Hello World! How Are You?' to slug: hello-world-how-are-you

            Console.WriteLine($"'{title}' to Title Case: {title.ToTitleCaseEx("how", "are")}");
            // Output: 'Hello World! How Are You?' to Title Case: Hello World! how Are You?

            string sensitive = "secretpassword123";
            Console.WriteLine($"Masked: {sensitive.MaskSensitive(2)}");
            // Output: Masked: se***********23

            Console.WriteLine("\n=== FLUENT API EXTENSIONS ===");

            var validation = "test@email.com"
                .Validate()
                .NotEmpty("Email cannot be empty")
                .Custom(s => s.Contains("@"), "Email must contain @")
                .Custom(s => s.Contains("."), "Email must contain a dot");

            var (isValid, errors) = validation.Result();
            Console.WriteLine($"Validation result: {isValid}, Errors: {string.Join(", ", errors)}");
            // Output: Validation result: True, Errors: 

            var sb = "Start".Build()
                .Append(" - Middle")
                .AppendLine(" - End")
                .AppendIf(true, " - Conditional");
            Console.WriteLine($"String builder result: {sb}");
            // Output: String builder result: Start - Middle - End
            //          - Conditional

            Console.WriteLine("\n=== COLLECTION EXTENSIONS ===");

            var numbers = new List<int> { 1, 2, 3 };
            numbers.AddIfNotNull<int>(4);
            numbers.AddIf(5, n => n > 3);
            Console.WriteLine($"List: {string.Join(", ", numbers)}");
            // Output: List: 1, 2, 3, 4, 5

            var element = numbers.GetOrDefault(10, -1);
            Console.WriteLine($"GetOrDefault(10, -1): {element}");
            // Output: GetOrDefault(10, -1): -1

            Console.WriteLine("\n=== DATETIME EXTENSIONS ===");

            DateTime now = DateTime.Now;
            Console.WriteLine($"Is today: {now.IsToday()}");
            // Output: Is today: True

            DateTime yesterday = DateTime.Now.AddDays(-1);
            Console.WriteLine($"Yesterday relative: {yesterday.ToRelativeTime()}");
            // Output: Yesterday relative: 1 day ago

            DateTime thisMonth = DateTime.Now;
            Console.WriteLine($"Start of month: {thisMonth.StartOfMonth():yyyy-MM-dd}");
            // Output: Start of month: 2026-04-01
        }
    }
}
