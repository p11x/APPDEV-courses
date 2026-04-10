/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Builder Variations
 * FILE      : 02_BuilderVariations.cs
 * PURPOSE   : Demonstrates different Builder pattern approaches
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational._03_Builder
{
    /// <summary>
    /// Demonstrates Builder variations
    /// </summary>
    public class BuilderVariations
    {
        /// <summary>
        /// Entry point for Builder variations
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Builder Variations ===
            Console.WriteLine("=== Builder Variations ===\n");

            // ── CONCEPT: Static Builder (Inner Builder) ────────────────────────
            // Builder as static inner class

            // Example 1: Static Inner Builder
            // Output: 1. Static Inner Builder:
            Console.WriteLine("1. Static Inner Builder:");
            
            // Use inner builder class
            var person = Person.Create()
                .WithName("John")
                .WithAge(30)
                .WithEmail("john@email.com")
                .Build();
            
            // Output: Person: John, 30, john@email.com
            Console.WriteLine($"   Person: {person.Name}, {person.Age}, {person.Email}");

            // ── CONCEPT: Object Initialization ───────────────────────────────────
            // Simplified builder using object initializer

            // Example 2: Object Initializer
            // Output: 2. Object Initializer:
            Console.WriteLine("\n2. Object Initializer:");
            
            // Use object initializer syntax
            var config = new AppConfig
            {
                Name = "MyApp", // property: app name
                Version = "1.0", // property: version
                Timeout = 30, // property: timeout in seconds
                MaxRetries = 3 // property: max retry attempts
            };
            
            // Output: Config: MyApp v1.0, Timeout: 30s, Retries: 3
            Console.WriteLine($"   Config: {config.Name} v{config.Version}, Timeout: {config.Timeout}s, Retries: {config.MaxRetries}");

            // ── CONCEPT: Step Builder ──────────────────────────────────────────
            // Enforces step order through interface chain

            // Example 3: Step Builder
            // Output: 3. Step Builder:
            Console.WriteLine("\n3. Step Builder:");
            
            // Enforced step order: First -> Then -> Then -> Build
            var user = UserBuilder.Start()
                .WithUsername("johndoe")
                .WithPassword("password123")
                .WithEmail("john@email.com")
                .Build();
            
            // Output: User: johndoe (john@email.com)
            Console.WriteLine($"   User: {user.Username} ({user.Email})");

            // ── CONCEPT: Director with Multiple Builders ─────────────────────────
            // Same director works with different builders

            // Example 4: Director with Multiple Builders
            // Output: 4. Director with Multiple Builders:
            Console.WriteLine("\n4. Director with Multiple Builders:");
            
            // Use same director for different products
            var reportDirector = new ReportDirector();
            
            var pdfBuilder = new PDFReportBuilder();
            var excelBuilder = new ExcelReportBuilder();
            
            var pdfReport = reportDirector.BuildReport(pdfBuilder);
            var excelReport = reportDirector.BuildReport(excelBuilder);
            
            // Output: PDF Report: 10 pages, Chart included
            Console.WriteLine($"   PDF Report: {pdfReport.Pages} pages, Chart: {pdfReport.HasChart}");
            // Output: Excel Report: 5 sheets, PivotTable included
            Console.WriteLine($"   Excel Report: {excelReport.Sheets} sheets, PivotTable: {excelReport.HasPivotTable}");

            // ── REAL-WORLD EXAMPLE: HTTP Request Builder ──────────────────────
            // Output: --- Real-World: HTTP Request Builder ---
            Console.WriteLine("\n--- Real-World: HTTP Request Builder ---");
            
            // Build HTTP requests programmatically
            var request = new HttpRequestBuilder()
                .WithMethod("POST")
                .WithUrl("https://api.example.com/users")
                .WithHeader("Content-Type", "application/json")
                .WithHeader("Authorization", "Bearer token")
                .WithBody("{\"name\":\"John\"}")
                .WithTimeout(5000)
                .Build();
            
            // Output: POST https://api.example.com/users (5000ms timeout)
            Console.WriteLine($"   {request.Method} {request.Url} ({request.Timeout}ms timeout)");
            // Output: Headers: Content-Type, Authorization
            Console.WriteLine($"   Headers: {string.Join(", ", request.Headers.Keys)}");

            Console.WriteLine("\n=== Builder Variations Complete ===");
        }
    }

    /// <summary>
    /// Person with static builder
    /// </summary>
    public class Person
    {
        public string Name { get; set; } // property: person's name
        public int Age { get; set; } // property: person's age
        public string Email { get; set; } // property: person's email
        
        /// <summary>
        /// Creates new builder
        /// </summary>
        public static PersonBuilder Create()
        {
            return new PersonBuilder();
        }
    }

    /// <summary>
    /// Static inner builder
    /// </summary>
    public class PersonBuilder
    {
        private Person _person = new Person(); // new person instance
        
        /// <summary>
        /// Sets name
        /// </summary>
        public PersonBuilder WithName(string name)
        {
            _person.Name = name;
            return this;
        }
        
        /// <summary>
        /// Sets age
        /// </summary>
        public PersonBuilder WithAge(int age)
        {
            _person.Age = age;
            return this;
        }
        
        /// <summary>
        /// Sets email
        /// </summary>
        public PersonBuilder WithEmail(string email)
        {
            _person.Email = email;
            return this;
        }
        
        /// <summary>
        /// Builds person
        /// </summary>
        public Person Build()
        {
            return _person;
        }
    }

    /// <summary>
    /// Configuration class (object initializer example)
    /// </summary>
    public class AppConfig
    {
        public string Name { get; set; } // property: application name
        public string Version { get; set; } // property: version string
        public int Timeout { get; set; } // property: timeout in seconds
        public int MaxRetries { get; set; } // property: max retry count
    }

    /// <summary>
    /// Step builder - enforces order
    /// </summary>
    public class UserBuilder
    {
        private string _username = ""; // stores username
        private string _password = ""; // stores password
        private string _email = ""; // stores email
        
        /// <summary>
        /// Starts builder
        /// </summary>
        public static UserNameStep Start()
        {
            return new UserBuilder().WithUsername;
        }
        
        /// <summary>
        /// Sets username
        /// </summary>
        public UserNameStep WithUsername(string username)
        {
            _username = username;
            return WithPassword;
        }
        
        /// <summary>
        /// Sets password - returns next step
        /// </summary>
        public PasswordStep WithPassword(string password)
        {
            _password = password;
            return WithEmail;
        }
        
        /// <summary>
        /// Sets email - returns next step
        /// </summary>
        public EmailStep WithEmail(string email)
        {
            _email = email;
            return new EmailStep(this);
        }
        
        /// <summary>
        /// Builds user
        /// </summary>
        public User Build()
        {
            return new User { Username = _username, Password = _password, Email = _email };
        }
    }

    // Step interfaces
    public interface UserNameStep { UserNameStep WithUsername(string username); }
    public interface PasswordStep { PasswordStep WithPassword(string password); }
    public interface EmailStep { EmailStep WithEmail(string email); BuildStep Build(); }
    public interface BuildStep { User Build(); }

    /// <summary>
    /// User product
    /// </summary>
    public class User
    {
        public string Username { get; set; } // property: username
        public string Password { get; set; } // property: password
        public string Email { get; set; } // property: email
    }

    /// <summary>
    /// Report product
    /// </summary>
    public class Report
    {
        public int Pages { get; set; } // property: page count
        public bool HasChart { get; set; } // property: has chart flag
        public int Sheets { get; set; } // property: sheet count
        public bool HasPivotTable { get; set; } // property: has pivot table flag
    }

    /// <summary>
    /// Report builder interface
    /// </summary>
    public interface IReportBuilder
    {
        void SetPages(int pages); // method: sets pages
        void SetChart(bool hasChart); // method: sets chart
        void SetSheets(int sheets); // method: sets sheets
        void SetPivotTable(bool hasPivot); // method: sets pivot table
        Report GetResult(); // method: returns built report
    }

    /// <summary>
    /// PDF report builder
    /// </summary>
    public class PDFReportBuilder : IReportBuilder
    {
        private Report _report = new Report();
        
        public void SetPages(int pages) { _report.Pages = pages; }
        public void SetChart(bool hasChart) { _report.HasChart = hasChart; }
        public void SetSheets(int sheets) { }
        public void SetPivotTable(bool hasPivot) { }
        public Report GetResult() => _report;
    }

    /// <summary>
    /// Excel report builder
    /// </summary>
    public class ExcelReportBuilder : IReportBuilder
    {
        private Report _report = new Report();
        
        public void SetPages(int pages) { }
        public void SetChart(bool hasChart) { }
        public void SetSheets(int sheets) { _report.Sheets = sheets; }
        public void SetPivotTable(bool hasPivot) { _report.HasPivotTable = hasPivot; }
        public Report GetResult() => _report;
    }

    /// <summary>
    /// Report director
    /// </summary>
    public class ReportDirector
    {
        /// <summary>
        /// Builds report using builder
        /// </summary>
        public Report BuildReport(IReportBuilder builder)
        {
            builder.SetPages(10);
            builder.SetChart(true);
            builder.SetSheets(5);
            builder.SetPivotTable(true);
            return builder.GetResult();
        }
    }

    /// <summary>
    /// HTTP request product
    /// </summary>
    public class HttpRequest
    {
        public string Method { get; set; } // property: HTTP method
        public string Url { get; set; } // property: request URL
        public System.Collections.Generic.Dictionary<string, string> Headers { get; set; } = new();
        public string Body { get; set; } // property: request body
        public int Timeout { get; set; } // property: timeout in ms
    }

    /// <summary>
    /// HTTP request builder
    /// </summary>
    public class HttpRequestBuilder
    {
        private HttpRequest _request = new HttpRequest();
        
        /// <summary>
        /// Sets HTTP method
        /// </summary>
        public HttpRequestBuilder WithMethod(string method)
        {
            _request.Method = method;
            return this;
        }
        
        /// <summary>
        /// Sets URL
        /// </summary>
        public HttpRequestBuilder WithUrl(string url)
        {
            _request.Url = url;
            return this;
        }
        
        /// <summary>
        /// Adds header
        /// </summary>
        public HttpRequestBuilder WithHeader(string key, string value)
        {
            _request.Headers[key] = value;
            return this;
        }
        
        /// <summary>
        /// Sets body
        /// </summary>
        public HttpRequestBuilder WithBody(string body)
        {
            _request.Body = body;
            return this;
        }
        
        /// <summary>
        /// Sets timeout
        /// </summary>
        public HttpRequestBuilder WithTimeout(int timeout)
        {
            _request.Timeout = timeout;
            return this;
        }
        
        /// <summary>
        /// Builds request
        /// </summary>
        public HttpRequest Build()
        {
            return _request;
        }
    }
}