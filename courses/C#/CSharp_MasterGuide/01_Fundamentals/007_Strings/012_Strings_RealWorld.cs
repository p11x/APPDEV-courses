/*
 * ============================================================
 * TOPIC     : Fundamentals - Strings
 * SUBTOPIC  : Real-World String Applications
 * FILE      : Strings_RealWorld.cs
 * PURPOSE   : Demonstrates string operations in real-world applications
 *            including data parsing, validation, formatting, and manipulation
 * ============================================================
 */

using System; // Core System namespace
using System.Text; // For StringBuilder
using System.Text.RegularExpressions; // For regex

namespace CSharp_MasterGuide._01_Fundamentals._07_Strings
{
    class Strings_RealWorld
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // APPLICATION 1: User Input Validation System
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== User Input Validation System ===\n");

            // ── Validate form submission ───────────────────────────────
            // Simulate form field inputs
            var formFields = new (string Field, string Value, bool Required)[]
            {
                ("Username", "john_doe", true),
                ("Email", "john@example.com", true),
                ("Phone", "555-123-4567", false),
                ("Age", "25", true),
                ("Website", "invalid-url", false) // Intentionally invalid
            };

            foreach (var (field, value, required) in formFields)
            {
                string result = ValidateField(field, value, required);
                Console.WriteLine(result);
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 2: CSV Data Processor
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== CSV Data Processor ===\n");

            // Simulate CSV content
            string csvContent = @"ID,Name,Email,Department,Salary
1,John Smith,john.smith@company.com,Engineering,75000
2,Jane Doe,jane.doe@company.com,Marketing,65000
3,Bob Wilson,bob.wilson@company.com,Sales,55000
4,Alice Brown,alice.brown@company.com,Engineering,80000
5,Charlie Davis,charlie.davis@company.com,HR,60000";

            // Parse CSV into structured data
            var employees = ParseCsv(csvContent);

            // Display parsed data
            Console.WriteLine("Parsed Employee Data:");
            foreach (var emp in employees)
            {
                Console.WriteLine($"  {emp.Id}: {emp.Name} ({emp.Department}) - ${emp.Salary:N0}");
            }

            // Filter and display engineers
            Console.WriteLine("\nEngineers:");
            foreach (var emp in employees.Where(e => e.Department == "Engineering"))
            {
                Console.WriteLine($"  {emp.Name}: ${emp.Salary:N0}");
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 3: URL Parser and Builder
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== URL Parser and Builder ===\n");

            // Parse various URLs
            string[] urls = {
                "https://api.example.com/users/123/orders?status=pending&page=1",
                "http://localhost:5000/api/products?category=electronics&sort=price",
                "https://docs.site.com/guide/getting-started?lang=en#intro"
            };

            foreach (string url in urls)
            {
                var parsed = ParseUrl(url);
                Console.WriteLine($"URL: {url}");
                Console.WriteLine($"  Scheme: {parsed.Scheme}");
                Console.WriteLine($"  Host: {parsed.Host}");
                Console.WriteLine($"  Port: {parsed.Port}");
                Console.WriteLine($"  Path: {parsed.Path}");
                Console.WriteLine($"  Query: {parsed.Query}");
                Console.WriteLine();
            }

            // Build URL programmatically
            var apiUrl = BuildUrl("https://api.example.com", "users", 
                new[] { ("status", "active"), ("limit", "10"), ("offset", "0") });
            Console.WriteLine($"Built URL: {apiUrl}");

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 4: Log Message Formatter
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Log Message Formatter ===\n");

            // Create structured log messages
            var logMessages = new[]
            {
                new LogEntry(DateTime.Now.AddMinutes(-10), "INFO", "Application started"),
                new LogEntry(DateTime.Now.AddMinutes(-5), "DEBUG", "Config loaded from settings.json"),
                new LogEntry(DateTime.Now.AddMinutes(-2), "WARN", "Cache miss for key: user_session_123"),
                new LogEntry(DateTime.Now.AddMinutes(-1), "ERROR", "Database connection timeout after 30s"),
                new LogEntry(DateTime.Now, "INFO", "User john.doe logged in successfully")
            };

            // Format logs in different formats
            Console.WriteLine("Simple Format:");
            foreach (var log in logMessages)
            {
                Console.WriteLine(FormatLogSimple(log));
            }

            Console.WriteLine("\nJSON Format:");
            foreach (var log in logMessages)
            {
                Console.WriteLine(FormatLogJson(log));
            }

            Console.WriteLine("\nDetailed Format:");
            foreach (var log in logMessages)
            {
                Console.WriteLine(FormatLogDetailed(log));
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 5: Name and Address Formatter
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Name and Address Formatter ===\n");

            // Simulate raw name/address data
            var rawAddresses = new[]
            {
                "JOHN DOE, 123 MAIN STREET, NEW YORK, NY, 10001, USA",
                "jane smith, 456 oak avenue, los angeles, ca, 90001, united states",
                "BOB WILSON, 789 PINE ROAD, CHICAGO, IL, 60601, US"
            };

            Console.WriteLine("Formatted Addresses:");
            foreach (string raw in rawAddresses)
            {
                var formatted = FormatAddress(raw);
                Console.WriteLine(formatted);
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 6: Text Sanitizer for Display
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Text Sanitizer for Display ===\n");

            // Simulate user input that needs sanitization
            string[] userInputs = {
                "<script>alert('XSS')</script>",
                "Hello <b>World</b> & \"Goodbye\"",
                "Visit https://example.com for more info",
                "Special chars: < > & \" ' and emoji 😀",
                "  Multiple   spaces   and\n\nnewlines"
            };

            Console.WriteLine("Original -> Sanitized:");
            foreach (string input in userInputs)
            {
                string sanitized = SanitizeForDisplay(input);
                Console.WriteLine($"  Input: {TruncateForDisplay(input, 40)}");
                Console.WriteLine($"  Output: {TruncateForDisplay(sanitized, 40)}");
                Console.WriteLine();
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 7: Code Snippet Formatter
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Code Snippet Formatter ===\n");

            // Simulate code with various formatting needs
            string[] codeSnippets = {
                "public static void main(string[] args){Console.WriteLine(\"Hello\");}",
                "var x=1;var y=2;var z=x+y;",
                "if(condition){DoSomething();}else{DoOther();}"
            };

            Console.WriteLine("Formatted Code:");
            foreach (string code in codeSnippets)
            {
                string formatted = FormatCodeSnippet(code);
                Console.WriteLine(formatted);
                Console.WriteLine();
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 8: File Path Utilities
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== File Path Utilities ===\n");

            // Various file path operations
            string[] filePaths = {
                @"C:\Users\John\Documents\Project\file.txt",
                @"/home/user/docs/image.png",
                "relative/path/to/file.cs",
                @"C:\Program Files\App\app.dll",
                "filename.with.dots.txt"
            };

            foreach (string path in filePaths)
            {
                Console.WriteLine($"Path: {path}");
                Console.WriteLine($"  Filename: {GetFileName(path)}");
                Console.WriteLine($"  Extension: {GetExtension(path)}");
                Console.WriteLine($"  Directory: {GetDirectory(path)}");
                Console.WriteLine();
            }

            // Normalize paths
            string messyPath = @"C:\users\john\..john\documents\.\file.txt";
            Console.WriteLine($"Original: {messyPath}");
            Console.WriteLine($"Normalized: {NormalizePath(messyPath)}");

            Console.WriteLine("\n=== Strings Real-World Complete ===");
        }

        // ═══════════════════════════════════════════════════════════
        // HELPER METHODS AND CLASSES
        // ═══════════════════════════════════════════════════════════

        // Application 1: Field Validation
        static string ValidateField(string fieldName, string value, bool required)
        {
            // Check required
            if (required && string.IsNullOrWhiteSpace(value))
            {
                return $"[ERROR] {fieldName}: Required field is empty";
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return $"[OK] {fieldName}: (empty, not required)";
            }

            // Validate based on field type
            switch (fieldName.ToLower())
            {
                case "username":
                    if (!Regex.IsMatch(value, @"^[a-zA-Z0-9_]{3,20}$"))
                        return $"[ERROR] {fieldName}: Invalid username (3-20 alphanum)";
                    break;

                case "email":
                    if (!Regex.IsMatch(value, @"^[\w\.-]+@[\w\.-]+\.\w{2,}$"))
                        return $"[ERROR] {fieldName}: Invalid email format";
                    break;

                case "phone":
                    if (!Regex.IsMatch(value, @"^\(?([0-9]{3})\)?[-.\s]?([0-9]{3})[-.\s]?([0-9]{4})$"))
                        return $"[ERROR] {fieldName}: Invalid phone format";
                    break;

                case "age":
                    if (!int.TryParse(value, out int age) || age < 0 || age > 150)
                        return $"[ERROR] {fieldName}: Invalid age";
                    break;

                case "website":
                    if (!Regex.IsMatch(value, @"^https?://[\w\.-]+"))
                        return $"[ERROR] {fieldName}: Invalid URL (must start with http:// or https://)";
                    break;
            }

            return $"[OK] {fieldName}: {value}";
        }

        // Application 2: CSV Parser
        class Employee
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public string Email { get; set; } = "";
            public string Department { get; set; } = "";
            public decimal Salary { get; set; }
        }

        static List<Employee> ParseCsv(string csv)
        {
            var employees = new List<Employee>();
            var lines = csv.Split('\n');

            // Skip header row
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                var parts = line.Split(',');
                if (parts.Length >= 5)
                {
                    employees.Add(new Employee
                    {
                        Id = int.Parse(parts[0].Trim()),
                        Name = parts[1].Trim(),
                        Email = parts[2].Trim(),
                        Department = parts[3].Trim(),
                        Salary = decimal.Parse(parts[4].Trim())
                    });
                }
            }

            return employees;
        }

        // Application 3: URL Parser
        class ParsedUrl
        {
            public string Scheme { get; set; } = "";
            public string Host { get; set; } = "";
            public int Port { get; set; }
            public string Path { get; set; } = "";
            public string Query { get; set; } = "";
        }

        static ParsedUrl ParseUrl(string url)
        {
            var result = new ParsedUrl();

            // Match pattern: scheme://host[:port]/path[?query]
            var match = Regex.Match(url, @"^(https?)://([^/:]+)(?::(\d+))?(/[^\?]*)?(\?.*)?$");

            if (match.Success)
            {
                result.Scheme = match.Groups[1].Value;
                result.Host = match.Groups[2].Value;
                result.Port = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : (result.Scheme == "https" ? 443 : 80);
                result.Path = match.Groups[4].Success ? match.Groups[4].Value : "/";
                result.Query = match.Groups[5].Success ? match.Groups[5].Value : "";
            }

            return result;
        }

        static string BuildUrl(string baseUrl, string endpoint, (string Key, string Value)[] queryParams)
        {
            var sb = new StringBuilder(baseUrl.TrimEnd('/'));
            sb.Append('/');
            sb.Append(endpoint.TrimStart('/'));
            
            if (queryParams.Length > 0)
            {
                sb.Append('?');
                for (int i = 0; i < queryParams.Length; i++)
                {
                    if (i > 0) sb.Append('&');
                    sb.Append(Uri.EscapeDataString(queryParams[i].Key));
                    sb.Append('=');
                    sb.Append(Uri.EscapeDataString(queryParams[i].Value));
                }
            }

            return sb.ToString();
        }

        // Application 4: Log Formatter
        class LogEntry
        {
            public DateTime Timestamp { get; }
            public string Level { get; }
            public string Message { get; }

            public LogEntry(DateTime timestamp, string level, string message)
            {
                Timestamp = timestamp;
                Level = level;
                Message = message;
            }
        }

        static string FormatLogSimple(LogEntry log)
        {
            return $"{log.Timestamp:HH:mm:ss} [{log.Level}] {log.Message}";
        }

        static string FormatLogJson(LogEntry log)
        {
            return $"{{\"timestamp\":\"{log.Timestamp:o}\",\"level\":\"{log.Level}\",\"message\":\"{log.Message}\"}}";
        }

        static string FormatLogDetailed(LogEntry log)
        {
            return $"[{log.Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{log.Level.PadRight(5)}] {log.Message}";
        }

        // Application 5: Address Formatter
        static string FormatAddress(string raw)
        {
            var parts = raw.Split(',').Select(p => p.Trim()).ToArray();
            
            if (parts.Length < 6) return raw;

            string name = ToTitleCase(parts[0]);
            string street = ToTitleCase(parts[1]);
            string city = ToTitleCase(parts[2]);
            string state = parts[3].ToUpper();
            string zip = parts[4];
            string country = parts[5].ToUpper();

            return $"{name}\n{street}\n{city}, {state} {zip}\n{country}";
        }

        static string ToTitleCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            
            return string.Join(" ", input.Split(' ')
                .Select(w => char.ToUpper(w[0]) + (w.Length > 1 ? w.Substring(1).ToLower() : "")));
        }

        // Application 6: Text Sanitizer
        static string SanitizeForDisplay(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            // HTML encode special characters
            string result = input
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&#x27;");

            // Convert URLs to links (simplified)
            result = Regex.Replace(result, 
                @"(https?://[^\s]+)", 
                "<a href=\"$1\">$1</a>");

            // Normalize whitespace
            result = Regex.Replace(result, @"\s+", " ");

            return result;
        }

        static string TruncateForDisplay(string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input) || input.Length <= maxLength)
                return input;

            return input.Substring(0, maxLength - 3) + "...";
        }

