/*
 * ============================================================
 * TOPIC     : Pattern Matching
 * SUBTOPIC  : List and Tuple Patterns - Real-World
 * FILE      : 04_ListTuple_RealWorld.cs
 * PURPOSE   : Real-world applications of list and tuple patterns
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._09_PatternMatching._04_ListTuplePatterns
{
    /// <summary>
    /// Real-world applications of list and tuple patterns
    /// </summary>
    public class ListTuple_RealWorld
    {
        /// <summary>
        /// Entry point for real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Real-World List/Tuple Patterns Demo ===
            Console.WriteLine("=== Real-World List/Tuple Patterns Demo ===\n");

            // ── REAL-WORLD: HTTP Request Parser ───────────────────────────────
            // Output: 1. HTTP Request Parser:
            Console.WriteLine("1. HTTP Request Parser:");
            
            // Parse HTTP request line
            // (method, path, version) = request components
            var req1 = new[] { "GET", "/api/users", "HTTP/1.1" };
            var req2 = new[] { "POST", "/api/users", "HTTP/1.1" };
            var req3 = new[] { "PUT", "/api/users/123", "HTTP/2" };
            var req4 = new[] { "DELETE", "/api/users/123", "HTTP/1.1" };
            
            // ParseHttpRequest returns parsed request info
            Console.WriteLine($"   GET: {ParseHttpRequest(req1)}");
            Console.WriteLine($"   POST: {ParseHttpRequest(req2)}");
            Console.WriteLine($"   PUT: {ParseHttpRequest(req3)}");
            Console.WriteLine($"   DELETE: {ParseHttpRequest(req4)}");

            // ── REAL-WORLD: CSV Row Parser ───────────────────────────────────
            // Output: 2. CSV Row Parser:
            Console.WriteLine("\n2. CSV Row Parser:");
            
            // Parse CSV data rows
            // string[] = CSV fields
            var csv1 = new[] { "Alice", "30", "Engineer" };
            var csv2 = new[] { "Bob", "25" };
            var csv3 = new[] { "Charlie", "35", "Manager", "TRUE" };
            
            // ParseCsvRow returns parsed row info
            Console.WriteLine($"   3 fields: {ParseCsvRow(csv1)}");
            Console.WriteLine($"   2 fields: {ParseCsvRow(csv2)}");
            Console.WriteLine($"   4 fields: {ParseCsvRow(csv3)}");

            // ── REAL-WORLD: Game State Machine ────────────────────────────────
            // Output: 3. Game State Machine:
            Console.WriteLine("\n3. Game State Machine:");
            
            // (playerCount, round, isFinal) = game state
            var game1 = (Players: 2, Round: 1, Final: false);
            var game2 = (Players: 4, Round: 5, Final: false);
            var game3 = (Players: 8, Round: 10, Final: true);
            var game4 = (Players: 1, Round: 1, Final: false);
            
            // GetGameState returns state description
            Console.WriteLine($"   2 players, round 1: {GetGameState(game1)}");
            Console.WriteLine($"   4 players, round 5: {GetGameState(game2)}");
            Console.WriteLine($"   8 players, round 10 final: {GetGameState(game3)}");
            Console.WriteLine($"   1 player: {GetGameState(game4)}");

            // ── REAL-WORLD: Weather Forecast ─────────────────────────────────
            // Output: 4. Weather Forecast:
            Console.WriteLine("\n4. Weather Forecast:");
            
            // (temp, humidity, conditions) = weather data
            var weather1 = (Temp: 25, Humidity: 60, Conditions: "Sunny");
            var weather2 = (Temp: 15, Humidity: 85, Conditions: "Rainy");
            var weather3 = (Temp: -5, Humidity: 40, Conditions: "Snowy");
            var weather4 = (Temp: 30, Humidity: 90, Conditions: "Humid");
            
            // GetWeatherAdvice returns advice
            Console.WriteLine($"   25°C, 60%, Sunny: {GetWeatherAdvice(weather1)}");
            Console.WriteLine($"   15°C, 85%, Rainy: {GetWeatherAdvice(weather2)}");
            Console.WriteLine($"   -5°C, 40%, Snowy: {GetWeatherAdvice(weather3)}");
            Console.WriteLine($"   30°C, 90%, Humid: {GetWeatherAdvice(weather4)}");

            // ── REAL-WORLD: Validation Pipeline ──────────────────────────────
            // Output: 5. Validation Pipeline:
            Console.WriteLine("\n5. Validation Pipeline:");
            
            // (field, value, errors) = validation result
            var valid1 = (Fields: new[] { "email", "password" }, Valid: true);
            var valid2 = (Fields: new[] { "email" }, Valid: false);
            var valid3 = (Fields: new string[] { }, Valid: false);
            
            // ValidateForm returns result
            Console.WriteLine($"   All fields: {ValidateForm(valid1)}");
            Console.WriteLine($"   Missing fields: {ValidateForm(valid2)}");
            Console.WriteLine($"   Empty: {ValidateForm(valid3)}");

            Console.WriteLine("\n=== Real-World List/Tuple Patterns Complete ===");
        }

        /// <summary>
        /// Parses HTTP request line using list patterns
        /// </summary>
        public static string ParseHttpRequest(string[] request)
        {
            // List pattern matching on request components
            return request switch
            {
                // GET request
                ["GET", var path, _] => $"GET {path}",
                
                // POST with data
                ["POST", var path, _] => $"POST to {path}",
                
                // PUT update
                ["PUT", var path, _] => $"PUT {path}",
                
                // DELETE request
                ["DELETE", var path, _] => $"DELETE {path}",
                
                // Unknown method
                [var method, var path, _] => $"Unknown: {method} {path}",
                
                // Invalid request
                _ => "Invalid request"
            };
        }

        /// <summary>
        /// Parses CSV row using list patterns
        /// </summary>
        public static string ParseCsvRow(string[] fields)
        {
            // List pattern with count
            return fields switch
            {
                // Single field
                [_] => "Single column",
                
                // Two fields = key-value
                [var key, var value] => $"Key-Value: {key}={value}",
                
                // Three fields = standard record
                [var col1, var col2, var col3] => $"Record: {col1}, {col2}, {col3}",
                
                // Four+ fields = extended record
                [var a, var b, var c, ..] => $"Extended: {a}, {b}, {c}, +{fields.Length - 3} more",
                
                // Empty
                [] => "Empty row"
            };
        }

        /// <summary>
        /// Gets game state description using tuple patterns
        /// </summary>
        public static string GetGameState((int Players, int Round, bool Final) state)
        {
            // Tuple pattern with relational
            return state switch
            {
                // Not enough players
                (< 2, _, _) => "Waiting for players",
                
                // Final round
                (_, _, true) => "Final Round!",
                
                // Last round before final
                (_, 9, _) => "Final Round Next",
                
                // Mid-game
                (>= 4, >= 3, _) => "Mid-Game",
                
                // Early game
                (_, < 3, _) => "Early Game",
                
                // Default
                _ => "In Progress"
            };
        }

        /// <summary>
        /// Gets weather advice using tuple patterns
        /// </summary>
        public static string GetWeatherAdvice((int Temp, int Humidity, string Conditions) weather)
        {
            // Tuple pattern with multiple conditions
            return weather switch
            {
                // Hot and humid = warning
                (> 28, > 80, _) => "Warning: Hot and humid! Stay hydrated.",
                
                // Cold and snowy = cold warning
                (< 0, _, "Snowy") => "Warning: Snow expected. Drive carefully.",
                
                // Rainy = bring umbrella
                (_, _, "Rainy") => "Bring umbrella!",
                
                // Sunny and warm = enjoy
                (> 20, < 70, "Sunny") => "Great weather! Enjoy outdoor activities.",
                
                // Default
                _ => "Check forecast for details."
            };
        }

        /// <summary>
        /// Validates form using list patterns
        /// </summary>
        public static string ValidateForm((string[] Fields, bool Valid) form)
        {
            // List pattern in tuple
            return form switch
            {
                // All fields valid
                ({ Length: > 0 }, true) => $"Valid ({form.Fields.Length} fields)",
                
                // Empty fields
                ({ Length: 0 }, _) => "Error: No fields to validate",
                
                // Invalid fields
                ({ Length: var count }, false) => $"Error: {count} invalid fields",
                
                // Default
                _ => "Unknown validation state"
            };
        }
    }
}
