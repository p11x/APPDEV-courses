/*
 * ============================================================
 * TOPIC     : Reflection and Metadata
 * SUBTOPIC  : Source Generators - Real-World Implementation
 * FILE      : 04_SourceGenerators_RealWorld.cs
 * PURPOSE   : Shows a complete example of Source Generator patterns
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List<T>

namespace CSharp_MasterGuide._10_ReflectionMetadata._04_SourceGenerators
{
    /// <summary>
    /// Real-world Source Generator pattern demonstrations
    /// </summary>
    public class SourceGeneratorsRealWorld
    {
        /// <summary>
        /// Entry point for real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Source Generators Real-World ===
            Console.WriteLine("=== Source Generators Real-World ===\n");

            // ── REAL-WORLD EXAMPLE 1: ToString Generator ──────────────────────
            // Generate ToString from properties

            // Example 1: Auto-ToString Pattern
            // Output: 1. Auto-ToString Generator:
            Console.WriteLine("1. Auto-ToString Generator:");
            
            // User defines partial class with [AutoToString]
            // Output: [AutoToString] class Product
            Console.WriteLine("   [AutoToString] class Product");
            
            // Generator produces ToString() override
            var product = new AutoToStringProduct
            {
                Id = 1, // property: product ID
                Name = "Laptop", // property: product name
                Price = 999.99 // property: product price
            };
            // Output: ToString: Product { Id=1, Name=Laptop, Price=999.99 }
            Console.WriteLine($"   ToString: {product.ToString()}");

            // ── REAL-WORLD EXAMPLE 2: Property Change Notification ────────────
            // Generate INotifyPropertyChanged implementation

            // Example 2: Property Notification Generator
            // Output: 2. Property Notification Generator:
            Console.WriteLine("\n2. Property Notification Generator:");
            
            // [NotifyPropertyChanged] triggers implementation
            // Output: [NotifyPropertyChanged] class Customer
            Console.WriteLine("   [NotifyPropertyChanged] class Customer");
            
            var customer = new NotifyPropertyChangedCustomer("Alice", "alice@email.com");
            customer.EmailChanged += (s, e) => Console.WriteLine($"   Email changed to: {e.NewValue}");
            
            // Changing property triggers notification
            customer.Email = "newemail@email.com";
            // Output: Email changed to: newemail@email.com
            Console.WriteLine($"   Changed property: Name={customer.Name}, Email={customer.Email}");

            // ── REAL-WORLD EXAMPLE 3: Dependency Injection ───────────────────
            // Auto-register services

            // Example 3: DI Registration Generator
            // Output: 3. DI Registration Generator:
            Console.WriteLine("\n3. DI Registration Generator:");
            
            // [Singleton] on class triggers registration
            // Output: [Singleton] class UserService
            Console.WriteLine("   [Singleton] class UserService");
            // Output: [Transient] class OrderService
            Console.WriteLine("   [Transient] class OrderService");
            
            // Generated registration
            var diGen = new DIGenerationDemo();
            diGen.GenerateRegistration();
            // Output: Services registered in DI container
            Console.WriteLine("   Services registered in DI container");

            // ── REAL-WORLD EXAMPLE 4: Equality Generator ──────────────────────
            // Generate Equals and GetHashCode

            // Example 4: Equality Generator
            // Output: 4. Equality Generator:
            Console.WriteLine("\n4. Equality Generator:");
            
            // [GenerateEquality] triggers implementation
            // Output: [GenerateEquality] class Order
            Console.WriteLine("   [GenerateEquality] class Order");
            
            var order1 = new GenerateEqualityOrder { Id = 1, Customer = "Alice", Amount = 100 };
            var order2 = new GenerateEqualityOrder { Id = 1, Customer = "Alice", Amount = 100 };
            var order3 = new GenerateEqualityOrder { Id = 2, Customer = "Bob", Amount = 200 };
            
            // Output: order1 == order2: True
            Console.WriteLine($"   order1 == order2: {order1.Equals(order2)}");
            // Output: order1 == order3: False
            Console.WriteLine($"   order1 == order3: {order1.Equals(order3)}");
            
            // Hash codes match for equal objects
            // Output: HashCodes equal: True
            Console.WriteLine($"   HashCodes equal: {order1.GetHashCode() == order2.GetHashCode()}");

            // ── REAL-WORLD EXAMPLE 5: Data Mapper Generator ───────────────────
            // Generate mapping between types

            // Example 5: Data Mapper Generator
            // Output: 5. Data Mapper Generator:
            Console.WriteLine("\n5. Data Mapper Generator:");
            
            // [Mapper] triggers mapping code generation
            // Output: [Mapper] generates DTO to Entity mapping
            Console.WriteLine("   [Mapper] generates DTO to Entity mapping");
            
            // User creates DTO and Entity
            var userDto = new UserDto { Name = "John", Email = "john@email.com" };
            
            // Generated mapper converts DTO to Entity
            var mapper = new DataMapperDemo();
            var userEntity = mapper.MapToEntity(userDto);
            // Output: Mapped: UserEntity(Name=John, Email=john@email.com)
            Console.WriteLine($"   Mapped: {userEntity}");

            // ── REAL-WORLD EXAMPLE 6: Logging Generator ──────────────────────
            // Generate logging statements

            // Example 6: Logging Generator
            // Output: 6. Logging Generator:
            Console.WriteLine("\n6. Logging Generator:");
            
            // [Log] triggers method entry/exit logging
            // Output: [Log] generates Enter/Exit logging
            Console.WriteLine("   [Log] generates Enter/Exit logging");
            
            var logger = new LoggingGeneratorDemo();
            logger.ProcessOrder(123);
            // Output: Log generated: Method entered, Parameter: 123, Method exiting
            Console.WriteLine("   Log generated: Method entered, Parameter: 123, Method exiting");

            Console.WriteLine("\n=== Source Generators Real-World Complete ===");
        }
    }

    /// <summary>
    /// Simulates auto-generated ToString
    /// </summary>
    public class AutoToStringProduct
    {
        public int Id { get; set; } // property: product ID
        public string Name { get; set; } // property: product name
        public decimal Price { get; set; } // property: product price
        
        // This would be generated by [AutoToString] attribute
        public override string ToString()
        {
            return $"Product {{ Id={Id}, Name={Name}, Price={Price} }}";
        }
    }

    /// <summary>
    /// Simulates auto-generated property change notification
    /// </summary>
    public class NotifyPropertyChangedCustomer
    {
        public event EventHandler<PropertyChangedEventArgs> EmailChanged; // event: raised when email changes
        
        public string Name { get; set; } // property: customer name
        
        private string _email; // field: backing store for email
        public string Email 
        {
            get => _email; // getter returns email
            set // setter with change notification
            {
                if (_email != value) // check if value changed
                {
                    _email = value; // assign new value
                    EmailChanged?.Invoke(this, new PropertyChangedEventArgs(value)); // raise event
                }
            }
        }
        
        public NotifyPropertyChangedCustomer(string name, string email)
        {
            Name = name; // set name
            _email = email; // set email
        }
    }

    /// <summary>
    /// Event args for property changes
    /// </summary>
    public class PropertyChangedEventArgs : EventArgs
    {
        public string NewValue { get; } // property: new property value
        
        public PropertyChangedEventArgs(string newValue)
        {
            NewValue = newValue; // store new value
        }
    }

    /// <summary>
    /// Simulates DI registration generation
    /// </summary>
    public class DIGenerationDemo
    {
        /// <summary>
        /// Generates service registrations
        /// </summary>
        public void GenerateRegistration()
        {
            // This would be generated:
            // services.AddSingleton<UserService, UserService>();
            // services.AddTransient<OrderService, OrderService>();
            Console.WriteLine("   services.AddSingleton<UserService, UserService>()");
            Console.WriteLine("   services.AddTransient<OrderService, OrderService>()");
        }
    }

    /// <summary>
    /// Simulates auto-generated equality
    /// </summary>
    public class GenerateEqualityOrder
    {
        public int Id { get; set; } // property: order ID
        public string Customer { get; set; } // property: customer name
        public decimal Amount { get; set; } // property: order amount
        
        // Generated Equals method
        public override bool Equals(object obj)
        {
            if (obj is GenerateEqualityOrder other) // pattern match
            {
                return Id == other.Id && Customer == other.Customer && Amount == other.Amount;
            }
            return false;
        }
        
        // Generated GetHashCode
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Customer, Amount);
        }
    }

    /// <summary>
    /// User DTO for mapping
    /// </summary>
    public class UserDto
    {
        public string Name { get; set; } // property: user's name
        public string Email { get; set; } // property: user's email
    }

    /// <summary>
    /// Simulates generated mapper
    /// </summary>
    public class DataMapperDemo
    {
        /// <summary>
        /// Maps DTO to entity (generated code)
        /// </summary>
        public UserEntity MapToEntity(UserDto dto)
        {
            // Generated mapper code:
            // return new UserEntity { Name = dto.Name, Email = dto.Email };
            return new UserEntity { Name = dto.Name, Email = dto.Email };
        }
    }

    /// <summary>
    /// User entity
    /// </summary>
    public class UserEntity
    {
        public string Name { get; set; } // property: user's name
        public string Email { get; set; } // property: user's email
        
        public override string ToString()
        {
            return $"UserEntity(Name={Name}, Email={Email})";
        }
    }

    /// <summary>
    /// Simulates logging generator
    /// </summary>
    public class LoggingGeneratorDemo
    {
        /// <summary>
        /// Process order with generated logging
        /// </summary>
        public void ProcessOrder(int orderId)
        {
            // Generated logging code:
            // _logger.Log("Entering ProcessOrder");
            // _logger.Log($"Parameter: orderId = {orderId}");
            // _logger.Log("Exiting ProcessOrder");
            Console.WriteLine("   [LOG] Entering ProcessOrder()");
            Console.WriteLine($"   [LOG] Parameter: orderId = {orderId}");
        }
    }
}