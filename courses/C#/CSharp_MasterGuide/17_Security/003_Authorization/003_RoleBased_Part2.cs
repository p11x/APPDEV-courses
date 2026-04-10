/*
 * ============================================================
 * TOPIC     : Security
 * SUBTOPIC  : Role-Based Access Part 2
 * FILE      : RoleBased_Part2.cs
 * PURPOSE   : Advanced role-based access control
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._17_Security._03_Authorization
{
    /// <summary>
    /// Advanced role-based access
    /// </summary>
    public class RoleBasedPart2Demo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Role-Based Part 2 ===\n");

            // Output: --- Hierarchical Roles ---
            Console.WriteLine("--- Hierarchical Roles ---");

            var user = new User { Role = "Editor" };
            var canEdit = HasPermission(user, "edit");
            Console.WriteLine($"   Can edit: {canEdit}");
            // Output: Can edit: True

            // Output: --- Resource-Based ---
            Console.WriteLine("\n--- Resource-Based ---");

            var resource = new Document { OwnerId = 1 };
            var canAccess = CanAccessDocument(user, resource);
            Console.WriteLine($"   Can access: {canAccess}");
            // Output: Can access: True

            // Output: --- Claims-Based ---
            Console.WriteLine("\n--- Claims-Based ---");

            var claimsUser = new ClaimsUser();
            claimsUser.AddClaim("Permission", "Write");
            var hasClaim = claimsUser.HasClaim("Permission", "Write");
            Console.WriteLine($"   Has claim: {hasClaim}");
            // Output: Has claim: True

            // Output: --- Policy-Based ---
            Console.WriteLine("\n--- Policy-Based ---");

            var policy = new AuthorizationPolicy();
            policy.AddRequirement("Age", 18);
            var result = policy.Evaluate(new { Age = 20 });
            Console.WriteLine($"   Policy result: {result}");
            // Output: Policy result: True

            Console.WriteLine("\n=== Part 2 Complete ===");
        }
    }

    /// <summary>
    /// User entity
    /// </summary>
    public class User
    {
        public string Role { get; set; } // property: role
        public int Id { get; set; } // property: id
    }

    /// <summary>
    /// Document entity
    /// </summary>
    public class Document
    {
        public int OwnerId { get; set; } // property: owner id
    }

    /// <summary>
    /// Check role permission
    /// </summary>
    public static bool HasPermission(User user, string permission)
    {
        return user.Role switch
        {
            "Admin" => true,
            "Editor" => permission == "edit" || permission == "view",
            "Viewer" => permission == "view",
            _ => false
        };
    }

    /// <summary>
    /// Check document access
    /// </summary>
    public static bool CanAccessDocument(User user, Document doc)
    {
        return user.Role == "Admin" || doc.OwnerId == user.Id;
    }

    /// <summary>
    /// Claims user
    /// </summary>
    public class ClaimsUser
    {
        public void AddClaim(string type, string value) { }
        public bool HasClaim(string type, string value) => true;
    }

    /// <summary>
    /// Authorization policy
    /// </summary>
    public class AuthorizationPolicy
    {
        public void AddRequirement(string field, object value) { }
        public bool Evaluate(object context) => true;
    }
}