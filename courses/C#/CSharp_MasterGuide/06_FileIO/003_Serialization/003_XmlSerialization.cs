/*
TOPIC: C# File I/O Operations
SUBTOPIC: Serialization
FILE: 03_XmlSerialization.cs
PURPOSE: XML serialization with System.Xml.Serialization
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace CSharp_MasterGuide._06_FileIO._03_Serialization
{
    public class NN_03_XmlSerialization
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== XML Serialization Demo ===");
            Console.WriteLine();

            BasicObjectSerialization();
            Console.WriteLine();

            CollectionSerialization();
            Console.WriteLine();

            DeserializationExamples();
            Console.WriteLine();

            XmlAttributesDemo();
            Console.WriteLine();

            RealWorldExample_EmployeeData();
            
            CleanupDemoFiles();
        }

        private static void BasicObjectSerialization()
        {
            Console.WriteLine("--- Basic Object Serialization ---");
            
            var person = new PersonXml
            {
                Name = "John Doe",
                Age = 30,
                Email = "john@example.com",
                Phone = "555-1234"
            };
            
            XmlSerializer serializer = new XmlSerializer(typeof(PersonXml));
            
            string filePath = "NN_person.xml";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, person);
            }
            
            Console.WriteLine("Serialized XML:");
            Console.WriteLine(File.ReadAllText(filePath));
            
            using (StreamReader reader = new StreamReader(filePath))
            {
                var restored = (PersonXml)serializer.Deserialize(reader)!;
                Console.WriteLine($"Deserialized: {restored.Name}, {restored.Age}, {restored.Email}");
            }
            
            File.Delete(filePath);
            Console.WriteLine("// Output: Object converted to/from XML format");
        }

        private static void CollectionSerialization()
        {
            Console.WriteLine("--- Collection Serialization ---");
            
            var products = new List<ProductXml>
            {
                new ProductXml { Id = 1, Name = "Laptop", Price = 999.99m },
                new ProductXml { Id = 2, Name = "Mouse", Price = 29.99m },
                new ProductXml { Id = 3, Name = "Keyboard", Price = 79.99m }
            };
            
            XmlSerializer serializer = new XmlSerializer(typeof(List<ProductXml>));
            
            string filePath = "NN_products.xml";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, products);
            }
            
            Console.WriteLine("Products XML:");
            Console.WriteLine(File.ReadAllText(filePath));
            
            using (StreamReader reader = new StreamReader(filePath))
            {
                var restored = (List<ProductXml>)serializer.Deserialize(reader)!;
                Console.WriteLine("Restored products:");
                foreach (var p in restored)
                {
                    Console.WriteLine($"  {p.Id}: {p.Name} - ${p.Price:F2}");
                }
            }
            
            File.Delete(filePath);
            Console.WriteLine("// Output: Collections serialized as XML arrays");
        }

        private static void DeserializationExamples()
        {
            Console.WriteLine("--- Deserialization Examples ---");
            
            string xmlString = @"<?xml version=""1.0"" encoding=""utf-8""?>
<CarXml>
  <Make>Toyota</Make>
  <Model>Camry</Model>
  <Year>2023</Year>
  <Mileage>15000</Mileage>
</CarXml>";
            
            XmlSerializer serializer = new XmlSerializer(typeof(CarXml));
            
            using (StringReader reader = new StringReader(xmlString))
            {
                var car = (CarXml)serializer.Deserialize(reader)!;
                Console.WriteLine($"From string: {car.Year} {car.Make} {car.Model}, {car.Mileage} miles");
            }
            
            string filePath = "NN_car.xml";
            File.WriteAllText(filePath, xmlString);
            
            using (StreamReader reader = new StreamReader(filePath))
            {
                var car = (CarXml)serializer.Deserialize(reader)!;
                Console.WriteLine($"From file: {car.Make} {car.Model}");
            }
            
            File.Delete(filePath);
            Console.WriteLine("// Output: XML string and file parsed into objects");
        }

        private static void XmlAttributesDemo()
        {
            Console.WriteLine("--- XML Attributes Demo ---");
            
            var employee = new EmployeeXml
            {
                Id = 1001,
                Name = "Alice Johnson",
                Department = "Engineering",
                Salary = 75000.50m,
                StartDate = new DateTime(2020, 3, 15),
                IsActive = true
            };
            
            XmlSerializer serializer = new XmlSerializer(typeof(EmployeeXml));
            
            string filePath = "NN_employee.xml";
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, employee, namespaces);
            }
            
            Console.WriteLine("Employee XML with attributes:");
            Console.WriteLine(File.ReadAllText(filePath));
            
            File.Delete(filePath);
            Console.WriteLine("// Output: XML element/attribute annotations applied");
        }

        private static void RealWorldExample_EmployeeData()
        {
            Console.WriteLine();
            Console.WriteLine("=== REAL-WORLD EXAMPLE: Employee Data Export ===");
            
            string dataPath = "NN_employees.xml";
            
            var company = new CompanyXml
            {
                Name = "TechCorp Inc.",
                Founded = new DateTime(2010, 1, 1),
                Employees = new List<EmployeeXml>
                {
                    new EmployeeXml
                    {
                        Id = 1001,
                        Name = "Alice Johnson",
                        Department = "Engineering",
                        Salary = 85000m,
                        StartDate = new DateTime(2020, 3, 15),
                        IsActive = true
                    },
                    new EmployeeXml
                    {
                        Id = 1002,
                        Name = "Bob Smith",
                        Department = "Marketing",
                        Salary = 65000m,
                        StartDate = new DateTime(2019, 7, 22),
                        IsActive = true
                    },
                    new EmployeeXml
                    {
                        Id = 1003,
                        Name = "Carol Williams",
                        Department = "Engineering",
                        Salary = 90000m,
                        StartDate = new DateTime(2018, 11, 5),
                        IsActive = true
                    }
                }
            };
            
            XmlSerializer serializer = new XmlSerializer(typeof(CompanyXml));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            
            using (StreamWriter writer = new StreamWriter(dataPath))
            {
                serializer.Serialize(writer, company, namespaces);
            }
            
            Console.WriteLine("Company XML exported:");
            Console.WriteLine(File.ReadAllText(dataPath));
            
            var loadedCompany = new CompanyXml();
            using (StreamReader reader = new StreamReader(dataPath))
            {
                loadedCompany = (CompanyXml)serializer.Deserialize(reader)!;
            }
            
            Console.WriteLine("Loaded company data:");
            Console.WriteLine($"  Company: {loadedCompany.Name}");
            Console.WriteLine($"  Founded: {loadedCompany.Founded:yyyy-MM-dd}");
            Console.WriteLine($"  Employees: {loadedCompany.Employees.Count}");
            
            decimal totalSalary = 0;
            foreach (var emp in loadedCompany.Employees)
            {
                totalSalary += emp.Salary;
            }
            Console.WriteLine($"  Total salary: ${totalSalary:N2}");
            Console.WriteLine($"  Average salary: ${totalSalary / loadedCompany.Employees.Count:N2}");
            
            File.Delete(dataPath);
            Console.WriteLine("// Output: Company employee data exported to XML");
        }

        private static void CleanupDemoFiles()
        {
            string[] files = { "NN_person.xml", "NN_products.xml", "NN_car.xml", "NN_employee.xml", "NN_employees.xml" };
            foreach (string f in files)
            {
                if (File.Exists(f)) File.Delete(f);
            }
            Console.WriteLine("[Cleanup] Demo files removed");
        }
    }

    [XmlRoot("Person")]
    public class PersonXml
    {
        [XmlElement("Name")]
        public string Name { get; set; } = "";
        
        [XmlElement("Age")]
        public int Age { get; set; }
        
        [XmlElement("Email")]
        public string Email { get; set; } = "";
        
        [XmlAttribute("phone")]
        public string Phone { get; set; } = "";
    }

    public class ProductXml
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        
        [XmlElement("Name")]
        public string Name { get; set; } = "";
        
        [XmlElement("Price")]
        public decimal Price { get; set; }
    }

    public class CarXml
    {
        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; }
        public int Mileage { get; set; }
    }

    [XmlType("Employee")]
    public class EmployeeXml
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        
        [XmlElement("Name")]
        public string Name { get; set; } = "";
        
        [XmlElement("Department")]
        public string Department { get; set; } = "";
        
        [XmlElement("Salary")]
        public decimal Salary { get; set; }
        
        [XmlElement("StartDate")]
        public DateTime StartDate { get; set; }
        
        [XmlElement("IsActive")]
        public bool IsActive { get; set; }
    }

    [XmlRoot("Company")]
    public class CompanyXml
    {
        [XmlElement("Name")]
        public string Name { get; set; } = "";
        
        [XmlElement("Founded")]
        public DateTime Founded { get; set; }
        
        [XmlArray("Employees")]
        [XmlArrayItem("Employee")]
        public List<EmployeeXml> Employees { get; set; } = new List<EmployeeXml>();
    }
}