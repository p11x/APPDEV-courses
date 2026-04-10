/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Generics - Covariance and Contravariance Part 2
 * FILE      : Covariance_Contravariance_Part2.cs
 * PURPOSE   : Teaches practical covariance/contravariance examples,
 *            common interfaces, and real-world applications
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._01_Generics
{
    class Covariance_Contravariance_Part2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Covariance and Contravariance Part 2 ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: IEnumerable<T> - Built-in Covariance
            // ═══════════════════════════════════════════════════════════

            // IEnumerable<T> is defined as IEnumerable<out T>
            // This allows implicit conversion from IEnumerable<Derived> to IEnumerable<Base>

            List<Cat> cats = new List<Cat> { new Cat(), new Cat() };
            IEnumerable<Animal> animals = cats; // Covariance works!

            Console.WriteLine($"IEnumerable<Cat> converted to IEnumerable<Animal>");
            // Output: IEnumerable<Cat> converted to IEnumerable<Animal>

            // LINQ methods preserve covariance
            IEnumerable<Animal> filteredAnimals = cats.Where(c => c.Name == "Test");
            Console.WriteLine($"Filtered cats as animals: {filteredAnimals.GetType().Name}");
            // Output: Filtered cats as animals: <FilteredEnumerable>

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: IComparer<T> - Built-in Contravariance
            // ═══════════════════════════════════════════════════════════

            // IComparer<T> is defined as IComparer<in T>
            // Allows comparing derived types using base comparer

            IComparer<Animal> animalComparer = new SizeComparer();
            IComparer<Dog> dogComparer = animalComparer; // Contravariance

            Dog d1 = new Dog { Name = "Rex" };
            Dog d2 = new Dog { Name = "Buddy" };
            int result = dogComparer.Compare(d1, d2);
            Console.WriteLine($"Comparison result: {result}");
            // Output: Comparison result: 0

            // Array.Sort uses IComparer<T>
            Animal[] animalArray = { new Dog { Name = "A" }, new Cat { Name = "B" } };
            Array.Sort(animalArray, animalComparer);
            Console.WriteLine($"Sorted array: {animalArray[0].Name}, {animalArray[1].Name}");
            // Output: Sorted array: A, B

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Func and Action Delegates
            // ═══════════════════════════════════════════════════════════

            // Func<T, TResult> - covariant in TResult
            Func<Cat> catFactory = () => new Cat();
            Func<Animal> animalFactory = catFactory; // Return type covariance
            Animal createdAnimal = animalFactory();
            Console.WriteLine($"Func covariance result: {createdAnimal.GetType().Name}");
            // Output: Func covariance result: Cat

            // Func<T, TResult> - contravariant in T input
            Func<Animal, string> animalToString = a => $"Animal: {a.Name}";
            Func<Dog, string> dogToString = animalToString; // Parameter contravariance
            string dogString = dogToString(new Dog { Name = "Max" });
            Console.WriteLine($"Func contravariance result: {dogString}");
            // Output: Func contravariance result: Animal: Max

            // Action<T> - contravariant in T
            Action<Animal> printAnimal = a => Console.WriteLine($"Print: {a.Name}");
            Action<Cat> printCat = printAnimal; // Parameter contravariance
            printCat(new Cat { Name = "Whiskers" });
            // Output: Print: Whiskers

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Real-World - Event Handlers
            // ═══════════════════════════════════════════════════════════

            // EventHandler<T> uses contravariance
            EventHandler<AnimalEventArgs> animalHandler = (s, e) => 
                Console.WriteLine($"Animal event: {e.Animal?.GetType().Name}");

            EventHandler<DogEventArgs> dogHandler = animalHandler; // Contravariance
            dogHandler(null, new DogEventArgs { Dog = new Dog { Name = "Rex" } });
            // Output: Animal event: Dog

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Real-World - Repository Pattern
            // ═══════════════════════════════════════════════════════════

            // Read-only repository interface (covariant)
            IReadOnlyRepository<Animal> animalRepo = new AnimalRepository();
            IReadOnlyRepository<Dog> dogRepo = animalRepo; // Covariance

            Animal animal = dogRepo.GetById(1);
            Console.WriteLine($"Got from dog repo: {animal?.GetType().Name}");
            // Output: Got from dog repo: Animal

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World - Pipeline Pattern
            // ═══════════════════════════════════════════════════════════

            // Converter<TInput, TOutput> - covariant in output, contravariant in input
            Converter<Dog, Animal> dogToAnimal = d => new Animal { Name = d.Name };
            Converter<Cat, Animal> catToAnimal = dogToAnimal; // Works!

            Cat cat = new Cat { Name = "Whiskers" };
            Animal converted = catToAnimal(cat);
            Console.WriteLine($"Converted: {converted.Name}");
            // Output: Converted: Whiskers

            // Multiple transformations
            Converter<string, int> stringToLength = s => s.Length;
            Converter<int, string> intToString = stringToLength; // Contravariance
            
            string input = "Hello";
            string result2 = intToString(input);
            Console.WriteLine($"Pipeline result: {result2}");
            // Output: Pipeline result: 5

            Console.WriteLine("\n=== Covariance and Contravariance Part 2 Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Base and Derived Classes
    // ═══════════════════════════════════════════════════════════

    class Animal2
    {
        public string Name { get; set; }
    }

    class Dog2 : Animal2
    {
        public string Bark() => "Woof!";
    }

    class Cat2 : Animal2
    {
        public string Meow() => "Meow!";
    }

    // ═══════════════════════════════════════════════════════════
    // IComparer Implementation for Contravariance
    // ═══════════════════════════════════════════════════════════

    class SizeComparer : IComparer<Animal2>
    {
        public int Compare(Animal2 x, Animal2 y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            
            return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // EventArgs for Contravariance
    // ═══════════════════════════════════════════════════════════

    class AnimalEventArgs : EventArgs
    {
        public Animal2 Animal { get; set; }
    }

    class DogEventArgs : AnimalEventArgs
    {
        public Dog2 Dog { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: Read-Only Repository (Covariant)
    // ═══════════════════════════════════════════════════════════

    interface IReadOnlyRepository<out T>
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
    }

    class AnimalRepository : IReadOnlyRepository<Animal2>
    {
        private List<Animal2> _animals = new List<Animal2>
        {
            new Animal2 { Name = "Generic Animal" }
        };

        public Animal2 GetById(int id)
        {
            return id >= 0 && id < _animals.Count ? _animals[id] : null;
        }

        public IEnumerable<Animal2> GetAll()
        {
            return _animals;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Converter Delegate (Combined Variance)
    // ═══════════════════════════════════════════════════════════

    // Converter<TInput, TOutput> can convert TInput to TOutput
    // TOutput is covariant, TInput is contravariant
    delegate TOutput Converter<in TInput, out TOutput>(TInput input);
}