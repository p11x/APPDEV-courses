/*
 * ============================================================
 * TOPIC     : Pattern Matching
 * SUBTOPIC  : Deconstruction Patterns - Real-World
 * FILE      : 03_Deconstruction_RealWorld.cs
 * PURPOSE   : Real-world applications of deconstruction patterns
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._09_PatternMatching._05_DeconstructionPatterns
{
    /// <summary>
    /// Real-world applications of deconstruction patterns
    /// </summary>
    public class Deconstruction_RealWorld
    {
        /// <summary>
        /// Entry point for real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Real-World Deconstruction Patterns Demo ===
            Console.WriteLine("=== Real-World Deconstruction Patterns Demo ===\n");

            // ── REAL-WORLD: API Response Handler ─────────────────────────────
            // Output: 1. API Response Handler:
            Console.WriteLine("1. API Response Handler:");
            
            // Handle different API response types
            var successResp = new ApiResponse2(true, 200, "Success", "User data", null);
            var errorResp = new ApiResponse2(false, 500, "Error", null, "Internal server error");
            var authResp = new ApiResponse2(false, 401, "Unauthorized", null, "Token expired");
            
            // ProcessApiResponse returns handling action
            Console.WriteLine($"   Success: {ProcessApiResponse(successResp)}");
            Console.WriteLine($"   Error: {ProcessApiResponse(errorResp)}");
            Console.WriteLine($"   Auth: {ProcessApiResponse(authResp)}");

            // ── REAL-WORLD: Transaction Processor ───────────────────────────
            // Output: 2. Transaction Processor:
            Console.WriteLine("\n2. Transaction Processor:");
            
            // Process different transaction types
            var cardTxn = new Transaction(TransactionType.Card, 100.00m, "USD", true);
            var bankTxn = new Transaction(TransactionType.Bank, 500.00m, "EUR", false);
            var cashTxn = new Transaction(TransactionType.Cash, 50.00m, "USD", true);
            
            // GetTransactionAdvice returns advice
            Console.WriteLine($"   Card $100: {GetTransactionAdvice(cardTxn)}");
            Console.WriteLine($"   Bank $500: {GetTransactionAdvice(bankTxn)}");
            Console.WriteLine($"   Cash $50: {GetTransactionAdvice(cashTxn)}");

            // ── REAL-WORLD: User Profile Parser ──────────────────────────────
            // Output: 3. User Profile Parser:
            Console.WriteLine("\n3. User Profile Parser:");
            
            // Parse user profile data
            var profile1 = new UserProfile("Alice", "alice@email.com", true, true);
            var profile2 = new UserProfile("Bob", "bob@email.com", false, true);
            var profile3 = new UserProfile("Charlie", "charlie@email.com", false, false);
            
            // GetProfileAccess returns access level
            Console.WriteLine($"   {profile1.Name}: {GetProfileAccess(profile1)}");
            Console.WriteLine($"   {profile2.Name}: {GetProfileAccess(profile2)}");
            Console.WriteLine($"   {profile3.Name}: {GetProfileAccess(profile3)}");

            // ── REAL-WORLD: Inventory Item ───────────────────────────────────
            // Output: 4. Inventory Item Classifier:
            Console.WriteLine("\n4. Inventory Item Classifier:");
            
            // Classify inventory items
            var item1 = new InventoryItem("Laptop", 10, 999.99m, true);
            var item2 = new InventoryItem("Pencil", 1000, 0.99m, false);
            var item3 = new InventoryItem("Chair", 5, 149.99m, true);
            
            // ClassifyInventory returns classification
            Console.WriteLine($"   {item1.Name}: {ClassifyInventory(item1)}");
            Console.WriteLine($"   {item2.Name}: {ClassifyInventory(item2)}");
            Console.WriteLine($"   {item3.Name}: {ClassifyInventory(item3)}");

            Console.WriteLine("\n=== Real-World Deconstruction Patterns Complete ===");
        }

        /// <summary>
        /// Processes API response using deconstruction
        /// </summary>
        public static string ProcessApiResponse(ApiResponse2 response)
        {
            // Deconstruct: (IsSuccess, StatusCode, Status, Data, Error)
            return response switch
            {
                // Success with data
                (true, 200, _, var data, _) when data != null => $"OK - Data: {data}",
                
                // Success but no data
                (true, _, var status, _, _) => $"Success: {status}",
                
                // Authentication error
                (false, 401, _, _, var error) => $"Auth Error: {error}",
                
                // Server error
                (false, >= 500, _, _, var error) => $"Server Error: {error}",
                
                // Client error
                (false, 400, _, _, var error) => $"Client Error: {error}",
                
                // Default error
                (false, _, var status, _, var error) => $"Error {status}: {error}"
            };
        }

        /// <summary>
        /// Gets transaction advice using deconstruction
        /// </summary>
        public static string GetTransactionAdvice(Transaction txn)
        {
            // Deconstruct: (Type, Amount, Currency, IsInternational)
            return txn switch
            {
                // Card, international = warning
                (TransactionType.Card, var amount, _, true) when amount > 1000 => 
                    $"Card international warning: ${amount}",
                
                // Bank, large amount = verify
                (TransactionType.Bank, var amount, _, _) when amount > 500 => 
                    $"Bank transfer - verification recommended: ${amount}",
                
                // Cash, domestic = simple
                (TransactionType.Cash, var amount, var currency, false) => 
                    $"Cash {currency} transaction: ${amount}",
                
                // Card = process normally
                (TransactionType.Card, var amount, var currency, _) => 
                    $"Process card {currency} {amount}",
                
                // Any bank = bank transfer
                (TransactionType.Bank, _, _, _) => "Bank transfer initiated",
                
                // Default
                _ => "Process as-is"
            };
        }

        /// <summary>
        /// Gets profile access level using deconstruction
        /// </summary>
        public static string GetProfileAccess(UserProfile profile)
        {
            // Deconstruct: (Name, Email, IsVerified, IsPremium)
            return profile switch
        {
            // Premium and verified = full access
            (var name, _, true, true) => $"Full access: {name}",
            
            // Verified but not premium = standard
            (var name, _, true, false) => $"Standard access: {name}",
            
            // Premium but not verified = limited
            (var name, var email, false, true) => $"Limited (verify {email}): {name}",
            
            // Not verified = restricted
            (var name, var email, false, false) => $"Restricted - verify {email}: {name}",
            
            // Default
            _ => "No access"
        };
    }

    /// <summary>
    /// Classifies inventory using deconstruction
    /// </summary>
    public static string ClassifyInventory(InventoryItem item)
    {
        // Deconstruct: (Name, Quantity, Price, IsHighValue)
        return item switch
        {
            // High value and low quantity = critical
            (var name, < 10, var price, true) when price > 100 => 
                $"CRITICAL: {name} (${price}, qty {item.Quantity})",
            
            // Low stock = reorder
            (var name, < 20, _, _) => $"Reorder: {name}",
            
            // High value = valuable
            (var name, _, var price, true) => $"Valuable: {name} (${price})",
            
            // Bulk items = discount eligible
            (var name, > 500, _, _) => $"Bulk: {name}",
            
            // Default
            (var name, var qty, var price, _) => $"Standard: {name} (${price}, qty {qty})"
        };
    }

    // ── REAL-WORLD CLASSES ───────────────────────────────────────────────
    /// <summary>
    /// API response with deconstruction
    /// </summary>
    public class ApiResponse2
    {
        public bool IsSuccess { get; }
        public int StatusCode { get; }
        public string Status { get; }
        public string? Data { get; }
        public string? Error { get; }

        public ApiResponse2(bool success, int code, string status, string? data, string? error)
        {
            IsSuccess = success;
            StatusCode = code;
            Status = status;
            Data = data;
            Error = error;
        }

        public void Deconstruct(out bool isSuccess, out int statusCode, out string status, 
            out string? data, out string? error)
        {
            isSuccess = IsSuccess;
            statusCode = StatusCode;
            status = Status;
            data = Data;
            error = Error;
        }
    }

    /// <summary>
    /// Transaction type enum
    /// </summary>
    public enum TransactionType { Card, Bank, Cash }

    /// <summary>
    /// Transaction with deconstruction
    /// </summary>
    public class Transaction
    {
        public TransactionType Type { get; }
        public decimal Amount { get; }
        public string Currency { get; }
        public bool IsInternational { get; }

        public Transaction(TransactionType type, decimal amount, string currency, bool international)
        {
            Type = type;
            Amount = amount;
            Currency = currency;
            IsInternational = international;
        }

        public void Deconstruct(out TransactionType type, out decimal amount, 
            out string currency, out bool isInternational)
        {
            type = Type;
            amount = Amount;
            currency = Currency;
            isInternational = IsInternational;
        }
    }

    /// <summary>
    /// User profile with deconstruction
    /// </summary>
    public class UserProfile
    {
        public string Name { get; }
        public string Email { get; }
        public bool IsVerified { get; }
        public bool IsPremium { get; }

        public UserProfile(string name, string email, bool verified, bool premium)
        {
            Name = name;
            Email = email;
            IsVerified = verified;
            IsPremium = premium;
        }

        public void Deconstruct(out string name, out string email, 
            out bool isVerified, out bool isPremium)
        {
            name = Name;
            email = Email;
            isVerified = IsVerified;
            isPremium = IsPremium;
        }
    }

    /// <summary>
    /// Inventory item with deconstruction
    /// </summary>
    public class InventoryItem
    {
        public string Name { get; }
        public int Quantity { get; }
        public decimal Price { get; }
        public bool IsHighValue { get; }

        public InventoryItem(string name, int qty, decimal price, bool highValue)
        {
            Name = name;
            Quantity = qty;
            Price = price;
            IsHighValue = highValue;
        }

        public void Deconstruct(out string name, out int quantity, 
            out decimal price, out bool isHighValue)
        {
            name = Name;
            quantity = Quantity;
            price = Price;
            isHighValue = IsHighValue;
        }
    }
}
