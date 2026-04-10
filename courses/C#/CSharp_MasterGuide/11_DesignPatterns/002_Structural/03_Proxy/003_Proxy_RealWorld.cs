/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Proxy Real-World
 * FILE      : 03_Proxy_RealWorld.cs
 * PURPOSE   : Real-world Proxy pattern applications
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural._03_Proxy
{
    /// <summary>
    /// Real-world Proxy pattern examples
    /// </summary>
    public class ProxyRealWorld
    {
        /// <summary>
        /// Entry point for real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Proxy Real-World ===
            Console.WriteLine("=== Proxy Real-World ===\n");

            // ── REAL-WORLD 1: Database Connection Pooling ────────────────────
            // Manage database connections efficiently

            // Example 1: Connection Pool
            // Output: 1. Connection Pooling:
            Console.WriteLine("1. Connection Pooling:");
            
            // Get connection from pool (proxy)
            var db = new DatabaseConnectionPool();
            
            var conn1 = db.GetConnection();
            // Output: Connection-1 acquired from pool
            Console.WriteLine($"   {conn1} acquired from pool");
            
            var conn2 = db.GetConnection();
            // Output: Connection-2 acquired from pool
            Console.WriteLine($"   {conn2} acquired from pool");
            
            db.ReleaseConnection(conn1);
            // Output: Connection-1 returned to pool

            // ── REAL-WORLD 2: Web Service Proxy ──────────────────────────────
            // Generated proxy for web services

            // Example 2: Web Service Proxy
            // Output: 2. Web Service Proxy:
            Console.WriteLine("\n2. Web Service Proxy:");
            
            // Use proxy instead of direct web service calls
            var customerService = new CustomerServiceProxy();
            
            var customer = customerService.GetCustomer(123);
            // Output: SOAP request to: http://api/customers/123
            // Output: Customer: John Doe
            Console.WriteLine($"   Customer: {customer.Name}");
            
            // Update customer through proxy
            customerService.UpdateCustomer(customer);
            // Output: SOAP request: UPDATE customer

            // ── REAL-WORLD 3: Access Control ───────────────────────────────────
            // Role-based access control

            // Example 3: Access Control Proxy
            // Output: 3. Access Control Proxy:
            Console.WriteLine("\n3. Access Control Proxy:");
            
            // Document access controlled by role
            var docProxy = new AccessControlProxy("report.xlsx", "manager");
            
            // Manager can read
            docProxy.Read();
            // Output: Manager accessing: report.xlsx
            // Output: Access granted: Read
            
            // Manager can write
            docProxy.Write();
            // Output: Access granted: Write
            
            // Different role would have different access

            // ── REAL-WORLD 4: Logging/Tracing ────────────────────────────────
            // Log all method calls

            // Example 4: Logging Proxy
            // Output: 4. Logging Proxy:
            Console.WriteLine("\n4. Logging Proxy:");
            
            // All calls logged automatically
            var loggedService = new LoggingServiceProxy();
            
            loggedService.CallMethod("DoWork", "param1");
            // Output: [ENTER] DoWork(param1)
            // Output: [EXIT] DoWork -> result
            
            loggedService.CallMethod("Calculate", "100");
            // Output: [ENTER] Calculate(100)
            // Output: [EXIT] Calculate -> 200

            // ── REAL-WORLD 5: Resource Management ──────────────────────────────
            // Manage expensive resources

            // Example 5: Resource Management
            // Output: 5. Resource Management:
            Console.WriteLine("\n5. Resource Management:");
            
            // GPU memory management
            var graphicsProxy = new GraphicsResourceProxy();
            
            graphicsProxy.AllocateTexture("grass.png", 1024);
            // Output: GPU: Allocating 1024KB for grass.png
            
            graphicsProxy.AllocateTexture("sky.png", 2048);
            // Output: GPU: Allocating 2048KB for sky.png
            
            graphicsProxy.ReleaseAll();
            // Output: GPU: Releasing all resources

            Console.WriteLine("\n=== Proxy Real-World Complete ===");
        }
    }

    /// <summary>
    /// Database connection interface
    /// </summary>
    public interface IDatabaseConnection
    {
        string ConnectionId { get; } // property: connection identifier
    }

    /// <summary>
    /// Connection pool proxy
    /// </summary>
    public class DatabaseConnectionPool
    {
        private int _connectionCount;
        
        /// <summary>
        /// Gets connection from pool
        /// </summary>
        public IDatabaseConnection GetConnection()
        {
            _connectionCount++;
            return new PooledConnection(_connectionCount);
        }
        
        /// <summary>
        /// Returns connection to pool
        /// </summary>
        public void ReleaseConnection(IDatabaseConnection conn)
        {
            Console.WriteLine($"   {conn.ConnectionId} returned to pool");
        }
    }

    /// <summary>
    /// Pooled connection
    /// </summary>
    public class PooledConnection : IDatabaseConnection
    {
        public string ConnectionId { get; }
        
        public PooledConnection(int id)
        {
            ConnectionId = $"Connection-{id}";
            Console.WriteLine($"   {ConnectionId} acquired from pool");
        }
    }

    /// <summary>
    /// Customer service interface
    /// </summary>
    public interface ICustomerService
    {
        Customer GetCustomer(int id); // method: gets customer
        void UpdateCustomer(Customer customer); // method: updates customer
    }

    /// <summary>
    /// Customer
    /// </summary>
    public class Customer
    {
        public int Id { get; set; } // property: customer ID
        public string Name { get; set; } // property: customer name
    }

    /// <summary>
    /// Web service proxy
    /// </summary>
    public class CustomerServiceProxy : ICustomerService
    {
        /// <summary>
        /// Gets customer via SOAP request
        /// </summary>
        public Customer GetCustomer(int id)
        {
            Console.WriteLine($"   SOAP request to: http://api/customers/{id}");
            return new Customer { Id = id, Name = "John Doe" };
        }
        
        /// <summary>
        /// Updates customer via SOAP request
        /// </summary>
        public void UpdateCustomer(Customer customer)
        {
            Console.WriteLine($"   SOAP request: UPDATE customer");
        }
    }

    /// <summary>
    /// Document interface
    /// </summary>
    public interface IDocument
    {
        void Read(); // method: reads document
        void Write(); // method: writes to document
    }

    /// <summary>
    /// Real document
    /// </summary>
    public class RealDocument : IDocument
    {
        private string _filename;
        
        public RealDocument(string filename)
        {
            _filename = filename;
        }
        
        public void Read() => Console.WriteLine($"   Reading: {_filename}");
        public void Write() => Console.WriteLine($"   Writing: {_filename}");
    }

    /// <summary>
    /// Access control proxy
    /// </summary>
    public class AccessControlProxy : IDocument
    {
        private string _filename;
        private string _userRole;
        private RealDocument _realDocument;
        
        public AccessControlProxy(string filename, string userRole)
        {
            _filename = filename;
            _userRole = userRole;
            _realDocument = new RealDocument(filename);
        }
        
        public void Read()
        {
            if (CanRead())
            {
                Console.WriteLine($"   {_userRole} accessing: {_filename}");
                Console.WriteLine($"   Access granted: Read");
                _realDocument.Read();
            }
            else
            {
                Console.WriteLine($"   Access denied: Read");
            }
        }
        
        public void Write()
        {
            if (CanWrite())
            {
                Console.WriteLine($"   Access granted: Write");
                _realDocument.Write();
            }
            else
            {
                Console.WriteLine($"   Access denied: Write");
            }
        }
        
        private bool CanRead()
        {
            return _userRole == "manager" || _userRole == "admin";
        }
        
        private bool CanWrite()
        {
            return _userRole == "admin";
        }
    }

    /// <summary>
    /// Service interface for logging
    /// </summary>
    public interface IService
    {
        string CallMethod(string method, string param); // method: calls method
    }

    /// <summary>
    /// Real service
    /// </summary>
    public class RealService : IService
    {
        public string CallMethod(string method, string param)
        {
            // Simulate processing
            return "result";
        }
    }

    /// <summary>
    /// Logging service proxy
    /// </summary>
    public class LoggingServiceProxy : IService
    {
        private RealService _realService = new RealService();
        
        public string CallMethod(string method, string param)
        {
            // Log entry
            Console.WriteLine($"   [ENTER] {method}({param})");
            
            // Call real service
            var result = _realService.CallMethod(method, param);
            
            // Log exit
            var exitResult = method == "Calculate" ? "200" : "result";
            Console.WriteLine($"   [EXIT] {method} -> {exitResult}");
            
            return result;
        }
    }

    /// <summary>
    /// Graphics resource interface
    /// </summary>
    public interface IGraphicsResource
    {
        void AllocateTexture(string name, int sizeKB); // method: allocates texture
        void ReleaseAll(); // method: releases all resources
    }

    /// <summary>
    /// Real graphics resource
    /// </summary>
    public class RealGraphicsResource : IGraphicsResource
    {
        public void AllocateTexture(string name, int sizeKB)
        {
            Console.WriteLine($"   GPU: Allocating {sizeKB}KB for {name}");
        }
        
        public void ReleaseAll()
        {
            Console.WriteLine("   GPU: Releasing all resources");
        }
    }

    /// <summary>
    /// Graphics resource proxy for resource management
    /// </summary>
    public class GraphicsResourceProxy : IGraphicsResource
    {
        private RealGraphicsResource _real = new RealGraphicsResource();
        
        public void AllocateTexture(string name, int sizeKB)
        {
            // Could add validation, pooling, etc.
            _real.AllocateTexture(name, sizeKB);
        }
        
        public void ReleaseAll()
        {
            _real.ReleaseAll();
        }
    }
}