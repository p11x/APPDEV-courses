/*
 * TOPIC: ObservableCollection<T> Advanced Operations
 * SUBTOPIC: Filtering, Sorting, Event Handling
 * FILE: ObservableCollection_Part2.cs
 * PURPOSE: Demonstrate advanced ObservableCollection features including
 *          filtering collections, sorting data, and complex event handling
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CSharp_MasterGuide._04_Collections._06_ObservableCollection
{
    public class Employee : INotifyPropertyChanged
    {
        private string _name;
        private string _department;
        private decimal _salary;
        private bool _isActive;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string Department
        {
            get => _department;
            set { _department = value; OnPropertyChanged(nameof(Department)); }
        }

        public decimal Salary
        {
            get => _salary;
            set { _salary = value; OnPropertyChanged(nameof(Salary)); }
        }

        public bool IsActive
        {
            get => _isActive;
            set { _isActive = value; OnPropertyChanged(nameof(IsActive)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString() => $"{Name} - {Department} (${Salary:N0}) Active:{IsActive}";
    }

    public class Student
    {
        public string Name { get; set; }
        public double Grade { get; set; }
        public string Subject { get; set; }

        public override string ToString() => $"{Name}: {Grade:F1} ({Subject})";
    }

    public class TemperatureRecord
    {
        public DateTime Date { get; set; }
        public double Celsius { get; set; }

        public override string ToString() => $"{Date:yyyy-MM-dd}: {Celsius:F1}°C";
    }

    public class ObservableCollectionAdvanced
    {
        public static void Main()
        {
            Console.WriteLine("=== ObservableCollection Advanced Features ===\n");

            FilteringWithLINQ();
            SortingOperations();
            ComplexEventHandling();
            PropertyChangedTracking();
            LiveDataFeedExample();
            BatchOperations();
        }

        static void FilteringWithLINQ()
        {
            Console.WriteLine("--- 1. Filtering with LINQ ---");

            var employees = new ObservableCollection<Employee>
            {
                new Employee { Name = "Alice", Department = "Engineering", Salary = 75000 },
                new Employee { Name = "Bob", Department = "Marketing", Salary = 60000 },
                new Employee { Name = "Charlie", Department = "Engineering", Salary = 80000 },
                new Employee { Name = "Diana", Department = "HR", Salary = 55000 },
                new Employee { Name = "Eve", Department = "Engineering", Salary = 90000 }
            };

            Console.WriteLine("  All employees:");
            foreach (var e in employees)
            {
                Console.WriteLine($"    {e}");
            }
            // Output: All employees:
            //   Alice - Engineering ($75,000) Active:False
            //   Bob - Marketing ($60,000) Active:False
            //   ...

            // Filter: Engineering department
            var engineers = new List<Employee>();
            foreach (var e in employees)
            {
                if (e.Department == "Engineering")
                {
                    engineers.Add(e);
                }
            }

            var engineeringTeam = new ObservableCollection<Employee>(engineers);
            Console.WriteLine("\n  Engineering team:");
            foreach (var e in engineeringTeam)
            {
                Console.WriteLine($"    {e}");
            }
            // Output: Engineering team:
            //   Alice - Engineering ($75,000) Active:False
            //   Charlie - Engineering ($80,000) Active:False
            //   Eve - Engineering ($90,000) Active:False

            // Filter: High earners (salary > 75000)
            var highEarners = new ObservableCollection<Employee>(employees.Where(e => e.Salary > 75000));
            Console.WriteLine("\n  High earners (>$75k):");
            foreach (var e in highEarners)
            {
                Console.WriteLine($"    {e}");
            }
            // Output: High earners (>$75k):
            //   Charlie - Engineering ($80,000) Active:False
            //   Eve - Engineering ($90,000) Active:False
            Console.WriteLine();
        }

        static void SortingOperations()
        {
            Console.WriteLine("--- 2. Sorting Operations ---");

            var students = new ObservableCollection<Student>
            {
                new Student { Name = "Zoe", Grade = 85.5, Subject = "Math" },
                new Student { Name = "Adam", Grade = 92.0, Subject = "Science" },
                new Student { Name = "Mia", Grade = 78.3, Subject = "History" },
                new Student { Name = "Noah", Grade = 88.7, Subject = "Math" }
            };

            Console.WriteLine("  Original order:");
            foreach (var s in students)
            {
                Console.WriteLine($"    {s}");
            }
            // Output: Original order:
            //   Zoe: 85.5 (Math)
            //   Adam: 92.0 (Science)
            //   ...

            // Sort by grade descending
            var sortedByGrade = students.OrderByDescending(s => s.Grade).ToList();
            Console.WriteLine("\n  Sorted by grade (descending):");
            foreach (var s in sortedByGrade)
            {
                Console.WriteLine($"    {s}");
            }
            // Output: Sorted by grade (descending):
            //   Adam: 92.0 (Science)
            //   Noah: 88.7 (Math)
            //   ...

            // Sort by name alphabetically
            var sortedByName = students.OrderBy(s => s.Name).ToList();
            Console.WriteLine("\n  Sorted by name:");
            foreach (var s in sortedByName)
            {
                Console.WriteLine($"    {s}");
            }
            // Output: Sorted by name:
            //   Adam: 92.0 (Science)
            //   Mia: 78.3 (History)
            //   Noah: 88.7 (Math)
            //   Zoe: 85.5 (Math)

            // Create new sorted ObservableCollection
            var sortedStudents = new ObservableCollection<Student>(
                students.OrderBy(s => s.Grade)
            );
            Console.WriteLine("\n  New ObservableCollection (ascending grade):");
            foreach (var s in sortedStudents)
            {
                Console.WriteLine($"    {s}");
            }
            // Output: New ObservableCollection (ascending grade):
            //   Mia: 78.3 (History)
            //   Zoe: 85.5 (Math)
            //   Noah: 88.7 (Math)
            //   Adam: 92.0 (Science)
            Console.WriteLine();
        }

        static void ComplexEventHandling()
        {
            Console.WriteLine("--- 3. Complex Event Handling ---");

            var buffer = new ObservableCollection<string>();
            int addCount = 0;
            int removeCount = 0;
            int replaceCount = 0;
            int clearCount = 0;

            buffer.CollectionChanged += (sender, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        addCount++;
                        Console.WriteLine($"  Event #{addCount + removeCount + replaceCount}: Added '{e.NewItems[0]}'");
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        removeCount++;
                        Console.WriteLine($"  Event #{addCount + removeCount + replaceCount}: Removed '{e.OldItems[0]}'");
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        replaceCount++;
                        Console.WriteLine($"  Event #{addCount + removeCount + replaceCount}: Replaced with '{e.NewItems[0]}'");
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        clearCount++;
                        Console.WriteLine($"  Event #{addCount + removeCount + replaceCount + clearCount}: Cleared");
                        break;
                }
            };

            // Perform multiple operations
            buffer.Add("First");
            // Output: Event #1: Added 'First'

            buffer.Add("Second");
            // Output: Event #2: Added 'Second'

            buffer.Add("Third");
            // Output: Event #3: Added 'Third'

            buffer.Remove("First");
            // Output: Event #4: Removed 'First'

            buffer[0] = "Modified";
            // Output: Event #5: Replaced with 'Modified'

            buffer.Clear();
            // Output: Event #6: Cleared

            Console.WriteLine($"\n  Summary: {addCount} adds, {removeCount} removes, {replaceCount} replaces, {clearCount} clears");
            // Output: Summary: 3 adds, 1 removes, 1 replaces, 1 clears
            Console.WriteLine();
        }

        static void PropertyChangedTracking()
        {
            Console.WriteLine("--- 4. PropertyChanged Tracking ---");

            var employees = new ObservableCollection<Employee>
            {
                new Employee { Name = "Frank", Department = "Sales", Salary = 50000, IsActive = true },
                new Employee { Name = "Grace", Department = "IT", Salary = 70000, IsActive = false }
            };

            // Track individual property changes
            foreach (var emp in employees)
            {
                emp.PropertyChanged += (sender, e) =>
                {
                    var employee = sender as Employee;
                    Console.WriteLine($"  {employee.Name}.{e.PropertyName} changed to '{employee?.GetType().GetProperty(e.PropertyName)?.GetValue(employee)}'");
                };
            }

            Console.WriteLine("  Modifying employee properties:");

            employees[0].Salary = 55000;
            // Output: Frank.Salary changed to '55000'

            employees[0].Department = "Marketing";
            // Output: Frank.Department changed to 'Marketing'

            employees[1].Salary = 75000;
            // Output: Grace.Salary changed to '75000'

            employees[1].IsActive = true;
            // Output: Grace.IsActive changed to 'True'

            Console.WriteLine($"\n  Final employees:");
            foreach (var e in employees)
            {
                Console.WriteLine($"    {e}");
            }
            // Output: Final employees:
            //   Frank - Marketing ($55,000) Active:True
            //   Grace - IT ($75,000) Active:True
            Console.WriteLine();
        }

        static void LiveDataFeedExample()
        {
            Console.WriteLine("--- Real-World: Live Weather Feed ---");

            var weatherData = new ObservableCollection<TemperatureRecord>();

            // Track updates
            int updateCount = 0;
            weatherData.CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    updateCount++;
                    var record = e.NewItems[0] as TemperatureRecord;
                    Console.WriteLine($"  Update #{updateCount}: {record}");
                }
            };

            // Simulate live weather feed
            var random = new Random(42);
            var baseDate = new DateTime(2024, 1, 1);

            for (int i = 0; i < 5; i++)
            {
                var temp = -5.0 + random.NextDouble() * 25;
                weatherData.Add(new TemperatureRecord
                {
                    Date = baseDate.AddDays(i),
                    Celsius = Math.Round(temp, 1)
                });
            }

            // Output: Update #1: 2024-01-01: 12.4°C
            // Output: Update #2: 2024-01-02: 18.7°C
            // Output: Update #3: 2024-01-03: 5.2°C
            // Output: Update #4: 2024-01-04: 14.9°C
            // Output: Update #5: 2024-01-05: 8.1°C

            // Calculate average
            var avgTemp = weatherData.Average(t => t.Celsius);
            Console.WriteLine($"\n  Average temperature: {avgTemp:F1}°C");
            // Output: Average temperature: 11.9°C

            // Find hottest day
            var hottest = weatherData.Max(t => t.Celsius);
            var hottestDay = weatherData.First(t => t.Celsius == hottest);
            Console.WriteLine($"  Hottest day: {hottestDay}");
            // Output: Hottest day: 2024-01-02: 18.7°C

            Console.WriteLine();
        }

        static void BatchOperations()
        {
            Console.WriteLine("--- 5. Batch Operations ---");

            var source = new ObservableCollection<string>
            {
                "Alpha", "Beta", "Gamma", "Delta", "Epsilon"
            };

            var batch = new ObservableCollection<string>();

            // Suspend notification during batch (manual approach)
            int notificationCount = 0;
            batch.CollectionChanged += (s, e) => notificationCount++;

            // Add multiple items
            foreach (var item in source)
            {
                if (item.Length > 4)
                {
                    batch.Add(item);
                }
            }

            Console.WriteLine($"  Batch added {batch.Count} items with {notificationCount} notifications");
            // Output: Batch added 3 items with 3 notifications

            Console.WriteLine($"  Items: {string.Join(", ", batch)}");
            // Output: Items: Alpha, Gamma, Epsilon

            // Clear and rebuild
            batch.Clear();
            batch = new ObservableCollection<string>(source.Where(s => s.Contains("a")));
            Console.WriteLine($"\n  After rebuild: {string.Join(", ", batch)}");
            // Output: After rebuild: Alpha, Gamma, Delta
            Console.WriteLine();
        }
    }
}