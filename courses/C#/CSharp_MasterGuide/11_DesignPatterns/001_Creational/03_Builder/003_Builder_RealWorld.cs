/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Builder Real-World
 * FILE      : 03_Builder_RealWorld.cs
 * PURPOSE   : Real-world Builder pattern applications
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List<T>

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational._03_Builder
{
    /// <summary>
    /// Real-world Builder pattern examples
    /// </summary>
    public class BuilderRealWorld
    {
        /// <summary>
        /// Entry point for real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Builder Real-World ===
            Console.WriteLine("=== Builder Real-World ===\n");

            // ── REAL-WORLD 1: Email Builder ───────────────────────────────────
            // Construct complex emails with attachments

            // Example 1: Email Builder
            // Output: 1. Email Builder:
            Console.WriteLine("1. Email Builder:");
            
            // Build email with multiple recipients and attachments
            var email = new EmailBuilder()
                .From("sender@example.com")
                .To("recipient@example.com")
                .CC("manager@example.com")
                .Subject("Project Update")
                .Body("The project is progressing well.")
                .WithAttachment("report.pdf")
                .WithAttachment("data.xlsx")
                .SetPriority(EmailPriority.High)
                .Build();
            
            // Output: Email: sender@example.com -> recipient@example.com, Subject: Project Update
            Console.WriteLine($"   Email: {email.From} -> {email.To}, Subject: {email.Subject}");
            // Output: Attachments: 2
            Console.WriteLine($"   Attachments: {email.Attachments.Count}");

            // ── REAL-WORLD 2: API Response Builder ────────────────────────────
            // Build consistent API responses

            // Example 2: API Response Builder
            // Output: 2. API Response Builder:
            Console.WriteLine("\n2. API Response Builder:");
            
            // Build success response
            var successResponse = new ApiResponseBuilder()
                .Success()
                .WithData(new { Id = 1, Name = "Product" })
                .WithMessage("Data retrieved successfully")
                .WithPagination(1, 10, 100)
                .Build();
            
            // Output: Response: Success, Status: 200
            Console.WriteLine($"   Response: {successResponse.Status}, Status: {successResponse.StatusCode}");
            // Output: Data: { Id = 1, Name = Product }
            Console.WriteLine($"   Data: {successResponse.Data}");
            
            // Build error response
            var errorResponse = new ApiResponseBuilder()
                .Error()
                .WithErrorCode("ERR_001")
                .WithMessage("Invalid request")
                .Build();
            
            // Output: Error: ERR_001 - Invalid request
            Console.WriteLine($"   Error: {errorResponse.ErrorCode} - {errorResponse.Message}");

            // ── REAL-WORLD 3: Test Data Builder ──────────────────────────────
            // Create test objects with defaults

            // Example 3: Test Data Builder
            // Output: 3. Test Data Builder:
            Console.WriteLine("\n3. Test Data Builder:");
            
            // Build test user with defaults
            var testUser = new TestUserBuilder()
                .WithDefaultValues() // apply default values
                .WithEmail("test@test.com") // override specific field
                .Build();
            
            // Output: TestUser: testuser (password: test123)
            Console.WriteLine($"   TestUser: {testUser.Username} (password: {testUser.Password})");
            
            // Build admin user
            var adminUser = new TestUserBuilder()
                .AsAdmin() // preset for admin
                .WithEmail("admin@test.com")
                .Build();
            
            // Output: Admin: admin (role: Admin)
            Console.WriteLine($"   Admin: {adminUser.Username} (role: {adminUser.Role})");

            // ── REAL-WORLD 4: Report Builder ──────────────────────────────────
            // Build complex reports with sections

            // Example 4: Report Builder
            // Output: 4. Report Builder:
            Console.WriteLine("\n4. Report Builder:");
            
            // Build report with multiple sections
            var report = new ReportBuilder()
                .WithTitle("Sales Report")
                .WithHeader("Q1 2024 Sales Analysis")
                .AddSection("Executive Summary", "Sales increased by 20%...")
                .AddSection("Revenue", "$1.2M total revenue")
                .AddSection("Top Products", "Product A: $500K")
                .AddChart("Sales by Region")
                .WithFooter("Generated: " + DateTime.Now)
                .Build();
            
            // Output: Report: Sales Report (3 sections, 1 chart)
            Console.WriteLine($"   Report: {report.Title} ({report.Sections.Count} sections, {report.Charts.Count} chart)");

            // ── REAL-WORLD 5: Domain Object Builder ────────────────────────────
            // Build domain entities with validation

            // Example 5: Order Builder
            // Output: 5. Order Builder:
            Console.WriteLine("\n5. Order Builder:");
            
            // Build order with items
            var order = new OrderBuilder()
                .WithCustomerId(123)
                .WithShippingAddress("123 Main St", "City", "State", "12345")
                .AddItem(1, "Product A", 2, 29.99m)
                .AddItem(2, "Product B", 1, 49.99m)
                .WithPriorityShipping()
                .WithGiftWrap(true)
                .Build();
            
            // Output: Order #123, Items: 2, Total: $109.97
            Console.WriteLine($"   Order #{order.OrderId}, Items: {order.Items.Count}, Total: ${order.Total}");
            // Output: Shipping: 123 Main St, City, State 12345
            Console.WriteLine($"   Shipping: {order.ShippingAddress.Street}, {order.ShippingAddress.City}, {order.ShippingAddress.State} {order.ShippingAddress.Zip}");

            Console.WriteLine("\n=== Builder Real-World Complete ===");
        }
    }

    /// <summary>
    /// Email product
    /// </summary>
    public class Email
    {
        public string From { get; set; } // property: sender email
        public string To { get; set; } // property: recipient email
        public string CC { get; set; } // property: CC recipients
        public string Subject { get; set; } // property: email subject
        public string Body { get; set; } // property: email body
        public List<string> Attachments { get; set; } = new();
        public EmailPriority Priority { get; set; } // property: email priority
    }

    public enum EmailPriority { Low, Normal, High }

    /// <summary>
    /// Email builder
    /// </summary>
    public class EmailBuilder
    {
        private Email _email = new Email();
        
        public EmailBuilder From(string from)
        {
            _email.From = from;
            return this;
        }
        
        public EmailBuilder To(string to)
        {
            _email.To = to;
            return this;
        }
        
        public EmailBuilder CC(string cc)
        {
            _email.CC = cc;
            return this;
        }
        
        public EmailBuilder Subject(string subject)
        {
            _email.Subject = subject;
            return this;
        }
        
        public EmailBuilder Body(string body)
        {
            _email.Body = body;
            return this;
        }
        
        public EmailBuilder WithAttachment(string file)
        {
            _email.Attachments.Add(file);
            return this;
        }
        
        public EmailBuilder SetPriority(EmailPriority priority)
        {
            _email.Priority = priority;
            return this;
        }
        
        public Email Build() => _email;
    }

    /// <summary>
    /// API response product
    /// </summary>
    public class ApiResponse
    {
        public bool IsSuccess { get; set; } // property: success flag
        public int StatusCode { get; set; } // property: HTTP status code
        public string Message { get; set; } // property: response message
        public object Data { get; set; } // property: response data
        public string ErrorCode { get; set; } // property: error code
        public int Page { get; set; } // property: current page
        public int PageSize { get; set; } // property: page size
        public int TotalCount { get; set; } // property: total items
    }

    /// <summary>
    /// API response builder
    /// </summary>
    public class ApiResponseBuilder
    {
        private ApiResponse _response = new ApiResponse();
        
        public ApiResponseBuilder Success()
        {
            _response.IsSuccess = true;
            _response.StatusCode = 200;
            return this;
        }
        
        public ApiResponseBuilder Error()
        {
            _response.IsSuccess = false;
            _response.StatusCode = 400;
            return this;
        }
        
        public ApiResponseBuilder WithData(object data)
        {
            _response.Data = data;
            return this;
        }
        
        public ApiResponseBuilder WithMessage(string message)
        {
            _response.Message = message;
            return this;
        }
        
        public ApiResponseBuilder WithErrorCode(string code)
        {
            _response.ErrorCode = code;
            return this;
        }
        
        public ApiResponseBuilder WithPagination(int page, int pageSize, int total)
        {
            _response.Page = page;
            _response.PageSize = pageSize;
            _response.TotalCount = total;
            return this;
        }
        
        public ApiResponse Build() => _response;
    }

    /// <summary>
    /// Test user product
    /// </summary>
    public class TestUser
    {
        public string Username { get; set; } // property: username
        public string Password { get; set; } // property: password
        public string Email { get; set; } // property: email
        public string Role { get; set; } // property: user role
        public bool IsActive { get; set; } // property: active flag
    }

    /// <summary>
    /// Test user builder
    /// </summary>
    public class TestUserBuilder
    {
        private TestUser _user = new TestUser();
        
        public TestUserBuilder WithDefaultValues()
        {
            _user.Username = "testuser";
            _user.Password = "test123";
            _user.Role = "User";
            _user.IsActive = true;
            return this;
        }
        
        public TestUserBuilder AsAdmin()
        {
            _user.Username = "admin";
            _user.Password = "admin123";
            _user.Role = "Admin";
            _user.IsActive = true;
            return this;
        }
        
        public TestUserBuilder WithEmail(string email)
        {
            _user.Email = email;
            return this;
        }
        
        public TestUser Build() => _user;
    }

    /// <summary>
    /// Report product
    /// </summary>
    public class Report
    {
        public string Title { get; set; } // property: report title
        public string Header { get; set; } // property: report header
        public List<string> Sections { get; set; } = new();
        public List<string> Charts { get; set; } = new();
        public string Footer { get; set; } // property: report footer
    }

    /// <summary>
    /// Report builder
    /// </summary>
    public class ReportBuilder
    {
        private Report _report = new Report();
        
        public ReportBuilder WithTitle(string title)
        {
            _report.Title = title;
            return this;
        }
        
        public ReportBuilder WithHeader(string header)
        {
            _report.Header = header;
            return this;
        }
        
        public ReportBuilder AddSection(string title, string content)
        {
            _report.Sections.Add($"{title}: {content}");
            return this;
        }
        
        public ReportBuilder AddChart(string chartType)
        {
            _report.Charts.Add(chartType);
            return this;
        }
        
        public ReportBuilder WithFooter(string footer)
        {
            _report.Footer = footer;
            return this;
        }
        
        public Report Build() => _report;
    }

    /// <summary>
    /// Order product
    /// </summary>
    public class Order
    {
        public int OrderId { get; set; } // property: order ID
        public int CustomerId { get; set; } // property: customer ID
        public List<OrderItem> Items { get; set; } = new();
        public ShippingAddress ShippingAddress { get; set; } // property: shipping address
        public decimal Total { get; set; } // property: order total
        public bool PriorityShipping { get; set; } // property: priority shipping flag
        public bool GiftWrap { get; set; } // property: gift wrap flag
    }

    public class OrderItem
    {
        public int ProductId { get; set; } // property: product ID
        public string Name { get; set; } // property: product name
        public int Quantity { get; set; } // property: quantity
        public decimal Price { get; set; } // property: unit price
    }

    public class ShippingAddress
    {
        public string Street { get; set; } // property: street address
        public string City { get; set; } // property: city
        public string State { get; set; } // property: state
        public string Zip { get; set; } // property: ZIP code
    }

    /// <summary>
    /// Order builder
    /// </summary>
    public class OrderBuilder
    {
        private Order _order = new Order();
        private static int _orderId = 1000;
        
        public OrderBuilder WithCustomerId(int customerId)
        {
            _order.CustomerId = customerId;
            _order.OrderId = _orderId++;
            return this;
        }
        
        public OrderBuilder WithShippingAddress(string street, string city, string state, string zip)
        {
            _order.ShippingAddress = new ShippingAddress 
            { 
                Street = street, 
                City = city, 
                State = state, 
                Zip = zip 
            };
            return this;
        }
        
        public OrderBuilder AddItem(int productId, string name, int quantity, decimal price)
        {
            _order.Items.Add(new OrderItem 
            { 
                ProductId = productId, 
                Name = name, 
                Quantity = quantity, 
                Price = price 
            });
            return this;
        }
        
        public OrderBuilder WithPriorityShipping()
        {
            _order.PriorityShipping = true;
            return this;
        }
        
        public OrderBuilder WithGiftWrap(bool wrap)
        {
            _order.GiftWrap = wrap;
            return this;
        }
        
        public Order Build()
        {
            _order.Total = 0;
            foreach (var item in _order.Items)
            {
                _order.Total += item.Price * item.Quantity;
            }
            return _order;
        }
    }
}