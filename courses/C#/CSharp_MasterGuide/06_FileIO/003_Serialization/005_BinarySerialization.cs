/*
TOPIC: C# File I/O Operations
SUBTOPIC: Serialization
FILE: 05_BinarySerialization.cs
PURPOSE: BinaryFormatter (legacy but important for understanding)
WARNING: BinaryFormatter is obsolete in .NET 5+ but understanding it is important for legacy code
*/

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace CSharp_MasterGuide._06_FileIO._03_Serialization
{
    [Serializable]
    public class NN_05_BinarySerialization
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Binary Serialization Demo (Legacy) ===");
            Console.WriteLine("WARNING: BinaryFormatter is obsolete - use JSON/XML for new code");
            Console.WriteLine();

            BasicBinarySerialization();
            Console.WriteLine();

            SerializationWithReferences();
            Console.WriteLine();

            CustomSerialization();
            Console.WriteLine();

            VersioningConsiderations();
            Console.WriteLine();

            RealWorldExample_DataTransfer();
            
            CleanupDemoFiles();
        }

        private static void BasicBinarySerialization()
        {
            Console.WriteLine("--- Basic Binary Serialization ---");
            
            var person = new SerializablePerson
            {
                Name = "John Doe",
                Age = 30,
                Email = "john@example.com",
                IsActive = true
            };
            
            string filePath = "NN_person.bin";
            
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream stream = File.Create(filePath))
            {
                formatter.Serialize(stream, person);
            }
            
            Console.WriteLine($"Serialized to binary: {filePath}");
            Console.WriteLine($"File size: {new FileInfo(filePath).Length} bytes");
            
            using (Stream stream = File.OpenRead(filePath))
            {
                var restored = (SerializablePerson)formatter.Deserialize(stream)!;
                Console.WriteLine($"Deserialized: {restored.Name}, Age: {restored.Age}, Email: {restored.Email}");
            }
            
            File.Delete(filePath);
            Console.WriteLine("// Output: Object serialized to binary format");
        }

        private static void SerializationWithReferences()
        {
            Console.WriteLine("--- Serialization with Object References ---");
            
            var company = new SerializableCompany
            {
                Name = "TechCorp",
                Employees = new System.Collections.Generic.List<SerializableEmployee>
                {
                    new SerializableEmployee { Name = "Alice", Department = "Engineering", Salary = 75000 },
                    new SerializableEmployee { Name = "Bob", Department = "Marketing", Salary = 65000 },
                    new SerializableEmployee { Name = "Carol", Department = "Engineering", Salary = 80000 }
                }
            };
            
            string filePath = "NN_company.bin";
            
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream stream = File.Create(filePath))
            {
                formatter.Serialize(stream, company);
            }
            
            Console.WriteLine($"Company serialized with {company.Employees.Count} employees");
            Console.WriteLine($"File size: {new FileInfo(filePath).Length} bytes");
            
            using (Stream stream = File.OpenRead(filePath))
            {
                var restored = (SerializableCompany)formatter.Deserialize(stream)!;
                Console.WriteLine($"Restored: {restored.Name}");
                foreach (var emp in restored.Employees)
                {
                    Console.WriteLine($"  - {emp.Name}: {emp.Department}");
                }
            }
            
            File.Delete(filePath);
            Console.WriteLine("// Output: Complex object graph serialized");
        }

        private static void CustomSerialization()
        {
            Console.WriteLine("--- Custom Serialization (ISerializable) ---");
            
            var point = new CustomPoint(100, 200);
            
            string filePath = "NN_point.bin";
            
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream stream = File.Create(filePath))
            {
                formatter.Serialize(stream, point);
            }
            
            using (Stream stream = File.OpenRead(filePath))
            {
                var restored = (CustomPoint)formatter.Deserialize(stream)!;
                Console.WriteLine($"Deserialized: X={restored.X}, Y={restored.Y}");
            }
            
            File.Delete(filePath);
            Console.WriteLine("// Output: Custom serialization callback used");
        }

        private static void VersioningConsiderations()
        {
            Console.WriteLine("--- Versioning Considerations ---");
            
            var person = new VersionedPerson
            {
                Name = "Test User",
                Age = 25,
                Email = "test@example.com"
            };
            
            string filePath = "NN_versioned.bin";
            
            BinaryFormatter formatter = new BinaryFormatter();
            SurrogateSelector selector = new SurrogateSelector();
            selector.AddSurrogate(typeof(VersionedPerson), new StreamingContext(StreamingContextStates.All), new VersionedPersonSurrogate());
            formatter.SurrogateSelector = selector;
            
            using (Stream stream = File.Create(filePath))
            {
                formatter.Serialize(stream, person);
            }
            
            using (Stream stream = File.OpenRead(filePath))
            {
                var restored = (VersionedPerson)formatter.Deserialize(stream)!;
                Console.WriteLine($"Deserialized with surrogate: {restored.Name}");
            }
            
            File.Delete(filePath);
            Console.WriteLine("// Output: Surrogate handles version differences");
        }

        private static void RealWorldExample_DataTransfer()
        {
            Console.WriteLine();
            Console.WriteLine("=== REAL-WORLD EXAMPLE: Legacy Data Transfer ===");
            Console.WriteLine("Note: This pattern exists in legacy systems - new code should use JSON");
            
            string cachePath = "NN_cache.dat";
            
            var cacheData = new CacheEntry
            {
                Key = "user_session_123",
                Value = "session_data_here",
                CreatedAt = DateTime.Now.AddHours(-1),
                ExpiresAt = DateTime.Now.AddHours(23),
                Metadata = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "user_id", "12345" },
                    { "ip_address", "192.168.1.1" },
                    { "user_agent", "Mozilla/5.0" }
                }
            };
            
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream stream = File.Create(cachePath))
            {
                formatter.Serialize(stream, cacheData);
            }
            
            Console.WriteLine($"Cached data saved: {cacheData.Key}");
            Console.WriteLine($"Size: {new FileInfo(cachePath).Length} bytes");
            
            using (Stream stream = File.OpenRead(cachePath))
            {
                var loaded = (CacheEntry)formatter.Deserialize(stream)!;
                Console.WriteLine($"Loaded: {loaded.Key}");
                Console.WriteLine($"  Created: {loaded.CreatedAt}");
                Console.WriteLine($"  Expires: {loaded.ExpiresAt}");
                Console.WriteLine($"  Valid: {(loaded.ExpiresAt > DateTime.Now ? "Yes" : "No")}");
            }
            
            File.Delete(cachePath);
            Console.WriteLine("// Output: Binary cache data (legacy pattern)");
        }

        private static void CleanupDemoFiles()
        {
            string[] files = { "NN_person.bin", "NN_company.bin", "NN_point.bin", "NN_versioned.bin", "NN_cache.dat" };
            foreach (string f in files)
            {
                if (File.Exists(f)) File.Delete(f);
            }
            Console.WriteLine("[Cleanup] Demo files removed");
        }
    }

    [Serializable]
    public class SerializablePerson
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        
        [NonSerialized]
        private string? _cachedHash;
    }

    [Serializable]
    public class SerializableCompany
    {
        public string Name { get; set; } = "";
        public System.Collections.Generic.List<SerializableEmployee> Employees { get; set; } = new System.Collections.Generic.List<SerializableEmployee>();
    }

    [Serializable]
    public class SerializableEmployee
    {
        public string Name { get; set; } = "";
        public string Department { get; set; } = "";
        public decimal Salary { get; set; }
    }

    [Serializable]
    public class CustomPoint : ISerializable
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        
        public CustomPoint() { X = 0; Y = 0; }
        public CustomPoint(int x, int y) { X = x; Y = y; }
        
        public CustomPoint(SerializationInfo info, StreamingContext context)
        {
            X = info.GetInt32("X_Coordinate");
            Y = info.GetInt32("Y_Coordinate");
        }
        
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("X_Coordinate", X);
            info.AddValue("Y_Coordinate", Y);
        }
    }

    [Serializable]
    public class VersionedPerson
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public string? Email { get; set; }
        public string? NewField { get; set; }
    }

    public class VersionedPersonSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            VersionedPerson person = (VersionedPerson)obj;
            info.AddValue("Name", person.Name);
            info.AddValue("Age", person.Age);
            info.AddValue("Email", person.Email ?? "");
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            VersionedPerson person = (VersionedPerson)obj;
            person.Name = info.GetString("Name") ?? "";
            person.Age = info.GetInt32("Age");
            person.Email = info.GetString("Email");
            return person;
        }
    }

    [Serializable]
    public class CacheEntry
    {
        public string Key { get; set; } = "";
        public string Value { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public System.Collections.Generic.Dictionary<string, string>? Metadata { get; set; }
    }
}