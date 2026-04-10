/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Strategy Variations
 * FILE      : 02_StrategyVariations.cs
 * PURPOSE   : Demonstrates different Strategy pattern approaches
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral._02_Strategy
{
    /// <summary>
    /// Demonstrates Strategy variations
    /// </summary>
    public class StrategyVariations
    {
        /// <summary>
        /// Entry point for Strategy variations
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Strategy Variations ===
            Console.WriteLine("=== Strategy Variations ===\n");

            // ── CONCEPT: Generic Strategy ─────────────────────────────────────
            // Type-safe strategy with generics

            // Example 1: Generic Strategy
            // Output: 1. Generic Strategy:
            Console.WriteLine("1. Generic Strategy:");
            
            // Use generic strategy for different types
            var converter = new GenericConverter<string, int>(new StringToIntStrategy());
            var result = converter.Convert("42");
            // Output: Converted: 42
            
            var boolConverter = new GenericConverter<string, bool>(new StringToBoolStrategy());
            var boolResult = boolConverter.Convert("true");
            // Output: Converted: True

            // ── CONCEPT: Strategy with Configuration ─────────────────────────
            // Strategies with parameters

            // Example 2: Strategy with Configuration
            // Output: 2. Strategy with Configuration:
            Console.WriteLine("\n2. Strategy with Configuration:");
            
            // Compressor with configuration
            var compressor = new Compressor();
            
            // High quality - slower
            compressor.Compress("data", Quality.High);
            // Output: Compressed with High quality
            
            // Low quality - faster
            compressor.Compress("data", Quality.Low);
            // Output: Compressed with Low quality

            // ── CONCEPT: Strategy Factory ─────────────────────────────────────
            // Create strategies dynamically

            // Example 3: Strategy Factory
            // Output: 3. Strategy Factory:
            Console.WriteLine("\n3. Strategy Factory:");
            
            // Get strategy based on type
            var auth1 = AuthStrategyFactory.GetStrategy("OAuth");
            var auth2 = AuthStrategyFactory.GetStrategy("JWT");
            var auth3 = AuthStrategyFactory.GetStrategy("APIKey");
            
            // Output: OAuth strategy created
            // Output: JWT strategy created
            // Output: APIKey strategy created

            // ── REAL-WORLD EXAMPLE: Compression ────────────────────────────────
            // Output: --- Real-World: Compression ---
            Console.WriteLine("\n--- Real-World: Compression ---");
            
            // File compression with different algorithms
            var fileCompressor = new FileCompressor();
            
            // Use different compression levels
            var gzipResult = fileCompressor.Compress("document.txt", "GZip");
            var zipResult = fileCompressor.Compress("document.txt", "Zip");
            var lz4Result = fileCompressor.Compress("document.txt", "LZ4");
            
            // Output: GZip compressed: 500 bytes
            // Output: Zip compressed: 550 bytes
            // Output: LZ4 compressed: 400 bytes (fastest)

            Console.WriteLine("\n=== Strategy Variations Complete ===");
        }
    }

    /// <summary>
    /// Generic converter interface
    /// </summary>
    public interface IConverter<TInput, TOutput>
    {
        TOutput Convert(TInput input); // method: converts input to output
    }

    /// <summary>
    /// String to int converter
    /// </summary>
    public class StringToIntStrategy : IConverter<string, int>
    {
        public int Convert(string input)
        {
            var result = int.Parse(input);
            Console.WriteLine($"   Converted: {result}");
            return result;
        }
    }

    /// <summary>
    /// String to bool converter
    /// </summary>
    public class StringToBoolStrategy : IConverter<string, bool>
    {
        public bool Convert(string input)
        {
            var result = bool.Parse(input);
            Console.WriteLine($"   Converted: {result}");
            return result;
        }
    }

    /// <summary>
    /// Generic converter
    /// </summary>
    public class GenericConverter<TInput, TOutput> : IConverter<TInput, TOutput>
    {
        private IConverter<TInput, TOutput> _strategy;
        
        public GenericConverter(IConverter<TInput, TOutput> strategy)
        {
            _strategy = strategy;
        }
        
        public TOutput Convert(TInput input)
        {
            return _strategy.Convert(input);
        }
    }

    /// <summary>
    /// Compression quality
    /// </summary>
    public enum Quality { Low, Medium, High }

    /// <summary>
    /// Compression strategy interface
    /// </summary>
    public interface ICompressionStrategy
    {
        byte[] Compress(string data, Quality quality); // method: compresses data
    }

    /// <summary>
    /// Default compression
    /// </summary>
    public class DefaultCompressionStrategy : ICompressionStrategy
    {
        public byte[] Compress(string data, Quality quality)
        {
            var size = quality switch
            {
                Quality.High => data.Length / 2,
                Quality.Medium => data.Length / 3,
                Quality.Low => data.Length * 2 / 3
            };
            Console.WriteLine($"   Compressed with {quality} quality");
            return new byte[size];
        }
    }

    /// <summary>
    /// Compressor using strategy
    /// </summary>
    public class Compressor
    {
        private ICompressionStrategy _strategy = new DefaultCompressionStrategy();
        
        public void Compress(string data, Quality quality)
        {
            _strategy.Compress(data, quality);
        }
    }

    /// <summary>
    /// Authentication strategy interface
    /// </summary>
    public interface IAuthStrategy
    {
        void Authenticate(); // method: authenticates user
    }

    /// <summary>
    /// OAuth strategy
    /// </summary>
    public class OAuthStrategy : IAuthStrategy
    {
        public void Authenticate()
        {
            Console.WriteLine("   OAuth strategy created");
        }
    }

    /// <summary>
    /// JWT strategy
    /// </summary>
    public class JWTStrategy : IAuthStrategy
    {
        public void Authenticate()
        {
            Console.WriteLine("   JWT strategy created");
        }
    }

    /// <summary>
    /// API Key strategy
    /// </summary>
    public class APIKeyStrategy : IAuthStrategy
    {
        public void Authenticate()
        {
            Console.WriteLine("   APIKey strategy created");
        }
    }

    /// <summary>
    /// Auth strategy factory
    /// </summary>
    public static class AuthStrategyFactory
    {
        public static IAuthStrategy GetStrategy(string type)
        {
            return type switch
            {
                "OAuth" => new OAuthStrategy(),
                "JWT" => new JWTStrategy(),
                "APIKey" => new APIKeyStrategy(),
                _ => throw new ArgumentException($"Unknown auth: {type}")
            };
        }
    }

    /// <summary>
    /// File compressor
    /// </summary>
    public class FileCompressor
    {
        public string Compress(string fileName, string algorithm)
        {
            var size = algorithm switch
            {
                "GZip" => 500,
                "Zip" => 550,
                "LZ4" => 400,
                _ => 500
            };
            Console.WriteLine($"   {algorithm} compressed: {size} bytes");
            return $"{fileName}.{algorithm.ToLower()}";
        }
    }
}