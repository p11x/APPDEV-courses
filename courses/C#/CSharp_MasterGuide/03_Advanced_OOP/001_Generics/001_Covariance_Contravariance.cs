/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Generics - Covariance and Contravariance
 * FILE      : Covariance_Contravariance.cs
 * PURPOSE   : Teaches out (covariance) and in (contravariance)
 *            generic type parameters in C#
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._01_Generics
{
    class Covariance_Contravariance
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Covariance and Contravariance in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Understanding Variance
            // ═══════════════════════════════════════════════════════════

            // Covariance: Can convert IEnumerable<Derived> to IEnumerable<Base>
            // Contravariance: Can convert IAction<Base> to IAction<Derived>

            // Without variance, this wouldn't work:
            // List<Dog> dogs = new List<Animal>(); // Error!

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Covariance (out keyword)
            // ═══════════════════════════════════════════════════════════

            // IAnimal is covariant - returns T
            IAnimal<Animal> animal = new AnimalHolder<Animal>();
            IAnimal<Animal> animal2 = new AnimalHolder<Dog>(); // Covariance in action

            // Using covariant interface
            List<Dog> dogs = new List<Dog> { new Dog(), new Dog() };
            IEnumerable<Animal> animals = dogs; // Implicit conversion
            Console.WriteLine($"Converted {dogs.Count} dogs to IEnumerable<Animal>");
            // Output: Converted 2 dogs to IEnumerable<Animal>

            // IEnumerable<T> is covariant in T
            IEnumerable<Animal> animalList = GetDogs();
            Console.WriteLine($"Got animals from dogs list");
            // Output: Got animals from dogs list

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Contravariance (in keyword)
            // ═══════════════════════════════════════════════════════════

            // IAction<T> is contravariant - accepts T
            IAction<Animal> animalAction = (a) => Console.WriteLine($"Animal: {a.GetType().Name}");
            IAction<Dog> dogAction = animalAction; // Contravariance in action

            dogAction(new Dog()); // Works - passes Dog where Animal expected
            // Output: Animal: Dog

            // More contravariance example
            IAction<Dog> dogSpecific = (d) => Console.WriteLine($"Dog: {d.Bark()}");
            IAction<Animal> animalGeneral = dogSpecific; // Reverse conversion
            animalGeneral(new Dog()); // Works - accepts Animal, but Dog is passed
            // Output: Dog: Woof!

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Real-World Example - Factory Pattern
            // ═══════════════════════════════════════════════════════════

            // Covariant factory - creates derived types
            IFactory<Animal> animalFactory = new AnimalFactory();
            Animal created = animalFactory.Create();
            Console.WriteLine($"Created: {created.GetType().Name}");
            // Output: Created: Animal

            IFactory<Dog> dogFactory = new AnimalFactory(); // Covariance
            Dog dogCreated = dogFactory.Create();
            Console.WriteLine($"Created dog: {dogCreated.GetType().Name}");
            // Output: Created dog: Animal

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Real-World Example - Comparer Pattern
            // ═══════════════════════════════════════════════════════════

            // Contravariant comparer - compares base types
            IComparer<Animal> animalComparer = new AnimalSizeComparer();
            IComparer<Dog> dogComparer = animalComparer; // Contravariance

            Dog smallDog = new Dog();
            Dog bigDog = new Dog();
            // Using dog comparer where animal comparer is expected
            // This demonstrates the flexibility contravariance provides

            Console.WriteLine($"Comparer works for dogs");
            // Output: Comparer works for dogs

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Func and Action Delegates
            // ═══════════════════════════════════════════════════════════

            // Func is covariant in return type
            Func<Dog> dogFunc = () => new Dog();
            Func<Animal> animalFunc = dogFunc; // Covariance in Func
            Animal result = animalFunc();
            Console.WriteLine($"Func result: {result.GetType().Name}");
            // Output: Func result: Dog

            // Action is contravariant in parameters
            Action<Animal> animalAct = (a) => Console.WriteLine($"Animal: {a.GetType().Name}");
            Action<Dog> dogAct = animalAct; // Contravariance in Action
            dogAct(new Dog());
            // Output: Animal: Dog

            Console.WriteLine("\n=== Covariance and Contravariance Complete ===");
        }

        // Helper method returning IEnumerable<Dog>
        static IEnumerable<Dog> GetDogs()
        {
            return new List<Dog> { new Dog() };
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Covariant Interface - out T
    // ═══════════════════════════════════════════════════════════

    // IAnimal<T> uses 'out' for covariance - T can only be in output positions
    interface IAnimal<out T>
    {
        T GetAnimal(); // Output position - returns T
    }

    // Implementation
    class AnimalHolder<T> : IAnimal<T>
    {
        private T _animal;

        public T GetAnimal() => _animal;
    }

    // Base and derived classes for demonstration
    class Animal
    {
        public string Name { get; set; }
    }

    class Dog : Animal
    {
        public string Bark() => "Woof!";
    }

    class Cat : Animal
    {
        public string Meow() => "Meow!";
    }

    // ═══════════════════════════════════════════════════════════
    // Contravariant Interface - in T
    // ═══════════════════════════════════════════════════════════

    // IAction<T> uses 'in' for contravariance - T can only be in input positions
    interface IAction<in T>
    {
        void Perform(T action);
    }

    // Implementation
    class ActionHandler<T> : IAction<T>
    {
        public void Perform(T action)
        {
            Console.WriteLine($"Performing action on: {action?.GetType().Name}");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: Covariant Factory Interface
    // ═══════════════════════════════════════════════════════════

    // IFactory<out T> - covariant, returns T
    interface IFactory<out T>
    {
        T Create();
    }

    // Factory implementation
    class AnimalFactory : IFactory<Animal>
    {
        public Animal Create()
        {
            return new Animal();
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: Contravariant Comparer Interface
    // ═══════════════════════════════════════════════════════════

    // IComparer<in T> - contravariant, accepts T
    interface IComparer<in T>
    {
        int Compare(T x, T y);
    }

    // Comparer implementation
    class AnimalSizeComparer : IComparer<Animal>
    {
        public int Compare(Animal x, Animal y)
        {
            // Simple comparison based on name length
            return x.Name?.Length ?? 0.CompareTo(y.Name?.Length ?? 0);
        }
    }
}