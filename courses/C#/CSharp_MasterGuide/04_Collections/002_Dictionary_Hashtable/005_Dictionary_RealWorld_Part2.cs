/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : Dictionary<TKey,TValue> - More Real-World Examples
 * FILE      : Dictionary_RealWorld_Part2.cs
 * PURPOSE   : Continues with practical real-world applications
 *            including data aggregation, state management,
 *            translation tables, and more
 * ============================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._04_Collections._02_Dictionary_Hashtable
{
    class DictionaryRealWorldPart2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Dictionary<TKey,TValue> More Real-World Examples ===\n");

            // ═══════════════════════════════════════════════════════════
            // Example 1: URL Parameter Parsing
            // ═══════════════════════════════════════════════════════════

            string url = "https://example.com/search?query=dotnet&page=1&sort=date&order=desc";
            var urlParams = ParseQueryString(url);

            Console.WriteLine("=== URL Parameters ===");
            foreach (var param in urlParams)
            {
                Console.WriteLine($"  {param.Key} = {param.Value}");
            }
            // Output:
            //   query = dotnet
            //   page = 1
            //   sort = date
            //   order = desc

            // ═══════════════════════════════════════════════════════════
            // Example 2: State Transition Machine
            // ═══════════════════════════════════════════════════════════

            var orderStatus = new Dictionary<string, List<string>>
            {
                { "Pending", new List<string> { "Confirmed", "Cancelled" } },
                { "Confirmed", new List<string> { "Processing", "Cancelled" } },
                { "Processing", new List<string> { "Shipped", "Cancelled" } },
                { "Shipped", new List<string> { "Delivered" } },
                { "Delivered", new List<string>() },
                { "Cancelled", new List<string>() }
            };

            string currentStatus = "Confirmed";
            string newStatus = "Processing";

            Console.WriteLine($"\n=== Order Status Transition ===");
            Console.WriteLine($"Current: {currentStatus} -> {newStatus}");

            if (CanTransition(orderStatus, currentStatus, newStatus))
            {
                currentStatus = newStatus;
                Console.WriteLine($"Transition allowed! New status: {currentStatus}");
                // Output: Transition allowed! New status: Processing
            }
            else
            {
                Console.WriteLine("Transition not allowed!");
            }

            // Try invalid transition
            string invalidStatus = "Delivered";
            Console.WriteLine($"\nCan go from {currentStatus} to {invalidStatus}: {CanTransition(orderStatus, currentStatus, invalidStatus)}");
            // Output: Can go from Processing to Delivered: False

            // ═══════════════════════════════════════════════════════════
            // Example 3: Translation/Localization Dictionary
            // ═══════════════════════════════════════════════════════════

            var translations = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "en", new Dictionary<string, string>
                    {
                        { "welcome", "Welcome" },
                        { "goodbye", "Goodbye" },
                        { "submit", "Submit" },
                        { "cancel", "Cancel" }
                    }
                },
                {
                    "es", new Dictionary<string, string>
                    {
                        { "welcome", "Bienvenido" },
                        { "goodbye", "Adios" },
                        { "submit", "Enviar" },
                        { "cancel", "Cancelar" }
                    }
                },
                {
                    "fr", new Dictionary<string, string>
                    {
                        { "welcome", "Bienvenue" },
                        { "goodbye", "Au revoir" },
                        { "submit", "Soumettre" },
                        { "cancel", "Annuler" }
                    }
                }
            };

            string language = "es";
            string key = "welcome";

            Console.WriteLine($"\n=== Translation [{language}] ===");
            Console.WriteLine($"{key}: {GetTranslation(translations, language, key)}");
            // Output: welcome: Bienvenido

            // List all translations for a language
            Console.WriteLine($"\nAll {language} translations:");
            foreach (var t in translations[language])
            {
                Console.WriteLine($"  {t.Key} = {t.Value}");
            }

            // ═══════════════════════════════════════════════════════════
            // Example 4: Sales Data Aggregation
            // ═══════════════════════════════════════════════════════════

            var salesData = new List<Sale>
            {
                new Sale { Product = "Laptop", Quantity = 5, Amount = 4999.95m },
                new Sale { Product = "Mouse", Quantity = 10, Amount = 299.90m },
                new Sale { Product = "Laptop", Quantity = 3, Amount = 2999.97m },
                new Sale { Product = "Keyboard", Quantity = 8, Amount = 639.92m },
                new Sale { Product = "Mouse", Quantity = 15, Amount = 449.85m },
                new Sale { Product = "Laptop", Quantity = 2, Amount = 1999.98m }
            };

            // Aggregate sales by product
            var salesByProduct = new Dictionary<string, SalesSummary>();

            foreach (var sale in salesData)
            {
                if (salesByProduct.TryGetValue(sale.Product, out var summary))
                {
                    summary.TotalQuantity += sale.Quantity;
                    summary.TotalAmount += sale.Amount;
                }
                else
                {
                    salesByProduct[sale.Product] = new SalesSummary
                    {
                        Product = sale.Product,
                        TotalQuantity = sale.Quantity,
                        TotalAmount = sale.Amount
                    };
                }
            }

            Console.WriteLine("\n=== Sales by Product ===");
            foreach (var summary in salesByProduct.Values.OrderByDescending(s => s.TotalAmount))
            {
                Console.WriteLine($"  {summary.Product}: {summary.TotalQuantity} units, {summary.TotalAmount:C}");
            }
            // Output:
            //   Laptop: 10 units, $9,999.90
            //   Mouse: 25 units, $749.75
            //   Keyboard: 8 units, $639.92

            // ═══════════════════════════════════════════════════════════
            // Example 5: Routing Table
            // ═══════════════════════════════════════════════════════════

            var routingTable = new Dictionary<string, string>
            {
                { "192.168.1.0/24", "Gateway1" },
                { "192.168.2.0/24", "Gateway2" },
                { "10.0.0.0/8", "Gateway3" },
                { "172.16.0.0/12", "Gateway4" },
                { "0.0.0.0/0", "DefaultGateway" }
            };

            string ipToRoute = "192.168.1.50";
            string route = FindRoute(routingTable, ipToRoute);

            Console.WriteLine($"\n=== Routing Table ===");
            Console.WriteLine($"IP {ipToRoute} -> {route}");
            // Output: IP 192.168.1.50 -> Gateway1

            ipToRoute = "8.8.8.8";
            route = FindRoute(routingTable, ipToRoute);
            Console.WriteLine($"IP {ipToRoute} -> {route}");
            // Output: IP 8.8.8.8 -> Gateway3 (10.0.0.0/8 matches)

            ipToRoute = "172.16.50.100";
            route = FindRoute(routingTable, ipToRoute);
            Console.WriteLine($"IP {ipToRoute} -> {route}");
            // Output: IP 172.16.50.100 -> Gateway4

            // ═══════════════════════════════════════════════════════════
            // Example 6: Tag/Category System
            // ═══════════════════════════════════════════════════════════

            var articles = new List<Article>
            {
                new Article { Title = "Getting Started with C#", Tags = new List<string> { "C#", "Beginner", "Programming" } },
                new Article { Title = "Advanced LINQ", Tags = new List<string> { "C#", "LINQ", "Advanced" } },
                new Article { Title = "Python Basics", Tags = new List<string> { "Python", "Beginner" } },
                new Article { Title = "Web Development", Tags = new List<string> { "Web", "HTML", "CSS" } }
            };

            // Reverse index: tag -> articles
            var tagIndex = new Dictionary<string, List<Article>>();

            foreach (var article in articles)
            {
                foreach (var tag in article.Tags)
                {
                    if (!tagIndex.ContainsKey(tag))
                    {
                        tagIndex[tag] = new List<Article>();
                    }
                    tagIndex[tag].Add(article);
                }
            }

            Console.WriteLine("\n=== Articles by Tag ===");
            string searchTag = "C#";
            if (tagIndex.TryGetValue(searchTag, out var taggedArticles))
            {
                Console.WriteLine($"Tag '{searchTag}':");
                foreach (var a in taggedArticles)
                {
                    Console.WriteLine($"  - {a.Title}");
                }
            }
            // Output:
            //   Tag 'C#':
            //     - Getting Started with C#
            //     - Advanced LINQ

            // List all tags
            Console.WriteLine($"\nAll tags: {string.Join(", ", tagIndex.Keys)}");
            // Output: All tags: C#, Beginner, Programming, LINQ, Advanced, Python, Web, HTML, CSS

            // ═══════════════════════════════════════════════════════════
            // Example 7: Metric/Aggregate Tracking
            // ═══════════════════════════════════════════════════════════

            var metrics = new Dictionary<string, MetricTracker>();

            // Simulate tracking various metrics
            RecordMetric(metrics, "api_calls", 1);
            RecordMetric(metrics, "api_calls", 1);
            RecordMetric(metrics, "api_calls", 1);
            RecordMetric(metrics, "response_time_ms", 45);
            RecordMetric(metrics, "response_time_ms", 32);
            RecordMetric(metrics, "response_time_ms", 67);
            RecordMetric(metrics, "errors", 1);

            Console.WriteLine("\n=== System Metrics ===");
            foreach (var metric in metrics.Values)
            {
                Console.WriteLine($"  {metric.Name}:");
                Console.WriteLine($"    Count: {metric.Count}, Sum: {metric.Sum}, Avg: {metric.Average:F2}");
            }
            // Output:
            //   api_calls: Count: 3, Sum: 3, Avg: 1.00
            //   response_time_ms: Count: 3, Sum: 144, Avg: 48.00
            //   errors: Count: 1, Sum: 1, Avg: 1.00

            // ═══════════════════════════════════════════════════════════
            // Example 8: Command Pattern Handler
            // ═══════════════════════════════════════════════════════════

            var commands = new Dictionary<string, Action<string>>
            {
                { "help", ShowHelp },
                { "version", ShowVersion },
                { "exit", ExitApp },
                { "clear", ClearScreen }
            };

            Console.WriteLine("\n=== Command Handler ===");

            // Simulate command execution
            string command = "version";
            if (commands.TryGetValue(command, out Action<string> handler))
            {
                Console.WriteLine($"Executing: {command}");
                handler("");
            }
            // Output:
            //   Executing: version
            //   Version: 1.0.0

            // Check if command exists
            command = "unknown";
            Console.WriteLine($"Command '{command}' exists: {commands.ContainsKey(command)}");
            // Output: Command 'unknown' exists: False

            Console.WriteLine("\n=== Dictionary More Real-World Examples Complete ===");
        }

        // Helper methods
        static Dictionary<string, string> ParseQueryString(string url)
        {
            var result = new Dictionary<string, string>();
            int queryStart = url.IndexOf('?');
            if (queryStart == -1) return result;

            string query = url.Substring(queryStart + 1);
            string[] pairs = query.Split('&');

            foreach (var pair in pairs)
            {
                string[] keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    result[keyValue[0]] = keyValue[1];
                }
            }

            return result;
        }

        static bool CanTransition(Dictionary<string, List<string>> transitions, string from, string to)
        {
            if (!transitions.TryGetValue(from, out var allowedTransitions))
                return false;
            return allowedTransitions.Contains(to);
        }

        static string GetTranslation(Dictionary<string, Dictionary<string, string>> translations, string lang, string key)
        {
            if (translations.TryGetValue(lang, out var langDict) && langDict.TryGetValue(key, out var value))
                return value;
            return key;
        }

        static string FindRoute(Dictionary<string, string> routingTable, string ip)
        {
            foreach (var route in routingTable)
            {
                if (ip.StartsWith(route.Key.Split('/')[0].Substring(0, route.Key.IndexOf('/'))))
                {
                    return route.Value;
                }
            }
            return routingTable["0.0.0.0/0"];
        }

        static void RecordMetric(Dictionary<string, MetricTracker> metrics, string name, double value)
        {
            if (!metrics.ContainsKey(name))
            {
                metrics[name] = new MetricTracker { Name = name };
            }
            metrics[name].Add(value);
        }

        static void ShowHelp(string args) => Console.WriteLine("Available commands: help, version, exit, clear");
        static void ShowVersion(string args) => Console.WriteLine("Version: 1.0.0");
        static void ExitApp(string args) => Console.WriteLine("Exiting...");
        static void ClearScreen(string args) => Console.Clear();
    }

    class Sale
    {
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
    }

    class SalesSummary
    {
        public string Product { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
    }

    class Article
    {
        public string Title { get; set; }
        public List<string> Tags { get; set; }
    }

    class MetricTracker
    {
        public string Name { get; set; }
        public int Count { get; private set; }
        public double Sum { get; private set; }

        public double Average => Count > 0 ? Sum / Count : 0;

        public void Add(double value)
        {
            Count++;
            Sum += value;
        }
    }
}
