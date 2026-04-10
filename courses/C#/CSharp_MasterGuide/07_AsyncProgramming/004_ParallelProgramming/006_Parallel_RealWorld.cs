/*
 * TOPIC: Parallel Programming
 * SUBTOPIC: Parallel Real-World
 * FILE: 06_Parallel_RealWorld.cs
 * PURPOSE: Real-world parallel programming examples
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._04_ParallelProgramming
{
    public class ParallelRealWorld
    {
        public static void Main()
        {
            Console.WriteLine("=== Parallel Real-World Demo ===\n");

            var demo = new ParallelRealWorld();

            // Example 1: Parallel file processing
            Console.WriteLine("1. Parallel file processing:");
            demo.ParallelFileProcessing();

            // Example 2: Parallel data processing pipeline
            Console.WriteLine("\n2. Data pipeline:");
            demo.DataPipelineDemo();

            // Example 3: Parallel web scraping
            Console.WriteLine("\n3. Parallel web scraping:");
            demo.WebScrapingDemo();

            // Example 4: Parallel batch processing
            Console.WriteLine("\n4. Batch processing:");
            demo.BatchProcessingDemo();

            // Example 5: Parallel report generation
            Console.WriteLine("\n5. Report generation:");
            demo.ReportGenerationDemo();

            // Example 6: Parallel data import
            Console.WriteLine("\n6. Data import:");
            demo.DataImportDemo();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public void ParallelFileProcessing()
        {
            var files = new[] { "f1.txt", "f2.txt", "f3.txt", "f4.txt" };
            var results = new System.Collections.Concurrent.ConcurrentBag<string>();

            // Ensure files exist
            foreach (var f in files)
            {
                if (!File.Exists(f))
                    File.WriteAllTextAsync(f, f).Wait();
            }

            Parallel.ForEach(files, file =>
            {
                var content = File.ReadAllText(file);
                var processed = content.ToUpper();
                results.Add(processed);
            });

            Console.WriteLine($"   Processed: {results.Count} files");
        }

        public void DataPipelineDemo()
        {
            var sw = Stopwatch.StartNew();
            var data = Enumerable.Range(1, 1000).ToList();

            // Stage 1: Transform
            var transformed = new List<int>();
            Parallel.ForEach(data, item =>
            {
                lock (transformed) transformed.Add(item * 2);
            });

            // Stage 2: Filter
            var filtered = new List<int>();
            Parallel.ForEach(transformed, item =>
            {
                if (item % 3 == 0)
                    lock (filtered) filtered.Add(item);
            });

            // Stage 3: Aggregate
            int sum = 0;
            Parallel.ForEach(filtered, item =>
            {
                Interlocked.Add(ref sum, item);
            });

            sw.Stop();
            Console.WriteLine($"   Sum: {sum}, Time: {sw.ElapsedMilliseconds}ms");
        }

        public void WebScrapingDemo()
        {
            var urls = new[] { "url1", "url2", "url3", "url4", "url5" };
            var results = new Dictionary<string, string>();

            Parallel.ForEach(urls, url =>
            {
                // Simulate web request
                Thread.Sleep(50);
                lock (results) results[url] = $"Data from {url}";
            });

            Console.WriteLine($"   Scraped: {results.Count} pages");
        }

        public void BatchProcessingDemo()
        {
            var batches = Enumerable.Range(1, 10).Select(i => 
                Enumerable.Range(i * 10, 10).ToArray()).ToArray();

            var processed = new System.Collections.Concurrent.ConcurrentBag<int>();

            Parallel.ForEach(batches, batch =>
            {
                int batchSum = batch.Sum();
                Thread.Sleep(30);
                processed.Add(batchSum);
            });

            Console.WriteLine($"   Batches processed: {processed.Count}");
        }

        public void ReportGenerationDemo()
        {
            var sw = Stopwatch.StartNew();
            var reportData = new Dictionary<string, List<int>>();

            // Generate multiple report sections in parallel
            Parallel.Invoke(
                () => reportData["Sales"] = GenerateSalesData(),
                () => reportData["Inventory"] = GenerateInventoryData(),
                () => reportData["Customers"] = GenerateCustomerData()
            );

            sw.Stop();
            Console.WriteLine($"   Reports: {reportData.Count}, Time: {sw.ElapsedMilliseconds}ms");
        }

        private List<int> GenerateSalesData() { Thread.Sleep(30); return new List<int> { 100, 200 }; }
        private List<int> GenerateInventoryData() { Thread.Sleep(30); return new List<int> { 50, 75 }; }
        private List<int> GenerateCustomerData() { Thread.Sleep(30); return new List<int> { 300 }; }

        public void DataImportDemo()
        {
            var records = Enumerable.Range(1, 500).ToList();
            var imported = new System.Collections.Concurrent.ConcurrentBag<string>();

            Parallel.ForEach(records, record =>
            {
                // Validate
                if (record > 0)
                {
                    // Transform
                    var data = $"Record_{record}";
                    // Save
                    imported.Add(data);
                }
            });

            Console.WriteLine($"   Imported: {imported.Count} records");
        }
    }

    // Real-world service implementations
    public class ParallelImageProcessor
    {
        public void ProcessImages(IEnumerable<string> imagePaths, string outputDir)
        {
            Directory.CreateDirectory(outputDir);

            Parallel.ForEach(imagePaths, path =>
            {
                // Load
                byte[] data = File.ReadAllBytes(path);
                // Process
                byte[] processed = ApplyFilter(data);
                // Save
                string output = Path.Combine(outputDir, Path.GetFileName(path));
                File.WriteAllBytes(output, processed);
            });
        }

        private byte[] ApplyFilter(byte[] data)
        {
            // Simulate processing
            Thread.Sleep(20);
            return data;
        }
    }

    public class ParallelDataAnalyzer
    {
        public AnalysisResult AnalyzeData(DataSet dataset)
        {
            var result = new AnalysisResult();

            Parallel.Invoke(
                () => result.Statistics = ComputeStatistics(dataset),
                () => result.Distributions = ComputeDistributions(dataset),
                () => result.Correlations = ComputeCorrelations(dataset)
            );

            return result;
        }

        private object ComputeStatistics(DataSet ds) { Thread.Sleep(30); return new object(); }
        private object ComputeDistributions(DataSet ds) { Thread.Sleep(30); return new object(); }
        private object ComputeCorrelations(DataSet ds) { Thread.Sleep(30); return new object(); }
    }

    public class DataSet { }
    public class AnalysisResult
    {
        public object Statistics { get; set; }
        public object Distributions { get; set; }
        public object Correlations { get; set; }
    }

    public class ParallelETL
    {
        public void RunETL(ETLJob job)
        {
            // Extract
            var rawData = ExtractParallel(job.Sources);

            // Transform
            var transformed = TransformParallel(rawData);

            // Load
            LoadParallel(transformed, job.Destination);
        }

        private List<string> ExtractParallel(List<string> sources)
        {
            var data = new System.Collections.Concurrent.ConcurrentBag<string>();
            Parallel.ForEach(sources, source =>
            {
                Thread.Sleep(20);
                data.Add($"Data from {source}");
            });
            return data.ToList();
        }

        private List<string> TransformParallel(List<string> rawData)
        {
            var transformed = new System.Collections.Concurrent.ConcurrentBag<string>();
            Parallel.ForEach(rawData, item =>
            {
                transformed.Add(item.ToUpper());
            });
            return transformed.ToList();
        }

        private void LoadParallel(List<string> data, string destination)
        {
            Parallel.ForEach(data, item =>
            {
                Thread.Sleep(10);
            });
        }
    }

    public class ETLJob
    {
        public List<string> Sources { get; set; } = new();
        public string Destination { get; set; }
    }

    public class ParallelSearchEngine
    {
        public List<SearchResult> Search(string query, IEnumerable<string> indexes)
        {
            var results = new System.Collections.Concurrent.ConcurrentBag<SearchResult>();

            Parallel.ForEach(indexes, index =>
            {
                var hits = SearchIndex(index, query);
                foreach (var hit in hits)
                    results.Add(hit);
            });

            return results.OrderByDescending(r => r.Relevance).ToList();
        }

        private List<SearchResult> SearchIndex(string index, string query)
        {
            Thread.Sleep(30);
            return new List<SearchResult>
            {
                new SearchResult { Index = index, Relevance = 0.8 }
            };
        }
    }

    public class SearchResult
    {
        public string Index { get; set; }
        public double Relevance { get; set; }
    }
}