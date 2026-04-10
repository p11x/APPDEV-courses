/*
TOPIC: C# File I/O Operations
SUBTOPIC: Serialization
FILE: 06_CustomSerializer.cs
PURPOSE: Custom serialization implementations for special scenarios
*/

using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace CSharp_MasterGuide._06_FileIO._03_Serialization
{
    public class NN_06_CustomSerializer
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Custom Serializer Demo ===");
            Console.WriteLine();

            CustomJsonConverterDemo();
            Console.WriteLine();

            CustomXmlSerializerDemo();
            Console.WriteLine();

            FlatFileFormatDemo();
            Console.WriteLine();

            CompressionWithSerialization();
            Console.WriteLine();

            RealWorldExample_CustomDataFormat();
            
            CleanupDemoFiles();
        }

        private static void CustomJsonConverterDemo()
        {
            Console.WriteLine("--- Custom JSON Converter ---");
            
            var settings = new SensorData
            {
                SensorId = "TEMP-001",
                ReadingTime = DateTime.Now,
                Temperature = 23.5,
                Humidity = 65.0,
                Pressure = 1013.25
            };
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new SensorDataConverter() }
            };
            
            string json = JsonSerializer.Serialize(settings, options);
            Console.WriteLine("Custom JSON format:");
            Console.WriteLine(json);
            
            var restored = JsonSerializer.Deserialize<SensorData>(json, options);
            Console.WriteLine($"Restored: {restored?.SensorId}, {restored?.Temperature}°C");
            Console.WriteLine("// Output: Custom JSON converter for special types");
        }

        private static void CustomXmlSerializerDemo()
        {
            Console.WriteLine("--- Custom XML Serialization ---");
            
            var book = new Book
            {
                Isbn = "978-0-123456-78-9",
                Title = "Mastering C#",
                Author = "Jane Smith",
                Price = 49.99m,
                Tags = new[] { "programming", "csharp", "tutorial" }
            };
            
            string filePath = "NN_book_custom.xml";
            SaveBookAsCustomXml(book, filePath);
            
            Console.WriteLine("Custom XML format:");
            Console.WriteLine(File.ReadAllText(filePath));
            
            var loaded = LoadBookFromCustomXml(filePath);
            Console.WriteLine($"Loaded: {loaded.Title} by {loaded.Author}");
            
            File.Delete(filePath);
            Console.WriteLine("// Output: Custom XML format with special handling");
        }

        private static void FlatFileFormatDemo()
        {
            Console.WriteLine("--- Flat File Format Serializer ---");
            
            var products = new[]
            {
                new ProductRecord { Id = 1, Name = "Widget A", Price = 19.99m, Stock = 100 },
                new ProductRecord { Id = 2, Name = "Widget B", Price = 29.99m, Stock = 50 },
                new ProductRecord { Id = 3, Name = "Gadget X", Price = 49.99m, Stock = 25 }
            };
            
            string filePath = "NN_products.txt";
            SaveAsFlatFile(products, filePath);
            
            Console.WriteLine("Flat file content:");
            Console.WriteLine(File.ReadAllText(filePath));
            
            var loaded = LoadFromFlatFile<ProductRecord>(filePath);
            Console.WriteLine("Loaded products:");
            foreach (var p in loaded)
            {
                Console.WriteLine($"  {p.Id}: {p.Name} - ${p.Price} ({p.Stock} in stock)");
            }
            
            File.Delete(filePath);
            Console.WriteLine("// Output: Fixed-width or delimited flat file");
        }

        private static void CompressionWithSerialization()
        {
            Console.WriteLine("--- Compression with Serialization ---");
            
            var data = new LargeDataSet
            {
                Id = "DS-001",
                Timestamp = DateTime.Now,
                Records = new System.Collections.Generic.List<DataRecord>()
            };
            
            for (int i = 0; i < 1000; i++)
            {
                data.Records.Add(new DataRecord
                {
                    Index = i,
                    Value = $"Record_{i:D4}",
                    Measurement = i * 1.5
                });
            }
            
            string json = JsonSerializer.Serialize(data);
            Console.WriteLine($"JSON size: {json.Length} bytes");
            
            string compressedPath = "NN_compressed.json";
            CompressToFile(json, compressedPath);
            
            Console.WriteLine($"Compressed file: {new FileInfo(compressedPath).Length} bytes");
            Console.WriteLine($"Compression ratio: {(1 - (double)new FileInfo(compressedPath).Length / json.Length) * 100:F1}%");
            
            string decompressed = DecompressFromFile(compressedPath);
            var loaded = JsonSerializer.Deserialize<LargeDataSet>(decompressed);
            Console.WriteLine($"Decompressed records: {loaded?.Records.Count}");
            
            File.Delete(compressedPath);
            Console.WriteLine("// Output: JSON compressed with GZip");
        }

        private static void RealWorldExample_CustomDataFormat()
        {
            Console.WriteLine();
            Console.WriteLine("=== REAL-WORLD EXAMPLE: Financial Data Export ===");
            
            string exportPath = "NN_financial.dat";
            
            var financialData = new FinancialReport
            {
                ReportId = "RPT-2024-001",
                GeneratedAt = DateTime.Now,
                Currency = "USD",
                Accounts = new System.Collections.Generic.List<AccountSummary>
                {
                    new AccountSummary { AccountId = "ACC-001", Name = "Checking", Balance = 15430.50m, Type = AccountType.Checking },
                    new AccountSummary { AccountId = "ACC-002", Name = "Savings", Balance = 50000.00m, Type = AccountType.Savings },
                    new AccountSummary { AccountId = "ACC-003", Name = "Investment", Balance = 125000.75m, Type = AccountType.Investment }
                },
                Transactions = new System.Collections.Generic.List<Transaction>
                {
                    new Transaction { Date = DateTime.Now.AddDays(-1), Amount = -150.00m, Description = "Grocery Store" },
                    new Transaction { Date = DateTime.Now.AddDays(-2), Amount = 2500.00m, Description = "Salary" },
                    new Transaction { Date = DateTime.Now.AddDays(-3), Amount = -45.99m, Description = "Utilities" }
                }
            };
            
            var customFormat = new FinancialDataFormat();
            customFormat.Save(financialData, exportPath);
            
            Console.WriteLine("Saved financial data:");
            Console.WriteLine(File.ReadAllText(exportPath));
            
            var loaded = customFormat.Load<FinancialReport>(exportPath);
            Console.WriteLine("Loaded report:");
            Console.WriteLine($"  ID: {loaded.ReportId}");
            Console.WriteLine($"  Generated: {loaded.GeneratedAt}");
            Console.WriteLine($"  Accounts: {loaded.Accounts.Count}");
            decimal totalBalance = 0;
            foreach (var acc in loaded.Accounts)
            {
                totalBalance += acc.Balance;
            }
            Console.WriteLine($"  Total balance: ${totalBalance:N2}");
            Console.WriteLine($"  Transactions: {loaded.Transactions.Count}");
            
            File.Delete(exportPath);
            Console.WriteLine("// Output: Custom financial data format");
        }

        private static void SaveBookAsCustomXml(Book book, string filePath)
        {
            XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Book");
                writer.WriteElementString("isbn", book.Isbn);
                writer.WriteElementString("title", book.Title);
                writer.WriteElementString("author", book.Author);
                writer.WriteElementString("price", book.Price.ToString("F2"));
                writer.WriteStartElement("tags");
                foreach (var tag in book.Tags)
                {
                    writer.WriteElementString("tag", tag);
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }

        private static Book LoadBookFromCustomXml(string filePath)
        {
            var book = new Book();
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            
            book.Isbn = doc.SelectSingleNode("//isbn")?.InnerText ?? "";
            book.Title = doc.SelectSingleNode("//title")?.InnerText ?? "";
            book.Author = doc.SelectSingleNode("//author")?.InnerText ?? "";
            book.Price = decimal.Parse(doc.SelectSingleNode("//price")?.InnerText ?? "0");
            
            var tagNodes = doc.SelectNodes("//tag");
            if (tagNodes != null)
            {
                book.Tags = new string[tagNodes.Count];
                for (int i = 0; i < tagNodes.Count; i++)
                {
                    book.Tags[i] = tagNodes[i]!.InnerText;
                }
            }
            
            return book;
        }

        private static void SaveAsFlatFile<T>(T[] items, string filePath) where T : new()
        {
            var props = typeof(T).GetProperties();
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("# " + string.Join("|", Array.ConvertAll(props, p => p.Name)));
                
                foreach (var item in items)
                {
                    var values = Array.ConvertAll(props, p => p.GetValue(item)?.ToString() ?? "");
                    writer.WriteLine(string.Join("|", values));
                }
            }
        }

        private static T[] LoadFromFlatFile<T>(string filePath) where T : new()
        {
            var props = typeof(T).GetProperties();
            var items = new System.Collections.Generic.List<T>();
            
            using (StreamReader reader = new StreamReader(filePath))
            {
                reader.ReadLine();
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split('|');
                    var item = new T();
                    for (int i = 0; i < props.Length && i < values.Length; i++)
                    {
                        var propType = props[i].PropertyType;
                        var value = Convert.ChangeType(values[i], propType);
                        props[i].SetValue(item, value);
                    }
                    items.Add(item);
                }
            }
            
            return items.ToArray();
        }

        private static void CompressToFile(string data, string filePath)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            using (var fileStream = File.Create(filePath))
            using (var gzipStream = new System.IO.Compression.GZipStream(fileStream, System.IO.Compression.CompressionMode.Compress))
            {
                gzipStream.Write(bytes, 0, bytes.Length);
            }
        }

        private static string DecompressFromFile(string filePath)
        {
            using (var fileStream = File.OpenRead(filePath))
            using (var gzipStream = new System.IO.Compression.GZipStream(fileStream, System.IO.Compression.CompressionMode.Decompress))
            using (var reader = new StreamReader(gzipStream))
            {
                return reader.ReadToEnd();
            }
        }

        private static void CleanupDemoFiles()
        {
            string[] files = { "NN_book_custom.xml", "NN_products.txt", "NN_compressed.json", "NN_financial.dat" };
            foreach (string f in files)
            {
                if (File.Exists(f)) File.Delete(f);
            }
            Console.WriteLine("[Cleanup] Demo files removed");
        }
    }

    public class SensorData
    {
        public string SensorId { get; set; } = "";
        public DateTime ReadingTime { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
    }

    public class SensorDataConverter : JsonConverter<SensorData>
    {
        public override SensorData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var data = new SensorData();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string prop = reader.GetString() ?? "";
                    reader.Read();
                    switch (prop)
                    {
                        case "sensor": data.SensorId = reader.GetString() ?? ""; break;
                        case "time": data.ReadingTime = DateTime.Parse(reader.GetString() ?? ""); break;
                        case "temp": data.Temperature = reader.GetDouble(); break;
                        case "humidity": data.Humidity = reader.GetDouble(); break;
                        case "pressure": data.Pressure = reader.GetDouble(); break;
                    }
                }
            }
            return data;
        }

        public override void Write(Utf8JsonWriter writer, SensorData value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("sensor", value.SensorId);
            writer.WriteString("time", value.ReadingTime.ToString("o"));
            writer.WriteNumber("temp", value.Temperature);
            writer.WriteNumber("humidity", value.Humidity);
            writer.WriteNumber("pressure", value.Pressure);
            writer.WriteEndObject();
        }
    }

    public class Book
    {
        public string Isbn { get; set; } = "";
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public decimal Price { get; set; }
        public string[] Tags { get; set; } = Array.Empty<string>();
    }

    public class ProductRecord
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

    public class LargeDataSet
    {
        public string Id { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public System.Collections.Generic.List<DataRecord> Records { get; set; } = new();
    }

    public class DataRecord
    {
        public int Index { get; set; }
        public string Value { get; set; } = "";
        public double Measurement { get; set; }
    }

    public class FinancialReport
    {
        public string ReportId { get; set; } = "";
        public DateTime GeneratedAt { get; set; }
        public string Currency { get; set; } = "";
        public System.Collections.Generic.List<AccountSummary> Accounts { get; set; } = new();
        public System.Collections.Generic.List<Transaction> Transactions { get; set; } = new();
    }

    public class AccountSummary
    {
        public string AccountId { get; set; } = "";
        public string Name { get; set; } = "";
        public decimal Balance { get; set; }
        public AccountType Type { get; set; }
    }

    public enum AccountType { Checking, Savings, Investment }

    public class Transaction
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = "";
    }

    public class FinancialDataFormat
    {
        public void Save<T>(T data, string filePath)
        {
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            using (var fs = File.Create(filePath))
            using (var gz = new System.IO.Compression.GZipStream(fs, System.IO.Compression.CompressionMode.Compress))
            {
                gz.Write(bytes, 0, bytes.Length);
            }
        }

        public T Load<T>(string filePath)
        {
            using (var fs = File.OpenRead(filePath))
            using (var gz = new System.IO.Compression.GZipStream(fs, System.IO.Compression.CompressionMode.Decompress))
            using (var reader = new StreamReader(gz))
            {
                string json = reader.ReadToEnd();
                return JsonSerializer.Deserialize<T>(json)!;
            }
        }
    }
}