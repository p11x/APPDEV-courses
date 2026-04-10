/*
 * ============================================================
 * TOPIC     : Pattern Matching
 * SUBTOPIC  : Property Patterns - Real-World
 * FILE      : 03_PropertyPatterns_RealWorld.cs
 * PURPOSE   : Real-world applications of property patterns in business scenarios
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._09_PatternMatching._03_PropertyPatterns
{
    /// <summary>
    /// Real-world applications of property patterns
    /// </summary>
    public class PropertyPatterns_RealWorld
    {
        /// <summary>
        /// Entry point for real-world property pattern examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Real-World Property Patterns Demo ===
            Console.WriteLine("=== Real-World Property Patterns Demo ===\n");

            // ── REAL-WORLD: Loan Eligibility Checker ─────────────────────────
            // Output: 1. Loan Eligibility Checker:
            Console.WriteLine("1. Loan Eligibility Checker:");
            
            // Check loan eligibility based on applicant properties
            // CheckLoanEligibility returns eligibility result
            var applicant1 = new LoanApplicant { Income = 75000, CreditScore = 750, DebtRatio = 0.3, EmploymentYears = 5 };
            var applicant2 = new LoanApplicant { Income = 40000, CreditScore = 620, DebtRatio = 0.4, EmploymentYears = 2 };
            var applicant3 = new LoanApplicant { Income = 120000, CreditScore = 800, DebtRatio = 0.5, EmploymentYears = 10 };
            
            Console.WriteLine($"   High income, good credit: {CheckLoanEligibility(applicant1)}");
            Console.WriteLine($"   Low income, fair credit: {CheckLoanEligibility(applicant2)}");
            Console.WriteLine($"   High income, excellent credit: {CheckLoanEligibility(applicant3)}");

            // ── REAL-WORLD: Hotel Room Rate Calculator ───────────────────────
            // Output: 2. Hotel Room Rate Calculator:
            Console.WriteLine("\n2. Hotel Room Rate Calculator:");
            
            // Calculate room rate based on room properties
            var standard = new HotelRoom("standard", 2, false, false);
            var deluxe = new HotelRoom("deluxe", 2, true, false);
            var suite = new HotelRoom("suite", 4, true, true);
            var penthouse = new HotelRoom("penthouse", 4, true, true);
            
            // CalculateRoomRate returns nightly rate
            Console.WriteLine($"   Standard: ${CalculateRoomRate(standard):F2}");
            Console.WriteLine($"   Deluxe: ${CalculateRoomRate(deluxe):F2}");
            Console.WriteLine($"   Suite: ${CalculateRoomRate(suite):F2}");
            Console.WriteLine($"   Penthouse: ${CalculateRoomRate(penthouse):F2}");

            // ── REAL-WORLD: Email Classifier ────────────────────────────────
            // Output: 3. Email Classifier:
            Console.WriteLine("\n3. Email Classifier:");
            
            // Classify emails based on properties
            var urgent = new Email("Urgency", "Alice", "high", true);
            var promotion = new Email("Sale!", "Shop", "low", false);
            var meeting = new Email("Meeting Notes", "Bob", "normal", false);
            var spam = new Email("Win Prize!", "Unknown", "none", false);
            
            // ClassifyEmail returns classification
            Console.WriteLine($"   {urgent.Subject}: {ClassifyEmail(urgent)}");
            Console.WriteLine($"   {promotion.Subject}: {ClassifyEmail(promotion)}");
            Console.WriteLine($"   {meeting.Subject}: {ClassifyEmail(meeting)}");
            Console.WriteLine($"   {spam.Subject}: {ClassifyEmail(spam)}");

            // ── REAL-WORLD: Game Achievement Unlocks ───────────────────────
            // Output: 4. Game Achievement Unlocks:
            Console.WriteLine("\n4. Game Achievement Unlocks:");
            
            // Check achievement unlocks based on player stats
            var beginner = new GamePlayer("Player1", 100, 10, 1, false);
            var intermediate = new GamePlayer("Player2", 5000, 100, 5, true);
            var expert = new GamePlayer("Player3", 50000, 500, 10, true);
            var champion = new GamePlayer("Player4", 100000, 1000, 20, true);
            
            // CheckAchievements returns achievements
            Console.WriteLine($"   {beginner.Name}: {CheckAchievements(beginner)}");
            Console.WriteLine($"   {intermediate.Name}: {CheckAchievements(intermediate)}");
            Console.WriteLine($"   {expert.Name}: {CheckAchievements(expert)}");
            Console.WriteLine($"   {champion.Name}: {CheckAchievements(champion)}");

            // ── REAL-WORLD: Restaurant Order Validator ──────────────────────
            // Output: 5. Restaurant Order Validator:
            Console.WriteLine("\n5. Restaurant Order Validator:");
            
            // Validate food orders based on properties
            var validOrder = new FoodOrder("Burger", 12.99, false, false);
            var invalidOrder = new FoodOrder("Steak", 45.00, true, false);
            var complexOrder = new FoodOrder("Pasta", 18.00, true, true);
            
            // ValidateOrder returns validation result
            Console.WriteLine($"   {validOrder.Item}: {ValidateOrder(validOrder)}");
            Console.WriteLine($"   {invalidOrder.Item}: {ValidateOrder(invalidOrder)}");
            Console.WriteLine($"   {complexOrder.Item}: {ValidateOrder(complexOrder)}");

            Console.WriteLine("\n=== Real-World Property Patterns Complete ===");
        }

        /// <summary>
        /// Checks loan eligibility based on applicant properties
        /// </summary>
        public static string CheckLoanEligibility(LoanApplicant applicant)
        {
            // Multiple property patterns with conditions
            return applicant switch
            {
                // High income, excellent credit, low debt = approved premium
                { Income: > 100000, CreditScore: >= 750, DebtRatio: < 0.4 } => "Approved (Premium Rate)",
                
                // Good income, good credit = approved standard
                { Income: > 50000, CreditScore: >= 650, DebtRatio: < 0.5 } => "Approved (Standard Rate)",
                
                // Fair credit, reasonable debt = approved with conditions
                { CreditScore: >= 600, DebtRatio: < 0.5 } => "Approved (High Rate)",
                
                // Poor credit or high debt = denied
                { CreditScore: < 600 } or { DebtRatio: >= 0.5 } => "Denied (Credit/ Debt Issue)",
                
                // Default
                _ => "Review Required"
            };
        }

        /// <summary>
        /// Calculates hotel room rate based on room properties
        /// </summary>
        public static double CalculateRoomRate(HotelRoom room)
        {
            // Base rate from type, add for amenities
            return room switch
            {
                // Penthouse with all amenities = highest rate
                { Type: "penthouse", HasView: true, HasKitchen: true } => 500.00,
                
                // Suite with view = premium
                { Type: "suite", HasView: true, _ } => 300.00,
                
                // Suite = standard suite rate
                { Type: "suite", _ } => 220.00,
                
                // Deluxe with view = premium
                { Type: "deluxe", HasView: true, _ } => 180.00,
                
                // Deluxe = standard deluxe rate
                { Type: "deluxe", _ } => 140.00,
                
                // Standard with view = upgraded standard
                { Type: "standard", HasView: true, _ } => 120.00,
                
                // Standard = base rate
                { Type: "standard", _ } => 100.00,
                
                // Default
                _ => 100.00
            };
        }

        /// <summary>
        /// Classifies email based on properties
        /// </summary>
        public static string ClassifyEmail(Email email)
        {
            // Property pattern matching on multiple properties
            return (email.Priority, email.IsUrgent, email.Sender) switch
            {
                // Urgent flag set = urgent
                (_, true, _) => "Urgent",
                
                // High priority from known sender = important
                ("high", _, { Length: > 0 }) => "Important",
                
                // Low priority = promotion
                ("low", _, _) => "Promotion",
                
                // Unknown sender, normal priority = inbox
                ("normal", _, "Unknown") => "Potential Spam",
                
                // Known sender = normal email
                ("normal", _, { Length: > 0 }) => "Normal",
                
                // Default = other
                _ => "Other"
            };
        }

        /// <summary>
        /// Checks game achievements based on player properties
        /// </summary>
        public static string CheckAchievements(GamePlayer player)
        {
            // Build achievement list using property patterns
            var achievements = new System.Collections.Generic.List<string>();
            
            // Check each achievement condition
            if (player.Score >= 100)
                achievements.Add("Beginner");
            if (player.Score >= 5000)
                achievements.Add("Intermediate");
            if (player.Score >= 50000)
                achievements.Add("Expert");
            if (player.Score >= 100000)
                achievements.Add("Master");
            
            if (player.Kills >= 10)
                achievements.Add("Combatant");
            if (player.Level >= 5)
                achievements.Add("Rising Star");
            if (player.IsVIP)
                achievements.Add("VIP Member");
            
            // Return achievements or none
            return achievements.Count > 0 
                ? string.Join(", ", achievements) 
                : "None";
        }

        /// <summary>
        /// Validates food order based on properties
        /// </summary>
        public static string ValidateOrder(FoodOrder order)
        {
            // Property patterns for validation rules
            return order switch
            {
                // Premium item with modifications = needs approval
                { Price: > 40, HasModifications: true } => "Needs Manager Approval",
                
                // Expensive item = needs approval
                { Price: > 30 } => "Needs Supervisor Approval",
                
                // Item with allergens marked = warning
                { ContainsAllergens: true, HasModifications: false } => "Contains Allergens - Warning Issued",
                
                // Valid simple order = approved
                { HasModifications: false, ContainsAllergens: false } => "Approved",
                
                // Modified order = needs review
                { HasModifications: true } => "Needs Kitchen Review",
                
                // Default
                _ => "Requires Review"
            };
        }
    }

    // ── REAL-WORLD CLASSES ───────────────────────────────────────────────
    /// <summary>
    /// Loan applicant with financial properties
    /// </summary>
    public class LoanApplicant
    {
        public decimal Income { get; set; }
        public int CreditScore { get; set; }
        public double DebtRatio { get; set; }
        public int EmploymentYears { get; set; }
    }

    /// <summary>
    /// Hotel room with amenities
    /// </summary>
    public class HotelRoom
    {
        public string Type { get; }
        public int MaxOccupancy { get; }
        public bool HasView { get; }
        public bool HasKitchen { get; }
        
        public HotelRoom(string type, int max, bool view, bool kitchen)
        {
            Type = type;
            MaxOccupancy = max;
            HasView = view;
            HasKitchen = kitchen;
        }
    }

    /// <summary>
    /// Email with properties for classification
    /// </summary>
    public class Email
    {
        public string Subject { get; }
        public string Sender { get; }
        public string Priority { get; }
        public bool IsUrgent { get; }
        
        public Email(string subject, string sender, string priority, bool urgent)
        {
            Subject = subject;
            Sender = sender;
            Priority = priority;
            IsUrgent = urgent;
        }
    }

    /// <summary>
    /// Game player with achievement properties
    /// </summary>
    public class GamePlayer
    {
        public string Name { get; }
        public int Score { get; }
        public int Kills { get; }
        public int Level { get; }
        public bool IsVIP { get; }
        
        public GamePlayer(string name, int score, int kills, int level, bool vip)
        {
            Name = name;
            Score = score;
            Kills = kills;
            Level = level;
            IsVIP = vip;
        }
    }

    /// <summary>
    /// Food order with dietary properties
    /// </summary>
    public class FoodOrder
    {
        public string Item { get; }
        public decimal Price { get; }
        public bool HasModifications { get; }
        public bool ContainsAllergens { get; }
        
        public FoodOrder(string item, decimal price, bool mods, bool allergens)
        {
            Item = item;
            Price = price;
            HasModifications = mods;
            ContainsAllergens = allergens;
        }
    }
}
