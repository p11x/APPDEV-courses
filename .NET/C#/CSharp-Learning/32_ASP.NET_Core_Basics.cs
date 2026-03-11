/*
================================================================================
TOPIC 32: ASP.NET CORE BASICS
================================================================================

ASP.NET Core is a modern web framework for building web applications and APIs.

TABLE OF CONTENTS:
1. What is ASP.NET Core?
2. Project Structure
3. Middleware Pipeline
4. Request/Response Cycle
5. Hosting
================================================================================
*/

// Note: This is conceptual code - ASP.NET Core uses Program.cs differently

namespace AspNetCoreConcepts
{
    // ====================================================================
    // PROGRAM.CS (Entry Point)
    // ====================================================================
    
    // Example of minimal API (ASP.NET Core 6+):
    /*
    var builder = WebApplication.CreateBuilder(args);
    var app = builder.Build();
    
    app.MapGet("/", () => "Hello World!");
    app.MapGet("/api/users", () => new[] { "John", "Jane" });
    
    app.Run();
    */
    
    // ====================================================================
    // TRADITIONAL PROGRAM.CS
    // ====================================================================
    
    // Example with dependency injection:
    /*
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services
            builder.Services.AddControllers();
            builder.Services.AddDbContext<AppDbContext>();
            builder.Services.AddScoped<IUserService, UserService>();
            
            var app = builder.Build();
            
            // Middleware pipeline
            app.UseRouting();
            app.MapControllers();
            
            app.Run();
        }
    }
    */
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ASP.NET Core Concepts ===");
            
            // Key concepts
            Console.WriteLine("1. Middleware: Components in request pipeline");
            Console.WriteLine("2. Services: Reusable business logic (DI)");
            Console.WriteLine("3. Routing: URL to controller/action mapping");
            Console.WriteLine("4. Controllers: Handle HTTP requests");
            Console.WriteLine("5. Views: Render HTML (MVC)");
            Console.WriteLine("6. Razor Pages: Page-based model");
        }
    }
}

/*
ASP.NET CORE FEATURES:
---------------------
- Cross-platform (Windows, Linux, macOS)
- High performance
- Modular (pay-for-play)
- Dependency injection built-in
- Unified MVC and Web API
- Real-time with SignalR
- Microservices support
*/

// ================================================================================
// INTERVIEW QUESTIONS
// =============================================================================

/*
Q1: What is middleware in ASP.NET Core?
A: Software components that form a pipeline to handle requests/responses.
   Each middleware can either handle the request or pass to next.

Q2: What is the difference between Use() and Run()?
A: Use() adds middleware to pipeline, allows calling next middleware.
   Run() terminates the pipeline (shortcuts).

Q3: How does dependency injection work in ASP.NET Core?
A: Services are registered in ConfigureServices(), injected via constructor
   or [FromServices] attribute.
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 33 covers Web API development.
*/
