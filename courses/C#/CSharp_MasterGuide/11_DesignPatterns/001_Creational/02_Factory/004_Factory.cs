/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Factory Method Extended
 * FILE      : 03_Factory.cs
 * PURPOSE   : Extended Factory Method pattern with variations
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational._02_Factory
{
    /// <summary>
    /// Demonstrates Factory Method extensions and variations
    /// </summary>
    public class FactoryMethodExtended
    {
        /// <summary>
        /// Entry point for Factory Method examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Factory Method Extended ===
            Console.WriteLine("=== Factory Method Extended ===\n");

            // ── CONCEPT: Static Factory Method ─────────────────────────────────
            // Factory method is static - no instantiation needed

            // Example 1: Static Factory
            // Output: 1. Static Factory:
            Console.WriteLine("1. Static Factory:");
            
            // FromString is static - creates instance without new
            var point1 = PointFactory.FromPolar(5, 0.5);
            var point2 = PointFactory.FromCartesian(3, 4);
            
            // Output: Polar: (5, 0.5), Cartesian: (3, 4)
            Console.WriteLine($"   Polar: ({point1.X:F1}, {point1.Y:F1}), Cartesian: ({point2.X}, {point2.Y})");

            // Example 2: Multiple Factory Methods
            // Output: 2. Multiple Factory Methods:
            Console.WriteLine("\n2. Multiple Factory Methods:");
            
            // Different factory methods for different scenarios
            var emptyList = ListFactory.CreateEmpty<string>();
            var filledList = ListFactory.CreateFilled(3, "Item");
            
            // Output: Empty: 0, Filled: 3
            Console.WriteLine($"   Empty: {emptyList.Count}, Filled: {filledList.Count}");

            // ── CONCEPT: Generic Factory ───────────────────────────────────────
            // Factory works with any type via generics

            // Example 3: Generic Factory
            // Output: 3. Generic Factory:
            Console.WriteLine("\n3. Generic Factory:");
            
            // Create<T> uses generic type parameter
            var intValue = GenericFactory.Create<int>(100);
            var stringValue = GenericFactory.Create<string>("Hello");
            var dateValue = GenericFactory.Create<DateTime>(DateTime.Now);
            
            // Output: Int: 100, String: Hello, Date: 2024-01-01
            Console.WriteLine($"   Int: {intValue}, String: {stringValue}, Date: {dateValue:yyyy-MM-dd}");

            // ── CONCEPT: Factory Registry ───────────────────────────────────────
            // Register and retrieve factories dynamically

            // Example 4: Factory Registry
            // Output: 4. Factory Registry:
            Console.WriteLine("\n4. Factory Registry:");
            
            // Register factory by key
            VehicleRegistry.Register("Car", () => new Car());
            VehicleRegistry.Register("Truck", () => new Truck());
            VehicleRegistry.Register("Motorcycle", () => new Motorcycle());
            
            // Create by registered key
            var car = VehicleRegistry.Create("Car");
            var truck = VehicleRegistry.Create("Truck");
            
            // Output: Car: Drives, Truck: Hauls
            Console.WriteLine($"   Car: {car.Drive()}, Truck: {truck.Drive()}");

            // ── REAL-WORLD EXAMPLE: Notification Factory ───────────────────────
            // Output: --- Real-World: Notification System ---
            Console.WriteLine("\n--- Real-World: Notification System ---");
            
            // Email notification
            var emailFactory = new EmailNotificationFactory();
            var email = emailFactory.CreateNotification();
            // Output: Email: Sent via SMTP
            Console.WriteLine($"   Email: {email.Send("Hello")}");
            
            // SMS notification
            var smsFactory = new SMSNotificationFactory();
            var sms = smsFactory.CreateNotification();
            // Output: SMS: Sent via Gateway
            Console.WriteLine($"   SMS: {sms.Send("Hello")}");
            
            // Push notification
            var pushFactory = new PushNotificationFactory();
            var push = pushFactory.CreateNotification();
            // Output: Push: Sent to device
            Console.WriteLine($"   Push: {push.Send("Hello")}");

            Console.WriteLine("\n=== Factory Method Extended Complete ===");
        }
    }

    /// <summary>
    /// Point class with static factory methods
    /// </summary>
    public class Point
    {
        // Internal constructor - use factories
        private Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        
        // Public properties
        public double X { get; }
        public double Y { get; }
    }

    /// <summary>
    /// Static factory for Point
    /// </summary>
    public class PointFactory
    {
        /// <summary>
        /// Creates point from polar coordinates
        /// </summary>
        /// <param name="radius">Distance from origin</param>
        /// <param name="angle">Angle in radians</param>
        /// <returns>Point instance</returns>
        public static Point FromPolar(double radius, double angle)
        {
            // Convert polar to cartesian
            var x = radius * Math.Cos(angle);
            var y = radius * Math.Sin(angle);
            return new Point(x, y);
        }
        
        /// <summary>
        /// Creates point from cartesian coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Point instance</returns>
        public static Point FromCartesian(double x, double y)
        {
            return new Point(x, y);
        }
    }

    /// <summary>
    /// Generic list factory
    /// </summary>
    public static class ListFactory
    {
        /// <summary>
        /// Creates empty list
        /// </summary>
        /// <typeparam name="T">List element type</typeparam>
        /// <returns>Empty list</returns>
        public static List<T> CreateEmpty<T>()
        {
            return new List<T>();
        }
        
        /// <summary>
        /// Creates list filled with value
        /// </summary>
        /// <typeparam name="T">List element type</typeparam>
        /// <param name="count">Number of elements</param>
        /// <param name="value">Value to fill</param>
        /// <returns>Filled list</returns>
        public static List<T> CreateFilled<T>(int count, T value)
        {
            var list = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(value);
            }
            return list;
        }
    }

    /// <summary>
    /// Generic factory for any type
    /// </summary>
    public static class GenericFactory
    {
        /// <summary>
        /// Creates instance of type with parameter
        /// </summary>
        /// <typeparam name="T">Type to create</typeparam>
        /// <param name="value">Initial value</param>
        /// <returns>New instance</returns>
        public static T Create<T>(T value)
        {
            return value;
        }
    }

    /// <summary>
    /// Vehicle interface
    /// </summary>
    public interface IVehicle
    {
        string Drive();
    }

    /// <summary>
    /// Car implementation
    /// </summary>
    public class Car : IVehicle
    {
        /// <summary>
        /// Drives the car
        /// </summary>
        /// <returns>Drive action</returns>
        public string Drive() => "Drives";
    }

    /// <summary>
    /// Truck implementation
    /// </summary>
    public class Truck : IVehicle
    {
        /// <summary>
        /// Drives the truck
        /// </summary>
        /// <returns>Drive action</returns>
        public string Drive() => "Hauls";
    }

    /// <summary>
    /// Motorcycle implementation
    /// </summary>
    public class Motorcycle : IVehicle
    {
        /// <summary>
        /// Drives the motorcycle
        /// </summary>
        /// <returns>Drive action</returns>
        public string Drive() => "Rides";
    }

    /// <summary>
    /// Factory registry for dynamic creation
    /// </summary>
    public static class VehicleRegistry
    {
        // Dictionary stores factory functions
        private static readonly Dictionary<string, Func<IVehicle>> _factories = 
            new Dictionary<string, Func<IVehicle>>();
        
        /// <summary>
        /// Registers a vehicle factory
        /// </summary>
        /// <param name="key">Vehicle type key</param>
        /// <param name="factory">Factory function</param>
        public static void Register(string key, Func<IVehicle> factory)
        {
            _factories[key] = factory;
        }
        
        /// <summary>
        /// Creates vehicle by registered key
        /// </summary>
        /// <param name="key">Vehicle type</param>
        /// <returns>IVehicle instance</returns>
        public static IVehicle Create(string key)
        {
            return _factories[key]();
        }
    }

    /// <summary>
    /// Notification interface
    /// </summary>
    public interface INotification
    {
        string Send(string message);
    }

    /// <summary>
    /// Email notification
    /// </summary>
    public class EmailNotification : INotification
    {
        /// <summary>
        /// Sends email notification
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <returns>Status</returns>
        public string Send(string message) => "Sent via SMTP";
    }

    /// <summary>
    /// SMS notification
    /// </summary>
    public class SMSNotification : INotification
    {
        /// <summary>
        /// Sends SMS notification
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <returns>Status</returns>
        public string Send(string message) => "Sent via Gateway";
    }

    /// <summary>
    /// Push notification
    /// </summary>
    public class PushNotification : INotification
    {
        /// <summary>
        /// Sends push notification
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <returns>Status</returns>
        public string Send(string message) => "Sent to device";
    }

    /// <summary>
    /// Email notification factory
    /// </summary>
    public class EmailNotificationFactory
    {
        /// <summary>
        /// Creates email notification
        /// </summary>
        /// <returns>INotification</returns>
        public INotification CreateNotification() => new EmailNotification();
    }

    /// <summary>
    /// SMS notification factory
    /// </summary>
    public class SMSNotificationFactory
    {
        /// <summary>
        /// Creates SMS notification
        /// </summary>
        /// <returns>INotification</returns>
        public INotification CreateNotification() => new SMSNotification();
    }

    /// <summary>
    /// Push notification factory
    /// </summary>
    public class PushNotificationFactory
    {
        /// <summary>
        /// Creates push notification
        /// </summary>
        /// <returns>INotification</returns>
        public INotification CreateNotification() => new PushNotification();
    }
}