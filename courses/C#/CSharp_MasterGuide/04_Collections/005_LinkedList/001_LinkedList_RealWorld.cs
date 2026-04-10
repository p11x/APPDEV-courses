/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : LinkedList<T> - Real-World Applications
 * FILE      : LinkedList_RealWorld.cs
 * PURPOSE   : Practical applications of LinkedList in real scenarios -
 *             music playlist, browser navigation history, transaction log
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._04_Collections._05_LinkedList
{
    public class Song
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public TimeSpan Duration { get; set; }

        public Song(string title, string artist, TimeSpan duration)
        {
            Title = title;
            Artist = artist;
            Duration = duration;
        }

        public override string ToString() => $"{Title} - {Artist} ({Duration:mm\\:ss})";
    }

    public class WebPage
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public DateTime VisitedAt { get; set; }

        public WebPage(string url, string title)
        {
            Url = url;
            Title = title;
            VisitedAt = DateTime.Now;
        }

        public override string ToString() => Title;
    }

    public class Transaction
    {
        public string TransactionId { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }

        public Transaction(string id, string type, decimal amount, string description)
        {
            TransactionId = id;
            Type = type;
            Amount = amount;
            Description = description;
            Timestamp = DateTime.Now;
        }

        public override string ToString() => $"[{Type}] {TransactionId}: {Description} - ${Amount:N2}";
    }

    class LinkedList_RealWorld
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== LinkedList Real-World Examples ===\n");

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 1: Music Playlist Manager
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("--- Scenario 1: Music Playlist ---");

            var playlist = new LinkedList<Song>();

            void AddSong(Song song) => playlist.AddLast(song);

            void PlayNext()
            {
                if (playlist.Count == 0)
                {
                    Console.WriteLine("Playlist is empty!");
                    return;
                }

                var current = playlist.First;
                Console.WriteLine($"Now playing: {current.Value}");
                playlist.RemoveFirst();
                playlist.AddLast(current.Value);
            }

            void SkipTo(string title)
            {
                var song = playlist.Find(title);
                if (song != null)
                {
                    Console.WriteLine($"Skipping to: {song.Value}");
                }
                else
                {
                    Console.WriteLine($"Song '{title}' not found");
                }
            }

            void ShufflePlay()
            {
                if (playlist.Count == 0) return;

                var random = new Random();
                var songs = new List<Song>(playlist);
                playlist.Clear();

                for (int i = songs.Count - 1; i > 0; i--)
                {
                    int j = random.Next(i + 1);
                    var temp = songs[i];
                    songs[i] = songs[j];
                    songs[j] = temp;
                }

                foreach (var song in songs)
                {
                    playlist.AddLast(song);
                }

                Console.WriteLine("Playlist shuffled!");
            }

            // Build playlist
            AddSong(new Song("Bohemian Rhapsody", "Queen", new TimeSpan(0, 5, 55)));
            AddSong(new Song("Stairway to Heaven", "Led Zeppelin", new TimeSpan(0, 8, 2)));
            AddSong(new Song("Hotel California", "Eagles", new TimeSpan(0, 6, 30)));
            AddSong(new Song("Sweet Child O' Mine", "Guns N' Roses", new TimeSpan(0, 5, 56)));
            AddSong(new Song("Imagine", "John Lennon", new TimeSpan(0, 3, 7)));

            Console.WriteLine($"Playlist created with {playlist.Count} songs");
            // Output: 5

            Console.WriteLine("Current playlist:");
            foreach (var song in playlist)
            {
                Console.WriteLine($"  {song}");
            }
            // Output: Lists all songs

            // Play next song (advances queue)
            Console.WriteLine($"\nPlaying next:");
            PlayNext();
            // Output: Bohemian Rhapsody

            Console.WriteLine($"After play - {playlist.Count} songs remaining");
            // Output: 4

            // Skip to specific song
            Console.WriteLine($"\nSkip to specific:");
            SkipTo("Imagine");
            // Output: Location updated

            // Add new song to end
            playlist.AddLast(new Song("Comfortably Numb", "Pink Floyd", new TimeSpan(0, 6, 22)));
            Console.WriteLine($"After adding new song: {playlist.Count}");
            // Output: 5

            // Shuffle playlist
            Console.WriteLine($"\nShuffle:");
            ShufflePlay();

            Console.WriteLine("Shuffled playlist:");
            foreach (var song in playlist)
            {
                Console.WriteLine($"  {song.Title}");
            }

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 2: Browser Navigation History
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n--- Scenario 2: Browser Navigation History ---");

            var history = new LinkedList<WebPage>();
            var currentPage = history.First;

            void NavigateTo(string url, string title)
            {
                var page = new WebPage(url, title);

                if (currentPage != null)
                {
                    var futureHistory = currentPage.Next;
                    while (futureHistory != null)
                    {
                        var toRemove = futureHistory;
                        futureHistory = futureHistory.Next;
                        history.Remove(toRemove);
                    }
                }

                history.AddLast(page);
                currentPage = history.Last;

                Console.WriteLine($"Navigated to: {page}");
            }

            void GoBack()
            {
                if (currentPage?.Previous != null)
                {
                    currentPage = currentPage.Previous;
                    Console.WriteLine($"Went back to: {currentPage.Value}");
                }
                else
                {
                    Console.WriteLine("Cannot go back - at beginning");
                }
            }

            void GoForward()
            {
                if (currentPage?.Next != null)
                {
                    currentPage = currentPage.Next;
                    Console.WriteLine($"Went forward to: {currentPage.Value}");
                }
                else
                {
                    Console.WriteLine("Cannot go forward - at end");
                }
            }

            void ShowHistory()
            {
                Console.WriteLine("Full history:");
                foreach (var page in history)
                {
                    bool isCurrent = page == currentPage.Value;
                    string marker = isCurrent ? " [CURRENT]" : "";
                    Console.WriteLine($"  {page.Title} ({page.Url}){marker}");
                }
            }

            // Simulate browsing
            NavigateTo("https://example.com", "Example Homepage");
            NavigateTo("https://example.com/products", "Products");
            NavigateTo("https://example.com/products/laptops", "Laptops");
            NavigateTo("https://example.com/products/laptops/thinkpad", "ThinkPad X1");

            Console.WriteLine($"History count: {history.Count}");
            // Output: 4

            ShowHistory();
            // Output: Shows all visited pages with current indicator

            // Go back twice
            Console.WriteLine($"\nGoing back:");
            GoBack();
            // Output: Laptops page
            GoBack();
            // Output: Products page

            // Navigate to new page (clears forward history)
            NavigateTo("https://example.com/about", "About Us");

            Console.WriteLine($"\nAfter new navigation:");
            ShowHistory();
            // Output: Forward history cleared

            // Go forward
            Console.WriteLine($"\nGoing forward:");
            GoForward();
            // Output: About Us

            // Check if can go back/forward
            Console.WriteLine($"Can go back: {currentPage?.Previous != null}");
            // Output: True
            Console.WriteLine($"Can go forward: {currentPage?.Next != null}");
            // Output: False (at last page)

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 3: Transaction History / Audit Log
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n--- Scenario 3: Transaction History ---");

            var transactions = new LinkedList<Transaction>();

            void RecordTransaction(Transaction tx) => transactions.AddLast(tx);

            Transaction[] sampleTransactions = {
                new Transaction("TXN001", "DEPOSIT", 1000.00m, "Salary Deposit"),
                new Transaction("TXN002", "WITHDRAWAL", -50.00m, "ATM Withdrawal"),
                new Transaction("TXN003", "TRANSFER", -250.00m, "Transfer to Savings"),
                new Transaction("TXN004", "PAYMENT", -75.50m, "Utility Bill"),
                new Transaction("TXN005", "DEPOSIT", 500.00m, "Refund"),
                new Transaction("TXN006", "TRANSFER", -100.00m, "Transfer from Checking"),
            };

            foreach (var tx in sampleTransactions)
            {
                RecordTransaction(tx);
            }

            Console.WriteLine($"Total transactions: {transactions.Count}");
            // Output: 6

            // Show all transactions
            Console.WriteLine("All transactions:");
            foreach (var tx in transactions)
            {
                Console.WriteLine($"  {tx}");
            }
            // Output: Lists all transactions

            // Find transaction by ID
            var findTxn = transactions.Find(new Transaction("TXN003", "", 0, ""));
            Console.WriteLine($"\nLooking up TXN003...");

            // Manual lookup since we can't search by ID easily
            var searchResult = transactions.First;
            while (searchResult != null)
            {
                if (searchResult.Value.TransactionId == "TXN003")
                {
                    Console.WriteLine($"Found: {searchResult.Value}");
                    break;
                }
                searchResult = searchResult.Next;
            }
            // Output: TXN003: Transfer to Savings - $250.00

            // Get recent transactions (last 3)
            Console.WriteLine($"\nRecent transactions (last 3):");
            var recentNode = transactions.Last;
            int recentCount = 0;
            while (recentNode != null && recentCount < 3)
            {
                Console.WriteLine($"  {recentNode.Value}");
                recentNode = recentNode.Previous;
                recentCount++;
            }
            // Output: Last 3 transactions

            // Calculate running balance
            Console.WriteLine($"\nRunning balance:");
            decimal balance = 0;
            var balanceNode = transactions.First;
            while (balanceNode != null)
            {
                balance += balanceNode.Value.Amount;
                Console.WriteLine($"  {balanceNode.Value.TransactionId}: ${balance:N2} (balance after)");
                balanceNode = balanceNode.Next;
            }
            // Output: Shows running balance

            // Undo last transaction (reverse withdrawal)
            Console.WriteLine($"\n--- Undo Last Transaction ---");
            var toReverse = transactions.Last;
            if (toReverse != null)
            {
                var reversedAmount = -toReverse.Value.Amount;
                var newTx = new Transaction(
                    "TXN" + (transactions.Count + 1).ToString("D3"),
                    "REVERSAL",
                    reversedAmount,
                    $"Reversed: {toReverse.Value.Description}"
                );
                transactions.AddLast(newTx);
                Console.WriteLine($"Reversed: {toReverse.Value.TransactionId}");
            }

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 4: Undo/Redo System
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n--- Scenario 4: Undo/Redo System ---");

            var actionHistory = new LinkedList<string>();
            var currentAction = actionHistory.Last;

            void PerformAction(string action)
            {
                if (currentAction?.Next != null)
                {
                    var future = currentAction.Next;
                    while (future != null)
                    {
                        var toRemove = future;
                        future = future.Next;
                        actionHistory.Remove(toRemove);
                    }
                }

                actionHistory.AddLast(action);
                currentAction = actionHistory.Last;

                Console.WriteLine($"Action: {action}");
            }

            void Undo()
            {
                if (currentAction?.Previous != null)
                {
                    var undone = currentAction.Value;
                    currentAction = currentAction.Previous;
                    Console.WriteLine($"Undone: {undone}");
                }
                else
                {
                    Console.WriteLine("Nothing to undo");
                }
            }

            void Redo()
            {
                if (currentAction?.Next != null)
                {
                    currentAction = currentAction.Next;
                    Console.WriteLine($"Redone: {currentAction.Value}");
                }
                else
                {
                    Console.WriteLine("Nothing to redo");
                }
            }

            // Perform actions
            PerformAction("Type 'Hello'");
            PerformAction("Type ' World'");
            PerformAction("Bold text");
            PerformAction("Change font");

            Console.WriteLine($"History count: {actionHistory.Count}");
            // Output: 4

            // Undo
            Console.WriteLine($"\nUndoing:");
            Undo();
            // Output: Change font
            Undo();
            // Output: Bold text

            Console.WriteLine($"History count: {actionHistory.Count}");
            // Output: 2

            // Redo
            Console.WriteLine($"\nRedoing:");
            Redo();
            // Output: Bold text

            // New action clears redo history
            PerformAction("Underline text");

            Console.WriteLine($"After new action - can redo Bold: {currentAction?.Next != null}");
            // Output: False (redo cleared)

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 5: Employee Directory (by seniority)
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n--- Scenario 5: Employee Seniority List ---");

            var employees = new LinkedList<(string Name, int Years)>();

            void AddEmployeeBySeniority(string name, int years)
            {
                var newEmployee = (name, years);

                if (employees.Count == 0 || years >= employees.Last.Value.Years)
                {
                    employees.AddLast(newEmployee);
                    return;
                }

                var current = employees.First;
                while (current != null && current.Value.Years > years)
                {
                    current = current.Next;
                }

                if (current != null)
                {
                    employees.AddBefore(current, newEmployee);
                }
                else
                {
                    employees.AddLast(newEmployee);
                }
            }

            // Add employees in random order
            AddEmployeeBySeniority("Alice", 3);
            AddEmployeeBySeniority("Bob", 8);
            AddEmployeeBySeniority("Carol", 1);
            AddEmployeeBySeniority("David", 5);
            AddEmployeeBySeniority("Eve", 10);

            Console.WriteLine("Employees by seniority (most to least):");
            foreach (var emp in employees)
            {
                Console.WriteLine($"  {emp.Name}: {emp.Years} years");
            }
            // Output: Eve(10), Bob(8), David(5), Alice(3), Carol(1)

            // Promote employee (move up in list)
            var promotedEmployee = employees.Find(("Alice", 3));
            if (promotedEmployee != null)
            {
                employees.Remove(promotedEmployee);
                employees.AddBefore(employees.Find(("Carol", 1)), ("Alice", 4));
            }

            Console.WriteLine($"\nAfter Alice promoted:");
            foreach (var emp in employees)
            {
                Console.WriteLine($"  {emp.Name}: {emp.Years} years");
            }
            // Output: Eve(10), Bob(8), David(5), Alice(4), Carol(1)

            Console.WriteLine("\n=== Real-World Examples Complete ===");
        }
    }
}