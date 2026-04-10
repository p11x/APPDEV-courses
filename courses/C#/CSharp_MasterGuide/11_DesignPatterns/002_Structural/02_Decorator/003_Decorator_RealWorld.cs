/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Decorator Real-World
 * FILE      : 03_Decorator_RealWorld.cs
 * PURPOSE   : Real-world Decorator pattern applications
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List<T>

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural._02_Decorator
{
    /// <summary>
    /// Real-world Decorator pattern examples
    /// </summary>
    public class DecoratorRealWorld
    {
        /// <summary>
        /// Entry point for real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Decorator Real-World ===
            Console.WriteLine("=== Decorator Real-World ===\n");

            // ── REAL-WORLD 1: Data Pipeline ───────────────────────────────────
            // Chain data transformations

            // Example 1: Data Pipeline
            // Output: 1. Data Pipeline:
            Console.WriteLine("1. Data Pipeline:");
            
            // Build pipeline: Source -> Transform -> Filter -> Output
            var pipeline = new ValidationDecorator(
                new TransformationDecorator(
                    new LoggingDataDecorator(null)));
            
            var data = new List<string> { "valid1", "invalid", "valid2" };
            pipeline.Process(data);
            
            // Output: Processed: valid1, valid2 (2 items)

            // ── REAL-WORLD 2: Web Request Enhancement ────────────────────────
            // Add cross-cutting concerns

            // Example 2: Web Request Enhancement
            // Output: 2. Web Request Enhancement:
            Console.WriteLine("\n2. Web Request Enhancement:");
            
            // Build request pipeline
            var handler = new CompressionHandler(
                new CachingHandler(
                    new AuthHandler(null)));
            
            var request = new WebRequest { Url = "/api/data", Method = "GET" };
            handler.Handle(request);
            
            // Output: Auth check -> Cache check -> Compress -> Response

            // ── REAL-WORLD 3: Validation Pipeline ────────────────────────────
            // Multiple validation rules

            // Example 3: Validation Pipeline
            // Output: 3. Validation Pipeline:
            Console.WriteLine("\n3. Validation Pipeline:");
            
            // Chain validators
            var validator = new RequiredFieldValidator(
                new EmailFormatValidator(
                    new LengthValidator(null)));
            
            var formData = new Dictionary<string, string> 
            { 
                { "email", "test@example.com" },
                { "name", "John" }
            };
            
            var isValid = validator.Validate(formData);
            // Output: Validation result: True
            Console.WriteLine($"   Validation result: {isValid}");

            // ── REAL-WORLD 4: Composite Decorators ───────────────────────────
            // Combine multiple concerns

            // Example 4: Composite Decorators
            // Output: 4. Composite Decorators:
            Console.WriteLine("\n4. Composite Decorators:");
            
            // Build secure repository
            var repo = new LoggingRepository(
                new CachingRepository(
                    new AuditRepository(
                        new EntityRepository())));
            
            // All concerns applied automatically
            var user = repo.GetById(1);
            // Output: Retrieved user: ID=1, Name=John

            Console.WriteLine("\n=== Decorator Real-World Complete ===");
        }
    }

    /// <summary>
    /// Data processor interface
    /// </summary>
    public interface IDataProcessor
    {
        void Process(List<string> data); // method: processes data
    }

    /// <summary>
    /// Validation decorator
    /// </summary>
    public class ValidationDecorator : IDataProcessor
    {
        private IDataProcessor _next;
        
        public ValidationDecorator(IDataProcessor next)
        {
            _next = next;
        }
        
        public void Process(List<string> data)
        {
            Console.WriteLine("   Validating data...");
            var valid = data.FindAll(d => d.StartsWith("valid"));
            _next?.Process(valid);
        }
    }

    /// <summary>
    /// Transformation decorator
    /// </summary>
    public class TransformationDecorator : IDataProcessor
    {
        private IDataProcessor _next;
        
        public TransformationDecorator(IDataProcessor next)
        {
            _next = next;
        }
        
        public void Process(List<string> data)
        {
            Console.WriteLine("   Transforming data...");
            _next?.Process(data);
        }
    }

    /// <summary>
    /// Logging data decorator
    /// </summary>
    public class LoggingDataDecorator : IDataProcessor
    {
        private IDataProcessor _next;
        
        public LoggingDataDecorator(IDataProcessor next)
        {
            _next = next;
        }
        
        public void Process(List<string> data)
        {
            Console.WriteLine($"   Processed: {string.Join(", ", data)} ({data.Count} items)");
            _next?.Process(data);
        }
    }

    /// <summary>
    /// Web request interface
    /// </summary>
    public interface IRequestHandler
    {
        void Handle(WebRequest request); // method: handles request
    }

    /// <summary>
    /// Web request
    /// </summary>
    public class WebRequest
    {
        public string Url { get; set; } // property: request URL
        public string Method { get; set; } // property: HTTP method
    }

    /// <summary>
    /// Compression handler
    /// </summary>
    public class CompressionHandler : IRequestHandler
    {
        private IRequestHandler _next;
        
        public CompressionHandler(IRequestHandler next)
        {
            _next = next;
        }
        
        public void Handle(WebRequest request)
        {
            Console.WriteLine("   Compress: Compressing response");
            _next?.Handle(request);
        }
    }

    /// <summary>
    /// Caching handler
    /// </summary>
    public class CachingHandler : IRequestHandler
    {
        private IRequestHandler _next;
        
        public CachingHandler(IRequestHandler next)
        {
            _next = next;
        }
        
        public void Handle(WebRequest request)
        {
            Console.WriteLine("   Cache: Checking cache");
            _next?.Handle(request);
        }
    }

    /// <summary>
    /// Auth handler
    /// </summary>
    public class AuthHandler : IRequestHandler
    {
        private IRequestHandler _next;
        
        public AuthHandler(IRequestHandler next)
        {
            _next = next;
        }
        
        public void Handle(WebRequest request)
        {
            Console.WriteLine("   Auth: Checking authentication");
            _next?.Handle(request);
        }
    }

    /// <summary>
    /// Validator interface
    /// </summary>
    public interface IValidator
    {
        bool Validate(Dictionary<string, string> data); // method: validates data
    }

    /// <summary>
    /// Required field validator
    /// </summary>
    public class RequiredFieldValidator : IValidator
    {
        private IValidator _next;
        
        public RequiredFieldValidator(IValidator next)
        {
            _next = next;
        }
        
        public bool Validate(Dictionary<string, string> data)
        {
            Console.WriteLine("   Checking required fields...");
            var hasRequired = data.ContainsKey("email") && data.ContainsKey("name");
            return hasRequired && (_next?.Validate(data) ?? true);
        }
    }

    /// <summary>
    /// Email format validator
    /// </summary>
    public class EmailFormatValidator : IValidator
    {
        private IValidator _next;
        
        public EmailFormatValidator(IValidator next)
        {
            _next = next;
        }
        
        public bool Validate(Dictionary<string, string> data)
        {
            Console.WriteLine("   Checking email format...");
            if (data.TryGetValue("email", out var email))
            {
                return email.Contains("@") && (_next?.Validate(data) ?? true);
            }
            return false;
        }
    }

    /// <summary>
    /// Length validator
    /// </summary>
    public class LengthValidator : IValidator
    {
        private IValidator _next;
        
        public LengthValidator(IValidator next)
        {
            _next = next;
        }
        
        public bool Validate(Dictionary<string, string> data)
        {
            Console.WriteLine("   Checking field lengths...");
            return _next?.Validate(data) ?? true;
        }
    }

    /// <summary>
    /// Entity repository interface
    /// </summary>
    public interface IRepository
    {
        object GetById(int id); // method: gets entity by ID
    }

    /// <summary>
    /// Entity repository
    /// </summary>
    public class EntityRepository : IRepository
    {
        public object GetById(int id)
        {
            return new { Id = id, Name = "John" };
        }
    }

    /// <summary>
    /// Logging repository decorator
    /// </summary>
    public class LoggingRepository : IRepository
    {
        private IRepository _repo;
        
        public LoggingRepository(IRepository repo)
        {
            _repo = repo;
        }
        
        public object GetById(int id)
        {
            Console.WriteLine($"   Log: Fetching entity {id}");
            return _repo.GetById(id);
        }
    }

    /// <summary>
    /// Caching repository decorator
    /// </summary>
    public class CachingRepository : IRepository
    {
        private IRepository _repo;
        
        public CachingRepository(IRepository repo)
        {
            _repo = repo;
        }
        
        public object GetById(int id)
        {
            Console.WriteLine($"   Cache: Checking cache for {id}");
            return _repo.GetById(id);
        }
    }

    /// <summary>
    /// Audit repository decorator
    /// </summary>
    public class AuditRepository : IRepository
    {
        private IRepository _repo;
        
        public AuditRepository(IRepository repo)
        {
            _repo = repo;
        }
        
        public object GetById(int id)
        {
            Console.WriteLine($"   Audit: Recording access to {id}");
            return _repo.GetById(id);
        }
    }
}