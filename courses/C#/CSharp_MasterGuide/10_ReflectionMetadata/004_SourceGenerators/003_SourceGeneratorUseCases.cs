/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Source Generators - Common Use Cases
 * FILE      : 03_SourceGeneratorUseCases.cs
 * PURPOSE   : Explores common practical uses of Source Generators
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._10_ReflectionMetadata._04_SourceGenerators
{
    /// <summary>
    /// Demonstrates practical Source Generator use cases
    /// </summary>
    public class SourceGeneratorUseCases
    {
        /// <summary>
        /// Entry point for use cases demonstration
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Source Generator Use Cases ===
            Console.WriteLine("=== Source Generator Use Cases ===\n");

            // ── USE CASE 1: Dependency Injection ─────────────────────────────
            // Auto-register services from attributes

            // Example 1: DI Container Generation
            // Output: 1. Dependency Injection:
            Console.WriteLine("1. Dependency Injection:");
            
            // [Singleton] attribute triggers registration
            // Output: - [Singleton] generates: services.AddSingleton<T>()
            Console.WriteLine("   - [Singleton] -> AddSingleton");
            
            // [Transient] attribute
            // Output: - [Transient] generates: services.AddTransient<T>()
            Console.WriteLine("   - [Transient] -> AddTransient");
            
            // [Scoped] attribute
            // Output: - [Scoped] generates: services.AddScoped<T>()
            Console.WriteLine("   - [Scoped] -> AddScoped");
            
            // Demo the generated registration
            var diDemo = new DIGeneratorDemo();
            diDemo.RegisterServices();
            // Output: Generated: Singleton, Transient, Scoped
            Console.WriteLine("   Generated: Singleton, Transient, Scoped");

            // ── USE CASE 2: JSON Serialization ────────────────────────────────
            // Generate serialization code at compile time

            // Example 2: JSON Serialization
            // Output: 2. JSON Serialization:
            Console.WriteLine("\n2. JSON Serialization:");
            
            // System.Text.Json source generator
            // Output: - System.Text.Json generates JsonSerializer code
            Console.WriteLine("   - System.Text.Json generates code");
            
            // JsonSerializable attribute triggers generation
            // Output: - [JsonSerializable] generates serializer
            Console.WriteLine("   - [JsonSerializable] generates serializer");
            
            // Benefits over reflection-based serialization
            // Output: - Faster than reflection-based serializers
            Console.WriteLine("   - Faster than reflection");
            // Output: - No reflection Emit required
            Console.WriteLine("   - No Emit needed");
            // Output: - Trimming-safe for AOT
            Console.WriteLine("   - Trimming-safe for AOT");

            // ── USE CASE 3: Record Enhancements ───────────────────────────────
            // Auto-generate equality, cloning, with expressions

            // Example 3: Record Support
            // Output: 3. Record Enhancements:
            Console.WriteLine("\n3. Record Enhancements:");
            
            // Records get Equals, GetHashCode, == automatically
            // Output: - Records: Equals, GetHashCode, ==, !=
            Console.WriteLine("   - Records: Equals, GetHashCode");
            
            // With expressions for immutable updates
            // Output: - With expressions: record with new values
            Console.WriteLine("   - With expressions for updates");
            
            // Deconstruct method
            // Output: - Deconstruct method auto-generated
            Console.WriteLine("   - Deconstruct method generated");
            
            // Demo
            var original = new GeneratedRecord("John", 30);
            var modified = original with { Age = 31 };
            // Output: Original: John-30, Modified: John-31
            Console.WriteLine($"   Original: {original.Name}-{original.Age}");
            Console.WriteLine($"   Modified: {modified.Name}-{modified.Age}");

            // ── USE CASE 4: Mock Generation ───────────────────────────────────
            // Generate mock implementations for testing

            // Example 4: Mock Generation
            // Output: 4. Mock Generation:
            Console.WriteLine("\n4. Mock Generation:");
            
            // Moq, NSubstitute use source generators
            // Output: - Moq/NSubstitute generate mocks at compile time
            Console.WriteLine("   - Moq/NSubstitute generate mocks");
            
            // No runtime Emit or Assembly.Load
            // Output: - No runtime code generation
            Console.WriteLine("   - No runtime code generation");
            
            // Full type information available
            // Output: - Full type info for mocking
            Console.WriteLine("   - Full type info available");

            // ── USE CASE 5: API Client Generation ─────────────────────────────
            // Generate HTTP clients from OpenAPI specs

            // Example 5: API Client Generation
            // Output: 5. API Client Generation:
            Console.WriteLine("\n5. API Client Generation:");
            
            // Refit, RestSharp generate from specs
            // Output: - Refit generates strongly-typed clients
            Console.WriteLine("   - Refit generates typed clients");
            
            // HTTP methods from attributes
            // Output: - [Get("/api/users")] generates HTTP call
            Console.WriteLine("   - [Get] attribute -> HTTP call");
            
            // Response mapping automatically
            // Output: - Response mapping automatic
            Console.WriteLine("   - Response mapping automatic");
            
            // Demo
            var apiClient = new ApiClientDemo();
            var result = apiClient.GetUser(1);
            // Output: API call: GET /users/1 -> User(id=1, name=John)
            Console.WriteLine($"   API call: GET /users/1 -> {result}");

            // ── USE CASE 6: CQRS/Event Sourcing ──────────────────────────────
            // Generate commands, handlers, events

            // Example 6: CQRS Pattern
            // Output: 6. CQRS Pattern:
            Console.WriteLine("\n6. CQRS Pattern:");
            
            // [Command] attribute generates command class
            // Output: - [Command] generates command class
            Console.WriteLine("   - [Command] generates command");
            
            // [Query] attribute generates query class
            // Output: - [Query] generates query class
            Console.WriteLine("   - [Query] generates query");
            
            // Handler registration auto-generated
            // Output: - Handler registration auto-generated
            Console.WriteLine("   - Handler registration auto-generated");
            
            // Demo command generation
            var command = new CreateUserCommand { UserName = "Alice" };
            // Output: Generated: CreateUserCommand(UserName=Alice)
            Console.WriteLine($"   Generated: {command}");

            Console.WriteLine("\n=== Source Generator Use Cases Complete ===");
        }
    }

    /// <summary>
    /// Demonstrates DI registration that would be generated
    /// </summary>
    public class DIGeneratorDemo
    {
        /// <summary>
        /// Simulates generated service registration
        /// </summary>
        public void RegisterServices()
        {
            // This would be generated code like:
            // services.AddSingleton<ISingletonService, SingletonService>();
            // services.AddTransient<ITransientService, TransientService>();
            // services.AddScoped<IScopedService, ScopedService>();
            Console.WriteLine("   services.AddSingleton<IService, Service>()");
            Console.WriteLine("   services.AddTransient<IService, Service>()");
            Console.WriteLine("   services.AddScoped<IService, Service>()");
        }
    }

    /// <summary>
    /// Demonstrates generated record functionality
    /// </summary>
    public record GeneratedRecord(string Name, int Age);

    /// <summary>
    /// Demonstrates generated API client
    /// </summary>
    public class ApiClientDemo
    {
        /// <summary>
        /// Simulates generated GET method
        /// </summary>
        public string GetUser(int id)
        {
            // Would be generated as: return await _httpClient.GetAsync($"/users/{id}");
            return $"User(id={id}, name=John)";
        }
    }

    /// <summary>
    /// Demonstrates generated command
    /// </summary>
    public class CreateUserCommand
    {
        public string UserName { get; set; } // property: username for new user
        
        public override string ToString()
        {
            return $"CreateUserCommand(UserName={UserName})";
        }
    }
}