        // Application 7: Code Formatter
        static string FormatCodeSnippet(string code)
        {
            // Simple formatting - add spaces around operators
            string formatted = Regex.Replace(code, @"([{};=])", " $1 ");
            
            // Clean up multiple spaces
            formatted = Regex.Replace(formatted, @"\s+", " ").Trim();
            
            // Add newlines after { and before }
            formatted = Regex.Replace(formatted, @"\{\s*", "{\n");
            formatted = Regex.Replace(formatted, @"\s*\}", "\n}");
            
            return formatted;
        }

        // Application 8: Path Utilities
        static string GetFileName(string path)
        {
            int lastSep = Math.Max(path.LastIndexOf('/'), path.LastIndexOf('\\'));
            return lastSep >= 0 ? path.Substring(lastSep + 1) : path;
        }

        static string GetExtension(string path)
        {
            string fileName = GetFileName(path);
            int dot = fileName.LastIndexOf('.');
            return dot >= 0 ? fileName.Substring(dot) : "";
        }

        static string GetDirectory(string path)
        {
            int lastSep = Math.Max(path.LastIndexOf('/'), path.LastIndexOf('\\'));
            return lastSep >= 0 ? path.Substring(0, lastSep) : "";
        }

        static string NormalizePath(string path)
        {
            // Simple normalization - replace backslashes, handle ..
            return path.Replace('\\', '/')
                      .Replace("//", "/");
        }
    }
}