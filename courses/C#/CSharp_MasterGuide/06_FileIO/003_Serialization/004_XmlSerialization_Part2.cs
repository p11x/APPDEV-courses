/*
TOPIC: C# File I/O Operations
SUBTOPIC: Serialization
FILE: 04_XmlSerialization_Part2.cs
PURPOSE: More XML serialization - attributes, namespaces, encoding, mixed content
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CSharp_MasterGuide._06_FileIO._03_Serialization
{
    public class NN_04_XmlSerialization_Part2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== XML Serialization Advanced Demo ===");
            Console.WriteLine();

            XmlNamespacesDemo();
            Console.WriteLine();

            XmlElementAttributesDemo();
            Console.WriteLine();

            MixedContentDemo();
            Console.WriteLine();

            EncodingAndFormatting();
            Console.WriteLine();

            RealWorldExample_SOAPLikeMessage();
            
            CleanupDemoFiles();
        }

        private static void XmlNamespacesDemo()
        {
            Console.WriteLine("--- XML Namespaces Demo ---");
            
            var order = new OrderXml
            {
                OrderId = "ORD-2024-001",
                CustomerName = "Acme Corp",
                Items = new List<OrderItemXml>
                {
                    new OrderItemXml { Product = "Widget A", Quantity = 10, UnitPrice = 9.99m },
                    new OrderItemXml { Product = "Widget B", Quantity = 5, UnitPrice = 19.99m }
                }
            };
            
            XmlSerializer serializer = new XmlSerializer(typeof(OrderXml));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("order", "http://example.com/orders/2024");
            ns.Add("cust", "http://example.com/customers");
            
            string filePath = "NN_order_ns.xml";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, order, ns);
            }
            
            Console.WriteLine("XML with namespaces:");
            Console.WriteLine(File.ReadAllText(filePath));
            
            File.Delete(filePath);
            Console.WriteLine("// Output: Custom XML namespaces added");
        }

        private static void XmlElementAttributesDemo()
        {
            Console.WriteLine("--- XML Element Attributes Demo ---");
            
            var book = new BookXml
            {
                Isbn = "978-0-123456-78-9",
                Title = "The C# Programming Language",
                Author = "John Smith",
                Publisher = "Tech Press",
                Year = 2023,
                Pages = 450,
                Chapters = new List<string>
                {
                    "Introduction", "Basic Concepts", "Advanced Topics", "Best Practices"
                }
            };
            
            XmlSerializer serializer = new XmlSerializer(typeof(BookXml));
            
            string filePath = "NN_book.xml";
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                Encoding = Encoding.UTF8
            };
            
            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                serializer.Serialize(writer, book, ns);
            }
            
            Console.WriteLine("Book XML with attributes:");
            Console.WriteLine(File.ReadAllText(filePath));
            
            File.Delete(filePath);
            Console.WriteLine("// Output: XML element and array attributes applied");
        }

        private static void MixedContentDemo()
        {
            Console.WriteLine("--- Mixed Content Demo ---");
            
            var doc = new XmlDocument();
            doc.LoadXml(@"<Article>
  <Title>Understanding C# Generics</Title>
  <Content>
    <Para>Generics provide a way to create reusable components.</Para>
    <Para>They offer type safety without sacrificing performance.</Para>
    <Code>List&lt;T&gt; list = new List&lt;int&gt;();</Code>
    <Para>This example shows a generic list of integers.</Para>
  </Content>
</Article>");
            
            Console.WriteLine("Mixed content XML:");
            Console.WriteLine(doc.OuterXml);
            Console.WriteLine();
            
            XmlNodeList paragraphs = doc.SelectNodes("//Para");
            Console.WriteLine($"Found {paragraphs?.Count ?? 0} paragraphs");
            
            XmlNode? codeNode = doc.SelectSingleNode("//Code");
            Console.WriteLine($"Code section: {codeNode?.InnerText}");
            
            Console.WriteLine("// Output: XML with mixed text and elements");
        }

        private static void EncodingAndFormatting()
        {
            Console.WriteLine("--- Encoding and Formatting ---");
            
            var settings = new ConfigXml
            {
                AppName = "MyApp",
                Version = "1.0.0",
                Debug = true,
                ConnectionStrings = new Dictionary<string, string>
                {
                    { "main", "Server=localhost;Database=mydb" },
                    { "backup", "Server=backup;Database=mydb" }
                }
            };
            
            XmlSerializer serializer = new XmlSerializer(typeof(ConfigXml));
            
            XmlWriterSettings writerSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    ",
                NewLineOnAttributes = false,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false
            };
            
            string filePath = "NN_config_formatted.xml";
            using (XmlWriter writer = XmlWriter.Create(filePath, writerSettings))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                serializer.Serialize(writer, settings, ns);
            }
            
            Console.WriteLine("Formatted XML:");
            Console.WriteLine(File.ReadAllText(filePath));
            
            Console.WriteLine($"File size: {new FileInfo(filePath).Length} bytes");
            
            File.Delete(filePath);
            Console.WriteLine("// Output: Properly formatted and encoded XML");
        }

        private static void RealWorldExample_SOAPLikeMessage()
        {
            Console.WriteLine();
            Console.WriteLine("=== REAL-WORLD EXAMPLE: SOAP-Like Message ===");
            
            string messagePath = "NN_soap_message.xml";
            
            var envelope = new SoapEnvelope
            {
                Header = new SoapHeader
                {
                    Action = "http://services.example.com/GetCustomer",
                    MessageId = Guid.NewGuid().ToString(),
                    Timestamp = DateTime.UtcNow
                },
                Body = new GetCustomerResponse
                {
                    CustomerId = "CUST-12345",
                    Name = "John Doe",
                    Email = "john@example.com",
                    Phone = "555-1234",
                    Address = new AddressXml
                    {
                        Street = "123 Main St",
                        City = "Anytown",
                        State = "CA",
                        ZipCode = "12345",
                        Country = "USA"
                    },
                    Orders = new List<OrderSummary>
                    {
                        new OrderSummary { OrderId = "ORD-001", Total = 150.00m, Date = new DateTime(2024, 1, 10) },
                        new OrderSummary { OrderId = "ORD-002", Total = 75.50m, Date = new DateTime(2024, 1, 15) }
                    }
                }
            };
            
            XmlSerializer serializer = new XmlSerializer(typeof(SoapEnvelope));
            
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false
            };
            
            using (XmlWriter writer = XmlWriter.Create(messagePath, settings))
            {
                serializer.Serialize(writer, envelope);
            }
            
            Console.WriteLine("SOAP message:");
            Console.WriteLine(File.ReadAllText(messagePath));
            
            var loadedEnvelope = new SoapEnvelope();
            using (StreamReader reader = new StreamReader(messagePath))
            {
                loadedEnvelope = (SoapEnvelope)serializer.Deserialize(reader)!;
            }
            
            Console.WriteLine("Parsed message:");
            Console.WriteLine($"  Action: {loadedEnvelope.Header.Action}");
            Console.WriteLine($"  Customer: {loadedEnvelope.Body.Name}");
            Console.WriteLine($"  Address: {loadedEnvelope.Body.Address.Street}, {loadedEnvelope.Body.Address.City}");
            Console.WriteLine($"  Orders: {loadedEnvelope.Body.Orders.Count}");
            
            File.Delete(messagePath);
            Console.WriteLine("// Output: SOAP-style message serialized/deserialized");
        }

        private static void CleanupDemoFiles()
        {
            string[] files = { "NN_order_ns.xml", "NN_book.xml", "NN_config_formatted.xml", "NN_soap_message.xml" };
            foreach (string f in files)
            {
                if (File.Exists(f)) File.Delete(f);
            }
            Console.WriteLine("[Cleanup] Demo files removed");
        }
    }

    [XmlRoot("Order", Namespace = "http://example.com/orders/2024")]
    public class OrderXml
    {
        [XmlAttribute("id")]
        public string OrderId { get; set; } = "";
        
        [XmlElement("Customer", Namespace = "http://example.com/customers")]
        public string CustomerName { get; set; } = "";
        
        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public List<OrderItemXml> Items { get; set; } = new List<OrderItemXml>();
    }

    public class OrderItemXml
    {
        [XmlElement("Product")]
        public string Product { get; set; } = "";
        
        [XmlElement("Quantity")]
        public int Quantity { get; set; }
        
        [XmlElement("Price")]
        public decimal UnitPrice { get; set; }
    }

    [XmlRoot("Book")]
    public class BookXml
    {
        [XmlAttribute("isbn")]
        public string Isbn { get; set; } = "";
        
        [XmlElement("Title")]
        public string Title { get; set; } = "";
        
        [XmlElement("Author")]
        public string Author { get; set; } = "";
        
        [XmlElement("Publisher")]
        public string Publisher { get; set; } = ""
        
        [XmlElement("Year")]
        public int Year { get; set; }
        
        [XmlElement("Pages")]
        public int Pages { get; set; }
        
        [XmlArray("Chapters")]
        [XmlArrayItem("Chapter")]
        public List<string> Chapters { get; set; } = new List<string>();
    }

    [XmlRoot("Config")]
    public class ConfigXml
    {
        [XmlAttribute("appName")]
        public string AppName { get; set; } = "";
        
        [XmlAttribute("version")]
        public string Version { get; set; } = "";
        
        [XmlElement("Debug")]
        public bool Debug { get; set; }
        
        [XmlArray("ConnectionStrings")]
        [XmlArrayItem("Connection")]
        public Dictionary<string, string> ConnectionStrings { get; set; } = new Dictionary<string, string>();
    }

    [XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class SoapEnvelope
    {
        [XmlElement("Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public SoapHeader? Header { get; set; }
        
        [XmlElement("Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public GetCustomerResponse? Body { get; set; }
    }

    public class SoapHeader
    {
        [XmlElement("Action")]
        public string Action { get; set; } = "";
        
        [XmlElement("MessageID")]
        public string MessageId { get; set; } = "";
        
        [XmlElement("Timestamp")]
        public DateTime Timestamp { get; set; }
    }

    [XmlRoot("GetCustomerResponse")]
    public class GetCustomerResponse
    {
        [XmlElement("CustomerId")]
        public string CustomerId { get; set; } = "";
        
        [XmlElement("Name")]
        public string Name { get; set; } = "";
        
        [XmlElement("Email")]
        public string Email { get; set; } = "";
        
        [XmlElement("Phone")]
        public string Phone { get; set; } = "";
        
        [XmlElement("Address")]
        public AddressXml? Address { get; set; }
        
        [XmlArray("Orders")]
        [XmlArrayItem("Order")]
        public List<OrderSummary> Orders { get; set; } = new List<OrderSummary>();
    }

    public class AddressXml
    {
        [XmlElement("Street")]
        public string Street { get; set; } = "";
        
        [XmlElement("City")]
        public string City { get; set; } = "";
        
        [XmlElement("State")]
        public string State { get; set; } = "";
        
        [XmlElement("ZipCode")]
        public string ZipCode { get; set; } = "";
        
        [XmlElement("Country")]
        public string Country { get; set; } = "";
    }

    public class OrderSummary
    {
        [XmlElement("OrderId")]
        public string OrderId { get; set; } = "";
        
        [XmlElement("Total")]
        public decimal Total { get; set; }
        
        [XmlElement("Date")]
        public DateTime Date { get; set; }
    }
}