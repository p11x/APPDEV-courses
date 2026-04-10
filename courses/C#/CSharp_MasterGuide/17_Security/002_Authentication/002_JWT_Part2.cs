/*
 * ============================================================
 * TOPIC     : Security
 * SUBTOPIC  : JWT Part 2 - Advanced
 * FILE      : JWT_Part2.cs
 * PURPOSE   : Advanced JWT techniques
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._17_Security._02_Authentication
{
    /// <summary>
    /// Advanced JWT demonstration
    /// </summary>
    public class JWTPart2Demo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== JWT Part 2 ===\n");

            // Output: --- Refresh Tokens ---
            Console.WriteLine("--- Refresh Tokens ---");

            var refreshToken = GenerateRefreshToken();
            var newAccessToken = RefreshAccessToken(refreshToken);
            Console.WriteLine($"   New access token: {newAccessToken.Substring(0, 20)}...");
            // Output: New access token: [token]

            // Output: --- Token Storage ---
            Console.WriteLine("\n--- Token Storage ---");

            Console.WriteLine("   Store in memory (SPA)");
            Console.WriteLine("   HttpOnly cookie (MVC)");
            // Output: Store in memory (SPA)
            // Output: HttpOnly cookie (MVC)

            // Output: --- Token Revocation ---
            Console.WriteLine("\n--- Token Revocation ---");

            RevokeToken("token123");
            Console.WriteLine("   Token revoked");
            // Output: Token revoked

            // Output: --- Token Introspection ---
            Console.WriteLine("\n--- Token Introspection ---");

            var info = IntrospectToken("token123");
            Console.WriteLine($"   Active: {info.Active}");
            Console.WriteLine($"   Username: {info.Username}");
            // Output: Active: True
            // Output: Username: Alice

            Console.WriteLine("\n=== JWT Part 2 Complete ===");
        }
    }

    /// <summary>
    /// Generate refresh token
    /// </summary>
    public static string GenerateRefreshToken()
    {
        return "refresh_token_value";
    }

    /// <summary>
    /// Exchange refresh token for access token
    /// </summary>
    public static string RefreshAccessToken(string refreshToken)
    {
        return "new_access_token";
    }

    /// <summary>
    /// Revoke token
    /// </summary>
    public static void RevokeToken(string token)
    {
    }

    /// <summary>
    /// Token introspection result
    /// </summary>
    public class TokenInfo
    {
        public bool Active { get; set; } = true;
        public string Username { get; set; } = "Alice";
    }

    /// <summary>
    /// Introspect token
    /// </summary>
    public static TokenInfo IntrospectToken(string token)
    {
        return new TokenInfo();
    }
}