/*
================================================================================
TOPIC 31: .NET VS .NET FRAMEWORK VS .NET CORE
================================================================================

Understanding the .NET ecosystem and the differences between platforms.

TABLE OF CONTENTS:
1. .NET Framework
2. .NET Core
3. .NET (5, 6, 7, 8)
4. .NET Standard
5. Choosing the Right Platform
================================================================================
*/

// ================================================================================
// SECTION 1: .NET FRAMEWORK
// =============================================================================

/*
.NET FRAMEWORK:
---------------
- Released in 2002
- Windows-only
- Mature and stable
- Large ecosystem of libraries
- Not open source
- Will continue to be supported

Use when:
- Legacy Windows applications
- Windows-specific features needed
- Existing .NET Framework projects
*/

// ================================================================================
// SECTION 2: .NET CORE
// =============================================================================

/*
.NET CORE:
----------
- Released in 2016
- Cross-platform (Windows, Linux, macOS)
- Open source
- High performance
- Modular architecture

Use when:
- Cross-platform needed
- Microservices
- Docker containers
- High performance required
*/

// ================================================================================
// SECTION 3: .NET (MODERN)
// =============================================================================

/*
.NET 5, 6, 7, 8:
-----------------
- .NET 5 (2020): Unified platform, first cross-platform .NET
- .NET 6 (2021): LTS, Blazor, minimal APIs
- .NET 7 (2022): Performance improvements
- .NET 8 (2023): Latest LTS, AI integration

Benefits:
- Single codebase
- Cross-platform
- Modern features
- Performance
- Active development
*/

namespace DotNetComparison
{
    class Program
    {
        // Runtime information
        static void Main(string[] args)
        {
            // Environment information
            Console.WriteLine("=== .NET Runtime Info ===");
            Console.WriteLine($"Framework: {Environment.Version}");
            Console.WriteLine($"OS: {Environment.OSVersion}");
            Console.WriteLine($"Processor Count: {Environment.ProcessorCount}");
            Console.WriteLine($"Is 64-bit: {Environment.Is64BitOperatingSystem}");
            
            // .NET version (for .NET Core 3.1+)
            // Console.WriteLine($"Runtime: {System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier}");
        }
    }
}

/*
CHOOSING .NET PLATFORM:
-----------------------
| Need                    | Use            |
|------------------------|----------------|
| New cross-platform     | .NET 6/7/8    |
| Windows-only legacy   | .NET Framework |
| Web API development   | ASP.NET Core   |
| Microservices         | .NET + Docker  |
| Enterprise apps       | .NET 6/7/8     |
*/

// ================================================================================
// INTERVIEW QUESTIONS
// =============================================================================

/*
Q1: What is the difference between .NET Framework and .NET Core?
A: .NET Framework is Windows-only, .NET Core is cross-platform and open source.
   .NET (5+) unifies both into a single platform.

Q2: What is .NET Standard?
A: A specification for API compatibility across .NET implementations.
   Libraries targeting .NET Standard work on all .NET platforms.

Q3: Why choose .NET 6/7/8 for new projects?
A: Cross-platform, better performance, modern features, active development,
   and long-term support.
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 32 covers ASP.NET Core basics.
*/
