/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : HashSet/SortedSet - Real-World Examples
 * FILE      : HashSet_RealWorld.cs
 * PURPOSE   : Practical applications of HashSet and SortedSet in
 *             real-world scenarios - deduplication, membership,
 *             set operations, and data filtering
 * ============================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._04_Collections._04_HashSet_SortedSet
{
    class HashSet_RealWorld
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== HashSet & SortedSet Real-World Examples ===\n");

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 1: Removing Duplicates from User Input
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("--- Scenario 1: Deduplicating User Input ---");

            // Simulating user entering email addresses (with duplicates)
            List<string> userEmails = new List<string>
            {
                "john@example.com",
                "jane@example.com",
                "john@example.com",  // Duplicate
                "bob@example.com",
                "jane@example.com", // Duplicate
                "alice@example.com"
            };

            // Use HashSet to get unique emails
            var uniqueEmails = new HashSet<string>(userEmails);

            Console.WriteLine($"Input emails ({userEmails.Count}): {string.Join(", ", userEmails)}");
            Console.WriteLine($"Unique emails ({uniqueEmails.Count}): {string.Join(", ", uniqueEmails)}");
            // Output: 4 unique emails

            // Preserve original order while removing duplicates
            var orderedUnique = new List<string>();
            var seen = new HashSet<string>();

            foreach (var email in userEmails)
            {
                if (seen.Add(email))
                {
                    orderedUnique.Add(email);
                }
            }

            Console.WriteLine($"Ordered unique: {string.Join(", ", orderedUnique)}");
            // Output: john@example.com, jane@example.com, bob@example.com, alice@example.com

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 2: Finding Common Tags Between Articles
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n--- Scenario 2: Finding Common Article Tags ---");

            HashSet<string> article1Tags = new HashSet<string>
            {
                "C#", "Programming", "Tutorial", "Beginner", "Web"
            };

            HashSet<string> article2Tags = new HashSet<string>
            {
                "C#", "Programming", "Advanced", "Performance", "Web"
            };

            // Find common tags
            var commonTags = new HashSet<string>(article1Tags);
            commonTags.IntersectWith(article2Tags);

            Console.WriteLine($"Article 1 tags: {string.Join(", ", article1Tags)}");
            Console.WriteLine($"Article 2 tags: {string.Join(", ", article2Tags)}");
            Console.WriteLine($"Common tags: {string.Join(", ", commonTags)}");
            // Output: C#, Programming, Web

            // Find unique to each article
            var uniqueToArticle1 = new HashSet<string>(article1Tags);
            uniqueToArticle1.ExceptWith(article2Tags);

            var uniqueToArticle2 = new HashSet<string>(article2Tags);
            uniqueToArticle2.ExceptWith(article1Tags);

            Console.WriteLine($"Unique to Article 1: {string.Join(", ", uniqueToArticle1)}");
            // Output: Tutorial, Beginner
            Console.WriteLine($"Unique to Article 2: {string.Join(", ", uniqueToArticle2)}");
            // Output: Advanced, Performance

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 3: User Permission System
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n--- Scenario 3: User Permission System ---");

            // Define permission sets
            HashSet<string> adminPermissions = new HashSet<string>
            {
                "read", "write", "delete", "manage_users", "view_logs", "manage_settings"
            };

            HashSet<string> editorPermissions = new HashSet<string>
            {
                "read", "write", "edit_own", "view_logs"
            };

            HashSet<string> viewerPermissions = new HashSet<string>
            {
                "read", "view_logs"
            };

            // Check if editor can delete
            bool canDelete = editorPermissions.Contains("delete");
            Console.WriteLine($"Editor can delete: {canDelete}");
            // Output: False

            // Check if admin has all editor permissions
            bool hasAllEditorPerms = adminPermissions.IsSupersetOf(editorPermissions);
            Console.WriteLine($"Admin has all editor permissions: {hasAllEditorPerms}");
            // Output: True

            // Check permission overlap between roles
            bool hasOverlap = adminPermissions.Overlaps(viewerPermissions);
            Console.WriteLine($"Admin/Viewer have overlapping permissions: {hasOverlap}");
            // Output: True

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 4: Inventory - Unique Product IDs
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n--- Scenario 4: Inventory Management ---");

            // Track unique product IDs
            HashSet<string> productIds = new HashSet<string>();

            // Add products
            string[] newProducts = { "PROD-001", "PROD-002", "PROD-003", "PROD-001" };

            foreach (var productId in newProducts)
            {
                if (productIds.Add(productId))
                {
                    Console.WriteLine($"Added: {productId}");
                }
                else
                {
                    Console.WriteLine($"Duplicate rejected: {productId}");
                }
            }

            Console.WriteLine($"Total unique products: {productIds.Count}");
            // Output: 3

            // Check if product exists
            Console.WriteLine($"Has PROD-002: {productIds.Contains("PROD-002")}");
            Console.WriteLine($"Has PROD-999: {productIds.Contains("PROD-999")}");
            // Output: True, False

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 5: Sorted Leaderboard with Unique Scores
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n--- Scenario 5: Game Leaderboard ---");

            // SortedSet maintains uniqueness and sorted order
            SortedSet<int> leaderboard = new SortedSet<int>(Comparer<int>.Create((a, b) => b.CompareTo(a)));

            // Add scores (duplicates ignored automatically)
            leaderboard.Add(5000);
            leaderboard.Add(8500);
            leaderboard.Add(3000);
            leaderboard.Add(8500); // Duplicate - ignored
            leaderboard.Add(12000);
            leaderboard.Add(6000);

            Console.WriteLine("Top Scores (sorted high to low):");
            int rank = 1;
            foreach (var score in leaderboard)
            {
                Console.WriteLine($"  #{rank++}: {score:N0}");
            }
            // Output: #1: 12000, #2: 8500, #3: 6000, #4: 5000, #5: 3000

            // Get top 3
            var top3 = leaderboard.Take(3).ToList();
            Console.WriteLine($"Top 3: {string.Join(", ", top3)}");
            // Output: 12000, 8500, 6000

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 6: Date Range Tracking with SortedSet
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n--- Scenario 6: Event Date Scheduler ---");

            SortedSet<DateTime> scheduledDates = new SortedSet<DateTime>();

            // Add booking dates
            DateTime[] bookings = {
                new DateTime(2024, 3, 15),
                new DateTime(2024, 1, 10),
                new DateTime(2024, 3, 15), // Duplicate
                new DateTime(2024, 6, 20),
                new DateTime(2024, 12, 25),
                new DateTime(2024, 9, 1)
            };

            foreach (var date in bookings)
            {
                scheduledDates.Add(date);
            }

            Console.WriteLine($"Total unique booking dates: {scheduledDates.Count}");
            // Output: 5

            Console.WriteLine("Upcoming events (sorted chronologically):");
            foreach (var date in scheduledDates)
            {
                Console.WriteLine($"  {date:yyyy-MM-dd}");
            }
            // Output: 2024-01-10, 2024-03-15, 2024-06-20, 2024-09-01, 2024-12-25

            // Get events in first half of year
            var firstHalf = scheduledDates.GetViewBetween(
                new DateTime(2024, 1, 1),
                new DateTime(2024, 6, 30)
            );

            Console.WriteLine("First half events:");
            foreach (var date in firstHalf)
            {
                Console.WriteLine($"  {date:yyyy-MM-dd}");
            }
            // Output: 2024-01-10, 2024-03-15, 2024-06-20

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 7: Word Frequency with Set Operations
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n--- Scenario 7: Word Analysis ---");

            string text1 = "The quick brown fox jumps over the lazy dog";
            string text2 = "The lazy dog is sleeping in the sun";

            // Extract unique words from each text
            HashSet<string> words1 = new HashSet<string>(
                text1.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries)
            );

            HashSet<string> words2 = new HashSet<string>(
                text2.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries)
            );

            Console.WriteLine($"Text 1 unique words ({words1.Count}): {string.Join(", ", words1)}");
            Console.WriteLine($"Text 2 unique words ({words2.Count}): {string.Join(", ", words2)}");

            // Words in both texts
            var commonWords = new HashSet<string>(words1);
            commonWords.IntersectWith(words2);
            Console.WriteLine($"Common words: {string.Join(", ", commonWords)}");
            // Output: the, lazy, dog

            // Words unique to text1
            var uniqueWords1 = new HashSet<string>(words1);
            uniqueWords1.ExceptWith(words2);
            Console.WriteLine($"Only in text 1: {string.Join(", ", uniqueWords1)}");
            // Output: quick, brown, fox, jumps, over

            // All unique words combined
            var allWords = new HashSet<string>(words1);
            allWords.UnionWith(words2);
            Console.WriteLine($"All unique words ({allWords.Count}): {string.Join(", ", allWords)}");
            // Output: 13 unique words

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 8: Rate Limiting with Membership Test
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n--- Scenario 8: API Rate Limiting ---");

            // Track unique API keys accessing in current window
            HashSet<string> activeApiKeys = new HashSet<string>();
            int maxRequestsPerMinute = 100;

            // Simulate incoming requests
            string[] incomingRequests = { "key1", "key2", "key1", "key3", "key1", "key2" };

            foreach (var apiKey in incomingRequests)
            {
                if (activeApiKeys.Count < maxRequestsPerMinute)
                {
                    if (activeApiKeys.Add(apiKey))
                    {
                        Console.WriteLine($"Request accepted from: {apiKey}");
                    }
                    else
                    {
                        Console.WriteLine($"Request from {apiKey} - already counted this minute");
                    }
                }
                else
                {
                    Console.WriteLine($"Rate limit exceeded for: {apiKey}");
                }
            }

            Console.WriteLine($"Unique API keys this minute: {activeApiKeys.Count}");
            // Output: 3

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 9: Filter Excluded Items
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n--- Scenario 9: Filtering Excluded Items ---");

            // Available products
            List<string> allProducts = new List<string>
            {
                "Laptop", "Phone", "Tablet", "Watch", "Headphones",
                "Camera", "Speaker", "Monitor", "Keyboard", "Mouse"
            };

            // Excluded due to out of stock
            HashSet<string> outOfStock = new HashSet<string> { "Tablet", "Camera", "Speaker" };

            // Excluded due to discontinued
            HashSet<string> discontinued = new HashSet<string> { "Watch", "Monitor" };

            // Combine all exclusions
            HashSet<string> excludedItems = new HashSet<string>(outOfStock);
            excludedItems.UnionWith(discontinued);

            // Get available products
            var availableProducts = allProducts
                .Where(p => !excludedItems.Contains(p))
                .ToList();

            Console.WriteLine($"All products ({allProducts.Count}): {string.Join(", ", allProducts)}");
            Console.WriteLine($"Excluded: {string.Join(", ", excludedItems)}");
            Console.WriteLine($"Available ({availableProducts.Count}): {string.Join(", ", availableProducts)}");
            // Output: Laptop, Phone, Headphones, Keyboard, Mouse

            Console.WriteLine("\n=== Real-World Examples Complete ===");
        }
    }
}
