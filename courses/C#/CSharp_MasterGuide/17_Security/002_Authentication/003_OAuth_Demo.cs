/*
 * ============================================================
 * TOPIC     : Security
 * SUBTOPIC  : OAuth 2.0
 * FILE      : OAuth_Demo.cs
 * PURPOSE   : OAuth authorization framework
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._17_Security._02_Authentication
{
    /// <summary>
    /// OAuth demonstration
    /// </summary>
    public class OAuthDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== OAuth 2.0 Demo ===\n");

            // Output: --- Authorization Code Flow ---
            Console.WriteLine("--- Authorization Code Flow ---");

            // Step 1: Redirect to authorization server
            var authUrl = BuildAuthorizationUrl("client_id", "redirect_uri");
            Console.WriteLine($"   Auth URL: {authUrl}");
            // Output: Auth URL: https://auth.example.com/authorize

            // Step 2: User authorizes
            Console.WriteLine("   User authorizes");
            // Output: User authorizes

            // Step 3: Receive authorization code
            var code = "authorization_code";
            Console.WriteLine($"   Code: {code}");
            // Output: Code: authorization_code

            // Step 4: Exchange code for token
            var token = ExchangeCodeForToken(code, "client_id");
            Console.WriteLine($"   Token: {token.Substring(0, 20)}...");
            // Output: Token: [access token]

            // Output: --- Client Credentials ---
            Console.WriteLine("\n--- Client Credentials ---");

            var clientToken = ClientCredentialsGrant("client_id", "client_secret");
            Console.WriteLine($"   Client token: {clientToken.Substring(0, 20)}...");
            // Output: Client token: [token]

            // Output: --- OAuth Scopes ---
            Console.WriteLine("\n--- OAuth Scopes ---");

            Console.WriteLine("   read:user - Read user profile");
            Console.WriteLine("   write:user - Modify user profile");
            Console.WriteLine("   read:orders - Read orders");
            // Output: read:user - Read user profile
            // Output: write:user - Modify user profile
            // Output: read:orders - Read orders

            // Output: --- PKCE Flow ---
            Console.WriteLine("\n--- PKCE Flow ---");

            var codeVerifier = GenerateCodeVerifier();
            var codeChallenge = GenerateCodeChallenge(codeVerifier);
            Console.WriteLine("   PKCE code verifier and challenge generated");
            // Output: PKCE code verifier and challenge generated

            Console.WriteLine("\n=== OAuth Complete ===");
        }
    }

    /// <summary>
    /// Build authorization URL
    /// </summary>
    public static string BuildAuthorizationUrl(string clientId, string redirectUri)
    {
        return $"https://auth.example.com/authorize?client_id={clientId}&redirect_uri={redirectUri}";
    }

    /// <summary>
    /// Exchange authorization code for token
    /// </summary>
    public static string ExchangeCodeForToken(string code, string clientId)
    {
        return "access_token_value";
    }

    /// <summary>
    /// OAuth client credentials grant
    /// </summary>
    public static string ClientCredentialsGrant(string clientId, string clientSecret)
    {
        return "access_token_value";
    }

    /// <summary>
    /// Generate PKCE code verifier
    /// </summary>
    public static string GenerateCodeVerifier()
    {
        return "code_verifier_value";
    }

    /// <summary>
    /// Generate PKCE code challenge
    /// </summary>
    public static string GenerateCodeChallenge(string verifier)
    {
        return "code_challenge_value";
    }
}