/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Operators - Logical Operators
 * FILE      : LogicalOperators.cs
 * PURPOSE   : This file covers logical operators in C#: && (AND), || (OR), ! (NOT).
 *             These operators work with boolean values and perform boolean algebra.
 * ============================================================
 */

// --- SECTION: Logical Operators ---
// Logical operators combine or negate boolean values
// && (AND), || (OR), ! (NOT) are the primary logical operators

using System;

namespace CSharp_MasterGuide._01_Fundamentals._04_Operators
{
    class LogicalOperators
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: NOT Operator (!)
            // ═══════════════════════════════════════════════════════════════
            
            // Logical NOT - inverts boolean value
            bool isActive = true;
            bool isInactive = !isActive;
            Console.WriteLine($"!true = {isInactive}"); // Output: !true = False
            
            bool isEnabled = false;
            bool isDisabled = !isEnabled;
            Console.WriteLine($"!false = {isDisabled}"); // Output: !false = True
            
            // Double negation - useful for normalization
            bool original = false;
            bool doubleNegated = !!original;
            Console.WriteLine($"!!false = {doubleNegated}"); // Output: !!false = False

            // ═══════════════════════════════════════════════════════════════
            // SECTION: AND Operator (&&)
            // ═══════════════════════════════════════════════════════════════
            
            // Logical AND - true only if BOTH operands are true
            bool a = true;
            bool b = false;
            
            bool andResult = a && b;
            Console.WriteLine($"true && false = {andResult}"); // Output: true && false = False
            
            bool bothTrue = true && true;
            Console.WriteLine($"true && true = {bothTrue}"); // Output: true && true = True
            
            // Short-circuit evaluation: stops if first is false
            // This is useful for null checks
            string? name = null;
            
            // Without short-circuit, would throw NullReferenceException
            if (name != null && name.Length > 0)
            {
                Console.WriteLine($"Name length: {name.Length}");
            }
            else
            {
                Console.WriteLine("Name is null or empty"); // Output: Name is null or empty
            }
            
            // Practical validation
            int age = 25;
            bool hasLicense = true;
            bool canDrive = age >= 18 && hasLicense;
            Console.WriteLine($"Can drive: {canDrive}"); // Output: Can drive: True

            // ═══════════════════════════════════════════════════════════════
            // SECTION: OR Operator (||)
            // ═══════════════════════════════════════════════════════════════
            
            // Logical OR - true if AT LEAST ONE operand is true
            bool x = true;
            bool y = false;
            
            bool orResult = x || y;
            Console.WriteLine($"true || false = {orResult}"); // Output: true || false = True
            
            bool bothFalse = false || false;
            Console.WriteLine($"false || false = {bothFalse}"); // Output: false || false = False
            
            // Short-circuit: stops if first is true
            bool isAdmin = true;
            bool isSuperUser = false;
            bool hasAccess = isAdmin || isSuperUser;
            Console.WriteLine($"Has access: {hasAccess}"); // Output: Has access: True
            
            // Practical: Default value selection
            string? config = null;
            string value = config ?? "default";
            Console.WriteLine($"Config value: {value}"); // Output: Config value: default

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Combining Logical Operators
            // ═══════════════════════════════════════════════════════════════
            
            // Complex boolean expressions
            bool isLoggedIn = true;
            bool isPremium = false;
            bool hasPermission = true;
            
            // Precedence: ! → && → ||
            bool canAccessPremium = isLoggedIn && isPremium;
            bool canAccessBasic = isLoggedIn && hasPermission;
            bool canAccessEither = isPremium || hasPermission;
            
            Console.WriteLine($"Can premium: {canAccessPremium}"); // Output: Can premium: False
            Console.WriteLine($"Can basic: {canAccessBasic}"); // Output: Can basic: True
            Console.WriteLine($"Can either: {canAccessEither}"); // Output: Can either: True
            
            // With NOT
            bool canEdit = isLoggedIn && !isPremium || isAdmin; // (hypothetical)
            
            // Using parentheses for clarity
            bool canDelete = isLoggedIn && (isPremium || hasPermission);
            Console.WriteLine($"Can delete: {canDelete}"); // Output: Can delete: True

            // ═══════════════════════════════════════════════════════════════
            // SECTION: De Morgan's Laws
            // ═══════════════════════════════════════════════════════════════
            
            // NOT (A AND B) = (NOT A) OR (NOT B)
            bool p = true, q = false;
            bool leftSide = !(p && q);
            bool rightSide = !p || !q;
            Console.WriteLine($"NOT(p && q) = {!p || !q}"); // Output: NOT(p && q) = True
            
            // NOT (A OR B) = (NOT A) AND (NOT B)
            leftSide = !(p || q);
            rightSide = !p && !q;
            Console.WriteLine($"NOT(p || q) = {!p && !q}"); // Output: NOT(p || q) = False

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── Input validation ───────────────────────────────────────────
            string? username = "john";
            string? email = "john@example.com";
            string? password = "password123";
            
            bool isValid = !string.IsNullOrWhiteSpace(username) &&
                           !string.IsNullOrWhiteSpace(email) &&
                           !string.IsNullOrWhiteSpace(password) &&
                           password.Length >= 8;
            
            Console.WriteLine($"Registration valid: {isValid}"); // Output: Registration valid: True
            
            // ── Role-based access ──────────────────────────────────────────
            bool isUser = true;
            bool isModerator = false;
            bool isAdmin2 = false;
            
            bool canModerate = isUser && (isModerator || isAdmin2);
            Console.WriteLine($"Can moderate: {canModerate}"); // Output: Can moderate: False
            
            // ── Game state ────────────────────────────────────────────────
            bool isPlaying = true;
            bool isPaused = false;
            bool gameOver = false;
            
            bool isActive = isPlaying && !isPaused && !gameOver;
            Console.WriteLine($"Game active: {isActive}"); // Output: Game active: True
            
            // ── Business rules ─────────────────────────────────────────────
            decimal orderTotal = 150m;
            bool isPremiumMember = true;
            bool hasCoupon = false;
            bool freeShipping = orderTotal >= 100 || isPremiumMember;
            Console.WriteLine($"Free shipping: {freeShipping}"); // Output: Free shipping: True
        }
    }
}
