/*
================================================================================
TOPIC 39: AUTHENTICATION AND AUTHORIZATION
================================================================================

Securing ASP.NET Core applications with authentication and authorization.

TABLE OF CONTENTS:
1. Authentication
2. Authorization
3. JWT Tokens
4. Policy-based Authorization
5. Roles
================================================================================
*/

namespace AuthConcepts
{
    // ====================================================================
    // AUTHENTICATION
    // ====================================================================
    
    // Example: Configuring authentication
    /*
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "myissuer",
                ValidAudience = "myaudience",
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("mysecretkey"))
            };
        });
    */
    
    // ====================================================================
    // AUTHORIZATION
    // ====================================================================
    
    // Example: Controller authorization
    /*
    [Authorize]
    public class SecureController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly() => Ok("Admin content");
        
        [Authorize(Policy = "RequireElevatedPrivileges")]
        public IActionResult Elevated() => Ok("Elevated content");
        
        [AllowAnonymous]
        public IActionResult Public() => Ok("Public content");
    }
    */
    
    // ====================================================================
    // POLICY EXAMPLE
    // ====================================================================
    
    /*
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireElevatedPrivileges", policy =>
            policy.RequireRole("Admin")
                  .RequireClaim("Privilege", "Elevated"));
        
        options.AddPolicy("AtLeast21", policy =>
            policy.Requirements.Add(new MinimumAgeRequirement(21)));
    });
    */
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Authentication & Authorization ===");
            
            Console.WriteLine("\nAuthentication Schemes:");
            Console.WriteLine("- Cookie Authentication");
            Console.WriteLine("- JWT Bearer (JSON Web Tokens)");
            Console.WriteLine("- OAuth 2.0 / OpenID Connect");
            Console.WriteLine("- Windows Authentication");
            Console.WriteLine("- External Providers (Google, Facebook)");
            
            Console.WriteLine("\nAuthorization:");
            Console.WriteLine("- Role-based (Authorize(Roles = \"Admin\"))");
            Console.WriteLine("- Policy-based (complex rules)");
            Console.WriteLine("- Claims-based");
            
            Console.WriteLine("\nJWT Token Contains:");
            Console.WriteLine("- Header (algorithm, type)");
            Console.WriteLine("- Payload (claims)");
            Console.WriteLine("- Signature (verification)");
        }
    }
}

/*
SECURITY BEST PRACTICES:
------------------------
- Use HTTPS always
- Validate and sanitize input
- Use parameterized queries
- Hash passwords (BCrypt, PBKDF2)
- Implement rate limiting
- Use secure cookies
- Enable CORS properly
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 40 covers Microservices.
*/
